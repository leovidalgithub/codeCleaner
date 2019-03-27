using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using codeCleaner.BLL;
using System.Linq;
using codeCleaner.Helpers;

namespace codeCleaner.DAL {
    public static class RepositoryDB {
        private static readonly string thisMachineName = Environment.MachineName;

        public static DataTable GetCodeCleanerInfoDB() {
            DataTable ResultsTable = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter("SELECT * FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] WHERE MachineName = @MachineName;", UtilitiesDB.ConnectDB())) {
                DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                DA.FillSchema(ResultsTable, SchemaType.Mapped);
                DA.Fill(ResultsTable);
            }
            return ResultsTable;
        }

        public static List<Files> GetCodeCleanerContentDB() {
            DataTable ResultsTable = new DataTable();
            List<Files> files = new List<Files>();

            using (SqlDataAdapter DA = new SqlDataAdapter("SELECT Content.* " +
                                                                "FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] Info " +
                                                                "INNER JOIN [DBCleaningBackup].[dbo].[CodeCleanerContent] Content " +
                                                                "ON Info.ID = Content.CodeCleanerInfoID " +
                                                                "WHERE Info.MachineName = @MachineName;", UtilitiesDB.ConnectDB())) {
                DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                DA.FillSchema(ResultsTable, SchemaType.Mapped);
                DA.Fill(ResultsTable);
            }
            foreach(DataRow fileRow in ResultsTable.AsEnumerable()) {
                Files file = new Files();
                file.CodeCleanerInfoID = Int32.Parse(fileRow["CodeCleanerInfoID"].ToString());
                file.Path = fileRow["Path"].ToString();
                file.Created = DateTime.Parse(fileRow["Created"].ToString());
                file.Modified = DateTime.Parse(fileRow["Modified"].ToString());
                file.Accessed = DateTime.Parse(fileRow["Accessed"].ToString());
                file.Size = float.Parse(fileRow["Size"].ToString());
                file.Changes = int.Parse(fileRow["Changes"].ToString());
                file.Active = (bool)fileRow["Active"];
                files.Add(file);
            }
            return files;
        }
        public static void SaveCodeCleanerContentDB(List<Files> files) {
            using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.codeCleanerStringConnection)) {
                sqlConnection.Open();
                using (SqlCommand sqlCommandz = new SqlCommand("Cleanup_TrackFileChanges", sqlConnection)) {
                    sqlCommandz.CommandType = CommandType.StoredProcedure;
                    sqlCommandz.Parameters.AddRange(new List<SqlParameter>() {
                        new SqlParameter() { ParameterName = "@TrackedFiles", SqlDbType = SqlDbType.Structured, Value = ListExtensions.ToDataTable(files) }
                }.ToArray()
                        );
                    sqlCommandz.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            //MessageBox.Show("Database has been updated successfully!", "Update DB", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
