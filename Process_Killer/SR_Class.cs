using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Select_Research;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Xml;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Globalization;

namespace e_Verify_BACK_OFFICE_Service

{
    public class SR_Class
    {
        public static string SQLStr;
        public static string TrnServer_Name_And_Instance;
        public static string MISServer_Name_And_Instance;
        public static string Trxn_Product_ID = "";
        public static string Trxn_Print_ID = "";
        public static bool Live_Instance = true;
        public static double Testing_Balance = 0.0;
        public static System.Data.SqlClient.SqlConnection Conn = new SqlConnection();

        //public static DBConnections.TrxnDBConnection TrxnDBConnection = new DBConnections.TrxnDBConnection();
        //public static Select_Research.SQLDB TrxnDB = new Select_Research.SQLDB(TrxnDBConnection.ConnString);
        public static int ListViewItemNumber = 1;
        private static ListViewItem lstItem;
        public static string[] Tag_Separator = new string[] { "BCN" };
        public static System.String eVerify_Version = "1.1.4 20150325";
       // public static System.Object Cellt_Bridge_Resp = new Cellulant_Bridge.Cellulant_Bridge_Main();

        public enum enumObjectType
        {
            StrType = 0,
            IntType = 1,
            DblType = 2
        }
        public static bool CheckRights(string User_ID, string userPermission, bool MsgOut)
        {
            bool returnValue = false;
            DataTable rightsTable  = new DataTable();
            bool      LocalBool    = false;
            string    rightDesc   = "UnKnown";

            SQLStr    = string.Format("exec [dbo].[ustp_UserPermission_Check] @UserId_C = '{0}', @groupPermssion = '{1}' ", User_ID, userPermission);
            LocalBool = (SqlHelper.GetTable(ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString, SQLStr).Rows.Count > 0) ? true : false;

            rightsTable = SqlHelper.GetTable(ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString, SQLStr);

            rightsTable = SqlHelper.GetTable(ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString, SQLStr);
            if (MsgOut && !(LocalBool))
            {
                SQLStr = string.Format( "Select * from tbl_Sys_Permissions Where perm_PermissionId = '{0}'" , userPermission);
                rightsTable = SqlHelper.GetTable(SQLStr);
                if (rightsTable.Rows.Count != 0)
                {
                    foreach (DataRow rightsRow in rightsTable.Rows)
                    {
                        rightDesc = rightsRow["perm_Permission_Desc"].ToString();
                    }
                }
                // string msgApprove = "Please enter your User ID";
             
                // RadWindowManager1.RadAlert(msgApprove.ToString(), 520, 100, "".ToString().Trim(), "", "");

                // Interaction.MsgBox("Insufficient User Rights to Module (User : " + User_ID.Trim() + ", Group : " + User_Group.Trim() + "). Must Have (" + Rights_Desc + ") Rights", (int)Constants.vbOKOnly + Constants.vbInformation, (new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.Title + ". User Rights");
            }
            returnValue = LocalBool;
            return returnValue;
        }


        public static dynamic fn_Save_UserLogging_Detail(string UserId_C, string Login_Success_YN, string login_Details_C, string loginGuid)
        {

            string strHostName  = "";
            string strIPAddress = "";
            strHostName = System.Convert.ToString(System.Net.Dns.GetHostName());
            //strIPAddress = GetIPAddress();
            try
            {
                SQLStr = string.Format("[dbo].[ustp_Create_SysLogin_Log_Ver2] @UserId_C = '{0}', @Host_Name_C = '{1}', @Host_IP_C = '{2}',@Login_Success_YN  = '{3}', @login_Details_C = '{4}', @loginGuid = '{5}'", UserId_C, strHostName, "none", Login_Success_YN, login_Details_C, loginGuid);

                SqlHelper.RunSql(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), SQLStr);
            }
            catch (Exception)
            {
                string Saverec = "";
            }
            return "";
        }

        //public static string SR_Encrypt(string txt, string cryptkey)
        //{
        //    if (txt == "")
        //    {
        //        return null;
        //    }
        //    if (cryptkey == "")
        //    {
        //        return null;
        //    }
        //    string keyval = keyvalue(cryptkey);
        //    string[] kv = keyval.Split('/');
        //    string estr = "";
        //    int i = 0;
        //    string e = "";
        //    int rndval = 0;
        //    for (i = 0; i <= txt.Length; i++)
        //    {
        //        if (!string.IsNullOrEmpty(e))
        //        {
        //            e = txt.Substring(i + 1 - 1);
        //            e = e.Substring(0, 1);
        //            e = System.Convert.ToString(Encoding.ASCII.GetBytes(e));

        //            e = System.Convert.ToString(Convert.ToInt32(System.Convert.ToDouble(Convert.ToInt32(e)) + System.Convert.ToDouble(Convert.ToInt32(kv[0]))));
        //            e =System.Convert.ToString(Convert.ToInt32(System.Convert.ToDouble(Convert.ToInt32(e)) * System.Convert.ToDouble(Convert.ToInt32(kv[1]))));
        //            VBMath.Randomize();
        //            rndval = System.Convert.ToInt32(Convert.ToInt32((90 - 65 + 1) * VBMath.Rnd() + 65));
        //            estr = estr + System.Convert.ToString(Strings.Chr(rndval)) + e;
        //        }
        //    }
        //    return estr;
        //}

        //-----------------------------------------------
        //@Name: Decrypt()
        //@Args: txt-> String to decrypt.
        //@Args: cryptkey -> String used to encrypt the string.
        //@Returns: estr -> Decrypted string.
        //-----------------------------------------------


        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                 // Encrypted string to return  
            RijndaelManaged aesAlg = null;        // RijndaelManaged object used to encrypt the data.  

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object  
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

                // Create a decryptor to perform the stream transform.  
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.  
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    // prepend the IV  
                    msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
                    using (CryptoStream csEncrypt =
                       new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            catch (Exception ex)
            {
                //Label l1 = new Label();
                //l1.ForeColor = Color.Red;
                //l1.Text = "Enter Proper Key value.";
                //l1.Show();
                //Form1 f = new Form1();
                //f.Controls.Add(l1);
            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.  
            return outStr;
        }

        /// <summary>  
        /// Decrypt the given string.  Assumes the string was encrypted using  
        /// EncryptStringAES(), using an identical sharedSecret.  
        /// </summary>  
        /// <param name="cipherText">The text to decrypt.</param>  
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>  
        public static string DecryptStringAES(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object  
            // used to decrypt the data.  
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt  
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create the streams used for decryption.  
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    // Create a RijndaelManaged object  
                    // with the specified key and IV.  
                    aesAlg = new RijndaelManaged();
                    aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                    // Get the initialization vector from the encrypted stream  
                    aesAlg.IV = ReadByteArray(msDecrypt);
                    // Create a decrytor to perform the stream transform.  
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (CryptoStream csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                //Label l = new Label();
                //l.ForeColor = Color.Red;
                //l.Text = "Enter Proper Key value.";
                //l.Show();
                //Form1 f = new Form1();
                //f.Controls.Add(l);

            }
            finally
            {
                // Clear the RijndaelManaged object.  
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }
        public string LogError(string error_Module, string error_Desc, string Tkn_ID)
        {
            string Error_Text = System.Text.RegularExpressions.Regex.Replace(error_Desc, @"\s{2,}", " ").Replace("\n", "");
            try
            {
              
                Hashtable m_hashtable = new Hashtable();
                m_hashtable.Add("Err_Module_C", error_Module);
                m_hashtable.Add("Err_Desc_C", Error_Text);
                m_hashtable.Add("Err_Date_D", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                m_hashtable.Add("Err_Token", Tkn_ID);
                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "tbl_ErrorLog", m_hashtable);

                return "ERROR";
            }
            catch (Exception ErrException)
            {
                e_Verify_BACK_OFFICE_Service.LogToFile(error_Module, Error_Text);
                return "ERROR";
            }
        }
        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SystemException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new SystemException("Did not read byte array properly");
            }

            return buffer;
        }


