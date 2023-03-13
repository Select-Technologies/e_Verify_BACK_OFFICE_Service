using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class cls_MoneyGramTrxn
    {

        public string Batch_ID_C { get; set; }
        public string Originating_Country_Code { get; set; }
        public string Originating_Currency_Code { get; set; }
        public string Terminal_ID { get; set; }
        public string Op_ID { get; set; }
        public string Superv_Op_ID { get; set; }
        public string Username { get; set; }
        public string MTCN { get; set; }
        public string Receiver_Name { get; set; }
        public string Sender { get; set; }
        public string Destination_Country_Code { get; set; }
        public string Destination_Currency_Code { get; set; }
        public string Trn_Type { get; set; }
        public string Creation_Date { get; set; }
        public string Principal_Amount { get; set; }
        public string Charge { get; set; }
        public string Delivery_Charge { get; set; }
        public string Message_Charge { get; set; }
        public string Promotion_Discount { get; set; }
        public string Collect_Amount { get; set; }
        public string Exchange_Rate { get; set; }
        public string Expected_Payout_Amount { get; set; }
        public string Total_Charges { get; set; }
        public string Total_Taxes { get; set; }
        public string Payment_Type { get; set; }
        public string Destination { get; set; }
        public string Receiver_Address { get; set; }
        public string Receiver_Primary_ID_Type { get; set; }
        public string Receiver_Primary_ID { get; set; }
        public string Receiver_Telephone_number { get; set; }
        public string Sending_currency { get; set; }
        public string Sender_Principal_Amount { get; set; }
        public string Sender_Country { get; set; }
        public string Sender_Primary_ID_Type { get; set; }
        public string Sender_Primary_ID { get; set; }
        public string Sender_Address { get; set; }
        public string Sender_Telephone_number { get; set; }

        public string Initialise_Details()
        {
            Batch_ID_C = "";
            Originating_Country_Code = "";
            Originating_Currency_Code = "";
            Terminal_ID = "";
            Op_ID = "";
            Superv_Op_ID = "";
            Username = "";
            MTCN = "";
            Receiver_Name = "";
            Sender = "";
            Destination_Country_Code = "";
            Destination_Currency_Code = "";
            Trn_Type = "";
            Creation_Date = "";
            Principal_Amount = "";
            Charge = "";
            Delivery_Charge = "";
            Message_Charge = "";
            Promotion_Discount = "";
            Collect_Amount = "";
            Exchange_Rate = "";
            Expected_Payout_Amount = "";
            Total_Charges = "";
            Total_Taxes = "";
            Payment_Type = "";
            Destination = "";
            Receiver_Address = "";
            Receiver_Primary_ID_Type = "";
            Receiver_Primary_ID = "";
            Receiver_Telephone_number = "";
            Sending_currency = "";
            Sender_Principal_Amount = "";
            Sender_Country = "";
            Sender_Primary_ID_Type = "";
            Sender_Primary_ID = "";
            Sender_Address = "";
            Sender_Telephone_number = "";

            return "";
        }
    }
}
