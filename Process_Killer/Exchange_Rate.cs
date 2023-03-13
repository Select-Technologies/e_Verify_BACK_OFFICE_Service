using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class Exchange_Rate
    {
        public string Branch_Code
        {
            get;
            set;
        }
        public string From_Curr
        {
            get;
            set;
        }
        public string To_Curr
        {
            get;
            set;
        }
        public string RateType
        {
            get;
            set;
        }
        public float Mid_Rate
        {
            get;
            set;
        }
        public float Buy_Spread
        {
            get;
            set;
        }
        public float Sale_Spread
        {
            get;
            set;
        }
        public float Buy_Rate
        {
            get;
            set;
        }
        public float Sale_Rate
        {
            get;
            set;
        }
        public string Rate_Serial
        {
            get;
            set;
        }

        public DateTime Rate_Date
        {
            get;
            set;
        }

        public string From_Curr_Number
        {
            get;
            set;
        }
        public string From_Curr_Position_Acc_C
        {
            get;
            set;
        }
        public string From_Curr_Card_SuspenseAcc_C
        {
            get;
            set;
        }
        public string From_Curr_Wallet_Acc_C
        {
            get;
            set;
        }
        public string From_Curr_Charges_Acc_C
        {
            get;
            set;
        }
        public string From_Curr_Transmission_Acc
        {
            get;
            set;
        }
        public string From_Curr_Splt_Acc_C
        {
            get;
            set;
        }
        public string From_Curr_Gvt_Levy_Acc
        {
            get;
            set;
        }
        public string To_Curr_Number
        {
            get;
            set;
        }
        public string To_Curr_Position_Acc_C
        {
            get;
            set;
        }
        public string To_Curr_Card_SuspenseAcc_C
        {
            get;
            set;
        }
        public string To_Curr_Wallet_Acc_C
        {
            get;
            set;
        }
        public string To_Curr_Charges_Acc_C
        {
            get;
            set;
        }
        public string To_Curr_Transmission_Acc
        {
            get;
            set;
        }
        public string To_Curr_Splt_Acc_C
        {
            get;
            set;
        }
        public string To_Curr_Gvt_Levy_Acc
        {
            get;
            set;
        }
        public double Amount_FromCurr
        {
            get;
            set;
        }
        public float Amount_ToCurr
        {
            get;
            set;
        }
        public string Rate_Status
        {
            get;
            set;
        }
        public string Rate_Status_Narrative
        {
            get;
            set;
        }
    }
}
