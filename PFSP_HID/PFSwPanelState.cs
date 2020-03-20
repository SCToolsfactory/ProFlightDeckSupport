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
  /// State for the PFSwPanel Rotary knob
  /// </summary>
  public enum PFSwPanelRotaryState
  {
    Pos_Off = 0,
    Pos_R,
    Pos_L,
    Pos_All,
    Pos_Start
  }

  /// <summary>
  /// State for the PFSwPanel Switches - Up(ON) or Down(OFF)
  /// </summary>
  public enum PFSwPanelSwitchState
  {
    Off_Down = 0,
    On_Up = 1,
  }

  /// <summary>
  /// All Panel Switches
  /// </summary>
  public enum PFSwPanelSwitches
  {
    // top row
    MASTER_BATT = 0,
    MASTER_ALT,
    AVIONICS_MASTER,
    FUEL_PUMP,
    DE_ICE,
    PITOT_HEAT,
    // right end
    GEAR,
    // bottom row
    COWL_CLOSE,
    PANEL,
    BEACON,
    NAV,
    STROBE,
    TAXI,
    LANDING,
    //Undef or disable
    NONE,
  }

  /// <summary>
  /// State for the PFSwPanel LED
  /// </summary>
  public enum PFSwPanelLedState
  {
    Led_Off = 0,
    Led_Green,
    Led_Red,
    Led_Amber // Orange...
  }

  /// <summary>
  /// All PFSwPanel LEDs
  /// </summary>
  public enum PFSwPanelLeds
  {
    GEAR_N = 0,
    GEAR_L,
    GEAR_R,
    //Undef or disable
    GEAR_NONE,
  }

  /// <summary>
  /// Helper to provide array lengths, Enums a prefabricated arrays for Switches and Leds
  /// </summary>
  public class Consts
  {
    private static readonly IList<PFSwPanelSwitches> m_switches = new List<PFSwPanelSwitches>( )
      { PFSwPanelSwitches.MASTER_BATT, PFSwPanelSwitches.MASTER_ALT, PFSwPanelSwitches.AVIONICS_MASTER, PFSwPanelSwitches.FUEL_PUMP,
          PFSwPanelSwitches.DE_ICE, PFSwPanelSwitches.PITOT_HEAT,
        PFSwPanelSwitches.GEAR,
        PFSwPanelSwitches.COWL_CLOSE, PFSwPanelSwitches.PANEL, PFSwPanelSwitches.BEACON, PFSwPanelSwitches.NAV, PFSwPanelSwitches.STROBE,
          PFSwPanelSwitches.TAXI, PFSwPanelSwitches.LANDING
      };

    private static readonly IList<PFSwPanelSwitchState> m_SwitcheStates = new List<PFSwPanelSwitchState>( )
      {  PFSwPanelSwitchState.Off_Down, PFSwPanelSwitchState.On_Up };

    private static readonly IList<PFSwPanelLeds> m_leds = new List<PFSwPanelLeds>( )
      { PFSwPanelLeds.GEAR_N, PFSwPanelLeds.GEAR_L, PFSwPanelLeds.GEAR_R };

    private static readonly IList<PFSwPanelLedState> m_ledStates = new List<PFSwPanelLedState>( )
      {  PFSwPanelLedState.Led_Off, PFSwPanelLedState.Led_Green, PFSwPanelLedState.Led_Red, PFSwPanelLedState.Led_Amber };

    private static readonly IList<PFSwPanelRotaryState> m_rotaryStates = new List<PFSwPanelRotaryState>( )
      {  PFSwPanelRotaryState.Pos_Off, PFSwPanelRotaryState.Pos_R,  PFSwPanelRotaryState.Pos_L, PFSwPanelRotaryState.Pos_All, PFSwPanelRotaryState.Pos_Start };

    /// <summary>
    /// The number of Switches
    /// </summary>
    public static readonly int NumSwitches = EnumSwitches.Count;

    /// <summary>
    /// The number of LEDs
    /// </summary>
    public static readonly int NumLeds = EnumLeds.Count;

    /// <summary>
    /// Return a list of valid Switch Enums
    /// </summary>
    public static IList<PFSwPanelSwitches> EnumSwitches { get => m_switches; }

    public static int[] ESwitchesValues
    {
      get {
        var values = Enum.GetValues( typeof( PFSwPanelSwitches ) );
        int[]  nValues = new int[values.Length-1];
        for (int i=0; i<nValues.Length; i++ ) {
          nValues[i] = (int)values.GetValue(i);
        }
        return nValues;
      }
    }


    /// <summary>
    /// Return a list of valid SwitchState Enums
    /// </summary>
    public static IList<PFSwPanelSwitchState> EnumSwitcheState { get => m_SwitcheStates; }

    public static Array ESwitchStateValues { get => Enum.GetValues( typeof( PFSwPanelSwitchState ) ); }

    /// <summary>
    /// Return a list of valid Led Enums
    /// </summary>
    public static IList<PFSwPanelLeds> EnumLeds { get => m_leds; }

    public static Array ELedsValues
    {
      get {
        var values = Enum.GetValues( typeof( PFSwPanelLeds ) );
        int[] nValues = new int[values.Length - 1];
        for ( int i = 0; i < nValues.Length; i++ ) {
          nValues[i] = (int)values.GetValue( i );
        }
        return nValues;
      }
    }

    /// <summary>
    /// Return a list of valid LedState Enums
    /// </summary>
    public static IList<PFSwPanelLedState> EnumLedState { get => m_ledStates; }

    public static Array ELedStateValues { get => Enum.GetValues( typeof( PFSwPanelLedState ) ); }

    /// <summary>
    /// Return a list of valid RotaryState Enums
    /// </summary>
    public static IList<PFSwPanelRotaryState> EnumRotaryState { get => m_rotaryStates; }

    public static Array ERotaryStateValues { get => Enum.GetValues( typeof( PFSwPanelRotaryState ) ); }



    /// <summary>
    /// Return the Enum Name of an item with its value
    /// </summary>
    /// <typeparam name="T">The Enum type</typeparam>
    /// <param name="pos"></param>
    /// <returns>The Enum Name</returns>
    public static string GetEnumName<T>( int enumValue ) where T : Enum
    {
      return Enum.GetName( typeof( T ), enumValue );
    }




    /// <summary>
    /// Return a new Switches Array
    /// </summary>
    public static PFSwPanelSwitchState[] SwitchArray
    {
      get { return new PFSwPanelSwitchState[NumSwitches]; }
    }

    /// <summary>
    /// Return a new Leds Array
    /// </summary>
    public static PFSwPanelLedState[] LedArray
    {
      get { return new PFSwPanelLedState[NumLeds]; }
    }


  }


  /// <summary>
  /// Holds the state for the Pro Flight Switch Panel
  /// 
  /// The struct is immutable.
  /// </summary>
  public struct PFSwPanelState
  {

    private PFSwPanelRotaryState m_rotary;
    private PFSwPanelSwitchState[] m_switches;
    //    private PFSwPanelLedState[] m_leds;
    private bool m_valid;

    /// <summary>
    /// Initializes a new instance (valid) of the PFSwPanelState class.
    /// </summary>
    /// <param name="switchState"></param>
    /// <param name="ledState"></param>
    public PFSwPanelState( PFSwPanelSwitchState[] switchState, PFSwPanelRotaryState rotaryState )
    {
      m_switches = Consts.SwitchArray;
      foreach ( int s in Consts.ESwitchesValues ) {
        m_switches[s] = switchState[s];
      }
      m_rotary = rotaryState;

      this.m_valid = true;
    }

    /// <summary>
    /// Gets the Pro Flight Switch Panel Switch state
    /// </summary>
    public PFSwPanelSwitchState SwitchState( PFSwPanelSwitches switch_ )
    {
      return m_switches[(int)switch_];
    }

    /// <summary>
    /// Gets the Pro Flight Switch Panel Rotary Switch state
    /// </summary>
    public PFSwPanelRotaryState RotaryState()
    {
      return m_rotary;
    }

    /*
    /// <summary>
    /// Gets the Pro Flight Switch Panel Switch state
    /// </summary>
    public PFSwPanelLedState SwitchState( PFSwPanelLeds led )
    {
      return m_leds[(int)led];
    }
    */

    /// <summary>
    /// Gets a value indicating in the PowerMateState instance is valid.
    /// </summary>
    public bool IsValid
    {
      get { return m_valid; }
    }
  }
}