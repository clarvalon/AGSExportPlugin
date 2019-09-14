#region License
/* AGSExportPlugin 
 * Copyright 2010-2018 - Dan Alexander
 *
 * Released under the MIT License.  See LICENSE for details. */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Clarvalon.XAGE.Global;
using AGSExportPlugin;
using System.Diagnostics;

namespace CustomExportPlugin
{
    public partial class DialogExportToXage : Form
    {
        public CustomExportEditorComponent ComponentRef;

        private bool PrepError = true;
        private FileInfo SettingFile;

        public DialogExportToXage(CustomExportEditorComponent sloppyRef)
        {
            this.ComponentRef = sloppyRef;

            string EN = Environment.NewLine;
            InitializeComponent();
            CheckEnableOK();
            CheckChecked();
            lblHeader.Visible = false;
            string pathAgsExe = System.Reflection.Assembly.GetEntryAssembly().Location;
            pathAgsExe = Directory.GetParent(pathAgsExe).FullName;

            // Update Potrace & ffmpeg available labels
            FileInfo poTraceFile = new FileInfo(Path.Combine(pathAgsExe, "potrace.exe"));
            FileInfo mkBitmapFile = new FileInfo(Path.Combine(pathAgsExe, "mkbitmap.exe"));
            FileInfo ffmpegFile = new FileInfo(Path.Combine(pathAgsExe, "ffmpeg.exe"));
            if (poTraceFile.Exists && mkBitmapFile.Exists)
            {
                lblUsePotrace.Text = "YES";
                lblUsePotrace.ForeColor = Color.Green;
            }
            if (ffmpegFile.Exists)
            {
                lblUseFfmpeg.Text = "YES";
                lblUseFfmpeg.ForeColor = Color.Green;
            }

            // Update Potrace linklabel
            LinkLabel.Link link = new LinkLabel.Link();
            string linkText = "SourceForge";
            string linkDestination = @"http://potrace.sourceforge.net/#downloading";
            link.LinkData = linkDestination;
            link.Start = lblPotrace.Text.IndexOf(linkText);
            link.Length = linkText.Length;
            lblPotrace.Links.Add(link);
            lblPotrace.LinkClicked += LblPotrace_LinkClicked;

            // Update Potrace linklabel
            LinkLabel.Link link2 = new LinkLabel.Link();
            linkText = "here";
            linkDestination = @"https://www.ffmpeg.org/download.html#build-windows";
            link2.LinkData = linkDestination;
            link2.Start = lblFfmpeg.Text.IndexOf(linkText);
            link2.Length = linkText.Length;
            lblFfmpeg.Links.Add(link2);
            lblFfmpeg.LinkClicked += LblFfmpeg_LinkClicked;

            // Attempt to load previous path using game name:
            string gameName = sloppyRef.editor.CurrentGame.Settings.GameName.Replace(":", "");
            string appDir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "XAGE");
            Directory.CreateDirectory(appDir);
            string name = "ExportSettings.xml";
            SettingFile = new FileInfo(Path.Combine(appDir, name));
            sloppyRef.PopDebugMessage(SettingFile.FullName);
            if (SettingFile.Exists)
            {
                sloppyRef.PopDebugMessage("6");
                TextReader fs = SettingFile.OpenText();
                string lastPath = fs.ReadToEnd();
                lastPath = lastPath.Replace(Environment.NewLine, "");
                if (!lastPath.EndsWith(@"\"))
                    lastPath += @"\";
                fs.Close();
                sloppyRef.PopDebugMessage(lastPath);

                chkSpecifyFolder.Checked = true;
                tbEmptyFolder.Text = lastPath;
                sloppyRef.PopDebugMessage("7");
            }
        }

        private void LblPotrace_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = e.Link;
            ProcessStartInfo sInfo = new ProcessStartInfo((string)link.LinkData);
            Process.Start(sInfo);
        }

        private void LblFfmpeg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var link = e.Link;
            ProcessStartInfo sInfo = new ProcessStartInfo((string)link.LinkData);
            Process.Start(sInfo);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool ok = false;
            if (chkSpecifyFolder.Checked)
            {
                if (!Directory.Exists(tbEmptyFolder.Text))
                    MessageBox.Show("Directory doesn't exist.");
                else
                    ok = true;
            }
            else
                ok = true;
            if (ok)
                BeginPreparation();
        }

        private void BeginPreparation()
        {
            UpdateStatus(string.Empty);
            lblHeader.Visible = true;
            lblStatus.Visible = true;
            tbEmptyFolder.Enabled = false;
            btnGetEmptyFolder.Enabled = false;
            btnOK.Enabled = false;
            this.Update();
            this.Refresh();

            // Generate temp folder if required
            string desiredEmptyFolder = tbEmptyFolder.Text;
            bool generateFolderSuffix = false;
            if (!chkSpecifyFolder.Checked)
            {
                desiredEmptyFolder = Path.GetTempPath();
                if (!desiredEmptyFolder.EndsWith(@"\"))
                    desiredEmptyFolder = desiredEmptyFolder + @"\";
                generateFolderSuffix = true;
            }
            else
            {
                TextWriter tw = SettingFile.CreateText();
                tw.WriteLine(desiredEmptyFolder);
                tw.Close();
            }

            // Start preparation
            try
            {
                PrepError = ComponentRef.DoPreparation(desiredEmptyFolder, generateFolderSuffix, this);
                if (PrepError == false)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Error encountered!");
                    DialogResult = DialogResult.Cancel;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unhandled exception: " + e.Message);
                MessageBox.Show("Stack Trace: " + e.StackTrace);
                DialogResult = DialogResult.Cancel;
            }
        }

        public void UpdateStatus(string status)
        {
            lblStatus.Text = status;
            this.Update();
            this.Refresh();
        }

        private void tbFolder_TextChanged(object sender, EventArgs e)
        {
            CheckEnableOK();

            // Also:  See if a valid conversion file already exists here
            // - if so, populate audio folder textboxes
            ComponentRef.PopDebugMessage("--" + tbEmptyFolder.Text + "--");
            DirectoryInfo di = new DirectoryInfo(tbEmptyFolder.Text);
            if (di == null || !di.Exists)
                return;
            FileInfo[] files = di.GetFiles("*.a2x");
            if (files.Length == 1)
            {
                string folderName = tbEmptyFolder.Text;
                string projFileName = files[0].Name;
                MessageBox.Show("Existing conversion project found: " + projFileName);

                projFileName = Path.Combine(folderName, projFileName);
                AgsConversionProject conProj = GenericXmlSerializer.DeSerializeFromFile<AgsConversionProject>(projFileName);
            }
        }

        private void CheckEnableOK()
        {
            bool ok = false;
            if (chkSpecifyFolder.Checked)
            {
                if (Directory.Exists(tbEmptyFolder.Text))
                    ok = true;
            }
            else
                ok = true;

            if (ok)
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Please choose an empty folder to export the game data";
            browser.SelectedPath = tbEmptyFolder.Text;
            if (browser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tbEmptyFolder.Text = browser.SelectedPath;
        }
        
        private void chkSpecifyFolder_CheckedChanged(object sender, EventArgs e)
        {
            CheckChecked();
            CheckEnableOK();
        }

        private void CheckChecked()
        {
            if (chkSpecifyFolder.Checked)
            {
                tbEmptyFolder.Enabled = true;
                btnGetEmptyFolder.Enabled = true;
            }
            else
            {
                tbEmptyFolder.Enabled = false;
                btnGetEmptyFolder.Enabled = false;
            }
        }
    }
}
