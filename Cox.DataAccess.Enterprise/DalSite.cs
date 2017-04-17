using System;
using System.Text;
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
	/// Summary description for DalSite.
	/// </summary>
	public class DalSite:Dal
	{
		#region constants
		/// <summary>
		/// Contains the key into the configuration block for retrieving the
		/// underling connection string.
		/// </summary>
		protected const string __connectionStringKey = "enterpriseConnectionString";
		/// <summary>
		/// Message to use when the SiteId and/or FranchiseId could not be found.
		/// </summary>
		protected const string __notFound = "The requested SiteId and/or FranchiseId could not be located.";
		/// <summary>
		/// Message to use when an unexpected error occurred trying to get the SiteId and/or FranchiseId.
		/// </summary>
		protected const string __unexpected = "An unexpected error occurred attempting to retrieve the requested SiteId and/or FranchiseId.";
		#endregion constants

		#region ctors
		/// <summary>Private contructor only called by instance member.</summary>
		public DalSite() : base(__connectionStringKey){}
		#endregion ctors

		#region public functions
		/// <summary>
		/// This method hits the ProductOffer db to call spGetSiteIdAndFranchiseFromZip 
		/// which will find a site_id for given zip5
		/// 
		/// This method will not thrown an exception if the record is not found. Instead it will
		/// return a -1.
		/// </summary>
		/// <param name="zip5"></param>
		/// <returns></returns>
		public int ZipToSiteId(string zip5)
		{
			try
			{
				using(SqlConnection sqlConnection=new SqlConnection(_connectionString))
				{
					try{sqlConnection.Open();}
					catch(Exception){throw new LogonException();}

					using(SqlCommand cmd = new SqlCommand("spGetSiteIdFromZip",sqlConnection))
					{
						cmd.CommandType=CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("ReturnValue",
							SqlDbType.Int,4,ParameterDirection.ReturnValue,
							false,0,0,string.Empty,DataRowVersion.Default,null));

						cmd.Parameters.Add("@zipCode",SqlDbType.VarChar,5).Value=zip5;

						cmd.Parameters.Add(new SqlParameter("@siteId",SqlDbType.Int,4,ParameterDirection.Output,false,0,0,string.Empty,DataRowVersion.Default,null));

						cmd.ExecuteNonQuery();
						int ret = 0;
						try{ret = Convert.ToInt32(cmd.Parameters["ReturnValue"].Value);}
						catch{/*do nothing*/}
						switch(ret)
						{
							case 0: case 2:
								break;
							default:
								throw new DataSourceException(__unexpected);
						}
						return ret==0?Convert.ToInt32(cmd.Parameters["@siteId"].Value):0;
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
		/// <summary>
		/// This method hits the ProductOffer db to call spGetSiteIdAndFranchiseFromZip 
		/// which will find a site_id for given zip5
		/// 
		/// This method will thrown a RecordDoesNotExistException if the record is not found.
		/// </summary>
		/// <param name="zip5"></param>
		/// <returns></returns>
		public SiteIdAndFranchiseId GetSiteIdAndFranchiseIdFromZip(string zip5)
		{
			try
			{
				using(SqlConnection sqlConnection=new SqlConnection(_connectionString))
				{
					try{sqlConnection.Open();}
					catch(Exception){throw new LogonException();}

					using(SqlCommand cmd = new SqlCommand("spGetSiteIdAndFranchiseFromZip",sqlConnection))
					{
						cmd.CommandType=CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("ReturnValue",
							SqlDbType.Int,4,ParameterDirection.ReturnValue,
							false,0,0,string.Empty,DataRowVersion.Default,null));

						cmd.Parameters.Add("@zipCode",SqlDbType.VarChar,5).Value=zip5;
						cmd.Parameters.Add(new SqlParameter("@siteId",SqlDbType.Int,4,ParameterDirection.Output,false,0,0,string.Empty,DataRowVersion.Default,null));
						cmd.Parameters.Add(new SqlParameter("@franchiseId",SqlDbType.Int,4,ParameterDirection.Output,false,0,0,string.Empty,DataRowVersion.Default,null));

						cmd.ExecuteNonQuery();
					
						int ret = 0;
						try{ret = Convert.ToInt32(cmd.Parameters["ReturnValue"].Value);}
						catch{/*do nothing*/}
						switch(ret)
						{
							case 0:
								break;
							case 2:
								throw new RecordDoesNotExistException(__notFound);
							default:
								throw new DataSourceException(__unexpected);
						}

						// get output values
						int siteId=0;
						try{siteId=Convert.ToInt32(cmd.Parameters["@siteId"].Value);}
						catch{/* don't care */}

						int franchiseId=0;
						try{franchiseId = Convert.ToInt32(cmd.Parameters["@franchiseId"].Value);}
						catch{/* don't care */}

						return new SiteIdAndFranchiseId(siteId, franchiseId);
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
		/// <summary>
		/// Methods returns the requested Carriers. If Local is true it returns the
		/// local providers. If local is false it returns the long distance providers.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="local"></param>
		/// <returns></returns>
		public CarrierSchema.CarriersDataTable GetCarriers(int siteId, bool local)
		{
			try
			{
				using(SqlConnection sqlConnection=new SqlConnection(_connectionString))
				{
					// open connection
					try{sqlConnection.Open();}
					catch{throw new LogonException();}
					// now setup the command object.
					string procName=local?"spGetCarriersIntraLata":"spGetCarriersInterLata";
					using(SqlCommand cmd=new SqlCommand(procName,sqlConnection))
					{
						// build the command object
						cmd.CommandType=CommandType.StoredProcedure;
						cmd.Parameters.Add("@siteId",SqlDbType.Int).Value=siteId;
						// build the dataadapter
						SqlDataAdapter da = new SqlDataAdapter(cmd);
						// create the dataset to fill
						CarrierSchema ds=new CarrierSchema();
						da.Fill(ds.Carriers);
						// all done, return results.
						return ds.Carriers;
					}
				}
			}
			catch(DataSourceException)
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch(Exception ex)
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}

        /// <summary>
        /// Gets the data center for a given site id.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int GetDataCenter(int siteId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    try { sqlConnection.Open(); }
                    catch (Exception) { throw new LogonException(); }

                    using (SqlCommand cmd = new SqlCommand("spGetDataCenterBySiteId", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;
                        cmd.Parameters.Add(new SqlParameter("@dataCenterId", SqlDbType.Int, 4,
                            ParameterDirection.Output, false, 0, 0, string.Empty, DataRowVersion.Default, null));

                        cmd.ExecuteNonQuery();

                        int returnValue = -1;

                        try { returnValue = Convert.ToInt32(cmd.Parameters["@dataCenterId"].Value.ToString()); }
                        catch {/*do nothing*/}

                        return returnValue;
                    }
                }
            }
            catch (DataSourceException)
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
		#endregion public functions
	}
	#region public structures
	/// <summary>
	/// Simple structure for site/franchise information.
	/// </summary>
	public struct SiteIdAndFranchiseId
	{
		#region member variables
		/// <summary>Member variable containing the siteId</summary>
		private int _siteId;
		/// <summary>Member variable containing the franchiseId</summary>
		private int _franchiseId;
		#endregion member variables

		#region properties
		/// <summary>
		/// Gets or sets the SiteId
		/// </summary>
		public int SiteId
		{
			get{return _siteId;}
			set{_siteId=value;}
		}
		/// <summary>
		/// Gets or sets the FranchiseId
		/// </summary>
		public int FranchiseId
		{
			get{return _franchiseId;}
			set{_franchiseId=value;}
		}
		#endregion properties

		#region ctors
		/// <summary>
		/// The parameterized constructor.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="franchiseId"></param>
		public SiteIdAndFranchiseId(int siteId, int franchiseId)
		{
			_siteId=siteId;
			_franchiseId=franchiseId;
		}
		#endregion ctors
	}
	#endregion public structures
}