using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;
using Microsoft.Vbe.Interop;

namespace e_Verify_BACK_OFFICE_Service
{
    public static class SqlHelper
    {
        public static Int64 Get_Batch_Number_And_Increament_By_One()
        {

            Int64 varBatch_Number = 0;
            SqlConnection conE = new SqlConnection();
            try
            {
                conE.ConnectionString = ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"];

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conE;

                cd.CommandText = "Get_Batch_Number_And_Increament_By_One";

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cd;
                DataTable dt = new DataTable();
                conE.Open();

                da.Fill(dt);

                varBatch_Number = Convert.ToInt64(dt.Rows[0]["Batch_Number"]);
      
                return varBatch_Number;

            }

            catch (SqlException ex)
            {
                LogError(ex.Message.ToString());
                return varBatch_Number;

            }
            finally
            {
                conE.Close();
            }
        }


        public static Int64 Get_Parameter_One_value(string parParameter_One_Name)
        {

            Int64 varParameterOne = 0;
            SqlConnection conE = new SqlConnection();
            try
            {

                conE.ConnectionString = ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"];

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conE;

                cd.CommandText = "procGetParameter_One_value";

                cd.Parameters.Add(new SqlParameter("@parParameter_One_Name", parParameter_One_Name));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cd;
                DataTable dt = new DataTable();
                conE.Open();

                da.Fill(dt);

                varParameterOne = Convert.ToInt64(dt.Rows[0]["Parameter_One_value"]);

                return varParameterOne;

            }

            catch (SqlException ex)
            {
                LogError(ex.Message.ToString());
                return varParameterOne;

            }
            finally
            {
                conE.Close();
            }
        }
        public static  string funcTitleCase(string varTextFromControls)
        {

            if (varTextFromControls.Trim().Length != 0)
            {
                varTextFromControls = CultureInfo.CurrentCulture.TextInfo.ToUpper(varTextFromControls.ToUpper());
            }
            else
            {
                varTextFromControls = string.Empty;
            }

            return varTextFromControls;

        }

        public static  void LogError(string error)
        {
            SqlConnection conErr = new SqlConnection();
            try
            {
                conErr.ConnectionString = ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"];

                conErr.Open();

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conErr;

                cd.CommandText = "spx_logerror";

                cd.Parameters.Add(new SqlParameter("@Error_decription", error));

                int recordCount = cd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                LogError_No_Pop_Up_Msg(ex.StackTrace.ToString(), "LogError sub routine in LoanTracker");
            }
            finally
            {
                try { conErr.Close(); }
                catch { }
            }
        }

        public static  void LogError_No_Pop_Up_Msg(string varError_decription, string varstep_descrption)
        {
            SqlConnection conErr = new SqlConnection();
            try
            {

                conErr.ConnectionString = ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"];
                conErr.Open();

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conErr;

                cd.CommandText = "spx_logerror";

                cd.Parameters.Add(new SqlParameter("@Error_decription", varError_decription));
                cd.Parameters.Add(new SqlParameter("@STEP_DESCR", varstep_descrption));

                int recordCount = cd.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                //LogError_No_Pop_Up_Msg(ex.Message.ToString(), "LogError_No_Pop_Up_Msg sub routine in LoanTracker");
            }
            finally
            {
                try { conErr.Close(); }
                catch { }
            }
        }

        public static void ExecuteTable(string storedProcedureName, StoredProcedureAction storedProcedureAction, DataTable dataTable, SqlParameter[] parameters, int batchSize)
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command   = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType      = CommandType.StoredProcedure;
                    command.CommandTimeout   = 1000;
                    command.Parameters.AddRange(parameters);
                    command.UpdatedRowSource = UpdateRowSource.None;

