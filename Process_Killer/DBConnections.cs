using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;

namespace e_Verify_BACK_OFFICE_Service
{
    public class DBConnections
    {
         public class TrxnDBConnection
        {
            private string _connString;

            public string ConnString
            {
                get
                {
                   _connString = System.Configuration.ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString();
                   return _connString;
                }
            }
        }

        public class SMSDBConnection
        {
            private string _connString;

            public string ConnString
            {
                get
                {
                    _connString = System.Configuration.ConfigurationManager.AppSettings["SMS_DB_Connection"];
                   return _connString;
                }
            }
        }

    }
}
