using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using dxKbdInterfaceWrap;
using ProFlightPanelSupport.SwitchPanel;

namespace ProFlightPanelSupport
{
  public partial class PFPanel : Form
  {

    private bool m_cancelPending = false;
    private bool m_reportEvents = false;

    #region Form Handling

    /// <summary>
    /// Checks if a rectangle is visible on any screen
    /// </summary>
    /// <param name="formRect"></param>
    /// <returns>True if visible</returns>
    private static bool IsOnScreen( Rectangle formRect )
    {
      Screen[] screens = Screen.AllScreens;
      foreach ( Screen screen in screens ) {
        if ( screen.WorkingArea.Contains( formRect ) ) {
          return true;
        }
      }
      return false;
    }



    public PFPanel()
    {
      InitializeComponent( );
    }

    private void PFPanel_Load( object sender, EventArgs e )
    {

      AppSettings.Instance.Reload( );

      // Assign Size property - check if on screen, else use defaults
      if ( IsOnScreen( new Rectangle( AppSettings.Instance.FormLocation, this.Size ) ) ) {
        this.Location = AppSettings.Instance.FormLocation;
      }

      string version = Application.ProductVersion;  // get the version information
      // BETA VERSION; TODO -  comment out if not longer
      //lblTitle.Text += " - V " + version.Substring( 0, version.IndexOf( ".", version.IndexOf( "." ) + 1 ) ); // PRODUCTION
      lblVersion.Text = "Version: " + version + " beta"; // BETA

      string s = AppSettings.Instance.ConfigFile;
      if ( !string.IsNullOrEmpty( s ) && File.Exists( s ) ) {
        txConfigFile.Text = s;
      }

      btStopService.Enabled = false;

      // vJoy DLL
      cbxJoystick.Items.Clear( );
      lblVJoy.Text = "not available";
      if ( vJoyInterfaceWrap.vJoy.isDllLoaded ) {
        var tvJoy = new vJoyInterfaceWrap.vJoy( );
        for ( uint i = 1; i <= 16; i++ ) {
          if ( tvJoy.isVJDExists( i ) ) {
            cbxJoystick.Items.Add( $"Joystick#{i}" );
          }
        }
        if ( cbxJoystick.Items.Count > 0 ) {
          cbxJoystick.SelectedIndex = 0;
          // select the one in AppSettings
          string[] js = AppSettings.Instance.JoystickUsed.Split( new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries ); // a list
          for ( int i = 0; i < js.Length; i++ ) {
            var idx = cbxJoystick.Items.IndexOf( js[i] );
            if ( idx >= 0 ) {
              cbxJoystick.SetItemChecked( idx, true );
            }
          }
        }
        lblVJoy.Text = $"loaded   - {cbxJoystick.Items.Count:#} device(s)";
        tvJoy = null;
      }
      // Kbd DLL
      if ( SCdxKeyboard.isDllLoaded ) {
        lblSCdx.Text = "loaded";
        cbxKBon.Enabled = true;
        cbxKBon.Checked = AppSettings.Instance.UseKeyboard;
      }
      else {
        lblSCdx.Text = "not available";
        cbxKBon.Checked = false;
        cbxKBon.Enabled = false;
      }

    }

    private void PFPanel_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( m_cancelPending ) return; // had a call before - so just die now

      // don't record minimized, maximized forms
      if ( this.WindowState == FormWindowState.Normal ) {
        AppSettings.Instance.FormLocation = this.Location;
      }
      AppSettings.Instance.UseKeyboard = cbxKBon.Checked;
      string s = "";
      foreach ( var cl in cbxJoystick.CheckedItems ) {
        s += cl.ToString( ) + " ";
      }
      AppSettings.Instance.JoystickUsed = s;
      AppSettings.Instance.ReportEvents = cbxReport.Checked;
      AppSettings.Instance.Save( );

      if ( BGW_Hid.IsBusy ) {
        BGW_Hid.CancelAsync( );
        m_cancelPending = true;
        e.Cancel = true; // will close if we finished the worker
      }
    }

