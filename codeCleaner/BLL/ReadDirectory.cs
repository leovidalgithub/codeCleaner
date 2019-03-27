using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using codeCleaner.ExtensionMethods;
using codeCleaner.DAL;

namespace codeCleaner.BLL {
    public static class ReadDirectory {
        private static List<string> errors = new List<string>();

        public static List<Files> GetFilesInfo() {
            ConcurrentBag<Files> allFiles = new ConcurrentBag<Files>();
            DataTable CodeCleanerInfo = new DataTable();
            CodeCleanerInfo = RepositoryDB.GetCodeCleanerInfoDB();

            foreach (DataRow item in CodeCleanerInfo.Rows) {
                int CodeCleanerInfoID = Int32.Parse(item["ID"].ToString().Trim());
                string SearchRootFolder = item["SearchRootFolder"].ToString().Trim();

                try {
                    TraverseTreeParallelForEach(@SearchRootFolder, f => { 
                        try {
                            FileInfo ff = new FileInfo(f);
                            allFiles.Add(new Files(CodeCleanerInfoID, ff.FullName, ff.CreationTime.TrimMilliseconds(), ff.LastWriteTime.TrimMilliseconds(), ff.LastAccessTime.TrimMilliseconds(), ff.Length));
                        }
                        catch (FileNotFoundException e) { errors.Add(e.Message); }
                        catch (IOException e) { errors.Add(e.Message); }
                        catch (UnauthorizedAccessException e) { errors.Add(e.Message); }
                        catch (SecurityException e) { errors.Add(e.Message); }
                    });                                         }
                catch (ArgumentException e) {
                    errors.Add("Directory " + @SearchRootFolder + " does not exist. " + e.Message);
                }
            }
            return allFiles.ToList();
        }

        public static void TraverseTreeParallelForEach(string root, Action<string> action) {
            //Count of files traversed and timer for diagnostic output
            int fileCount = 0;
            //var sw = Stopwatch.StartNew();

            // Determine whether to parallelize file processing on each folder based on processor count.
            int procCount = Environment.ProcessorCount;

            // Data structure to hold names of subfolders to be examined for files.
            Stack<string> dirs = new Stack<string>();

            if (!Directory.Exists(root)) {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0) {
                string currentDir = dirs.Pop();
                string[] subDirs = { };
                string[] files = { };

                try {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                // Thrown if we do not have discovery permission on the directory.
                catch (UnauthorizedAccessException e) {
                    errors.Add(e.Message);
                    continue;
                }
                // Thrown if another process has deleted the directory after we retrieved its name.
                catch (DirectoryNotFoundException e) {
                    errors.Add(e.Message);
                    continue;
                }

                try {
                    files = Directory.GetFiles(currentDir);
                } catch (UnauthorizedAccessException e) {
                    errors.Add(e.Message);
                    continue;
                } catch (DirectoryNotFoundException e) {
                    errors.Add(e.Message);
                    continue;
                } catch (IOException e) {
                    errors.Add(e.Message);
                    continue;
                }

                // Execute in parallel if there are enough files in the directory.
                // Otherwise, execute sequentially.Files are opened and processed
                // synchronously but this could be modified to perform async I/O.
                try {
                    if (files.Length < procCount) {
                        foreach (var file in files) {
                            action(file);
                            fileCount++;
                        }
                    } else {
                        Parallel.ForEach(files, () => 0, (file, loopState, localCount) => {
                            action(file);
                            return (int)++localCount;
                        },
                                         (c) => {
                                             Interlocked.Add(ref fileCount, c);
                                         });
                    }
                } catch (AggregateException ae) {
                    ae.Handle((e) => {
                        if (e is UnauthorizedAccessException) {
                            // Here we just output a message and go on.
                            errors.Add(e.Message);
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
            //MessageBox.Show(string.Format("GetFilesInfo() Processed {0} files in {1} milliseconds" + Environment.NewLine + errors, fileCount, sw.ElapsedMilliseconds));
        }
    }
}