        public static dynamic CheckMobileNumber(string MobileNum)
        {
            string LocalMobileNum = MobileNum;
            LocalMobileNum = System.Convert.ToString(LocalMobileNum.Replace(" ", "").Replace("-", "").Replace("+", ""));
            string NewMobileNum = "";

            if (LocalMobileNum.Length == 10)
            {
                if (LocalMobileNum.Substring(0, 1) == "0")
                {
                    NewMobileNum = string.Format("{0}{1}", "263", LocalMobileNum.Substring(1, 9));
                }
            }
            if (LocalMobileNum.Length == 12)
            {
                NewMobileNum = LocalMobileNum;
            }
            if (LocalMobileNum.Length > 10)
            {
                if (LocalMobileNum.Substring(0, 5) == "00263")
                {
                    NewMobileNum = "263" + LocalMobileNum.Substring(5, LocalMobileNum.Length - 5);
                }
            }
            if (NewMobileNum.Length > 10)
            {
                string Mob_Stem = NewMobileNum.Substring(0, 5);
               // int Stems_Count = System.Convert.ToInt32(Val(TrxnDB.getDataSet(string.Format("SELECT COUNT(*) AS Sterm_Count FROM tbl_Mobile_Sterms WHERE Mobile_Sterm_C ='{0}'", Mob_Stem)).Tables[0].Rows[0][0].ToString()));
                //if (Stems_Count == 0)
                //{
                //    NewMobileNum = "";
                //}
            }
            return NewMobileNum;
        }



        public static License_Obj Check_License(string Lic_Code, int Lic_Len, string DeCryptkey, DateTime Server_Date)
        {
            int Lic_Err_Code = System.Convert.ToInt32(0.0);
            License_Obj Tmp_LicenseClass = new License_Obj();
            try
            {
                string Open_Key = "";
                string Lic_Exp_Date = "";
                string Lic_Exp_Str = "";
                string Lic_Hash_Code = "";
                string Lic_Err_Desc = "";
                string Lic_Warning = "";
                string Lic_Code_No_Hash = "";
                string Lic_Hash_Code_Calculated = "";

                if (Lic_Code.Trim().Length != Lic_Len)
                {
                    Lic_Err_Code = 5010;
                    Lic_Err_Desc = string.Format("License Length not {0}", Lic_Len);
                }
                else
                {
                    Lic_Code_No_Hash = Lic_Code.Substring(0, Lic_Len - 4);
                    //Lic_Hash_Code_Calculated = GetHashValue(Lic_Code_No_Hash).ToString();
                    Lic_Hash_Code = Lic_Code.Substring(Lic_Len - 4, 4);
                    if (Lic_Hash_Code != Lic_Hash_Code_Calculated)
                    {
                        Lic_Err_Code = 5011;
                        Lic_Err_Desc = "License Length not equal to Hashed Length";
                    }
                }

                //Open_Key = SR_Decrypt(Lic_Code, DeCryptkey)
               // Open_Key = Decrypt_Pass(Lic_Code_No_Hash, DeCryptkey);
                if (Open_Key.Trim().Length != 15)
                {
                    Lic_Err_Code = 5012;
                    Lic_Err_Desc = "License Length not 15";
                }

                Lic_Exp_Date = string.Format("{0}{3}{1}{3}{2}", Open_Key.Substring(8, 4), Open_Key.Substring(4, 2), Open_Key.Substring(0, 2), "-");
                Lic_Exp_Str = string.Format("{0}{1}{2}", Open_Key.Substring(2, 2), Open_Key.Substring(6, 2), Open_Key.Substring(12, 3));
                if (Lic_Code == "")
                {
                    Lic_Err_Code = 5013;
                    Lic_Err_Desc = "License Length is blank";
                }

                if (Lic_Code.Trim().Length != Lic_Len)
                {
                    Lic_Err_Code = 5014;
                    Lic_Err_Desc = "License Length not equal to expected Length";
                }

                if (Lic_Exp_Str != string.Format("SrTc{0}", DeCryptkey))
                {
                    Lic_Err_Code = 5015;
                    Lic_Err_Desc = "Encrypted license not in the correct Format";
                }

                if (DateTime.Parse(Lic_Exp_Date) > Server_Date.AddMonths(12))
                {
                    Lic_Err_Code = 5016;
                    Lic_Err_Desc = "Encrypted expiry date is greater than 12 monhts from today";
                }

                if (DateTime.Parse(Lic_Exp_Date) < Server_Date)
                {
                    Lic_Err_Code = 5017;
                    Lic_Err_Desc = "License code has expired";
                }

                //if (Information.IsDate(Lic_Exp_Date))
                //{
                //    if (Server_Date.AddDays(14) > DateTime.Parse(Lic_Exp_Date))
                //    {
                //        //Lic_Warning = System.Convert.ToString(string.Format("Consider renewing your license.{2}It is expiring in {0} day{1}.{2}Please contact IT.", DateAndTime.DateDiff(DateInterval.Day, Server_Date, DateTime.Parse(Lic_Exp_Date)), (DateAndTime.DateDiff(DateInterval.Day, Server_Date, DateTime.Parse(Lic_Exp_Date)) > 1) ? "s" : "", Environment.NewLine));
                //    }
                //}

                Tmp_LicenseClass.Expiry_Date = Lic_Exp_Date;
                Tmp_LicenseClass.Response_Code = Lic_Err_Code;
                Tmp_LicenseClass.Response_Desc = Lic_Err_Desc;
                Tmp_LicenseClass.Warning_Msg = Lic_Warning;

                return Tmp_LicenseClass;

            }
            catch (Exception Licence_Ex)
            {
                Lic_Err_Code = 5009;
                Tmp_LicenseClass.Response_Code = Lic_Err_Code;
                Tmp_LicenseClass.Response_Desc = Licence_Ex.StackTrace.ToString();
                // MsgBox("The Key has been tempered with and cannot be used", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Licensing Application")
                return Tmp_LicenseClass;
            }
        }


        public static dynamic GetHashValue(string HashString)
        {
            string HashValue = "";
            int HashLoop = 0;
            double HashTotal = 0;
            double HashingInt = 0;
            double HashStrLen = HashString.Length;
            HashStrLen = System.Convert.ToDouble(HashStrLen == 0 ? 0 : HashStrLen - 1);
            for (HashLoop = 0; HashLoop <= System.Convert.ToInt32(HashStrLen); HashLoop++)
            {
               // HashTotal = HashTotal + Strings.Asc(HashString.Substring(HashLoop, 1));
            }
          //  HashingInt = Int(HashTotal / 9999);
           // HashValue = System.Convert.ToString((HashTotal - (HashingInt * 9999)).ToString().PadLeft(4, "0"));
            return HashValue;
        }

        //public static string Decrypt_Pass(string txt, string cryptkey)
        //{
        //    string keyval = keyvalue(cryptkey);
        //    string[] kv = keyval.Split('/');
        //    string estr = "";
        //    string tmp = "";
        //    int i = 0;
        //    for (i = 1; i <= txt.Length; i++)
        //    {
                //if (Microsoft.VisualBasic.Left(txt.Substring(i - 1), 1) != "")
                //{
                //   // if ((Strings.Asc(txt.Substring(i - 1).Substring(0, 1)) > 64) && (Strings.Asc(txt.Substring(i - 1).Substring(0, 1)) < 91))
                //    {
                //        if (!string.IsNullOrEmpty(tmp))
                //        {
                //            //tmp = System.Convert.ToString(Int(tmp / Int(kv[1])));
                //            //tmp = System.Convert.ToString(Int(System.Convert.ToDouble(tmp - Int(kv[0]))));
                //            //estr = estr + System.Convert.ToString(Strings.Chr(System.Convert.ToInt32(tmp)));
                //            //tmp = "";
                //        }
                //    }
            //        else
            //        {
            //            tmp = tmp + txt.Substring(i - 1).Substring(0, 1);
            //        }
            //    }
            //}
            //tmp = System.Convert.ToString(Int(tmp / Int(kv[1])));
            //tmp = System.Convert.ToString(Int(System.Convert.ToDouble(tmp - Int(kv[0]))));
            //estr = estr + System.Convert.ToString(Strings.Chr(System.Convert.ToInt32(tmp)));
        //    return estr;
        //}

