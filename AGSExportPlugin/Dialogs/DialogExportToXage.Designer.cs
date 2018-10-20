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
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetMP3Folder = new System.Windows.Forms.Button();
            this.tbMP3Folder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGetWAVFolder = new System.Windows.Forms.Button();
            this.tbWAVFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chkSpecifyFolder = new System.Windows.Forms.CheckBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.btnGetSpeechFolder = new System.Windows.Forms.Button();
            this.tbSpeechFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnGetOggFolder = new System.Windows.Forms.Button();
            this.tbOggFolder = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.lblPotrace = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.Location = new System.Drawing.Point(94, 403);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(382, 48);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Once ready, click \'OK\' to begin.";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(320, 454);
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
            this.btnCancel.Location = new System.Drawing.Point(401, 454);
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
            this.lblHeader.Location = new System.Drawing.Point(48, 403);
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
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(9, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(467, 64);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // btnGetMP3Folder
            // 
            this.btnGetMP3Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetMP3Folder.Location = new System.Drawing.Point(447, 233);
            this.btnGetMP3Folder.Name = "btnGetMP3Folder";
            this.btnGetMP3Folder.Size = new System.Drawing.Size(29, 23);
            this.btnGetMP3Folder.TabIndex = 11;
            this.btnGetMP3Folder.Text = "...";
            this.btnGetMP3Folder.UseVisualStyleBackColor = true;
            this.btnGetMP3Folder.Click += new System.EventHandler(this.btnGetMP3Folder_Click);
            // 
            // tbMP3Folder
            // 
            this.tbMP3Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMP3Folder.Location = new System.Drawing.Point(135, 235);
            this.tbMP3Folder.Name = "tbMP3Folder";
            this.tbMP3Folder.Size = new System.Drawing.Size(306, 20);
            this.tbMP3Folder.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 238);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Music Folder (MP3):";
            // 
            // btnGetWAVFolder
            // 
            this.btnGetWAVFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetWAVFolder.Location = new System.Drawing.Point(447, 285);
            this.btnGetWAVFolder.Name = "btnGetWAVFolder";
            this.btnGetWAVFolder.Size = new System.Drawing.Size(29, 23);
            this.btnGetWAVFolder.TabIndex = 14;
            this.btnGetWAVFolder.Text = "...";
            this.btnGetWAVFolder.UseVisualStyleBackColor = true;
            this.btnGetWAVFolder.Click += new System.EventHandler(this.btnGetWAVFolder_Click);
            // 
            // tbWAVFolder
            // 
            this.tbWAVFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWAVFolder.Location = new System.Drawing.Point(135, 287);
            this.tbWAVFolder.Name = "tbWAVFolder";
            this.tbWAVFolder.Size = new System.Drawing.Size(306, 20);
            this.tbWAVFolder.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 290);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Sounds Folder (WAV):";
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
            // lblNotes
            // 
            this.lblNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNotes.Location = new System.Drawing.Point(12, 364);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(464, 39);
            this.lblNotes.TabIndex = 17;
            this.lblNotes.Text = "Notes:";
            this.lblNotes.Visible = false;
            // 
            // btnGetSpeechFolder
            // 
            this.btnGetSpeechFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetSpeechFolder.Location = new System.Drawing.Point(447, 311);
            this.btnGetSpeechFolder.Name = "btnGetSpeechFolder";
            this.btnGetSpeechFolder.Size = new System.Drawing.Size(29, 23);
            this.btnGetSpeechFolder.TabIndex = 20;
            this.btnGetSpeechFolder.Text = "...";
            this.btnGetSpeechFolder.UseVisualStyleBackColor = true;
            this.btnGetSpeechFolder.Click += new System.EventHandler(this.btnGetSpeechFolder_Click);
            // 
            // tbSpeechFolder
            // 
            this.tbSpeechFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSpeechFolder.Location = new System.Drawing.Point(135, 313);
            this.tbSpeechFolder.Name = "tbSpeechFolder";
            this.tbSpeechFolder.Size = new System.Drawing.Size(306, 20);
            this.tbSpeechFolder.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 316);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Speech Folder (MP3):";
            // 
            // btnGetOggFolder
            // 
            this.btnGetOggFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetOggFolder.Location = new System.Drawing.Point(447, 259);
            this.btnGetOggFolder.Name = "btnGetOggFolder";
            this.btnGetOggFolder.Size = new System.Drawing.Size(29, 23);
            this.btnGetOggFolder.TabIndex = 23;
            this.btnGetOggFolder.Text = "...";
            this.btnGetOggFolder.UseVisualStyleBackColor = true;
            this.btnGetOggFolder.Click += new System.EventHandler(this.btnGetOggFolder_Click);
            // 
            // tbOggFolder
            // 
            this.tbOggFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOggFolder.Location = new System.Drawing.Point(135, 261);
            this.tbOggFolder.Name = "tbOggFolder";
            this.tbOggFolder.Size = new System.Drawing.Size(306, 20);
            this.tbOggFolder.TabIndex = 22;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 264);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 13);
            this.label8.TabIndex = 21;
            this.label8.Text = "Music Folder (OGG):";
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
            // DialogExportToXage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 489);
            this.ControlBox = false;
            this.Controls.Add(this.lblPotrace);
            this.Controls.Add(this.btnGetOggFolder);
            this.Controls.Add(this.tbOggFolder);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnGetSpeechFolder);
            this.Controls.Add(this.tbSpeechFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.chkSpecifyFolder);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnGetWAVFolder);
            this.Controls.Add(this.tbWAVFolder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnGetMP3Folder);
            this.Controls.Add(this.tbMP3Folder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnGetMP3Folder;
        private System.Windows.Forms.TextBox tbMP3Folder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnGetWAVFolder;
        private System.Windows.Forms.TextBox tbWAVFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSpecifyFolder;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Button btnGetSpeechFolder;
        private System.Windows.Forms.TextBox tbSpeechFolder;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnGetOggFolder;
        private System.Windows.Forms.TextBox tbOggFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel lblPotrace;
    }
}