namespace ProFlightPanelSupport
{
  partial class PFPanel
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PFPanel));
      this.btStartService = new System.Windows.Forms.Button();
      this.btStopService = new System.Windows.Forms.Button();
      this.RTB = new System.Windows.Forms.RichTextBox();
      this.BGW_Hid = new System.ComponentModel.BackgroundWorker();
      this.cbxNLed = new System.Windows.Forms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.txConfigFile = new System.Windows.Forms.TextBox();
      this.btLoadConfigFile = new System.Windows.Forms.Button();
      this.OFD = new System.Windows.Forms.OpenFileDialog();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.cbxJoystick = new System.Windows.Forms.CheckedListBox();
      this.cbxKBon = new System.Windows.Forms.CheckBox();
      this.lblSCdx = new System.Windows.Forms.Label();
      this.lblVJoy = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.lblVersion = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.cbxReport = new System.Windows.Forms.CheckBox();
      this.groupBox3.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
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
      // RTB
      // 
      this.RTB.Location = new System.Drawing.Point(16, 129);
      this.RTB.Name = "RTB";
      this.RTB.Size = new System.Drawing.Size(475, 341);
      this.RTB.TabIndex = 3;
      this.RTB.Text = "";
      // 
      // BGW_Hid
      // 
      this.BGW_Hid.WorkerReportsProgress = true;
      this.BGW_Hid.WorkerSupportsCancellation = true;
      this.BGW_Hid.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BGW_Hid_DoWork);
      this.BGW_Hid.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BGW_Hid_ProgressChanged);
      this.BGW_Hid.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BGW_Hid_RunWorkerCompleted);
      // 
      // cbxNLed
      // 
      this.cbxNLed.AutoSize = true;
      this.cbxNLed.Location = new System.Drawing.Point(400, 123);
      this.cbxNLed.Name = "cbxNLed";
      this.cbxNLed.Size = new System.Drawing.Size(91, 17);
      this.cbxNLed.TabIndex = 4;
      this.cbxNLed.Text = "N_Led Green";
      this.cbxNLed.UseVisualStyleBackColor = true;
      this.cbxNLed.Visible = false;
      this.cbxNLed.CheckedChanged += new System.EventHandler(this.cbxNLed_CheckedChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 28);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(63, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "ConfigFile:";
      // 
      // txConfigFile
      // 
      this.txConfigFile.Cursor = System.Windows.Forms.Cursors.Arrow;
      this.txConfigFile.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txConfigFile.Location = new System.Drawing.Point(101, 25);
      this.txConfigFile.Name = "txConfigFile";
      this.txConfigFile.ReadOnly = true;
      this.txConfigFile.Size = new System.Drawing.Size(345, 19);
      this.txConfigFile.TabIndex = 6;
      // 
      // btLoadConfigFile
      // 
      this.btLoadConfigFile.Location = new System.Drawing.Point(452, 25);
      this.btLoadConfigFile.Name = "btLoadConfigFile";
      this.btLoadConfigFile.Size = new System.Drawing.Size(39, 22);
      this.btLoadConfigFile.TabIndex = 7;
      this.btLoadConfigFile.Text = "...";
      this.btLoadConfigFile.UseVisualStyleBackColor = true;
      this.btLoadConfigFile.Click += new System.EventHandler(this.btLoadConfigFile_Click);
      // 
      // OFD
      // 
      this.OFD.DefaultExt = "json";
      this.OFD.FileName = "PFSWconfig.json";
      this.OFD.Filter = "Config Files|*.json|All Files|*.*";
      this.OFD.SupportMultiDottedExtensions = true;
      this.OFD.Title = "Load Pro Flight Panel Configuration";
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.cbxJoystick);
      this.groupBox3.Controls.Add(this.cbxKBon);
      this.groupBox3.Controls.Add(this.lblSCdx);
      this.groupBox3.Controls.Add(this.lblVJoy);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Controls.Add(this.label4);
      this.groupBox3.Controls.Add(this.cbxNLed);
      this.groupBox3.Location = new System.Drawing.Point(12, 25);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(510, 146);
      this.groupBox3.TabIndex = 8;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Device Support";
      // 
      // cbxJoystick
      // 
      this.cbxJoystick.FormattingEnabled = true;
      this.cbxJoystick.Location = new System.Drawing.Point(149, 41);
      this.cbxJoystick.Name = "cbxJoystick";
      this.cbxJoystick.Size = new System.Drawing.Size(90, 72);
      this.cbxJoystick.TabIndex = 14;
      this.cbxJoystick.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cbxJoystick_ItemCheck);
      // 
      // cbxKBon
      // 
      this.cbxKBon.AutoSize = true;
      this.cbxKBon.Enabled = false;
      this.cbxKBon.Location = new System.Drawing.Point(148, 119);
      this.cbxKBon.Name = "cbxKBon";
      this.cbxKBon.Size = new System.Drawing.Size(40, 17);
      this.cbxKBon.TabIndex = 7;
      this.cbxKBon.Text = "on";
      this.cbxKBon.UseVisualStyleBackColor = true;
      this.cbxKBon.CheckedChanged += new System.EventHandler(this.cbxKBon_CheckedChanged);
      // 
      // lblSCdx
      // 
      this.lblSCdx.AutoSize = true;
      this.lblSCdx.Location = new System.Drawing.Point(197, 120);
      this.lblSCdx.Name = "lblSCdx";
      this.lblSCdx.Size = new System.Drawing.Size(16, 13);
      this.lblSCdx.TabIndex = 6;
      this.lblSCdx.Text = "...";
      // 
      // lblVJoy
      // 
      this.lblVJoy.AutoSize = true;
      this.lblVJoy.Location = new System.Drawing.Point(145, 25);
      this.lblVJoy.Name = "lblVJoy";
      this.lblVJoy.Size = new System.Drawing.Size(16, 13);
      this.lblVJoy.TabIndex = 5;
      this.lblVJoy.Text = "...";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 120);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(93, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "SCdx - Keyboard:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 25);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(80, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "vJoy - Joystick:";
      // 
      // lblVersion
      // 
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new System.Drawing.Point(17, 9);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new System.Drawing.Size(44, 13);
      this.lblVersion.TabIndex = 9;
      this.lblVersion.Text = "label10";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.cbxReport);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.txConfigFile);
      this.groupBox1.Controls.Add(this.btLoadConfigFile);
      this.groupBox1.Controls.Add(this.RTB);
      this.groupBox1.Controls.Add(this.btStartService);
      this.groupBox1.Controls.Add(this.btStopService);
      this.groupBox1.Location = new System.Drawing.Point(12, 177);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(510, 485);
      this.groupBox1.TabIndex = 10;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Service";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(146, 101);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(237, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "If the service does not stop, toggle a switch..";
      // 
      // cbxReport
      // 
      this.cbxReport.AutoSize = true;
      this.cbxReport.Location = new System.Drawing.Point(149, 62);
      this.cbxReport.Name = "cbxReport";
      this.cbxReport.Size = new System.Drawing.Size(97, 17);
      this.cbxReport.TabIndex = 9;
      this.cbxReport.Text = "Report Events";
      this.cbxReport.UseVisualStyleBackColor = true;
      this.cbxReport.CheckedChanged += new System.EventHandler(this.cbxReport_CheckedChanged);
      // 
      // PFPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(532, 674);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.lblVersion);
      this.Controls.Add(this.groupBox3);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PFPanel";
      this.Text = "Pro Flight Panel Support";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PFPanel_FormClosing);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PFPanel_FormClosed);
      this.Load += new System.EventHandler(this.PFPanel_Load);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btStartService;
    private System.Windows.Forms.Button btStopService;
    private System.Windows.Forms.RichTextBox RTB;
    private System.ComponentModel.BackgroundWorker BGW_Hid;
    private System.Windows.Forms.CheckBox cbxNLed;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox txConfigFile;
    private System.Windows.Forms.Button btLoadConfigFile;
    private System.Windows.Forms.OpenFileDialog OFD;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.CheckedListBox cbxJoystick;
    private System.Windows.Forms.CheckBox cbxKBon;
    private System.Windows.Forms.Label lblSCdx;
    private System.Windows.Forms.Label lblVJoy;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label lblVersion;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox cbxReport;
    private System.Windows.Forms.Label label2;
  }
}

