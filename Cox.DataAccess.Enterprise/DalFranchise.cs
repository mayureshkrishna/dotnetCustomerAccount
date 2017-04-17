using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Enterprise
{
	/// <summary>
	/// This class handles the caching of franchise and franchiseGroups. 
	/// ALL CLASSES SHOULD USE THIS AS OPPOSED TO the franchise class. This class
	/// handles caching the data so hits to the database don't have to occur each time.
	/// 
	/// This class is contained in the DataAccess layer as opposed to the business layer 
	/// because 1) Its data related and not business logic related. And 2) It can be 
	/// reused by the data layer itself.
	/// </summary>
	public sealed class DalFranchise:Dal
	{

		#region constants
		/// <summary>
		/// Defines the query string for getting records based on franchiseId/siteId.
		/// </summary>
		private const string __franchiseIdSiteIdQuery="FranchiseId='{0}' and SiteId={1}";
		/// <summary>
		/// Defines the query string for getting records based on franchiseGroupId.
		/// </summary>
		private const string __franchiseGroupIdQuery="FranchiseGroupId='{0}'";
		/// <summary>
		/// This private constant contains the lookup key into the config file for
		/// retrieving the default LeaseDuration for this component. Since this
		/// class is a cache of franchiseId mappings, the duration of the 
		/// "Lease" in minutes denotes the amount of time that must pass before
		/// the next lookup will cause the cache to be refreshed.
		/// </summary>
		private const string _leaseDurationLookupKey = "franchiseLeaseDuration";
		/// <summary>
		/// This private constant contains the default value for the lease duration.
		/// This is necessary in case the duration specified in the config file does
		/// not exist or is invalid. 
		/// </summary>
		private const int __defaultLeaseDuration = 1440;
		/// <summary>
		/// Contains the key into the configuration block for retrieving the
		/// underling connection string.
		/// </summary>
		private const string __connectionStringKey ="enterpriseConnectionString";
		#endregion constants

		#region member variables
		/// <summary>
		/// Private member variable containing dataset of franchise information.
		/// </summary>
		private FranchiseSchema _franchiseSchema=null;
		/// <summary>
		/// Private member variable containing the time the cache was loaded.
		/// </summary>
		private DateTime _cacheStartTime = DateTime.Now;
		/// <summary>
		/// Private member variable containing how long the duration of the Lease in minutes.
		/// </summary>
		private int _leaseDuration = 0;
		/// <summary>
		/// Private member variable used to lock 
		/// </summary>
		private object _padLock=new object();
		/// <summary>
		/// We are implementing the singleton design pattern...So create a public static
		/// readonly variables (keep in mind that .Net takes care of the synchronization
		/// of threads so that this values only gets initialized once.
		/// </summary>
		public static readonly DalFranchise Instance = new DalFranchise();

		#endregion member variables

		#region ctors
		/// <summary>
		/// Not directly callable by user. This constructor takes care of initializing key values such
		/// as LeaseDuration and ConnectionString(s). The .Net framework takes care of locking the class
		/// for us, so there is no need to specify a synchronization lock on this constructor. 
		/// </summary>
		private DalFranchise() : base( __connectionStringKey )
		{
			try
			{
				_leaseDuration = Convert.ToInt32( ConfigurationManager.AppSettings[ _leaseDurationLookupKey ]);
				if(_leaseDuration==0)_leaseDuration	= __defaultLeaseDuration;
			}
			catch
			{
				// an error occurred. this means that the lease property is not
				// configured properly in the configuration file. so set it to
				// its defalt value.
				_leaseDuration	= __defaultLeaseDuration;
			}
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Internal private property used to provide a synch lock when setting the leaseDuration member variable.
		/// </summary>
		private int leaseDuration
		{
			get
			{
				return _leaseDuration;
			}
			set
			{
				// we only care about locking this when setting the value. since this value 
				// spans the entire class, we want to execute a lock on the class type.
				lock(_padLock)
				{
					_leaseDuration = value;
				}
			}
		}
		/// <summary>
		/// Internal private property used to provide a synch lock on the internal cacheStartTime property.
		/// </summary>
		private DateTime cacheStartTime
		{
			get
			{
				return _cacheStartTime;
			}
			set
			{
				// we only care about locking this when setting the value. since this value 
				// spans the entire class, we want to execute a lock on the class type.
				lock(_padLock)
				{
					_cacheStartTime = value;
				}
			}
		}
		#endregion properties

		#region public methods
		/// <summary>
		/// Returns the FranchiseGroupId based on the given parameters.
		/// </summary>
		/// <param name="franchiseId"></param>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public int GetFranchiseGroupId(int franchiseId,int siteId)
		{
			return getRecords(string.Format(__franchiseIdSiteIdQuery,
				franchiseId,siteId))[0].FranchiseGroupId;
		}
		/// <summary>
		/// Returns the FranchiseGroupName based on the given parameters.
		/// </summary>
		/// <param name="franchiseId"></param>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public string GetFranchiseGroupName(int franchiseId,int siteId)
		{
			return getRecords(string.Format(__franchiseIdSiteIdQuery,
				franchiseId,siteId))[0].FranchiseGroupName;
		}
		/// <summary>
		/// Returns the FranchiseGroupName based on the given parameters.
		/// </summary>
		/// <param name="franchiseGroupId"></param>
		/// <returns></returns>
		public string GetFranchiseGroupName(int franchiseGroupId)
		{
			return getRecords(string.Format(__franchiseGroupIdQuery,
				franchiseGroupId))[0].FranchiseGroupName;
		}
		/// <summary>
		/// This method returns back a copy of the internal structure.
		/// </summary>
		/// <returns></returns>
		public FranchiseSchema.FranchisesDataTable GetFranchiseGroupInformation()
		{
			// loadData(this method checks to see if the data needs to be loaded.
			loadData();
			return (FranchiseSchema.FranchisesDataTable)_franchiseSchema.Franchises.Copy();
		}
		/// <summary>
		/// This method resets the lease on the cache so that the next time
		/// it is hit it will reload the data.
		/// </summary>
		public void ResetLease()
		{
			cacheStartTime=cacheStartTime.AddMinutes(leaseDuration);
		}
		/// <summary>
		/// This method returns true if the lease has expired, false otherwise.
		/// </summary>
		/// <returns></returns>
		public bool HasLeaseExpired()
		{
			// check to see if the lease has expired
			return ((TimeSpan)(DateTime.Now-_cacheStartTime)).Minutes >= leaseDuration;
		}
		#endregion public methods

		#region private methods
		/// <summary>
		/// Internal method used to get records from internal DataTable using
		/// the supplied query.
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		private FranchiseSchema.Franchise[] getRecords(string sql)
		{
			// loadData(this method checks to see if the data needs to be loaded.
			loadData();
			// get rows (there should only be 1 in this situation).
			FranchiseSchema.Franchise[]rows=(FranchiseSchema.Franchise[])
				_franchiseSchema.Franchises.Select(sql);

			// if no records found, then throw an exception.
			if(rows.Length==0)
			{
				throw new RecordDoesNotExistException();
			}

			return rows;
		}
		/// <summary>
		/// Private method used to load the internal data structures.
		/// </summary>
		private void loadData()
		{
			lock(_padLock)
			{
				// check the count and see if the lease has expired.
				if(_franchiseSchema==null||HasLeaseExpired())
				{
					// reset cachestarttime
					cacheStartTime = DateTime.Now;
					try
					{
						using(SqlConnection sqlConnection=new SqlConnection(_connectionString))
						{
							// open connection
							try{sqlConnection.Open();}
							catch{throw new LogonException();}
							// now setup the command object.
							using(SqlCommand cmd=new SqlCommand("spGetFranchiseInformation",sqlConnection))
							{
								// build the command object
								cmd.CommandType=CommandType.StoredProcedure;
								// build the dataadapter
								SqlDataAdapter da = new SqlDataAdapter(cmd);
								// fill the dataset.
								_franchiseSchema=new FranchiseSchema();
								da.Fill(_franchiseSchema.Franchises);
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
			}
		}
		#endregion private methods
	}
}
