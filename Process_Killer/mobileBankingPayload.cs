using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class mobileBankingPayload
    {
        public string customerName               { get; set; } = "";
        public string gender                     { get; set; } = "";
        public string accountStatus              { get; set; } = "";
        public string transactionDate            { get; set; } = "";
        public string lastTransactionDate        { get; set; } = "";
        public string uniqueIdentificationNumber { get; set; } = "";
        public string customerCategory           { get; set; } = "";
        public string mobileTransactionType      { get; set; } = "";
        public string serviceCategory            { get; set; } = "";
        public string subServiceCategory         { get; set; } = "";
        public string serviceStatus              { get; set; } = "";
        public string transactionRef             { get; set; } = "";
        public string currency                   { get; set; } = "";
        public decimal orgAmount                 { get; set; } = 0;
        public decimal tzsAmount                 { get; set; } = 0;
        public decimal valueAddedTaxAmount       { get; set; } = 0;
        public decimal exciseDutyAmount          { get; set; } = 0;
        public decimal electronicLevyAmount      { get; set; } = 0;
    }
}
