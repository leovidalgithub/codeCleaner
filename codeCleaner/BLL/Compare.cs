using System;
using System.Collections.Generic;
using System.Linq;

namespace codeCleaner.BLL {

    public static class Compare {
        public static List<Files> CompareFiles1(List<Files> CurrentRunFiles, List<Files> PreviousRunFiles) {
            List<Files> ChangedFiles = new List<Files>();
            Dictionary<string, Files> previousFiles = PreviousRunFiles.ToDictionary(file => file.Path, file => file);
            List<Files> previousMissingFiles = PreviousRunFiles.Except(CurrentRunFiles)
                                                .Where(file => file.Active == true)
                                                .Select(file => { file.Active = false;
                                                                  file.Changes = 0;
                                                                  return file; }).ToList();

            foreach (Files currentFile in CurrentRunFiles) {
                if (previousFiles.ContainsKey(currentFile.Path)) {
                    Files previousFile = previousFiles[currentFile.Path];
                    if (!previousFile.Active || CheckForDateChanges(previousFile, currentFile)) {
                        if (previousFile.Active) currentFile.Changes = ++previousFile.Changes;
                        ChangedFiles.Add(currentFile);
                    }
                } else { 
                    ChangedFiles.Add(currentFile);
                }
            }
            ChangedFiles.AddRange(previousMissingFiles);
            return ChangedFiles;
        }
        public static List<Files> CompareFiles2(List<Files> CurrentRunFiles, List<Files> PreviousRunFiles) {
            List<Files> ChangedFiles = new List<Files>();
            List<Files> commonFiles = CurrentRunFiles.Intersect(PreviousRunFiles).ToList();
            List<Files> newFiles = CurrentRunFiles.Except(PreviousRunFiles).ToList();
            Dictionary<string, Files> previousFiles = PreviousRunFiles.ToDictionary(file => file.Path, file => file);
            List<Files> previousMissingFiles = PreviousRunFiles.Except(CurrentRunFiles)
                                                .Where(file => file.Active == true)
                                                .Select(file => {
                                                    file.Active = false;
                                                    file.Changes = 0;
                                                    return file;
                                                }).ToList();

            foreach (Files commonFile in commonFiles) {
                Files previousFile = previousFiles[commonFile.Path];
                //Files LastRunFile = PreviousRunFiles.Find(previousFile => previousFile.Path == commonFile.Path);
                if (!previousFile.Active || CheckForDateChanges(commonFile, previousFile)) {
                    if (previousFile.Active) commonFile.Changes = ++previousFile.Changes;
                    ChangedFiles.Add(commonFile);
                }
            }
            ChangedFiles.AddRange(newFiles);
            ChangedFiles.AddRange(previousMissingFiles);
            return ChangedFiles;
        }

        private static bool CheckForDateChanges(Files FirstFile, Files SecondFile) {
            return DateTime.Compare(FirstFile.Accessed, SecondFile.Accessed) != 0 ||
                   DateTime.Compare(FirstFile.Modified, SecondFile.Modified) != 0;
        }
    }
}

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