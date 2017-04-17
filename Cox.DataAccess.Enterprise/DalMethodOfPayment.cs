using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;


namespace Cox.DataAccess.Enterprise
{

	/// <summary>
	/// This class implemnts the Data Access Layer for anything related to 
	/// a method of payment. This handles translation of MOPs from the
	/// internal system of record numbers (arbitrary) to a integer type
	/// representation.
	/// </summary>
	public class DalMethodOfPayment : Dal
	{

		#region Constants

		/// <summary>
		/// This constant indicates the key used to lookup the database
		/// connection string for this component.
		/// </summary>
		private const string kstrConnectionStringKey = "enterpriseConnectionString";

		#endregion // Constants


		#region Construction/Destruction

		/// <summary>
		/// This the default contructor for this class.
		/// </summary>
		public DalMethodOfPayment()
			: base( kstrConnectionStringKey )
		{ /* None necessary */ }

		#endregion // Construction/Destruction


		#region Methods

		/// <summary>
		/// This method looks up an MOP using the client username and a valid
		/// payment type.
		/// </summary>
		/// <param name="strUsername">
		/// The username indicating the source client.
		/// </param>
		/// <param name="intPaymentType">
		/// The integer representation of the payment type. This is the
		/// business facing version of the MOP.
		/// </param>
		/// <returns>
		/// The version of the MOP as found within the system of record is 
		/// returned.
		/// </returns>
		public int GetMopByUserPaymentType( string strUsername, int intPaymentType )
		{
			int intReturn = 0;

			try
			{
				using( SqlConnection con = new SqlConnection( _connectionString ) )
				{
					// build the command object
					SqlCommand cmd = new SqlCommand( "spGetMopByUserPaymentType", con );
					cmd.CommandType = CommandType.StoredProcedure;
					// Parameters
					cmd.Parameters.Add( "@strUsername",SqlDbType.VarChar ).Value = strUsername;
					cmd.Parameters.Add( "@intPaymentTypeId", SqlDbType.Int ).Value = intPaymentType;
					cmd.Parameters.Add( "@intMop", SqlDbType.Int ).Direction = ParameterDirection.Output;
					// Return value
					cmd.Parameters.Add( "Return", SqlDbType.Int ).Direction = ParameterDirection.ReturnValue;
					// Open connection and execute proc
					con.Open();
					cmd.ExecuteNonQuery();
					// Get the output variable(s)
					intReturn = (int) cmd.Parameters[ "@intMop" ].Value;
					// Get the return value
					int intError = (int) cmd.Parameters[ "Return" ].Value;
					if( intError != 0 )
					{
						throw new DataSourceException( string.Format( "The data could not be found - Return code: {0}", intError ) );
					} // if( intError != 0 )
				} // using( SqlConnection con =... )
			} // try
			catch( DataSourceException )
			{
				throw;
			} // catch( DataSourceException excData )
			catch( SqlException excSql )
			{
				throw new LogonException( excSql );
			} // catch( SqlException excSql )
			catch( Exception exc )
			{
				throw new DataSourceException( exc );
			} // catch( Exception exc )

			return intReturn;
		} // GetMopByUserPaymentType()


		/// <summary>
		/// This method looks up a payment type using the client username and
		/// a valid MOP as found in the system of record.
		/// </summary>
		/// <param name="strUsername">
		/// The username indicating the source client.
		/// </param>
		/// <param name="intMop">
		/// This is the version of the MOP as found within the system of
		/// record.
		/// </param>
		/// <returns>
		/// The integer representation of the payment type. This is the
		/// business facing version of the MOP.
		/// </returns>
		public int GetPaymentTypeByUserMop( string strUsername, int intMop )
		{
			int intReturn = 0;

			try
			{
				using( SqlConnection con = new SqlConnection( _connectionString ) )
				{
					// build the command object
					SqlCommand cmd = new SqlCommand( "spTranslateICOMSMop", con );
					cmd.CommandType = CommandType.StoredProcedure;
					// Parameters
					cmd.Parameters.Add( "@IcomsMop", SqlDbType.Int ).Value = intMop;
					cmd.Parameters.Add( "@intMop", SqlDbType.Int ).Direction = ParameterDirection.Output;
					// Return value
					cmd.Parameters.Add( "Return", SqlDbType.Int ).Direction = ParameterDirection.ReturnValue;
					// Open connection and execute proc
					con.Open();
					cmd.ExecuteNonQuery();
					// Get the output variable(s)
					intReturn = (int) cmd.Parameters[ "@intMop" ].Value;
					// Get the return value
					int intError = (int) cmd.Parameters[ "Return" ].Value;
					if( intError != 0 )
					{
						throw new DataSourceException( string.Format( "The data could not be found - Return code: {0}", intError ) );
					} // if( intError != 0 )
				} // using( SqlConnection con =... )
			} // try
			catch( DataSourceException )
			{
				throw;
			} // catch( DataSourceException excData )
			catch( SqlException excSql )
			{
				throw new LogonException( excSql );
			} // catch( SqlException excSql )
			catch( Exception exc )
			{
				throw new DataSourceException( exc );
			} // catch( Exception exc )

			return intReturn;
		} // GetPaymentTypeByUserMop()

		#endregion // Methods

	} // class DalMethodOfPayment

} // namespace Cox.DataAccess.Enterprise

