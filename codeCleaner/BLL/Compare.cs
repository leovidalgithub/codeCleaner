using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

namespace codeCleaner.BLL
{
    public static class Compare
    {
        public static List<Files> CompareFiles(List<Files> CurrentRunFiles, List<Files> PreviousRunPaths)
        {
            List<string> errors = new List<string>();
            List<Files> ChangedFiles = new List<Files>();
            Dictionary<string, Files> previousRun = PreviousRunPaths.ToDictionary(x => x.Path, x => x);
            foreach(var currentFile in CurrentRunFiles)
            {
                if (previousRun.ContainsKey(currentFile.Path))
                {
                    //file already existed in the last run
                    Files LastRunFile = previousRun[currentFile.Path];
                    if (CheckForDateChanges(LastRunFile,currentFile)) //The dates have changed
                    {

                        //Add some logic with LastRunFile & currentFile here, probably.
                        ChangedFiles.Add(currentFile);
                    }
                }else
                {
                    //This file was not found in the previous run
                    ChangedFiles.Add(currentFile);
                }
            }
            return ChangedFiles;
            //try
            //{
            //    Parallel.ForEach(CurrentRunFiles, new ParallelOptions { MaxDegreeOfParallelism = 8 }, (file) =>
            //    {
            //        try
            //        {

            //            //List of my past run
            //            //Current run list

            //            // from old in PastRunList
            //            // from new in NewRunlist
            //            lock (PreviousRunPaths)
            //            {
            //                var DR = PreviousRunPaths.AsEnumerable().SingleOrDefault(x => (string)x["path"] == file.Path);
            //                if (DR is null)
            //                {
            //                    DataRow new_DR = PreviousRunPaths.NewRow();
            //                    new_DR["path"] = file.Path;
            //                    new_DR["created"] = file.Created;
            //                    new_DR["modified"] = file.Modified;
            //                    new_DR["accessed"] = file.Accessed;
            //                    new_DR["size"] = file.Size;
            //                    new_DR["changes"] = file.Changes;
            //                    new_DR["active"] = file.Active;
            //                    PreviousRunPaths.Rows.Add(new_DR);
            //                }
            //                else
            //                {
            //                    if (AnyChange(file, DR)) DR["changes"] = (int)DR["changes"] + 1;
            //                    DR["created"] = file.Created;
            //                    DR["modified"] = file.Modified;
            //                    DR["accessed"] = file.Accessed;
            //                    DR["size"] = file.Size;
            //                    DR["active"] = true;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            errors.Add("IN - " + ex.Message + "-" + file.Path);
            //        }
            //    });

            //}
            //catch (AggregateException ex)
            //{
            //    errors.Add("OUT AggregateException - " + ex.Message);
            //}
            //catch (NullReferenceException ex)
            //{
            //    errors.Add("OUT NullReferenceException - " + ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    errors.Add("OUT Exception - " + ex.Message);
            //}

            //if (errors.Count > 0)
            //{
            //    MessageBox.Show("Errors = " + errors.Count);
            //    foreach (string er in errors)
            //    {
            //        MessageBox.Show(er);
            //    }
            //}
            //return PreviousRunPaths;
        }

        //private static bool AnyChange(Files _file, DataRow _DR)
        //{
        //    return DateTime.Compare(_file.Accessed, (DateTime)_DR["accessed"]) != 0 ||
        //           DateTime.Compare(_file.Modified, (DateTime)_DR["modified"]) != 0;
        //}
        private static bool CheckForDateChanges (Files FirstFile,Files SecondFile)
        {
            return DateTime.Compare(FirstFile.Accessed, SecondFile.Accessed) != 0 ||
                   DateTime.Compare(FirstFile.Modified, SecondFile.Modified) != 0;
        }
    }
}

//Collection was modified; enumeration operation might not execute."
