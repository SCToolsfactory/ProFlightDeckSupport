using System;
using System.ComponentModel;
using System.Threading;

using PFPanelClient.Protocol;
using PFSP_HID;
using vjMapper.JInput;

namespace PFPanelClient.SwitchPanel
{
  /// <summary>
  /// Command and Control class for the Background worker
  /// </summary>
  public class BgwSwitchPanelContext
  {
    // The configuration to be used here
    public string ConfigFile = "";
    // Server
    public string ServerIP = "";
    public string ServerPort = "";

    //
    // LED signaling
    public bool LedChanged = false;
    public PFSwPanelLedState L_Led = PFSwPanelLedState.Led_Off;
    public PFSwPanelLedState N_Led = PFSwPanelLedState.Led_Off;
    public PFSwPanelLedState R_Led = PFSwPanelLedState.Led_Off;
  }

  /// <summary>
  /// Main Switch Panel Support 
  ///   Attached to the device and processes it's events
  /// </summary>
  class SwitchPanelSupport
  {
    private BackgroundWorker m_worker = null;
    private PFSwPanelManager m_pfspManager = null;
    private bool m_devAttached = false;
    private BgwSwitchPanelContext m_bgwContext;
    private SwitchPanelConfig m_panelConfig = null;
    private UdpMessenger m_udp = null;

    /// <summary>
    /// Main Background worker thread entry
    /// </summary>
    /// <param name="bgwContext">The command and control struct</param>
    public void DoWork( object sender, BgwSwitchPanelContext bgwContext )
    {
      m_worker = sender as BackgroundWorker;

      m_worker.ReportProgress( 0, "SwitchPanelSupport - About to start" ); // first message..

      m_bgwContext = bgwContext;
      m_pfspManager = new PFSwPanelManager( );
      bool abort = false;
      bool eventsHooked = false;

      // Try to connect to the HID driver
      if ( m_pfspManager.OpenDevice( ) ) {
        // attach to the events from the HID device
        m_pfspManager.DeviceAttached += PfspManager_DeviceAttached;
        m_pfspManager.DeviceRemoved += PfspManager_DeviceRemoved;
        m_pfspManager.ButtonUp += PfspManager_ButtonUp;
        m_pfspManager.ButtonDown += PfspManager_ButtonDown;
        m_pfspManager.RotaryChanged += PfspManager_RotaryChanged;
        eventsHooked = true;

        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_L, PFSwPanelLedState.Led_Off );
        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_R, PFSwPanelLedState.Led_Off );
        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_N, PFSwPanelLedState.Led_Off );

