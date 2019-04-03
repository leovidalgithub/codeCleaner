using System;
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
            var swTotal = Stopwatch.StartNew();
            List<Files> CurrentRunFiles  = new List<Files>();
            List<Files> PreviousRunFiles = new List<Files>();
            List<Files> ChangedFiles     = new List<Files>();
            logs.PropertyChanged += new PropertyChangedEventHandler(ErrorPropertiesChangedEventHandler);

            CurrentRunFiles  = ReadDirectory.GetCurrentFiles();

            PreviousRunFiles = RepositoryDB.GetPreviousFiles();

            ChangedFiles = Compare.CompareFiles(CurrentRunFiles, PreviousRunFiles);

            RepositoryDB.SaveCodeCleanerContentDB(ChangedFiles);

            Program.logs.ElapsedTimeTotal = swTotal.ElapsedMilliseconds;
            Program.logs.FinalStepOKay = true;
            RepositoryDB.SaveCodeCleanerLogDB(logs);

            //Console.ReadKey();
        }
        public static void ErrorPropertiesChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
        }
    }
}
/*
             //Console.WriteLine("COMPARING FILES - CompareFiles\n" +
                            "CurrentRunFiles.Count = {0:n0} files\n" +
                            "LastRunFiles.Count = {1:n0} files\n" +
                            "ChangedFiles.Count = {2:n0} files\n" +
                            "Time = {3:n0} ms", CurrentRunFiles.Count, PreviousRunFiles.Count, ChangedFiles.Count, sw_elapsed);
 */
