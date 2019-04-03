using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using codeCleanerConsole.Models;
using System.Linq;
using codeCleanerConsole.Helpers;
using System.Diagnostics;

namespace codeCleanerConsole.DAL
{
    public static class RepositoryDB
    {
        private static readonly string thisMachineName = Program.logs.ThisMachineName;

        public static DataTable GetCodeCleanerInfoDB()
        {
            DataTable repositoryInfoDataTable = new DataTable();
            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter("SELECT * FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] WHERE MachineName = @MachineName;", UtilitiesDB.ConnectDB()))
                {
                    DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                    DA.FillSchema(repositoryInfoDataTable, SchemaType.Mapped);
                    DA.Fill(repositoryInfoDataTable);
                }
                Program.logs.LogSetRepositoryInfo(repositoryInfoDataTable);
            }
            catch (Exception ex)
            {
                Program.logs.ErrorGettingRepoInfo += "#100 - " + ex;
            }
            return repositoryInfoDataTable;
        }

        public static List<Files> GetPreviousFiles()
        {
            var swPrevious = Stopwatch.StartNew();
            DataTable ResultsTable = new DataTable();
            List<Files> files = new List<Files>();

            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter("SELECT Content.* " +
                                                                    "FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] Info " +
                                                                    "INNER JOIN [DBCleaningBackup].[dbo].[CodeCleanerContent] Content " +
                                                                    "ON Info.ID = Content.CodeCleanerInfoID " +
                                                                    "WHERE Info.MachineName = @MachineName;", UtilitiesDB.ConnectDB()))
                {
                    DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                    DA.FillSchema(ResultsTable, SchemaType.Mapped);
                    DA.Fill(ResultsTable);
                }
                foreach (DataRow fileRow in ResultsTable.AsEnumerable())
                {
                    Files file = new Files();
                    file.CodeCleanerInfoID = Int32.Parse(fileRow["CodeCleanerInfoID"].ToString());
                    file.Path = fileRow["Path"].ToString();
                    file.Created = DateTime.Parse(fileRow["Created"].ToString());
                    file.Modified = DateTime.Parse(fileRow["Modified"].ToString());
                    file.Accessed = DateTime.Parse(fileRow["Accessed"].ToString());
                    file.Size = long.Parse(fileRow["Size"].ToString());
                    file.Changes = int.Parse(fileRow["Changes"].ToString());
                    file.Active = (bool)fileRow["Active"];
                    files.Add(file);
                }
            }
            catch (Exception ex)
            {
                Program.logs.ErrorGettingPrevious+= "#101 - " + ex;
            }
            Program.logs.ElapsedTimePrevious = swPrevious.ElapsedMilliseconds;
            Program.logs.FilesCountPrevious += files.Count;
            return files;
        }

        public static void SaveCodeCleanerContentDB(List<Files> files)
        {
            var swSaving = Stopwatch.StartNew();
            StoreProcedureSaving("Cleanup_TrackFileChanges", files);
            Program.logs.ElapsedTimeSaving = swSaving.ElapsedMilliseconds;
        }
        public static void SaveCodeCleanerLogDB(Logs logs)
        {
            StoreProcedureSaving("Cleanup_LogInfo", new List<Logs>() { logs });
        }

        private static void StoreProcedureSaving<T>(string storeProcedureName, List<T> objectList)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(Properties.Settings.Default.codeCleanerStringConnection))
                {
                    sqlConnection.Open();
                    using (SqlCommand sqlCommandz = new SqlCommand(storeProcedureName, sqlConnection))
                    {
                        sqlCommandz.CommandType = CommandType.StoredProcedure;
                        sqlCommandz.Parameters.AddRange(new List<SqlParameter>() {
                        new SqlParameter() { ParameterName = "@TrackedFiles", SqlDbType = SqlDbType.Structured, Value = ListExtensions.ToDataTable(objectList)}
                }.ToArray()
                            );
                        sqlCommandz.ExecuteNonQuery();
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("#102 - " + ex.Message);
                Program.logs.ErrorSaving += "#102 - " + ex.Message;
            }
        }
    }
}