        //public static string ReportConnection_String()
        //{
        //   // string collectionDoc = string.Format("{0}\\everify.config", (new Microsoft.VisualBasic.ApplicationServices.ConsoleApplicationBase()).Info.DirectoryPath);
        //    DataSet DTSet = new DataSet();
        //    string DecryptKey = "SELECT";
        // //   DTSet.ReadXml(collectionDoc);

        //    string Rpt_ServerName = "";
        //    string Rpt_DatabaserName = "";
        //    string Rpt_UserName = "";
        //    string Rpt_Password = "";
        //    string Rpt_IntSec;

        //    //    MyReport = New ReportDocument
        //    //    Dim ReportPath As String = SR_Decrypt(DTSet.Tables[0].Rows[0]["_ReportPath"), DecryptKey)
        //    //    MyReport.Load(ReportPath & "\TellerPrinting_ZIMRA.rpt")

        //    Rpt_ServerName = SR_Decrypt(System.Convert.ToString(DTSet.Tables[0].Rows[0]["_ServerName"]), DecryptKey);
        //    Rpt_DatabaserName = SR_Decrypt(System.Convert.ToString(DTSet.Tables[0].Rows[0]["_Database"]), DecryptKey);
        //    Rpt_UserName = SR_Decrypt(System.Convert.ToString(DTSet.Tables[0].Rows[0]["_UserID"]), DecryptKey);
        //    Rpt_Password = SR_Decrypt(System.Convert.ToString(DTSet.Tables[0].Rows[0]["_Password"]), DecryptKey);
        //    Rpt_IntSec = SR_Decrypt(System.Convert.ToString(DTSet.Tables[0].Rows[0]["_UseIntegSec"]), DecryptKey);

        //    string sConnString = "";
        //    // sConnString = String.Format("Server={0};Database={1};UID={2};PWD={3}", My.Settings._ServerName, My.Settings._Database, My.Settings._UserID, My.Settings._Password)
        //    sConnString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", Rpt_ServerName, Rpt_DatabaserName, Rpt_UserName, Rpt_Password);
        //    return sConnString;
        //}


      

        public dynamic CheckDBNull(object obj, enumObjectType ObjectType = enumObjectType.StrType)
        {
            object objReturn = null;
            objReturn = obj;
            if (ObjectType == enumObjectType.StrType & Convert.IsDBNull(obj))
            {
                objReturn = "";
            }
            else if (ObjectType == enumObjectType.IntType & Convert.IsDBNull(obj))
            {
                objReturn = 0;
            }
            else if (ObjectType == enumObjectType.DblType & Convert.IsDBNull(obj))
            {
                objReturn = 0.0;
            }
            return objReturn;
        }

        //public static void Save_XML_Record(string XML_Type, string XML_Ref, string XML_String_C, string XML_Processing_Time)
        //{
        //    DBConnections.TrxnDBConnection TrxnDBConnection = new DBConnections.TrxnDBConnection();
        //    Select_Research.SQLDB TrxnDB = new Select_Research.SQLDB(TrxnDBConnection.ConnString);
        //    Select_Research.SQLDB SQL_DB_Conn = new Select_Research.SQLDB(Conn);
        //    Hashtable Hash_Rec = default(Hashtable);

        //    Hash_Rec = new Hashtable();
        //    Hash_Rec.Add("XML_Type_C", XML_Type);
        //    Hash_Rec.Add("XML_Ref_C", XML_Ref);
        //    Hash_Rec.Add("XML_String_C", XML_String_C);
        //    Hash_Rec.Add("XML_Date_D", XML_Processing_Time);
        //    TrxnDB.insertSQL("tbl_XML_log", Hash_Rec);

        //}
      
     
        public static void Log_to_Error(string Error_No, Exception Error_Expt, string Error_Module, string Err_Unique_Ref)
        {
            string Local_Error_Descript = System.Convert.ToString(Error_Expt.StackTrace.ToString());
            try
            {
                Hashtable Error_Hash = new Hashtable();
                string    Err_Time   = string.Format("{0:yyyy/MM/dd HH:mmm:ss}", DateTime.Now);
                Local_Error_Descript = System.Convert.ToString(System.Text.RegularExpressions.Regex.Replace(Local_Error_Descript, "\\s{2,}", " ").Replace("\\n", ""));
                Local_Error_Descript = System.Convert.ToString(Local_Error_Descript.Replace("!", " ").Replace("&", " And ").Replace("<", "").Replace(">", "").Replace("'", "").Replace(",", " "));

                if (Local_Error_Descript.Length > 8000)
                {
                    Local_Error_Descript = Local_Error_Descript.Substring(0, 8000);
                }
               
                Error_Hash.Add("Err_Number_N", Error_No);
                Error_Hash.Add("Err_Desc_C",   Local_Error_Descript);
                Error_Hash.Add("Err_Module_C", Error_Module);
                Error_Hash.Add("Err_Date_D",   Err_Time);
                Error_Hash.Add("Err_Details",  Err_Unique_Ref);

                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"], "tbl_ErrorLog", Error_Hash);
            }
            catch (Exception LogErrorException)
            {
                string Errstr = System.Convert.ToString(LogErrorException.ToString());
                e_Verify_BACK_OFFICE_Service.LogToFile(Error_Module, Local_Error_Descript);
                e_Verify_BACK_OFFICE_Service.LogToFile(Error_Module, Err_Unique_Ref);
            }
        }

        public static void Log_to_Error(string Error_No, Exception Error_Expt, string Error_Module, string Err_Unique_Ref, string DBConnctionStr)
        {
            string Local_Error_Descript = System.Convert.ToString(Error_Expt.StackTrace.ToString());
            try
            {
                Hashtable Error_Hash = new Hashtable();
                string    Err_Time   = string.Format("{0:yyyy/MM/dd HH:mmm:ss}", DateTime.Now);
                Local_Error_Descript = System.Convert.ToString(System.Text.RegularExpressions.Regex.Replace(Local_Error_Descript, "\\s{2,}", " ").Replace("\\n", ""));
                Local_Error_Descript = System.Convert.ToString(Local_Error_Descript.Replace("!", " ").Replace("&", " And ").Replace("<", "").Replace(">", "").Replace("'", "").Replace(",", " "));

                if (Local_Error_Descript.Length > 8000)
                {
                    Local_Error_Descript = Local_Error_Descript.Substring(0, 8000);
                }

                Error_Hash.Add("Err_Number_N"   , Error_No              );
                Error_Hash.Add("Err_Desc_C"     , Local_Error_Descript  );
                Error_Hash.Add("Err_Module_C"   , Error_Module          );
                Error_Hash.Add("Err_Date_D"     , Err_Time              );
                Error_Hash.Add("Err_Details"    , Err_Unique_Ref        );

                SqlHelper.insertSQL(DBConnctionStr, "tbl_ErrorLog", Error_Hash);
            }
            catch (Exception LogErrorException)
            {
                string Errstr = System.Convert.ToString(LogErrorException.ToString());

                e_Verify_BACK_OFFICE_Service.LogToFile(Error_Module, Local_Error_Descript);
                e_Verify_BACK_OFFICE_Service.LogToFile(Error_Module, Err_Unique_Ref);
            }
        }


