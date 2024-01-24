using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Collections;
using System.Configuration;
using System;


namespace e_Verify_BACK_OFFICE_Service
{
   static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///
       
        static void Main()
        {
            Hashtable tmpHash = new Hashtable();
            #if (!DEBUG)
      
                tmpHash = new Hashtable();
                string Error_Time = string.Format("{0:yyyy-MM-dd} {1}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss"));
                tmpHash.Add("STEP_DESCR"    , "Release Entry 1");
                tmpHash.Add("LOGGER_DESCR"  , "Release Entry 1");
                tmpHash.Add("DATE_LOGGED"   , Error_Time);
                tmpHash.Add("LOGGER_NODE_ID", e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Node_ID);
                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "Service_Logger", tmpHash);

                ServiceBase[] ServicesToRun;

                tmpHash = new Hashtable();
                Error_Time = string.Format("{0:yyyy-MM-dd} {1}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss"));
                tmpHash.Add("STEP_DESCR"    , "Release Entry 2");
                tmpHash.Add("LOGGER_DESCR"  , "Release Entry 2");
                tmpHash.Add("DATE_LOGGED"   , Error_Time);
                tmpHash.Add("LOGGER_NODE_ID", e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Node_ID);
                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "Service_Logger", tmpHash);

                // More than one user Service may run within the same process. To add
                // another service to this process, change the following line to
                // create a second service object. For example,
                //
                //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
       
                ServicesToRun = new ServiceBase[] { new e_Verify_BACK_OFFICE_Service() };

                tmpHash = new Hashtable();
                Error_Time = string.Format("{0:yyyy-MM-dd} {1}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss"));
                tmpHash.Add("STEP_DESCR"    , "Release Entry 3");
                tmpHash.Add("LOGGER_DESCR"  , "Release Entry 3");
                tmpHash.Add("DATE_LOGGED"   , Error_Time);
                tmpHash.Add("LOGGER_NODE_ID", e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Node_ID);
            SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "Service_Logger", tmpHash);
      
                ServiceBase.Run(ServicesToRun);
            
                tmpHash = new Hashtable();
                Error_Time = string.Format("{0:yyyy-MM-dd} {1}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss"));
                tmpHash.Add("STEP_DESCR"    , "Release Entry 4");
                tmpHash.Add("LOGGER_DESCR"  , "Release Entry 4");
                tmpHash.Add("DATE_LOGGED"   , Error_Time);
                tmpHash.Add("LOGGER_NODE_ID", e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Node_ID);
                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "Service_Logger", tmpHash);
#else
                tmpHash = new Hashtable();
                string Error_Time = string.Format("{0:yyyy-MM-dd} {1}", DateTime.Today.Date, DateTime.Now.ToString("HH:mm:ss"));
                tmpHash.Add("STEP_DESCR"    , "Debug Entry 1");
                tmpHash.Add("LOGGER_DESCR"  , "Debug Entry 1");
                tmpHash.Add("DATE_LOGGED"   , Error_Time);
                tmpHash.Add("LOGGER_NODE_ID", e_Verify_BACK_OFFICE_Service_Interface.Properties.Settings.Default.Node_ID);
                SqlHelper.insertSQL(ConfigurationManager.AppSettings["Interface_SQL_DB_Connection"].ToString(), "Service_Logger", tmpHash);

                // Debug code: this allows the process to run as a non-service.
                // It will kick off the service start point, but never kill it.
                // Shut down the debugger to exit
                e_Verify_BACK_OFFICE_Service service = new e_Verify_BACK_OFFICE_Service();

               /*
                   service.Post_Mobile_Charges_n_Levy_ABC();
                   service.Reverse_Mobile_Transactions_Data_Insert();
                   service.Reverse_Mobile_Transactions();

                   service.Reverse_Mobile_Transactions_Data_Insert_Zipit();
                   service.Reverse_Mobile_Transactions_Zipit();

                   //service.fn_Post_to_Barclays_Brains();
                   service.fn_Post_to_ZIMRA();
                   service.Alert_for_ZIMRA_Transactions();
                   service.fn_Post_to_ZIMRA_OfflineFile();
                   //Synch_Customers_to_Selcom();
                   //Synch_Accounts_to_Selcom();

                   //De_Synch_Accounts_from_Selcom();
                   //De_Synch_Customers_from_Selcom();
                   //service.Import_Barclays_StaticData();

                   //service.Import_SCB_AML_Transactions();
                   //service.Import_SCB_Static_Data();

                   service.CreateAML_File_CTR_SCB();
                   service.CreateAML_File_STR();

                   // Put a breakpoint on the following line to always catch
                   // your service when it has finished its work
                   //System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

              */
               System.Threading.Thread.Sleep(1000);
#endif
        }
    }
}