        // Try to load the config file
        m_panelConfig = SwitchPanelConfig.FromJson( m_bgwContext.ConfigFile );
        if ( m_panelConfig.Valid ) {
          m_worker.ReportProgress( 0, $" Config: {m_panelConfig.ConfigName}" ); // first message..
        }
        else {
          m_worker.ReportProgress( 0, $" Invalid Config File - aborting" ); // first message..
          m_worker.ReportProgress( 0, $"   {SwitchPanelConfig.ErrorMsg}" ); // first message..
          abort = true;
        }
      }
      else {
        m_worker.ReportProgress( 0, "SwitchPanelSupport - Device open failed" ); // first message..
        abort = true;
      }
      // Try to connect the JoystickServer
      if ( int.TryParse( m_bgwContext.ServerPort, out int portNo ) ) {
        if ( UdpMessenger.CheckIP( m_bgwContext.ServerIP ) ) {
          m_udp = new UdpMessenger( m_bgwContext.ServerIP, portNo );
        }
        else {
          m_worker.ReportProgress( 0, $"SwitchPanelSupport - Invalid IP # {m_bgwContext.ServerIP}" );
        }
      }
      else {
        m_worker.ReportProgress( 0, $"SwitchPanelSupport - Invalid Port number # {m_bgwContext.ServerPort}" );
      }

      // Report about incidents..

      // Task loop - wait until killed - handling happens in the events
      while ( !abort ) {
        if ( m_bgwContext.LedChanged ) {
          m_pfspManager.SetLed( PFSwPanelLeds.GEAR_L, m_bgwContext.L_Led );
          m_pfspManager.SetLed( PFSwPanelLeds.GEAR_N, m_bgwContext.N_Led );
          m_pfspManager.SetLed( PFSwPanelLeds.GEAR_R, m_bgwContext.R_Led );
          m_bgwContext.LedChanged = false; // commit
        }

        Thread.Sleep( 200 ); // check every 200 ms for abort or LED changes
        abort = m_worker.CancellationPending; // check once in a while
      }

      // clean UDP
      m_udp = null;

      // clean HID up
      // if the device is still attached
      if ( m_devAttached ) {
        // final message...
        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_L, PFSwPanelLedState.Led_Red );
        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_R, PFSwPanelLedState.Led_Green );
        m_pfspManager.SetLed( PFSwPanelLeds.GEAR_N, PFSwPanelLedState.Led_Amber );

        m_pfspManager.CloseDevice( );
      }
      // events
      if ( eventsHooked ) {
        m_pfspManager.DeviceAttached -= PfspManager_DeviceAttached;
        m_pfspManager.DeviceRemoved -= PfspManager_DeviceRemoved;
        m_pfspManager.ButtonUp -= PfspManager_ButtonUp;
        m_pfspManager.ButtonDown -= PfspManager_ButtonDown;
        m_pfspManager.RotaryChanged -= PfspManager_RotaryChanged;
      }
      // the HID object
      m_pfspManager.Dispose( );
      m_pfspManager = null;

      m_worker.ReportProgress( 100, "SwitchPanelSupport - Ending now" ); // final message..
    }

    #region HID device events (note this runs in the BGWorker thread !!!


    private void PfspManager_DeviceAttached( object sender, EventArgs e )
    {
      m_worker.ReportProgress( 10, "SwitchPanelSupport - Device attached" );
      m_devAttached = true;
    }

    private void PfspManager_DeviceRemoved( object sender, EventArgs e )
    {
      m_worker.ReportProgress( 90, "SwitchPanelSupport - Device removed" );
      m_devAttached = false;
    }

    private void PfspManager_RotaryChanged( object sender, PFSwPanelRotaryEventArgs e )
    {
      m_worker.ReportProgress( 50, $"SwitchPanelSupport - Rotary changed to {e.State.RotaryState( ).ToString( )}" );
      if ( m_udp == null ) return;

      string key = InputRotary.Rotary_Pos( "ROTARY", (int)e.State.RotaryState( ) );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          m_udp.SendMsg( cmd.JString );
          SwitchPanelLed.HandleLed( cmd.CtrlExt1, m_pfspManager );
        }
      }
    }

    private void PfspManager_ButtonDown( object sender, PFSwPanelSwitchEventArgs e )
    {
      m_worker.ReportProgress( 50, $"SwitchPanelSupport - {e.Switch.ToString( )} Down" ); // kind of debug only
      if ( m_udp == null ) return;

      string key =  InputSwitch.Input_Off( e.Switch.ToString() );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          m_udp.SendMsg( cmd.JString );
          SwitchPanelLed.HandleLed( cmd.CtrlExt1, m_pfspManager );
        }
      }
    }

    private void PfspManager_ButtonUp( object sender, PFSwPanelSwitchEventArgs e )
    {
      m_worker.ReportProgress( 50, $"SwitchPanelSupport - {e.Switch.ToString( )} Up" );
      if ( m_udp == null ) return;

      string key = InputSwitch.Input_On( e.Switch.ToString() );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          m_udp.SendMsg( cmd.JString );
          SwitchPanelLed.HandleLed( cmd.CtrlExt1, m_pfspManager );
        }
      }
    }
    #endregion

  }
}
