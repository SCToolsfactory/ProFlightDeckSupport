using PFSP_HID;

namespace ProFlightPanelSupport.SwitchPanel
{
  /// <summary>
  /// Wraps the Panel LED handling
  /// </summary>
  class SwitchPanelLed
  {
    public static void HandleLed( string ledString, PFSwPanelManager manager )
    {
      PFSwPanelLedState ledColor;
      PFSwPanelLeds led;

      if ( string.IsNullOrEmpty( ledString ) ) ledString = "n"; // none is default if nothing is given
      switch ( ledString.ToUpperInvariant( ) ) {
        case "NO": // Nose Off
          led = PFSwPanelLeds.GEAR_N;
          ledColor = PFSwPanelLedState.Led_Off;
          break;
        case "NG": // Nose Green
          led = PFSwPanelLeds.GEAR_N;
          ledColor = PFSwPanelLedState.Led_Green;
          break;
        case "NR": // Nose Red
          led = PFSwPanelLeds.GEAR_N;
          ledColor = PFSwPanelLedState.Led_Red;
          break;
        case "NA": // Nose Amber
          led = PFSwPanelLeds.GEAR_N;
          ledColor = PFSwPanelLedState.Led_Amber;
          break;
        case "LO": // Left Off
          led = PFSwPanelLeds.GEAR_L;
          ledColor = PFSwPanelLedState.Led_Off;
          break;
        case "LG": // Left Green
          led = PFSwPanelLeds.GEAR_L;
          ledColor = PFSwPanelLedState.Led_Green;
          break;
        case "LR": // Left Red
          led = PFSwPanelLeds.GEAR_L;
          ledColor = PFSwPanelLedState.Led_Red;
          break;
        case "LA": // Left Amber
          led = PFSwPanelLeds.GEAR_L;
          ledColor = PFSwPanelLedState.Led_Amber;
          break;
        case "RO": // Right Off
          led = PFSwPanelLeds.GEAR_R;
          ledColor = PFSwPanelLedState.Led_Off;
          break;
        case "RG": // Right Green
          led = PFSwPanelLeds.GEAR_R;
          ledColor = PFSwPanelLedState.Led_Green;
          break;
        case "RR": // Right Red
          led = PFSwPanelLeds.GEAR_R;
          ledColor = PFSwPanelLedState.Led_Red;
          break;
        case "RA": // Right Amber
          led = PFSwPanelLeds.GEAR_R;
          ledColor = PFSwPanelLedState.Led_Amber;
          break;
        default: // none
          led = PFSwPanelLeds.GEAR_NONE;
          ledColor = PFSwPanelLedState.Led_Off;
          break;
      }

      if ( led == PFSwPanelLeds.GEAR_NONE ) return;

      manager.SetLed( led, ledColor );
    }
  }
}
