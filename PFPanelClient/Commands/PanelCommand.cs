using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFPanelClient.Commands
{
  // Message Handling 
  public enum VJ_ControllerType
  {
    VJ_Unknown = -1,
    VJ_Button = 0,
    VJ_Axis,
    VJ_RotAxis,
    VJ_Slider,
    VJ_Hat,

    DX_Key, // key input 

    VX_Macro, // macro as input
  }

  // Message Handling 
  public enum VJ_Modifier
  {
    VJ_None = 0,
    VJ_LCtrl,
    VJ_RCtrl,
    VJ_LAlt,
    VJ_RAlt,
    VJ_LShift,
    VJ_RShift,
  }

  public enum VJ_ControllerDirection
  {
    VJ_NotUsed = 0,

    VJ_X,
    VJ_Y,
    VJ_Z,

    VJ_Center,    // POV
    VJ_Left,      // POV
    VJ_Right,     // POV
    VJ_Up,        // POV or button or key
    VJ_Down,      // POV or button or key
    VJ_Tap,       // button or key
    VJ_DoubleTap, // button or key
  }

  #region VJCommand

  public class VJCommand
  {
    // the defaults
    internal const int VJ_MAXBUTTON = 60;  // the last allowed button number
    internal const int DEFAULT_DELAY = 150; // msec
    internal const int DEFAULT_SHORTDELAY = 5; // msec - short tap const

    internal const string MACRO = "~"; // Trailer for Macro naming..

    /// <summary>
    /// The controller e.g. Button, Axis, etc.
    /// </summary>
    public VJ_ControllerType CtrlType { get; set; } = VJ_ControllerType.VJ_Unknown;

    /// <summary>
    /// The controller direction e.g. Axis X,Y,Z or POV direction or button up or down
    /// </summary>
    public VJ_ControllerDirection CtrlDirection { get; set; } = VJ_ControllerDirection.VJ_NotUsed;

    private List<VJ_Modifier> m_modifiers = new List<VJ_Modifier>( );
    /// <summary>
    /// The controller modifiers 
    /// </summary>
    public IList<VJ_Modifier> CtrlModifier { get => m_modifiers; }

    /// <summary>
    /// The index of the controller 1..n
    /// for K it is the Key Integer VK_Key
    /// </summary>
    public int CtrlIndex { get; set; } = 0;

    /// <summary>
    /// The value of the controller 1..1000
    /// for B and K  Taps it is the delay in mseconds 
    /// </summary>
    public int CtrlValue { get; set; } = 0;

    /// <summary>
    /// The string of a controller
    /// for M it is the MacroName to call
    /// </summary>
    public string CtrlString { get; set; } = "";

    /// <summary>
    /// The LED to illuminate
    /// </summary>
    public PFSP_HID.PFSwPanelLeds CtrlLed { get; set; } = PFSP_HID.PFSwPanelLeds.GEAR_NONE;

    /// <summary>
    /// The LED color
    /// </summary>
    public PFSP_HID.PFSwPanelLedState CtrlLedColor { get; set; } = PFSP_HID.PFSwPanelLedState.Led_Off;

    /// <summary>
    /// Returns true for a valid command
    /// </summary>
    public bool IsValid { get => this.CtrlType != VJ_ControllerType.VJ_Unknown; }

    /// <summary>
    /// true if it is not a Key message
    /// </summary>
    public bool IsVJoyMessage { get => this.CtrlType != VJ_ControllerType.DX_Key; }

    /// <summary>
    /// true if it is a Key message
    /// </summary>
    public bool IsKeyMessage { get => this.CtrlType == VJ_ControllerType.DX_Key; }

    /// <summary>
    /// true if it is a Key message
    /// </summary>
    public bool IsMacroMessage { get => this.CtrlType == VJ_ControllerType.VX_Macro; }

    private string m_jString = "";
    /// <summary>
    /// Set the JString for Macros
    /// Note: should be used by postprocess only...
    /// </summary>
    /// <param name="jString">The Json String of a Macro Command</param>
    internal void SetJString(string jString )
    {
      m_jString = jString;
    }

    /// <summary>
    /// Returns the command string for any command ready to send 
    /// </summary>
    public string JString
    {
      get {
        // create the string if it was empty before (exec JCommand only once)
        // The command is not supposed to change once created, may be make it immutable ??
        if ( string.IsNullOrEmpty( m_jString ) ) {
          m_jString = JCommand;        
        }
        return m_jString;
      }
    }


    #region Create Json Command

    private string JAxisDirection
    {
      get {
        // "Direction": "X|Y|Z"
        switch ( this.CtrlDirection ) {
          case VJ_ControllerDirection.VJ_X: return $"\"Direction\": \"X\"";
          case VJ_ControllerDirection.VJ_Y: return $"\"Direction\": \"Y\"";
          case VJ_ControllerDirection.VJ_Z: return $"\"Direction\": \"Z\"";
          default: return "";
        }
      }
    }

    private string JPOVDirection
    {
      get {
        // "Direction": "c | u | r | d | l"
        switch ( this.CtrlDirection ) {
          case VJ_ControllerDirection.VJ_Center: return $"\"Direction\": \"c\"";
          case VJ_ControllerDirection.VJ_Up: return $"\"Direction\": \"u\"";
          case VJ_ControllerDirection.VJ_Right: return $"\"Direction\": \"u\"";
          case VJ_ControllerDirection.VJ_Down: return $"\"Direction\": \"d\"";
          case VJ_ControllerDirection.VJ_Left: return $"\"Direction\": \"l\"";
          default: return "";
        }
      }
    }

    private string JHitMode
    {
      get {
        // "Mode": "p|r|t|s|d", "Delay":100
        switch ( this.CtrlDirection ) {
          case VJ_ControllerDirection.VJ_Down: return $"\"Mode\": \"p\"";
          case VJ_ControllerDirection.VJ_Up: return $"\"Mode\": \"r\"";
          case VJ_ControllerDirection.VJ_Tap:
            if ( this.CtrlValue == DEFAULT_SHORTDELAY ) {
              return $"\"Mode\": \"s\"";
            }
            else {
              return $"\"Mode\": \"t\", \"Delay\": {this.CtrlValue}";
            }
          case VJ_ControllerDirection.VJ_DoubleTap: return $"\"Mode\": \"d\", \"Delay\": {this.CtrlValue}";
          default: return "";
        }
      }
    }

    private string JKeyModifier
    {
      get {
        // "Modifier": "mod"  - [mod[&mod]] (n)one, (lc)trl, (rc)trl, (la)lt, (ra)lt, (ls)hift, (rs)hift
        string mod = "";
        if ( this.CtrlModifier.Count == 0 ) {
          mod = "n";
        }
        else {
          foreach ( var m in CtrlModifier ) {
            switch ( m ) {
              case VJ_Modifier.VJ_LAlt:
                mod += "la"; break;
              case VJ_Modifier.VJ_RAlt:
                mod += "ra"; break;
              case VJ_Modifier.VJ_LCtrl:
                mod += "lc"; break;
              case VJ_Modifier.VJ_RCtrl:
                mod += "rc"; break;
              case VJ_Modifier.VJ_LShift:
                mod += "ls"; break;
              case VJ_Modifier.VJ_RShift:
                mod += "rs"; break;
              default: break;
            }
            if ( m != CtrlModifier.Last( ) ) {
              mod += "&";
            }
          }

        }
        return $"\"Modifier\": \"{mod}\"";
      }
    }

    private string JSIndexNumber
    {
      get {
        // "Index": N
        return $"\"Index\": {this.CtrlIndex}";
      }
    }

    private string JSKeyCodeNumber
    {
      get {
        // "VKcode": n  (use the int version - less overhead)
        return $"\"VKcode\": {this.CtrlIndex}";
      }
    }

    private string JAnalogValue
    {
      get {
        // "Value": number
        switch ( this.CtrlDirection ) {
          case VJ_ControllerDirection.VJ_X: return $"\"Value\": {this.CtrlValue}";
          case VJ_ControllerDirection.VJ_Y: return $"\"Value\": {this.CtrlValue}";
          case VJ_ControllerDirection.VJ_Z: return $"\"Value\": {this.CtrlValue}";
          default: return "";
        }
      }
    }

    private string JCommand
    {
      get {
        string ret = "";
        switch ( this.CtrlType ) {
          case VJ_ControllerType.VX_Macro:
            //  a string commands to be built outside, this is for one command only
            ret = $"{MACRO}";
            break;
          case VJ_ControllerType.VJ_Axis:
            // { "A": {"Direction": "X|Y|Z", "Value": number } }
            ret = $"{{ \"A\": {{ {JAxisDirection}, {JAnalogValue} }} }}";
            break;
          case VJ_ControllerType.VJ_RotAxis:
            // { "R": {"Direction": "X|Y|Z", "Value": number } }
            ret = $"{{ \"R\": {{ {JAxisDirection}, {JAnalogValue} }} }}";
            break;
          case VJ_ControllerType.VJ_Slider:
            // { "S": {"Index": 1|2, "Value": number} }
            ret = $"{{ \"S\": {{ {JSIndexNumber}, {JAnalogValue} }} }}";
            break;
          case VJ_ControllerType.VJ_Hat:
            // { "P": {"Index": 1|2|3|4, "Direction": "c | u | r | d | l" } }   
            ret = $"{{ \"P\": {{ {JSIndexNumber}, {JPOVDirection} }} }}";
            break;
          case VJ_ControllerType.VJ_Button:
            // Button:   { "B": {"Index": n, "Mode": "p|r|t|s|d", "Delay":100 } } 
            ret = $"{{ \"B\": {{ {JSIndexNumber}, {JHitMode} }} }}";
            break;
          case VJ_ControllerType.DX_Key:
            // { "K": {"VKcodeEx": "keyName", "VKcode": n, "Mode": "p|r|t|s|d", "Modifier": "mod", "Delay": 100 } }  
            ret = $"{{ \"K\": {{ {JSKeyCodeNumber}, {JHitMode}, {JKeyModifier} }} }}";
            break;
          default:
            ret = "{}";
            break;
        }
        return ret;
      }
    }

    #endregion

  }; // class

  #endregion

  /// <summary>
  /// Small helpers for Panel commands
  /// To have the logic in one place..
  /// </summary>
  class PanelCommand
  {
    public static string Input_On( string input )
    {
      return input + "_on";
    }
    public static string Input_Off( string input )
    {
      return input + "_off";
    }

  }

}

