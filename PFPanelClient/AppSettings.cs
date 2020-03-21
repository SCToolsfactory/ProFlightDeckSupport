using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFPanelClient
{
  sealed class AppSettings : ApplicationSettingsBase
  {

    // Singleton
    private static readonly Lazy<AppSettings> m_lazy = new Lazy<AppSettings>( () => new AppSettings( ) );
    public static AppSettings Instance { get => m_lazy.Value; }

    private AppSettings()
    {
      if ( this.FirstRun ) {
        // migrate the settings to the new version if the app runs the first time
        try {
          this.Upgrade( );
        }
        catch { }
        this.FirstRun = false;
        this.Save( );
      }
    }

    #region Setting Properties

    // manages Upgrade
    [UserScopedSetting( )]
    [DefaultSettingValue( "True" )]
    public bool FirstRun
    {
      get { return (bool)this["FirstRun"]; }
      set { this["FirstRun"] = value; }
    }


    // Control bound settings
    [UserScopedSetting( )]
    [DefaultSettingValue( "10, 10" )]
    public Point FormLocation
    {
      get { return (Point)this["FormLocation"]; }
      set { this["FormLocation"] = value; }
    }

    // User Config Settings

    // Devices
    [UserScopedSetting( )]
    [DefaultSettingValue( "False" )]
    public bool UseKeyboard
    {
      get { return (bool)this["UseKeyboard"]; }
      set { this["UseKeyboard"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string JoystickUsed
    {
      get { return (string)this["JoystickUsed"]; }
      set { this["JoystickUsed"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "False" )]
    public bool ReportEvents
    {
      get { return (bool)this["ReportEvents"]; }
      set { this["ReportEvents"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "" )]
    public string ConfigFile
    {
      get { return (string)this["ConfigFile"]; }
      set { this["ConfigFile"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "127.0.0.1" )]
    public string ServerIP
    {
      get { return (string)this["ServerIP"]; }
      set { this["ServerIP"] = value; }
    }

    [UserScopedSetting( )]
    [DefaultSettingValue( "34123" )]
    public string ServerPort
    {
      get { return (string)this["ServerPort"]; }
      set { this["ServerPort"] = value; }
    }


    #endregion


  }
}
