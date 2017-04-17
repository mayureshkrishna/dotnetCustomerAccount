using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalEquipment.
	/// </summary>
	public class DalCollections : Dal
	{
		#region Constants
		/// <summary>
		/// The connectionKey into the connections configuration section of the config block.
		/// </summary>
		protected const string __connectionKey="accountConnectionString";
		#endregion constants
		
		#region Ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalCollections():this(__connectionKey){}
		/// <summary>
		/// Constructor that takes a param for the conn key then retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalCollections(string connectionKey):base(connectionKey){}
		#endregion 

		#region Public Methods
		/// <summary>
		/// Gets the latest date that a customer can promise to pay by
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="statementCode"></param>
		/// <param name="promiseToPayDate"></param>
		/// <returns></returns>
		public string GetPromiseToPayByDate(int siteId, string siteCode, 
			string accountNumber9, int statementCode, string promiseToPayDate)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select COLLECTION_ACTION_DATE " );
					sql.AppendFormat( "from {0}_COLLECTION_CST_EVENT_ACT ", siteCode.Trim() );
					sql.AppendFormat("where SITE_ID = {0} ",siteId);
					sql.AppendFormat("and ACCOUNT_NUMBER = {0} and STATEMENT_CODE = {1} ", accountNumber9,statementCode);
					sql.Append( "and COLLECTION_EVENT_ID = 'PTP' ");
					sql.AppendFormat("and COLLECTION_ACTION_DATE >  {0} ",promiseToPayDate);
					
					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}
					
					// build the command object
					using(OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn ))
					{
						cmd.CommandType=CommandType.Text;
						using(OracleDataReader reader = cmd.ExecuteReader())
						{
							string ptpDate = null;
							//read just the first record
							if(reader.Read())
							{
								ptpDate = reader["COLLECTION_ACTION_DATE"].ToString();
							}
							return ptpDate;
						}
					}
				}
			}
			catch( DataSourceException )
			{
				// just rethrow it
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}		
		#endregion
	}
}
