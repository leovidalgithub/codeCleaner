using System.Data.SqlClient;

namespace codeCleaner.DAL {
    public static class UtilitiesDB {
        private static readonly string ConnectionString = Properties.Settings.Default.codeCleanerStringConnection;
        public static SqlConnection ConnectDB() {
            return new SqlConnection(ConnectionString);
        }
    }
}
