using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///
/// Shameless plug from:
/// https://github.com/mikeobrien/HidLibrary/tree/master/examples/GriffinPowerMate/PowerMate
/// adopted for the Panel
/// </summary>
namespace PFSP_HID
{
  /// <summary>
  /// Provides data for Pro Flight Switch Panel Switch events.
  /// </summary>
  public class PFSwPanelSwitchEventArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the PFSwPanelSwitchEventArgs class.
    /// </summary>
    /// <param name="state"></param>
    public PFSwPanelSwitchEventArgs( PFSwPanelState state, PFSwPanelSwitches switch_ )
    {
      State = state;
      Switch = switch_;
    }

    /// <summary>
    /// Gets the current Pro Flight Switch Panel state.
    /// </summary>
    public PFSwPanelState State { get; private set; }
    /// <summary>
    /// Gets the changed Pro Flight Switch
    /// </summary>
    public PFSwPanelSwitches Switch { get; private set; }
  }



  /// <summary>
  /// Provides data for Pro Flight Switch Panel Rotary events.
  /// </summary>
  public class PFSwPanelRotaryEventArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the PFSwPanelRotaryEventArgs class.
    /// </summary>
    /// <param name="state"></param>
    public PFSwPanelRotaryEventArgs( PFSwPanelState state )
    {
      State = state;
    }

    /// <summary>
    /// Gets the current Pro Flight Panel state.
    /// </summary>
    public PFSwPanelState State { get; private set; }
  }

}