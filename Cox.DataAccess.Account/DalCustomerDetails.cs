using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalCustomerDetails.
	/// </summary>
	public class DalCustomerDetails : DalAccountBase
	{
		#region ctors
		/// <summary>
		/// Default constructor.
		/// </summary>
		public DalCustomerDetails():base(__accountConnectionKey){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// This method retrieves customer HSI information.
		/// </summary>
		/// <param name="siteId">Site ID</param>
		/// <param name="siteCode">Site Code</param>
		/// <param name="accountNumber9">Customer Account Number</param>
		/// <returns>CustomerDetailsSchema.CustomerHSIDataRow</returns>
		public CustomerDetailsSchema.CustomerHSIDataRow GetHSIUserInfo(int siteId, string siteCode, string accountNumber9)
		{
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {

                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }
                    // create the sql statement
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select account_number, site_id, hsd_user_login_id ");
                    sql.AppendFormat("FROM {0}_high_speed_data_users where account_number = {1}", siteCode, accountNumber9);
                    sql.Append(" and CST_SERVICES_OCCURRENCE = 1 and site_id = ");
                    sql.AppendFormat(siteId.ToString());

                    // build the command object
                    OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn);
                    cmd.CommandType = CommandType.Text;
                    // build the dataadapter
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    // create the dataset to fill
                    CustomerDetailsSchema ds = new CustomerDetailsSchema();
                    // now fill it
                    da.Fill(ds.CustomerHSIData);
                    // all done, return
                    return ds.CustomerHSIData.Count == 1 ? ds.CustomerHSIData[0] : null;
                }
            }
            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
			
		}
		/// <summary>
		/// This method retrieves customer plan information.
		/// </summary>
		/// <param name="siteId">Site ID</param>
		/// <param name="siteCode">Site Code</param>
		/// <param name="accountNumber9">Customer Account Number</param>
		/// <returns>CustomerDetailsSchema.CustomerPlanDataRow</returns>
		public CustomerDetailsSchema.CustomerPlanDataRow GetCustomerPlanInfo(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.AppendFormat("select pln_account, site_id, pln_assignment_exp_date from {0}_plan_assignment ", siteCode);
					sql.AppendFormat("where pln_account = {0} ", accountNumber9);
					sql.AppendFormat("and site_id = {0} ", siteId.ToString());
					sql.Append(" and plan_surrogate IN ('55', '56')");
					
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn );
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter( cmd );
					// create the dataset to fill
					CustomerDetailsSchema ds = new CustomerDetailsSchema();
  					// now fill it
					da.Fill(ds.CustomerPlanData);
					// all done, return
					return ds.CustomerPlanData.Count == 1 ? ds.CustomerPlanData[0] : null;
				}
			}
			catch( LogonException )
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}

		/// <summary>
		/// This method retrieves customer Telephone data.
		/// </summary>
		/// <param name="siteId">Site ID</param>
		/// <param name="siteCode">Site Code</param>
		/// <param name="accountNumber9">Customer Account Number</param>
		/// <returns>CustomerDetailsSchema.CustomerTelephoneDataDataTable</returns>
		public CustomerDetailsSchema.CustomerTelephoneDataDataTable GetCustomerTelephoneInfo(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.Append("select a.ACCOUNT_NUMBER, a.SITE_ID, TN_CHARACTER_STRING, CUSTOMER_TN_TYPE_ID, toll_carrier, ");
					sql.AppendFormat("intra_lata_carrier, CNAM_pres_ind from {0}_customer_telephone a, {1}_customer_TN b ", siteCode, siteCode);
					sql.Append("where a.account_number = b.account_number(+) and a.site_id = b.site_id(+) and thousand_number = tn_line_number(+) ");
					sql.AppendFormat("and a.account_number = {0} and a.site_id = {1} ", accountNumber9, siteId.ToString());
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn );
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter( cmd );
					// create the dataset to fill
					CustomerDetailsSchema ds = new CustomerDetailsSchema();
					// now fill it
					da.Fill(ds.CustomerTelephoneData);
					// all done, return
					return ds.CustomerTelephoneData;					
				}
			}
			catch( LogonException )
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}
		/// <summary>
		/// This method retrieves comments about the customer.
		/// </summary>
		/// <param name="siteId">Site ID</param>
		/// <param name="siteCode">Site Code</param>
		/// <param name="accountNumber9">Customer Account Number</param>
		/// <returns>CustomerDetailsSchema.CustomerCommentsDataDataTable</returns>
		public CustomerDetailsSchema.CustomerCommentsDataDataTable GetCustomerComments(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.AppendFormat("select ACCOUNT_NUMBER, SITE_ID, COMMENT_ENTER_DATE, COMMENT_LINE, USER_ID, SEQ_NBR as SEQUENCE_NUMBER from {0}_customer_comments ", siteCode);
					sql.AppendFormat("where account_number = {0} and site_id = {1}", accountNumber9, siteId.ToString());
					
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}

					// build the command object
					using(OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn ))
					{
						cmd.CommandType = CommandType.Text;
						// build the dataadapter
						using(OracleDataAdapter da = new OracleDataAdapter( cmd ))
						{
							// create the dataset to fill
							CustomerDetailsSchema ds = new CustomerDetailsSchema();
							// now fill it
							da.Fill(ds.CustomerCommentsData);
							// all done, return
							return ds.CustomerCommentsData;
						}
					}
				}
			}
			catch( LogonException )
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}

		/// <summary>
		/// This method retrieves comments about the customer.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="fromDate"></param>
		/// <param name="toDate"></param>
		/// <returns></returns>
		public CustomerDetailsSchema.CustomerCommentsDataDataTable GetCustomerComments(int siteId, string siteCode, 
			string accountNumber9, string fromDate, string toDate)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.AppendFormat("select ACCOUNT_NUMBER, SITE_ID, COMMENT_ENTER_DATE, COMMENT_LINE, USER_ID, SEQ_NBR as SEQUENCE_NUMBER from {0}_customer_comments ", siteCode);
					sql.AppendFormat("where account_number = {0} and site_id = {1}", accountNumber9, siteId.ToString());
					sql.AppendFormat("and COMMENT_ENTER_DATE >= '{0}'and COMMENT_ENTER_DATE <='{1}'", fromDate, toDate); 

					
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}

					// build the command object
					using(OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn ))
					{
						cmd.CommandType = CommandType.Text;
						// build the dataadapter
						using(OracleDataAdapter da = new OracleDataAdapter( cmd ))
						{
							// create the dataset to fill
							CustomerDetailsSchema ds = new CustomerDetailsSchema();
							// now fill it
							da.Fill(ds.CustomerCommentsData);
							// all done, return
							return ds.CustomerCommentsData;
						}
					}
				}
			}
			catch( LogonException )
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}
		#endregion public methods
	}
}
