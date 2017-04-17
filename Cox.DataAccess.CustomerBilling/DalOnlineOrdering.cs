using System;
using System.Data;
using System.Data.SqlClient;

using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.CustomerBilling
{
	/// <summary>
	/// Provides a method to determine if the account meets minamum critera to order services online.
	/// </summary>
	public class DalOnlineOrdering : Dal
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
		public DalOnlineOrdering() :base(__connectionKey){}
		#endregion Ctors

		#region Public Methods
		/// <summary>
		/// Returns a data set with max account balance and past due amount info
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public OnlineOrderingSchema.OnlineOrderingRow GetOnlineOrderingInfo(int siteId)
		{
			try
			{
				using(SqlConnection conn = new SqlConnection(_connectionString))
				{
					//create cmd
					using (SqlCommand cmd = new SqlCommand("spGetOnlineOrderingInfo",conn))
					{						
						cmd.CommandType = CommandType.StoredProcedure;
					
						//set params
						cmd.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;

						//open conn
                        if (conn.State == ConnectionState.Closed)
                        {
                            try { conn.Open(); }
                            catch (Exception exx)
                            {
                                throw new LogonException(exx);
                            }
                        }
                        else
                        {
                            conn.Close();
                            try { conn.Open(); }
                            catch { throw new LogonException(); }
                        }

					
						// build the data adapter
						using (SqlDataAdapter da = new SqlDataAdapter(cmd))
						{
							// create the dataset to fill
							OnlineOrderingSchema ds = new OnlineOrderingSchema();
							// now fill it
							da.Fill(ds.OnlineOrdering);
						
							// all done, return
							return ds.OnlineOrdering.Count == 1 ? ds.OnlineOrdering[0] : null;
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
