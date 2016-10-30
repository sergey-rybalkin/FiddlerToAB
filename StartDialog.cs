using System;
using System.IO;
using System.Windows.Forms;
using Fiddler;

namespace FiddlerToWcat
{
    public partial class StartDialog : Form
    {
        public StartDialog()
        {
            InitializeComponent();
        }

        public string SelectedPath { get; private set; }

        public int Clients { get; private set; }

        public int DurationSeconds { get; private set; }

        public bool SkipRun { get; private set; }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (DialogResult.OK == dlg.ShowDialog())
                txtDirectory.Text = dlg.SelectedPath;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string directory = txtDirectory.Text;
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                FiddlerApplication.AlertUser("FiddlerToWCAT", "Specified directory does not exist.");
                return;
            }

            SelectedPath = directory;
            Clients = (int)txtClients.Value;
            DurationSeconds = (int)txtDuration.Value;
            SkipRun = chkCreateOnly.Checked;

            Close();
        }
    }
}