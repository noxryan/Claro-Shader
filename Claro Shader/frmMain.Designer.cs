namespace Claro_Shader
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.trkH = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trkS = new System.Windows.Forms.TrackBar();
            this.trkL = new System.Windows.Forms.TrackBar();
            this.chkLess = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnFolder = new System.Windows.Forms.Button();
            this.chkInvert = new System.Windows.Forms.CheckBox();
            this.chkBW = new System.Windows.Forms.CheckBox();
            this.chkGray = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numGrayTolerance = new System.Windows.Forms.NumericUpDown();
            this.numH = new System.Windows.Forms.NumericUpDown();
            this.numS = new System.Windows.Forms.NumericUpDown();
            this.numL = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.trkH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrayTolerance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numL)).BeginInit();
            this.SuspendLayout();
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(13, 14);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(548, 20);
            this.txtFolder.TabIndex = 1;
            this.txtFolder.Text = "Path to Claro...";
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Location = new System.Drawing.Point(12, 183);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(710, 73);
            this.txtLog.TabIndex = 4;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(648, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // trkH
            // 
            this.trkH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trkH.Cursor = System.Windows.Forms.Cursors.Default;
            this.trkH.Enabled = false;
            this.trkH.LargeChange = 10;
            this.trkH.Location = new System.Drawing.Point(422, 42);
            this.trkH.Maximum = 180;
            this.trkH.Minimum = -180;
            this.trkH.Name = "trkH";
            this.trkH.Size = new System.Drawing.Size(120, 45);
            this.trkH.TabIndex = 8;
            this.trkH.TickFrequency = 30;
            this.trkH.Scroll += new System.EventHandler(this.trkH_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(403, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "H";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(404, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "S";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(405, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "L";
            // 
            // trkS
            // 
            this.trkS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trkS.Cursor = System.Windows.Forms.Cursors.Default;
            this.trkS.Enabled = false;
            this.trkS.LargeChange = 10;
            this.trkS.Location = new System.Drawing.Point(423, 93);
            this.trkS.Maximum = 100;
            this.trkS.Minimum = -100;
            this.trkS.Name = "trkS";
            this.trkS.Size = new System.Drawing.Size(120, 45);
            this.trkS.TabIndex = 16;
            this.trkS.TickFrequency = 17;
            this.trkS.Scroll += new System.EventHandler(this.trkS_Scroll);
            // 
            // trkL
            // 
            this.trkL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trkL.Cursor = System.Windows.Forms.Cursors.Default;
            this.trkL.Enabled = false;
            this.trkL.LargeChange = 10;
            this.trkL.Location = new System.Drawing.Point(423, 144);
            this.trkL.Maximum = 100;
            this.trkL.Minimum = -100;
            this.trkL.Name = "trkL";
            this.trkL.Size = new System.Drawing.Size(120, 45);
            this.trkL.TabIndex = 17;
            this.trkL.TickFrequency = 17;
            this.trkL.Scroll += new System.EventHandler(this.trkL_Scroll);
            // 
            // chkLess
            // 
            this.chkLess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLess.AutoSize = true;
            this.chkLess.Location = new System.Drawing.Point(552, 45);
            this.chkLess.Name = "chkLess";
            this.chkLess.Size = new System.Drawing.Size(171, 17);
            this.chkLess.TabIndex = 18;
            this.chkLess.Text = "View/Edit .less before compile.";
            this.chkLess.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(13, 42);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(351, 138);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnFolder
            // 
            this.btnFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolder.Location = new System.Drawing.Point(567, 11);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(75, 23);
            this.btnFolder.TabIndex = 0;
            this.btnFolder.Text = "Browse...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // chkInvert
            // 
            this.chkInvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkInvert.AutoSize = true;
            this.chkInvert.Enabled = false;
            this.chkInvert.Location = new System.Drawing.Point(552, 65);
            this.chkInvert.Name = "chkInvert";
            this.chkInvert.Size = new System.Drawing.Size(85, 17);
            this.chkInvert.TabIndex = 19;
            this.chkInvert.Text = "Invert Colors";
            this.chkInvert.UseVisualStyleBackColor = true;
            this.chkInvert.CheckedChanged += new System.EventHandler(this.chkInvert_CheckedChanged);
            // 
            // chkBW
            // 
            this.chkBW.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkBW.AutoSize = true;
            this.chkBW.Checked = true;
            this.chkBW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBW.Enabled = false;
            this.chkBW.Location = new System.Drawing.Point(552, 85);
            this.chkBW.Name = "chkBW";
            this.chkBW.Size = new System.Drawing.Size(131, 17);
            this.chkBW.TabIndex = 20;
            this.chkBW.Text = "Keep Blacks && Whites";
            this.chkBW.UseVisualStyleBackColor = true;
            this.chkBW.CheckedChanged += new System.EventHandler(this.chkBW_CheckedChanged);
            // 
            // chkGray
            // 
            this.chkGray.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkGray.AutoSize = true;
            this.chkGray.Enabled = false;
            this.chkGray.Location = new System.Drawing.Point(552, 108);
            this.chkGray.Name = "chkGray";
            this.chkGray.Size = new System.Drawing.Size(81, 17);
            this.chkGray.TabIndex = 21;
            this.chkGray.Text = "Keep Grays";
            this.chkGray.UseVisualStyleBackColor = true;
            this.chkGray.CheckedChanged += new System.EventHandler(this.chkGray_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(597, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Tolerance";
            // 
            // numGrayTolerance
            // 
            this.numGrayTolerance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numGrayTolerance.Enabled = false;
            this.numGrayTolerance.Location = new System.Drawing.Point(552, 131);
            this.numGrayTolerance.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numGrayTolerance.Name = "numGrayTolerance";
            this.numGrayTolerance.Size = new System.Drawing.Size(44, 20);
            this.numGrayTolerance.TabIndex = 23;
            this.numGrayTolerance.ValueChanged += new System.EventHandler(this.numGrayTolerance_ValueChanged);
            // 
            // numH
            // 
            this.numH.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numH.Enabled = false;
            this.numH.Location = new System.Drawing.Point(374, 40);
            this.numH.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numH.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numH.Name = "numH";
            this.numH.Size = new System.Drawing.Size(43, 20);
            this.numH.TabIndex = 24;
            this.numH.ValueChanged += new System.EventHandler(this.numH_ValueChanged);
            // 
            // numS
            // 
            this.numS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numS.Enabled = false;
            this.numS.Location = new System.Drawing.Point(374, 93);
            this.numS.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numS.Name = "numS";
            this.numS.Size = new System.Drawing.Size(43, 20);
            this.numS.TabIndex = 25;
            this.numS.ValueChanged += new System.EventHandler(this.numS_ValueChanged);
            // 
            // numL
            // 
            this.numL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numL.Enabled = false;
            this.numL.Location = new System.Drawing.Point(373, 144);
            this.numL.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numL.Name = "numL";
            this.numL.Size = new System.Drawing.Size(44, 20);
            this.numL.TabIndex = 26;
            this.numL.ValueChanged += new System.EventHandler(this.numL_ValueChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 263);
            this.Controls.Add(this.numL);
            this.Controls.Add(this.numS);
            this.Controls.Add(this.numH);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.numGrayTolerance);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkGray);
            this.Controls.Add(this.chkBW);
            this.Controls.Add(this.chkInvert);
            this.Controls.Add(this.chkLess);
            this.Controls.Add(this.trkL);
            this.Controls.Add(this.trkS);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.trkH);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.btnFolder);
            this.MinimumSize = new System.Drawing.Size(751, 301);
            this.Name = "frmMain";
            this.Text = "Claro Shader";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trkH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGrayTolerance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog dirDialog;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TrackBar trkH;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trkS;
        private System.Windows.Forms.TrackBar trkL;
        private System.Windows.Forms.CheckBox chkLess;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.CheckBox chkInvert;
        private System.Windows.Forms.CheckBox chkBW;
        private System.Windows.Forms.CheckBox chkGray;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numGrayTolerance;
        private System.Windows.Forms.NumericUpDown numH;
        private System.Windows.Forms.NumericUpDown numS;
        private System.Windows.Forms.NumericUpDown numL;
    }
}

