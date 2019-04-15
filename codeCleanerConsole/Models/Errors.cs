using codeCleanerConsole.DAL;
using System;
using System.ComponentModel;

namespace codeCleanerConsole.Models
{
    public class Errors : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string _errorFSutilBehavior;
        private string _errorComparing;
        private string _errorGettingRepoInfo;
        private string _errorGettingCurrent;
        private string _errorGettingPrevious;
        private string _errorSaving;

        public Errors()
        {
            ErrorDetected = false;
        }

        public bool ErrorDetected { get; private set; }

        public string ErrorFSutilBehavior
        {
            get => _errorFSutilBehavior;
            set
            {
                if (value != this._errorFSutilBehavior)
                {
                    this._errorFSutilBehavior = value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
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
                    this._errorGettingCurrent = value;
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
                    this._errorGettingPrevious = value;
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
                    this._errorSaving = value;
                    NotifyErrorPropertiesChanged();
                }
            }
        }
        protected void NotifyErrorPropertiesChanged(
                                    [System.Runtime.CompilerServices.CallerMemberName] string memberName     = "",
                                    [System.Runtime.CompilerServices.CallerFilePath]   string sourceFilePath = "",
                                    [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber  = 0)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(memberName));
                this.ErrorDetected = true;
                RepositoryDB.SaveCodeCleanerLogDB(Program.logs);
                Environment.Exit(0);
            }
        }
    }
}
