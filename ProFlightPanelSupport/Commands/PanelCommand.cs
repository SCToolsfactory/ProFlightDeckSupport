using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProFlightPanelSupport.Commands
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
    public const int VJ_MAXBUTTON = 60;  // the last allowed button number
    public const int DEFAULT_DELAY = 150; // msec
    public const int DEFAULT_SHORTDELAY = 5; // msec - short tap const

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
    public IList<VJ_Modifier> CtrlModifier { get=> m_modifiers; }

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
    public bool IsVKeyMessage { get => this.CtrlType == VJ_ControllerType.DX_Key; }

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

