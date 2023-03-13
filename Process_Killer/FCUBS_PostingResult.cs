using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class FCUBS_PostingResult
    {
        public string DE_Result
        {
            get;
            set;
        }
        public string DE_Result_Desc
        {
            get;
            set;
        }
        public string DE_Core_Ref
        {
            get;
            set;
        }

        public void IntialiseResults()
        {
            DE_Result      = "";
            DE_Result_Desc = "";
            DE_Core_Ref    = "";
        }
    }
}
