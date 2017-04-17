using System;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;

using Cox.ActivityLog;
using Cox.BusinessObjects;


namespace Cox.ActivityLog.CustomerAccount
{
	/// <summary>
    /// The custoemr account service activity logger
	/// </summary>
	public class CustomerAccountLogEntry : LogEntry
	{
		#region constants

        /// <summary>
		/// Key into connection data configuration block.
		/// </summary>
        protected const string __connectionKey = "customerAccountConnectionString";
		/// <summary>
		/// Procedure name to call to insert records.
		/// </summary>
		protected const string __procedureName = "spInsertActivityLogEntry";

        #endregion constants

		#region member variables

        /// <summary>Member variables containing the SiteId.</summary>
		protected int _siteId = 0;
		/// <summary>Member variables containing the CustomerAccountActivityType.</summary>
		protected eCustomerAccountActivityType _customerAccountActivityType = eCustomerAccountActivityType.Unknown;
		/// <summary>Member variables containing the CustomerAccountNumber.</summary>
		protected string _customerAccountNumber = null;
        /// <summary>Member variables containing the ConnectionManager ErrorNumber.</summary>
		protected int _cmErrorNumber = 0;
        /// <summary>Member variables containing the note as to the operation inputs.</summary>
        protected string _note = null;

        //[02-02-09] Start Changes for Q-matic

        /// <summary>Member variables containing the phone number.</summary>
        protected string _phoneNumber = null;

        

        /// <summary>Member variables containing the street Number</summary>
	    protected string _streetNumber = null;

        //[02-02-09] End Changes for Q-matic


        #endregion member variables

		#region ctors

        //[04-02-09] Start Changes for Q-matic
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomerAccountLogEntry():base()
        {

        }
        //[04-02-09] Start Changes for Q-matic

	    /// <summary>
		/// Constructor taking all information except Error Parameters.
		/// The thought it that you will probably never set the Error
		/// Information unless you have a problem and this will occur
		/// after construction.
		/// </summary>
		/// <param name="CustomerAccountActivityType"></param>
        public CustomerAccountLogEntry(
            eCustomerAccountActivityType customerAccountActivityType,
            int siteId )
            : base()
		{
			_customerAccountActivityType = customerAccountActivityType;
            _siteId = siteId;
        }

        /// <summary>
		/// Constructor taking all information except Error Parameters.
		/// The thought it that you will probably never set the Error
		/// Information unless you have a problem and this will occur
		/// after construction.
		/// </summary>
        /// <param name="customerAccountActivityType"></param>
		/// <param name="siteID"></param>
		/// <param name="accountNumber9"></param>
        public CustomerAccountLogEntry(
            eCustomerAccountActivityType customerAccountActivityType,
            int siteId, string accountNumber9)
            : base()
		{
            _customerAccountActivityType = customerAccountActivityType;
            _siteId=siteId;
            _customerAccountNumber = accountNumber9;
		}

        /// <summary>
        /// Constructor taking all information except Error Parameters.
        /// The thought it that you will probably never set the Error
        /// Information unless you have a problem and this will occur
        /// after construction.
        /// </summary>
        /// <param name="customerAccountActivityType"></param>
        /// <param name="PhoneNumber"></param>

        public CustomerAccountLogEntry(
            eCustomerAccountActivityType customerAccountActivityType,
            string phoneNumber10)
            : base()
        {
            _customerAccountActivityType = customerAccountActivityType;
            _phoneNumber = phoneNumber10;
        }

        //[02-02-09] Start Changes for Q-matic

        public CustomerAccountLogEntry(eCustomerAccountActivityType customerAccountActivityType, string phoneNumber10, string streetNumber)
        {
            _customerAccountActivityType = customerAccountActivityType;
            _phoneNumber = phoneNumber10;
            _streetNumber = streetNumber;
            
        }

        //[02-02-09] End Changes for Q-matic

        #endregion ctors

