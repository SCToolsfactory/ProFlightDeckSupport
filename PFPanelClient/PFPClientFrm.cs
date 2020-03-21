using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using PFPanelClient.SwitchPanel;

namespace PFPanelClient
{
  public partial class PFPClientFrm : Form
  {

    private const string AppName = "Pro Flight Panel - SCJoyClient";

    private bool m_cancelPending = false;
    private bool m_reportEvents = false;


    #region Form Handling

    private string IconStringRunning { get => $"{AppName}\nService active"; }
    private string IconStringStopped { get => $"{AppName}\nService stopped"; }
    private string IconStringIdle { get => $"{AppName}\nService idle"; }

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

    public PFPClientFrm()
    {
      InitializeComponent( );
    }

    private void PFPClientFrm_Load( object sender, EventArgs e )
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

      ICON.Text = IconStringIdle;

      txLocIP.Text = AppSettings.Instance.ServerIP;
      txPort.Text = AppSettings.Instance.ServerPort;

      string s = AppSettings.Instance.ConfigFile;
      if ( !string.IsNullOrEmpty( s ) && File.Exists( s ) ) {
        txConfigFile.Text = s;
      }

      btStopService.Enabled = false;
    }

    private void PFPClientFrm_FormClosing( object sender, FormClosingEventArgs e )
    {
      if ( m_cancelPending ) return; // had a call before - so just die now
      ICON.Text = IconStringStopped;

      // don't record minimized, maximized forms
      if ( this.WindowState == FormWindowState.Normal ) {
        AppSettings.Instance.FormLocation = this.Location;
      }
      AppSettings.Instance.ServerIP = txLocIP.Text;
      AppSettings.Instance.ServerPort = txPort.Text;
      AppSettings.Instance.ReportEvents = cbxReport.Checked;
      AppSettings.Instance.Save( );

      if ( BGW_Hid.IsBusy ) {
        BGW_Hid.CancelAsync( );
        m_cancelPending = true;
        e.Cancel = true; // will close if we finished the worker
      }
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
      ICON.Text = IconStringIdle;

    }

    #endregion

    private void btStartService_Click( object sender, EventArgs e )
    {
      btStartService.Enabled = false;
      // prepare context
      m_switchPanelSupport.LedChanged = false;
      m_switchPanelSupport.ServerIP = txLocIP.Text;
      m_switchPanelSupport.ServerPort = txPort.Text;
      m_switchPanelSupport.ConfigFile = txConfigFile.Text;

      RTB.Text = $"Starting Service\n";
      // START
      BGW_Hid.RunWorkerAsync( m_switchPanelSupport );
      ICON.Text = IconStringRunning;
      btStopService.Enabled = true;
    }

    private void btStopService_Click( object sender, EventArgs e )
    {
      btStopService.Enabled = false;
      ICON.Text = IconStringStopped;
      BGW_Hid.CancelAsync( );

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

    private void btMyIP_Click( object sender, EventArgs e )
    {
      txLocIP.Text = Protocol.UdpMessenger.GetLocalIP( );
    }

    private void ICON_DoubleClick( object sender, EventArgs e )
    {
      // Show the form when the user double clicks on the notify icon.
      if ( this.WindowState == FormWindowState.Minimized )
        this.WindowState = FormWindowState.Normal;

      // Activate the form.
      this.Activate( );
    }


  }
}
