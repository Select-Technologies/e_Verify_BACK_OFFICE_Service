using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class MT101
    {
        public string MSG_Ref            { get; set; } = "";
        public string Reference          { get; set; } = "";
        public string Currency           { get; set; } = "";
        public string Beneficary_Account { get; set; } = "";
        public string Beneficary_Name    { get; set; } = "";
        public string TranAmount         { get; set; } = "";
    }
}
