using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class Account_Branch
    {
        public string BRANCH_CODE
        {
            get;
            set;
        }
        public string CUST_AC_NO
        {
            get;
            set;
        }

        public string CCY
        {
            get;
            set;
        }

        public int Branch_Instances
        {
            get;
            set;
        }

        public void IntialiseResults()
        {
            BRANCH_CODE      = "";
            CUST_AC_NO       = "";
            CCY              = "";
            Branch_Instances = 0;
        }
    }
}
