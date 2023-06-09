﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online {
    using System.Data;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/", ConfigurationName="ZIMRA_Online.ZimraWebServiceSoap")]
    public interface ZimraWebServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/CheckStatus", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int CheckStatus();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/ZIMRA_PaymentAdvice", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ZIMRA_PaymentAdvice(string SelectCode, string AccountNumber, string Amount, string BPNumber, string CaptureTime, string ClientName, string Currency, string PaymentDate, string ReferenceNumber, string Region, string RRN, string SerialNumber, string TaxCode, string UserID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/ZIMRA_ContractQry", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.Response ZIMRA_ContractQry(string SelectCode, string BPNumber_C, string TaxCoder_C);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/AU_PostTransaction", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string AU_PostTransaction(string SelectCode, string schoolCode, double bankAccNo, string studentID, string studentName, double payAmnt, string paymentRef, string PaymentDesc, string PaymentType, string UniqueRef);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/AU_verifyStudent_No", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable AU_verifyStudent_No(string SelectCode, string studentNO_Local, string schoolCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/post_ZIMRA_Transaction", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.post_ZIMRA_Transaction_Response post_ZIMRA_Transaction(string postingUserID, string postingUserPass, string BPN, string stationCode, string obligationID, bool isWalkIn, double tranAmount, string currencyCode, string sourceReference);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/keyField_Verification", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable keyField_Verification(string searchUserID, string searchUserPass, string searchProduct, string searchType, string searchKey);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/CIMAS_verifyMember_No", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CIMAS_verifyMember_No(string SelectCode, string memberType, string memberNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/Presbyterian_verifyMember_No", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.cls_Presbyterian_Detail Presbyterian_verifyMember_No(string SelectCode, string memberType, string memberNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/Presbyterian_Payment", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.cls_Presbyterian_Detail Presbyterian_Payment(string SelectCode, string studentID, string studentName, string payAmount, string refNum, string Narration);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/AU_GetAllStudents", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        System.Data.DataTable AU_GetAllStudents(string SelectCode, string schoolCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/ZIMRA_RecieverRequest", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.Record[] ZIMRA_RecieverRequest(string SelectCode, string dt_DateCreated);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/PPC_PaymentAdvice", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string PPC_PaymentAdvice(string SelectCode, string Trn_CustName, System.DateTime Trn_Date, System.DateTime Trn_ValueDate, string Trn_Burks, string Trn_AgentCode, string Trn_Txt, string Trn_Amnt, string Trn_Invoice);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://10.237.61.33:62188/Zimra.asmx/ZesaTokenPurchase", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ZesaTokenPurchase(string SelectCode, string SelectPin, string mobile, string BillID, double paymentAmount, string Trxn_ID, string Source_System);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/")]
    public partial class Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ExtensionDataObject extensionDataField;
        
        private string bPNumberField;
        
        private string accountField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
                this.RaisePropertyChanged("ExtensionData");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string BPNumber {
            get {
                return this.bPNumberField;
            }
            set {
                this.bPNumberField = value;
                this.RaisePropertyChanged("BPNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string Account {
            get {
                return this.accountField;
            }
            set {
                this.accountField = value;
                this.RaisePropertyChanged("Account");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/")]
    public partial class ExtensionDataObject : object, System.ComponentModel.INotifyPropertyChanged {
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/")]
    public partial class Record : object, System.ComponentModel.INotifyPropertyChanged {
        
        private ExtensionDataObject extensionDataField;
        
        private string bPNumberField;
        
        private string bPNameField;
        
        private string dateCreatedField;
        
        private string bPStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
                this.RaisePropertyChanged("ExtensionData");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string BPNumber {
            get {
                return this.bPNumberField;
            }
            set {
                this.bPNumberField = value;
                this.RaisePropertyChanged("BPNumber");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string BPName {
            get {
                return this.bPNameField;
            }
            set {
                this.bPNameField = value;
                this.RaisePropertyChanged("BPName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public string DateCreated {
            get {
                return this.dateCreatedField;
            }
            set {
                this.dateCreatedField = value;
                this.RaisePropertyChanged("DateCreated");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string BPStatus {
            get {
                return this.bPStatusField;
            }
            set {
                this.bPStatusField = value;
                this.RaisePropertyChanged("BPStatus");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/")]
    public partial class cls_Presbyterian_Detail : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string codeField;
        
        private string studentIdField;
        
        private string studentNameField;
        
        private bool successField;
        
        private string messageField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string studentId {
            get {
                return this.studentIdField;
            }
            set {
                this.studentIdField = value;
                this.RaisePropertyChanged("studentId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string studentName {
            get {
                return this.studentNameField;
            }
            set {
                this.studentNameField = value;
                this.RaisePropertyChanged("studentName");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool Success {
            get {
                return this.successField;
            }
            set {
                this.successField = value;
                this.RaisePropertyChanged("Success");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://10.237.61.33:62188/Zimra.asmx/")]
    public partial class post_ZIMRA_Transaction_Response : object, System.ComponentModel.INotifyPropertyChanged {
        
        private bool successOrFailureField;
        
        private string responseIDField;
        
        private string responseNarrativeField;
        
        private bool responseErrorField;
        
        private System.DateTime responseTimeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public bool successOrFailure {
            get {
                return this.successOrFailureField;
            }
            set {
                this.successOrFailureField = value;
                this.RaisePropertyChanged("successOrFailure");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string ResponseID {
            get {
                return this.responseIDField;
            }
            set {
                this.responseIDField = value;
                this.RaisePropertyChanged("ResponseID");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string ResponseNarrative {
            get {
                return this.responseNarrativeField;
            }
            set {
                this.responseNarrativeField = value;
                this.RaisePropertyChanged("ResponseNarrative");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public bool ResponseError {
            get {
                return this.responseErrorField;
            }
            set {
                this.responseErrorField = value;
                this.RaisePropertyChanged("ResponseError");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public System.DateTime ResponseTime {
            get {
                return this.responseTimeField;
            }
            set {
                this.responseTimeField = value;
                this.RaisePropertyChanged("ResponseTime");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ZimraWebServiceSoapChannel : e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.ZimraWebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ZimraWebServiceSoapClient : System.ServiceModel.ClientBase<e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.ZimraWebServiceSoap>, e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.ZimraWebServiceSoap {
        
        public ZimraWebServiceSoapClient() {
        }
        
        public ZimraWebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ZimraWebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ZimraWebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ZimraWebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public int CheckStatus() {
            return base.Channel.CheckStatus();
        }
        
        public string ZIMRA_PaymentAdvice(string SelectCode, string AccountNumber, string Amount, string BPNumber, string CaptureTime, string ClientName, string Currency, string PaymentDate, string ReferenceNumber, string Region, string RRN, string SerialNumber, string TaxCode, string UserID) {
            return base.Channel.ZIMRA_PaymentAdvice(SelectCode, AccountNumber, Amount, BPNumber, CaptureTime, ClientName, Currency, PaymentDate, ReferenceNumber, Region, RRN, SerialNumber, TaxCode, UserID);
        }
        
        public e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.Response ZIMRA_ContractQry(string SelectCode, string BPNumber_C, string TaxCoder_C) {
            return base.Channel.ZIMRA_ContractQry(SelectCode, BPNumber_C, TaxCoder_C);
        }
        
        public string AU_PostTransaction(string SelectCode, string schoolCode, double bankAccNo, string studentID, string studentName, double payAmnt, string paymentRef, string PaymentDesc, string PaymentType, string UniqueRef) {
            return base.Channel.AU_PostTransaction(SelectCode, schoolCode, bankAccNo, studentID, studentName, payAmnt, paymentRef, PaymentDesc, PaymentType, UniqueRef);
        }
        
        public System.Data.DataTable AU_verifyStudent_No(string SelectCode, string studentNO_Local, string schoolCode) {
            return base.Channel.AU_verifyStudent_No(SelectCode, studentNO_Local, schoolCode);
        }
        
        public e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.post_ZIMRA_Transaction_Response post_ZIMRA_Transaction(string postingUserID, string postingUserPass, string BPN, string stationCode, string obligationID, bool isWalkIn, double tranAmount, string currencyCode, string sourceReference) {
            return base.Channel.post_ZIMRA_Transaction(postingUserID, postingUserPass, BPN, stationCode, obligationID, isWalkIn, tranAmount, currencyCode, sourceReference);
        }
        
        public System.Data.DataTable keyField_Verification(string searchUserID, string searchUserPass, string searchProduct, string searchType, string searchKey) {
            return base.Channel.keyField_Verification(searchUserID, searchUserPass, searchProduct, searchType, searchKey);
        }
        
        public string CIMAS_verifyMember_No(string SelectCode, string memberType, string memberNo) {
            return base.Channel.CIMAS_verifyMember_No(SelectCode, memberType, memberNo);
        }
        
        public e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.cls_Presbyterian_Detail Presbyterian_verifyMember_No(string SelectCode, string memberType, string memberNo) {
            return base.Channel.Presbyterian_verifyMember_No(SelectCode, memberType, memberNo);
        }
        
        public e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.cls_Presbyterian_Detail Presbyterian_Payment(string SelectCode, string studentID, string studentName, string payAmount, string refNum, string Narration) {
            return base.Channel.Presbyterian_Payment(SelectCode, studentID, studentName, payAmount, refNum, Narration);
        }
        
        public System.Data.DataTable AU_GetAllStudents(string SelectCode, string schoolCode) {
            return base.Channel.AU_GetAllStudents(SelectCode, schoolCode);
        }
        
        public e_Verify_BACK_OFFICE_Service_Interface.ZIMRA_Online.Record[] ZIMRA_RecieverRequest(string SelectCode, string dt_DateCreated) {
            return base.Channel.ZIMRA_RecieverRequest(SelectCode, dt_DateCreated);
        }
        
        public string PPC_PaymentAdvice(string SelectCode, string Trn_CustName, System.DateTime Trn_Date, System.DateTime Trn_ValueDate, string Trn_Burks, string Trn_AgentCode, string Trn_Txt, string Trn_Amnt, string Trn_Invoice) {
            return base.Channel.PPC_PaymentAdvice(SelectCode, Trn_CustName, Trn_Date, Trn_ValueDate, Trn_Burks, Trn_AgentCode, Trn_Txt, Trn_Amnt, Trn_Invoice);
        }
        
        public string ZesaTokenPurchase(string SelectCode, string SelectPin, string mobile, string BillID, double paymentAmount, string Trxn_ID, string Source_System) {
            return base.Channel.ZesaTokenPurchase(SelectCode, SelectPin, mobile, BillID, paymentAmount, Trxn_ID, Source_System);
        }
    }
}
