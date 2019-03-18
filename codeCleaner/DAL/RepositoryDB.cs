using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using codeCleaner.BLL;
using System.Linq;
using codeCleaner.Helpers;

namespace codeCleaner.DAL {
    public static class RepositoryDB {

        public static readonly string repositoryName = "cc_tailbaseconsole";
        private static SqlCommand sqlCommand = new SqlCommand("SELECT * FROM " + repositoryName + ";", UtilitiesDB.ConnectDB());

        public static List<Files> GetRepositoriesDS() {
            DataSet dataSet = new DataSet("dsRepositories");
            DataTable ResultsTable = new DataTable();
            using (SqlDataAdapter DA = new SqlDataAdapter(sqlCommand)) {
                DA.FillSchema(dataSet, SchemaType.Mapped);
                DA.Fill(ResultsTable);
            }
            //dataSet.Tables[0].TableName = repositoryName;
            List<Files> files = new List<Files>();
            foreach(DataRow fileRow in dataSet.Tables[0].AsEnumerable())
            {
                Files file = new Files();

                file.Path = fileRow["Path"].ToString();
                file.Created = DateTime.Parse(fileRow["Created"].ToString());
                file.Modified = DateTime.Parse(fileRow["Modified"].ToString());
                file.Accessed = DateTime.Parse(fileRow["Accessed"].ToString());
                file.Size = float.Parse(fileRow["Size"].ToString());
                file.Changes = int.Parse(fileRow["Changes"].ToString());
                file.Active = (bool) fileRow["Active"];

                files.Add(file);
            }

            return files;
        }
        public static void SaveRepositoryData( List<Files> files)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.codeCleanerStringConnection))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommandz = new SqlCommand("Cleanup_TrackFileChanges", sqlConnection))
                {
                    sqlCommandz.CommandType = CommandType.StoredProcedure;
                    sqlCommandz.Parameters.AddRange(new List<SqlParameter>() {
                    new SqlParameter() { ParameterName = "@TrackedFiles", SqlDbType = SqlDbType.Structured, Value = ListExtensions.ToDataTable(files) }
                }.ToArray()
                        );

                    sqlCommandz.ExecuteNonQuery();
                }
                sqlConnection.Close();
            }
            MessageBox.Show("Database has been updated successfully!", "Update DB", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
