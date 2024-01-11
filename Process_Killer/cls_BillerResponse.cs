using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class cls_BillerResponse
    {
        public string transid       { get; set; } = "";
        public string reference     { get; set; } = "";
        public string resultcode    { get; set; } = "";
        public string result        { get; set; } = "";
        public string message       { get; set; } = "";
        public string status        { get; set; } = "";
        public string rawdata       { get; set; } = "";
        public List<string> data    { get; set; }
    }
}