		#region public methods
		/// <summary>
		/// Method to append parameters with application specific SqlParameters.
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public override void AppendParameters( SqlCommand cmd )
		{
			SqlParameter param = new SqlParameter( "@siteId", SqlDbType.Int );
			param.Value = _siteId;
			cmd.Parameters.Add(param);

			param = new SqlParameter( "@activityType", SqlDbType.Int );
			param.Value = (int)_customerAccountActivityType;
			cmd.Parameters.Add(param);

            param = new SqlParameter( "@customerAccountNumber", SqlDbType.VarChar, 16 );
            param.Value = _customerAccountNumber == null ? string.Empty : _customerAccountNumber;
            cmd.Parameters.Add( param );
            
            param = new SqlParameter("@cmErrorNumber", SqlDbType.Int );
            param.Value = _cmErrorNumber;
            cmd.Parameters.Add(param);

            param = new SqlParameter( "@note", SqlDbType.VarChar, 8000 );
            param.Value = ( null == _note )? string.Empty : _note;
            cmd.Parameters.Add( param );
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
			get{ return _siteId; }
			set{ _siteId=value; }
		}

        /// <summary>
		/// CustomerAccountActivityType Property
		/// </summary>
		public eCustomerAccountActivityType CustomerAccountActivityType
		{
			get{ return _customerAccountActivityType; }
			set{ _customerAccountActivityType = value; }
		}

        /// <summary>
        /// CustomerAccountNumber Property
        /// </summary>
        public string CustomerAccountNumber
        {
            get{ return _customerAccountNumber; }
            set{ _customerAccountNumber = value; }
        }

        /// <summary>
		/// CMErrorNumber Property
		/// </summary>
		public int CMErrorNumber
		{
			get{ return _cmErrorNumber; }
			set{ _cmErrorNumber = value; }
		}

        /// <summary>
        /// Note Property
        /// </summary>
        public string Note
        {
            get{ return _note; }
            set{ _note = value; }
        }

		#endregion // properties

    } // class CustomerAccountLogEntry 

	/// <summary>
	/// Defines the enumerations used by this ActivityLog class
	/// </summary>
	public enum eCustomerAccountActivityType
	{
		/// <summary>
		/// Unknown ActivityType
		/// </summary>
		Unknown		= -1,
        /// <summary>
        /// ModifyInfo ActivityType
        /// </summary>
        ModifyInformation = 1,
        /// <summary>
        /// CreateAccount ActivityType
        /// </summary>
        CreateAccount = 2,
        /// <summary>
        /// GetCustomerAccountByAccountNumberAndSiteId ActivityType
        /// </summary>
        GetCustomerAccountByAccountNumberAndSiteId = 3,

        /// <summary>
        /// CreateRoommateAccount ActivityType
        /// </summary>
        CreateRoommateAccount = 4,
    
        /// <summary>
        /// CreateRoommateAccount ActivityType
        /// </summary>
        GetCustomerAccountInformation = 5,

        //[02-02-09] Start Changes for Q-matic

        /// <summary>
        /// GetCustomerAccountByAccountNumber ActivityType
        /// </summary>
        GetCustomerAccountByAccountNumber = 26,

        /// <summary>
        /// GetCustomerAccountByPhoneNbrAndStreetNbr ActivityType
        /// </summary>
        GetCustomerAccountByPhoneNbrAndStreetNbr = 27

        //[02-02-09] End Changes for Q-matic

    } // enum eCustomerAccountActivityType

    /// <summary>
	/// Converter class to convert strings into the underlying eActivityType
	/// </summary>
	internal class CustomerAccountActivityTypeConverter : EnumConverter
	{
		#region member variables

        /// <summary>
		/// Member variables containing underlying activityType
		/// </summary>
		eCustomerAccountActivityType _eat = eCustomerAccountActivityType.Unknown;

        #endregion member variables

		#region ctors

        /// <summary>
		/// The default constructor
		/// </summary>
		public CustomerAccountActivityTypeConverter() : 
			base( typeof( eCustomerAccountActivityType ) ){}
		/// <summary>
		/// Constructor taking a string that evaluates to an underlying activityType
		/// </summary>
		/// <param name="str"></param>
		public CustomerAccountActivityTypeConverter( string str )
			: base( typeof( eCustomerAccountActivityType ) )
		{
			_eat = ConvertFromString( str );
		}

        #endregion ctors

		#region helper functions

        /// <summary>
		/// Converts a string to the underlying activityType
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public new eCustomerAccountActivityType ConvertFromString( string str )
		{
			eCustomerAccountActivityType _eat = eCustomerAccountActivityType.Unknown;
			try
			{
				_eat = (eCustomerAccountActivityType) base.ConvertFromString( str );
			}
			catch{/*None necessary*/}
			return _eat;
		}

		#endregion // helper functions

    } // class CustomerAccountActivityTypeConverter

} // Cox.ActivityLog.CustomerAccount