    private void PFPanel_FormClosed( object sender, FormClosedEventArgs e )
    {

    }

    #endregion


    #region BGW Hid

    /// <summary>
    /// The command and control IF
    /// </summary>
    private BgwSwitchPanelContext m_switchPanelSupport = new BgwSwitchPanelContext( );

    // this is already asynch - no GUI interaction here
    private void BGW_Hid_DoWork( object sender, DoWorkEventArgs e )
    {
      if ( e.Argument is BgwSwitchPanelContext ) {
        var SP = new SwitchPanelSupport( );
        SP.DoWork( sender, (BgwSwitchPanelContext)e.Argument );
        // returns only if cancelled..
      }
    }

    private void BGW_Hid_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      if ( e.ProgressPercentage == 0 )
        RTB.Text += $"BGW_Hid report: {(string)e.UserState}\n";
      else if ( e.ProgressPercentage == 100 )
        RTB.Text += $"BGW_Hid report: {(string)e.UserState}\n";

      else if ( m_reportEvents )
        RTB.Text += $"BGW_Hid report: {(string)e.UserState}\n";

    }

    private void BGW_Hid_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
    {
      RTB.Text += $"BGW_Hid has completed\n";
      if ( m_cancelPending ) {
        this.Close( );
      }
      btStartService.Enabled = true;
      btStopService.Enabled = false;
      cbxJoystick.Enabled = true;
    }

    #endregion

    private void btStartService_Click( object sender, EventArgs e )
    {
      if ( cbxJoystick.CheckedIndices.Count < 1 ) {
        RTB.Text += "ERROR: No Joystick selected - cannot start";
        return;
      }

      btStartService.Enabled = false;
      cbxJoystick.Enabled = false;
      // prepare context
      m_switchPanelSupport.LedChanged = false;
      m_switchPanelSupport.JoystickNo = 0;
      m_switchPanelSupport.ConfigFile = txConfigFile.Text;
      var jsx = cbxJoystick.CheckedItems[0];
      string[] js = ( jsx as string ).Split( new char[] { '#' } );
      if ( js.Length > 1 ) {
        m_switchPanelSupport.JoystickNo = int.Parse( js[1] );
      }
      RTB.Text = $"Starting Service\n";
      // START
      BGW_Hid.RunWorkerAsync( m_switchPanelSupport );

      btStopService.Enabled = true;
    }

    private void btStopService_Click( object sender, EventArgs e )
    {
      btStopService.Enabled = false;
      BGW_Hid.CancelAsync( );
    }

    private void cbxNLed_CheckedChanged( object sender, EventArgs e )
    {
      m_switchPanelSupport.N_Led = ( cbxNLed.Checked ) ? PFSP_HID.PFSwPanelLedState.Led_Green : PFSP_HID.PFSwPanelLedState.Led_Off;
      m_switchPanelSupport.LedChanged = true; // trigger change
    }

    private void btLoadConfigFile_Click( object sender, EventArgs e )
    {
      OFD.FileName = txConfigFile.Text;
      if ( OFD.ShowDialog( this ) == DialogResult.OK ) {
        txConfigFile.Text = OFD.FileName;
        m_switchPanelSupport.ConfigFile = OFD.FileName;
        AppSettings.Instance.ConfigFile = OFD.FileName;
        AppSettings.Instance.Save( );
      }
    }

    private void cbxReport_CheckedChanged( object sender, EventArgs e )
    {
      m_reportEvents = cbxReport.Checked;
    }

    private void cbxKBon_CheckedChanged( object sender, EventArgs e )
    {
      if ( SCdxKeyboard.isDllLoaded )
        SCdxKeyboard.Enabled = cbxKBon.Checked;
    }

    private void cbxJoystick_ItemCheck( object sender, ItemCheckEventArgs e )
    {
      // if a new one is checked, others get unchecked..
      if ( e.NewValue == CheckState.Checked ) {
        foreach ( int i in cbxJoystick.CheckedIndices ) {
          if ( i != e.Index ) cbxJoystick.SetItemChecked( i, false );
        }
      }
    }


  }
}
