using System;
using System.Data;
using System.Data.OracleClient;
using System.ComponentModel;
using System.Text;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalEquipment.
	/// </summary>
	public class DalEquipment : Dal
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
		public DalEquipment():this(__connectionKey){}
		/// <summary>
		/// Constructor that takes a param for the conn key then retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalEquipment(string connectionKey):base(connectionKey){}
		#endregion 

		#region Public Methods
		/// <summary>
		/// Given a MacAddress, this method returns the customer's 9 digit accountNumber
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="macAddress"></param>
		/// <returns></returns>
		public string GetAccountFromSetTopBoxId(int siteId,string siteCode,string macAddress)
		{
			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch(Exception lex){throw new LogonException(lex);}

					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.AppendFormat( "select ACCOUNT_NUMBER from {0}_EQUIPMENT_DETAIL ",siteCode);
					sql.AppendFormat( "where SITE_ID={0} and ACCOUNT_NUMBER!=0 ",siteId);
					sql.Append( "and (port_type = 'CABLE' or port_type like 'DIG%' or port_type like 'ANA%') ");
					sql.AppendFormat( "and EQUIPMENT_ADDRESS='{0}' ",macAddress.ToUpper());
					// build the command object
					using (OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn))
					{
						cmd.CommandType = CommandType.Text;
						object ret=cmd.ExecuteScalar();
						if(ret!=null) return Convert.ToString(ret);
						return string.Empty;
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
		/// Returns an equipment dataset containing the customer's equipment info
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public EquipmentSchema GetEquipment(int siteId, string siteCode, string accountNumber9)
		{	
			OracleCommand cmd = null;
			OracleDataAdapter da = null;
			try
			{
				using( OracleConnection oracleConn = new OracleConnection( _connectionString ) )
				{
					// create the dataset to fill
					EquipmentSchema ds = new EquipmentSchema();

					// open connection
					try{oracleConn.Open();}
					catch(Exception lex){throw new LogonException(lex);}
					
					//get customer equipment
					//note: we want equipment with status codes V (waiting to be installed) 
					//		or 5 (is installed) or 7 (assinged to tech to be installed)
					StringBuilder equipmentSql = new StringBuilder();
					equipmentSql.Append( "select e.serial_number, e.item_number, ed.port_type, ed.equipment_address ");
					equipmentSql.AppendFormat( "from {0}_EQUIPMENT e, {0}_EQUIPMENT_DETAIL ed ", siteCode);
					equipmentSql.AppendFormat( "where e.account_number = {0} and e.site_id = {1}",accountNumber9,siteId);
					equipmentSql.Append( "and e.account_number = ed.account_number ");
					equipmentSql.Append( "and (e.equipment_status_code = 'V' or  e.equipment_status_code = '5' ");
					equipmentSql.Append( "or  e.equipment_status_code = '7') and e.equipment_type_code = 'C' ");
					equipmentSql.Append( "and (ed.CATEGORY_CODE = 'C' or ed.CATEGORY_CODE = 'D') ");
					equipmentSql.Append( "and e.serial_number = ed.serial_number ");
					equipmentSql.Append( "and e.item_number = ed.item_number ");
					equipmentSql.Append( "and e.site_id = ed.site_id");

					// build the command object
					cmd = new OracleCommand(equipmentSql.ToString(), oracleConn);
					cmd.CommandType = CommandType.Text;
					
					// build the dataadapter
					da = new OracleDataAdapter(cmd);

					// fill equipment info
					da.Fill(ds.CustomerEquipment);

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
				if(cmd != null)
				{
					cmd.Dispose();
				}
			}
		}
		
		#endregion
	}
}
