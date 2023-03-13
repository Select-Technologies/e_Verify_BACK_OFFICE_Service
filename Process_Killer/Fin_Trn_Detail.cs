using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service
{
    public class Fin_Trn_Detail
    {
        public string Business_Unit_Description
        {
            get;
            set;
        }
        public string Customer_Number
        {
            get;
            set;
        }
        public string Customer_First_Name
        {
            get;
            set;
        }
        public string Customer_Middle_Name
        {
            get;
            set;
        }
        public string Customer_Last_Name
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Gender
        {
            get;
            set;
        }
        public string Physical_Address_Line_1
        {
            get;
            set;
        }
        public string Physical_Address_Line_2
        {
            get;
            set;
        }
        public string Physical_Address_Line_3
        {
            get;
            set;
        }
        public string Physical_Postal_City
        {
            get;
            set;
        }
        public string Physical_Postal_Code
        {
            get;
            set;
        }
        public string Residential_Country_Code
        {
            get;
            set;
        }
        public string Account_Number
        {
            get;
            set;
        }
        public string Account_Name
        {
            get;
            set;
        }
        public string Scheme_Type
        {
            get;
            set;
        }
        public string Scheme_Code
        {
            get;
            set;
        }
        public string Scheme_Code_Description
        {
            get;
            set;
        }
        public string Transaction_Type_Code
        {
            get;
            set;
        }
        public string Transaction_Domicile_Branch_Code
        {
            get;
            set;
        }
        public string Transaction_Domicile_Branch_Name
        {
            get;
            set;
        }
        public string Transaction_Currency
        {
            get;
            set;
        }
        public string Transaction_Amount
        {
            get;
            set;
        }
        public string Transaction_Amount_LCY
        {
            get;
            set;
        }

        public string exchangeRate
        {
            get;
            set;
        }

        public string NewBal
        {
            get;
            set;
        }

        public string Transaction_Detail
        {
            get;
            set;
        }
        public string Transaction_Narrative
        {
            get;
            set;
        }
        public string Transaction_Remark
        {
            get;
            set;
        }
        public string Teller_ID
        {
            get;
            set;
        }
        public string Teller
        {
            get;
            set;
        }
        public string TrnDate
        {
            get;
            set;
        }


        public string Initialise_Details()
        {
            Business_Unit_Description = "";
            Customer_Number = "";
            Customer_First_Name = "";
            Customer_Middle_Name = "";
            Customer_Last_Name = "";
            Title = "";
            Gender = "";
            Physical_Address_Line_1 = "";
            Physical_Address_Line_2 = "";
            Physical_Address_Line_3 = "";
            Physical_Postal_City = "";
            Physical_Postal_Code = "";
            Residential_Country_Code = "";
            Account_Number = "";
            Account_Name = "";
            Scheme_Type = "";
            Scheme_Code = "";
            Scheme_Code_Description = "";
            Transaction_Type_Code = "";
            Transaction_Domicile_Branch_Code = "";
            Transaction_Domicile_Branch_Name = "";
            Transaction_Currency = "";
            Transaction_Amount = "";
            Transaction_Amount_LCY = "";
            Transaction_Detail = "";
            Transaction_Narrative = "";
            Transaction_Remark = "";
            Teller_ID = "";
            Teller = "";
            TrnDate = "";

            return "0";
        }

    }
}
