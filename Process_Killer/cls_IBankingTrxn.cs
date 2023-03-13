using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e_Verify_BACK_OFFICE_Service_Interface
{
    public class cls_IBankingTrxn
    {
        public string Batch_ID_C      { get; set; }  
        public string TXREFNO         { get; set; }
        public string SCANTIME        { get; set; }
        public string CREATIONDATE    { get; set; }
        public string COUNTRYCODE     { get; set; }
        public string STATIONID       { get; set; }
        public string BARCODE         { get; set; }
        public string ORGTXREFNO      { get; set; }
        public string BATCHNAME       { get; set; }
        public string CURRENTSTEPNAME { get; set; }
        public string CURRENTQUEUE    { get; set; }
        public string STATUS          { get; set; }
        public string BRANCHCODE      { get; set; }
        public string LASTSTEPNAME    { get; set; }
        public string MODIFIEDUSERID  { get; set; }
        public string ACCOUNT_NO      { get; set; }
        public string FULL_NAME       { get; set; }
        public string IDNUMBER        { get; set; }
        public string MOBILE_NUMBER   { get; set; }
        public string TYPE_OF_REQUEST { get; set; }
        public string RELATIONSHIP_NO { get; set; }
        public string CASE_STATUS     { get; set; }
        public string COMMENTS        { get; set; }

        public string Initialise_Details()
        {
            Batch_ID_C      = "";
            TXREFNO         = "";
            SCANTIME        = "";
            CREATIONDATE    = "";
            COUNTRYCODE     = "";
            STATIONID       = "";
            BARCODE         = "";
            ORGTXREFNO      = "";
            BATCHNAME       = "";
            CURRENTSTEPNAME = "";
            CURRENTQUEUE    = "";
            STATUS          = "";
            BRANCHCODE      = "";
            LASTSTEPNAME    = "";
            MODIFIEDUSERID  = "";
            ACCOUNT_NO      = "";
            FULL_NAME       = "";
            IDNUMBER        = "";
            MOBILE_NUMBER   = "";
            TYPE_OF_REQUEST = "";
            RELATIONSHIP_NO = "";
            CASE_STATUS     = "";
            COMMENTS        = "";

            return "";
        }
    }
}
