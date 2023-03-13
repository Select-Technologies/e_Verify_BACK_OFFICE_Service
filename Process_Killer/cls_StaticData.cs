using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class cls_StaticData
    {

        public string Acc_BranchNo
        {
            get;
            set;
        }
        public string Acc_AccountNo
        {
            get;
            set;
        }
        public string Acc_CurrCode
        {
            get;
            set;
        }
        public string Acc_ShortName
        {
            get;
            set;
        }
        public string Acc_LongName
        {
            get;
            set;
        }
        public string Acc_Type
        {
            get;
            set;
        }
        public string Acc_Segment
        {
            get;
            set;
        }
        public string Acc_Opened
        {
            get;
            set;
        }

        public string Initialise_Details()
        {
            Acc_BranchNo   = "";
            Acc_AccountNo  = "";
            Acc_CurrCode   = "";
            Acc_ShortName  = "";
            Acc_LongName   = "";
            Acc_Type       = "";
            Acc_Segment    = "";
            Acc_Opened     = "";

            return "0";
        }


       
    }
}
