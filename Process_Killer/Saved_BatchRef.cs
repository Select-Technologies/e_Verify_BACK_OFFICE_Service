using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class Saved_BatchRef
    {
        public string BatchNumber_C
        {
            get;
            set;
        }
        public string TrxnNo_C
        {
            get;
            set;
        }
        public void IntialiseResults()
        {
            BatchNumber_C = "";
            TrxnNo_C = "";
        }
    }
}
