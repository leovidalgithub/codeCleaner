using System;
using System.Collections.Generic;

namespace codeCleanerConsole.Models {
    public class Files : IEquatable<Files> {

        public int CodeCleanerInfoID { get; set; }
        public string Path { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }
        public long Size { get; set; }
        public int Changes { get; set; }
        public bool Active { get; set; }

        public Files() {}
        public Files(int _codeCleanerInfoID, string _path, DateTime _created, DateTime _modified, DateTime _accessed, long _size) {
            this.CodeCleanerInfoID = _codeCleanerInfoID;
            this.Path = _path;
            this.Created = _created;
            this.Modified = _modified;
            this.Accessed = _accessed;
            this.Size = _size;
            this.Changes = 0;
            this.Active = true;
        }
        /// <summary>
        /// Overriding Equals Method used by Enumerable.Except & Enumerable.Intersect
        /// in order to make comparison based on Files.Path<string> Property
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            return Equals(obj as Files);
        }
        public bool Equals(Files other) {
            return other != null &&
                   Path == other.Path;
        }
        public override int GetHashCode() {
            return 467214278 + EqualityComparer<string>.Default.GetHashCode(Path);
        }
    }
}
