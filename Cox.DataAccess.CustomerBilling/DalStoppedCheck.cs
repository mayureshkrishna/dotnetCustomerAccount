using System;
using System.Data;
using System.Data.SqlClient;

using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.CustomerBilling
{
	/// <summary>
	/// Provides a method to determine if bank account information has had a 'stop' put 
	/// on it.
	/// </summary>
	public class DalStoppedCheck : Dal
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
		public DalStoppedCheck() :base(__connectionKey){}
		#endregion Ctors

		#region Public Methods
		/// <summary>
		/// This method determines if a the bank account information provided has an 
		/// administrative 'stop'put on it/
		/// </summary>
		/// <param name="bankRoutingNumber">The bank's routing number or ABA number</param>
		/// <param name="bankAccountNumber">The bank account number</param>
		/// <returns></returns>
		public bool IsStoppedCheck(string bankRoutingNumber, string bankAccountNumber)
		{
			try
			{
				using(SqlConnection conn = new SqlConnection(_connectionString))
				{
					//create cmd
					using (SqlCommand cmd = new SqlCommand("spIsStoppedCheck",conn))
					{						
						cmd.CommandType = CommandType.StoredProcedure;
					
						//set params
						cmd.Parameters.Add("@scBankRoutingNumber", SqlDbType.VarChar,10).Value = bankRoutingNumber;
						cmd.Parameters.Add("@scBankAccountNumber", SqlDbType.VarChar,20).Value = bankAccountNumber;
						SqlParameter returnValueParameter = new SqlParameter("IsStoppedCheckReturnValue", SqlDbType.Bit, 1);
						returnValueParameter.Direction = ParameterDirection .ReturnValue;
						cmd.Parameters.Add(returnValueParameter);

						//open conn
						try{conn.Open();}
						catch{throw new LogonException();}
					
						//execute cmd
						cmd.ExecuteNonQuery();
					
						try
						{
							//get value from return value
							return Convert.ToBoolean(returnValueParameter.Value);
						}
						catch (Exception)
						{
							//throw DataSourceException
							throw new DataSourceException(string.Format(__invalidReturnMessage, returnValueParameter.Value.ToString()));
						}
					}
				}
			}
			catch(DataSourceException)
			{
				//just rethrow
				throw;
			}
			catch(Exception ex)
			{
				//throw DataSourceException
				throw new DataSourceException(ex);
			}
		}
		#endregion
		
	}
}
