using System.Collections.Generic;
using codeCleanerConsole.Models;
using codeCleanerConsole.BLL;
using codeCleanerConsole.DAL;
using System.Diagnostics;
using System.ComponentModel;

namespace codeCleanerConsole {
    class Program {
        public static Logs logs = new Logs();

        static void Main(string[] args) {
            var swOverall                = Stopwatch.StartNew();
            List<Files> CurrentRunFiles  = new List<Files>();
            List<Files> PreviousRunFiles = new List<Files>();
            List<Files> ChangedFiles     = new List<Files>();
            logs.PropertyChanged        += new PropertyChangedEventHandler(LogErrorPropertiesChangedEventHandler);

            FSutil.FSutilBehaviorCheck();
            CurrentRunFiles  = ReadDirectory.GetCurrentFiles();
            PreviousRunFiles = RepositoryDB.GetPreviousFiles();
            ChangedFiles     = Compare.CompareFiles(CurrentRunFiles, PreviousRunFiles);
            RepositoryDB.SaveCodeCleanerContentDB(ChangedFiles);

            Program.logs.ElapsedTimeOverall = swOverall.ElapsedMilliseconds;
            Program.logs.FinalStepOKay      = true;
            RepositoryDB.SaveCodeCleanerLogDB(logs);

            return;
        }
        public static void LogErrorPropertiesChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
