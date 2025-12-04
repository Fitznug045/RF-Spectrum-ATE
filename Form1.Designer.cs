namespace SpectrumAnalyzerGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.BtnConnect = new System.Windows.Forms.Button();
            this.BtnStart = new System.Windows.Forms.Button();
            this.TxtResource = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.LblStart = new System.Windows.Forms.Label();
            this.LblStop = new System.Windows.Forms.Label();
            this.LblPeakAmp = new System.Windows.Forms.Label();
            this.LblPeakFreq = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BtnStop = new System.Windows.Forms.Button();
            this.TxtCentFreq = new System.Windows.Forms.TextBox();
            this.TxtSpan = new System.Windows.Forms.TextBox();
            this.BtnApply = new System.Windows.Forms.Button();
            this.LblSpan = new System.Windows.Forms.Label();
            this.LblCentFreq = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BtnRunTest = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Freq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Power = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnImport = new System.Windows.Forms.Button();
            this.BtnClose = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.LblName = new System.Windows.Forms.Label();
            this.BtnDisconnect = new System.Windows.Forms.Button();
            this.BtnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtPort = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnConnect
            // 
            this.BtnConnect.Location = new System.Drawing.Point(312, 6);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(75, 23);
            this.BtnConnect.TabIndex = 0;
            this.BtnConnect.Text = "Connect";
            this.BtnConnect.UseVisualStyleBackColor = true;
            this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(393, 6);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 23);
            this.BtnStart.TabIndex = 1;
            this.BtnStart.Text = "Start";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // TxtResource
            // 
            this.TxtResource.Location = new System.Drawing.Point(92, 34);
            this.TxtResource.Multiline = true;
            this.TxtResource.Name = "TxtResource";
            this.TxtResource.Size = new System.Drawing.Size(181, 20);
            this.TxtResource.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(17, 37);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(64, 13);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "IP Address :";
            // 
            // LblStart
            // 
            this.LblStart.AutoSize = true;
            this.LblStart.Location = new System.Drawing.Point(17, 82);
            this.LblStart.Name = "LblStart";
            this.LblStart.Size = new System.Drawing.Size(87, 13);
            this.LblStart.TabIndex = 6;
            this.LblStart.Text = "Start Freq: - MHz";
            // 
            // LblStop
            // 
            this.LblStop.AutoSize = true;
            this.LblStop.Location = new System.Drawing.Point(17, 98);
            this.LblStop.Name = "LblStop";
            this.LblStop.Size = new System.Drawing.Size(87, 13);
            this.LblStop.TabIndex = 8;
            this.LblStop.Text = "Stop Freq: - MHz";
            // 
            // LblPeakAmp
            // 
            this.LblPeakAmp.AutoSize = true;
            this.LblPeakAmp.Location = new System.Drawing.Point(154, 95);
            this.LblPeakAmp.Name = "LblPeakAmp";
            this.LblPeakAmp.Size = new System.Drawing.Size(89, 13);
            this.LblPeakAmp.TabIndex = 9;
            this.LblPeakAmp.Text = "Peak Amp: - dBm";
            // 
            // LblPeakFreq
            // 
            this.LblPeakFreq.AutoSize = true;
            this.LblPeakFreq.Location = new System.Drawing.Point(154, 82);
            this.LblPeakFreq.Name = "LblPeakFreq";
            this.LblPeakFreq.Size = new System.Drawing.Size(90, 13);
            this.LblPeakFreq.TabIndex = 10;
            this.LblPeakFreq.Text = "Peak Freq: - MHz";
            // 
            // chart1
            // 
            this.chart1.Location = new System.Drawing.Point(293, 61);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(292, 156);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "S";
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(393, 32);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 17;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // TxtCentFreq
            // 
            this.TxtCentFreq.Enabled = false;
            this.TxtCentFreq.Location = new System.Drawing.Point(88, 116);
            this.TxtCentFreq.Name = "TxtCentFreq";
            this.TxtCentFreq.Size = new System.Drawing.Size(48, 20);
            this.TxtCentFreq.TabIndex = 18;
            // 
            // TxtSpan
            // 
            this.TxtSpan.Enabled = false;
            this.TxtSpan.Location = new System.Drawing.Point(88, 137);
            this.TxtSpan.Name = "TxtSpan";
            this.TxtSpan.Size = new System.Drawing.Size(48, 20);
            this.TxtSpan.TabIndex = 19;
            // 
            // BtnApply
            // 
            this.BtnApply.Enabled = false;
            this.BtnApply.Location = new System.Drawing.Point(192, 134);
            this.BtnApply.Name = "BtnApply";
            this.BtnApply.Size = new System.Drawing.Size(75, 23);
            this.BtnApply.TabIndex = 20;
            this.BtnApply.Text = "Apply";
            this.BtnApply.UseVisualStyleBackColor = true;
            this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // LblSpan
            // 
            this.LblSpan.AutoSize = true;
            this.LblSpan.Location = new System.Drawing.Point(17, 144);
            this.LblSpan.Name = "LblSpan";
            this.LblSpan.Size = new System.Drawing.Size(35, 13);
            this.LblSpan.TabIndex = 21;
            this.LblSpan.Text = "Span:";
            // 
            // LblCentFreq
            // 
            this.LblCentFreq.AutoSize = true;
            this.LblCentFreq.Location = new System.Drawing.Point(17, 123);
            this.LblCentFreq.Name = "LblCentFreq";
            this.LblCentFreq.Size = new System.Drawing.Size(65, 13);
            this.LblCentFreq.TabIndex = 22;
            this.LblCentFreq.Text = "Center Freq:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "MHz";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "MHz";
            // 
            // BtnRunTest
            // 
            this.BtnRunTest.Location = new System.Drawing.Point(111, 197);
            this.BtnRunTest.Name = "BtnRunTest";
            this.BtnRunTest.Size = new System.Drawing.Size(75, 23);
            this.BtnRunTest.TabIndex = 25;
            this.BtnRunTest.Text = "Run Test";
            this.BtnRunTest.UseVisualStyleBackColor = true;
            this.BtnRunTest.Click += new System.EventHandler(this.BtnRunTest_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Freq,
            this.Limit,
            this.Power,
            this.Status});
            this.dataGridView1.Location = new System.Drawing.Point(30, 226);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(544, 151);
            this.dataGridView1.TabIndex = 26;
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.MinimumWidth = 6;
            this.No.Name = "No";
            this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Freq
            // 
            this.Freq.HeaderText = "Freq (MHz)";
            this.Freq.MinimumWidth = 6;
            this.Freq.Name = "Freq";
            // 
            // Limit
            // 
            this.Limit.HeaderText = "Limit (dBm)";
            this.Limit.MinimumWidth = 6;
            this.Limit.Name = "Limit";
            // 
            // Power
            // 
            this.Power.HeaderText = "Power (dBm)";
            this.Power.MinimumWidth = 6;
            this.Power.Name = "Power";
            this.Power.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "Status";
            this.Status.MinimumWidth = 6;
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // BtnImport
            // 
            this.BtnImport.Location = new System.Drawing.Point(30, 197);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(75, 23);
            this.BtnImport.TabIndex = 28;
            this.BtnImport.Text = "Import";
            this.BtnImport.UseVisualStyleBackColor = true;
            this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Location = new System.Drawing.Point(474, 32);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(75, 23);
            this.BtnClose.TabIndex = 29;
            this.BtnClose.Text = "Close";
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(192, 197);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 30;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // LblName
            // 
            this.LblName.AutoSize = true;
            this.LblName.Location = new System.Drawing.Point(17, 15);
            this.LblName.Name = "LblName";
            this.LblName.Size = new System.Drawing.Size(84, 13);
            this.LblName.TabIndex = 31;
            this.LblName.Text = "Device Name : -";
            // 
            // BtnDisconnect
            // 
            this.BtnDisconnect.Enabled = false;
            this.BtnDisconnect.Location = new System.Drawing.Point(312, 32);
            this.BtnDisconnect.Name = "BtnDisconnect";
            this.BtnDisconnect.Size = new System.Drawing.Size(75, 23);
            this.BtnDisconnect.TabIndex = 32;
            this.BtnDisconnect.Text = "Disconnect";
            this.BtnDisconnect.UseVisualStyleBackColor = true;
            this.BtnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(111, 168);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 33;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Port :";
            // 
            // TxtPort
            // 
            this.TxtPort.Location = new System.Drawing.Point(92, 54);
            this.TxtPort.Multiline = true;
            this.TxtPort.Name = "TxtPort";
            this.TxtPort.Size = new System.Drawing.Size(61, 20);
            this.TxtPort.TabIndex = 35;
            this.TxtPort.Text = "5025";
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(609, 389);
            this.Controls.Add(this.TxtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnDisconnect);
            this.Controls.Add(this.LblName);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnImport);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.BtnRunTest);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LblCentFreq);
            this.Controls.Add(this.LblSpan);
            this.Controls.Add(this.BtnApply);
            this.Controls.Add(this.TxtSpan);
            this.Controls.Add(this.TxtCentFreq);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.LblPeakFreq);
            this.Controls.Add(this.LblPeakAmp);
            this.Controls.Add(this.LblStop);
            this.Controls.Add(this.LblStart);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.TxtResource);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.BtnConnect);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "GUI";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.TextBox TxtResource;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label LblStart;
        private System.Windows.Forms.Label LblStop;
        private System.Windows.Forms.Label LblPeakAmp;
        private System.Windows.Forms.Label LblPeakFreq;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.TextBox TxtCentFreq;
        private System.Windows.Forms.TextBox TxtSpan;
        private System.Windows.Forms.Button BtnApply;
        private System.Windows.Forms.Label LblSpan;
        private System.Windows.Forms.Label LblCentFreq;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BtnRunTest;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Freq;
        private System.Windows.Forms.DataGridViewTextBoxColumn Limit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Power;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.Label LblName;
        private System.Windows.Forms.Button BtnDisconnect;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtPort;
    }
}
