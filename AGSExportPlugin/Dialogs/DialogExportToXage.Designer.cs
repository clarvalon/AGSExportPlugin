namespace CustomExportPlugin
{
    partial class DialogExportToXage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogExportToXage));
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbEmptyFolder = new System.Windows.Forms.TextBox();
            this.btnGetEmptyFolder = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSpecifyFolder = new System.Windows.Forms.CheckBox();
            this.lblPotrace = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUsePotrace = new System.Windows.Forms.Label();
            this.lblFfmpeg = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblUseFfmpeg = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(94, 281);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(382, 48);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Once ready, click \'OK\' to begin.";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(320, 332);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(401, 332);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(48, 281);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(40, 13);
            this.lblHeader.TabIndex = 3;
            this.lblHeader.Text = "Status:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(464, 39);
            this.label2.TabIndex = 4;
            this.label2.Text = "In order to convert your AGS game to the XAGE engine, you must first prepare your" +
    " files.  This is done automatically and may take a few minutes.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Empty Folder:";
            // 
            // tbEmptyFolder
            // 
            this.tbEmptyFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEmptyFolder.Location = new System.Drawing.Point(94, 73);
            this.tbEmptyFolder.Name = "tbEmptyFolder";
            this.tbEmptyFolder.Size = new System.Drawing.Size(347, 20);
            this.tbEmptyFolder.TabIndex = 6;
            this.tbEmptyFolder.TextChanged += new System.EventHandler(this.tbFolder_TextChanged);
            // 
            // btnGetEmptyFolder
            // 
            this.btnGetEmptyFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetEmptyFolder.Location = new System.Drawing.Point(447, 71);
            this.btnGetEmptyFolder.Name = "btnGetEmptyFolder";
            this.btnGetEmptyFolder.Size = new System.Drawing.Size(29, 23);
            this.btnGetEmptyFolder.TabIndex = 7;
            this.btnGetEmptyFolder.Text = "...";
            this.btnGetEmptyFolder.UseVisualStyleBackColor = true;
            this.btnGetEmptyFolder.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(277, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Do you wish to specify the location of the temporary files?";
            // 
            // chkSpecifyFolder
            // 
            this.chkSpecifyFolder.AutoSize = true;
            this.chkSpecifyFolder.Location = new System.Drawing.Point(295, 48);
            this.chkSpecifyFolder.Name = "chkSpecifyFolder";
            this.chkSpecifyFolder.Size = new System.Drawing.Size(15, 14);
            this.chkSpecifyFolder.TabIndex = 16;
            this.chkSpecifyFolder.UseVisualStyleBackColor = true;
            this.chkSpecifyFolder.CheckedChanged += new System.EventHandler(this.chkSpecifyFolder_CheckedChanged);
            // 
            // lblPotrace
            // 
            this.lblPotrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPotrace.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblPotrace.Location = new System.Drawing.Point(9, 104);
            this.lblPotrace.Name = "lblPotrace";
            this.lblPotrace.Size = new System.Drawing.Size(467, 64);
            this.lblPotrace.TabIndex = 24;
            this.lblPotrace.Text = resources.GetString("lblPotrace.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Potrace in place?";
            // 
            // lblUsePotrace
            // 
            this.lblUsePotrace.AutoSize = true;
            this.lblUsePotrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsePotrace.ForeColor = System.Drawing.Color.Red;
            this.lblUsePotrace.Location = new System.Drawing.Point(105, 168);
            this.lblUsePotrace.Name = "lblUsePotrace";
            this.lblUsePotrace.Size = new System.Drawing.Size(25, 13);
            this.lblUsePotrace.TabIndex = 26;
            this.lblUsePotrace.Text = "NO";
            // 
            // lblFfmpeg
            // 
            this.lblFfmpeg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFfmpeg.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.lblFfmpeg.Location = new System.Drawing.Point(9, 194);
            this.lblFfmpeg.Name = "lblFfmpeg";
            this.lblFfmpeg.Size = new System.Drawing.Size(467, 48);
            this.lblFfmpeg.TabIndex = 27;
            this.lblFfmpeg.Text = resources.GetString("lblFfmpeg.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 242);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "ffmpeg.exe in place?";
            // 
            // lblUseFfmpeg
            // 
            this.lblUseFfmpeg.AutoSize = true;
            this.lblUseFfmpeg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUseFfmpeg.ForeColor = System.Drawing.Color.Red;
            this.lblUseFfmpeg.Location = new System.Drawing.Point(123, 242);
            this.lblUseFfmpeg.Name = "lblUseFfmpeg";
            this.lblUseFfmpeg.Size = new System.Drawing.Size(25, 13);
            this.lblUseFfmpeg.TabIndex = 29;
            this.lblUseFfmpeg.Text = "NO";
            // 
            // DialogExportToXage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 367);
            this.ControlBox = false;
            this.Controls.Add(this.lblUseFfmpeg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblFfmpeg);
            this.Controls.Add(this.lblUsePotrace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPotrace);
            this.Controls.Add(this.chkSpecifyFolder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnGetEmptyFolder);
            this.Controls.Add(this.tbEmptyFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogExportToXage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prepare files for XAGE";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbEmptyFolder;
        private System.Windows.Forms.Button btnGetEmptyFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSpecifyFolder;
        private System.Windows.Forms.LinkLabel lblPotrace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsePotrace;
        private System.Windows.Forms.LinkLabel lblFfmpeg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblUseFfmpeg;
    }
}