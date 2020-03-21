namespace PFPanelClient
{
  partial class PFPClientFrm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) ) {
        components.Dispose( );
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PFPClientFrm));
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.txLocIP = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.txPort = new System.Windows.Forms.TextBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.label4 = new System.Windows.Forms.Label();
      this.cbxReport = new System.Windows.Forms.CheckBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txConfigFile = new System.Windows.Forms.TextBox();
      this.btLoadConfigFile = new System.Windows.Forms.Button();
      this.RTB = new System.Windows.Forms.RichTextBox();
      this.btStartService = new System.Windows.Forms.Button();
      this.btStopService = new System.Windows.Forms.Button();
      this.lblVersion = new System.Windows.Forms.Label();
      this.BGW_Hid = new System.ComponentModel.BackgroundWorker();
      this.OFD = new System.Windows.Forms.OpenFileDialog();
      this.btMyIP = new System.Windows.Forms.Button();
      this.ICON = new System.Windows.Forms.NotifyIcon(this.components);
      this.groupBox1.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btMyIP);
      this.groupBox1.Controls.Add(this.txLocIP);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txPort);
      this.groupBox1.Location = new System.Drawing.Point(12, 25);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(375, 53);
      this.groupBox1.TabIndex = 7;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Server to connect to";
      // 
      // txLocIP
      // 
      this.txLocIP.Location = new System.Drawing.Point(78, 19);
      this.txLocIP.Name = "txLocIP";
      this.txLocIP.Size = new System.Drawing.Size(114, 20);
      this.txLocIP.TabIndex = 1;
      this.txLocIP.Text = "127.0.0.1";
      this.txLocIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(248, 22);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(29, 13);
      this.label3.TabIndex = 4;
      this.label3.Text = "Port:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(63, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Connect IP:";
      // 
      // txPort
      // 
      this.txPort.Location = new System.Drawing.Point(283, 19);
      this.txPort.Name = "txPort";
      this.txPort.Size = new System.Drawing.Size(81, 20);
      this.txPort.TabIndex = 2;
      this.txPort.Text = "34123";
      this.txPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Controls.Add(this.cbxReport);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.txConfigFile);
      this.groupBox3.Controls.Add(this.btLoadConfigFile);
      this.groupBox3.Controls.Add(this.RTB);
      this.groupBox3.Controls.Add(this.btStartService);
      this.groupBox3.Controls.Add(this.btStopService);
      this.groupBox3.Location = new System.Drawing.Point(12, 84);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(375, 485);
      this.groupBox3.TabIndex = 11;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Service";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(135, 101);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(218, 13);
      this.label4.TabIndex = 10;
      this.label4.Text = "If the service does not stop, toggle a switch..";
      // 
      // cbxReport
      // 
      this.cbxReport.AutoSize = true;
      this.cbxReport.Location = new System.Drawing.Point(149, 62);
      this.cbxReport.Name = "cbxReport";
      this.cbxReport.Size = new System.Drawing.Size(94, 17);
      this.cbxReport.TabIndex = 9;
      this.cbxReport.Text = "Report Events";
      this.cbxReport.UseVisualStyleBackColor = true;
      this.cbxReport.CheckedChanged += new System.EventHandler(this.cbxReport_CheckedChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(16, 28);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(56, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "ConfigFile:";
      // 
      // txConfigFile
      // 
      this.txConfigFile.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.txConfigFile.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txConfigFile.Location = new System.Drawing.Point(78, 25);
      this.txConfigFile.Name = "txConfigFile";
      this.txConfigFile.ReadOnly = true;
      this.txConfigFile.Size = new System.Drawing.Size(286, 19);
      this.txConfigFile.TabIndex = 6;
      // 
      // btLoadConfigFile
      // 
      this.btLoadConfigFile.Location = new System.Drawing.Point(325, 50);
      this.btLoadConfigFile.Name = "btLoadConfigFile";
      this.btLoadConfigFile.Size = new System.Drawing.Size(39, 22);
      this.btLoadConfigFile.TabIndex = 7;
      this.btLoadConfigFile.Text = "...";
      this.btLoadConfigFile.UseVisualStyleBackColor = true;
      this.btLoadConfigFile.Click += new System.EventHandler(this.btLoadConfigFile_Click);
      // 
      // RTB
      // 
      this.RTB.Location = new System.Drawing.Point(16, 129);
      this.RTB.Name = "RTB";
      this.RTB.Size = new System.Drawing.Size(348, 341);
      this.RTB.TabIndex = 3;
      this.RTB.Text = "";
      // 
      // btStartService
      // 
      this.btStartService.Location = new System.Drawing.Point(19, 53);
      this.btStartService.Name = "btStartService";
      this.btStartService.Size = new System.Drawing.Size(110, 32);
      this.btStartService.TabIndex = 0;
      this.btStartService.Text = "Start Service";
      this.btStartService.UseVisualStyleBackColor = true;
      this.btStartService.Click += new System.EventHandler(this.btStartService_Click);
      // 
      // btStopService
      // 
      this.btStopService.Location = new System.Drawing.Point(19, 91);
      this.btStopService.Name = "btStopService";
      this.btStopService.Size = new System.Drawing.Size(110, 32);
      this.btStopService.TabIndex = 1;
      this.btStopService.Text = "Stop Service";
      this.btStopService.UseVisualStyleBackColor = true;
      this.btStopService.Click += new System.EventHandler(this.btStopService_Click);
      // 
      // lblVersion
      // 
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new System.Drawing.Point(12, 9);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(41, 13);
      this.lblVersion.TabIndex = 12;
      this.lblVersion.Text = "label10";
      // 
      // BGW_Hid
      // 
      this.BGW_Hid.WorkerReportsProgress = true;
      this.BGW_Hid.WorkerSupportsCancellation = true;
      this.BGW_Hid.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_Hid_DoWork);
      this.BGW_Hid.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BGW_Hid_ProgressChanged);
      this.BGW_Hid.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_Hid_RunWorkerCompleted);
      // 
      // OFD
      // 
      this.OFD.DefaultExt = "json";
      this.OFD.FileName = "PFSWconfig.json";
      this.OFD.Filter = "Config Files|*.json|All Files|*.*";
      this.OFD.SupportMultiDottedExtensions = true;
      this.OFD.Title = "Load Pro Flight Panel Configuration";
      // 
      // btMyIP
      // 
      this.btMyIP.Location = new System.Drawing.Point(198, 14);
      this.btMyIP.Name = "btMyIP";
      this.btMyIP.Size = new System.Drawing.Size(44, 28);
      this.btMyIP.TabIndex = 5;
      this.btMyIP.Text = "myIP";
      this.btMyIP.UseVisualStyleBackColor = true;
      this.btMyIP.Click += new System.EventHandler(this.btMyIP_Click);
      // 
      // ICON
      // 
      this.ICON.Icon = ((System.Drawing.Icon)(resources.GetObject("ICON.Icon")));
      this.ICON.Text = "ProFlightPanel - SCJoyClient";
      this.ICON.Visible = true;
      this.ICON.DoubleClick += new System.EventHandler(this.ICON_DoubleClick);
      // 
      // PFPClientFrm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(396, 577);
      this.Controls.Add(this.lblVersion);
      this.Controls.Add(this.groupBox3);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PFPClientFrm";
      this.Text = "Pro Flight Panel - SCJoyClient";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PFPClientFrm_FormClosing);
      this.Load += new System.EventHandler(this.PFPClientFrm_Load);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox txLocIP;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txPort;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.CheckBox cbxReport;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txConfigFile;
    private System.Windows.Forms.Button btLoadConfigFile;
    private System.Windows.Forms.RichTextBox RTB;
    private System.Windows.Forms.Button btStartService;
    private System.Windows.Forms.Button btStopService;
    private System.Windows.Forms.Label lblVersion;
    private System.ComponentModel.BackgroundWorker BGW_Hid;
    private System.Windows.Forms.OpenFileDialog OFD;
    private System.Windows.Forms.Button btMyIP;
    private System.Windows.Forms.NotifyIcon ICON;
  }
}

