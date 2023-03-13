using System;
using System.Data;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;

using System.IO;

namespace e_Verify_BACK_OFFICE_Service
{
 public class OracleHelper
    {

        //public static string ExecuteScalar(string sqlText)
        //{
        //    object result = string.Empty;

        //    using (OracleConnection connection = new OracleConnection(Config.OracleConnection))
        //    {
        //        using (OracleCommand cmd = new OracleCommand(sqlText, connection))
        //        {
        //            cmd.CommandTimeout = 1000;
        //            connection.Open();
        //            result = cmd.ExecuteScalar();
        //        }
        //        connection.Close();
        //    }
        //    return result == null ? string.Empty : result.ToString();
        //}
        //}

        public static string RunSqlScalar(string connectionString, string sqlText)
        {
            string result = "";
            try 
            { 
                string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["FCUBS_Connection"] : connectionString;

                using (OracleConnection connection = new OracleConnection(dbConnection))
                {
                    using (OracleCommand cmd = new OracleCommand(sqlText, connection))
                    {
                        cmd.CommandTimeout = 1000;
                        connection.Open();
                        object o = cmd.ExecuteScalar();
                        if (o != null) result = o.ToString();
                    }
                    connection.Close();
                }
                return result;
            }
            catch (Exception Get_Table_Error)
            {
                // Log the Error
                string logFolder = System.Configuration.ConfigurationSettings.AppSettings["ErrorLog_Folder"];
                if (!logFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    logFolder += System.IO.Path.DirectorySeparatorChar;
                }
                string Error_Time = string.Format("{0:yyyy/MM/dd} {1}  {2}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss:ffffff"), "Get Table");
                string Error_to_Log = string.Format("{1}{0}{1}{2}{1}", Error_Time, Environment.NewLine, Get_Table_Error.StackTrace.ToString());

                File.AppendAllText(string.Format("{0}SelectWebService_Error_{1:yyyyMMddHHmm}.txt", logFolder, DateTime.Now), Error_to_Log);

                return null;
            }
        }

        public static DataTable GetTable(string sqlText)
        {
            try
            {
                DataTable result = new DataTable();
                //using (OracleConnection connection = new OracleConnection(Config.OracleConnection))
                using (OracleConnection OracleConnection = new OracleConnection(ConfigurationManager.AppSettings["FCUBS_Connection"]))
                {
                    //using (OracleCommand command = new OracleCommand(sqlText, connection))
                    using (OracleCommand command = new OracleCommand(sqlText, OracleConnection))
                    {
                        command.CommandTimeout = 0;
                        OracleConnection.Open();
                        //OracleDataReader reader = command.ExecuteReader();
                        OracleDataReader reader = command.ExecuteReader();
                        result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                    }
                    OracleConnection.Close();
                }

                // Enable column edits in all columns of a data table
                foreach (DataColumn dc in result.Columns)
                {
                    dc.ReadOnly = false;
                    dc.AllowDBNull = true;
                }
                return result;
            }
            catch (Exception Get_Table_Error)
            {
                // Log the Error
                string logFolder = System.Configuration.ConfigurationSettings.AppSettings["ErrorLog_Folder"];
                if (!logFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                  logFolder += System.IO.Path.DirectorySeparatorChar;
                }
                string Error_Time = string.Format("{0:yyyy/MM/dd} {1}  {2}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss:ffffff"), "Get Table");
                string Error_to_Log = string.Format("{1}{0}{1}{2}{1}", Error_Time, Environment.NewLine, Get_Table_Error.StackTrace.ToString());

                File.AppendAllText(string.Format("{0}SelectWebService_Error_{1:yyyyMMddHHmm}.txt",logFolder,DateTime.Now), Error_to_Log);

                return null;
            }
        }

        public static DataTable GetTable(string connectionString, string sqlText)
        {
            try
            {
                DataTable dt = new DataTable();

                string OraConnectionStr = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["FCUBS_Connection"] : connectionString;
                using (OracleConnection OracleConn = new OracleConnection(OraConnectionStr))
                {
                    //File.AppendAllText("C:/Temp/SelectWebService_Error.txt", string.Format("{2:yyyy/MM/dd HH:mm:ss} Oracle Timeout is {0}{1}", OracleConn.ConnectionTimeout.ToString(),Environment.NewLine,DateTime.Now));
                    using (OracleDataAdapter adapter = new OracleDataAdapter(sqlText, OracleConn))
                    {  
                        OracleConn.Open();
                        adapter.FillSchema(dt, SchemaType.Source);
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception Get_Table_Error)
            {
                // Log the Error
                string logFolder = System.Configuration.ConfigurationSettings.AppSettings["ErrorLog_Folder"];
                if (!logFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                {
                    logFolder += System.IO.Path.DirectorySeparatorChar;
                }
                string Error_Time = string.Format("{0:yyyy/MM/dd} {1}  {2}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss:ffffff"), "Get Table");
                string Error_to_Log = string.Format("{1}{0}{1}{2}{1}", Error_Time, Environment.NewLine, Get_Table_Error.StackTrace.ToString());

                File.AppendAllText(string.Format("{0}SelectWebService_Error_{1:yyyyMMddHHmm}.txt", logFolder, DateTime.Now), Error_to_Log);

                return null;
            }
        }


         //public static SqlDataReader ExecuteReader(string sqlText)
        //{
        //    SqlConnection connection = new SqlConnection(Config.DBConnection);
        //    SqlCommand cmd = new SqlCommand(sqlText, connection);
        //    cmd.CommandTimeout = 1000;
        //    connection.Open();
        //    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //}

        //public static SqlDataReader GetReader(string connectionString, string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    string dbConnection = string.IsNullOrEmpty(connectionString) ? Config.DBConnection : connectionString;

        //    SqlConnection connection = new SqlConnection(dbConnection);

        //    SqlCommand cmd = new SqlCommand(sqlText, connection);

        //    cmd.CommandTimeout = 1000;
        //    connection.Open();
        //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    return reader;
        //}


        //public static DataSet GetMultipleResults(string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(Config.DBConnection))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static DataSet GetMultipleResults(string connectionString, string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? Config.DBConnection : connectionString;
        //    using (SqlConnection connection = new SqlConnection(dbConnection))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            ds.Load(reader, LoadOption.OverwriteChanges, tableNames);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}


        //public static SqlDataReader DataReader(string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    using (SqlConnection connection = new SqlConnection(Config.DBConnection))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sqlText, connection))
        //        {
        //            connection.Open();
        //            cmd.CommandTimeout = 1000;
        //            reader = cmd.ExecuteReader();
        //        }
        //        connection.Close();
        //    }
        //    return reader;
        //}


        //public static DataSet GetDataSet(string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(Config.DBConnection))
        //    {
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
        //        {
        //            connection.Open();
        //            adapter.Fill(ds, tableName);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}

        //public static DataSet GetDataSet(string connectionString, string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? Config.DBConnection : connectionString;

        //    using (SqlConnection connection = new SqlConnection(dbConnection))
        //    {
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
        //        {
        //            connection.Open();
        //            adapter.Fill(ds, tableName);
        //        }
        //        connection.Close();
        //    }
        //    return ds;
        //}

    }
}