        //public string RemoveChars(string InStr = "")
        //{
        //    // The procedure leaves only numbers and alphabet and removes special characters.
        //    string TempStr = "";
        //    double LenOfStr = InStr.Length;
        //    double CharAscii = 0;
        //    double I = 0;
        //    if (LenOfStr == 0)
        //    {
        //        TempStr = "";
        //    }
        //    else
        //    {
        //        for (I = 0; I <= LenOfStr - 1; I++)
        //        {
        //            CharAscii = Strings.Asc(InStr.Substring(I, 1));
        //            if ((CharAscii >= 47 & CharAscii <= 57))
        //            {
        //                TempStr = TempStr + InStr.Substring(I, 1);
        //            }
        //            else
        //            {
        //                if ((CharAscii >= 65 & CharAscii <= 90))
        //                {
        //                    TempStr = TempStr + InStr.Substring(I, 1);
        //                }
        //                else
        //                {
        //                    if ((CharAscii >= 97 & CharAscii <= 122))
        //                    {
        //                        TempStr = TempStr + InStr.Substring(I, 1);
        //                    }
        //                    else
        //                    {
        //                        if (CharAscii == 32)
        //                        {
        //                            TempStr = TempStr + InStr.Substring(I, 1);
        //                        }
        //                        else
        //                        {
        //                            TempStr = TempStr;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    TempStr = TempStr.Replace("&", "n").Replace("?", " ");
        //    return TempStr;
        //}

        //public static Class_Charges_New fn_Calculate_Charge_Gvt_Levy_And_Commision(string TrnType, string Trxn_Amount, double RealExchangeRate, string Account_2_Charge)
        //{
        //    Class_Charges_New Tmp_Charge_Class = new Class_Charges_New();
        //    DataTable ds = default(DataTable);
        //    bool Is_Charging = false;
        //    double Gvt_Levy_Amnt = 0;
        //    double Charging_Percent = 0;
        //    bool Charging_Type = false;
        //    double Charging_Amnt = 0;
        //    double Commission_Amnt = 0;

        //    Tmp_Charge_Class.Initialise_Details();

        //    if ((TrnType == "3") && (clsLogin._LoggedUserBranch_IsCharging == Content.vbTrue))
        //    {
        //        ds = new DataTable();
        //        ds = TrxnDB.getDataSet(string.Format("SELECT top 1 Type_Charge_N, Type_Charge_YN_B,Type_Gvt_Levy_N FROM tbl_ZIMRA_PayTypes WITH (NOLOCK) WHERE Type_Code='{0}'", TrnType)).Tables[0];
        //        if (ds.Rows.Count != 0)
        //        {
        //            Is_Charging = bool.Parse((System.Convert.ToBoolean(ds.Rows[0]["Type_Charge_YN_B"])).ToString());
        //            Gvt_Levy_Amnt = double.Parse(ds.Rows[0]["Type_Gvt_Levy_N"].ToString());
        //            Charging_Amnt = System.Convert.ToDouble(ds.Rows[0]["Type_Charge_N"]);
        //        }

        //        if (Is_Charging == true)
        //        {
        //            // Check whether Transaction type is charging
        //            ds = new DataTable();
        //            ds = TrxnDB.getDataSet("SELECT Top 1 Charge_Type_B, Charge_Percent_N , MCA_Charge_Percent_N FROM tbl_SysParam WITH (NOLOCK) WHERE ParamID='CTL'").Tables[0];
        //            if (ds.Rows.Count != 0)
        //            {
        //                Charging_Type = System.Convert.ToBoolean(ds.Rows[0]["Charge_Type_B"]);
        //                Charging_Percent = System.Convert.ToDouble(ds.Rows[0]["Charge_Percent_N"]);
        //                Commission_Amnt = System.Convert.ToDouble(ds.Rows[0]["MCA_Charge_Percent_N"]);
        //            }

        //            // Convert the amount to the Transacting Currency using Exchange Rate
        //            // Gvt Levy is a fixed amount in USD and goes regardless of Charging Type
        //            if (Charging_Type == true)
        //            {
        //               // Charging_Amnt = double.Parse(String.Format(Charging_Amnt * RealExchangeRate, "####0.00"));
        //            }
        //            else
        //            {
        //               // Charging_Amnt = double.Parse(Strings.Format(double.Parse(Trxn_Amount) / 100 * Charging_Percent * RealExchangeRate, "####0.00"));
        //            }

        //            //Tmp_Charge_Class.GvtLevy_Amnt = Strings.Format(Gvt_Levy_Amnt * RealExchangeRate, "####0.00");
        //            //Tmp_Charge_Class.Comm_Amnt = Strings.Format(Commission_Amnt * RealExchangeRate, "####0.00");
        //            // Check if Account is exempt from Charges
        //            // The below is a switch in Finacle.  Compare with MUB
        //            ds = TrxnDB.getDataSet(string.Format("SELECT Non_Charging_Acc_C FROM tbl_Non_Charging WITH (NOLOCK) WHERE Non_Charging_Acc_C ='{0}'", Account_2_Charge)).Tables[0];
        //            if (ds.Rows.Count == 0)
        //            {
        //               // Tmp_Charge_Class.Charge_Amnt = Charging_Amnt;
        //            }
        //            else
        //            {
        //                Tmp_Charge_Class.Charge_Amnt = "0.00";
        //            }
        //        }
        //    }
        //    return Tmp_Charge_Class;
        //}


       

        //Function ValidatePassword(ByVal pwd As String, Optional ByVal minLength As Integer = 8, Optional ByVal numUpper As Integer = 2, Optional ByVal numLower As Integer = 2, Optional ByVal numNumbers As Integer = 2, Optional ByVal numSpecial As Integer = 2) As Boolean

        //    ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
        //    Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
        //    Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
        //    Dim number As New System.Text.RegularExpressions.Regex("[0-9]")
        //    ' Special is "none of the above".
        //    Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

        //    ' Check the length.
        //    If Len(pwd) < minLength Then Return False
        //    ' Check for minimum number of occurrences.
        //    If upper.Matches(pwd).Count < numUpper Then Return False
        //    If lower.Matches(pwd).Count < numLower Then Return False
        //    If number.Matches(pwd).Count < numNumbers Then Return False
        //    If special.Matches(pwd).Count < numSpecial Then Return False

        //    ' Passed all checks.
        //    Return True
        //End Function


        //public static dynamic fn_Save_UserLogging_Detail(string UserId_C, string Login_Success_YN, string login_Details_C, string loginGuid)
        //{

        //    string strHostName = "";
        //    string strIPAddress = "";
        //    strHostName = System.Convert.ToString(System.Net.Dns.GetHostName());
        //    strIPAddress = System.Convert.ToString(System.Net.Dns.GetHostByName(strHostName).AddressList[0].ToString());
        //    try
        //    {
        //        SQLStr = string.Format("[dbo].[ustp_Create_SysLogin_Log] @UserId_C = '{0}', @Host_Name_C = '{1}', @Host_IP_C = '{2}',@Login_Success_YN  = '{3}', @login_Details_C = '{4}', @loginGuid = '{5}'", UserId_C, strHostName, strIPAddress, Login_Success_YN, login_Details_C, loginGuid);

        //        SqlHelper.RunSql(System.Configuration.ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString, SQLStr);
        //    }
        //    catch (Exception)
        //    {
        //        string Saverec = "";
        //    }
        //    return "";
        //}



        //public static dynamic GetCharge_Levy_And_Commission(string TrnType, string Trxn_Amount, double RealExchangeRate, string Account_2_Charge)
        //{
        //    DataTable ds = default(DataTable);
        //    bool Is_Charging = false;
        //    double Gvt_Levy_Amnt = 0;
        //    double Charging_Percent = 0;
        //    bool Charging_Type = false;
        //    double Charging_Amnt = 0;

