using System;
using System.Collections;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// This object references specific information about a customer account.
	/// </summary>
	public class AccountAddress
	{
		#region member variables
		/// <summary>
		/// A 13-digit account number is unique enough across all
		/// sites, but not too specific as to reference a particular
		///  statement.
		/// </summary>
		protected string _accountNumber13=null;
		/// <summary>
		/// The site number references the proper database within which
		/// the associated account number is located.
		/// </summary>
		protected string _siteNumber=null;
		/// <summary>
		/// The site code is the three letter identifier external 
		/// datasources use for table names.
		/// </summary>
		protected string _siteCode=null;
		/// <summary>
		/// This is the first name of the customer name associated with the account number.
		/// </summary>
		protected string _firstName=null;
		/// <summary>
		/// This is the middle initial of the customer name associated with the account number.
		/// </summary>
		protected string _middleInitial=null;
		/// <summary>
		/// This is the last name of the customer name associated with the account number.
		/// </summary>
		protected string _lastName=null;
		/// <summary>
		/// This is the customer name associated with the account number.
		/// </summary>
		protected string _customerName=null;
		/// <summary>
		/// This is line one of the address associated with the account number.
		/// </summary>
		protected string _addressLine1=null;
		/// <summary>
		/// This is line two of the address associated with the account number.
		/// </summary>
		protected string _addressLine2=null;
		/// <summary>
		/// This is line three of the address associated with the account number.
		/// </summary>
		protected string _addressLine3=null;
		/// <summary>
		/// This is line four of the address associated with the account number.
		/// </summary>
		protected string _addressLine4=null;
		/// <summary>
		/// This is the city portion of the address associated with the account number.
		/// </summary>
		protected string _city=null;
		/// <summary>
		/// This is the state portion of the address associated with the account number.
		/// </summary>
		protected string _state=null;
		/// <summary>
		/// This is the zip portion of the address associated with the account number.
		/// </summary>
		protected string _zip=null;
		/// <summary>
		/// This is the home telephone number associated with the account number.
		/// </summary>
		protected string _homeTelephoneNumber=null;
		/// <summary>
		/// This is the business telephone number associated with the account number.
		/// </summary>
		protected string _businessTelephoneNumber=null;
		/// <summary>
		/// This is the other telephone number associated with the account number.
		/// </summary>
		protected string _otherTelephoneNumber=null;
		/// <summary>
        /// This is the Online Ordering Opt Out Flag associated with the account number.
        /// </summary>
        protected int _onlineOrderingOptOut = 0;
        /// <summary>
		/// This is the bill type code associated with the account number.
		/// </summary>
		protected BillType _billType = BillType.Unknown;	
		#endregion member variables

		#region ctors
		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public AccountAddress(){}
		#endregion ctors

		#region properties
		/// <summary>
		/// This property returns the 13-digit account number that describes
		/// this customer account.
		/// </summary>
		[XmlAttribute("AccountNumber13")]
		public string AccountNumber13
		{
			get{ return _accountNumber13; }
			set{ _accountNumber13=value; }
		}
		/// <summary>
		/// This property returns the site number for this customer account.
		/// </summary>
		[XmlAttribute("SiteNumber")]
		public string SiteNumber
		{
			get{ return _siteNumber; }
			set{ _siteNumber=value; }
		}
		/// <summary>
		/// This property returns the site code for this customer account.
		/// </summary>
		[XmlAttribute("SiteCode")]
		public string SiteCode
		{
			get{ return _siteCode; }
			set{ _siteCode=value; }
		}
		/// <summary>
		/// This property returns the firstName of the customerName for this account.
		/// </summary>
		[XmlAttribute("FirstName")]
		public string FirstName
		{
			get{ return _firstName; }
			set{ _firstName=value; }
		}
		/// <summary>
		/// This property returns the middleInitial of the customerName for this account.
		/// </summary>
		[XmlAttribute("MiddleInitial")]
		public string MiddleInitial
		{
			get{ return _middleInitial; }
			set{ _middleInitial=value; }
		}
		/// <summary>
		/// This property returns the lastName of the customerName for this account.
		/// </summary>
		[XmlAttribute("LastName")]
		public string LastName
		{
			get{ return _lastName; }
			set{ _lastName=value; }
		}
		/// <summary>
		/// This property returns the customerName for this account.
		/// </summary>
		[XmlAttribute("CustomerName")]
		public string CustomerName
		{
			get{ return _customerName; }
			set{ _customerName=value; }
		}
		/// <summary>
		/// This property returns the address line one on this account.
		/// </summary>
		[XmlAttribute("AddressLine1")]
		public string AddressLine1
		{
			get{ return _addressLine1; }
			set{ _addressLine1=value; }
		}
		/// <summary>
		/// This property returns the address line two on this account.
		/// </summary>
		[XmlAttribute("AddressLine2")]
		public string AddressLine2
		{
			get{ return _addressLine2; }
			set{ _addressLine2=value; }
		}
		/// <summary>
		/// This property returns the address line three on this account.
		/// </summary>
		[XmlAttribute("AddressLine3")]
		public string AddressLine3
		{
			get{ return _addressLine3; }
			set{ _addressLine3=value; }
		}
		/// <summary>
		/// This property returns the address line four on this account.
		/// </summary>
		[XmlAttribute("AddressLine4")]
		public string AddressLine4
		{
			get{ return _addressLine4; }
			set{ _addressLine4=value; }
		}
		/// <summary>
		/// This property returns the city portion of the address for this account.
		/// </summary>
		[XmlAttribute("City")]
		public string City
		{
			get{ return _city; }
			set{ _city=value; }
		}
		/// <summary>
		/// This property returns the city portion of the address for this account.
		/// </summary>
		[XmlAttribute("State")]
		public string State
		{
			get{ return _state; }
			set{ _state=value; }
		}
		/// <summary>
		/// This property returns the zip portion of the address for this account.
		/// </summary>
		[XmlAttribute("Zip")]
		public string Zip
		{
			get{ return _zip; }
			set{ _zip=value; }
		}
		/// <summary>
		/// This property returns the home telephone number on this account.
		/// </summary>
		[XmlAttribute("HomeTelephoneNumber")]
		public string HomeTelephoneNumber
		{
			get{ return _homeTelephoneNumber; }
			set{ _homeTelephoneNumber=value; }
		}
		/// <summary>
		/// This property returns the business telephone number on this account.
		/// </summary>
		[XmlAttribute("BusinessTelephoneNumber")]
		public string BusinessTelephoneNumber
		{
			get{ return _businessTelephoneNumber; }
			set{ _businessTelephoneNumber=value; }
		}
		/// <summary>
		/// This property returns the other telephone number on this account.
		/// </summary>
		[XmlAttribute("OtherTelephoneNumber")]
		public string OtherTelephoneNumber
		{
			get{ return _otherTelephoneNumber; }
			set{ _otherTelephoneNumber=value; }
		}
        /// <summary>
        /// This property returns the online ordering opt out information on this account.
        /// </summary>
        [XmlIgnore()]
        public int OnlineOrderingOptOut
        {
            get { return _onlineOrderingOptOut; }
            set { _onlineOrderingOptOut = value; }
        }
		#region jms
		/// <summary>
		/// This property returns the type of dwelling.
		/// </summary>
		[XmlAttribute("BillTypeCode")]
		public BillType BillTypeCode
		{
			get{ return _billType; }
			set{_billType=value; }
		}

		#endregion jms

		#endregion properties
	}
	/// <summary>
	/// This object references specific information about a customer account.
	/// </summary>
	public class Account:AccountAddress
	{
		#region member variables
		/// <summary>
		/// Indicates if checks payments can be accepted from the customer.
		/// </summary>
		private bool _acceptChecks;
		/// <summary>
		/// Indicates if credit card payments can be accepted from the customer.
		/// </summary>
		private bool _acceptCreditCards;
		/// <summary>
		/// This is an array list of Statement instances associated with this
		/// account number.
		/// </summary>
		private StatementCollection _statements = new StatementCollection();
		//		/// <summary>
		//		/// This is an array list of previous payment credentials associated with this
		//		/// account number.
		//		/// </summary>
		//		private PreviousPaymentCollection _previousPayments = new PreviousPaymentCollection();
		/// <summary>
		/// Indicates if the account can order services online.
		/// </summary>
		private bool _allowOnlineOrdering = true;
		#region jms
		/// <summary>Indicates if a pin exists</summary>
		protected bool _pinExists;
		/// <summary>Indicates if a ssn exists</summary>
		protected bool _ssnExists;
		#endregion jms
		#endregion member variables

		#region ctors
		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public Account(){}
		#endregion ctors

		#region properties
		/// <summary>
		/// Indicates if checks payments can be accepted from the customer.
		/// </summary>
		[XmlAttribute("AcceptChecks")]
		public bool AcceptChecks
		{
			get{ return _acceptChecks; }
			set{ _acceptChecks=value; }
		}
		/// <summary>
		/// Indicates if credit card payments can be accepted from the customer.
		/// </summary>
		[XmlAttribute("AcceptCreditCards")]
		public bool AcceptCreditCards
		{
			get{ return _acceptCreditCards; }
			set{ _acceptCreditCards=value; }
		}
		/// <summary>
		/// This property returns the statent list on this account.
		/// </summary>
		[XmlArrayItemAttribute(typeof(Statement))]
		public StatementCollection Statements
		{
			get{ return _statements; }
			set{ _statements=value; }
		}
		/// <summary>
		/// Indicates if the account can order services online.
		/// </summary>
		[XmlAttribute("AllowOnlineOrdering")]
		public bool AllowOnlineOrdering
		{
			get{ return _allowOnlineOrdering; }
			set{ _allowOnlineOrdering=value; }
		}

		#region jms
		/// <summary>
		/// Gets or sets if the pin exists
		/// </summary>
		[XmlAttribute("PinExists")]
		public bool PinExists
		{
			get{ return _pinExists; }
			set{ _pinExists=value; }
		}
		/// <summary>
		/// Gets or sets if the ssn exists
		/// </summary>
		[XmlAttribute("SsnExists")]
		public bool SsnExists
		{
			get{ return _ssnExists;}
			set{ _ssnExists=value; }
		}
		#endregion jms


		#endregion properties
	}
}
