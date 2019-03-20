﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using codeCleaner.BLL;
using codeCleaner.DAL;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace codeCleaner.GUI {
    public partial class codeCleanerForm : Form {
        private ConcurrentBag<Files> filesList;
        private DataSet filesDS;
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
            var sw = Stopwatch.StartNew();

            this.filesList = new ConcurrentBag<Files>();
            //this.filesDS = new DataSet();

            this.filesList = ReadDirectory.GetFilesInfo();
            //this.filesDS = RepositoryDB.GetRepositoriesDS();

            //DataTable DT = Compare.CompareFiles(filesList, filesDS.Tables[RepositoryDB.repositoryName]);
            //RepositoryDB.SaveRpositoryDT(DT);
            MessageBox.Show(string.Format("*END: Processed {0:n0} files in {1:n0} ms", this.filesList.Count, sw.ElapsedMilliseconds),"CodeCleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void codeCleanerForm_Load(object sender, EventArgs e) {
            //var sw = Stopwatch.StartNew();
            //this.btnReadDirectory.PerformClick();
            //MessageBox.Show(string.Format("Processed {0:n0} files in {1:n0} ms", this.filesList.Count, sw.ElapsedMilliseconds),"CodeCleaner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //Application.Exit();
        }
    }
}
