using System;
using System.Data.SqlClient;

namespace codeCleanerConsole.DAL {
    public static class UtilitiesDB {
        private static readonly string ConnectionString = Properties.Settings.Default.codeCleanerStringConnection;
        public static SqlConnection ConnectDB() {
            return new SqlConnection(ConnectionString);
        }
    }
}
