using System;
using System.Data;
using System.Data.OracleClient;
using System.Text;

using Cox.BusinessLogic;
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalProfileEquipment.
	/// </summary>
	public class DalProfileEquipment : Dal
	{
		#region Constants
		/// <summary>
		/// The connectionKey into the connections configuration section of the config block.
		/// </summary>
		protected const string __connectionKey="equipmentConnectionString";
		#endregion constants
		
		#region Ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalProfileEquipment():this(__connectionKey){}
		/// <summary>
		/// Constructor that takes a param for the conn key then retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalProfileEquipment(string connectionKey):base(connectionKey){}
		#endregion 

		#region Public Methods
		/// <summary>
		/// Given a SetTopBoxId, this method contructs and fills a strongly typed
		/// CustomerAccount DataTable. Using this dataset does not require any knowledge
		/// of the underlying datasource. Nor does it require knowledge of the underlying
		/// data access libraries (e.g. SqlClient,OleDb,OracleClient,etc. ).
		/// </summary>
		/// <param name="setTopBoxId">SetTopBoxId is a key into ICOMS that enables 
		/// us to retrieve the correct customer account data.</param>
		/// <returns>A strongly typed dataset of type CustomerAccountSchema.CustomerAccount.</returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		public CustomerAccountSchema.CustomerAccount GetAccountFromSetTopBoxId(string setTopBoxId)
		{
			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.Append( "select distinct SITE_ID,ACCOUNT_NUMBER,COMPANY_NUMBER,DIVISION_NUMBER " );
					sql.Append( "from ALL_EQUIPMENT where SERIAL_NUMBER = '" );
					sql.Append( setTopBoxId );
					sql.Append( "' and EQUIPMENT_STATUS_CODE = '5'" );
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter(cmd);
					// create the dataset to fill
					CustomerAccountSchema ds = new CustomerAccountSchema();
					// now fill it
					da.Fill( ds.CustomerAccounts );
					// all done, return
					return ds.CustomerAccounts.Count == 1?ds.CustomerAccounts[0]:null;
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
		/// Given a SetTopBoxId, this method contructs and fills a strongly typed
		/// CustomerAccount DataTable. Using this dataset does not require any knowledge
		/// of the underlying datasource. Nor does it require knowledge of the underlying
		/// data access libraries (e.g. SqlClient,OleDb,OracleClient,etc. ). Also, this
		/// method is preferred over the version that just uses SetTopBoxId because it
		/// can go directly to the correct site table to get the information.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="setTopBoxId">SetTopBoxId is a key into ICOMS that enables 
		/// us to retrieve the correct customer account data.</param>
		/// <returns>A strongly typed dataset of type CustomerAccountSchema.CustomerAccount.</returns>
		/// <returns>A strongly typed dataset of type CustomerAccountSchema.CustomerAccount.</returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		public CustomerAccountSchema.CustomerAccount GetAccountFromSetTopBoxId(
			int siteId,string siteCode,string setTopBoxId)
		{
			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.Append( "select distinct SITE_ID,ACCOUNT_NUMBER,COMPANY_NUMBER,DIVISION_NUMBER " );
					sql.AppendFormat( "from {0}_EQUIPMENT where SERIAL_NUMBER = '{1}' and SITE_ID={2} ",
						siteCode,setTopBoxId,siteId);
					sql.Append( "and EQUIPMENT_STATUS_CODE = '5'");
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter( cmd );
					// create the dataset to fill
					CustomerAccountSchema ds = new CustomerAccountSchema();
					// now fill it
					da.Fill( ds.CustomerAccounts );
					// all done, return
					return ds.CustomerAccounts.Count == 1?ds.CustomerAccounts[0]:null;
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
		/// Returns an equipment dataset containing the customer's equipment info
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public EquipmentSchema GetEquipment(int siteId, string siteCode, string accountNumber9)
		{	
			siteId = 541;
			siteCode = "SAN";
			accountNumber9 = "65297304";
			OracleCommand cmd = null;
			OracleCommand cmd2 = null;
			OracleDataAdapter da = null;
			OracleDataAdapter da2 = null;
			try
			{
				using( OracleConnection oracleConn = new OracleConnection( _connectionString ) )
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					
					//get customer equipment
					StringBuilder equipmentSql = new StringBuilder();
					equipmentSql.Append( "select distinct serial_number, equipment_type_code ");
					equipmentSql.AppendFormat( "from {0}_EQUIPMENT ", siteCode);
					//equipmentSql.Append( "where equipment_type_code != 'O' ");
					equipmentSql.AppendFormat( "where account_number = {0} and site_id = {1}",accountNumber9,siteId);
					
					// build the command object
					cmd = new OracleCommand( equipmentSql.ToString(), oracleConn );
					cmd.CommandType = CommandType.Text;
					
					// build the dataadapter
					da = new OracleDataAdapter( cmd );

					// create the dataset to fill
					EquipmentSchema ds = new EquipmentSchema();

					// fill equipment info
					da.Fill(ds.CustomerEquipment);

					//convert date to icoms date for query
					Icoms1900Date now = new Icoms1900Date(DateTime.Now.Subtract(new TimeSpan(2500,0,0,0)));
					//Icoms1900Date now = new  Icoms1900Date(1030925);
					string time = now.Date.Hour.ToString() + now.Date.Minute.ToString();
					
					//get ppv events for account
					StringBuilder ppvSql = new StringBuilder();
					ppvSql.Append( "select distinct p.serial_number, t.event_title, p.event_start_date, ");
					ppvSql.Append( "p.event_start_time, d.end_date, d.end_time ");
					ppvSql.AppendFormat( "from {0}_title t, {0}_ppv_purchase p, {0}_pricing_group_dtl d ", siteCode);
					ppvSql.Append( "where p.title_code = t.title_code and p.event_start_date = d.start_date ");
					ppvSql.Append( "and p.event_start_time = d.start_time and p.showing_number = d.showing_number  ");
					ppvSql.AppendFormat( "and p.ACCOUNT_NUMBER = {0} and p.SITE_ID = {1} ", accountNumber9, siteId);
					ppvSql.Append( "and p.purchase_status = 'P' ");
					ppvSql.AppendFormat( "and ((d.end_date > {0}) or (d.end_date = {0} and d.end_time >= {1}))",now.ToString(), time);

					//Oracle does not support multiple recordsets like SQL Server does
					//so we have to use another cmd and da...grr					
					// build the command object
					cmd2 = new OracleCommand( ppvSql.ToString(), oracleConn );
					cmd2.CommandType = CommandType.Text;
					
					// build the dataadapter
					da2 = new OracleDataAdapter( cmd2 );

					// now fill ppv dt
					da2.Fill(ds.ActivePpvEvents);

					// all done, return
					return ds;
				}
			}
			catch(LogonException)
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch(Exception ex)
			{
				// DataSourceException.
				throw new DataSourceException(ex);
			}
			finally
			{	
				//clean up
				if(da != null)
				{
					da.Dispose();
				}
				if(da2 != null)
				{
					da2.Dispose();
				}
				if(cmd != null)
				{
					cmd.Dispose();
				}
				if(cmd2 != null)
				{
					cmd2.Dispose();
				}
			}
		}
		
		#endregion
	}
}
