using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerAccount
{
    [Serializable]
    public class CustomerAccount
    {
        #region Member Variables

        protected int _siteId;
        protected string _accountNumber9 = string.Empty;
        protected int _companyNumber;
        protected int _divisionNumber;
        protected int _franchiseNumber;
        protected string _lastName = string.Empty;
        protected string _firstName = string.Empty;
        protected string _customerName = string.Empty;
        protected string _commercialBusinessName = string.Empty;
        protected string _title = string.Empty;
        protected string _houseNumber = string.Empty;
        protected string _residentNumber = string.Empty;
        protected string _serviceAddressLine1 = string.Empty;
        protected string _serviceAddressLine2 = string.Empty;
        protected string _serviceAddressLine3 = string.Empty;
        protected string _serviceAddressLine4 = string.Empty;
        protected string _zipCode4 = string.Empty;
        protected string _zipCode5 = string.Empty;
        protected string _customerType = string.Empty;
        protected string _accountStatus = string.Empty;
        protected string _collectionStatus = string.Empty;
        protected double _pastDueBalance = 0;
        protected DateTime _customerEstablishedDate = DateTime.MinValue;
        protected DateTime _customerDisconnectDate = DateTime.MinValue;
        protected bool _doNotCallResidential = false;
        protected bool _doNotCallWireless = false;
        protected bool _doNotAcceptCheck = false;
        protected bool _doNotAcceptCreditCard = false ;
        protected bool _doNotAcceptEmail = false;
        protected bool _doNotAcceptMail = false;
        protected string _customerValueSegmentation = string.Empty;
        protected string _dwellingType = string.Empty;
        protected string _dwellingTypeDescription = string.Empty;
        protected bool _enrolledInEasyPay = false;
        protected bool _enrolledInStopPaperBill = false;
        protected bool _enrolledInEmailBillReminder = false;
        protected string _contactEmailAddress = string.Empty;
        protected string _languagePreference = string.Empty;
        protected double _depositAmountRequired = 0; 
        protected List<CurrentCampaignDetail> _currentCampaignDetail = null; 
        protected string _currentCampaignCodeDescription;
        protected bool _underContract;
        protected int _contractId;
        protected string _contractDescription;
        protected DateTime _currentEndDate;
        protected double _monthlyRecurringRevenue;


        

        


        #endregion Member Variables
        #region Ctors
        /// <summary>
		/// Default ctor
		/// </summary>
		public CustomerAccount(){}

		/// <summary>
        /// <summary>
		/// Ctor taking params
		/// </summary>
        /// Property enabling the getting of siteId
       
		#endregion Ctors

		#region Properties
                
        /// <summary>
        /// ICOMS Site Id - 3 Digits
        /// </summary>
        [XmlAttribute( "SiteId")] 
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        /// <summary>
        /// ICCOMS Account Number - 9 Digits
        /// </summary>
       [XmlAttribute( "AccountNumber9")] 
        public string AccountNumber9
        {
            get { return _accountNumber9; }
            set { _accountNumber9 = value; }
        }

        /// <summary>
        /// ICOMS Company Number - 2 Digits
        /// </summary>
        [XmlAttribute("CompanyNumber")] 
        public int CompanyNumber
        {
            get { return _companyNumber; }
            set { _companyNumber = value; }
        }

        /// <summary>
        /// ICOMS Division Number - 2 Digits
        /// </summary>
        [XmlAttribute( "DivisionNumber")] 
        public int DivisionNumber
        {
            get { return _divisionNumber; }
            set { _divisionNumber = value; }
        }
        
        /// <summary>
        /// ICOMS Franchise Number - 3 Digits
        /// </summary>
        [XmlAttribute( "FranchiseNumber")] 
        public int FranchiseNumber
        {
            get { return _franchiseNumber; }
            set { _franchiseNumber = value; }
        }

        /// <summary>
        /// Last Name on file for Account Number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.LAST_NAME
        /// </summary>
        [XmlAttribute( "LastName")] 
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// First Name on file for acccount number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.FIRST_NAME
        /// </summary>
        [XmlAttribute( "FirstName")] 
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
        
        /// <summary>
        /// Title on file for acccount number in ICOMS. (Mr., Mrs., Dr., etc)
        /// Source : XXX_CUSTOMER_MASTER.TITLE
        /// </summary>
        [XmlAttribute( "Title")] 
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// ICOMS House Number - 7 Digits
        /// Source  :  XXX_CUSTOMER_MASTER.HOUSE_NUMBER
        /// </summary>
        [XmlAttribute("HouseNumber")]
        public string HouseNumber
        {
            get { return _houseNumber; }
            set { _houseNumber = value; }
        }

        /// <summary>
        /// ICOMS House Resident Number - 2 Digits
        /// Source  :  XXX_CUSTOMER_MASTER.HOUSE_RESIDENT_NUMBER
        /// </summary>
        [XmlAttribute("ResidentNumber")]
        public string ResidentNumber
        {
            get { return _residentNumber; }
            set { _residentNumber = value; }
        }

        /// <summary>
        /// Service Address Line 1
        /// Source : XXX_HOUSE_MASTER.ADDRESS_LINE_1
        /// </summary>
        [XmlAttribute("ServiceAddressLine1")]
        public string ServiceAddressLine1
        {
            get { return _serviceAddressLine1; }
            set { _serviceAddressLine1 = value; }
        }
        /// <summary>
        /// Service Address Line 2
        /// Source : XXX_HOUSE_MASTER.ADDRESS_LINE_2
        /// </summary>
        [XmlAttribute("ServiceAddressLine2")]
        public string ServiceAddressLine2
        {
            get { return _serviceAddressLine2; }
            set { _serviceAddressLine2 = value; }
        }
        /// <summary>
        /// Service Address Line 3
        /// Source : XXX_HOUSE_MASTER.ADDRESS_LINE_3
        /// </summary>
        [XmlAttribute("ServiceAddressLine3")]
        public string ServiceAddressLine3
        {
            get { return _serviceAddressLine3; }
            set { _serviceAddressLine3 = value; }
        }
        /// <summary>
        /// Service Address Line 4
        /// Source : XXX_HOUSE_MASTER.ADDRESS_LINE_4
        /// </summary>
        [XmlAttribute("ServiceAddressLine4")]
        public string ServiceAddressLine4
        {
            get { return _serviceAddressLine4; }
            set { _serviceAddressLine4 = value; }
        }

        /// <summary>
        /// ICOMS Zip Code - 4 Digits
        /// Source  :  XXX_CUSTOMER_MASTER.Zip_Code_4
        /// </summary>        /// 
        [XmlAttribute("ZipCode4")]
        public String ZipCode4
        {
            get { return _zipCode4; }
            set { _zipCode4 = value; }
        }

        /// <summary>
        /// ICOMS Zip Code - 5 Digits
        /// Source  :  XXX_CUSTOMER_MASTER.Zip_Code_5
        /// </summary>
        [XmlAttribute("ZipCode5")]
        public string ZipCode5
        {
            get { return _zipCode5; }
            set { _zipCode5 = value; }
        }

        /// <summary>
        /// Account status in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.CUSTOMER_TYPE
        /// </summary>
        [XmlAttribute("CustomerType")]
        public string CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; }
        }
        
        /// <summary>
       /// Account status in ICOMS
       /// Source : XXX_CUSTOMER_MASTER.STATUS_CODE
       /// </summary>
       [XmlAttribute( "AccountStatus")] 
        public string AccountStatus
        {
            get { return _accountStatus; }
            set { _accountStatus = value; }
        }

        /// <summary>
        /// Account status in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.STATUS_CODE
        /// </summary>
        [XmlAttribute("CollectionStatus")]
        public string CollectionStatus
        {
            get { return _collectionStatus; }
            set { _collectionStatus = value; }
        }

        /// <summary>
        /// Past Due Amount Balance - 7,2 Digits
        /// Source : Past_Due_Balance 
        /// </summary>
        [XmlAttribute("PastDueBalance")]
        public double PastDueBalance
        {
            get { return _pastDueBalance; }
            set { _pastDueBalance = value; }
        }

        /// <summary>
        /// Date of when account was establised for customer
        /// Source : XXX_CUSTOMER_MASTER.??? 
        /// </summary>
        [XmlAttribute("CustomerEstablishedDate")]
        public DateTime CustomerEstablishedDate
        {
            get { return _customerEstablishedDate; }
            set { _customerEstablishedDate = value; }
        } 

        /// <summary>
        /// Date customer disconnects all services with Cox; when they become  "former"
        /// Source: XXX_CUSTOMER_MASTER.STATUS_DATES, where customer status = 'F'
        /// </summary>
        [XmlAttribute("CustomerDisconnectDate")]
        public DateTime CustomerDisconnectDate
        {
            get { return _customerDisconnectDate; }
            set { _customerDisconnectDate = value; }
        }        

        /// <summary>
        /// Residential Privacy Flag for Do Not Call National Registry
        /// Source : FTC_DNC_REGISTRY (residential), FTC_WLDNC_REGISTRY (wireless)
        /// </summary>
       [XmlAttribute( "DoNotCallResidential")]  
        public bool DoNotCallResidential
        {
            get { return _doNotCallResidential; }
            set { _doNotCallResidential = value; }
        }

        /// <summary>
        /// Wireless Privacy Flag for Do Not Call National Registry
        /// Source : FTC_DNC_REGISTRY (residential), FTC_WLDNC_REGISTRY (wireless)
        /// </summary>
       [XmlAttribute( "DoNotCallWireless")]  
        public bool DoNotCallWireless
        {
            get { return _doNotCallWireless; }
            set { _doNotCallWireless = value; }
        }

        /// <summary>
        /// Do not accept check as a payment type for customer
        /// Source : XXX_CUSTOMER_DEMOGRAPHICS.VCR_DATA_CODE
        /// </summary>
        [XmlAttribute( "DoNotAcceptCheck")]   
        public bool DoNotAcceptCheck
        {
            get { return _doNotAcceptCheck; }
            set { _doNotAcceptCheck = value; }
        }


        /// <summary>
        /// Customer Value Segmentation as calculated by marketing sciences. 'G','S','B' or other value
        /// TODO: need to determine whether or not to use enumeration for this...
        /// Source : CCRM.ACCOUNT_PROFILE.CUSTOMER_VALUE_SEGMENTATION
        /// </summary>
        [XmlAttribute( "CustomerValueSegmentation")]  
        public string CustomerValueSegmentation
        {
            get { return _customerValueSegmentation; }
            set { _customerValueSegmentation = value; }
        }

       
        /// <summary>
        /// Dwelling Type for service address
        /// </summary>
        [XmlAttribute( "DwellingType")]  
        public string DwellingType
        {
            get { return _dwellingType; }
            set { _dwellingType = value; }
        }
          
        /// <summary>
        /// Enrolled in Easy Pay
        /// Source: XXX_CUSTOMER_MOP.Customer_MOP_Status, where CustomerMOPStatus = 'A'
        /// </summary
       [XmlAttribute( "EnrolledInEasyPay")]  
        public bool EnrolledInEasyPay
        {
            get { return _enrolledInEasyPay; }
            set { _enrolledInEasyPay = value; }
        }

        /// <summary>
        /// Enrolled in Stop Paper Bill
        /// Source: tblStopPaperBillRequest
        /// </summary>
        [XmlAttribute("EnrolledInStopPaperBill")]  
        public bool EnrolledInStopPaperBill
        {
            get { return _enrolledInStopPaperBill; }
            set { _enrolledInStopPaperBill = value; }
        }

        /// <summary>
        /// Enrolled in Email Bill Reminder
        /// Source: tblStatementsEnrolledInEmail      
        /// </summary
        [XmlAttribute("EnrolledinEmailBillReminder")]  
        public bool EnrolledinEmailBillReminder
        {
            get { return _enrolledInEmailBillReminder; }
            set { _enrolledInEmailBillReminder = value; }
        }
                
	    /// NEED TO DETERMINE
        [XmlAttribute("ContactEmailAddress")]  
	    public string ContactEmailAddress
            {
                get { return _contactEmailAddress; }
                set { _contactEmailAddress = value; }
            }

        /// <summary>
        /// Language Preference
        /// Source: XXX_Customer_Master.Language_Code
        /// </summmary 
        [XmlAttribute("LanguagePreference")]   
        public string LanguagePreference
        {
            get { return _languagePreference; }
            set { _languagePreference = value; }
        }
                
        /// <summary>
        /// Deposit Amount Required - 7,2 Digits
        /// Source : XXX_Customer_AR_Aging.Deposit_Due_Amount (Deposit Due $)
        /// </summary>
        [XmlAttribute( "DepositAmountRequired")]   
        public double DepositAmountRequired
        {
            get { return _depositAmountRequired; }
            set { _depositAmountRequired = value; }
        }


        [XmlElement("CurrentCampaignDetail")]
        public List<CurrentCampaignDetail> currentCampaignDetail
        {
            get { return _currentCampaignDetail; }
            set { _currentCampaignDetail = value; }
        }

        /// <summary>
        /// Current Active Campaigns for a Customer
        /// Source : XXX_CUSTOMER_SERVICES.CAMPAIGN_CODE, where Campaign Active = 'Y'
        /// NOTE:  Do we want to display the actual campaign code or lookup in Promotion Pricing Master the Description??
        /// </summary>
        //[XmlAttribute( "CurrentCampaignCode")]   
        //public string CurrentCampaignCode
        //{
        //    get { return _currentCampaignCode; }
        //     set { _currentCampaignCode = value; }
        //}

        /// <summary>
        /// Current Active Campaigns for a Customer
        /// Source : XXX_CUSTOMER_SERVICES.CAMPAIGN_DESCRIPTION, where Campaign Active = 'Y'
        /// NOTE:  Do we want to display the actual campaign code or lookup in Promotion Pricing Master the Description??
        /// </summary>
        //[XmlAttribute("CurrentCampaignDescription")]
        //public string CurrentCampaignDescription
        //{
        //    get { return _currentCampaignDescription; }
        //    set { _currentCampaignDescription = value; }
        //}

        /// <summary>
        /// Under Contract
        /// Source: XXX_Customer_Contract.Contract_Attached
        /// </summary
        
        [XmlAttribute( "UnderContract")]   
        public bool UnderContract
        {
            get { return _underContract; }
            set { _underContract = value; }
        }

        /// <summary>
        /// Contract Id
        /// Source: XXX_Customer_Contract.Contract_Id
        /// </summary>
        [XmlAttribute("ContractId")]   
        public int ContractId
        {
            get { return _contractId; }
            set { _contractId = value; }
        }

        /// <summary>
        /// Contract Id
        /// Source: XXX_Customer_Contract.Contract_Id
        /// </summary>
        [XmlAttribute("ContractDescription")]
        public string ContractDescription
        {
            get { return _contractDescription; }
            set { _contractDescription = value; }
        } 


        /// <summary>
        /// Monthly Recurring Revenue
        /// Source: XXX_Customer_STATEMENTS.MONTHLY_SVC_AMOUNT
        /// NOTE:  Not for sure if this is the correct value we want to use.
        /// </summary>
        [XmlAttribute( "MonthlyRecurringRevenue")]   
        public double MonthlyRecurringRevenue
        {
            get { return _monthlyRecurringRevenue; }
            set { _monthlyRecurringRevenue = value; }
        }
