using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Collections;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{

	/// <summary>
	/// DataAccessLayer component used to access the CustomerAccount 
	/// information from its system of record. 
	/// </summary>
	public class DalServiceBundle : DalCustomer
	{
		#region ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// configuration information from the configuration block.
		/// </summary>
		public DalServiceBundle(int siteId, string siteCode):base(siteId,siteCode){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// This method returns the codes associated with a particular service bundle.
		/// </summary>
		/// <param name="bundleNames"></param>
		/// <returns></returns>
		public ServiceBundleSchema.ServiceBundlesDataTable GetServiceBundleInformation(string[] bundleNames)
		{
			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the tableName
					string tableName=string.Format("{0}CMFILES.CMK2REP@ICOMSDEV,{0}CMFILES.CF06PF@ICOMSDEV",
						_siteCode);
					//string tableName=string.Format("{0}CMFILES.CMK2REP@{0},{0}CMFILES.CF06PF@{0}",_siteCode);
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.AppendFormat("select K2CVIK,E7CEK6,K2SVCD from {0} where K2NROV=E7NROV ",tableName);
					sql.AppendFormat("and K2SVCD=E7SVCD and K2NROV={0} and K2CVIK in ('",_siteId);
					for(int i=0;i<bundleNames.Length;i++)
					{
						if(i>0) sql.Append("','");
						sql.Append(bundleNames[i]);
					}
					sql.Append("') order by K2CVIK,E7CEK6,K2SVCD");
					// build the command object
					OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType = CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da = new OracleDataAdapter( cmd );
					// create the dataset to fill
					ServiceBundleSchema ds = new ServiceBundleSchema();
					// now fill it
					da.Fill(ds.ServiceBundles);
					// all done, return
					return ds.ServiceBundles;
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
