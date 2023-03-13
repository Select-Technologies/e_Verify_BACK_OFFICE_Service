using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class cls_UserAccount
    {
         public string TelephoneNo { get; set;}
         public string AccountNo { get; set;}
         public string Acc_Name_C { get; set;}
         public string Currency { get; set;}
         public string Limit_N { get; set;}
         public string Last_Modified_D { get; set;}
         public string Last_ModifiedBy_C { get; set;}
         public string Charged { get; set;}
         public string Source_System_C { get; set;}
         public string Product_C { get; set;}
         public string ServiceProvider_C { get; set;}
         public string Extracted_YN_B { get; set;}
         public string Active_YN { get; set;}
         public string chargeType_C { get; set;}
         public string Approved_YN { get; set;}
         public string Approved_By { get; set;}
         public string comment { get; set;}
         public string Approval_Date { get; set;}
         public string KYC_Address { get; set;}
         public string KYC_Mobile_Number { get; set;}
         public string KYC_Acc_Name { get; set;}
         public string Fin_Retry_No { get; set;}
         public string userBranch { get; set;}
         public string Declined_YN { get; set;}
         public string KYC_IDNumber_C { get; set;}
         public string KYC_IDType_IP { get; set;}
         public string KYC_IDType_LF { get; set;}
         public string AccType_IM { get; set;}
         public string Deleted_YN_B { get; set;}

         public string Initialise_Details()
         {
            TelephoneNo = "";
            AccountNo = "";
            Acc_Name_C = "";
            Currency = "";
            Limit_N = "";
            Last_Modified_D = "";
            Last_ModifiedBy_C = "";
            Charged = "";
            Source_System_C = "";
            Product_C = "";
            ServiceProvider_C = "";
            Extracted_YN_B = "";
            Active_YN = "";
            chargeType_C = "";
            Approved_YN = "";
            Approved_By = "";
            comment = "";
            Approval_Date = "";
            KYC_Address = "";
            KYC_Mobile_Number = "";
            KYC_Acc_Name = "";
            Fin_Retry_No = "";
            userBranch = "";
            Declined_YN = "";
            KYC_IDNumber_C = "";
            KYC_IDType_IP = "";
            KYC_IDType_LF = "";
            AccType_IM = "";
            Deleted_YN_B = "";

             return "";
         }
    }
}
