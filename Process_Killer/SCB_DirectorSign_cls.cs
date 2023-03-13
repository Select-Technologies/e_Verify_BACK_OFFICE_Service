using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class SCB_DirectorSign_cls
    {
        public string PRINRELATIONSHIPNO { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string CORPORATETITLE { get; set; }
        public string RLRELATIONSHIPNO { get; set; }

        public string Initialise_Details()
        {
            PRINRELATIONSHIPNO = "";
            FIRSTNAME = "";
            LASTNAME = "";
            CORPORATETITLE = "";
            RLRELATIONSHIPNO = "";
            return "done";
        }
    }
}
