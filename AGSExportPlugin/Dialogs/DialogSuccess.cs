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
using System.Windows.Forms;
using System.Diagnostics;

namespace CustomExportPlugin
{
    public partial class DialogSuccess : Form
    {
        private string _a2xFile;

        public DialogSuccess()
        {
            InitializeComponent();
        }

        public void SetA2XFile(string a2x)
        {
            _a2xFile = a2x;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Launch a2x file to load XAGE Editor
            // This only works if you've started up XAGE Editor and it has associated *.a2x files with itself
            Process.Start(_a2xFile);
            DialogResult = DialogResult.OK;
        }
    }
}
