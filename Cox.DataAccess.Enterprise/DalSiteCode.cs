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
	/// This class handles the caching of sitecodes and how to map them to accounts, 
	/// company, divisions, etc. ALL CLASSES SHOULD USE THIS AS OPPOSED TO the Site
	/// class. This class handles caching the data so hits to the database don't
	/// have to occur each time.
	/// 
	/// This class is contained in the DataAccess layer as opposed to the business layer 
	/// because 1) Its data related and not business logic related. And 2) It can be 
	/// reused by the data layer itself.
	/// </summary>
	public sealed class DalSiteCode : Dal
	{

		#region constants
		/// <summary>
		/// This private constant contains the lookup key into the config file for
		/// retrieving the default LeaseDuration for this component. Since this
		/// class is a cache of siteCode/siteId mappings, the duration of the 
		/// "Lease" in minutes denotes the amount of time that must pass before
		/// the next lookup will cause the cache to be refreshed.
		/// </summary>
		private const string _leaseDurationLookupKey = "siteCodeLeaseDuration";
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
		/// Private member variable containing synchronized hashtable of active sites.
		/// </summary>
		private SiteCodeSchema.CoxSitesDataTable _activeSites = new  SiteCodeSchema.CoxSitesDataTable();
		/// <summary>
		/// Private member variable containing synchronized hashtable of company divisions to sites.
		/// </summary>
		private Hashtable _companyDivisionSites = Hashtable.Synchronized( new Hashtable() );
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
		public static readonly DalSiteCode Instance = new DalSiteCode();

		#endregion member variables

		#region ctors
		/// <summary>
		/// Not directly callable by user. This constructor takes care of initializing key values such
		/// as LeaseDuration and ConnectionString(s). The .Net framework takes care of locking the class
		/// for us, so there is no need to specify a synchronization lock on this constructor. 
		/// </summary>
		private DalSiteCode() : base( __connectionStringKey )
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
		/// Given a company number and division number, this 
		/// method will return the corresponding site code.
		/// </summary>
		/// <param name="company"></param>
		/// <param name="division"></param>
		/// <returns></returns>
		public string GetSiteCode( int company, int division )
		{
			int siteId = GetSiteId( company, division );
			return GetSiteCode( siteId );
		}
		/// <summary>
		/// Given a siteId, this method will return the corresponding site code.
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		/// <exception cref="RecordDoesNotExistException">Thrown when the requested
		/// information does not exist.
		/// </exception>
		public string GetSiteCode( int siteId )
		{
			loadData();
			
//			if( !_activeSites.ContainsKey( siteId ) )
//			{
//				throw new RecordDoesNotExistException();
//			}
//			return _activeSites[ siteId ].ToString();

			SiteCodeSchema.CoxSite[] rows =  (SiteCodeSchema.CoxSite[])_activeSites.Select("stId = " + siteId);
			if(rows.Length < 1)
			{
				throw new RecordDoesNotExistException();
			}
			//return the first one
			return rows[0].SiteCode.ToString();
		}
		/// <summary>
		/// Given a company number and division number, this 
		/// method will return the corresponding siteid.
		/// </summary>
		/// <param name="company"></param>
		/// <param name="division"></param>
		/// <returns></returns>
		/// <exception cref="RecordDoesNotExistException">Thrown when the requested
		/// information does not exist.
		/// </exception>
		public int GetSiteId( int company, int division )
		{
			loadData();
			string key = buildCompanyDivisionMapKey( 
				company.ToString(), division.ToString() );
			if( !_companyDivisionSites.ContainsKey( key ) )
			{
				throw new RecordDoesNotExistException();
			}
			return ( int )_companyDivisionSites[ key ];
		}
		/// <summary>
		/// Given a 16 digit account number, this method will return the corresponding site code.
		/// </summary>
		/// <param name="accountNumber16"></param>
		/// <returns></returns>
		public int GetSiteId( string accountNumber16 )
		{
			int companyNumber=Int32.Parse( accountNumber16.Substring( 3, 2 ) );
			int divisionNumber=Int32.Parse( accountNumber16.Substring( 5, 2 ) );
			return GetSiteId( companyNumber, divisionNumber );
		}

		/// <summary>
		/// Gets the name of the site
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public string GetSiteName(int siteId)
		{
			loadData();
			SiteCodeSchema.CoxSite[] rows =  (SiteCodeSchema.CoxSite[])_activeSites.Select("stId = " + siteId);
			if(rows.Length < 1)
			{
				throw new RecordDoesNotExistException();
			}
			//return the first one
			return rows[0].SiteDescription.ToString();
		}
		/// <summary>
		/// Gets the name of the site
		/// </summary>
		/// <param name="siteCode"></param>
		/// <returns></returns>
		public string GetSiteName(string siteCode)
		{
			loadData();
			SiteCodeSchema.CoxSite[] rows =  (SiteCodeSchema.CoxSite[])_activeSites.Select("stCode = " + siteCode);
			if(rows.Length < 1)
			{
				throw new RecordDoesNotExistException();
			}
			//return the first one
			return rows[0].SiteDescription.ToString();
		}	
		#endregion public methods

		#region private methods
		/// <summary>
		/// this method returns true if the lease has expired, false otherwise.
		/// </summary>
		/// <returns></returns>
		private bool hasLeaseExpired()
		{
			// check to see if the lease has expired
			return ((TimeSpan)(DateTime.Now-_cacheStartTime)).Minutes >= leaseDuration;
		}
		/// <summary>
		/// This method uses its parameters to build a key to return to 
		/// caller for internal hashtables used to store and index values.
		/// </summary>
		/// <param name="company"></param>
		/// <param name="division"></param>
		/// <returns></returns>
		private string buildCompanyDivisionMapKey( string company, string division )
		{
			return company + "/" + division;
		}
		/// <summary>
		/// Private method used to load the internal data structures.
		/// </summary>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		private void loadData()
		{
			// ok, we are locking it here because if we 1st check the count and the expiration
			// then it is possible for 2 threads to both get a true on either a count of 0 or
			// a leaseexpired of true and then both hit the lock at the same time. this is ok,
			// but then they will both try to fill the _activesites hashtable and we are screwed
			// because they will both end up filling the hashtable. the only way around this is
			// to put the synchronized lock on it before we attempt to see if the lease has expired
			// or if the count is 0.
			lock(_padLock)
			{
				// check the count and see if the lease has expired.
				if( 0 == _companyDivisionSites.Count || hasLeaseExpired() )
				{
					// first clear hashtables (if its already clear it won't hurt anything).
					_companyDivisionSites.Clear();
					_activeSites.Clear();

					// reset cachestarttime
					cacheStartTime = DateTime.Now;

					// retrieve datatable of company/division/sites and enumerate the records
					// this will throw its own errors, so don't wrap in try/catch i debated
					// putting this outside of the synch lock, but at  the end of the day, 
					// we don't want anyone to access the  cache if it is expired, so it 
					// doesn't hurt anything anyway. also, this method manages its own exceptions
					// in other words, it will wrap exceptions in the correct exception type.
					SiteCodeSchema.CompanyDivisionSitesDataTable cdSites = GetCompanyDivisionSites();
					foreach( SiteCodeSchema.CompanyDivisionSite row
								in cdSites.Rows )
					{
						string cdsKey = buildCompanyDivisionMapKey( 
							row.CompanyNumber.ToString(), 
							row.DivisionNumber.ToString() );

						// now add to the collection
						_companyDivisionSites[ cdsKey ] = row.SiteId;
					}
				}

				// retrieve datatable of sites and enumerate the records
				// this will throw its own errors, so don't wrap in try/catch
				// i debated putting this outside of the synch lock, but at 
				// the end of the day, we don't want anyone to access the 
				// cache if it is expired, so it doesn't hurt anything anyway.
				_activeSites = GetSites();
				
//				SiteCodeSchema.CoxSitesDataTable sites = GetSites();
//				foreach( SiteCodeSchema.CoxSite row in sites.Rows )
//				{
//					_activeSites[ row.SiteId ] = row.SiteCode;
//				}
			}
		}
		/// <summary>
		/// retrieves the list of siteids and their corresponding 3 character sitecode.
		/// use the SiteAtt enumeration for a list of columnNames
		/// within returned DataTable
		/// </summary>
		/// <returns></returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		private SiteCodeSchema.CoxSitesDataTable GetSites()
		{
			// here, we are wrapping all exceptions with a datasource exception.
			// the inner exception will contain the actual stack trace and the
			// corresponding base exception (and rdbms specific error).
			try
			{
				// connect to the profile database to get this information.
				// do this with a using statement in order to make sure the
				// connection gets closed.
				using( SqlConnection sqlConn = new SqlConnection( _connectionString ) )
				{
					// build the command object
					using(SqlCommand cmd = new SqlCommand( "spGetSites", sqlConn ))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						// build the dataadapter
						using(SqlDataAdapter da = new SqlDataAdapter( cmd ))
						{

							// create the dataset to fill
							SiteCodeSchema ds = new SiteCodeSchema();

							// now fill it
							da.Fill( ds.CoxSites );

							// return the single table
							return ds.CoxSites;
						}
					}
				}
			}
			catch( System.Data.SqlClient.SqlException sqlEx )
			{
				// Opening a connection will throw either a 
				//		System.InvalidOperationException: Connection is already open
				//		System.Data.SqlClient.SqlException: Connection level exception
				//
				//		Since we know that the connection is not already open, we don't
				//		need to specialize the catch block for InvalidOperationException
				//		(plus this is an error also thrown by other SqlXXX objects
				throw new LogonException( sqlEx );
			}
			catch( Exception ex )
			{
				throw new DataSourceException( ex );
			}
		}
		/// <summary>
		/// retrieves the list of siteid's to respective division/sites
		/// use the CompanyDivisionAtt enumeration for a list of columnNames
		/// within returned DataTable
		/// </summary>
		/// <returns></returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		private SiteCodeSchema.CompanyDivisionSitesDataTable GetCompanyDivisionSites()
		{
			// here, we are wrapping all exceptions with a datasource exception.
			// the inner exception will contain the actual stack trace and the
			// corresponding base exception (and rdbms specific error).
			try
			{
				// connect to the profile database to get this information.
				// do this with a using statement in order to make sure the
				// connection gets closed.
				using( SqlConnection sqlConn = new SqlConnection( _connectionString ) )
				{
					// build the command object
					using(SqlCommand cmd = new SqlCommand( "spGetCompanyDivisionSites", sqlConn ))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						// build the dataadapter
						using(SqlDataAdapter da = new SqlDataAdapter( cmd ))
						{

							// create the dataset to fill
							SiteCodeSchema ds = new SiteCodeSchema();

							// now fill it
							da.Fill( ds.CompanyDivisionSites );

							// return the single table
							return ds.CompanyDivisionSites;
						}
					}
				}
			}
			catch( System.Data.SqlClient.SqlException sqlEx )
			{
				// Opening a connection will throw either a 
				//		System.InvalidOperationException: Connection is already open
				//		System.Data.SqlClient.SqlException: Connection level exception
				//
				//		Since we know that the connection is not already open, we don't
				//		need to specialize the catch block for InvalidOperationException
				//		(plus this is an error also thrown by other SqlXXX objects
				throw new LogonException( sqlEx );
			}
			catch( Exception ex )
			{
				throw new DataSourceException( ex );
			}
		}

		#endregion private methods

	}
}
