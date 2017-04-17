using System;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Enterprise
{
	/// <summary>
	/// Summary description for DalPaymentReceipt.
	/// </summary>
	public class DalPaymentReceipt : Dal
	{
		#region constants
		/// <summary>
		/// Contains the key into the configuration block for retrieving the
		/// underling connection string.
		/// </summary>
		protected const string __connectionStringKey ="enterpriseConnectionString";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalPaymentReceipt() : base( __connectionStringKey ){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public string GetPaymentReceiptType(int userId)
		{
			try
			{
				using(SqlConnection sqlConnection=new SqlConnection(_connectionString))
				{
					try{sqlConnection.Open();}
					catch(Exception){throw new LogonException();}

					using(SqlCommand cmd = new SqlCommand("spGetPaymentReceiptType", sqlConnection))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;
						cmd.Parameters.Add(new SqlParameter("@paymentReceiptType", SqlDbType.Char, 3, 
							ParameterDirection.Output, false, 0, 0, string.Empty, DataRowVersion.Default, null));

						cmd.ExecuteNonQuery();

						string returnValue = string.Empty;

						try{returnValue = cmd.Parameters["@paymentReceiptType"].Value.ToString();}
						catch{/*do nothing*/}
						
						return returnValue;
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
		#endregion public methods
	}
}
