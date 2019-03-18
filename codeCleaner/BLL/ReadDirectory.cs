using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using codeCleaner.ExtensionMethods;
using System.Linq;

namespace codeCleaner.BLL {
    public static class ReadDirectory {
        public static string errors;
        public static List<Files> GetFilesInfo() {

            ConcurrentBag<Files> allFiles = new ConcurrentBag<Files>();
            try {
                //TraverseTreeParallelForEach(AppDomain.CurrentDomain.BaseDirectory, f => { //To be used later when done <AppDomain.CurrentDomain.BaseDirectory>
                    TraverseTreeParallelForEach(@"C:\kibana-6.4.2-windows-x86_64", f => { //To be used later when done <AppDomain.CurrentDomain.BaseDirectory>

                        try {
                            
                        FileInfo ff = new FileInfo(f);
                             
                            allFiles.Add(new Files(ff.FullName, ff.CreationTime.TrimMilliseconds(), ff.LastWriteTime.TrimMilliseconds(), ff.LastAccessTime.TrimMilliseconds(), ff.Length));
                    }
                    catch (FileNotFoundException e) { errors += e.Message; }
                    catch (IOException e) { errors += e.Message; }
                    catch (UnauthorizedAccessException e) { errors += e.Message;  }
                    catch (SecurityException e) { errors += e.Message;  }
                });
            } catch (ArgumentException e) {
                errors += @"The directory 'C:\Program Files' does not exist." + e.Message;
            }
            return allFiles.ToList();
        }
        public static void TraverseTreeParallelForEach(string root, Action<string> action) {
            //Count of files traversed and timer for diagnostic output
            int fileCount = 0;
            var sw = Stopwatch.StartNew();

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
                    errors += e.Message;
                    continue;
                }
                // Thrown if another process has deleted the directory after we retrieved its name.
                catch (DirectoryNotFoundException e) {
                    errors += e.Message;
                    continue;
                }

                try {
                    files = Directory.GetFiles(currentDir);
                } catch (UnauthorizedAccessException e) {
                    errors += e.Message;
                    continue;
                } catch (DirectoryNotFoundException e) {
                    errors += e.Message;
                    continue;
                } catch (IOException e) {
                    errors += e.Message;
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
                    ae.Handle((ex) => {
                        if (ex is UnauthorizedAccessException) {
                            // Here we just output a message and go on.
                            errors += ex.Message;
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
            MessageBox.Show(string.Format("GetFilesInfo() Processed {0} files in {1} milliseconds" + Environment.NewLine + errors, fileCount, sw.ElapsedMilliseconds));
        }
    }
}

//static string CalculateMD5(string filename)
//{
//    using (var md5 = MD5.Create())
//    {
//        using (var stream = File.OpenRead(filename))
//        {
//            var hash = md5.ComputeHash(stream);
//            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
//        }
//    }
//}
