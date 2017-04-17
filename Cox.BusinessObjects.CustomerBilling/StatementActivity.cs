using System;
using System.Collections;
using System.Xml.Serialization;


namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// This class represents the statement activity since the last statement
	/// was generated.
	/// </summary>
	public class StatementActivity
	{
		#region member variables
		/// <summary>
		/// This represents the 16-digit account number associated with this
		/// statement instance.
		/// </summary>
		protected string _accountNumber16=null;
		/// <summary>
		/// The statement code is the 3-digit portion of the account number
		/// provided separately form the account numebr for convenience.
		/// </summary>
		protected string _statementCode= null;
		/// <summary>
		///  This is an array list of transactions associated with this
		///  statement.
		/// </summary>
		protected TransactionCollection _transactions=new TransactionCollection();
		#endregion member variables

		#region ctors
		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public StatementActivity(){}
		#endregion ctors

		#region properties
		/// <summary>
		/// This property returns the 16-digit account number on this statement.
		/// </summary>
		[XmlAttribute( "AccountNumber16" )]
		public string AccountNumber16
		{
			get{ return _accountNumber16; }
			set{ _accountNumber16 = value; }
		}
		/// <summary>
		/// This property returns the statement code on this statement.
		/// </summary>
		[XmlAttribute( "StatementCode" )]
		public string StatementCode
		{
			get{ return _statementCode; }
			set{ _statementCode = value; }
		}
		/// <summary>
		/// This property returns the list of transaction for this statement.
		/// </summary>
		[XmlArrayItemAttribute(typeof(Transaction))]
		public TransactionCollection Transactions
		{
			get{ return _transactions; }
			set{ _transactions = value; }
		}
		#endregion properties
	}
}
