using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
   public class Trn_Narr1_Narr2_AndRef
    {
        public string DR_Trn_Narrative1 { get; set; } = "";
        public string DR_Trn_Narrative2 { get; set; } = "";
        public string DR_Trn_Reference  { get; set; } = "";

        public string CR_Trn_Narrative1 { get; set; } = "";
        public string CR_Trn_Narrative2 { get; set; } = "";
        public string CR_Trn_Reference  { get; set; } = "";
        public void IntialiseObject()
        {
            CR_Trn_Narrative1 = "";
            CR_Trn_Narrative2 = "";
            CR_Trn_Reference  = "";

            DR_Trn_Narrative1 = "";
            DR_Trn_Narrative2 = "";
            DR_Trn_Reference  = "";
        }
    }
}
