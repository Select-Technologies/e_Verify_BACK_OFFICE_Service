using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class ImportResponse_cls
    {
        public string OutputFileName
        {
            get;
            set;
        }

        public string ImportStatus
        {
            get;
            set;
        }
        public string InitialiseResponse()
        {
            OutputFileName = "";
            ImportStatus = "SUCCESS";
            return "";
        }
    }
}
