using System.Runtime.Serialization;

using vjMapper.JInput;
using vjMapper.VjOutput;

namespace ProFlightPanelSupport.Commands
{

  /// <summary>
  /// The Device Mapping File
  /// </summary>
  [DataContract]
  internal class ConfigFile
  {
    /*
        {
          "_Comment" : "SwitchPanel Config File",
          "MapName" : "AnyNameWillDo",
      	  "Macros"   : [MACRO,..],
          "SwitchMap": [
              { "Input": "MASTER_BATT",       "Cmd" : [ COMMAND_on, COMMAND_off ] },
              ...
            ],
          "Rotary": 
              { "Input": "ROTARY",               "Cmd" : [COMMAND_posOff, COMMAND_posR, COMMAND_posL, COMMAND_posAll, COMMAND_posStart] }
        }

         COMMAND is defined in vjMapping
    */

    // public members
    [DataMember( Name = "Address" )]    // optional
    public string _Comment { get; set; }
    [DataMember( IsRequired = true, Name = "MapName" )]
    public string MapName { get; set; }

    // private members
    [DataMember( Name = "Macros" )]     // optional
    private MacroDefList Macros { get; set; } = new MacroDefList( );
    [DataMember( Name = "SwitchMap" )]  // optional
    private InputSwitchList SwitchMap { get; set; } = new InputSwitchList( );
    [DataMember( Name = "Rotary" )]     // optional
    private InputRotary Rotary { get; set; } = new InputRotary( );

    // non Json

    /// <summary>
    /// Return the dictionary of commands 
    /// </summary>
    /// <returns>A Dictionary with Input NAME as Key and the corresponding VJCommand</returns>
    public VJCommandDict VJCommandDict
    {
      get {
        // combine all commands
        var ret = Rotary.VJCommands( Macros );              // get rotary
        ret.Append( SwitchMap.VJCommandDict( Macros ) );    // get switches

        return ret; // all collected commands
      }

    }

  }



}