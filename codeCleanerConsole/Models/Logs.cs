using System;
using System.Data;
using System.Linq;
using codeCleanerConsole.Helpers;
using codeCleanerConsole.DAL;
using System.ComponentModel;

namespace codeCleanerConsole.Models
{
    public class Logs : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate {};

        private string _errorComparing;
        private string _errorGettingRepoInfo;
        private string _errorGettingCurrent;
        private string _errorGettingPrevious;
        private string _errorSaving;

        public DateTime DateOfTheRun { get; private set; }

        public string ThisMachineName { get; private set; }
        public string SearchRootFolder { get; private set; }
        public string RepositoryName { get; private set; }

        //public int CurrentRunFiles { get; set; }
        //public int PreviousRunFiles { get; set; }
        //public int DetectedNewFiles { get; set; }
        //public int DetectedChangedFiles { get; set; }
        //public int DetectedMissingFiles { get; set; }

        public int FilesCountCurrent { get; set; }
        public int FilesCountPrevious { get; set; }
        public int FilesCountNew { get; set; }
        public int FilesCountChanged { get; set; }
        public int FilesCountMissing { get; set; }

        public long ElapsedTimeCurrent { get; set; }
        public long ElapsedTimePrevious { get; set; }
        public long ElapsedTimeCompare { get; set; }
        public long ElapsedTimeSaving { get; set; }
        public long ElapsedTimeTotal { get; set; }

        public bool ErrorDetected { get; private set; }
        public string ErrorGettingRepoInfo
        {
            get => _errorGettingRepoInfo;
            set
            {
                if (value != this._errorGettingRepoInfo)
                {
                    this._errorGettingRepoInfo = value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
        public string ErrorGettingCurrent
        {
            get => _errorGettingCurrent;
            set
            {
                if (value != this._errorGettingCurrent)
                {
                    this._errorGettingCurrent= value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
        public string ErrorGettingPrevious
        {
            get => _errorGettingPrevious;
            set
            {
                if (value != this._errorGettingPrevious)
                {
                    this._errorGettingPrevious= value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
        public string ErrorComparing
        {
            get => _errorComparing;
            set
            {
                if (value != this._errorComparing)
                {
                    this._errorComparing = value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
        public string ErrorSaving
        {
            get => _errorSaving;
            set
            {
                if (value != this._errorSaving)
                {
                    this._errorSaving= value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }

        public bool FinalStepOKay { get; set; }

        protected void NotifyErrorPropertiesChanged(
                                            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                                            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
                                            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(memberName));
                this.ErrorDetected = true;
                RepositoryDB.SaveCodeCleanerLogDB(this);
                Environment.Exit(0);
            }
        }
        public Logs()
        {
            this.DateOfTheRun    = DateTime.Now;
            this.ThisMachineName = Environment.MachineName;

            this.SearchRootFolder = String.Empty;
            this.RepositoryName   = String.Empty;

            this.FilesCountCurrent  = 0;
            this.FilesCountPrevious = 0;
            this.FilesCountNew = 0;
            this.FilesCountChanged = 0;
            this.FilesCountMissing = 0;

            this.ElapsedTimeCurrent  = 0;
            this.ElapsedTimePrevious = 0;
            this.ElapsedTimeCompare  = 0;
            this.ElapsedTimeSaving   = 0;
            this.ElapsedTimeTotal    = 0;

            this.ErrorDetected = false;

            this.FinalStepOKay = false;
        }
        /// <summary>
        /// Storing Logs.SearchRootFolders & Logs.RepositoryNames Info
        /// </summary>
        /// <param name="repositoryInfoDataTable"></param>
        public void LogSetRepositoryInfo(DataTable repositoryInfoDataTable)
        {
            DataTable repositoryNamesOnlyDataTable = repositoryInfoDataTable.AsEnumerable()
                                                .GroupBy(r => r.Field<string>("RepositoryName"))
                                                .Select(g => g.First()).CopyToDataTable();
            ClassesHelpers.AddContentWithSeparator(this, "SearchRootFolder", repositoryInfoDataTable);
            ClassesHelpers.AddContentWithSeparator(this, "RepositoryName", repositoryNamesOnlyDataTable);
        }
    }
}