        //    //if ((TrnType == "3") && (clsLogin._LoggedUserBranch_IsCharging == Constants.vbTrue))
        //    {
        //        ds = new DataTable();
        //        ds = TrxnDB.getDataSet(string.Format("SELECT top 1 Type_Charge_N, Type_Charge_YN_B,Type_Gvt_Levy_N FROM tbl_ZIMRA_PayTypes WITH (NOLOCK) WHERE Type_Code='{0}'", TrnType)).Tables[0];
        //        if (ds.Rows.Count != 0)
        //        {
        //            Is_Charging = bool.Parse((System.Convert.ToBoolean(ds.Rows[0]["Type_Charge_YN_B"])).ToString());
        //            Gvt_Levy_Amnt = double.Parse(ds.Rows[0]["Type_Gvt_Levy_N"].ToString());
        //            Charging_Amnt = System.Convert.ToDouble(ds.Rows[0]["Type_Charge_N"]);
        //        }


        //        if (Is_Charging == true)
        //        {
        //            // Check whether Transaction type is charging
        //            ds = new DataTable();
        //            ds = TrxnDB.getDataSet("SELECT top 1 Charge_Type_B, Charge_Percent_N FROM tbl_SysParam WITH (NOLOCK) WHERE ParamID='CTL'").Tables[0];
        //            if (ds.Rows.Count != 0)
        //            {
        //                Charging_Type = System.Convert.ToBoolean(ds.Rows[0]["Charge_Type_B"]);
        //                Charging_Percent = System.Convert.ToDouble(ds.Rows[0]["Charge_Percent_N"]);
        //            }

        // Convert the amount to the Transacting Currency using Exchange Rate
        // Gvt Levy is a fixed amount in USD and goes regardless of Charging 
        //  Type
        //if (Charging_Type == true)
        //{
        //   // Charging_Amnt = double.Parse(Strings.Format(Charging_Amnt * RealExchangeRate, "####0.00"));
        //}
        //else
        //{
        //   // Charging_Amnt = double.Parse(Strings.Format(double.Parse(Trxn_Amount) / 100 * Charging_Percent * RealExchangeRate, "####0.00"));
        //}

        //Class_Charges.GvtLevyAmnt(Strings.Format(Gvt_Levy_Amnt * RealExchangeRate, "####0.00"));
        // Check if Account is exempt from Charges
        // The below is a switch in Finacle.  Compare with MUB
        // ds = TrxnDB.getDataSet(string.Format("SELECT Non_Charging_Acc_C FROM tbl_Non_Charging WITH (NOLOCK) WHERE Non_Charging_Acc_C ='{0}'", Account_2_Charge)).Tables[0];
        //if (ds.Rows.Count == 0)
        //{
        //  //  Class_Charges.ChargeAmnt(Charging_Amnt);
        //}
        //else
        //{
        //    Class_Charges.ChargeAmnt("0.00");
        //}
        //  }
        //  }
        //else
        //{
        //    Class_Charges.ChargeAmnt("0.00");
        //    Class_Charges.GvtLevyAmnt("0.00");
        //    Class_Charges.CommAmnt("0.00");
        //}
        // return Gvt_Levy_Amnt;
        // }


      


        //public static void Add_AgentDetails(string Tmp_BusPartner, string Cust_Product, string Local_Cust_Name_C, string Local_Cust_Street_C, string Local_Cust_City_C, string Local_Cust_Mobile_C)
        //{
        //    DataTable DS = new DataTable();

        //    DS = new DataTable();
        //    string Trn_Date = TrxnDB.getDataSet("SELECT CONVERT(VARCHAR(10),CURRENT_TIMESTAMP,25) + ' ' + CONVERT(VARCHAR(8),CURRENT_TIMESTAMP,114)").Tables[0].Rows[0][0].ToString();
        //    string SQLSTR = string.Format("SELECT Top 1 Local_Cust_ID_C, Local_Cust_Product_C FROM tbl_LocalCustomer WITH (NOLOCK) WHERE Local_Cust_ID_C ='{0}' AND Local_Cust_Product_C = '{1}'", Tmp_BusPartner, Cust_Product);
        //    DS = TrxnDB.getDataSet(SQLSTR).Tables[0];
        //    if (DS.Rows.Count == 0)
        //    {
        //        // Add the New Record
        //        Hashtable Cust_Hast = new Hashtable();
        //        Cust_Hast.Add("Local_Cust_ID_C", Tmp_BusPartner);
        //        Cust_Hast.Add("Local_Cust_Product_C", Cust_Product);
        //        Cust_Hast.Add("Local_Cust_Name_C", Local_Cust_Name_C);
        //        if (Local_Cust_Street_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_Street_C", Local_Cust_Street_C);
        //        }
        //        if (Local_Cust_City_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_City_C", Local_Cust_City_C);
        //        }
        //        if (Local_Cust_Mobile_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_Mobile_C", Local_Cust_Mobile_C);
        //        }
        //        Cust_Hast.Add("Local_Cust_AddedBy_C", clsLogin._loggedUser);
        //        Cust_Hast.Add("Local_Cust_DateAdded_D", Trn_Date);
        //        TrxnDB.insertSQL("tbl_LocalCustomer", Cust_Hast);
        //    }
        //    else
        //    {
        //        //Edit and Existing Record
        //        Hashtable Cust_Hast = new Hashtable();
        //        Cust_Hast.Add("Local_Cust_ID_C", Tmp_BusPartner);
        //        Cust_Hast.Add("Local_Cust_Product_C", Cust_Product);
        //        Cust_Hast.Add("Local_Cust_Name_C", Local_Cust_Name_C);
        //        if (Local_Cust_Street_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_Street_C", Local_Cust_Street_C);
        //        }
        //        if (Local_Cust_City_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_City_C", Local_Cust_City_C);
        //        }
        //        if (Local_Cust_Mobile_C != "")
        //        {
        //            Cust_Hast.Add("Local_Cust_Mobile_C", Local_Cust_Mobile_C);
        //        }
        //        Cust_Hast.Add("Local_Cust_ModifiedBy_C", clsLogin._loggedUser);
        //        Cust_Hast.Add("Local_Cust_Date_Modified_D", Trn_Date);

        //        Hashtable whereHash = new Hashtable();
        //        whereHash.Add("Local_Cust_ID_C", Tmp_BusPartner);
        //        whereHash.Add("Local_Cust_Product_C", Cust_Product);
        //        TrxnDB.updateSQL("tbl_LocalCustomer", Cust_Hast, whereHash);
        //    }
        //}


        //public static Trn_Narr1_Narr2_AndRef fn_CreateNarratives(string DR_Parm_TrnDesc, string DR_Parm_TrnRef, string CR_Parm_TrnDesc, string CR_Parm_TrnRef)
        //{

        //    Trn_Narr1_Narr2_AndRef Local_Trn_Narr1_Narr2_Ref = new Trn_Narr1_Narr2_AndRef();

        //    string DR_Local_Narr1 = "";
        //    string DR_Local_Narr2 = "";
        //    string DR_Local_Ref = DR_Parm_TrnRef;

        //    string CR_Local_Narr1 = "";
        //    string CR_Local_Narr2 = "";
        //    string CR_Local_Ref = CR_Parm_TrnRef;


        //    //Trn Field	        Finacle Field
        //    //DR Reference	    DrTranRemarks1 ----- (30 chars)
        //    //DR Narrative(s)	DrTranParticulars--- (50 chars) & DrTranRemarks2 - (50 chars)

        //    //CR Reference	   CrTranRemarks1 ----- (30 chars)
        //    //CR Narrative(s)   CrTranParticulars--- (50 chars) & CrTranRemarks2 - (50 chars)

        //    Local_Trn_Narr1_Narr2_Ref.Initialise_Details();
        //    short Narr1Length = (short)30; // Transaction References is 30 Characters
        //    short Narr2Length = (short)50; // Each of the Narratives

