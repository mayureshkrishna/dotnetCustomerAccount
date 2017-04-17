using System;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

using Cox.ActivityLog;
using Cox.BusinessObjects;
using Cox.BusinessLogic.CustomerBilling;

namespace Cox.ActivityLog.CustomerBilling
{
	/// <summary>
	/// Summary description for ActivityLog.
	/// </summary>
	public class BillingLogEntry : LogEntry
	{
		#region constants
		/// <summary>
		/// Key into connection data configuration block.
		/// </summary>
		protected const string __connectionKey="customerBillingConnectionString";
		/// <summary>
		/// Procedure name to call to insert records.
		/// </summary>
		protected const string __procedureName="spInsertActivityLogEntry";
		#endregion constants

		#region member variables
		/// <summary>
		/// Member variables containing the SiteId.
		/// </summary>
		protected int _siteId=0;
		/// <summary>
		/// Member variables containing the SetTopBoxId.
		/// </summary>
		protected string _setTopBoxId = null;
		/// <summary>
		/// Member variables containing the BillingActivityType.
		/// </summary>
		protected eBillingActivityType _billingActivityType=eBillingActivityType.Unknown;
		/// Member variables containing the CustomerAccountNumber.
		protected string _customerAccountNumber=null;
		/// Member variables containing the PaymentType.
		protected ePaymentType _paymentType = ePaymentType.Unknown;
		/// Member variables containing the Amount.
		protected double _amount=0d;
		/// Member variables containing the ConnectionManager ErrorNumber.
		protected int _cmErrorNumber=0;
		/// Member variables containing the phoneNumber.
		protected string _phoneNumber=null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// Constructor taking all information except Error Parameters.
		/// The thought it that you will probably never set the Error
		/// Information unless you have a problem and this will occur
		/// after construction.
		/// </summary>
		/// <param name="billingActivityType"></param>
		/// <param name="customerAccountNumber"></param>
		/// <param name="amount"></param>
		public BillingLogEntry( eBillingActivityType billingActivityType, 
			string customerAccountNumber, double amount ) : base()
		{
			_billingActivityType=billingActivityType;
			_customerAccountNumber=customerAccountNumber;
			_amount=amount;
		}
		/// <summary>
		/// Constructor typically used by AccountInquiry and StatementActivity
		/// </summary>
		/// <param name="billingActivityType"></param>
		/// <param name="customerAccountNumber"></param>
		public BillingLogEntry( eBillingActivityType billingActivityType, 
			string customerAccountNumber ) : base()
		{
			_billingActivityType=billingActivityType;
			_customerAccountNumber=customerAccountNumber;
		}
		
		/// <summary>
		/// Constructor used by AccountInquiry when looking up information 
		/// based on site id and set top box id.  
		/// </summary>
		/// <param name="billingActivityType"></param>
		/// <param name="siteId"></param>
		/// <param name="setTopBoxId"></param>
		public BillingLogEntry( eBillingActivityType billingActivityType, 
			int siteId, string setTopBoxId) : base()
		{
			_billingActivityType = billingActivityType;
			_siteId = siteId;
			_setTopBoxId = setTopBoxId;
		}
		
		/// <summary>
		/// Constructor typically used by InquireStatementCode
		/// </summary>
		/// <param name="billingActivityType"></param>
		/// <param name="customerAccountNumber"></param>
		/// <param name="phoneNumber"></param>
		public BillingLogEntry( eBillingActivityType billingActivityType, 
			string customerAccountNumber, string phoneNumber ) : base()
		{
			_billingActivityType=billingActivityType;
			_customerAccountNumber=customerAccountNumber;
			_phoneNumber=phoneNumber;
		}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Method to append parameters with application specific SqlParameters.
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public override void AppendParameters(SqlCommand cmd)
		{
			SqlParameter param = new SqlParameter( "@siteId",SqlDbType.Int );
			param.Value = _siteId;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@setTopBoxId",SqlDbType.VarChar,32);
			param.Value = _setTopBoxId==null?string.Empty:_setTopBoxId;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@activityType",SqlDbType.Int );
			param.Value = (int)_billingActivityType;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@customerAccountNumber",SqlDbType.VarChar, 16 );
			param.Value = _customerAccountNumber==null?string.Empty:_customerAccountNumber;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@paymentType",SqlDbType.Int );
			param.Value = _paymentType;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@amount",SqlDbType.Money );
			param.Value = _amount;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@cmErrorNumber",SqlDbType.Int );
			param.Value = _cmErrorNumber;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@phoneNumberLast4Digits",SqlDbType.VarChar, 4 );
			param.Value = _phoneNumber==null?string.Empty:_phoneNumber;
			cmd.Parameters.Add(param);

		}
		/// <summary>
		/// Returns the underlying connection string.
		/// </summary>
		/// <returns></returns>
		public override string GetConnectionKey()
		{
			return __connectionKey;
		}
		/// <summary>
		/// Returns the procedure name that this ActivityLog
		/// class uses to store its data.
		/// </summary>
		/// <returns></returns>
		public override string GetProcedureName()
		{
			return __procedureName;
		}
		/// <summary>
		/// Helper method to set errorInformation.
		/// </summary>
		/// <param name="errorMessage"></param>
		/// <param name="cmErrorNumber"></param>
		public void SetError( string errorMessage, int cmErrorNumber )
		{
			_errorText = errorMessage;
			_activityState = eActivityState.Failure;
			_cmErrorNumber = cmErrorNumber;
		}
		#endregion public methods

