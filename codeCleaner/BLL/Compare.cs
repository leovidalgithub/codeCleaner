using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace codeCleaner.BLL {
    public static class Compare {
        public static DataTable CompareFiles(List<Files> _LIST, DataTable _DT) {

            List <string> errors = new List<string>();

            Parallel.ForEach<Files>(_LIST, new ParallelOptions {  MaxDegreeOfParallelism = 17 }, file => {
                try {
                    var DR = _DT.AsEnumerable().SingleOrDefault(x => (string)x["path"] == file.Path);
                    if (DR is null) {
                        lock (_DT) {
                            DataRow new_DR = _DT.NewRow();
                            new_DR["path"] = file.Path;
                            new_DR["created"] = file.Created;
                            new_DR["modified"] = file.Modified;
                            new_DR["accessed"] = file.Accessed;
                            new_DR["size"] = file.Size;
                            new_DR["changes"] = file.Changes;
                            new_DR["active"] = file.Active;
                            _DT.Rows.Add(new_DR);
                        }
                    } else {
                        lock (_DT) {
                            if (AnyChange(file, DR)) DR["changes"] = (int)DR["changes"] + 1;
                            DR["created"] = file.Created;
                            DR["modified"] = file.Modified;
                            DR["accessed"] = file.Accessed;
                            DR["size"] = file.Size;
                            DR["active"] = true;
                        }
                    }
                }
                //catch (InvalidOperationException) { }
                //catch (ArgumentNullException) { }
                catch (Exception e) { errors.Add(e.Message); }
            });
            MessageBox.Show("Parallel.ForEach<Files> Errors count = " + errors.Count);
            return _DT;
        }
        private static bool AnyChange(Files _file, DataRow _DR) {
            return DateTime.Compare(_file.Accessed, (DateTime)_DR["accessed"]) != 0 ||
                   DateTime.Compare(_file.Modified, (DateTime)_DR["modified"]) != 0;
        }
    }
}
//Collection was modified; enumeration operation might not execute."
