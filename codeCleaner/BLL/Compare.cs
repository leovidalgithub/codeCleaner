using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace codeCleaner.BLL {
    public static class Compare {
        public static DataTable CompareFiles(List<Files> _List, DataTable _DT) {
            List<string> errors = new List<string>();

            try {
            TraverseTreeParallelForEach(_List, file => {
                try {
                    lock (_DT) {
                        var DR = _DT.AsEnumerable().SingleOrDefault(x => (string)x["path"] == file.Path);
                        if (DR is null) {
                                DataRow new_DR = _DT.NewRow();
                                new_DR["path"] = file.Path;
                                new_DR["created"] = file.Created;
                                new_DR["modified"] = file.Modified;
                                new_DR["accessed"] = file.Accessed;
                                new_DR["size"] = file.Size;
                                new_DR["changes"] = file.Changes;
                                new_DR["active"] = file.Active;
                                _DT.Rows.Add(new_DR);
                        } else {
                            if (AnyChange(file, DR)) DR["changes"] = (int)DR["changes"] + 1;
                            DR["created"] = file.Created;
                            DR["modified"] = file.Modified;
                            DR["accessed"] = file.Accessed;
                            DR["size"] = file.Size;
                            DR["active"] = true;
                        }
                    }
                } catch (Exception ex) {
                    errors.Add("IN - " + ex.Message + "-" + file.Path);
                }
            });

            } catch (AggregateException ex) {
                errors.Add("OUT AggregateException - " + ex.Message);
            }
            catch (NullReferenceException ex) {
                errors.Add("OUT NullReferenceException - " + ex.Message);
            }
            catch (Exception ex) {
                errors.Add("OUT Exception - " + ex.Message);
            }

            if (errors.Count > 0) {
                MessageBox.Show("Errors = " + errors.Count);
                foreach (string er in errors) {
                    MessageBox.Show(er);
                }
            }
            return _DT;
        }
        private static void TraverseTreeParallelForEach(List<Files> _List, Action<Files> action) {
            Parallel.ForEach(_List, new ParallelOptions { MaxDegreeOfParallelism = 8 } , (file, loopState, localCount) => {
                action(file);
            });
        }
        private static bool AnyChange(Files _file, DataRow _DR) {
            return DateTime.Compare(_file.Accessed, (DateTime)_DR["accessed"]) != 0 ||
                   DateTime.Compare(_file.Modified, (DateTime)_DR["modified"]) != 0;
        }
    }
}

//Collection was modified; enumeration operation might not execute."