                    using (SqlDataAdapter adapter = new SqlDataAdapter())
                    {
                        adapter.UpdateBatchSize = batchSize;
                        switch (storedProcedureAction)
                        {
                            case StoredProcedureAction.Delete: adapter.DeleteCommand = command; break;
                            case StoredProcedureAction.Insert: adapter.InsertCommand = command; break;
                            case StoredProcedureAction.Select: adapter.SelectCommand = command; break;
                            case StoredProcedureAction.Update: adapter.UpdateCommand = command; break;
                            default: break;
                        }

                        connection.Open();
                        int rowsAffected = adapter.Update(dataTable);
                    }
                }
                connection.Close();
            }
        }

        public static  string ExecuteScalar(string storedProcedure, SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection = new SqlConnection( ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    connection.Open();
                    result = command.ExecuteScalar();
                }
                connection.Close();
            }
            return result == null ? null : result.ToString();
        }

        public static string ExecuteScalar(string connectionStr, string storedProcedure, SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    connection.Open();
                    result = command.ExecuteScalar();
                }
                connection.Close();
            }
            return result == null ? null : result.ToString();
        }


        public static string insertSQL(string connstr, string targetTable, Hashtable InsertHash)
        {
            string    sqlText      = string.Format(" INSERT INTO {0} (", targetTable);
            string    sqlValues    = "SELECT ";
            int       sqlTextLen   = sqlText.Length;
            int       sqlValuesLen = sqlValues.Length;
            DataTable result       = new DataTable();
            string    InsertResult = "SUCCESSFUL";

            try
            {
                // Parse the HashTable
                foreach (DictionaryEntry tmpHash in InsertHash)
                {
                    if (tmpHash.Value.ToString() != "")
                    {
                        if (sqlText.Length != sqlTextLen)
                        {
                            sqlText += string.Format(",[{0}]", tmpHash.Key.ToString().Trim());
                        }
                        else
                        {
                            sqlText += string.Format("[{0}]", tmpHash.Key.ToString().Trim());
                        }

                        if (sqlValues.Length != sqlValuesLen)
                        {
                            sqlValues += string.Format(",'{0}'", tmpHash.Value.ToString().Trim().Replace("'", "''"));
                        }
                        else
                        {
                            sqlValues += string.Format("'{0}'", tmpHash.Value.ToString().Trim().Replace("'", "''"));
                        }
                    }
                    //Console.WriteLine("Key  and  value are " + tmpHash.Key + "   " + tmpHash.Value);
                    //Console.Read();
                }
                sqlText += string.Format(") {0}", sqlValues);

                using (SqlConnection connection = new SqlConnection(connstr))
                {
                    using (SqlCommand command = new SqlCommand(sqlText, connection))
                    {
                        command.CommandTimeout = 1000;
                        if (connection != null && connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        command.ExecuteNonQuery();
                    }
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                // Enable column edits in all columns of a data table
                //foreach (DataColumn dc in result.Columns)
                //{
                //    dc.ReadOnly = false;
                //    dc.AllowDBNull = true;
            }
            catch (Exception dbInsertException)
            {
                SR_Class.Log_to_Error("252588", dbInsertException, "SqlHelper.insertSQL", sqlText);
                InsertResult = string.Format("Insert FAILED with error : {0}", dbInsertException.Message);
            }
            finally
            {
            }
            //}
            return InsertResult;

        }

        public static string updateSQL(string connstr, string targetTable, Hashtable UpdateHash, Hashtable whereHash)
        {
            string sqlText = string.Format(" Update {0}  SET ", targetTable);
            int sqlTextLen = sqlText.Length;
            string sqlValues = " WHERE ";
            int sqlValuesLen = sqlValues.Length;
            DataTable result = new DataTable();

            // Parse the HashTable
            foreach (DictionaryEntry tmpHash in UpdateHash)
            {
                if (tmpHash.Value.ToString() != "")
                {
                    if (sqlText.Length != sqlTextLen)
                    {
                        sqlText += string.Format(",[{0}] = '{1}'", tmpHash.Key.ToString().Trim(), tmpHash.Value.ToString().Trim().Replace("'", "''"));
                    }
                    else
                    {
                        sqlText += string.Format("[{0}] = '{1}'", tmpHash.Key.ToString().Trim(), tmpHash.Value.ToString().Trim().Replace("'", "''"));
                    }
                }
            }

            foreach (DictionaryEntry tmpHash in whereHash)
            {
                if (tmpHash.Value.ToString() != "")
                {
                    if (sqlValues.Length != sqlValuesLen)
                    {
                        sqlValues += string.Format(" AND [{0}] = '{1}' ", tmpHash.Key.ToString().Trim(), tmpHash.Value.ToString().Trim().Replace("'", "''"));
                    }
                    else
                    {
                        sqlValues += string.Format(" [{0}] = '{1}' ", tmpHash.Key.ToString().Trim(), tmpHash.Value.ToString().Trim().Replace("'", "''"));
                    }
                }
            }

            sqlText += sqlValues;

            using (SqlConnection connection = new SqlConnection(connstr))
            {
                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    command.CommandTimeout = 1000;
                    if (connection != null && connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    command.ExecuteNonQuery();
                }
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            // Enable column edits in all columns of a data table
            //foreach (DataColumn dc in result.Columns)
            //{
            //    dc.ReadOnly = false;
            //    dc.AllowDBNull = true;
            //}
            return "done";
        }

        public static  DataTable GetTable(string storedProcedure, SqlParameter[] parameters)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }
                connection.Close();
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }

        public static  DataSet GetDataSet(string storedProcedure, SqlParameter[] parameters, string[] output)
        {
            DataSet result = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges, output);
                }
                connection.Close();
            }

            // Enable column edits in all columns of a data table
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ReadOnly = false;
                    dc.AllowDBNull = true;
                }
            }

            return result;
        }

        public static   DataSet GetResultset(string storedProcedure, SqlParameter[] parameters, string[] output)
        {
            DataSet result = new DataSet();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters);
                    command.CommandTimeout = 1000;

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges, output);
                }
                connection.Close();
            }

            // Enable column edits in all columns of a data table
            foreach (DataTable dt in result.Tables)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    dc.ReadOnly = false;
                    dc.AllowDBNull = true;
                }
            }

            return result;
        }

        public static  string GetCurrentBranche()
        {
            //return "201"; // PROD Branch In TZ
            return "TZ1"; // PROD and UAT Branch In TZ use same Branch now..
        }

        public static   int RunSql(string sqlText)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    connection.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                connection.Close();
            }
            return rowsAffected;
        }


        public static  int RunSql_Check_TOP_One(string sqlText)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rowsAffected = dt.Rows.Count;

                    }
                    else
                    {
                        rowsAffected = 0;
                    }
                }
                connection.Close();
            }
            return rowsAffected;
        }

        public static  DataTable RunSql_Check_For_Insert_And_For_Update(string sqlText)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 1000;
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                   
                    connection.Close(); 
                    
                    return dt;
                }
               
            }
        }

        public static int RunSql(string connectionString, string sqlText)
        {
            string SQL_Op_Result = "";
            try
            {
                int rowsAffected = 0;

                string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"] : connectionString;

                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                    {
                        cmd.CommandTimeout = 1000;
                        connection.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                return rowsAffected;
            }
            catch (Exception SQL_Op_Exception)
            {
                Utilities.Log_to_Error("25258811", SQL_Op_Exception, "SqlHelper.RunSql", sqlText);
                SQL_Op_Result = string.Format("Insert FAILED with error : {0}", SQL_Op_Exception.Message);
                return 0;
            }
        }


        public static   string RunSqlScalar(string sqlText)
        {
            string result = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                    {
                        cmd.CommandTimeout = 1000;
                        connection.Open();
                        result = cmd.ExecuteScalar().ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception SQL_Op_Exception)
            {
                Utilities.Log_to_Error("25258811", SQL_Op_Exception, "SqlHelper.RunSqlScalar", sqlText);
                result = string.Format("Insert FAILED with error : {0}", SQL_Op_Exception.Message);
            }
            return result;
        }


        public static   string CheckDBConnection(string sqlText)
        {
            object result = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                {
                    cmd.CommandTimeout = 250;
                    connection.Open();
                    result = cmd.ExecuteScalar();
                }
                connection.Close();
            }
            return result == null ? string.Empty : result.ToString();
        }


        public static string ExecuteScalar(string sqlText)
        {
            object result = string.Empty;
            try
            {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                    {
                        cmd.CommandTimeout = 1000;
                        connection.Open();
                        result = cmd.ExecuteScalar();
                    }
                    connection.Close();
                }
            }
            catch (Exception SQL_Op_Exception)
            {
                Utilities.Log_to_Error("25258811", SQL_Op_Exception, "SqlHelper.ExecuteScalar", sqlText);
                result = string.Format("Insert FAILED with error : {0}", SQL_Op_Exception.Message);
            }
            return result == null ? string.Empty : result.ToString();
        }


        public static   string RunSqlScalar(string connectionString, string sqlText)
        {
            string result = "";

            try
            {
                string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]: connectionString;
                using (SqlConnection connection = new SqlConnection(dbConnection))
                {
                    using (SqlCommand cmd = new SqlCommand(sqlText, connection))
                    {
                        cmd.CommandTimeout = 1000;
                        connection.Open();
                        object o = cmd.ExecuteScalar();
                        if (o != null) result = o.ToString();
                    }
                    connection.Close();
                }
            }
            catch (Exception SQL_Op_Exception)
            {
                Utilities.Log_to_Error("25258811", SQL_Op_Exception, "SqlHelper.RunSqlScalar", sqlText);
                result = string.Format("Insert FAILED with error : {0}", SQL_Op_Exception.Message);
            }
            return result;
        }

        public static   DataTable GetTable(string sqlText)
        {
            DataTable result = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]))
            {
                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    command.CommandTimeout = 1000;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }
                connection.Close();
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }


        //public static   DataTable GetTable(string connectionString, string sqlText)
        //{
        //    DataTable dt = new DataTable();

        //    string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"] : connectionString;
        //    using (SqlConnection connection = new SqlConnection(dbConnection))
        //    {
        //        using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, connection))
        //        {
        //            connection.Open();
        //            adapter.FillSchema(dt, SchemaType.Source);
        //            adapter.Fill(dt);
        //            return dt;
        //        }
        //    }
        //}

        public static DataTable GetTable(string connectionString, string sqlText)
        {
            DataTable result = new DataTable();
            string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"] : connectionString;
            using (SqlConnection connection = new SqlConnection(dbConnection))
            {
                using (SqlCommand command = new SqlCommand(sqlText, connection))
                {
                    command.CommandTimeout = 1000;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    result.Load(reader, LoadOption.OverwriteChanges); //, LoadOption.OverwriteChanges, output);
                }
                connection.Close();
            }

            // Enable column edits in all columns of a data table
            foreach (DataColumn dc in result.Columns)
            {
                dc.ReadOnly = false;
                dc.AllowDBNull = true;
            }
            return result;
        }




        public static   SqlDataReader ExecuteReader(string sqlText)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"]);
            SqlCommand cmd = new SqlCommand(sqlText, connection);
            cmd.CommandTimeout = 1000;
            connection.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public static  void LogError_For_Valiation(string error, string varStepDesc = null)
        {
            SqlConnection conErr = new SqlConnection();
            try
            {

                conErr.ConnectionString = ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"];
                conErr.Open();

                SqlCommand cd = new SqlCommand();
                cd.CommandType = System.Data.CommandType.StoredProcedure;
                cd.Connection = conErr;

                cd.CommandText = "spx_logerror";

                //cd.Parameters.Add(new SqlParameter("@Error_decription", Session["FullName"].ToString().Trim() + " : " + error));

                cd.Parameters.Add(new SqlParameter("@Error_decription", error));
                cd.Parameters.Add(new SqlParameter("@STEP_DESCR", varStepDesc));

                int recordCount = cd.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                try { conErr.Close(); }
                catch { }
            }
        }

        public static  bool IsDate(string varAnyDate)
        {
            bool varValidDate = false;


            string varDateFromTextBox = varAnyDate;

            if (varDateFromTextBox.Length < 10 || varDateFromTextBox.Length > 10) // Date Format == DD-MM-YYYY = 10 Characters
            {
                varValidDate = false;
                return varValidDate;
            }
            string varDateYear = varDateFromTextBox.Substring(6, 4);
            string varDateMonth = varDateFromTextBox.Substring(3, 2);
            string varDateDay = varDateFromTextBox.Substring(0, 2);

            string varFirstSeparator = varDateFromTextBox.Substring(2, 1);
            string varSecondSeparator = varDateFromTextBox.Substring(5, 1);

            if (varFirstSeparator != "-") // Date Format == DD-MM-YYYY  or  DD/MM/YYYY = 10 Characters
            {
                if (varFirstSeparator != "/") // Date Format == DD-MM-YYYY  or  DD/MM/YYYY = 10 Characters
                {
                    varValidDate = false;
                    return false;

                }
            }

            if (varSecondSeparator != "-") // Date Format == DD-MM-YYYY  or DD/MM/YYYY = 10 Characters
            {
                if (varSecondSeparator != "/")
                {
                    varValidDate = false;
                    return false;

                }
            }

            if (varSecondSeparator != varFirstSeparator) // Date Format == DD-MM-YYYY  or DD-MM-YYYY = 10 Characters
            {
                varValidDate = false;
                return false;

            }


            //string varDateOfBirth = varDateYear + "-" + varDateMonth + "-" + varDateDay;
            /*
            if (Information.IsNumeric(varDateYear) && Information.IsNumeric(varDateMonth) && Information.IsNumeric(varDateMonth))
            {
                if (Convert.ToInt64(varDateYear) > 1900 && Convert.ToInt64(varDateYear) < 3000) // Range of years 
                {
                    if (Convert.ToInt64(varDateMonth) > 0 && Convert.ToInt64(varDateMonth) < 13) // range of months
                    {
                        if (Convert.ToInt64(varDateDay) > 0 && Convert.ToInt64(varDateDay) < 32) // Range of dates
                        {

                            // Logig for Leap years for February
                            // For February to get 28 Days only

                            varValidDate = true;
                            return true;

                        }

                    }
                }
            }
             */ 

            return varValidDate;
        }

        public static string subTextBox_Mandatory_Validation(string varTextBox, string varFieldDataType, string varNameOfTextBox, string varLoanID, string varFullname, out string strPassedOrFailed, out string strFailedValidation_Field_Names, string varValidationType = null)
        {
            strPassedOrFailed = "Failed";
            strFailedValidation_Field_Names = string.Empty;

            switch (varFieldDataType)
            {

                case "String": // String values

                    if (varTextBox.Trim().Length == 0 || varTextBox.Trim().ToString() == string.Empty)
                    {

                        if (varValidationType != "Save")
                        {
                            if (Get_Parameter_One_value("LostFocus") == 1) // 1 Enable logging for failed users else LoggingFailed == 0 then disable logging
                            {

                                if (varLoanID.Trim().Length != 0)
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                                else
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                            }
                        }
                    

                        if (varValidationType == "Save")
                        {
                            if (Get_Parameter_One_value("Save") == 1) // 1 Enable logging for failed users else LoggingFailed == 0 then disable logging
                            {
                                if (varLoanID.Trim().Length != 0)
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                                else
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                            }
                        }

                        //if (strFailedValidation_Field_Names.IndexOf(varNameOfTextBox, 0) == 0)
                        // {
                        strFailedValidation_Field_Names = varNameOfTextBox;
                        // }
                        strPassedOrFailed = "Failed";

                    }
                    else
                    {
                        strPassedOrFailed = "Success";
                    }


                    return strPassedOrFailed;

                case "Date": // Date values


                    if (varTextBox.Trim().Length == 0 || varTextBox.Trim().ToString() == string.Empty
                        || IsDate(varTextBox) == false
                        //|| DateTime.TryParse(varTextBox.Text.Trim(), out varDateFromTextBoxOne) == false
                        // || Information.IsDate(varTextBox.Text)==false
                        )
                    {
   

                        if (varValidationType != "Save")
                        {
                            if (varLoanID.Trim().Length != 0)
                            {
                                LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                            }
                            else
                            {
                                LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required validation - Please enter a value !! ", varNameOfTextBox);
                            }
                        }

                        if (varValidationType == "Save")
                        {
                            if (Get_Parameter_One_value("Save") == 1) // 1 Enable logging for failed users else LoggingFailed == 0 then disable logging
                            {
                                if (varLoanID.Trim().Length != 0)
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                                else
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                            }
                        }

                        //if (strFailedValidation_Field_Names.IndexOf(varNameOfTextBox, 0) == 0)
                        // {
                        strFailedValidation_Field_Names = varNameOfTextBox;
                        // }
                        strPassedOrFailed = "Failed";

                    }
                    else
                    {
                        strPassedOrFailed = "Success";
                    }

                    return strPassedOrFailed;

                case "Integer": // Integer values and // Decimal values

                    if (varTextBox.Trim().Length == 0 || varTextBox.Trim().ToString() == string.Empty)
                    {

                        strPassedOrFailed = "Failed";

                        if (varValidationType != "Save")
                        {
                            if (varLoanID.Trim().Length != 0)
                            {
                                LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                            }
                            else
                            {
                                LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required validation - Please enter a value !! ", varNameOfTextBox);
                            }
                        }

                        if (varValidationType == "Save")
                        {
                            if (Get_Parameter_One_value("Save") == 1) // 1 Enable logging for failed users else LoggingFailed == 0 then disable logging
                            {
                                if (varLoanID.Trim().Length != 0)
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " : Loan ID : " + varLoanID.ToString() + " : failed required validation - Please enter a value !! ", varNameOfTextBox);
                                }
                                else
                                {
                                    LogError_For_Valiation(varFullname + " : " + varValidationType + " - " + " failed required On Save validation - Please enter a value !! ", varNameOfTextBox);
                                }
                            }
                        }

                    }
                        strPassedOrFailed = "Success";
    
                        return strPassedOrFailed;

                        case "Decimal": // Decimal values

                    strPassedOrFailed = "Failed";
                    return strPassedOrFailed;

                default:

                    strPassedOrFailed = "Failed";
                    return strPassedOrFailed;
            }
        }
        
        //public static  SqlDataReader GetReader(string connectionString, string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    string dbConnection = string.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString : connectionString;

        //    SqlConnection connection = new SqlConnection(dbConnection);

        //    SqlCommand cmd = new SqlCommand(sqlText, connection);

        //    cmd.CommandTimeout = 1000;
        //    connection.Open();
        //    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        //    return reader;
        //}


        //public static  DataSet GetMultipleResults(string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString))
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


        //public static  DataSet GetMultipleResults(string connectionString, string sqlText, string[] tableNames)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString : connectionString;
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


        //public static  SqlDataReader DataReader(string sqlText)
        //{
        //    SqlDataReader reader = null;

        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString))
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


        //public static  DataSet GetDataSet(string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString))
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


        //public static  DataSet GetDataSet(string connectionString, string sqlText, string tableName)
        //{
        //    DataSet ds = new DataSet();
        //    string dbConnection = String.IsNullOrEmpty(connectionString) ? ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString : connectionString;

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


        //public static  string ExecuteStoredProcedure(string storedProcedureName, SqlParameter[] parameters)
        //{
        //    string result = string.Empty;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandTimeout = 1000;
        //            command.Parameters.AddRange(parameters);
        //            connection.Open();
        //            result = command.ExecuteScalar().ToString();
        //        }
        //        connection.Close();
        //    }
        //    return result;
        //}


        //public static  void ExecuteStoredProcedure(string storedProcedureName, StoredProcedureAction storedProcedureAction, DataTable dataTable, SqlParameter[] parameters, int batchSize)
        //{
        //    string result = string.Empty;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Interface_SQL_DB_Connection"].ConnectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandTimeout = 1000;
        //            command.Parameters.AddRange(parameters);
        //            command.UpdatedRowSource = UpdateRowSource.None;

        //            using (SqlDataAdapter adapter = new SqlDataAdapter())
        //            {
        //                adapter.UpdateBatchSize = batchSize;
        //                switch (storedProcedureAction)
        //                {
        //                    case StoredProcedureAction.Delete: adapter.DeleteCommand = command; break;
        //                    case StoredProcedureAction.Insert: adapter.InsertCommand = command; break;
        //                    case StoredProcedureAction.Select: adapter.SelectCommand = command; break;
        //                    case StoredProcedureAction.Update: adapter.UpdateCommand = command; break;
        //                    default: break;
        //                }

        //                connection.Open();
        //                int rowsAffected = adapter.Update(dataTable);
        //            }
        //        }
        //        connection.Close();
        //    }
        //}

        public enum StoredProcedureAction
        {
            Delete = 1,
            Insert = 2,
            Select = 3,
            Update = 4
        }
    }
}