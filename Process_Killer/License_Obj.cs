using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class License_Obj
    {
        public int Response_Code
        {
            get;
            set;
        }
        public string Response_Desc
        {
            get;
            set;
        }
        public DateTime License_Expiry_Date
        {
            get;
            set;
        }
        public DateTime License_Expiry_Date_Concessionary
        {
            get;
            set;
        }
        public string Expiry_Date { get; set; }
       
        public string Warning_Msg { get; set; }

        private object Initialise()
        {
            Expiry_Date = "";           
            Warning_Msg = "";
            return "0";
        }
        public void IntialiseResults()
        {
            Response_Code = 9;
            Response_Desc = "Unknown";
            License_Expiry_Date = DateTime.Now;
            License_Expiry_Date.AddDays(-1);
            License_Expiry_Date_Concessionary = DateTime.Now;
            License_Expiry_Date_Concessionary.AddDays(-1);
        }
    }
}
