using System;
using System.Collections;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// This object references data specific to aparticular statement on a
	/// particualr account.
	/// </summary>
	public class Statement
	{
		#region member variables
		/// <summary>
		/// The statement code is the 3-digit portion of the account number
		/// provided separately form the account numebr for convenience.
		/// </summary>
		protected string _statementCode=null;
		/// <summary>
		/// This represents the 16-digit account number associated with this
		/// statement instance.
		/// </summary>
		protected string _accountNumber16=null;
		/// <summary>
		/// This represents the billing option associated with this customer
		/// statement.
		/// </summary>
		protected eBillingOption _billingOption=eBillingOption.Unknown;
		/// <summary>
		/// This falg indicates that the statement is "due upon receipt."
		/// Generally, this occurs when an account has been closed or is
		/// delinquent.
		/// </summary>
		protected bool _dueOnReceipt=true;
		/// <summary>
		/// This is the due date associated with this statment. When the 
		/// DueOnReceipt flag is set, this will be the current date.
		/// </summary>
		protected DateTime _dueDate=DateTime.Now;
		/// <summary>
		/// This is equivalent to the Account Balance field on the ICOMS
		/// Green Screens.
		/// </summary>
		protected double _currentBalance=0.0d;
		/// <summary>
		/// This field is NOT sent to the client. It is the equivalent to the
		/// Current field on the ICOMS green screen.
		/// </summary>
		protected double _currentBucket=0.0d;
		/// <summary>
		/// This is the amount last billed as of the last statement date. 
		/// Unlike the CurrentBalance, it does not change as payments are
		/// made. Its value only changes once a new bill is created and
		/// sent to the customer.
		/// </summary>
		protected double _amountBilled=0.0d;
		/// <summary>
		/// This is the minimum amount due by the customer to prevent the loss
		/// of services. This value is a calculation and you can find more in
		/// the business logic design document discussing this field.
		/// </summary>
		protected double _minimumDue=0.0d;
		/// <summary>
		/// This is the write off amount retrieved from the customer ar aging table.
		/// </summary>
		protected double _writeOffAmount=0.0d;
		/// <summary>
		/// This is the deposit amount due from the customer. It is also tracked
		/// in the icoms ar aging table.
		/// </summary>
		protected double _depositDueAmount=0.0d;
		/// <summary>
		/// This is the pendign payment amount associated with an account.
		/// When electronic payments are made to an account, they are lumped
		/// into this bucket before they are processed in nightly batch processing.
		/// </summary>
		protected double _pendingPaymentAmount=0.0d;
		/// <summary>
		/// This is the unapplied payment amount associated with an account.
		/// When electronic payments are made to an account, they are lumped
		/// into nightly batch processing. Once they pass batch processing,
		/// they are lumped into this amount until the nightly batches are
		/// posted.
		/// </summary>
		protected double _unappliedPaymentAmount=0.0d;
		/// <summary>
		/// This is the unapplied adjustment amount associated with
		/// an account and it is the summation of both debit and credit
		/// balances. 
		/// </summary>
		protected double _unappliedAdjustmentAmount=0.0d;
		/// <summary>
		/// This is the unapplied debit adjustment amount associated with
		/// an account.
		/// </summary>
		protected double _unappliedDebitAdjustmentAmount=0.0d;
		/// <summary>
		/// This is the unapplied credit adjustment amount associated with
		/// an account.
		/// </summary>
		protected double _unappliedCreditAdjustmentAmount=0.0d;
		/// <summary>
		/// This is the AR 1-30 Days Bucket.
		/// </summary>
		protected double _ar1To30Amount=0.0d;
		/// <summary>
		/// This is the AR 31-60 Days Bucket.
		/// </summary>
		protected double _ar31To60Amount=0.0d;
		/// <summary>
		/// This is the AR 61-90 Days Bucket.
		/// </summary>
		protected double _ar61To90Amount=0.0d;
		/// <summary>
		/// This is the AR 91-120 Days Bucket.
		/// </summary>
		protected double _ar91To120Amount=0.0d;
		/// <summary>
		/// This is the AR 121-150 Days Bucket.
		/// </summary>
		protected double _ar121To150Amount=0.0d;
		/// <summary>
		/// This is the AR 150 Days Bucket.
		/// </summary>
		protected double _ar150PlusAmount=0.0d;
		/// <summary>
		/// This flag indicates that recurring payments are configured on this
		/// statement.
		/// </summary>
		protected bool _easyPayFlag=false;
		/// <summary>
		/// This value indicates the payment type of the recurring payment
		/// configured on this statement. When the EasyPayFlag is false, this
		/// value should be PaymentType.Unknown.
		/// </summary>
		protected ePaymentType _easyPayMopType=ePaymentType.Unknown;
		/// <summary>
		/// This is the date the last payment amount was applied to this
		/// statement.
		/// </summary>
		protected DateTime _lastStatementDate=DateTime.Now;
		/// <summary>
		/// This is the status of the statement.
		/// </summary>
		protected eStatementStatus _statementStatus=eStatementStatus.Unknown;
		/// <summary>
		/// This is the array list of recent payments. At this time an account
		/// will have no more then 3 payments in this list...it may change later.
		/// </summary>
		protected PaymentItemCollection _recentPayments = new PaymentItemCollection();
		/// <summary>
		/// This is the array list of service categoeries assocaited with this
		/// account statement.
		/// </summary>
		protected ServiceCategoryCollection _serviceCategories=new ServiceCategoryCollection();			
		/// <summary>
		/// This is the list of telephone numbers associated with with this statement.
		/// </summary>
		protected StatementPhoneNumberCollection _statementPhoneNumber = new StatementPhoneNumberCollection();
		/// <summary>
						
		#endregion member variables

		#region ctors
		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public Statement(){}
		#endregion ctors

		#region Properties
		/// <summary>
		/// This property returns the statement code on this statement.
		/// </summary>
		[XmlAttribute("StatementCode")]
		public string StatementCode
		{
			get{return _statementCode;}
			set{_statementCode = value;}
		}
		/// <summary>
		/// This property returns the 16-digit account number on this statement.
		/// </summary>
		[XmlAttribute("AccountNumber16")]
		public string AccountNumber16
		{
			get{return _accountNumber16;}
			set{_accountNumber16 = value;}
		}
		/// <summary>
		/// This property returns the billing option for this statement.
		/// </summary>
		[XmlElement("BillingOption")]
		public eBillingOption BillingOption
		{
			get{return _billingOption;}
			set{_billingOption = value;}
		}
		/// <summary>
		/// This property returns the due on receipt flag for this statement.
		/// </summary>
		[XmlAttribute("DueOnReceipt")]
		public bool DueOnReceipt
		{
			get{return _dueOnReceipt;}
			set{_dueOnReceipt = value;}
		}
		/// <summary>
		/// This property returns the due date for this statement.
		/// </summary>
		[XmlElement("DueDate")]
		public DateTime DueDate
		{
			get{return _dueDate;}
			set{_dueDate = value;}
		}
		/// <summary>
		/// This property returns the current balance on this statement.
		/// </summary>
		[XmlAttribute("CurrentBalance")]
		public double CurrentBalance
		{
			get{return _currentBalance;}
			set{_currentBalance = value;}
		}
		/// <summary>
		/// This property returns the current balance on this statement.
		/// </summary>
		[XmlIgnore()]
		public double CurrentBucket
		{
			get{return _currentBucket;}
			set{_currentBucket= value;}
		}
		/// <summary>
		/// This property returns the Amount Last Billed on this statement.
		/// </summary>
		[XmlAttribute("AmountBilled")]
		public double AmountBilled
		{
			get{return _amountBilled;}
			set{_amountBilled = value;}
		}
		/// <summary>
		/// This property gets/sets the pendingpayment amount on this account.
		/// </summary>
		[XmlAttribute("MinimumDue")]
		public double MinimumDue
		{
			get{return _minimumDue;}
			set{_minimumDue= value;}
		}
		/// <summary>
		/// This property gets/sets the pendingpayment amount on this account.
		/// </summary>
		[XmlAttribute("WriteOffAmount")]
		public double WriteOffAmount
		{
			get{return _writeOffAmount;}
			set{_writeOffAmount= value;}
		}
		/// <summary>
		/// This property gets/sets the pendingpayment amount on this account.
		/// </summary>
		[XmlAttribute("DepositDueAmount")]
		public double DepositDueAmount
		{
			get{return _depositDueAmount;}
			set{_depositDueAmount= value;}
		}
		/// <summary>
		/// This property gets/sets the pendingpayment amount on this account.
		/// </summary>
		[XmlAttribute("PendingPaymentAmount")]
		public double PendingPaymentAmount
		{
			get{return _pendingPaymentAmount;}
			set{_pendingPaymentAmount= value;}
		}
		/// <summary>
		/// This property returns the unapplied payment amount on this account.
		/// </summary>
		[XmlAttribute("UnappliedPaymentAmount")]
		public double UnappliedPaymentAmount
		{
			get{return _unappliedPaymentAmount;}
			set{_unappliedPaymentAmount = value;}
		}
		/// <summary>
		/// This property returns the unapplied adjustment amount on this account.
		/// </summary>
		[XmlAttribute("UnappliedAdjustmentAmount")]
		public double UnappliedAdjustmentAmount
		{
			get{return _unappliedAdjustmentAmount;}
			set{_unappliedAdjustmentAmount=value;}
		}
		/// <summary>
		/// This property returns the unapplied debit adjustment amount on this account.
		/// </summary>
		[XmlIgnore()]
		public double UnappliedDebitAdjustmentAmount
		{
			get{return _unappliedDebitAdjustmentAmount;}
			set{_unappliedDebitAdjustmentAmount=value;}
		}
		/// <summary>
		/// This property returns the unapplied credit adjustment amount on this account.
		/// </summary>
		[XmlIgnore()]
		public double UnappliedCreditAdjustmentAmount
		{
			get{return _unappliedCreditAdjustmentAmount;}
			set{_unappliedCreditAdjustmentAmount=value;}
		}
		#region jms
		/// <summary>
		/// This property returns the AR 1-30 Bucket.
		/// </summary>
		[XmlAttribute("AR1To30Amount")]
		public double AR1To30Amount
		{
			get{return _ar1To30Amount;}
			set{_ar1To30Amount=value;}
		}
		/// <summary>
		/// This property returns the AR 31-60 Bucket.
		/// </summary>
		[XmlAttribute("AR31To60Amount")]
		public double AR31To60Amount
		{
			get{return _ar31To60Amount;}
			set{_ar31To60Amount=value;}
		}
		/// <summary>
		/// This property returns the AR 61-90 Bucket.
		/// </summary>
		[XmlAttribute("AR61To90Amount")]
		public double AR61To90Amount
		{
			get{return _ar61To90Amount;}
			set{_ar61To90Amount=value;}
		}
		/// <summary>
		/// This property returns the AR 91-120 Bucket.
		/// </summary>
		[XmlAttribute("AR91To120Amount")]
		public double AR91To120Amount
		{
			get{return _ar91To120Amount;}
			set{_ar91To120Amount=value;}
		}
		/// <summary>
		/// This property returns the AR 121-150 Bucket.
		/// </summary>
		[XmlAttribute("AR121To150Amount")]
		public double AR121To150Amount
		{
			get{return _ar121To150Amount;}
			set{_ar121To150Amount=value;}
		}
		/// <summary>
		/// This property returns the AR 150 Plus Bucket.
		/// </summary>
		[XmlAttribute("AR150PlusAmount")]
		public double AR150PlusAmount
		{
			get{return _ar150PlusAmount;}
			set{_ar150PlusAmount=value;}
		}
		#endregion jms
		/// <summary>
		/// This property returns the EasyPay flag for this statement.
		/// </summary>
		[XmlAttribute("EasyPayFlag")]
		public bool EasyPayFlag
		{
			get{return _easyPayFlag;}
			set{_easyPayFlag = value;}
		}
		/// <summary>
		/// This property returns the EasyPay method of payment type
		/// associate with this statement.
		/// </summary>
		[XmlElement("EasyPayMopType")]
		public ePaymentType EasyPayMopType
		{
			get{return _easyPayMopType;}
			set{_easyPayMopType = value;}
		}
		/// <summary>
		/// This property returns the Status of the statement.
		/// </summary>
		[XmlElement("StatementStatus")]
		public eStatementStatus Status
		{
			get{return _statementStatus;}
			set{_statementStatus= value;}
		}
		/// <summary>
		/// This property returns the last payemnt date for this statement.
		/// </summary>
		[XmlElement("LastStatementDate")]
		public DateTime LastStatementDate
		{
			get{return _lastStatementDate;}
			set{_lastStatementDate = value;}
		}
		/// <summary>
		/// This property returns a list of last payments and when they were made.
		/// </summary>
		[XmlArrayItemAttribute(typeof(PaymentItem))]
		public PaymentItemCollection RecentPayments
		{
			get{return _recentPayments;}
			set{_recentPayments=value;}
		}
		/// <summary>
		/// This property returns the list of service categories for this
		/// statement.
		/// </summary>
		[XmlArrayItemAttribute(typeof(eServiceCategory))]
		public ServiceCategoryCollection ServiceCategories
		{
			get{return _serviceCategories;}
			set{_serviceCategories=value;}
		}
		/// <summary>
		/// This property returns the list of phone number for this statement.
		/// </summary>		
		[XmlArrayItemAttribute(typeof(StatementPhoneNumber))]
		public StatementPhoneNumberCollection StatementPhoneNumbers
		{
			get{ return _statementPhoneNumber; }
			set{ _statementPhoneNumber=value; }
		}
		
		#endregion properties
	}

}
