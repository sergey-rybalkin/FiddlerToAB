namespace FiddlerToWcat
{
    partial class StartDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblClients = new System.Windows.Forms.Label();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.txtClients = new System.Windows.Forms.NumericUpDown();
            this.txtDuration = new System.Windows.Forms.NumericUpDown();
            this.lblDuration = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkCreateOnly = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(312, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStart.Location = new System.Drawing.Point(231, 130);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(13, 47);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(78, 13);
            this.lblClients.TabIndex = 2;
            this.lblClients.Text = "Parallel Clients:";
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(13, 16);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(86, 13);
            this.lblDirectory.TabIndex = 3;
            this.lblDirectory.Text = "Target Directory:";
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(107, 13);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.Size = new System.Drawing.Size(244, 20);
            this.txtDirectory.TabIndex = 4;
            // 
            // txtClients
            // 
            this.txtClients.Location = new System.Drawing.Point(107, 43);
            this.txtClients.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.txtClients.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtClients.Name = "txtClients";
            this.txtClients.Size = new System.Drawing.Size(50, 20);
            this.txtClients.TabIndex = 6;
            this.txtClients.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // txtDuration
            // 
            this.txtDuration.Location = new System.Drawing.Point(107, 75);
            this.txtDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtDuration.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtDuration.Name = "txtDuration";
            this.txtDuration.Size = new System.Drawing.Size(50, 20);
            this.txtDuration.TabIndex = 8;
            this.txtDuration.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(13, 79);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(76, 13);
            this.lblDuration.TabIndex = 7;
            this.lblDuration.Text = "Duration (sec):";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(356, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(31, 23);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkCreateOnly
            // 
            this.chkCreateOnly.AutoSize = true;
            this.chkCreateOnly.Location = new System.Drawing.Point(16, 104);
            this.chkCreateOnly.Name = "chkCreateOnly";
            this.chkCreateOnly.Size = new System.Drawing.Size(210, 17);
            this.chkCreateOnly.TabIndex = 10;
            this.chkCreateOnly.Text = "Do not start session, create scripts only";
            this.chkCreateOnly.UseVisualStyleBackColor = true;
            // 
            // StartDialog
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(399, 165);
            this.Controls.Add(this.chkCreateOnly);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.txtClients);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.lblClients);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Run WCAT Session";
            ((System.ComponentModel.ISupportInitialize)(this.txtClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.NumericUpDown txtClients;
        private System.Windows.Forms.NumericUpDown txtDuration;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkCreateOnly;
    }
}