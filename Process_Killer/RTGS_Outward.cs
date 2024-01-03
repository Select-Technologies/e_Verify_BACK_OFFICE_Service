using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class RTGS_Outward
    {
        public string debitAccount             { get; set; } = "";
        public string requestID                { get; set; } = "";
        public string creditBICCode            { get; set; } = "";
        public string debitBICCode             { get; set; } = "";
        public string beneficiaryName          { get; set; } = "";
        public string transactionAmount        { get; set; } = "";
        public string transactionCurrency      { get; set; } = "";
        public string beneficiaryBankName      { get; set; } = "";
        public string beneficiaryAccountNumber { get; set; } = "";
        public string debitAccountName         { get; set; } = "";
        public string reference                { get; set; } = "";
        public string valueDate                { get; set; } = "";
        public string drChargeFlag             { get; set; } = "N";
    }
}
