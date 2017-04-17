using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalWorkOrder.
	/// </summary>
	public class DalWorkOrder : DalCustomer
	{
		#region ctors
		/// <summary>
		/// The default constructor. It sets the connectionstring to connect
		/// to the oracle account rdbms (typically this is phastage).
		/// </summary>
		public DalWorkOrder(int siteId, string siteCode):base(siteId,siteCode){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// This method retrieves a customers workorders based on the supplied parameters.
		/// </summary>
		/// <param name="accountNumber9"></param>
		/// <param name="fromDate"></param>
		/// <returns></returns>
		public WorkOrderSchema.WOrdersDataTable GetWorkOrders(string accountNumber9, int fromDate)
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
					sql.Append( "select WOCU#,WONUM,WOSTT,WOEDT,WOETM from " );
					sql.Append( _siteCode );
					sql.Append( "_WORDMPF where WOCU#=" );
					sql.Append( accountNumber9 );
					sql.Append( " and WONROV=" );
					sql.Append( _siteId );
					sql.Append( " and WOEDT >= " );
					sql.Append( fromDate );
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn );
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter( cmd );
					// create the dataset to fill
					WorkOrderSchema ds = new WorkOrderSchema();
					// now fill it
					da.Fill( ds.WOrders );
					// all done, return
					return ds.WOrders;
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
