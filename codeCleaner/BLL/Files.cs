using System;

namespace codeCleaner.BLL {
    public class Files {
        private string path;
        private DateTime created;
        private DateTime modified;
        private DateTime accessed;
        private Single size;
        private int changes;
        private bool active;

        public string Path { get => path; set => path = value; }
        public DateTime Created { get => created; set => created = value; }
        public DateTime Modified { get => modified; set => modified = value; }
        public DateTime Accessed { get => accessed; set => accessed = value; }
        public float Size { get => size; set => size = value; }
        public int Changes { get => changes; set => changes = value; }
        public bool Active { get => active; set => active = value; }

        public Files() {}
        public Files(string _path, DateTime _created, DateTime _modified, DateTime _accessed, float _size) {
            this.path = _path;
            this.created = _created;
            this.modified = _modified;
            this.accessed = _accessed;
            this.size = _size;
            this.changes = 0;
            this.active = true;
        }
    }
}