		#region properties
		/// <summary>
		/// SiteId Property
		/// </summary>
		public int SiteId
		{
			get{return _siteId;}
			set{_siteId=value;}
		}
		/// <summary>
		/// SetTopBoxId Property
		/// </summary>
		public string SetTopBoxId
		{
			get{return _setTopBoxId;}
			set{_setTopBoxId = value;}
		}
		/// <summary>
		/// BillingActivityType Property
		/// </summary>
		public eBillingActivityType BillingActivityType
		{
			get{return _billingActivityType;}
			set{_billingActivityType=value;}
		}
		/// <summary>
		/// CustomerAccountNumber Property
		/// </summary>
		public string CustomerAccountNumber
		{
			get{return _customerAccountNumber;}
			set{_customerAccountNumber=value;}
		}
		/// <summary>
		/// PaymentType Property
		/// </summary>
		public ePaymentType PaymentType
		{
			get{return _paymentType;}
			set{_paymentType=value;}
		}
		/// <summary>
		/// Amount Property
		/// </summary>
		public double Amount
		{
			get{return _amount;}
			set{_amount=value;}
		}
		/// <summary>
		/// CMErrorNumber Property
		/// </summary>
		public int CMErrorNumber
		{
			get{return _cmErrorNumber;}
			set{_cmErrorNumber=value;}
		}
		/// <summary>
		/// PhoneNumber Property
		/// </summary>
		public string PhoneNumber
		{
			get{return _phoneNumber;}
			set{_phoneNumber=value;}
		}
		#endregion properties
	}
	/// <summary>
	/// Defines the enumerations used by this ActivityLog class
	/// </summary>
	public enum eBillingActivityType
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// AccountInquiry
		/// </summary>
		AccountInquiry		= 1,
		/// <summary>
		/// StatementActivity
		/// </summary>
		StatementActivity	= 2,
		/// <summary>
		/// PayCheck
		/// </summary>
		PayCheck			= 3,
		/// <summary>
		/// PayCredit
		/// </summary>
		PayCredit			= 4,
		/// <summary>
		/// RecurringCheck
		/// </summary>
		RecurringCheck		= 5,
		/// <summary>
		/// RecurringCredit
		/// </summary>
		RecurringCredit		= 6,
		/// <summary>
		/// DeactivateRecurring
		/// </summary>
		DeactivateRecurring	= 7,
		/// <summary>
		/// UpdateBillOption
		/// </summary>
		UpdateBillOption	= 8,
		/// <summary>
		/// AccountAddress
		/// </summary>
		AccountAddress		= 9,
		/// <summary>
		/// GetStatementList
		/// </summary>
		GetStatementList	= 10,
		/// <summary>
		/// GetStatementPdf
		/// </summary>
		GetStatementPdf		= 11,
		/// <summary>
		/// StatementCodeInquiry
		/// </summary>
		StatementCodeInquiry = 12,
		/// <summary>
		/// PayPinlessDebit
		/// </summary>
		PayPinlessDebit = 13
	}
	/// <summary>
	/// Converter class to convert strings into the underlying eActivityType
	/// </summary>
	internal class BillingActivityTypeConverter : EnumConverter
	{
		#region member variables
		/// <summary>
		/// Member variables containing underlying activityType
		/// </summary>
		eBillingActivityType m_eat = eBillingActivityType.Unknown;
		#endregion member variables

		#region ctors
    	/// <summary>
		/// The default constructor
		/// </summary>
		public BillingActivityTypeConverter() : 
			base( typeof( eBillingActivityType ) ){}
		/// <summary>
		/// Constructor taking a string that evaluates to an underlying activityType
		/// </summary>
		/// <param name="str"></param>
		public BillingActivityTypeConverter( string str )
			: base( typeof( eBillingActivityType ) )
		{
			m_eat = ConvertFromString( str );
		}
		#endregion ctors

		#region helper functions
		/// <summary>
		/// Converts a string to the underlying activityType
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public new eBillingActivityType ConvertFromString( string str )
		{
			eBillingActivityType m_eat = eBillingActivityType.Unknown;
			try
			{
				m_eat = (eBillingActivityType) base.ConvertFromString( str );
			}
			catch{/*None necessary*/}
			return m_eat;
		}
		#endregion helper functions
	}
}
