using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using codeCleanerConsole.Models;
using System.Linq;
using codeCleanerConsole.Helpers;
using System.Diagnostics;
using MoreLinq;

namespace codeCleanerConsole.DAL
{
    public static class RepositoryDB
    {
        private static readonly string thisMachineName = Program.logs.ThisMachineName;
        private static readonly int sizeOfBuckets      = 750;
        private static readonly int timeOut            = 250;

        public static DataTable GetCodeCleanerInfoDB()
        {
            DataTable repositoryInfoDataTable = new DataTable();
            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter("SELECT * FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] " +
                                                                "WHERE MachineName = @MachineName;", UtilitiesDB.ConnectDB()))
                {
                    DA.SelectCommand.CommandTimeout = timeOut;
                    DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                    DA.FillSchema(repositoryInfoDataTable, SchemaType.Mapped);
                    DA.Fill(repositoryInfoDataTable);
                }
                Program.logs.LogSetRepositoryInfo(repositoryInfoDataTable);
            }
            catch (Exception ex)
            {
                Program.logs.ErrorGettingRepoInfo += "#100 - " + ex.GetType().Name + " = " + ex.Message;
            }
            return repositoryInfoDataTable;
        }

        public static List<Files> GetPreviousFiles()
        {
            var swPrevious = Stopwatch.StartNew();
            DataTable ResultsTable    = new DataTable();
            List<Files> previousFiles = new List<Files>();

            try
            {
                using (SqlDataAdapter DA = new SqlDataAdapter("SELECT Content.* " +
                                                                    "FROM [DBCleaningBackup].[dbo].[CodeCleanerInfo] Info " +
                                                                    "INNER JOIN [DBCleaningBackup].[dbo].[CodeCleanerContent] Content " +
                                                                    "ON Info.ID = Content.CodeCleanerInfoID " +
                                                                    "WHERE Info.MachineName = @MachineName;", UtilitiesDB.ConnectDB()))
                {
                    DA.SelectCommand.CommandTimeout = timeOut;
                    DA.SelectCommand.Parameters.AddWithValue("@MachineName", thisMachineName);
                    DA.FillSchema(ResultsTable, SchemaType.Mapped);
                    DA.Fill(ResultsTable);
                }
                foreach (DataRow fileRow in ResultsTable.AsEnumerable())
                {
                    previousFiles.Add(new Files(
                                                    Int32.Parse(fileRow["CodeCleanerInfoID"].ToString()),
                                                    fileRow["Path"].ToString(),
                                                    DateTime.Parse(fileRow["Created"].ToString()),
                                                    DateTime.Parse(fileRow["Modified"].ToString()),
                                                    DateTime.Parse(fileRow["Accessed"].ToString()),
                                                    long.Parse(fileRow["Size"].ToString()),
                                                    int.Parse(fileRow["Changes"].ToString()),
                                                    (bool)fileRow["Active"]                        
                    ));
                }
            }
            catch (Exception ex)
            {
                Program.logs.ErrorGettingPrevious+= "#101 - " + ex.GetType().Name + " = " + ex.Message;
            }
            Program.logs.ElapsedTimePrevious = swPrevious.ElapsedMilliseconds;
            Program.logs.FilesCountPrevious += previousFiles.Count;
            return previousFiles;
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
                    foreach (var objectBatch in objectList.Batch(sizeOfBuckets))
                    {
                        using (SqlCommand sqlCommandz = new SqlCommand(storeProcedureName, sqlConnection))
                        {
                            sqlCommandz.CommandTimeout = timeOut;
                            sqlCommandz.CommandType = CommandType.StoredProcedure;
                            sqlCommandz.Parameters.AddRange(new List<SqlParameter>() {
                        new SqlParameter() { ParameterName = "@TrackedFiles", SqlDbType = SqlDbType.Structured, Value = ListExtensions.ToDataTable(objectBatch.ToList())}
                }.ToArray()
                                );
                            sqlCommandz.ExecuteNonQuery();
                        }
                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // when this method is called for -Cleanup_LogInfo- storeProcedure and it fails, that row wouldn't be saved in DB so that, for now, I just show the error in Console  
                Console.WriteLine("#102 - SP = " + storeProcedureName + " - " + ex.Message);
                Program.logs.ErrorSaving += "#102 - SP = " + storeProcedureName + " - " + ex.GetType().Name + " = " + ex.Message;
            }
        }
    }
}
