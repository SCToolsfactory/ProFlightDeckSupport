﻿using vJoyInterfaceWrap;
using dxKbdInterfaceWrap;
using static dxKbdInterfaceWrap.SCdxKeyboard;

using vjMapper.VjOutput;

namespace ProFlightPanelSupport.VJoy
{
  /// <summary>
  /// Singleton that interacts with the VJoy DLL
  /// Accepts messages to be processed
  /// </summary>
  sealed class VJoyHandler
  {
    private VJoyHandler() { }

    public static VJoyHandler Instance { get; } = new VJoyHandler( );


    private vJoy m_joystick;  // the vJoy device
    private vJoystick m_vJoystick; // my virtual JS
    private uint m_jsId = 0; // invalid

    /// <summary>
    /// Check if Kbd and vJoy are available
    /// </summary>
    /// <returns>Returns true if Kbd and vJoy are available</returns>
    public bool AreLibrariesLoaded()
    {
      bool ret = true;
      ret &= vJoy.isDllLoaded;
      ret &= SCdxKeyboard.isDllLoaded;
      return ret;
    }

    /// <summary>
    /// Return the connection joystick state
    /// </summary>
    public bool Connected { get; private set; } = false;

    /// <summary>
    /// Connect to a Joystick instance
    /// </summary>
    /// <param name="n">The joystick ID 1..16</param>
    /// <returns>True if successfull</returns>
    public bool Connect( int n )
    {
      if ( Connected ) return true; // already connected
      try {
        if ( !vJoy.isDllLoaded ) return false; // ERROR exit DLL not loaded

        if ( n <= 0 || n > 16 ) return false; // ERROR exit invalid Joystick ID
        m_jsId = (uint)n;
        m_joystick = new vJoy( );
        if ( !m_joystick.vJoyEnabled( ) ) {
          Disconnect( ); // cleanup
          return false; // ERROR exit
        }

        // try to control..
        Connected = m_joystick.isVJDExists( m_jsId ); // exists?
        if ( Connected ) {
          Connected = m_joystick.AcquireVJD( m_jsId ); // to use?
        }
        if ( Connected ) {
          bool r = m_joystick.ResetVJD( m_jsId );
          m_vJoystick = new vJoystick( m_joystick, m_jsId ); // the one to use..
        }
        else {
          m_jsId = 0;
          m_joystick = null;
          return false; // ERROR exit
        }
      }
      catch {
        // wrong ...
        m_jsId = 0;
        m_joystick = null;
      }

      return Connected;
    }

    /// <summary>
    /// Last one close the door...
    /// 
    /// Disconnect the Joystick system
    /// </summary>
    public void Disconnect()
    {
      if ( Connected ) {
        m_joystick.ResetVJD( m_jsId );
        m_joystick.RelinquishVJD( m_jsId );
      }
      Connected = false;
      m_jsId = 0;
      m_joystick = null;
    }

    private void Modifier( VJ_Modifier modifier, bool press )
    {
      if ( modifier == VJ_Modifier.VJ_None ) return;
      int mod = 0;
      if ( modifier == VJ_Modifier.VJ_LCtrl )
        mod = (int)SCdxKeycode.VK_LCONTROL;
      else if ( modifier == VJ_Modifier.VJ_RCtrl )
        mod = (int)SCdxKeycode.VK_RCONTROL;
      else if ( modifier == VJ_Modifier.VJ_LAlt )
        mod = (int)SCdxKeycode.VK_LALT;
      else if ( modifier == VJ_Modifier.VJ_RAlt )
        mod = (int)SCdxKeycode.VK_RALT;
      else if ( modifier == VJ_Modifier.VJ_LShift )
        mod = (int)SCdxKeycode.VK_LSHIFT;
      else if ( modifier == VJ_Modifier.VJ_RShift )
        mod = (int)SCdxKeycode.VK_RSHIFT;

      if ( press ) {
        KeyDown( mod );
      }
      else {
        // release
        KeyUp( mod );
      }
    }

