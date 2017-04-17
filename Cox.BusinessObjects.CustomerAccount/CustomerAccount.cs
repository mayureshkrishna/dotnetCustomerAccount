using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerBilling;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region CustomerAccountProfile

    /// <summary>CustomerAccountProfile - Serializable object that represents one customer account profile</summary>
    [Serializable]
    public class CustomerAccountProfile
    {
        #region Member Variables

        protected int _siteId;
        protected string _accountNumber9 = string.Empty;
        protected bool _hsiAbuse = false;
        protected bool _onlineOrderBlock = false;
        protected bool _onlineOrderDelinquentBalance = false;
        protected int _companyNumber;
        protected int _divisionNumber;
        protected int _franchiseNumber;
        protected string _lastName = string.Empty;
        protected string _firstName = string.Empty;
        protected string _middleInitial = string.Empty;
        protected string _customerName = string.Empty;
        protected string _commercialBusinessName = string.Empty;
        protected string _title = string.Empty;
        protected string _houseNumber = string.Empty;
        protected string _residentNumber = string.Empty;
        protected string _serviceAddressLine1 = string.Empty;
        protected string _serviceAddressLine2 = string.Empty;
        protected string _serviceAddressLine3 = string.Empty;
        protected string _serviceAddressLine4 = string.Empty;
        protected string _city = string.Empty;
        protected string _state = string.Empty;
        protected string _zipCode4 = string.Empty;
        protected string _zipCode5 = string.Empty;
        protected string _building = string.Empty;
        protected string _apartment = string.Empty;
        protected CustomerType _customerType = CustomerType.Default;
        protected AccountStatus _accountStatus = AccountStatus.Unknown;       
        protected double _totalCurrentBalance = 0.0;
        protected DateTime _customerEstablishedDate = DateTime.MinValue;
        protected DateTime _customerDisconnectDate = DateTime.MinValue;        
        protected bool _doNotAcceptCheck = false;
        protected bool _doNotAcceptCreditCard = false;        
        protected string _customerValueSegmentation = string.Empty;
        protected DwellingType _dwellingType = DwellingType.House;
        protected string _contactEmailAddress = string.Empty;
        protected string _languagePreference = string.Empty;
        protected double _totalMonthlyRecurringRevenue = 0.0;
        protected BillType _billType = BillType.Unknown;
        protected DateTime _dateOfBirth = DateTime.MinValue;
        protected string _driversLicenseNumber = string.Empty;
        protected string _socialSecurityNumber = string.Empty;
        protected string _pinNumber = string.Empty;
        protected string _customerComment = string.Empty;
        //[30-10-2009] Start Changes for including Customer History Data 
        protected string _coxUniqueID = string.Empty;
        protected string _contractEligibleFlag = string.Empty;
        protected string _totalTenureInMonths = string.Empty;
        //[30-10-2009] End Changes for including Customer History Data 

        protected List<CurrentCampaignDetail> _currentCampaignDetails = null;
        protected List<ContractDetail> _contractDetails = null;    
        protected List<PhoneDetail> _phoneDetails = null;
        protected List<Statement> _statements = null;
        protected List<Privacy> _privacies = null;
        //[28-01-2009] Start Changes to reflect active services for an account
        protected List<AvailableService> _services;
        //[28-01-2009] End Changes to reflect active services for an account
        protected VIPCode _vipCode = VIPCode.Unknown;

        //Changes for adding tru2way status starts here
        protected int _activeTru2WayDevices;
        //Changes for adding tru2way status ends here

        //[17-05-11] Start changes for price lock enhancement
        protected List<PriceLockDetail> _priceLockDetails=null;
        //[17-05-11] Start changes for price lock enhancement

        #endregion Member Variables

        #region Ctors
        /// <summary>
		/// Default ctor
		/// </summary>
		public CustomerAccountProfile(){}

		/// <summary>
        /// <summary>
		/// Ctor taking params
		/// </summary>
        /// Property enabling the getting of siteId
       
		#endregion Ctors

		#region Properties

        /// <summary>
        /// Date of when account was establised for customer
        /// Source : ???
        /// </summary>
        [XmlElement("DateOfBirth")]
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        /// <summary>
        /// ICCOMS Drivers License Number
        /// </summary>
        [XmlAttribute("DriversLicenseNumber")]
        public string DriversLicenseNumber
        {
            get { return _driversLicenseNumber; }
            set { _driversLicenseNumber = value; }
        }


        /// <summary>
        /// ICCOMS Social Security Number
        /// </summary>
        [XmlAttribute("SocialSecurityNumber")]
        public string SocialSecurityNumber
        {
            get { return _socialSecurityNumber; }
            set { _socialSecurityNumber = value; }
        }


        /// <summary>
        /// ICCOMS Pin Number
        /// </summary>
        [XmlAttribute("PinNumber")]
        public string PinNumber
        {
            get { return _pinNumber; }
            set { _pinNumber = value; }
        }


        /// <summary>
        /// ICCOMS CustomerComment
        /// </summary>
        [XmlAttribute("CustomerComment")]
        public string CustomerComment
        {
            get { return _customerComment; }
            set { _customerComment = value; }
        }
       

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
        // Self Reg Changes Ernest Griffin ** Start **//
        /// <summary>
        /// High Speed Data Abuse flag
        /// </summary>
        [XmlAttribute("HsiAbuse")]
        public bool HsiAbuse
        {
            get { return _hsiAbuse; }
            set { _hsiAbuse = value; }
        }

        /// <summary>
        /// Online Ordering Block flag
        /// </summary>
        [XmlAttribute("OnlineOrderBlock")]
        public bool OnlineOrderBlock
        {
            get { return _onlineOrderBlock; }
            set { _onlineOrderBlock = value; }
        }

        /// <summary>
        /// Online Order Delinquency Balance
        /// </summary>
        [XmlAttribute("OnlineOrderDelinquentBalance")]
        public bool OnlineOrderDelinquentBalance
        {
            get { return _onlineOrderDelinquentBalance; }
            set { _onlineOrderDelinquentBalance = value; }
        }
        // Self Reg Changes Ernest Griffin ** End **//

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
        /// Last Name on file for Account Number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.LAST_NAME
        /// </summary>
        [XmlAttribute("MiddleInitial")]
        public string MiddleInitial
        {
            get { return _middleInitial; }
            set { _middleInitial = value; }
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
        /// Comercial Business Name
        /// Source : XXX_CUSTOMER_MASTER.Customer_business_name
        /// </summary>
        [XmlAttribute("CommercialBusinessName")]
        public string CommercialBusinessName
        {
            get { return _commercialBusinessName; }
            set { _commercialBusinessName = value; }
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
        /// ICOMS ADDR_CITY
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>        /// 
        [XmlAttribute("City")]
        public String City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// ICOMS ADDR_STATE
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>        /// 
        [XmlAttribute("State")]
        public String State
        {
            get { return _state; }
            set { _state = value; }
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
        /// ICOMS BUILDGING
        /// Source  :  XXX_CUSTOMER_MASTER.BUILDGING
        /// </summary>
        [XmlAttribute("Building")]
        public string Building
        {
            get { return _building; }
            set { _building = value; }
        }

        /// <summary>
        /// ICOMS APARTMENT
        /// Source  :  XXX_CUSTOMER_MASTER.APARTMENT
        /// </summary>
        [XmlAttribute("Apartment")]
        public string Apartment
        {
            get { return _apartment; }
            set { _apartment = value; }
        }

        /// <summary>
        /// Customer Type
        /// Source : XXX_CUSTOMER_MASTER.CUSTOMER_TYPE
        /// </summary>
        [XmlAttribute("CustomerType")]
        public CustomerType CustomerType
        {
            get { return _customerType; }
            set { _customerType = value; }
        }

        /// <summary>
       /// Account status in ICOMS
       /// Source : XXX_CUSTOMER_MASTER.STATUS_CODE
       /// </summary>
       [XmlAttribute( "AccountStatus")]
        public AccountStatus AccountStatus
        {
            get { return _accountStatus; }
            set { _accountStatus = value; }
        }
            
        /// <summary>
        /// Current Balance - 7,2 Digits
        /// Source : Past_Due_Balance 
        /// </summary>
        [XmlAttribute("TotalCurrentBalance")]
        public double TotalCurrentBalance
        {
            get { return _totalCurrentBalance; }
            set { _totalCurrentBalance = value; }
        }

        /// <summary>
        /// Date of when account was establised for customer
        /// Source : XXX_CUSTOMER_MASTER.??? 
        /// </summary>
        [XmlElement("CustomerEstablishedDate")]
        public DateTime CustomerEstablishedDate
        {
            get { return _customerEstablishedDate; }
            set { _customerEstablishedDate = value; }
        } 

        /// <summary>
        /// Date customer disconnects all services with Cox; when they become  "former"
        /// Source: XXX_CUSTOMER_MASTER.STATUS_DATES, where customer status = 'F'
        /// </summary>
        [XmlElement("CustomerDisconnectDate")]
        public DateTime CustomerDisconnectDate
        {
            get { return _customerDisconnectDate; }
            set { _customerDisconnectDate = value; }
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
        /// Do not accept credit card as a payment type for customer
        /// Source : XXX_CUSTOMER_DEMOGRAPHICS.VCR_DATA_CODE
        /// </summary>
        [XmlAttribute("DoNotAcceptCreditCard")]
        public bool DoNotAcceptCreditCard
        {
            get { return _doNotAcceptCreditCard; }
            set { _doNotAcceptCreditCard = value; }
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
        [XmlAttribute("DwellingType")]
        public DwellingType DwellingType
        {
            get { return _dwellingType; }
            set { _dwellingType = value; }
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
        /// Monthly Recurring Revenue
        /// Source: XXX_Customer_STATEMENTS.MONTHLY_SVC_AMOUNT
        /// NOTE:  Not for sure if this is the correct value we want to use.
        /// </summary>
        [XmlAttribute( "TotalMonthlyRecurringRevenue")]   
        public double TotalMonthlyRecurringRevenue
        {
            get { return _totalMonthlyRecurringRevenue; }
            set { _totalMonthlyRecurringRevenue = value; }
        }

        /// <summary>
        /// Dwelling Type for service address
        /// </summary>
        [XmlAttribute("BillType")]
        public BillType BillType
        {
            get { return _billType; }
            set { _billType = value; }
        }

        //[30-10-2009] Start Changes for including Customer History Data

        /// <summary>
        /// Unique Id for a COX customer
        /// </summary>
        [XmlAttribute("CoxUniqueID")]
        public string CoxUniqueID
        {
            get { return _coxUniqueID; }
            set { _coxUniqueID = value; }
        }

        /// <summary>
        /// Contract Eligibility flag
        /// </summary>
        [XmlAttribute("ContractEligibleFlag")]
        public string ContractEligibleFlag
        {
            get { return _contractEligibleFlag; }
            set { _contractEligibleFlag = value; }
        }

        /// <summary>
        /// Total Tenure of customer with COX.
        /// </summary>
        [XmlAttribute("TotalTenureInMonths")]
        public string TotalTenureInMonths
        {
            get { return _totalTenureInMonths; }
            set { _totalTenureInMonths = value; }
        }

        //[30-10-2009] End Changes for including Customer History Data

        [XmlElement("CurrentCampaignDetails")]
        public List<CurrentCampaignDetail> CurrentCampaignDetails
        {
            get { return _currentCampaignDetails; }
            set { _currentCampaignDetails = value; }
        }

        [XmlElement("ContractDetails")]
        public List<ContractDetail> ContractDetails
        {
            get { return _contractDetails; }
            set { _contractDetails = value; }
        }

        [XmlElement("Phones")]
        public List<PhoneDetail> Phones
        {
            get { return _phoneDetails; }
            set { _phoneDetails = value; }
        }

        [XmlElement("Statements")]
        public List<Statement> Statements
        {
            get { return _statements; }
            set { _statements = value; }

        }

        [XmlElement("Privacies")]
        public List<Privacy> Privacies
        {
            get { return _privacies; }
            set { _privacies = value; }

        }

        [XmlElement("AvailableServices")]
        public List<AvailableService> Services
        {
            get { return _services; }
            set { _services  = value; }
        }

        /// <summary>
        /// VIPCode 
        /// </summary>
        [XmlAttribute("VIPCode")]
        public VIPCode VIPCode
        {
            get { return _vipCode; }
            set { _vipCode = value; }
        }

        /// <summary>
        /// ActiveTru2WayDevice status 
        /// </summary>
        [XmlAttribute("ActiveTru2WayDevices")]
        public int ActiveTru2WayDevices
        {
            get { return _activeTru2WayDevices; }
            set { _activeTru2WayDevices = value; }
        }

        /// <summary>
        /// Customer Price Lock Details
        /// </summary>
        [XmlElement("PriceLockDetails")]
        public List<PriceLockDetail> PriceLockDetails
        {
            get { return _priceLockDetails; }
            set { _priceLockDetails = value; }
        }
        
    #endregion

    }
    #endregion CustomerAccountProfile

    #region CurrentCampaignDetail

      /// <summary>CurrentCampaignDetail - Serializable object that represents one current campaign detail</summary>
    public class CurrentCampaignDetail
    {
        #region Member Variables
        protected string _currentCampaignCode;
        protected string _currentCampaignDescription;
        //[23-09-2009] Start Changes for Current Campaign Data fetch request
        protected string _serviceCode;
        protected string _serviceOccurrence;
        protected ServiceStatus _serviceStatus = ServiceStatus.Unknown;
        protected string _serviceCategory;
        protected DiscountActive _discountActive = DiscountActive.Unknown;
        protected DateTime _campaignStartDate = DateTime.MinValue;
        protected DateTime _campaignEndDate = DateTime.MinValue;
        //[23-09-2009] End Changes for Current Campaign Data fetch request
        #endregion Member Variables

        #region Ctors

        public CurrentCampaignDetail() { }

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

        //[23-09-2009] Start Changes for Current Campaign Data fetch request

        [XmlElement("ServiceCode")]
        public string ServiceCode
        {
            get { return _serviceCode; }
            set { _serviceCode = value; }
        }

        [XmlElement("ServiceOccurrence")]
        public string ServiceOccurrence
        {
            get { return _serviceOccurrence; }
            set { _serviceOccurrence = value; }
        }

        [XmlElement("ServiceStatus")]
        public ServiceStatus ServiceStatus
        {
            get { return _serviceStatus; }
            set { _serviceStatus = value; }
        }
        //20 June 2012 CHS service category project changes
        //        [XmlElement("ServiceCategory")]
        [XmlElement(IsNullable = true, ElementName = "ServiceCategory")]
        public string ServiceCategory
        {
            get { return _serviceCategory; }
            set { _serviceCategory = value; }
        }

        [XmlElement("DiscountActive")]
        public DiscountActive DiscountActive
        {
            get { return _discountActive; }
            set { _discountActive = value; }
        }

        [XmlElement("CampaignStartDate")]
        public DateTime CampaignStartDate
        {
            get { return _campaignStartDate; }
            set { _campaignStartDate = value; }
        }

        [XmlElement("CampaignEndDate")]
        public DateTime CampaignEndDate
        {
            get { return _campaignEndDate; }
            set { _campaignEndDate = value; }
        }

        //[23-09-2009] End Changes for Current Campaign Data fetch request

        #endregion Properties
    }
    #endregion CurrentCampaignDetail

    #region ContractDetail

    /// <summary>ContractDetail - Serializable object that represents one contract detail</summary>
    public class ContractDetail
    {
        #region Member Variables
        protected string _contractId;
        protected string _contractDescription;
        protected DateTime _contractEndDate = DateTime.MinValue;
        protected DateTime _contractStartDate = DateTime.MinValue;
        protected double _earlyTerminationAssessmentAmount = 0.0;
        protected List<string> _serviceCategories;
        #endregion Member Variables

        #region Ctors

        public ContractDetail() { }

        public ContractDetail(string contractId, string contractDescription, DateTime contractEndDate, DateTime contractStartDate, double earlyTerminationAssessmentAmount, List<string> serviceCategories)
        {

            _contractId = contractId;
            _contractDescription = contractDescription;
            _contractEndDate = contractEndDate;
            _contractStartDate = contractStartDate;
            _earlyTerminationAssessmentAmount = earlyTerminationAssessmentAmount;
            _serviceCategories = serviceCategories;
        }
        #endregion Ctors

        #region Properties

        /// <summary>
        /// Contract Id
        /// Source: XXX_Customer_Contract.Contract_Id
        /// </summary>
        [XmlElement("ContractId")]
        public string ContractId
        {
            get { return _contractId; }
            set { _contractId = value; }
        }

        /// <summary>
        /// Contract Id
        /// Source: XXX_Customer_Contract.Contract_Description
        /// </summary>
        [XmlElement("ContractDescription")]
        public string ContractDescription
        {
            get { return _contractDescription; }
            set { _contractDescription = value; }
        }

        //Changes for adding contract start date starts here

        /// <summary>
        /// ContractStartDate
        /// Source : XXX_Customer_Contract.Effective_Date
        /// </summary>
        [XmlElement("ContractStartDate")]
        public DateTime ContractStartDate
        {
            get { return _contractStartDate; }
            set { _contractStartDate = value; }
        }

        //Changes for adding contract start date ends here

        /// <summary>
        /// Current Active Campaigns for a Customer
        /// Source : XXX_CUSTOMER_SERVICES.CAMPAIGN_DESCRIPTION, where Campaign Active = 'Y'
        /// NOTE:  Do we want to display the actual campaign code or lookup in Promotion Pricing Master the Description??
        /// </summary>
        [XmlElement("ContractEndDate")]
        public DateTime ContractEndDate
        {
            get { return _contractEndDate; }
            set { _contractEndDate = value; }
        }
        [XmlElement("ServiceCategories")]
        public List<string> ServiceCategories
        {
            get { return _serviceCategories; }
            set { _serviceCategories = value; }
        }
        //Changes for service_category_code ends here

        //Changes for adding Early Termination Assessment Amount starts here
        /// <summary>
        /// EarlyTerminationAssessmentAmount
        /// Source : XXX_Customer_Contract.Early_Term_Assesment_Amt
        /// </summary>
        [XmlElement("EarlyTerminationAssessmentAmount")]
        public double EarlyTerminationAssessmentAmount
        {
            get { return _earlyTerminationAssessmentAmount; }
            set { _earlyTerminationAssessmentAmount = value; }
        }

        //changes ends here

        

        #endregion Properties
    }
    #endregion ContractDetail

    #region PhoneDetail

    /// <summary>PhoneDetail - Serializable object that represents one phone detail</summary>
    public class PhoneDetail
    {
        #region Member Variables
        protected string _phoneNumber;
        protected string _phoneType;
        protected int _customerTNSequence;
        protected string _customerTNTypeId;        
        protected bool _isWireless = false;
        protected bool _doNotCall = false;
        #endregion Member Variables

        #region Ctors

        public PhoneDetail() { }
        
        public PhoneDetail(string phoneNumber, string phoneType, int customerTNSequence, string customerTNTypeId, bool isWireless, bool doNotCall)
        {
            _phoneNumber = phoneNumber;
            _phoneType = phoneType;
            _customerTNSequence = customerTNSequence;
            _customerTNTypeId = customerTNTypeId;            
            _isWireless = isWireless; 
            _doNotCall = doNotCall;
           }
        #endregion Ctors

        #region Properties

        /// <summary>
        /// Phone Number
        /// Source: 
        /// </summary>
        [XmlElement("PhoneNumber")]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        /// <summary>
        /// Phone Type
        /// Source: 
        /// </summary>
        [XmlElement("PhoneType")]
        public string PhoneType
        {
            get { return _phoneType; }
            set { _phoneType = value; }
        }

        /// <summary>
        /// Custoner TN Sequence
        /// Source : 
        /// </summary>
        [XmlElement("CustomerTNSequence")]
        public int CustomerTNSequence
        {
            get { return _customerTNSequence; }
            set { _customerTNSequence = value; }
        }

        /// <summary>
        /// Customer TN Type Id
        /// Source : 
        /// </summary>
        [XmlElement("CustomerTNTypeId")]
        public string CustomerTNTypeId
        {
            get { return _customerTNTypeId; }
            set { _customerTNTypeId = value; }
        }
                
        /// <summary>
        /// Is Phone Wireless
        /// Source : 
        /// </summary>
        [XmlElement("IsWireless")]
        public bool IsWireless
        {
            get { return _isWireless; }
            set { _isWireless = value; }
        }

        /// <summary>
        /// Do Not Call
        /// Source : 
        /// </summary>
        [XmlElement("DoNotCall")]
        public bool DoNotCall
        {
            get { return _doNotCall; }
            set { _doNotCall = value; }
        }
        #endregion Properties
    }
    
    #endregion PhoneDetail

    #region Statement

    /// <summary>Statement - Serializable object that represents one statement</summary>
    public class Statement
    {
        #region Member Variables
        protected string _statementCode;
        protected string _accountNumber16;
        protected double _monthlyRecurringRevenue = 0.0;
        protected double _currentBalance = 0.0;
        protected bool _enrolledInEasyPay = false;
        protected bool _enrolledInStopPaperBill = false;
        //[29-01-2009] Start Changes for reflecting AR amounts for Q-Matic 
        /// <summary>
        /// This is the AR 1-30 Days Bucket.
        /// </summary>
        protected double _ar1To30Amount = 0.0d;
        /// <summary>
        /// This is the AR 31-60 Days Bucket.
        /// </summary>
        protected double _ar31To60Amount = 0.0d;
        //[29-01-2009] End Changes for reflecting AR amounts for Q-Matic 
        //[05-02-2010] Start Changes for reflecting AR amounts for Q-Matic
        /// <summary>
        /// This is the AR 61-90 Days Bucket.
        /// </summary>
        protected double _ar61To90Amount = 0.0d;

        /// <summary>
        /// This is the AR 91-120 Days Bucket.
        /// </summary>
        protected double _ar91To120Amount = 0.0d;
        //[05-02-2010] End Changes for reflecting AR amount for Q-Matic
        //protected bool _enrolledInEmailBillReminder = false;
        #endregion Member Variables

        #region Ctors

        public Statement() { }

        //public Statement(string statementCode, string accountNumber16, double monthlyRecurringRevenue, double currentBalance, bool enrolledInEasyPay, bool enrolledInStopPaperBill, bool enrolledInEmailBillReminder)
        //{
        //    _statementCode = statementCode;
        //    _accountNumber16 = accountNumber16;
        //    _monthlyRecurringRevenue = monthlyRecurringRevenue;
        //    _currentBalance = currentBalance;
        //    _enrolledInEasyPay = enrolledInEasyPay;
        //    _enrolledInStopPaperBill = enrolledInStopPaperBill;
        //    _enrolledInEmailBillReminder = enrolledInEmailBillReminder;
        //}

        //[29-01-2009] Start Changes for reflecting AR amounts for Q-Matic


        public Statement(string statementCode, string accountNumber16, double monthlyRecurringRevenue, double currentBalance, bool enrolledInEasyPay, bool enrolledInStopPaperBill, double ar1To30Amount, double ar31To60Amount, double ar61To90Amount, double ar91To120Amount)
        {
            _statementCode = statementCode;
            _accountNumber16 = accountNumber16;
            _monthlyRecurringRevenue = monthlyRecurringRevenue;
            _currentBalance = currentBalance;
            _enrolledInEasyPay = enrolledInEasyPay;
            _enrolledInStopPaperBill = enrolledInStopPaperBill;
            _ar1To30Amount = ar1To30Amount;
            _ar31To60Amount = ar31To60Amount;
            _ar61To90Amount = ar61To90Amount;
            _ar91To120Amount = ar91To120Amount;
        }

        //[29-01-2009] End Changes for reflecting AR amounts for Q-Matic 

        #endregion Ctors

        #region Properties

        /// <summary>
        /// Sstatement Code
        /// Source: 
        /// </summary>
        [XmlElement("StatementCode")]
        public string StatementCode
        {
            get { return _statementCode; }
            set { _statementCode = value; }
        }

        /// <summary>
        /// 16 digit account Number
        /// Source: 
        /// </summary>
        [XmlElement("AccountNumber16")]
        public string AccountNumber16
        {
            get { return _accountNumber16; }
            set { _accountNumber16 = value; }
        }

        /// <summary>
        /// Monthly Recurring Revenue
        /// Source : 
        /// </summary>
        [XmlElement("MonthlyRecurringRevenue")]
        public double MonthlyRecurringRevenue
        {
            get { return _monthlyRecurringRevenue; }
            set { _monthlyRecurringRevenue = value; }
        }

        /// <summary>
        /// currentBalance
        /// Source : 
        /// </summary>
        [XmlElement("CurrentBalance")]
        public double CurrentBalance
        {
            get { return _currentBalance; }
            set { _currentBalance = value; }
        }

        /// <summary>
        /// Enrolled in Easy Pay
        /// Source : 
        /// </summary>
        [XmlElement("EnrolledInEasyPay")]
        public bool EnrolledInEasyPay
        {
            get { return _enrolledInEasyPay; }
            set { _enrolledInEasyPay = value; }
        }

        /// <summary>
        /// Enrolled in StopPaperBill
        /// Source : 
        /// </summary>
        [XmlElement("EnrolledInStopPaperBill")]
        public bool EnrolledInStopPaperBill
        {
            get { return _enrolledInStopPaperBill; }
            set { _enrolledInStopPaperBill = value; }
        }


        //[29-01-2009] Start Changes for reflecting AR amounts for Q-Matic


        /// <summary>
        /// This property returns the AR 1-30 Bucket.
        /// </summary>
        [XmlElement("AR1To30Amount")]
        public double AR1To30Amount
        {
            get { return _ar1To30Amount; }
            set { _ar1To30Amount = value; }
        }
        /// <summary>
        /// This property returns the AR 31-60 Bucket.
        /// </summary>
        [XmlElement("AR31To60Amount")]
        public double AR31To60Amount
        {
            get { return _ar31To60Amount; }
            set { _ar31To60Amount = value; }
        }

        //[29-01-2009] Start Changes for reflecting AR amounts for Q-Matic

        //[05-02-2010] Start Changes for reflecting AR amounts for Q-Matic

        /// <summary>
        /// This property returns the AR 61-90 Bucket.
        /// </summary>
        [XmlElement("AR61To90Amount")]
        public double AR61To90Amount
        {
            get { return _ar61To90Amount; }
            set { _ar61To90Amount = value; }
        }


        /// <summary>
        /// This property returns the AR 91-120 Bucket.
        /// </summary>
        [XmlElement("AR91To120Amount")]
        public double AR91To120Amount
        {
            get { return _ar91To120Amount; }
            set { _ar91To120Amount = value; }
        }


        //[05-02-2010] End Changes for reflecting AR amounts for Q-Matic


        // /// <summary>
        // /// Enrolled in Email Bill Reminder
        // /// Source : 
        // /// </summary>
        //[XmlElement("EnrolledInEmailBillReminder")]
        //public bool EnrolledInEmailBillReminder
        //{
        //    get { return _enrolledInEmailBillReminder; }
        //    set { _enrolledInEmailBillReminder = value; }
        //}
        #endregion Properties
    }
    #endregion Statement

    #region Privacy

    /// <summary>Privacy - Serializable object that represents one set of privacy codes and descrptions</summary>
    public class Privacy
    {
        #region Member Variables
        protected string _privacyCode;
        protected string _privacyDescription;    
        #endregion Member Variables

        #region Ctors

        public Privacy() { }

        public Privacy(string privacyCode, string privacyDescription)
        {

            _privacyCode = privacyCode;
            _privacyDescription = privacyDescription;
        }
        #endregion Ctors

        #region Properties

        /// <summary>
        /// Privacy Code
        /// Source: 
        /// </summary>
        [XmlElement("PrivacyCode")]
        public string PrivacyCode
        {
            get { return _privacyCode; }
            set { _privacyCode = value; }
        }

        /// <summary>
        /// Privacy Description
        /// Source: 
        /// </summary>
        [XmlElement("PrivacyDescription")]
        public string PrivacyDescription
        {
            get { return _privacyDescription; }
            set { _privacyDescription = value; }
        }
           
        #endregion Properties
    }
    #endregion Privacy

    #region CustomerContactInformation

    /// <summary>CustomerContactInformation - Serializable object that represents customer contact information</summary>
    //[Serializable]
    public class CustomerContactInformation
    {
        #region Member Variables

        protected int    _siteId;
        protected string _accountNumber13 = string.Empty;
        protected int    _franchiseNumber;
        protected string _lastName = string.Empty;
        protected string _firstName = string.Empty;
        protected string _middleInitial = string.Empty;
        protected string _customerName = string.Empty;
        protected string _serviceAddressLine1 = string.Empty;
        protected string _serviceAddressLine2 = string.Empty;
        protected string _serviceAddressLine3 = string.Empty;
        protected string _serviceAddressLine4 = string.Empty;
        protected string _city = string.Empty;
        protected string _state = string.Empty;
        protected string _zipCode4 = string.Empty;
        protected string _zipCode5 = string.Empty;
        protected UInt64 _phoneNumber = 0;
        protected UInt64 _businessPhoneNumber = 0;
        protected UInt64 _otherPhoneNumber = 0;
        protected UInt64 _customerTNTPhoneNumber = 0;
        protected string _billTypeCode = string.Empty;
        protected string _accountStatus = string.Empty;

        #endregion Member Variables

        #region Ctors
        /// <summary>
        /// Default ctor
        /// </summary>
        public CustomerContactInformation() { }

        /// <summary>
        /// <summary>
        /// Ctor taking params
        /// </summary>
        /// Property enabling the getting of siteId

        #endregion Ctors

        #region Properties

        /// <summary>
        /// ICCOMS Account Number - 13 Digits
        /// </summary>
        [XmlAttribute("AccountNumber13")]
        public string AccountNumber13
        {
            get { return _accountNumber13; }
            set { _accountNumber13 = value; }
        }

        /// <summary>
        /// ICOMS Site Id - 3 Digits
        /// </summary>
        [XmlAttribute("SiteId")]
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        /// <summary>
        /// First Name on file for acccount number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.FIRST_NAME
        /// </summary>
        [XmlAttribute("FirstName")]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        /// <summary>
        /// Last Name on file for Account Number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.LAST_NAME
        /// </summary>
        [XmlAttribute("MiddleInitial")]
        public string MiddleInitial
        {
            get { return _middleInitial; }
            set { _middleInitial = value; }
        }

        /// <summary>
        /// Last Name on file for Account Number in ICOMS
        /// Source : XXX_CUSTOMER_MASTER.LAST_NAME
        /// </summary>
        [XmlAttribute("LastName")]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        /// <summary>
        /// Customer Name
        /// </summary>
        [XmlAttribute("CustomerName")]
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
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
        /// ICOMS ADDR_CITY
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>        /// 
        [XmlAttribute("City")]
        public String City
        {
            get { return _city; }
            set { _city = value; }
        }

        /// <summary>
        /// ICOMS ADDR_STATE
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>        /// 
        [XmlAttribute("State")]
        public String State
        {
            get { return _state; }
            set { _state = value; }
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
        /// ICOMS Home TelephoneNumber - 10 Digits
        /// </summary>
        [XmlAttribute("PhoneNumber10")]
        public UInt64 PhoneNumber10
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        /// <summary>
        /// ICOMS Business Phone Number - 10 Digits
        /// </summary>
        [XmlAttribute("BusinessPhoneNumber")]
        public UInt64 BusinessPhoneNumber
        {
            get { return _businessPhoneNumber; }
            set { _businessPhoneNumber = value; }
        }

        /// <summary>
        /// ICOMS Other Phone Number - 5 Digits
        /// </summary>
        [XmlAttribute("OtherPhoneNumber")]
        public UInt64 OtherPhoneNumber
        {
            get { return _otherPhoneNumber; }
            set { _otherPhoneNumber = value; }
        }

        /// <summary>
        /// ICOMS Customer TN Phone Number - 5 Digits
        /// </summary>
        [XmlAttribute("CustomerTNNumber")]
        public UInt64 CustomerTNNumber
        {
            get { return _customerTNTPhoneNumber; }
            set { _customerTNTPhoneNumber = value; }
        }

        /// <summary>
        /// ICOMS Bill Type Code - 5 Digits
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>
        [XmlAttribute("BillTypeCode")]
        public string BillTypeCode
        {
            get { return _billTypeCode; }
            set { _billTypeCode = value; }
        }

        /// <summary>
        /// ICOMS Franchise Number - 3 Digits
        /// </summary>
        [XmlAttribute("FranchiseNumber")]
        public int FranchiseNumber
        {
            get { return _franchiseNumber; }
            set { _franchiseNumber = value; }
        }

        /// <summary>
        /// ICOMS Account Status
        /// Source  :  XXX_CUSTOMER_MASTER
        /// </summary>
        [XmlAttribute("AccountStatus")]
        public string AccountStatus
        {
            get { return _accountStatus; }
            set { _accountStatus = value; }
        }

        #endregion Properties

    }
    #endregion CustomerContactInformation

    //[29-01-2009] Start Changes for Reflecting Services (Active/Disconnected)

    #region Service
    public class AvailableService
    {
        #region Member Variables
        protected string _serviceCategory;
        protected ServiceStatus _serviceStatus;
        #endregion Member Variables

        #region Ctors

        public AvailableService()
        {
            
        }

        public AvailableService(string serviceCategory, ServiceStatus serviceStatus)
        {
            _serviceCategory = serviceCategory;
            _serviceStatus = serviceStatus;
        }
        #endregion Ctors

        #region Properties
        //20 June 2012 CHS service category project changes
        //[XmlElement("ServiceCategory")]
        [XmlElement(IsNullable = true, ElementName = "ServiceCategory")]
        public string ServiceCategory
        {
            get { return _serviceCategory; }
            set { _serviceCategory = value; }
        }

        [XmlElement("ServiceStatus")]
        public ServiceStatus ServiceStatus
        {
            get { return _serviceStatus; }
            set { _serviceStatus = value; }
        }

        #endregion Properties
    }

    #endregion Service

    //[29-01-2009] End Changes for Reflecting Services (Active/Disconnected)

    //[17-05-11] Start changes for price lock enhancement

    #region PriceLockDetail

    /// <summary>Price lock detail of the customer</summary>
    public class PriceLockDetail
    {
        #region Member Variables
        protected string _priceProtectionId;
        protected string _priceProtectionDescription;
        protected string _priceProtectionTermsAndConditionsId;
        protected ePriceProtectionStatus _priceProtectionStatus; 
        protected DateTime _priceProtectionStartDate;
        protected DateTime _priceProtectionEndDate;
        protected DateTime _activationDate;
        protected string _serviceCode;
        protected string _serviceOccurrence;
        protected string _serviceCategory;
        protected double _priceProtectedRate;
        #endregion Member Variables

        #region Ctors

        /// <summary>
        /// Default construtor.
        /// </summary>
        public PriceLockDetail() { }

        
        #endregion Ctors

        #region Properties

        /// <summary>
        /// Price Protection Id
        /// </summary>
        [XmlElement("PriceProtectionId")]
        public string PriceProtectionId
        {
            get { return _priceProtectionId; }
            set { _priceProtectionId = value; }
        }

        /// <summary>
        /// Price Protect Description
        /// </summary>
        [XmlElement("PriceProtectionDescription")]
        public string PriceProtectionDescription
        {
            get { return _priceProtectionDescription; }
            set { _priceProtectionDescription = value; }
        }

        /// <summary>
        /// Price Protection Terms And Conditions Id
        /// </summary>
        [XmlElement("PriceProtectionTermsAndConditionsId")]
        public string PriceProtectionTermsAndConditionsId
        {
            get { return _priceProtectionTermsAndConditionsId; }
            set { _priceProtectionTermsAndConditionsId = value; }
        }

        /// <summary>
        /// Price Protection Status
        /// </summary>
        [XmlElement("PriceProtectionStatus")]
        public ePriceProtectionStatus PriceProtectionStatus
        {
            get { return _priceProtectionStatus; }
            set { _priceProtectionStatus = value; }
        }

        /// <summary>
        /// Price Protection Start Date
        /// </summary>
        [XmlElement("PriceProtectionStartDate")]
        public DateTime PriceProtectionStartDate
        {
            get { return _priceProtectionStartDate; }
            set { _priceProtectionStartDate = value; }
        }

        /// <summary>
        /// Price Protection End Date
        /// </summary>
        [XmlElement("PriceProtectionEndDate")]
        public DateTime PriceProtectionEndDate
        {
            get { return _priceProtectionEndDate; }
            set { _priceProtectionEndDate = value; }
        }

        /// <summary>
        /// Activation Date
        /// </summary>
        [XmlElement("ActivationDate")]
        public DateTime ActivationDate
        {
            get { return _activationDate; }
            set { _activationDate = value; }
        }

        /// <summary>
        /// Service Code
        /// </summary>
        [XmlElement("ServiceCode")]
        public string ServiceCode
        {
            get { return _serviceCode; }
            set { _serviceCode = value; }
        }

        /// <summary>
        /// Service Occurrence
        /// </summary>
        [XmlElement("ServiceOccurrence")]
        public string ServiceOccurrence
        {
            get { return _serviceOccurrence; }
            set { _serviceOccurrence = value; }
        }

        /// <summary>
        /// Service Category Code
        /// </summary>
        //20 June 2012 CHS service category project changes
        //[XmlElement("ServiceCategory")]
        [XmlElement(IsNullable = true, ElementName = "ServiceCategory")]
        public string ServiceCategory
        {
            get { return _serviceCategory; }
            set { _serviceCategory = value; }
        }
        
        /// <summary>
        /// Price Protected Rate
        /// </summary>
        [XmlElement("PriceProtectedRate")]
        public double PriceProtectedRate
        {
            get { return _priceProtectedRate; }
            set { _priceProtectedRate = value; }
        }        

        #endregion Properties
    }
    #endregion PriceLockDetail

    //[17-05-11] End changes for price lock enhancement

}