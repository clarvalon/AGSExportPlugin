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
            tbMP3Folder.Enabled = false;
            tbWAVFolder.Enabled = false;
            tbOggFolder.Enabled = false;
            tbSpeechFolder.Enabled = false;
            btnGetMP3Folder.Enabled = false;
            btnGetWAVFolder.Enabled = false;
            btnGetOggFolder.Enabled = false;
            btnGetSpeechFolder.Enabled = false;
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
            PrepError = ComponentRef.DoPreparation(desiredEmptyFolder, generateFolderSuffix, tbMP3Folder.Text, tbWAVFolder.Text, tbSpeechFolder.Text, tbOggFolder.Text, this);
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
                if (conProj != null)
                {
                    tbMP3Folder.Text = conProj.PathAgsMusicFolder; 
                    tbWAVFolder.Text = conProj.PathAgsSoundsFolder; 
                    tbSpeechFolder.Text = conProj.PathAgsSpeechFolder;
                    tbOggFolder.Text = conProj.PathAgsOggMusicFolder;
                }
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

        private void btnGetMP3Folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Select the folder you have stored your mp3 music.";
            browser.SelectedPath = tbMP3Folder.Text;
            if (browser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tbMP3Folder.Text = browser.SelectedPath;
        }

        private void btnGetWAVFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Select the folder you have stored your wav sounds.";
            browser.SelectedPath = tbWAVFolder.Text;
            if (browser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tbWAVFolder.Text = browser.SelectedPath;
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

        private void btnGetSpeechFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Select the folder you have stored your speech sounds.";
            browser.SelectedPath = tbSpeechFolder.Text;
            if (browser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tbSpeechFolder.Text = browser.SelectedPath;
        }

        private void btnGetOggFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog browser = new FolderBrowserDialog();
            browser.Description = "Select the folder you have stored your ogg music.";
            browser.SelectedPath = tbOggFolder.Text;
            if (browser.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            tbOggFolder.Text = browser.SelectedPath;
        }
    }
}
