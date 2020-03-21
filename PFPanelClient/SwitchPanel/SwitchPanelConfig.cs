using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using PFPanelClient.Commands;

namespace PFPanelClient.SwitchPanel
{
  /// <summary>
  /// Configuration of the Switch Panel
  ///  this is feed with a JSON file 
  ///  
  /// </summary>
  class SwitchPanelConfig
  {
    /// <summary>
    /// Reads from the open stream one ConfigFile entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A ConfigFile obj or null for errors</returns>
    private static ConfigFile FromJson_low( Stream jStream )
    {
      try {
        var jsonSerializer = new DataContractJsonSerializer( typeof( ConfigFile ) );
        object objResponse = jsonSerializer.ReadObject( jStream );
        var jsonResults = objResponse as ConfigFile;
        return jsonResults;
      }
      catch ( Exception e ) {
        return null;
      }
    }

    /// <summary>
    /// Reads from a file one ConfigFile entry
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A ConfigFile obj or null for errors</returns>
    private static ConfigFile FromJson_low( string jFilename )
    {
      ConfigFile c = null;
      if ( File.Exists( jFilename ) ) {
        using ( var ts = File.OpenRead( jFilename ) ) {
          c = FromJson_low( ts );
        }
      }
      return c;
    }


    /// <summary>
    /// Reads from the open stream and returns a SwitchPanelConfig entry
    /// </summary>
    /// <param name="jStream">An open stream at position</param>
    /// <returns>A SwitchPanelConfig obj</returns>
    public static SwitchPanelConfig FromJson( Stream jStream )
    {
      return new SwitchPanelConfig( FromJson_low( jStream ) );
    }

    /// <summary>
    /// Reads from a file and returns a SwitchPanelConfig entry
    /// </summary>
    /// <param name="jFilename">The Json Filename</param>
    /// <returns>A SwitchPanelConfig obj</returns>
    public static SwitchPanelConfig FromJson( string jFilename )
    {
      return new SwitchPanelConfig( FromJson_low( jFilename ) );
    }


    #region Class

    public bool Valid { get; } = false;
    private Dictionary<string, VJCommand> m_commands = new Dictionary<string, VJCommand>( );
    /// <summary>
    /// All commands as dictonary (string key = input name)
    /// </summary>
    public Dictionary<string, VJCommand> VJCommands { get => m_commands; }

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
      m_commands = configFile.VJCommands;
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
