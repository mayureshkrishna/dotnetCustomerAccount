using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Configuration;

//DataAccess Stuff
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.CustomerAccountCCRM
{
    ///<summary>
    /// DataAccessLayer component used to access CustomerAccountCCRM
    ///</summary>
    public class DalCustomerAccountCCRM : Dal
    {
        #region Constants
        /// <summary>
        /// The connectionKey into the connections configuration section of the config block.
        /// </summary>
        protected const string __connectionKey = "accountCCRMConnectionString";
        #endregion constants
        
        #region Ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// connection information from the configuration block.
		/// </summary>
        public DalCustomerAccountCCRM():base(__connectionKey) { }
		#endregion Ctors 

        #region public methods
    
        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerAccountCCRMDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><string></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public string GetCRMAccountProfile(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }
                string sql = "GetCRMAccountProfile";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("siteId", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("accountNumber", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("errorCode", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("errorMessage", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    OracleParameter customerValueSegmentationOut = cmd.Parameters.Add("customerValueSegmentation", OracleType.VarChar, 2000);
                    customerValueSegmentationOut.Direction = ParameterDirection.Output;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new DataSourceException(ex);
                    }

                    if (int.Parse(errorCodeOut.Value.ToString()) != 0)
                    {
                        throw new DataSourceException(errorTextOut.Value.ToString());
                    }

                    // all done, return
                    return customerValueSegmentationOut.Value.ToString();
                }
            }
        #endregion public methods
        }
    }
}
