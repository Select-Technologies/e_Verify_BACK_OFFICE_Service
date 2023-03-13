using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class cls_MobileCredentials
    {

        public string mobUserID       { get; set; }
        public string mobUserPassword { get; set; }
        public bool   mobUserLive     { get; set; }

        public string Initialise()
        {
            mobUserID       = "";
            mobUserPassword = "";
            mobUserLive     = false;
            return "0";
        }
    }
}