#endregion
     
    }

    public class CurrentCampaignDetail
    {
        #region Member Variables
        protected string _currentCampaignCode;
        protected string _currentCampaignDescription;
        #endregion Member Variables

        #region Ctors

        public CurrentCampaignDetail() { }

        public CurrentCampaignDetail(string currentCampaignCode, string currentCampaignDescription)
        {

            _currentCampaignCode = currentCampaignCode;
            _currentCampaignDescription = currentCampaignDescription;
        }
        #endregion Ctors

        #region Properties

        /// <summary>
        /// Current Active Campaigns for a Customer
        /// Source : XXX_CUSTOMER_SERVICES.CAMPAIGN_CODE, where Campaign Active = 'Y'
        /// NOTE:  Do we want to display the actual campaign code or lookup in Promotion Pricing Master the Description??
        /// </summary>
        [XmlElement("CurrentCampaignCode")]
        public string CurrentCampaignCode
        {
            get { return _currentCampaignCode; }
            set { _currentCampaignCode = value; }
        }

        /// <summary>
        /// Current Active Campaigns for a Customer
        /// Source : XXX_CUSTOMER_SERVICES.CAMPAIGN_DESCRIPTION, where Campaign Active = 'Y'
        /// NOTE:  Do we want to display the actual campaign code or lookup in Promotion Pricing Master the Description??
        /// </summary>
        [XmlElement("CurrentCampaignDescription")]
        public string CurrentCampaignDescription
        {
            get { return _currentCampaignDescription; }
            set { _currentCampaignDescription = value; }
        }
        #endregion Properties
    }
}
