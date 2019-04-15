using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using codeCleanerConsole.Models;

namespace codeCleanerConsole.BLL
{
    public static class Compare
    {
        public static List<Files> CompareFiles(List<Files> CurrentRunFiles, List<Files> PreviousRunFiles)
        {
            var swCompare = Stopwatch.StartNew();
            List<Files> ChangedFiles = new List<Files>();
            int newFilesCount = 0;

            try
            {
                Dictionary<string, Files> previousFiles = PreviousRunFiles.ToDictionary(file => file.Path, file => file);
                List<Files> previousMissingFiles = PreviousRunFiles.Except(CurrentRunFiles)
                                                    .Where(file => file.Active == true)
                                                    .Select(file =>
                                                    {
                                                        file.Active = false;
                                                        file.Changes = 0;
                                                        return file;
                                                    }).ToList();

                foreach (Files currentFile in CurrentRunFiles)
                {
                    if (previousFiles.ContainsKey(currentFile.Path))
                    {
                        Files previousFile = previousFiles[currentFile.Path];
                        if (!previousFile.Active || CheckForDateChanges(previousFile, currentFile))
                        {
                            if (previousFile.Active) currentFile.Changes = ++previousFile.Changes;
                            ChangedFiles.Add(currentFile);
                        }
                    }
                    else
                    {
                        ChangedFiles.Add(currentFile);
                        newFilesCount++;
                    }
                }
                ChangedFiles.AddRange(previousMissingFiles);
                Program.logs.FilesCountNew     += newFilesCount;
                Program.logs.FilesCountChanged += ChangedFiles.Count;
                Program.logs.FilesCountMissing += previousMissingFiles.Count;
            }
            catch (Exception ex)
            {
                Program.logs.ErrorComparing += "#001 - " + ex.GetType().Name + " = " + ex.Message;
            }
            Program.logs.ElapsedTimeCompare = swCompare.ElapsedMilliseconds;
            return ChangedFiles;
        }
        private static bool CheckForDateChanges(Files FirstFile, Files SecondFile)
        {
            return DateTime.Compare(FirstFile.Accessed, SecondFile.Accessed) != 0 ||
                   DateTime.Compare(FirstFile.Modified, SecondFile.Modified) != 0;
        }
    }
}

/*
         /* Compare Approachs #1 #2 #3 (test. 6435 files)    => SCAN1-EMPTY  => SCAN2   => SCAN3     => SCAN4
         * Approach #1                                             11ms        3ms         4ms         4ms
         * Approach #2 Find()                                      3ms        258ms       219ms       240ms
         * Approach #3 Dictionary<>                                4ms         7ms         6ms         4ms

    public static List<Files> CompareFiles2(List<Files> CurrentRunFiles, List<Files> PreviousRunFiles)
    {
        List<Files> ChangedFiles = new List<Files>();
        List<Files> commonFiles = CurrentRunFiles.Intersect(PreviousRunFiles).ToList();
        List<Files> newFiles = CurrentRunFiles.Except(PreviousRunFiles).ToList();
        //Dictionary<string, Files> previousFiles = PreviousRunFiles.ToDictionary(file => file.Path, file => file); // Approach #3
        List<Files> previousMissingFiles = PreviousRunFiles.Except(CurrentRunFiles)
                                            .Where(file => file.Active == true)
                                            .Select(file =>
                                            {
                                                file.Active = false;
                                                file.Changes = 0;
                                                return file;
                                            }).ToList();
        foreach (Files commonFile in commonFiles)
        {
            //Files previousFile = previousFiles[commonFile.Path]; // Approach #3
            Files previousFile = PreviousRunFiles.Find(f => f.Path == commonFile.Path); // Approach #2
            if (!previousFile.Active || CheckForDateChanges(commonFile, previousFile))
            {
                if (previousFile.Active) commonFile.Changes = ++previousFile.Changes;
                ChangedFiles.Add(commonFile);
            }
        }
        ChangedFiles.AddRange(newFiles);
        ChangedFiles.AddRange(previousMissingFiles);
        return ChangedFiles;
    }
     */

//List<Files> l1 = new List<Files>();
//List<Files> l2 = new List<Files>();
//l1.AddRange(new[] {
//    new Files(1, "a", 1),
//    new Files(1, "b", 2),
//    new Files(1, "c", 3),
//    new Files(1, "d", 4),
//    new Files(1, "e", 5)
//});
//l2.AddRange(new[] {
//    new Files(1, "a", 6),
//    new Files(1, "b", 7),
//    new Files(1, "f", 8),
//    new Files(1, "g", 9),
//    new Files(1, "e", 10)
//});
////var testz = oldfilez.Where(x => newfilez.ContainsKey(x.Key)).Select(x => x.Value).ToList();
////var testz2 = newfilez.Where(x => !oldfilez.ContainsKey(x.Key)).Select(x => x.Value).ToList();
//var dic7 = l1.Intersect(l2).ToList();
//var dic17 = l2.Except(l1).ToList();
//var dic2 = dic7.Zip(dic17, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v);
//string result = string.Empty;
//foreach (var item in dic7) 
//    result += item.Path + " " + item.Size + Environment.NewLine;
//result += " -------------------------------- " + Environment.NewLine;
//foreach (var item in dic17)
//    result += item.Path + " " + item.Size + Environment.NewLine;
//System.Windows.Forms.MessageBox.Show(result);