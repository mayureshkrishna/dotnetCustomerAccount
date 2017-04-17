using System;
using System.Data;
using System.Data.SqlClient;

using Cox.DataAccess.Exceptions;


namespace Cox.DataAccess.CustomerBilling
{
	/// <summary>
	/// Provides a method to determine if a debit card is valid for pinless
	/// debit activity.
	/// </summary>
	public class DalPinlessDebit : Dal
	{
		#region Constants

		/// <summary>
		/// The connectionKey into the connections configuration section of the config block.
		/// </summary>
		protected const string __connectionKey = "customerBillingConnectionString";
		/// <summary>
		/// Error message for when the stored proc returns an unexpected value.
		/// </summary>
		protected const string __invalidReturnMessage = "Invalid return value from stored procedure: {0}";

		#endregion Constants

		#region Ctors

		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// configuration information from the configuration block.
		/// </summary>
		public DalPinlessDebit()
			: base( __connectionKey )
		{ /* None necessary */ }

		#endregion Ctors

		#region Public Methods

		/// <summary>
		/// This method determines if a card number meets the criteria defined
		/// within PaymentTech transmitted data.
		/// </summary>
		/// <param name="cardNumber">The card number to test</param>
		/// <returns>Pinless debit activity rights: true = allowed, false = not allowed</returns>
		public bool IsPinlessDebitCard( string cardNumber )
		{
			try
			{
				using( SqlConnection conn = new SqlConnection( _connectionString ) )
				{
					using( SqlCommand cmd = new SqlCommand( "spIsPinlessDebitCard", conn ) )
					{						
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add( "@DebitCardNumber", SqlDbType.VarChar, 19 ).Value = cardNumber;
						SqlParameter prmReturn = new SqlParameter( "IsPinlessDebitCardReturnValue", SqlDbType.Bit, 1 );
						prmReturn.Direction = ParameterDirection.ReturnValue;
						cmd.Parameters.Add( prmReturn );

						// Open the database connection
						try{ conn.Open(); }
						catch{ throw new LogonException(); }
					
						cmd.ExecuteNonQuery();
					
						// Translate the return value
						try
						{ return Convert.ToBoolean( prmReturn.Value ); }
						catch
						{
							throw new DataSourceException(
								string.Format( __invalidReturnMessage,
								prmReturn.Value.ToString() ) );
						} // catch
					} // using( SqlCommand cmd... )
				} // using( SqlConnection conn... )
			} // try
			catch( DataSourceException )
			{
				// just rethrow
				throw;
			} // catch( DataSourceException )
			catch( Exception exc )
			{
				// translate as a DataSourceException
				throw new DataSourceException( exc );
			} // catch( Exception exc )

		} // IsPinlessDebitCard()

		#endregion // Public Methods

	} // class DalPinlessDebit

} // namespace Cox.DataAccess.CustomerBilling


