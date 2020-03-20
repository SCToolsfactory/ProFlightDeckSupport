using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HidLibrary;

/// <summary>
///
/// Shameless plug from:
/// https://github.com/mikeobrien/HidLibrary/tree/master/examples/GriffinPowerMate/PowerMate
/// adopted for the Panel
/// </summary>
namespace PFSP_HID
{
  /// <summary>
  /// Manages one Pro Flight Switch Panel device.
  /// </summary>
  public class PFSwPanelManager : IDisposable
  {
    private const int m_vendorId = 0x06a3;  // Saitek PLC / Logitech SA
    private const int m_productId = 0x0d67; // Saitek Pro Flight Switch Panel
    private HidDevice m_device;
    private bool m_attached = false;
    private bool m_connectedToDriver = false;

    private PFSwPanelState m_prevState;
    private PFSwPanelLedState[] m_ledState;

    private bool m_debugPrintRawMessages = false;
    private bool m_disposed = false;

    /// <summary>
    /// Occurs when a Pro Flight Switch Panel device is attached.
    /// </summary>
    public event EventHandler DeviceAttached;

    /// <summary>
    /// Occurs when a Pro Flight Switch Panel device is removed.
    /// </summary>
    public event EventHandler DeviceRemoved;

    /// <summary>
    /// Occurs when a Pro Flight Switch Panel switch changes state from Up to Down.
    /// </summary>
    public event EventHandler<PFSwPanelSwitchEventArgs> ButtonDown;

    /// <summary>
    /// Occurs when a Pro Flight Switch Panel switch changes state from Down to Up.
    /// </summary>
    public event EventHandler<PFSwPanelSwitchEventArgs> ButtonUp;

    /// <summary>
    /// Occurs when the Pro Flight Switch Panel Rotary is rotated.
    /// </summary>
    public event EventHandler<PFSwPanelRotaryEventArgs> RotaryChanged;

    /// <summary>
    /// Initializes a new instance of the PFSwPanelManager class.
    /// </summary>
    public PFSwPanelManager()
    {
      m_prevState = new PFSwPanelState( Consts.SwitchArray, PFSwPanelRotaryState.Pos_Off ); // init with Off
      m_ledState = Consts.LedArray;
    }

    /// <summary>
    /// Attempts to connect to a Pro Flight Switch Panel device.
    /// 
    /// After a successful connection, a DeviceAttached event will normally be sent.
    /// </summary>
    /// <returns>True if a Pro Flight Switch Panel device is connected, False otherwise.</returns>
    public bool OpenDevice()
    {
      m_device = HidDevices.Enumerate( m_vendorId, m_productId ).FirstOrDefault( );

      if ( m_device != null ) {
        m_connectedToDriver = true;
        m_device.OpenDevice( );

        m_device.Inserted += DeviceAttachedHandler;
        m_device.Removed += DeviceRemovedHandler;

        m_device.MonitorDeviceEvents = true;

        m_device.ReadReport( OnReport );

        return true;
      }

      return false;
    }

    /// <summary>
    /// Closes the connection to the device.
    /// 
    /// FIXME: Verify that this also shuts down any thread waiting for device data.
    /// </summary>
    public void CloseDevice()
    {
      m_device.CloseDevice( );
      m_connectedToDriver = false;
    }

    /// <summary>
    /// Closes the connection to the device.
    /// </summary>
    public void Dispose()
    {
      Dispose( true );
      GC.SuppressFinalize( this );
    }


    /// <summary>
    /// Sends a message to the Pro Flight Switch Panel device to enable  LEDs with state
    /// </summary>
    public void SetLed( PFSwPanelLeds led, PFSwPanelLedState state )
    {
      /*
        -----------
        Set Feature
        -----------
                  LANDING GEAR LEDs  |
                  -------------------|
        Byte position:  0            |
        -----------------------------|
                  0x01 Nose green    |
                  0x02 Left green    |
                  0x04 Right green   |
                  0x09 Nose orange   |
                  0x12 Left orange   |
                  0x24 Right orange  |
                  0x08 Nose red      |
                  0x10 Left red      |
                  0x20 Right red     |

       */
      if ( m_connectedToDriver ) {
        m_ledState[(int)led] = state; // new state for the led

        byte[] data = new byte[2];
        data[0] = 0x00;
        data[1] = 0x00;
        // eval the complete set of LEDs
        if ( m_ledState[(int)PFSwPanelLeds.GEAR_N] == PFSwPanelLedState.Led_Amber ) data[1] = (byte)( data[1] | 0x09 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_N] == PFSwPanelLedState.Led_Red ) data[1] = (byte)( data[1] | 0x08 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_N] == PFSwPanelLedState.Led_Green ) data[1] = (byte)( data[1] | 0x01 );

