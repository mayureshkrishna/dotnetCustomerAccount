using System;
using System.Xml.Serialization;

using Cox.BusinessObjects;


namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// An instance of this clas is returned when a payment is made on a
	/// customer statement.
	/// </summary>
	public class PaymentReceipt
	{
		#region member variables
		/// <summary>
		/// This is the 16-digit account number associated with the
		/// account statement against which a payment was made.
		/// </summary>
		protected string _accountNumber16=null;
		/// <summary>
		/// This is the amount of the payment on the associated statement.
		/// </summary>
		protected double _amountPaid=0.0;
		/// <summary>
		/// This is the server date the payment was recorded.
		/// </summary>
		protected DateTime _transactionDate=DateTime.Now;
		/// <summary>
		/// This is the payment type used to make the payment.
		/// </summary>
		protected ePaymentType _paymentType=ePaymentType.Unknown;
		/// <summary>
		/// This is the server status of the payment.
		/// </summary>
		protected ePaymentStatus _status=ePaymentStatus.Unknown;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public PaymentReceipt(){}
		/// <summary>
		/// The parameterized constructor.
		/// </summary>
		/// <param name="accountNumber16"></param>
		/// <param name="amountPaid"></param>
		/// <param name="transactionDate"></param>
		/// <param name="paymentType"></param>
		/// <param name="status"></param>
		public PaymentReceipt(string accountNumber16,double amountPaid,
			DateTime transactionDate,ePaymentType paymentType,ePaymentStatus status)
		{
			_accountNumber16=accountNumber16;
			_amountPaid=amountPaid;
			_transactionDate=transactionDate;
			_paymentType=paymentType;
			_status=status;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// This property returns the 16-digit account number associated with
		/// this payment.
		/// </summary>
		[XmlAttribute( "AccountNumber16" )]
		public string AccountNumber16
		{
			get{ return _accountNumber16; }
			set{ _accountNumber16 = value; }
		}
		/// <summary>
		/// This property returns the amount of this payment.
		/// </summary>
		[XmlAttribute( "AmountPaid" )]
		public double AmountPaid
		{
			get{ return _amountPaid; }
			set{ _amountPaid = value; }
		}
		/// <summary>
		/// This property returns the date the payment was recorded.
		/// </summary>
		[XmlElement( "TransactionDate" )]
		public DateTime TransactionDate
		{
			get{ return _transactionDate; }
			set{ _transactionDate = value; }
		}
		/// <summary>
		/// This property returns the payment type of this payment.
		/// </summary>
		[XmlElement( "PaymentType" )]
		public ePaymentType PaymentType
		{
			get{ return _paymentType; }
			set{ _paymentType = value; }
		}
		/// <summary>
		/// This property returns the payment status of this payment.
		/// </summary>
		[XmlElement( "Status" )]
		public ePaymentStatus Status
		{
			get{ return _status; }
			set{ _status = value; }
		}
		#endregion properties
	}
}