        //    if (DR_Local_Ref.Length > Narr1Length)
        //    {
        //        DR_Local_Ref = DR_Local_Ref.Substring(0, Narr1Length);
        //    }

        //    if (CR_Local_Ref.Length > Narr1Length)
        //    {
        //        CR_Local_Ref = CR_Local_Ref.Substring(0, Narr1Length);
        //    }

        //    if (DR_Parm_TrnDesc.Length <= Narr2Length)
        //    {
        //        DR_Local_Narr1 = DR_Parm_TrnDesc;
        //    }
        //    else
        //    {
        //        DR_Local_Narr1 = DR_Parm_TrnDesc.Substring(0, Narr2Length);
        //        DR_Local_Narr2 = DR_Parm_TrnDesc.Substring(Narr2Length, DR_Parm_TrnDesc.Length - Narr2Length);
        //        if (DR_Local_Narr2.Length > Narr2Length)
        //        {
        //            DR_Local_Narr2 = DR_Local_Narr2.Substring(0, Narr2Length);
        //        }
        //    }

        //    if (CR_Parm_TrnDesc.Length <= Narr2Length)
        //    {
        //        CR_Local_Narr1 = CR_Parm_TrnDesc;
        //    }
        //    else
        //    {
        //        CR_Local_Narr1 = CR_Parm_TrnDesc.Substring(0, Narr2Length);
        //        //CR_Local_Narr2 = CR_Parm_TrnDesc.Substring(Narr2Length, CR_Parm_TrnDesc.Length - Narr2Length - 1)
        //        CR_Local_Narr2 = CR_Parm_TrnDesc.Substring(Narr2Length, CR_Parm_TrnDesc.Length - Narr2Length);
        //        if (CR_Local_Narr2.Length > Narr2Length)
        //        {
        //            CR_Local_Narr2 = CR_Local_Narr2.Substring(0, Narr2Length);
        //        }
        //    }

        //    Local_Trn_Narr1_Narr2_Ref.DR_Trn_Reference = DR_Local_Ref;
        //    Local_Trn_Narr1_Narr2_Ref.DR_Trn_Narrative1 = DR_Local_Narr1;
        //    Local_Trn_Narr1_Narr2_Ref.DR_Trn_Narrative2 = DR_Local_Narr2;

        //    Local_Trn_Narr1_Narr2_Ref.CR_Trn_Reference = CR_Local_Ref;
        //    Local_Trn_Narr1_Narr2_Ref.CR_Trn_Narrative1 = CR_Local_Narr1;
        //    Local_Trn_Narr1_Narr2_Ref.CR_Trn_Narrative2 = CR_Local_Narr2;

        //    return Local_Trn_Narr1_Narr2_Ref;

        //}

        ////public static Finacle_Bridge.Finacle_Response_Detail fn_Post_Finacle_Transaction(string Param_SR_UserID, string Param_SR_Password, string Param_channelId, string Param_RequestUUID, string Parm_ChannelRefNum, bool Parm_ReversalFlag, string Parm_OriginalChannelRefNum, string Parm_TranAmount, string Parm_TranCrncy, string Parm_DrAcctNo, string Parm_CrAcctNo, string Parm_CrValueDate, string Parm_DrBICCode, string Parm_CrBICCode, string Parm_DrAcctName, string Parm_DrAcctAddress1, Trn_Narr1_Narr2_AndRef ParmTrnDet, string Parm_TranParticularsCodeDr, string Parm_TranParticularsCodeCr, string Parm_SourceChannel)
        //{


        string HostTransaction_Status = "";
        string Parm_ChannelRefNum_Resp = "";
        string Resp_Code = "";
        string Resp_Remarks = "";
        string Off_UsRefNum = "";


        //Finacle_Bridge.Finacle_BridgeSoapClient Fin_Bridge_SC = new Finacle_Bridge.Finacle_BridgeSoapClient();
        //Finacle_Bridge.Finacle_Response_Detail Fin_Response = new Finacle_Bridge.Finacle_Response_Detail();

        //try
        //{
        //Fin_Request_Body.SR_UserID = Param_SR_UserID
        //Fin_Request_Body.SR_Password = Param_SR_Password
        //Fin_Request_Body.Param1_channelId = Param_channelId
        //Fin_Request_Body.Param1_RequestUUID = Param_RequestUUID
        //Fin_Request_Body.Parm1_ChannelRefNum = Parm_ChannelRefNum
        //Fin_Request_Body.Parm1_ReversalFlag = Parm_ReversalFlag
        //Fin_Request_Body.Parm1_OriginalChannelRefNum = Parm_OriginalChannelRefNum
        //Fin_Request_Body.Parm1_TranAmount = Parm_TranAmount
        //Fin_Request_Body.Parm1_TranCrncy = Parm_TranCrncy
        //Fin_Request_Body.Parm1_DrAcctNo = Parm_DrAcctNo
        //Fin_Request_Body.Parm1_CrAcctNo = Parm_CrAcctNo
        //Fin_Request_Body.Parm1_CrValueDate = Parm_CrValueDate
        //Fin_Request_Body.Parm1_DrBICCode = Parm_DrBICCode
        //Fin_Request_Body.Parm1_CrBICCode = Parm_CrBICCode
        //Fin_Request_Body.Parm1_DrAcctName = Parm_DrAcctName
        //Fin_Request_Body.Parm1_DrAcctAddress1 = Parm_DrAcctAddress1

        //Fin_Request_Body.Parm1_DrTranRemarks1 = ParmTrnDet.DR_Trn_Reference
        //Fin_Request_Body.Parm1_DrTranParticulars = ParmTrnDet.DR_Trn_Narrative1
        //Fin_Request_Body.Parm1_DrTranRemarks2 = ParmTrnDet.DR_Trn_Narrative2

        //Fin_Request_Body.Parm1_CrTranRemarks1 = ParmTrnDet.CR_Trn_Reference
        //Fin_Request_Body.Parm1_CrTranParticulars = ParmTrnDet.CR_Trn_Narrative1
        //Fin_Request_Body.Parm1_CrTranRemarks2 = ParmTrnDet.CR_Trn_Narrative2

        //Fin_Request_Body.Parm1_TranParticularsCodeDr = Parm_TranParticularsCodeDr
        //Fin_Request_Body.Parm1_TranParticularsCodeCr = Parm_TranParticularsCodeCr
        //Fin_Request_Body.Parm1_SourceChannel = Parm_SourceChannel
        //Fin_Response = Fin_Bridge_SC.PostToFinacle(Param_SR_UserID, Param_SR_Password, Param_channelId, Param_RequestUUID, Parm_ChannelRefNum, Parm_ReversalFlag, Parm_OriginalChannelRefNum, Parm_TranAmount, Parm_TranCrncy, Parm_DrAcctNo, Parm_CrAcctNo, Parm_CrValueDate, Parm_DrBICCode, Parm_CrBICCode, Parm_DrAcctName, Parm_DrAcctAddress1, ParmTrnDet.DR_Trn_Reference, ParmTrnDet.DR_Trn_Narrative1, ParmTrnDet.DR_Trn_Narrative2, ParmTrnDet.CR_Trn_Reference, ParmTrnDet.CR_Trn_Narrative1, ParmTrnDet.CR_Trn_Narrative2, Parm_TranParticularsCodeDr, Parm_TranParticularsCodeCr, Parm_SourceChannel);

        //    }
        //    catch (Exception Posting_Ex)
        //    {
        //        Log_to_Error("22222222", Posting_Ex, "Post Finacle Transaction Inner", Param_RequestUUID);

        //    }
        //    finally
        //    {
        //        // Return Tmp_Finacle_Response_Detail
        //    }
        //    return Fin_Response;
        //}