        if ( m_ledState[(int)PFSwPanelLeds.GEAR_L] == PFSwPanelLedState.Led_Amber ) data[1] = (byte)( data[1] | 0x12 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_L] == PFSwPanelLedState.Led_Red ) data[1] = (byte)( data[1] | 0x10 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_L] == PFSwPanelLedState.Led_Green ) data[1] = (byte)( data[1] | 0x02 );

        if ( m_ledState[(int)PFSwPanelLeds.GEAR_R] == PFSwPanelLedState.Led_Amber ) data[1] = (byte)( data[1] | 0x24 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_R] == PFSwPanelLedState.Led_Red ) data[1] = (byte)( data[1] | 0x20 );
        else if ( m_ledState[(int)PFSwPanelLeds.GEAR_R] == PFSwPanelLedState.Led_Green ) data[1] = (byte)( data[1] | 0x04 );

        var report = new HidReport( 2, new HidDeviceData( data, HidDeviceData.ReadStatus.Success ) );
        bool retVal = m_device.WriteFeatureData( data );

        if ( !retVal ) {
          System.Diagnostics.Debug.WriteLine( "Pro Flight Switch Panel SetLed FAILED" );
        }
      }
    }


    // trigger callbacks
    private void OnButtonDown( PFSwPanelState state, PFSwPanelSwitches switch_ )
    {
      ButtonDown?.Invoke( this, new PFSwPanelSwitchEventArgs( state, switch_ ) );
    }

    private void OnButtonUp( PFSwPanelState state, PFSwPanelSwitches switch_ )
    {
      ButtonUp?.Invoke( this, new PFSwPanelSwitchEventArgs( state, switch_ ) );
    }

    private void OnRotaryChanged( PFSwPanelState state )
    {
      RotaryChanged?.Invoke( this, new PFSwPanelRotaryEventArgs( state ) );
    }

    private void GenerateEvents( PFSwPanelState state )
    {
      foreach ( PFSwPanelSwitches s in Consts.EnumSwitches ) {
        if ( state.SwitchState( s ) == PFSwPanelSwitchState.Off_Down && m_prevState.SwitchState( s ) == PFSwPanelSwitchState.On_Up ) {
          OnButtonDown( state, s );
        }
        else if ( state.SwitchState( s ) == PFSwPanelSwitchState.On_Up && m_prevState.SwitchState( s ) == PFSwPanelSwitchState.Off_Down ) {
          OnButtonUp( state, s );
        }
      }
      if ( state.RotaryState( ) != m_prevState.RotaryState( ) ) {
        OnRotaryChanged( state );
      }
      m_prevState = state; // does this a deep copy of the arrays ?? - TODO to check
    }

    private void DeviceAttachedHandler()
    {
      m_attached = true;

      DeviceAttached?.Invoke( this, EventArgs.Empty );

      m_device.ReadReport( OnReport );
    }

    private void DeviceRemovedHandler()
    {
      m_attached = false;

      DeviceRemoved?.Invoke( this, EventArgs.Empty );
    }

    private void OnReport( HidReport report )
    {
      if ( m_attached == false ) { return; }

      if ( report.Data.Length >= 3 ) {
        PFSwPanelState state = ParseState( report.Data );
        if ( !state.IsValid ) {
          System.Diagnostics.Debug.WriteLine( "Invalid Pro Flight Switch Panel state" );
        }
        else {
          GenerateEvents( state );

          if ( m_debugPrintRawMessages ) {
            System.Diagnostics.Debug.Write( "Pro Flight Switch Panel raw data: " );
            for ( int i = 0; i < report.Data.Length; i++ ) {
              System.Diagnostics.Debug.Write( String.Format( "{0:000} ", report.Data[i] ) );
            }
            System.Diagnostics.Debug.WriteLine( "" );
          }
        }
      }

      m_device.ReadReport( OnReport );
    }

    /*
      Switch Panel messages:
       - 1 message when rotary switch is turned
       - 1 message for any switch
      ----
      Read
          - 3 bytes
          - knob position and switch positions are always returned in the read data
          - Engine Knob being turned generates 1 message
          - Each switch toggle generates 1 message
      ----
     
      -----------
      Get Feature
      -----------
              |--------|-------|-----|
      Byte    |    2   |   1   |  0  |
      --------|--------|-------|-----|--------------------------
              |        |       | 01  | MASTER BAT. ON
              |        |       | 02  | MASTER ALT. ON
              |        |       | 04  | AVIONICS MASTER ON
              |        |       | 08  | FUEL PUMP ON
              |        |       | 10  | DE-ICE ON
              |        |       | 20  | PITOT HEAT ON
              |        |       | 40  | COWL CLOSE
              |        |       | 80  | PANEL ON
              |        |   01  |     | BEACON ON
              |        |   02  |     | NAV. ON
              |        |   04  |     | STROBE ON
              |        |   08  |     | TAXI ON
              |        |   10  |     | LANDING ON
              |        |   20  |     | ROTARY SWITCH OFF
              |        |   40  |     | ROTARY SWITCH R
              |        |   80  |     | ROTARY SWITCH L
              |   01   |       |     | ROTARY SWITCH BOTH/ALL
              |   02   |       |     | ROTARY SWITCH START
              |   04   |       |     | GEAR UP
              |   08   |       |     | GEAR DOWN     
     */
    private PFSwPanelState ParseState( byte[] data )
    {
      if ( data.Length >= 3 ) {
        var switches = Consts.SwitchArray;
        PFSwPanelRotaryState rotary;

        switches[(int)PFSwPanelSwitches.MASTER_BATT] = ( ( data[0] & 0x01 ) == 0x01 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.MASTER_ALT] = ( ( data[0] & 0x02 ) == 0x02 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.AVIONICS_MASTER] = ( ( data[0] & 0x04 ) == 0x04 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.FUEL_PUMP] = ( ( data[0] & 0x08 ) == 0x08 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.DE_ICE] = ( ( data[0] & 0x10 ) == 0x10 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.PITOT_HEAT] = ( ( data[0] & 0x20 ) == 0x20 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.COWL_CLOSE] = ( ( data[0] & 0x40 ) == 0x40 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.PANEL] = ( ( data[0] & 0x80 ) == 0x80 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;

        switches[(int)PFSwPanelSwitches.BEACON] = ( ( data[1] & 0x01 ) == 0x01 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.NAV] = ( ( data[1] & 0x02 ) == 0x02 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.STROBE] = ( ( data[1] & 0x04 ) == 0x04 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.TAXI] = ( ( data[1] & 0x08 ) == 0x08 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;
        switches[(int)PFSwPanelSwitches.LANDING] = ( ( data[1] & 0x10 ) == 0x10 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;

        switches[(int)PFSwPanelSwitches.GEAR] = ( ( data[2] & 0x04 ) == 0x04 ) ? PFSwPanelSwitchState.On_Up : PFSwPanelSwitchState.Off_Down;

        // check backwards to get the 'most important' first..
        if ( ( data[2] & 0x02 ) == 0x02 ) rotary = PFSwPanelRotaryState.Pos_Start;
        else if ( ( data[2] & 0x01 ) == 0x01 ) rotary = PFSwPanelRotaryState.Pos_All;
        else if ( ( data[1] & 0x80 ) == 0x80 ) rotary = PFSwPanelRotaryState.Pos_L;
        else if ( ( data[1] & 0x40 ) == 0x40 ) rotary = PFSwPanelRotaryState.Pos_R;
        else rotary = PFSwPanelRotaryState.Pos_Off;

        return new PFSwPanelState( switches, rotary );
      }
      else {
        return new PFSwPanelState( ); // PFSwPanelState.Invalid() will return false
      }
    }

    /// <summary>
    /// Closes any connected devices.
    /// </summary>
    /// <param name="disposing"></param>
    private void Dispose( bool disposing )
    {
      if ( !this.m_disposed ) {
        if ( disposing ) {
          CloseDevice( );
        }

        m_disposed = true;
      }
    }

    /// <summary>
    /// Destroys instance and frees device resources (if not freed already)
    /// </summary>
    ~PFSwPanelManager()
    {
      Dispose( false );
    }

  }
}