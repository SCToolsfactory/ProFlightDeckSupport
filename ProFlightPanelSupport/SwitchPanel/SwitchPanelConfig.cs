using System.IO;

using ProFlightPanelSupport.Commands;
using vjMapper;
using vjMapper.VjOutput;

namespace ProFlightPanelSupport.SwitchPanel
{
  /// <summary>
  /// Configuration of the Switch Panel
  ///  this is feed with a JSON file 
  ///  
  /// </summary>
  class SwitchPanelConfig
  {
    /// <summary>
    /// Provides error information if we get an invalis object
    /// </summary>
    public static string ErrorMsg { get; private set; } = "";

    /// <summary>
    /// Reads from the open stream and returns a SwitchPanelConfig entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A SwitchPanelConfig obj</returns>
    public static SwitchPanelConfig FromJson( Stream jStream )
    {
      var c = vjMapping.FromJsonStream<ConfigFile>( jStream );
      if ( c == default( ConfigFile ) ) {
        ErrorMsg = vjMapping.ErrorMsg;
      }
      return new SwitchPanelConfig( c );
    }

    /// <summary>
    /// Reads from a file and returns a SwitchPanelConfig entry
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A SwitchPanelConfig obj</returns>
    public static SwitchPanelConfig FromJson( string jFilename )
    {
      var c = vjMapping.FromJsonFile<ConfigFile>( jFilename );
      if ( c == default( ConfigFile ) ) {
        ErrorMsg = vjMapping.ErrorMsg;
      }
      return new SwitchPanelConfig( c );
    }

    #region Class

    public bool Valid { get; } = false;
    private VJCommandDict m_commands = new VJCommandDict( );
    /// <summary>
    /// All commands as dictonary (string key = input name)
    /// </summary>
    public VJCommandDict VJCommands { get => m_commands; }

    private string m_configName = "";
    public string ConfigName
    {
      get { return m_configName; }
    }

    /// <summary>
    /// cTor: decompose the Config file
    /// </summary>
    /// <param name="configFile"></param>
    public SwitchPanelConfig( ConfigFile configFile )
    {
      if ( configFile == null ) return; // No configFile given Valid=> false;

      m_configName = configFile.MapName;
      m_commands = configFile.VJCommandDict;
      Valid = m_commands.Count > 0;
    }


    /// <summary>
    /// Returns a VJCommand for the Input string or null
    /// </summary>
    /// <param name="input">The Switch Name</param>
    /// <returns>A VJCommand for the input or null if not found</returns>
    public VJCommand GetCommand( string input )
    {
      if ( m_commands.ContainsKey( input ) ) {
        return m_commands[input];
      }
      return null;
    }

    /// <summary>
    /// Returns a VJCommand for the Input / RotaryState Enum or null
    /// </summary>
    /// <param name="input">The Switch Name</param>
    /// <returns>A VJCommand for the input or null if not found</returns>
    public VJCommand GetCommand( PFSP_HID.PFSwPanelSwitches input )
    {
      return GetCommand( input.ToString( ) );
    }

    #endregion


  }
}
