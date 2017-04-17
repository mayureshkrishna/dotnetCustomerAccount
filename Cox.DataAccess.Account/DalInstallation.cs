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
	/// Summary description for DalInstallation.
	/// </summary>
	public class DalInstallation : DalAccountBase
	{
		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalInstallation():this(__accountConnectionKey){}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalInstallation(string connectionKey):base(connectionKey){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Returns an installation dataset containing the available installation slots
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="startDate"></param>
		/// <param name="endDate"></param>
		/// <param name="poolCode"></param>
		/// <param name="qCode"></param>
		/// <param name="points"></param>
		/// <returns></returns>
		public InstallationSchema GetInstallationSlots(int siteId, string siteCode, DateTime startDate, DateTime endDate, string poolCode, string qCode, int points)
		{
			OracleCommand cmd = null;
			OracleDataAdapter da = null;

			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// create the dataset to fill
					InstallationSchema ds = new InstallationSchema();

					// open connection
					try 
					{
						oracleConn.Open();
					}
					catch
					{
						throw new LogonException();
					}

					// create the sql statement
					StringBuilder installationSql = new StringBuilder();
					installationSql.Append("SELECT SC.TIME_SLOT, SC.Q_CODE, SC.SCHEDULE_DATE, ");
					installationSql.Append("TS.SUN_START_TIME, TS.SUN_END_TIME, TS.MON_START_TIME, TS.MON_END_TIME, ");
					installationSql.Append("TS.TUE_START_TIME, TS.TUE_END_TIME, TS.WED_START_TIME, TS_WED_END_TIME, ");
					installationSql.Append("TS.THU_START_TIME, TS.THU_END_TIME, TS.FRI_START_TIME, TS.FRI_END_TIME, ");
					installationSql.Append("TS.SAT_START_TIME, TS.SAT_END_TIME ");
                    installationSql.AppendFormat("FROM {0}_CF_TIME_SLOTS TS, {0}_SCHEDULE_CALENDAR SC ", siteCode);
					installationSql.AppendFormat("WHERE TS.SITE_ID = {0} ", siteId);
                    installationSql.AppendFormat("AND SC.SCHEDULE_DATE = {0} ", startDate);
					installationSql.AppendFormat("AND SC.SCHEDULE_DATE = {0} ", endDate);
					installationSql.AppendFormat("AND TS.POOL = {0} ", poolCode);
					installationSql.AppendFormat("AND SC.Q_CODE = {0} ", qCode);
					installationSql.AppendFormat("AND (SC.POINTS_ALLOCATED - SC.POINTS_ASSIGNED) > {0} ", points);
					installationSql.Append("AND TS.POOL_TIME_SLOT = SC.TIME_SLOT ");
					installationSql.Append("AND TS.SITE_ID = SC.SITE_ID");

					// build the command object
					cmd = new OracleCommand(installationSql.ToString(), oracleConn);
					cmd.CommandType = CommandType.Text;
					
					// build the dataadapter
					da = new OracleDataAdapter(cmd);

					// fill installation slots
					da.Fill(ds.InstallationSlots);

					// all done, return
					return ds;
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
