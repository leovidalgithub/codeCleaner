using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using codeCleanerConsole.BLL;
using codeCleanerConsole.DAL;
using codeCleanerConsole.Models;
using System.Diagnostics;

namespace codeCleanerForm.GUI {
    public class myClass
    {
        public int myvar = 99;
    }

    public partial class codeCleanerWinForm : Form {
        public codeCleanerWinForm() {
            InitializeComponent();
        }
        private void btnExit_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Do you really want to Exit the App?", "Exit...", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
                Application.Exit();
            }
        }
        private void codeCleanerForm_Shown(object sender, EventArgs e){
            codeCleaner.BLL.Utilities.CrossFade(null, this);
        }
        private void btnReadDirectory_Click(object sender, EventArgs e) {
            List<Files> CurrentRunFiles = new List<Files>();
            List<Files> LastRunFiles = new List<Files>();
            List<Files> ChangedFiles = new List<Files>();

            CurrentRunFiles = ReadDirectory.GetCurrentFiles();
            LastRunFiles = RepositoryDB.GetPreviousFiles();

            var sw = Stopwatch.StartNew();
            ChangedFiles = Compare.CompareFiles(CurrentRunFiles, LastRunFiles);
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

        private void codeCleanerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            codeCleaner.BLL.Utilities.CrossFade(this, null);
        }
    }
}
