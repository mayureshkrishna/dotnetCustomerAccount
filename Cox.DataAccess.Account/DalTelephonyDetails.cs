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
	/// Summary description for TelephonyDetails.
	/// </summary>
	public class DalTelephonyDetails : Dal
			
	{
		#region constants
		/// <summary>
		/// The connectionKey into the connections configuration section of the config block.
		/// </summary>
		protected const string __connectionKey="accountConnectionString";
		#endregion constants

		#region ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// configuration information from the configuration block.
		/// </summary>
		public DalTelephonyDetails():base(__connectionKey){}
		#endregion ctors
		
		#region public methods
		
		/// <summary>
		/// Given a siteId (up to 3 digits), siteCode and the accountNumber (up to 9 digits), this method 
		/// contructs and fills a strongly typed DistinctiveRing DataTable. Using this DataTable 
		/// does not require any knowledge of the underlying datasource. Nor does it require knowledge
		/// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		public TelephonySchema.DistinctiveRingDataTable GetDistinctiveRings(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using( OracleConnection oracleConn = new OracleConnection( _connectionString ) )
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}

					//set the sql
					string sql = "ICOMS_INTERFACE_PKG.getCustTelDRing";

					// build the command object
					using(OracleCommand cmd = new OracleCommand( sql, oracleConn ))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						//add the params
						cmd.Parameters.Add("SIDIn", OracleType.Number).Value = siteId;
						cmd.Parameters.Add("SitePrfxIn", OracleType.VarChar).Value = siteCode;
						cmd.Parameters.Add("AcctNbrIn", OracleType.Number).Value = accountNumber9;					
						cmd.Parameters.Add("RetCursor", OracleType.Cursor);
						cmd.Parameters["RetCursor"].Direction = ParameterDirection.Output;

						// build the dataadapter
						using(OracleDataAdapter da = new OracleDataAdapter( cmd ))
						{

							// create the dataset to fill
							TelephonySchema ds = new TelephonySchema();

							// now fill it
							da.Fill(ds.DistinctiveRing);

							// all done, return
							return ds.DistinctiveRing;
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