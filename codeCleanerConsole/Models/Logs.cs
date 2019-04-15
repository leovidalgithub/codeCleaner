using System;
using System.Data;
using System.Linq;
using codeCleanerConsole.Helpers;

namespace codeCleanerConsole.Models
{
    public class Logs : Errors
    {
        public DateTime DateOfTheRun   { get; private set; }

        public string ThisMachineName  { get; private set; }
        public string SearchRootFolder { get; private set; }
        public string RepositoryName   { get; private set; }

        public int FilesCountCurrent  { get; set; }
        public int FilesCountPrevious { get; set; }
        public int FilesCountNew      { get; set; }
        public int FilesCountChanged  { get; set; }
        public int FilesCountMissing  { get; set; }

        public long ElapsedTimeCurrent  { get; set; }
        public long ElapsedTimePrevious { get; set; }
        public long ElapsedTimeCompare  { get; set; }
        public long ElapsedTimeSaving   { get; set; }
        public long ElapsedTimeOverall  { get; set; }

        public bool FinalStepOKay { get; set; }

        public Logs() : base()
        {
            this.DateOfTheRun    = DateTime.Now;
            this.ThisMachineName = Environment.MachineName;

            this.SearchRootFolder = String.Empty;
            this.RepositoryName   = String.Empty;

            this.FilesCountCurrent  = 0;
            this.FilesCountPrevious = 0;
            this.FilesCountNew      = 0;
            this.FilesCountChanged  = 0;
            this.FilesCountMissing  = 0;

            this.ElapsedTimeCurrent  = 0;
            this.ElapsedTimePrevious = 0;
            this.ElapsedTimeCompare  = 0;
            this.ElapsedTimeSaving   = 0;
            this.ElapsedTimeOverall  = 0;

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