    /// <summary>
    /// Dispatch the command message 
    /// </summary>
    /// <param name="message">A VJoy Message</param>
    public bool HandleMessage( VJCommand message )
    {
      if ( !AreLibrariesLoaded() ) return false; // ERROR - bail out for missing libraries
      if ( !message.IsValid ) return false; // ERROR - bail out for undef messages

      bool retVal = false;

      if ( message.IsVJoyCommand ) {
        // mutual exclusive access to the device
        if ( !Connected ) return false; // bail out if vJoy is not available

        lock ( m_joystick ) {
          try {
            switch ( message.CtrlType ) {
              case VJ_ControllerType.VJ_Axis:
                if ( !Connected ) return false; // ERROR - bail out if vJoy is not available
                switch ( message.CtrlDirection ) {
                  case VJ_ControllerDirection.VJ_X:
                    m_vJoystick.XAxis = message.CtrlValue;
                    break;
                  case VJ_ControllerDirection.VJ_Y:
                    m_vJoystick.YAxis = message.CtrlValue;
                    break;
                  case VJ_ControllerDirection.VJ_Z:
                    m_vJoystick.ZAxis = message.CtrlValue;
                    break;
                  default:
                    break;
                }
                break;

              case VJ_ControllerType.VJ_RotAxis:
                switch ( message.CtrlDirection ) {
                  case VJ_ControllerDirection.VJ_X:
                    m_vJoystick.XRotAxis = message.CtrlValue;
                    break;
                  case VJ_ControllerDirection.VJ_Y:
                    m_vJoystick.YRotAxis = message.CtrlValue;
                    break;
                  case VJ_ControllerDirection.VJ_Z:
                    m_vJoystick.ZRotAxis = message.CtrlValue;
                    break;
                  default:
                    break;
                }
                break;

              case VJ_ControllerType.VJ_Slider:
                switch ( message.CtrlIndex ) {
                  case 1:
                    m_vJoystick.Slider1 = message.CtrlValue;
                    break;
                  case 2:
                    m_vJoystick.Slider2 = message.CtrlValue;
                    break;
                  default:
                    break;
                }
                break;

              case VJ_ControllerType.VJ_Hat:
                switch ( message.CtrlDirection ) {
                  case VJ_ControllerDirection.VJ_Center:
                    m_vJoystick.SetPOV( message.CtrlIndex, vJoystick.POVType.Nil );
                    break;
                  case VJ_ControllerDirection.VJ_Left:
                    m_vJoystick.SetPOV( message.CtrlIndex, vJoystick.POVType.Left );
                    break;
                  case VJ_ControllerDirection.VJ_Right:
                    m_vJoystick.SetPOV( message.CtrlIndex, vJoystick.POVType.Right );
                    break;
                  case VJ_ControllerDirection.VJ_Up:
                    m_vJoystick.SetPOV( message.CtrlIndex, vJoystick.POVType.Up );
                    break;
                  case VJ_ControllerDirection.VJ_Down:
                    m_vJoystick.SetPOV( message.CtrlIndex, vJoystick.POVType.Down );
                    break;
                  default:
                    break;
                }
                break;

              case VJ_ControllerType.VJ_Button:
                switch ( message.CtrlDirection ) {
                  case VJ_ControllerDirection.VJ_Down:
                    m_vJoystick.SetButton( message.CtrlIndex, true );
                    break;
                  case VJ_ControllerDirection.VJ_Up:
                    m_vJoystick.SetButton( message.CtrlIndex, false );
                    break;
                  case VJ_ControllerDirection.VJ_Tap:
                    m_vJoystick.SetButton( message.CtrlIndex, true );
                    Sleep_ms( (uint)message.CtrlDelay );
                    m_vJoystick.SetButton( message.CtrlIndex, false );
                    break;
                  case VJ_ControllerDirection.VJ_DoubleTap:
                    m_vJoystick.SetButton( message.CtrlIndex, true );
                    Sleep_ms( (uint)message.CtrlDelay ); // tap delay
                    m_vJoystick.SetButton( message.CtrlIndex, false );
                    Sleep_ms( 25 ); // double tap delay is fixed
                    m_vJoystick.SetButton( message.CtrlIndex, true );
                    Sleep_ms( (uint)message.CtrlDelay ); // tap delay
                    m_vJoystick.SetButton( message.CtrlIndex, false );
                    break;
                  default:
                    break;
                }
                break;


              default:
                break;
            }//switch message type

            retVal = true; // finally we made it
          }
          catch { // anything
            Connected = false; // probably something went wrong...
          }

        }//endlock
      }
      else {
        // dxKey Message
        if ( message.CtrlType == VJ_ControllerType.DX_Key ) {
          switch ( message.CtrlDirection ) {
            case VJ_ControllerDirection.VJ_Down:
              foreach ( var m in message.CtrlModifier ) Modifier( m, true );
              KeyDown( message.CtrlIndex );
              break;
            case VJ_ControllerDirection.VJ_Up:
              KeyUp( message.CtrlIndex );
              foreach ( var m in message.CtrlModifier ) Modifier( m, false );
              break;
            case VJ_ControllerDirection.VJ_Tap:
              foreach ( var m in message.CtrlModifier ) Modifier( m, true );
              KeyStroke( message.CtrlIndex, (uint)message.CtrlDelay );
              foreach ( var m in message.CtrlModifier ) Modifier( m, false );
              break;
            case VJ_ControllerDirection.VJ_DoubleTap:
              foreach ( var m in message.CtrlModifier ) Modifier( m, true );
              KeyStroke( message.CtrlIndex, (uint)message.CtrlDelay );
              foreach ( var m in message.CtrlModifier ) Modifier( m, false );
              Sleep_ms( 25 ); // double tap delay is fixed
              foreach ( var m in message.CtrlModifier ) Modifier( m, true );
              KeyStroke( message.CtrlIndex, (uint)message.CtrlDelay );
              foreach ( var m in message.CtrlModifier ) Modifier( m, false );
              break;
            default:
              break;
          }
          retVal = true; // finally we made it
        }
      }

      return retVal;
    }

  }
}