        //public static dynamic Create_Finacle_ReqUUID()
        //{
        //    Random rng = new Random();
        //    int tmp_number = System.Convert.ToInt32(rng.Next(1, 1000000000));
        //    string sGUID = string.Format("SEL{0}{1}", DateTime.Now.ToString("yyMMddHHmmssffft"), tmp_number.ToString("0000000000"));

        //    int sGiudOccurs = int.Parse(SqlHelper.GetTable(System.Configuration.ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString,     string.Format("SELECT count(*) from [tbl_RequestUUID] WHERE [RequestUUID] = '{0}' and CONVERT(VARCHAR(10),[RequestUUID_Dt],25) = CONVERT(VARCHAR(10),CURRENT_TIMESTAMP,25)", sGUID)).Rows[0][0].ToString());
        //   // int sGiudOccurs = int.Parse(SqlHelper.RunSql(string.Format("SELECT count(*) from [tbl_RequestUUID] WHERE [RequestUUID] = '{0}' and CONVERT(VARCHAR(10),[RequestUUID_Dt],25) = CONVERT(VARCHAR(10),CURRENT_TIMESTAMP,25)", sGUID)).ToString());

        //    if (sGiudOccurs == 0)
        //    {

        //        // TrxnDB.exec(string.Format("INSERT INTO [tbl_RequestUUID]([RequestUUID], [RequestUUID_Dt]) SELECT '{0}', CURRENT_TIMESTAMP", sGUID));
        //        string SQLStr= (string.Format("INSERT INTO [tbl_RequestUUID]([RequestUUID], [RequestUUID_Dt]) SELECT '{0}', CURRENT_TIMESTAMP", sGUID));
        //        SqlHelper.RunSql(System.Configuration.ConfigurationManager.ConnectionStrings["everifyDBConnection"].ConnectionString, SQLStr);

        //    }
        //    else
        //    {
        //        sGUID = "0";
        //    }
        //    return sGUID;
        //}


        //public static dynamic fn_Create_Finacle_ChannelID()
        //{
        //    Random rng = new Random();
        //    int tmp_number = System.Convert.ToInt32(rng.Next(1, 1000000000));
        //    string sGUID = string.Format("ST{0:yyMMddHH}{1}", DateTime.Now, tmp_number.ToString("0000000000"));

        //    int sGiudOccurs = int.Parse(TrxnDB.getDataSet(string.Format("SELECT count(*) from tbl_ChannelRefNum WHERE ChannelRefNum = '{0}' and CONVERT(VARCHAR(10),ChannelRefNum_Dt,25) = CONVERT(VARCHAR(10),CURRENT_TIMESTAMP,25)", sGUID)).Tables[0].Rows[0][0].ToString());
        //    if (sGiudOccurs == 0)
        //    {
        //        TrxnDB.exec(string.Format("INSERT INTO tbl_ChannelRefNum(ChannelRefNum, ChannelRefNum_Dt) SELECT '{0}', CURRENT_TIMESTAMP", sGUID));
        //    }
        //    else
        //    {
        //        sGUID = "0";
        //    }
        //    return sGUID;
        //}


        //public static dynamic fn_Get_Finacle_ChannelID(string Current_TrnUniqueID, string Current_Product_ID)
        //{
        //    string tmp_ChannelRefNum = "0";
        //    tmp_ChannelRefNum = "0";
        //    var tmp_XML_Request_Rec = new DataTable();

        //    SQLStr = string.Format("SELECT TOP 1 * FROM dbo.tbl_XML_log where XML_Ref_C = '{0}' and XML_Type_C = 'Request' and Source_ID_C = '{1}'  ORDER BY 1 DESC", Current_TrnUniqueID, Current_Product_ID);
        //    tmp_XML_Request_Rec = TrxnDB.getDataSet(SQLStr).Tables[0];
        //    if (tmp_XML_Request_Rec.Rows.Count > 0)
        //    {
        //        foreach (DataRow tmp_XML_Request_Row in tmp_XML_Request_Rec.Rows)
        //        {
        //            tmp_ChannelRefNum = System.Convert.ToString(tmp_XML_Request_Row["ChannelRefNum_C"].ToString().Trim());
        //        }
        //    }
        //    else
        //    {
        //        while (tmp_ChannelRefNum == "0")
        //        {
        //            tmp_ChannelRefNum = System.Convert.ToString(fn_Create_Finacle_ChannelID());
        //        }
        //    }
        //    return tmp_ChannelRefNum;
        //}


        ////public static Finacle_Bridge.Account_Enquiry_Detail fn_Finacle_Account_Details_Request(string Param_AccountNumber, bool UseTestResponse)
        //{

        // Posting Contract
        //Finacle_Bridge.Account_Enquiry_Detail Fin_Response = new Finacle_Bridge.Account_Enquiry_Detail();
        //string HostTransaction_Status = "";
        //string Parm_ChannelRefNum_Resp = "";
        //string Resp_Code = "";
        //string Resp_Remarks = "";
        //string Off_UsRefNum = "";


        //Dim sGUID As String
        //sGUID = System.Guid.NewGuid.ToString()
        //MessageBox.Show(sGUID)

        //string sGUID = System.Convert.ToString(Create_Finacle_ReqUUID());

        //try
        //{

        //    Finacle_Bridge.Finacle_BridgeSoapClient Fin_Bridge_SC = new Finacle_Bridge.Finacle_BridgeSoapClient();
        //    Finacle_Bridge.Account_Enquiry_Detail AccountDet_Request = new Finacle_Bridge.Account_Enquiry_Detail();


        //    try
        //    {
        //        //AccountDet_Request_Body.Param_Account = Param_AccountNumber
        //        //AccountDet_Request_Body.Param_RequestUUID = sGUID
        //        //AccountDet_Request_Body.UsePseudoResponse = UseTestResponse

        //        //AccountDet_Request.Body = AccountDet_Request_Body
        //        Fin_Response = Fin_Bridge_SC.Finacle_AccountEnquiry(Param_AccountNumber, sGUID, UseTestResponse, "EVF");

        //}
        //catch (Exception Posting_Ex)
        //{
        //    Log_to_Error("22228822", Posting_Ex, "Finacle Balance Enquiry Inner", sGUID);
        //}
        //    }
        //    catch (Exception Posting_Main_Ex)
        //    {
        //        //Tmp_Finacle_Response_Detail.HostTransaction_Status = "FAILURE"
        //        //Tmp_Finacle_Response_Detail.Resp_Code = Posting_Main_Ex.StackTrace.ToString
        //        //Tmp_Finacle_Response_Detail.Resp_Remarks = "VB Handled Error"
        //        Log_to_Error("22228823", Posting_Main_Ex, "Finacle Balance Enquiry Outer", sGUID);
        //    }
        //    finally
        //    {
        //        // Return Tmp_Finacle_Response_Detail
        //    }
        //    return Fin_Response;
        //}


        //public static dynamic GetCustomerName_from_Finacle(string AccountNum, string Search_Product)
        //{
        //    string TmpAccName = "";
        //    try
        //    {
        //        bool UseTestResponse = System.Convert.ToBoolean(TrxnDB.getDataSet("SELECT  Top 1 UseFinaclePseudoResponse_B FROM tbl_SysParam WITH (NOLOCK) WHERE ParamID = 'CTL'").Tables[0].Rows[0][0]);
        //        //var Tmp_Fin_Account_Resp = new Finacle_Bridge.Account_Enquiry_Detail();
        //        //Tmp_Fin_Account_Resp = fn_Finacle_Account_Details_Request(AccountNum, UseTestResponse);
        //        //if (Tmp_Fin_Account_Resp.ACCTNAME != "")
        //        //{
        //        //    TmpAccName = System.Convert.ToString(Tmp_Fin_Account_Resp.ACCTNAME);
        //        //}
        //    }
        //    catch (Exception Posting_Main_Ex)
        //    {
        //        TmpAccName = "";
        //        Log_to_Error("22228823", Posting_Main_Ex, string.Format("Finacle Acc. Search - {0} :{1} ", Search_Product, clsLogin._loggedUser), AccountNum);
        //    }
        //    finally
        //    {
        //        // Return Tmp_Finacle_Response_Detail
        //    }
        //    return TmpAccName;
        //}

