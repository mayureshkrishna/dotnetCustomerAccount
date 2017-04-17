using System;
using System.Xml.Serialization;


namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// This calss represents a single transaction on a statement.
	/// </summary>
	public class Transaction
	{
		#region member variables
		/// <summary>
		/// This indicates the beginning effective date of the transaction.
		/// </summary>
		protected DateTime _fromDate=DateTime.Now;
		/// <summary>
		/// This indicates the ending effective date of the transaction.
		/// </summary>
		protected DateTime _toDate=DateTime.Now;
		/// <summary>
		/// This is a description of the transaction.
		/// </summary>
		protected string _description=null;
		/// <summary>
		/// This is the amount of the transaction when this applies.
		/// </summary>
		protected double _amount=0.0;
		/// <summary>
		/// This represents the type of the transaction.
		/// </summary>
		protected eTransactionType _transactionType=eTransactionType.Unknown;
		/// <summary>
		/// This represents the service category associated with the transaction.
		/// </summary>
		protected eServiceCategory _serviceCategory=eServiceCategory.Unknown;
		#endregion member variables

		#region ctors
		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public Transaction(){}
		#endregion ctors

		#region properties
		/// <summary>
		/// This property returns the from date for this transaction.
		/// </summary>
		[XmlElement( "FromDate" )]
		public DateTime FromDate
		{
			get{ return _fromDate; }
			set{ _fromDate = value; }
		}
		/// <summary>
		/// This property returns the to date for this transaction.
		/// </summary>
		[XmlElement( "ToDate" )]
		public DateTime ToDate
		{
			get{ return _toDate; }
			set{ _toDate = value; }
		}
		/// <summary>
		/// This property returns the description for this transaction.
		/// </summary>
		[XmlAttribute( "Description" )]
		public string Description
		{
			get{ return _description; }
			set{ _description = value; }
		}
		/// <summary>
		/// This property returns the amount for this transaction.
		/// </summary>
		[XmlAttribute( "Amount" )]
		public double Amount
		{
			get{ return _amount; }
			set{ _amount = value; }
		}
		/// <summary>
		/// This property returns the transaction type for this transaction.
		/// </summary>
		[XmlElement( "TransactionType" )]
		public eTransactionType TransactionType
		{
			get{ return _transactionType; }
			set{ _transactionType = value; }
		}
		/// <summary>
		/// This property returns the service category for this transaction.
		/// </summary>
		[XmlElement( "ServiceCategory" )]
		public eServiceCategory ServiceCategory
		{
			get{ return _serviceCategory; }
			set{ _serviceCategory = value; }
		}
		#endregion properties
	}
}
