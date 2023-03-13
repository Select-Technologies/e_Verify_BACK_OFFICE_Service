using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    class cls_License
    {
        public string Expiry_Date { get; set; }
        public int Response_Code { get; set; }
        public string Response_Desc { get; set; }
        public string Warning_Msg { get; set; }

        private object Initialise()
        {
            Expiry_Date = "";
            Response_Code = 0;
            Response_Desc = "";
            Warning_Msg = "";
            return "0";
        }
    }
}
