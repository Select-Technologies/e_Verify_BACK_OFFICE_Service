using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace e_Verify_BACK_OFFICE_Service
{
    public static class Utilities
    {
        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

        public static Random random = new Random((int)DateTime.Now.Ticks);
        public static object locker = new object();

        public static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                lock (locker)
                {
                    ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                }
                builder.Append(ch);
            }

            return builder.ToString().ToLower();
        }
        public static string SerializeToXML_No_xlmns<T>(T dataToSerialize)
        {
            XElement xmlTree;
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer   = new XmlSerializer(typeof(T));
                serializer.Serialize(stringwriter, dataToSerialize);
                string outstring = System.Text.RegularExpressions.Regex.Replace(stringwriter.ToString(), @"\s{2,}", " ").Replace("\n", "");

                xmlTree = XElement.Parse(outstring);
                outstring = RemoveAllNamespaces(xmlTree).ToString();
                return outstring;
            }
            catch
            {
                throw;
            }
        }

        public static XElement RemoveAllNamespaces(XElement e)
        {
            return new XElement(e.Name.LocalName,
              (from n in e.Nodes()
               select ((n is XElement) ? RemoveAllNamespaces(n as XElement) : n)),
                  (e.HasAttributes) ?
                    (from a in e.Attributes()
                     where (!a.IsNamespaceDeclaration)
                     select new XAttribute(a.Name.LocalName, a.Value)) : null);
        }

        public static string connect2PortandIP_PureText(string serverIP, Int32 port, string message, string messageType, bool EnableDebugging)
        {
            string output = "";
            const int bytesize = 1024 * 1024;
            byte[] receivedData = new byte[message.Length + 1000];
            Byte[] data = new byte[message.Length];
            Byte[] data1 = new byte[1];
            Byte[] data2 = new byte[1];

            string data_str = "";
            int looptimes = 0;
            string result = "";
            string HexString = "";
            string responseData = "";
            string tmpresponseData = "";

            try
            {
                LogErrorXMLFile("Request1", message, serverIP, port.ToString(), serverIP, true);

                //TcpClient client         = new TcpClient(serverIP, port);

                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //clientSocket.Connect(new IPEndPoint(Dns.GetHostEntry(serverIP).AddressList[0], port));
                clientSocket.Connect(serverIP, port);

                clientSocket.SendTimeout       = 50000;
                clientSocket.ReceiveTimeout    = 50000;
                clientSocket.ReceiveBufferSize = 64000;
                clientSocket.SendBufferSize    = 64000;

                LogErrorXMLFile("Request1_1", message, serverIP, port.ToString(), serverIP, EnableDebugging);

                Int32 divident = message.Length;
                Int32 divisor = 256;

                Int32 Quotient = divident / divisor;
                Int32 Remainder = divident % divisor;

                data_str = string.Format("{0}", message);

                LogErrorXMLFile("Request1_11", string.Format("{0}-{1}", Quotient, Remainder), serverIP, port.ToString(), serverIP, EnableDebugging);

                if (messageType.ToUpper() == "STRING")
                {
                    data = System.Text.Encoding.ASCII.GetBytes(message);
                }

                if (messageType.ToUpper() == "XML")
                {
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    data = encoding.GetBytes(data_str);
                    data1 = BitConverter.GetBytes(Quotient);
                    data2 = BitConverter.GetBytes(Remainder);
                }
                //Save the Binary Pure text to File_History ON eVerify
                if (EnableDebugging)
                {
                    Hashtable binHash = new Hashtable();
                    binHash.Add("XML_Ref_C"      , string.Format("{0}:{1}", serverIP, port));
                    binHash.Add("XML_Date_D"     , string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                    binHash.Add("Source_ID_C"    , messageType);
                    binHash.Add("ChannelRefNum_C", string.Format("{0}:{1}", serverIP, port));
                    binHash.Add("XML_String_C"   , data_str);
                    binHash.Add("XML_Type_C"     , "Request3");

                    result = SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "tbl_xml_Log", binHash);

                    //Now save Hex Representation
                    data_str = BitConverter.ToString(data);
                    binHash.Remove("XML_String_C");
                    binHash.Remove("XML_Type_C");

                    binHash.Add("XML_String_C", data_str);
                    binHash.Add("XML_Type_C", "Request4");
                    result = SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "tbl_xml_Log", binHash);
                }

                receivedData = new byte[data.Length + 1000];
                using (Stream netStream = new NetworkStream(clientSocket, true), bufStream = new BufferedStream(netStream, data.Length + 1000))
                {
                    netStream.ReadTimeout = 30000;
                    // Check whether the underlying stream supports seeking.
                    if (bufStream.CanWrite)
                    {
                        bufStream.Write(data1, 0, 1);
                        bufStream.Write(data2, 0, 1);
                        bufStream.Write(data, 0, data.Length);
                        bufStream.Flush();
                    }
                    else
                    {
                        LogErrorXMLFile("12", "Buffer.CanWrite is False", serverIP, port.ToString(), serverIP, true);
                    }

                    LogErrorXMLFile("Request1_41", "Data Saved to IP/Port", serverIP, port.ToString(), serverIP,true);
                    int numBytesToRead = data.Length;

                    //System.Threading.Thread.Sleep(1500);
                    looptimes = 0;
                    while (true)
                    {
                        if (looptimes == 0) LogErrorXMLFile("Request1_42", "Looping for Response - Entry ", serverIP, port.ToString(), serverIP, EnableDebugging);

                        bufStream.Read(receivedData, 0, receivedData.Length);
                        tmpresponseData = System.Text.Encoding.ASCII.GetString(receivedData, 0, receivedData.Length);
                        tmpresponseData = cleanMessage(WebUtility.HtmlDecode(tmpresponseData).Replace(@"\0", ""));

                        tmpresponseData = Remove_XML_SpecialCharacters(tmpresponseData);
                        tmpresponseData = tmpresponseData.Replace("MDE", "").Replace("", "").Replace("", "").Replace("", "");

                        responseData += tmpresponseData;

                        if (looptimes == 0) LogErrorXMLFile("Request1_43", "Looping for Response - Entry ", serverIP, port.ToString(), serverIP, EnableDebugging);

                        //if (responseData.EndsWith("</Iso8583PostXml>"))
                        if (responseData.EndsWith("#R##"))
                        {
                            LogErrorXMLFile("Request1_421", "Looping for Response - Exiting", serverIP, port.ToString(), serverIP, EnableDebugging);
                            break;
                        }
                        looptimes++;
                    }
                    bufStream.Close();
                }
                clientSocket.Close();

                //string ByteStream_Str = "";
                //ByteStream_Str += "041D3C3F786D6C2076657273696F6E3D22312E302220656E636F64696E673D225554462D38223F3E0A3C49736F38353833506F7374586D6C3E3C4D7367547970653E303630303C2F4D7367547970653E3C4669656C64733E3C4669656C645F3030323E353032313935383838303030353335333C2F4669656C645F3030323E3C4669656C645F3030333E3931303030303C2F4669656C645F3030333E3C4669656C645F3030373E303232303136323533353C2F4669656C645F3030373E3C4669656C645F3031313E3031323239363C2F4669656C645F3031313E3C4669656C645F3031323E3136323533353C2F4669656C645F3031323E3C4669656C645F3031";
                //ByteStream_Str += "333E303232303C2F4669656C645F3031333E3C4669656C645F3031343E343931323C2F4669656C645F3031343E3C4669656C645F3031353E303232303C2F4669656C645F3031353E3C4669656C645F3031383E363031323C2F4669656C645F3031383E3C4669656C645F3032323E3030303C2F4669656C645F3032323E3C4669656C645F3032353E30303C2F4669656C645F3032353E3C4669656C645F3033323E3530323139353C2F4669656C645F3033323E3C4669656C645F3033373E3030303030323633383337393C2F4669656C645F3033373E3C4669656C645F3034313E53544557443030313C2F4669656C645F3034313E3C4669656C645F3034323E";
                //ByteStream_Str += "3530323831303030303030303030313C2F4669656C645F3034323E3C4669656C645F3034333E537465776172642042616E6B205465737420312020202020202020202020202020202020202020203C2F4669656C645F3034333E3C4669656C645F3034353E36353736353837367836373C2F4669656C645F3034353E3C4669656C645F3035363E393030323C2F4669656C645F3035363E3C4669656C645F3035393E303030393338313634333C2F4669656C645F3035393E3C4669656C645F3130323E323030303030353335333C2F4669656C645F3130323E3C4669656C645F3132333E3231313230313231333134343030323C2F4669656C645F3132333E3C";
                //ByteStream_Str += "4669656C645F3132375F3030323E303030393338313634333C2F4669656C645F3132375F3030323E3C4669656C645F3132375F3030333E5465737453696D312020202054657374536E6B2020202020303030303032303132323936495361766547726F757020203C2F4669656C645F3132375F3030333E3C4669656C645F3132375F3032323E3231316D6573736167657479706532313542414E4B494E475F5345525649434531344D53444E3231323236333737373230333639333231345661734170706C69636174696F6E31344D504F533C2F4669656C645F3132375F3032323E3C4669656C645F3132375F3033333E333330303C2F4669656C645F313237";
                //ByteStream_Str += "5F3033333E3C2F4669656C64733E3C2F49736F38353833506F7374586D6C3E";
                //string Outtext = Hex_to_String(ByteStream_Str);

                LogErrorXMLFile("Request4_4", string.Format("looptime = {0}", looptimes), serverIP, port.ToString(), serverIP, EnableDebugging);

                LogErrorXMLFile("Request5", responseData, serverIP, port.ToString(), serverIP, true);

                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                //MessageBox.Show(output);
                Log_to_Error("158018", e, "connect2PortandIP1", message);
                return e.Message;
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                // MessageBox.Show(output);
                Log_to_Error("158019", e, "connect2PortandIP2", message);
                return e.Message;
            }
        }

        public static void LogToFile(string LogStr)
        {
            string OutFolder = e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.ErrorLog_Folder;
            if (!(OutFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))) OutFolder = OutFolder + System.IO.Path.DirectorySeparatorChar.ToString();
            bool Allow_Logging = e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Enable_Service_Logging;
            if (Allow_Logging)
            {
               File.AppendAllText(string.Format("{0}SelTech_stp_{1:yyyy_MM_dd}.txt", OutFolder, DateTime.Now), string.Format("{2}{0:yyyy/MM/dd HH:mm:ss}  {1}", DateTime.Now, LogStr, Environment.NewLine));
            }
        }

        public static void LogToFile(string LogStr, bool Logit)
        {
            string OutFolder =  e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.ErrorLog_Folder;
            if (!(OutFolder.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))) OutFolder = OutFolder + System.IO.Path.DirectorySeparatorChar.ToString();
            if (Logit)
            {
                File.AppendAllText(string.Format("{0}SelTech_stp_{1:yyyy_MM_dd}.txt", OutFolder, DateTime.Now), string.Format("{2}{0:yyyy/MM/dd HH:mm:ss}  {1}", DateTime.Now, LogStr, Environment.NewLine));
            }
        }



        public static string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }

        public static string LogErrorXMLFile(string XML_Type_C, string XML_String_C, string XML_Ref_C, string Source_ID_C, string ChannelRefNum_C, bool Logit)
        {
            try
            {
                if (!Logit)
                {
                    return "Logging not required";
                }
                {
                    string storedProcedure = "usp_LogXML";
                    List<SqlParameter>   p = new List<SqlParameter>();
                    p.Add(new SqlParameter("@XML_Type_C"     , XML_Type_C));
                    p.Add(new SqlParameter("@XML_String_C"   , XML_String_C));
                    p.Add(new SqlParameter("@XML_Ref_C"      , XML_Ref_C));
                    p.Add(new SqlParameter("@Source_ID_C"    , Source_ID_C));
                    p.Add(new SqlParameter("@ChannelRefNum_C", ChannelRefNum_C));

                    //string result = SqlHelper.ExecuteScalar(Config.DBConnection, storedProcedure, p.ToArray());
                    string result = SqlHelper.ExecuteScalar(storedProcedure, p.ToArray());
                    if (string.IsNullOrEmpty(result)) return "-1";
                    else return result;
                }
            }
            catch (Exception exception)
            {
                // Handle database errors
                return "-1";
            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb1 = new StringBuilder();
            int AsciiValue;
            foreach (char c in str)
            {
                AsciiValue = Convert.ToInt32(c);
                if ((AsciiValue >= 32) && (AsciiValue <= 126) || ((AsciiValue == 10) || (AsciiValue == 13) || (AsciiValue == 27)))
                {
                    sb1.Append(c);
                }
            }
            return sb1.ToString();
        }


        public static string RemoveDuplicate_Words(string Input_Str, int MaxLength)
        {
            string temp_str = string.Empty;
            try
            {
                {
                    var sent = Input_Str
                        .Split(' ')
                        .Distinct();
                    //.OrderBy(x >= x);

                    foreach (string s in sent)
                    {
                        //Console.WriteLine(s.ToLower());
                        if (s.Trim() != "")
                        {
                            temp_str += s.Trim() + " ";
                        }
                    }
                }
                //return temp_str;
            }
            catch (IOException ex)
            {
                // Do Nothing here
            }
            if (MaxLength > 0)
            {
                if (temp_str.Length > MaxLength)
                {
                    temp_str = temp_str.Substring(0, MaxLength);
                }
            }
            return temp_str;
        }


        public static string RemoveSpecialCharacters_SWIFT(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == '.') ||  (c == ' '))
                {
                   sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        public static string cleanMessage(string message)
        {
            //string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        public static bool IsNumeric(string text)
        {
            double test;
            return double.TryParse(text, out test);
        }
    
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static string fn_getField_121()
        {
            DateTime theDate = DateTime.Now;
            string formatted = theDate.ToString("yyyyMMddmmss");
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(formatted);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            Guid uid = new Guid(hashBytes);
            string str_uid = uid.ToString();

            string[] UUID_PORTIONS = str_uid.Split('-');

            string uuid1 = UUID_PORTIONS[0];
            string uuid2 = UUID_PORTIONS[1];
            string uuid3 = UUID_PORTIONS[2];
            string uuid4 = UUID_PORTIONS[3];
            string uuid5 = UUID_PORTIONS[4];

            // eutr for SWIFT should be in the format below
            // now format uuid 3 to start with a 4
            uuid3 = "4" + uuid3.Substring(1, 3);

            // now format uuid4 to start with either 8,9, a or b
            text_generator textix = new text_generator();
            List<string> boundlist = new List<string> { "8", "9", "a", "b" };
            List<string> RandomChar = new List<string>();
            Random rand = new Random();

            uuid4 = string.Format("{0}{1}", textix.getText(RandomChar, boundlist, rand), uuid4.Substring(1, 3));

            return string.Format("{0}121:{1}-{2}-{3}-{4}-{5}{6}", "{", uuid1, uuid2, uuid3, uuid4, uuid5, "}");
        }


        public static bool IsFileInUse(string filePath)
        {
            try
            {
                //Try to open or create the file
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    //We can check whether the file can be read or written by using fs.CanRead or fs.CanWrite here.
                }
                return false;
            }
            catch (IOException ex)
            {
                //check if the message is about file io.
                string message = ex.Message.ToString();
                //Check if the file is in use
                if (message.Contains("The process cannot access the file"))
                    return true;
                else
                    throw;
            }
        }

        public class text_generator
        {
            public int getWordIndex(List<string> source, Random value)
            {
                return value.Next(0, source.Count - 1);
            }
            public bool checkListLength(List<string> source)
            {
                return source.Count == 0;
            }
            public string getText(List<string> source, List<string> backup_source, Random value)
            {
                if (checkListLength(source))
                {
                    source.AddRange(backup_source);
                }
                ;
                int index = getWordIndex(source, value);
                string result = source[index];
                source.RemoveAt(index);
                return result;
            }
        }

        //public static string RemoveSpecialCharacters(string str)
        //{
        //    StringBuilder sb1 = new StringBuilder();
        //    int AsciiValue;
        //    foreach (char c in str)
        //    {
        //        AsciiValue = Convert.ToInt32(c);
        //        if ((AsciiValue >= 32) && (AsciiValue <= 126) || ((AsciiValue == 10) || (AsciiValue == 13) || (AsciiValue == 27)))
        //        {
        //            sb1.Append(c);
        //        }
        //    }
        //    return sb1.ToString();
        //}

        

        public static string getXMLPathValue(XmlDocument xml_Doc, string searchNode)
        {
            string tmpResp = "";

            if (xml_Doc.SelectSingleNode(searchNode) != null)
            {
                tmpResp = xml_Doc.SelectSingleNode(searchNode).InnerText;
            }

            return tmpResp;
        }

        public static string Get_TagValueWithEnd(string Tag_Search_String, string TagSeparator, string EndString, bool UseEnd)
        {

            string[] TagSeparatorLocal = { "BCN" };
            TagSeparatorLocal.SetValue(TagSeparator, 0);
            int    Array_Len      = 0;
            string Array_Val      = "";
            string Output_String  = "";
            int    Start_Position = 0;
            string OutStr_len     = "0";
            int    End_Position   = 0;
            double OutNum;

            try
            {
                string[] Search_String_Params = Tag_Search_String.Split(TagSeparatorLocal, StringSplitOptions.None);
                Array_Len = Search_String_Params.Length;
                if (Array_Len > 0) ;
                {
                    Array_Val = Search_String_Params[1];
                    if (UseEnd)
                    {
                        End_Position = Array_Val.IndexOf(EndString);
                        if (End_Position != -1)
                        {
                            Output_String = Array_Val.Substring(0, End_Position);
                        }
                    }
                    else
                    {
                        Output_String = Array_Val;
                    }
                }
            }
            catch (Exception ex)
            {
                string ex_msg = ex.ToString();
                Output_String = "";
            }
            return Output_String;
        }



        public static string DownloadFtpDirectory(string url, NetworkCredential credentials, string localPath)
        {
            try 
            { 
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(url);
                listRequest.Method        = WebRequestMethods.Ftp.ListDirectoryDetails;
                listRequest.Credentials   = credentials;

                List<string> lines = new List<string>();

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (Stream listStream           = listResponse.GetResponseStream())
                using (StreamReader listReader     = new StreamReader(listStream))
                {
                    while (!listReader.EndOfStream)
                    {
                        lines.Add(listReader.ReadLine());
                    }
                }

                foreach (string line in lines)
                {
                    string[] tokens =
                        line.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                    string name = tokens[8];
                    string permissions = tokens[0];

                    string localFilePath = Path.Combine(localPath, name);
                    string fileUrl = url + name;

                    if (permissions[0] == 'd')
                    {
                        if (!Directory.Exists(localFilePath))
                        {
                            Directory.CreateDirectory(localFilePath);
                        }

                        DownloadFtpDirectory(fileUrl + "/", credentials, localFilePath);
                    }
                    else
                    {
                        FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(fileUrl);
                        downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                        downloadRequest.Credentials = credentials;

                        using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
                        using (Stream sourceStream = downloadResponse.GetResponseStream())
                        using (Stream targetStream = File.Create(localFilePath))
                        {
                            byte[] buffer = new byte[10240];
                            int read;
                            while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                targetStream.Write(buffer, 0, read);
                            }
                        }
                    }
                }
            }
            catch (Exception LogErrorException)
            {
                string Errstr = System.Convert.ToString(LogErrorException.ToString());
            }
            finally{}
            return "downloaded";
        }

        public static string Get_TagValueWithEnd(string Tag_Search_String, string TagSeparator, string EndString, bool UseEnd, int ParamToUse)
        {

            string[] TagSeparatorLocal = { "BCN" };
            TagSeparatorLocal.SetValue(TagSeparator, 0);
            int    Array_Len      = 0;
            string Array_Val      = "";
            string Output_String  = "";
            int    Start_Position = 0;
            string OutStr_len     = "0";
            int    End_Position   = 0;
            double OutNum;
            try
            {
                string[] Search_String_Params = Tag_Search_String.Split(TagSeparatorLocal, StringSplitOptions.None);
                Array_Len = Search_String_Params.Length;
                //if (Array_Len >= 0)
                if (Array_Len >= ParamToUse)
                {
                    Array_Val = Search_String_Params[ParamToUse];
                    if (UseEnd)
                    {
                        End_Position = Array_Val.IndexOf(EndString);
                        if (End_Position != -1)
                        {
                            Output_String = Array_Val.Substring(0, End_Position);
                        }
                    }
                    else
                    {
                        Output_String = Array_Val;
                    }
                }
            }
            catch (Exception ex)
            {
                string ex_msg = ex.ToString();
                Output_String = "";
            }
            return Output_String;
        }
        //public static string FromHexString(string hexString)
        //{
        //    var bytes = new byte[hexString.Length / 2];
        //    for (var i = 0; i < bytes.Length; i++)
        //    {
        //        bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        //    }

        //    return Encoding.Unicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        //}


        public static string FromHexString(string HexString)
        {
            string stringValue = "";
            for (int i = 0; i < HexString.Length / 2; i++)
            {
                string hexChar = HexString.Substring(i * 2, 2);
                int hexValue = Convert.ToInt32(hexChar, 16);
                stringValue += Char.ConvertFromUtf32(hexValue);
            }
            return stringValue;
        }


        public static string Remove_XML_SpecialCharacters(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9') || (str[i] >= 'A' && str[i] <= 'z' || (str[i] == '.' || str[i] == '_')) || (str[i] == ' '))
                    sb.Append(str[i]);
            }

            sb = sb.Replace("/", " ").Replace(":", "").Replace(";", "").Replace(",", "").Replace("-", "").Replace("_", "").Replace("&", "").Replace(".", " ");
            return sb.ToString();
        }

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
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
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
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
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

        public static void Log_to_Error(string Error_No, Exception Error_Expt, string Error_Module, string Err_Unique_Ref)
        {
            try
            {
                Hashtable Error_Hash = new Hashtable();
                string Err_Time = string.Format("{0:yyyy/MM/dd HH:mmm:ss}", DateTime.Now);

                string Local_Error_Descript = ExceptionToStr(Error_Expt);

                if (Local_Error_Descript.Length > 8000)
                {
                    Local_Error_Descript = Local_Error_Descript.Substring(0, 8000);
                }

                Error_Hash.Add("Err_Number_N", Error_No);
                Error_Hash.Add("Err_Desc_C"  , Local_Error_Descript);
                Error_Hash.Add("Err_Module_C", Error_Module);
                Error_Hash.Add("Unique_Ref_C", Err_Unique_Ref);
                Error_Hash.Add("Err_Date_D"  , Err_Time);

                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "tbl_ErrorLog", Error_Hash);
            }
            catch (Exception LogErrorException)
            {
                string Errstr = System.Convert.ToString(LogErrorException.ToString());
            }
        }

        public static string ExceptionToStr(Exception Error_Exption)
        {
            string Local_Error_Descript = System.Convert.ToString(Error_Exption.StackTrace.ToString());
            Local_Error_Descript = System.Convert.ToString(System.Text.RegularExpressions.Regex.Replace(Local_Error_Descript, "\\s{2,}", " ").Replace("\\n", ""));
            Local_Error_Descript = System.Convert.ToString(Local_Error_Descript.Replace("!", " ").Replace("&", " And ").Replace("<", "").Replace(">", "").Replace("'", "").Replace(",", " "));
            return Local_Error_Descript;
        }

    }
}