using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class agentBankingPayload
    {
            public string agentId               { get; set; } = "";
            public string agentStatus           { get; set; } = "active";
            public string transactionDate       { get; set; } = "";
            public string lastTransactionDate   { get; set; } = "";
            public string transactionId         { get; set; } = "";
            public string transactionType       { get; set; } = "";
            public string serviceChannel        { get; set; } = "Point of Sale";
            public string tillNumber            { get; set; } = "0001";
            public string currency              { get; set; } = "TZS";
            public decimal tzsAmount            { get; set; } = 0;
    }
}
