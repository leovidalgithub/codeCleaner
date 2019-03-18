using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace codeCleaner.DAL {
    public static class RepositoryDB {

        public static readonly string repositoryName = "cc_tailbaseconsole";
        private static SqlCommand CMD = new SqlCommand("SELECT * FROM " + repositoryName + ";", UtilitiesDB.ConnectDB());

        public static DataSet GetRepositoriesDS() {
            DataSet DS = new DataSet("dsRepositories");
            using (SqlDataAdapter DA = new SqlDataAdapter(CMD)) {
                DA.FillSchema(DS, SchemaType.Mapped);
                DA.Fill(DS);
            }
            DS.Tables[0].TableName = repositoryName;
            DS.Tables[repositoryName].PrimaryKey = new DataColumn[] { DS.Tables[repositoryName].Columns["ID"] };
            DS.Tables[repositoryName].Columns["ID"].AutoIncrement = true;
            DS.Tables[repositoryName].Columns["ID"].AutoIncrementSeed = 1;
            DS.Tables[repositoryName].Columns["ID"].AutoIncrementStep = 1;
            DS.Tables[repositoryName].Columns["tracingFromDate"].DefaultValue = DateTime.Now;
            return DS;
        }
        public static void SaveRpositoryDT(DataTable DT) {
            using (SqlDataAdapter DA = new SqlDataAdapter(CMD)) {
                using (SqlCommandBuilder BLDR = new SqlCommandBuilder(DA)) {
                    BLDR.ConflictOption = ConflictOption.OverwriteChanges;
                    DA.Update(DT);
                    MessageBox.Show("Database has been updated successfully!", "Update DB", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
