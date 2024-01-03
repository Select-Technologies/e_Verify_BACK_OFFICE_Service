using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class eximBills_MT103
    {
        public string paymentID                { get; set; } = "";
        public string valueDate                { get; set; } = "";
        public string chargesPaidBy            { get; set; } = "";
        public string accountNumber            { get; set; } = "";
        public string accountName              { get; set; } = "";
        public string debitAccountCurrency     { get; set; } = "";
        public string totalTransferAmount      { get; set; } = "";
        public string transferCurrency         { get; set; } = "";
        public string debitReference           { get; set; } = "";
        public string exchnageRateType         { get; set; } = "";
        public string dealReference            { get; set; } = "";
        public string exchangeRate             { get; set; } = "";
        public string transactionID            { get; set; } = "";
        public string beneficiaryName          { get; set; } = "";
        public string transferAmount           { get; set; } = "";
        public string beneficiaryCode          { get; set; } = "";
        public string beneficiaryAccountNumber { get; set; } = "";
        public string beneficiaryIBAN          { get; set; } = "";
        public string beneficiaryBankBIC       { get; set; } = "";
        public string beneficiaryBanksortCode  { get; set; } = "";
        public string beneficiaryBankBranch    { get; set; } = "";
        public string beneficiaryReference     { get; set; } = "";
        public string BOPReason                { get; set; } = "";
        public string customerNameAndContact   { get; set; } = "";
        public string beneficiaryaddress       { get; set; } = "";
        public string beneficiarybankAddress   { get; set; } = "";
    }
}
