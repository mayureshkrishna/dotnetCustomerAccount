using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Enterprise
{
    public sealed class DalServiceCategory : Dal
    {
        #region constants

        private const string _leaseDurationLookupKey = "serviceCategoryLeaseDuration";

        private const int __defaultLeaseDuration = 1440;

        private const string __defaultServiceCategory = "U";

        private const string __connectionStringKey = "enterpriseConnectionString";
        #endregion constants

        #region member variables

        private Dictionary<string, string> _serviceCategories = new Dictionary<string, string>();

        private DateTime _cacheStartTime = DateTime.Now;

        private int _leaseDuration = 0;
        /// <summary>
        /// Private member variable used to lock 
        /// </summary>
        private object _padLock = new object();
        /// <summary>
        /// We are implementing the singleton design pattern...So create a public static
        /// readonly variables (keep in mind that .Net takes care of the synchronization
        /// of threads so that this values only gets initialized once.
        /// </summary>
        public static readonly DalServiceCategory Instance = new DalServiceCategory();

        #endregion member variables

        #region ctors
        /// <summary>
        /// Not directly callable by user. This constructor takes care of initializing key values such
        /// as LeaseDuration and ConnectionString(s). The .Net framework takes care of locking the class
        /// for us, so there is no need to specify a synchronization lock on this constructor. 
        /// </summary>
        private DalServiceCategory()
            : base(__connectionStringKey)
        {
            try
            {
                _leaseDuration = Convert.ToInt32(ConfigurationManager.AppSettings[_leaseDurationLookupKey]);
                if (_leaseDuration == 0) _leaseDuration = __defaultLeaseDuration;
            }
            catch
            {
                // an error occurred. this means that the lease property is not
                // configured properly in the configuration file. so set it to
                // its defalt value.
                _leaseDuration = __defaultLeaseDuration;
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
                lock (_padLock)
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
                lock (_padLock)
                {
                    _cacheStartTime = value;
                }
            }
        }
        #endregion properties

        #region public methods


        /// <summary>
        /// Given a serviceCategoryCode, this 
        /// method will return the corresponding serviceCategoryDesc.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="division"></param>
        /// <returns></returns>
        /// <exception cref="RecordDoesNotExistException">Thrown when the requested
        /// information does not exist.
        /// </exception>
        public string GetServiceCategoryDesc(string serviceCategoryCode)
        {
            loadData();

            if (serviceCategoryCode == null)
            {
                return _serviceCategories[__defaultServiceCategory];
            }

            if (!_serviceCategories.ContainsKey(serviceCategoryCode))
            {
                //throw new RecordDoesNotExistException();
                return _serviceCategories[__defaultServiceCategory];
            }
            return _serviceCategories[serviceCategoryCode];
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
            return ((TimeSpan)(DateTime.Now - _cacheStartTime)).Minutes >= leaseDuration;
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
            // but then they will both try to fill the dictionary and we are screwed
            // because they will both end up filling the dictionary. the only way around this is
            // to put the synchronized lock on it before we attempt to see if the lease has expired
            // or if the count is 0.
            lock (_padLock)
            {
                // check the count and see if the lease has expired.
                if (0 == _serviceCategories.Count || hasLeaseExpired())
                {
                    // first clear the dictionary (if its already clear it won't hurt anything).
                    _serviceCategories.Clear();

                    // reset cachestarttime
                    cacheStartTime = DateTime.Now;

                    // retrieve datatable of serviceCategories and enumerate the records
                    // this will throw its own errors, so don't wrap in try/catch i debated
                    // putting this outside of the synch lock, but at  the end of the day, 
                    // we don't want anyone to access the  cache if it is expired, so it 
                    // doesn't hurt anything anyway. also, this method manages its own exceptions
                    // in other words, it will wrap exceptions in the correct exception type.
                    ServiceCategorySchema.ServiceCategoryDataTable serviceCategories = GetServiceCategories();
                    foreach (ServiceCategorySchema.ServiceCategoryRow row
                                in serviceCategories.Rows)
                    {
                        string cdsKey = row.ServiceCategoryCode;

                        // now add to the collection
                        _serviceCategories[cdsKey] = row.ServiceCategoryDesc.ToString();
                    }
                }
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
        private ServiceCategorySchema.ServiceCategoryDataTable GetServiceCategories()
        {
            // here, we are wrapping all exceptions with a datasource exception.
            // the inner exception will contain the actual stack trace and the
            // corresponding base exception (and rdbms specific error).
            try
            {
                // connect to the profile database to get this information.
                // do this with a using statement in order to make sure the
                // connection gets closed.
                using (SqlConnection sqlConn = new SqlConnection(_connectionString))
                {
                    // build the command object
                    using (SqlCommand cmd = new SqlCommand("select * from ServiceCategory", sqlConn))
                    {
                        cmd.CommandType = CommandType.Text;

                        // build the dataadapter
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {

                            // create the dataset to fill
                            ServiceCategorySchema ds = new ServiceCategorySchema();

                            // now fill it
                            da.Fill(ds.ServiceCategory);

                            // return the single table
                            return ds.ServiceCategory;
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                // Opening a connection will throw either a 
                //		System.InvalidOperationException: Connection is already open
                //		System.Data.SqlClient.SqlException: Connection level exception
                //
                //		Since we know that the connection is not already open, we don't
                //		need to specialize the catch block for InvalidOperationException
                //		(plus this is an error also thrown by other SqlXXX objects
                throw new LogonException(sqlEx);
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }

        #endregion private methods

    }
}
