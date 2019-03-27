using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using codeCleaner.BLL;
using codeCleaner.DAL;
using System.Diagnostics;

namespace codeCleaner.GUI {
    public partial class codeCleanerForm : Form {
        public codeCleanerForm() {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Do you really want to Exit the App?", "Exit...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Utilities.CrossFade(this, null);
                Application.Exit();
            }
        }
        private void codeCleanerForm_Shown(object sender, EventArgs e){
            Utilities.CrossFade(null, this);
        }
        private void btnReadDirectory_Click(object sender, EventArgs e) {
            List<Files> CurrentRunFiles = new List<Files>();
            List<Files> LastRunFiles = new List<Files>();
            List<Files> ChangedFiles = new List<Files>();

            CurrentRunFiles = ReadDirectory.GetFilesInfo();
            LastRunFiles = RepositoryDB.GetCodeCleanerContentDB();

            var sw = Stopwatch.StartNew();
            ChangedFiles = Compare.CompareFiles2(CurrentRunFiles, LastRunFiles);
            var sw_elapsed = sw.ElapsedMilliseconds;

            RepositoryDB.SaveCodeCleanerContentDB(ChangedFiles);
            MessageBox.Show("COMPARING FILES - CompareFiles1" + Environment.NewLine +
                            string.Format("CurrentRunFiles.Count = {0:n0} files", CurrentRunFiles.Count) + Environment.NewLine +
                            string.Format("LastRunFiles.Count = {0:n0} files", LastRunFiles.Count) + Environment.NewLine +
                            string.Format("ChangedFiles.Count = {0:n0} files", ChangedFiles.Count) + Environment.NewLine +
                            string.Format("Time = {0:n0} ms", sw_elapsed), "CodeCleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void codeCleanerForm_Load(object sender, EventArgs e) {
            //var sw = Stopwatch.StartNew();
            //this.btnReadDirectory.PerformClick();
            //MessageBox.Show(string.Format("Processed {0:n0} files in {1:n0} ms", this.filesList.Count, sw.ElapsedMilliseconds),"CodeCleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Application.Exit();
        }
    }
}
