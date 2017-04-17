using System;
using System.Collections;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// Contains the information used by a customer to make a previous payment
	/// </summary>
	public class PreviousPayment
	{
		#region Memeber Variables
		/// <summary>The id of the payment from the data store. Can be used for quick retrieval later</summary>
		protected int _paymentId;
		/// <summary>The type of the previous payment, e.g. Visa, Checking, etc</summary>
		protected PaymentTypes _paymentType;
		/// <summary>The last for digits of the credit or bank account number used to make the payment</summary>
		protected string _accountNumber;
		/// <summary>The date this information was last used to make a payment</summary>
		protected DateTime _lastPaymentDate;
		#endregion

		#region Properties
		/// <summary>
		/// The id of the payment from the data store. Can be used for quick retrieval later
		/// </summary>
		[XmlAttributeAttribute()]
		public int PaymentId
		{
			get{ return _paymentId; }
			set{_paymentId = value;}
		}
		/// <summary>
		/// The type of the previous payment, e.g. Visa, Checking, etc
		/// </summary>
		[XmlElementAttribute()]
		public PaymentTypes PaymentType
		{
			get{ return _paymentType; }
			set{_paymentType = value;}
		}
		/// <summary>
		/// The last for digits of the credit or bank account number used to make the payment
		/// </summary>
		[XmlAttributeAttribute()]
		public string AccountNumber
		{
			get{ return _accountNumber; }
			set{_accountNumber = value;}
		}
		/// <summary>
		/// The date this information was last used to make a payment
		/// </summary>
		[XmlElementAttribute()]
		public DateTime LastPaymentDate
		{
			get{ return _lastPaymentDate; }
			set{_lastPaymentDate = value;}
		}
		#endregion

		#region Ctors
		/// <summary>
		/// Default ctor
		/// </summary>
		public PreviousPayment(){}
		/// <summary>
		/// Ctor that takes params
		/// </summary>
		/// <param name="paymentId"></param>
		/// <param name="paymentType"></param>
		/// <param name="accountNumber"></param>
		/// <param name="lastPaymentDate"></param>
		public PreviousPayment(int paymentId, PaymentTypes paymentType, 
								string accountNumber, DateTime lastPaymentDate)
		{
			_paymentId = paymentId;
			_paymentType = paymentType;
			_accountNumber = accountNumber;
			_lastPaymentDate = lastPaymentDate;
		}
		#endregion
	}
	/// <summary>
	/// This class extends PreviousPayment by adding specific 
	/// information releated to credit cards
	/// </summary>
	public class PreviousCreditCardPayment : PreviousPayment
	{
		#region Memeber Variables
		/// <summary>The credit card expiration date</summary>
		protected DateTime _expirationDate;
		#endregion

		#region Properties
		/// <summary>
		/// The credit card expiration date
		/// </summary>
		[XmlElementAttribute()]
		public DateTime ExpirationDate
		{
			get{ return _expirationDate; }
			set{_expirationDate = value;}
		}
		#endregion

		#region Ctors
		/// <summary>
		/// Default ctor
		/// </summary>
		public PreviousCreditCardPayment(){}
		/// <summary>
		/// Ctor that takes some params
		/// </summary>
		/// <param name="paymentId"></param>
		/// <param name="paymentType"></param>
		/// <param name="accountNumber"></param>
		/// <param name="lastPaymentDate"></param>
		/// <param name="expirationDate"></param>
		public PreviousCreditCardPayment(int paymentId, PaymentTypes paymentType, 
											string accountNumber, DateTime lastPaymentDate,
											DateTime expirationDate)
											: base(paymentId, paymentType, accountNumber, lastPaymentDate)
		{
			_expirationDate = expirationDate;
		}
		#endregion

	}
	/// <summary>
	/// Extends PreviousPayment by adding specific information related to
	/// bank accounts
	/// </summary>
	public class PreviousBankAccountPayment : PreviousPayment
	{
		#region Memeber Variables
		/// <summary>The bank account routing number</summary>
		protected string _bankRoutingNumber;
		#endregion

		#region Properties
		/// <summary>
		/// The bank account routing number
		/// </summary>
		[XmlAttributeAttribute()]
		public string BankRoutingNumber
		{
			get{ return _bankRoutingNumber; }
			set{_bankRoutingNumber = value;}
		}
		#endregion

		#region Ctors
		/// <summary>
		/// Default ctor
		/// </summary>
		public PreviousBankAccountPayment(){}
		/// <summary>
		/// Ctor that takes some params
		/// </summary>
		/// <param name="paymentId"></param>
		/// <param name="paymentType"></param>
		/// <param name="accountNumber"></param>
		/// <param name="lastPaymentDate"></param>
		/// <param name="bankRoutingNumber"></param>
		public PreviousBankAccountPayment(int paymentId, PaymentTypes paymentType, 
											string accountNumber, DateTime lastPaymentDate,
											string bankRoutingNumber)
											: base(paymentId, paymentType, accountNumber, lastPaymentDate)
		{
			_bankRoutingNumber = bankRoutingNumber;
		}
		#endregion

	}
	/// <summary>
	/// Enumeration containing the types of previous payments that my be returned
	/// </summary>
	public enum PaymentTypes
	{
		/// <summary/>
		MasterCard,
		/// <summary/>
		Discover,
		/// <summary/>
		AmericanExpress,
		/// <summary/>
		Visa,
		/// <summary/>
		Checking,
		/// <summary/>
		Savings
	}
}