        ////Public Function fn_Post_Finacle_Transaction(
        //                                       ByVal Param_channelId As String,
        //                                       ByVal Param_RequestUUID As String,
        //                                       ByVal Parm_ChannelRefNum As String,
        //                                       ByVal Parm_ReversalFlag As Boolean,
        //                                       ByVal Parm_OriginalChannelRefNum As String,
        //                                       ByVal Parm_TranAmount As String,
        //                                       ByVal Parm_TranCrncy As String,
        //                                       ByVal Parm_DrAcctNo As String,
        //                                       ByVal Parm_CrAcctNo As String,
        //                                       ByVal Parm_CrValueDate As String,
        //                                       ByVal Parm_DrBICCode As String,
        //                                       ByVal Parm_CrBICCode As String,
        //                                       ByVal Parm_DrAcctName As String,
        //                                       ByVal Parm_DrAcctAddress1 As String,
        //                                       ByVal Parm_DrTranParticulars As String,
        //                                       ByVal Parm_DrTranRemarks1 As String,
        //                                       ByVal Parm_CrTranParticulars As String,
        //                                       ByVal Parm_CrTranRemarks1 As String,
        //                                       ByVal Parm_TranParticularsCodeDr As String,
        //                                       ByVal Parm_TranParticularsCodeCr As String
        //                                       ) As Finacle_Response_Detail



        //                If Tmp_Response_XML.SelectSingleNode("/FIXML/Body/executeFinacleScriptResponse/executeFinacleScript_CustomData/Tran_RES/OffUsRefNum") Is Nothing = False Then
        //                    Off_UsRefNum = Tmp_Response_XML.SelectSingleNode("/FIXML/Body/executeFinacleScriptResponse/executeFinacleScript_CustomData/Tran_RES/OffUsRefNum").InnerText
        //                End If
        //                Tmp_Finacle_Response_Detail.HostTransaction_Status = HostTransaction_Status
        //                Tmp_Finacle_Response_Detail.Parm_ChannelRefNum_Resp = Parm_ChannelRefNum_Resp
        //                Tmp_Finacle_Response_Detail.Resp_Code = Resp_Code
        //                Tmp_Finacle_Response_Detail.Resp_Remarks = Resp_Remarks
        //                Tmp_Finacle_Response_Detail.Off_UsRefNum = Off_UsRefNum
        //            End If
        //        Catch Posting_Ex As Exception
        //            Log_to_Error("22222222", Posting_Ex, "Post Finacle Transaction Inner", Param_RequestUUID)
        //        End Try

        //        '  Mark - UmMark _Lien
        //        'Dim Fin_SC As New Finacle_Posting_SOAP.PS_Finacle_BlockUnblockFundsV1MaintainTransactionStatusClient()
        //        'Dim Fin_Request As New Finacle_Posting_SOAP.MaintainTransactionStatusRequest()
        //        'Dim Fin_Response As New Finacle_Posting_SOAP.MaintainTransactionStatusResponse()
        //        'Dim Fin_Response_Info As String

        //        '' Reauest Header
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.correlationId = "test123"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.countryISO = "ZW"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.messageId = "MESSAGE1"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.messageType = "tYPE 1"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.sourceSystem = "eVerify"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.trackingId = "rerereere"
        //        'Fin_Request.MaintainTransactionStatus.requestHeader.version = "001"

        //        ''Fin_Request Body
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.acid = "024005633"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.blockAmount.amountValue = 500
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.blockid = "024005633"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.branchId = "024005633"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.Fees.feesamount.amountValue = 10
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.lienExpDate = CDate("2016-01-23")
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.lienExpDateSpecified = True
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.localTxnDate = CDate("2016-01-23")
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.modifyIndicator = "024005633"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.remark = "Test Transaction"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.reversalIndicatorFlag = False
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.stan = "rerere"
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.valueDate = CDate("2016-01-23")
        //        'Fin_Request.MaintainTransactionStatus.MaintainTransactionStatusReq.Body.blockModifyRequest.BlockModifyInputVO.valueDateSpecified = True

        //        'Fin_Response = Fin_SC.Finacle_Posting_PS_Finacle_BlockUnblockFundsV1MaintainTransactionStatus_MaintainTransactionStatus(Fin_Request)

        //        '' GetConn Success pof Failure

        //        'Fin_Response_Info = Fin_Response.MaintainTransactionStatusResponse1.responseHeader.ResponseMessageInfo.ToString



        //        'Dim Lic_Response_Code As String

        //        'Lic_Response_Code = Check_License("F576F594Z681J774E579A582H684M729D582D576X579Q594D639P630V6663544", 64, "EBN", "2015-12-03")

        //        'Dim RecTime As String = String.Format("{0:yyyyMMddHHmmssfff}", DateTime.Now)
        //        'Dim Katsstrr As String = RandomString(10, 45).ToLower
        //        'MsgBox("Kats """)
        //        'MsgBox(Replace("Kats""", """", "``"))
        //        'GetTransactionCodes()
        //        'Dim KatsObj As Object = Convert.ToChar(28).ToString
        //        'Tag_Separator.SetValue(KatsObj, 0)

        //        'Dim Kats As String = ""
        //        'Kats = GetHashValue_XML("D02220729701025.00BARCLAYS BANK Split - (1 Trxn)")
        //        'Kats = GetHashValue_XML("C0222096293701793.00Admed - Blanckenberg63")
         //    ''Dim PostTime As String = CheckDBNull(TrxnDB.getDataSet("SELECT 'ttt' Where 1 = 9").Tables[0].Rows[0][0).ToString)

        //        'Dim RejectTime As String = Format(CDate(TrxnDB.getDataSet("SELECT CURRENT_TIMESTAMP AS PostTime").Tables[0].Rows[0][0)), "yyMMddHHmmm")
        //        'Dim KatsStr As String = "260062147`"
        //        'KatsStr = RemoveSWIFTChars(KatsStr)
        //        'MsgBox(KatsStr)
        //    Catch Posting_Main_Ex As Exception
        //        Tmp_Finacle_Response_Detail.HostTransaction_Status = "FAILURE"
        //        Tmp_Finacle_Response_Detail.Resp_Code = Posting_Main_Ex.StackTrace.ToString
        //        Tmp_Finacle_Response_Detail.Resp_Remarks = "VB Handled Error"
        //        Log_to_Error("22222223", Posting_Main_Ex, "Post  Finacle Transaction Out", Param_RequestUUID)
        //    Finally
        //        ' Return Tmp_Finacle_Response_Detail
        //    End Try
        //    Return Tmp_Finacle_Response_Detail
        //End Function

        private static string Request_XML()
        {
            throw (new NotImplementedException());
        }

        //static dynamic CheckDBNull
        //public string RemoveChars(string InStr)
        //{

        //   // The procedure leaves only numbers and alphabet and removes special characters.
        //    string TempStr = InStr;
        //   //var xmlPattern = "[^\u0001-\uD7FF\uE000-\uFFFD\ud800\udc00-\udbff\udfff]";
        //   TempStr = Regex.Replace(TempStr, @"[^\u0001-\uD7FF\uE000-\uFFFD\ud800\udc00-\udbff\udfff]",string.Empty); 
          
        //    return TempStr;
        //}



        public string RemoveInvalidXmlChars(string text)
        {
            var validXmlChars = text.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray();
            return new string(validXmlChars);
        }

        public static bool IsValidXmlString(string text)
        {
            try
            {
                XmlConvert.VerifyXmlChars(text);
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}