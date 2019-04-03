using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using codeCleanerConsole.Helpers;
using codeCleanerConsole.DAL;
using codeCleanerConsole.Models;
using System.Diagnostics;

namespace codeCleanerConsole.BLL
{
    public static class ReadDirectory
    {

        public static List<Files> GetCurrentFiles()
        {
            ConcurrentBag<Files> allFiles = new ConcurrentBag<Files>();
            DataTable CodeCleanerInfo = new DataTable();
            CodeCleanerInfo = RepositoryDB.GetCodeCleanerInfoDB();
            var swCurrent = Stopwatch.StartNew();

            foreach (DataRow item in CodeCleanerInfo.Rows)
            {
                int CodeCleanerInfoID = Int32.Parse(item["ID"].ToString().Trim());
                string SearchRootFolder = item["SearchRootFolder"].ToString().Trim();

                try
                {
                    TraverseTreeParallelForEach(@SearchRootFolder, f =>
                    {
                        try
                        {
                            FileInfo ff = new FileInfo(f);
                            allFiles.Add(new Files(CodeCleanerInfoID, ff.FullName, ff.CreationTime.TrimMilliseconds(), ff.LastWriteTime.TrimMilliseconds(), ff.LastAccessTime.TrimMilliseconds(), ff.Length));
                        }
                        catch (FileNotFoundException ex) { Program.logs.ErrorGettingCurrent += "#010 - " + ex.Message; }
                        catch (IOException ex) { Program.logs.ErrorGettingCurrent += "#011 - " + ex.Message; }
                        catch (UnauthorizedAccessException ex) { Program.logs.ErrorGettingCurrent += "#012 - " + ex.Message; }
                        catch (SecurityException ex) { Program.logs.ErrorGettingCurrent += "#013 - " + ex.Message; }
                    });
                }
                catch (ArgumentException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#014 - Directory does not exist: " + @SearchRootFolder + " - " + ex.Message;
                }
            }
            Program.logs.ElapsedTimeCurrent = swCurrent.ElapsedMilliseconds;
            return allFiles.ToList();
        }

        private static void TraverseTreeParallelForEach(string root, Action<string> action)
        {
            //Count of files traversed and timer for diagnostic output
            int fileCount = 0;
            //var sw = Stopwatch.StartNew();

            // Determine whether to parallelize file processing on each folder based on processor count.
            int procCount = Environment.ProcessorCount;

            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                string currentDir = dirs.Pop();
                string[] subDirs = { };
                string[] files = { };

                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // Thrown if we do not have discovery permission on the directory.
                catch (UnauthorizedAccessException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#015 - " + ex.Message;
                    continue;
                }
                // Thrown if another process has deleted the directory after we retrieved its name.
                catch (DirectoryNotFoundException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#016 - " + ex.Message;
                    continue;
                }

                try
                {
                    files = Directory.GetFiles(currentDir);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#017 - " + ex.Message;
                    continue;
                }
                catch (DirectoryNotFoundException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#018 - " + ex.Message;
                    continue;
                }
                catch (IOException ex)
                {
                    Program.logs.ErrorGettingCurrent += "#019 - " + ex.Message;
                    continue;
                }

                // Execute in parallel if there are enough files in the directory.
                // Otherwise, execute sequentially.Files are opened and processed
                // synchronously but this could be modified to perform async I/O.
                try
                {
                    if (files.Length < procCount)
                    {
                        foreach (var file in files)
                        {
                            action(file);
                            fileCount++;
                        }
                    }
                    else
                    {
                        Parallel.ForEach(files, () => 0, (file, loopState, localCount) =>
                        {
                            action(file);
                            return (int)++localCount;
                        },
                                         (c) =>
                                         {
                                             Interlocked.Add(ref fileCount, c);
                                         });
                    }
                }
                catch (AggregateException ae)
                {
                    ae.Handle((ex) =>
                    {
                        if (ex is UnauthorizedAccessException)
                        {
                            // Here we just output a message and go on.
                            Program.logs.ErrorGettingCurrent += "#020 - " + ex.Message;
                            return true;
                        }
                        // Handle other exceptions here if necessary...
                        return false;
                    });
                }
                // Push the subdirectories onto the stack for traversal.
                // This could also be done before handing the files.
                foreach (string str in subDirs)
                    dirs.Push(str);
            }
            Program.logs.FilesCountCurrent += fileCount;
        }
    }
}
