using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PFSP_HID;
using ProFlightPanelSupport.Commands;
using ProFlightPanelSupport.VJoy;

namespace ProFlightPanelSupport.SwitchPanel
{
  /// <summary>
  /// Command and Control class for the Background worker
  /// </summary>
  public class BgwSwitchPanelContext
  {
    // The configuration to be used here
    public string ConfigFile = "";
    // Joystick #1..
    public int JoystickNo = 0;
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
          abort = true;
        }
      }
      else {
        m_worker.ReportProgress( 0, "SwitchPanelSupport - Device open failed" ); // first message..
        abort = true;
      }
      // Try to connect the Joystick
      // First try to connect the Joystick interface
      if ( ( m_bgwContext.JoystickNo > 0 ) && ( m_bgwContext.JoystickNo <= 16 ) ) {
        if ( !VJoyHandler.Instance.Connect( m_bgwContext.JoystickNo )){
          m_worker.ReportProgress( 0, $"SwitchPanelSupport - Cannot connect joystick # {m_bgwContext.JoystickNo}" ); // first message..
        }
      }
      else {
        m_worker.ReportProgress( 0, $"SwitchPanelSupport - Invalid Joystick # {m_bgwContext.JoystickNo}" ); // first message..
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

      // clean vJoy up
      VJoyHandler.Instance.Disconnect( ); // shut the Joystick handler

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

    private void HandleLed ( PFSwPanelLeds led, PFSwPanelLedState state )
    {
      if ( led == PFSwPanelLeds.GEAR_NONE ) return;

      m_pfspManager.SetLed( led, state );
    }

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
      if ( !VJoyHandler.Instance.Connected ) return;

      string key = e.State.RotaryState( ).ToString( );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          VJoyHandler.Instance.HandleMessage( cmd );
          HandleLed( cmd.CtrlLed, cmd.CtrlLedColor );
        }
      }
    }

    private void PfspManager_ButtonDown( object sender, PFSwPanelSwitchEventArgs e )
    {
      m_worker.ReportProgress( 50, $"SwitchPanelSupport - {e.Switch.ToString( )} Down" ); // kind of debug only
      if ( !VJoyHandler.Instance.Connected ) return;

      string key = PanelCommand.Input_Off( e.Switch.ToString( ) );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          VJoyHandler.Instance.HandleMessage( cmd );
          HandleLed( cmd.CtrlLed, cmd.CtrlLedColor );
        }
      }
    }

    private void PfspManager_ButtonUp( object sender, PFSwPanelSwitchEventArgs e )
    {
      m_worker.ReportProgress( 50, $"SwitchPanelSupport - {e.Switch.ToString( )} Up" );
      if ( !VJoyHandler.Instance.Connected ) return;

      string key = PanelCommand.Input_On( e.Switch.ToString( ) );
      if ( m_panelConfig.VJCommands.ContainsKey( key ) ) {
        var cmd = m_panelConfig.VJCommands[key];
        if ( cmd.IsValid ) {
          VJoyHandler.Instance.HandleMessage( cmd );
          HandleLed( cmd.CtrlLed, cmd.CtrlLedColor );
        }
      }
    }
    #endregion

  }
}
