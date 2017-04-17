using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;
using System.Configuration;

//DataAccess Stuff
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.CustomerAccount
{
    /// <summary>
    /// DataAccessLayer component used to access Outage
    /// </summary>
    public class DalCustomerAccount : Dal
    {
        #region Constants
        /// <summary>
        /// The connectionKey into the connections configuration section of the config block.
        /// </summary>
        protected const string __connectionKey = "accountConnectionString";
        #endregion constants
        
        #region Ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// connection information from the configuration block.
		/// </summary>
		public DalCustomerAccount():base(__connectionKey){}
		#endregion Ctors 

        #region public methods

         /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerAccountProfileDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerAccountDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema GetAccountProfile(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                //[30-10-2009] Start Changes for including Customer History Data

                string sql = "GETACCOUNTPROFILEDETAILS";

                //[30-10-2009] End Changes for including Customer History Data

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;                       
                    cmd.Parameters.Add("nbrAcctNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcAccoutProfileOut", OracleType.Cursor);
                    cmd.Parameters["rfcAccoutProfileOut"].Direction = ParameterDirection.Output;

                    //[28-01-2009] Start Changes to reflect active services for an account

                    cmd.Parameters.Add("rfcAccoutServices", OracleType.Cursor);
                    cmd.Parameters["rfcAccoutServices"].Direction = ParameterDirection.Output;

                    


                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();
                        
                        // now fill it
                        try
                        {
                            da.TableMappings.Add("Table", "CustomerAccount");
                            da.TableMappings.Add("Table1", "CustomerServices");

                            da.Fill(ds);
                           
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds;
                        //[28-01-2009] End Changes to reflect active services for an account
                    }
                }
            }
        }

        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerCampaignDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerCampaignDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema.CustomerCampaignDataTable GetCustomerCampaign(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

               string sql = "GETCUSTOMERCAMPAIGN";
               
                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAccountNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcCustCampaignOut", OracleType.Cursor);
                    cmd.Parameters["rfcCustCampaignOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerCampaign);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerCampaign;
                    }
                }
            }
        }

        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerContractDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerContractDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema.CustomerContractDataTable GetCustomerContract(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                //Changes for adding service categories for contract starts here
                string sql = "GETCUSTOMERCONTRACTINFO";
                
                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAccountNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcCustCntrctOut", OracleType.Cursor);
                    cmd.Parameters["rfcCustCntrctOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerContract);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerContract;
                    }
                }
            }
        }

        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerContractDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerPhoneDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema.CustomerPhoneDataTable GetAllAccountPhones(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }
                string sql = "GETALLACCOUNTPHONES";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAcctNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcAccountPhonesOut", OracleType.Cursor);
                    cmd.Parameters["rfcAccountPhonesOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerPhone);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerPhone;
                    }
                }
            }
        }

        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerPrivacyDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerPrivacyDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema.CustomerPrivacyDataTable GetCustomerPrivacy(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }
                string sql = "GETACCOUNTPRIVACY";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAcctNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcAccountPrivacyOut", OracleType.Cursor);
                    cmd.Parameters["rfcAccountPrivacyOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerPrivacy);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerPrivacy;
                    }
                }
            }
        }      
         
        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerMonthlyServiceAmountDataTable. Using this DataTable 
        /// does not require any knowledge of the underlying datasource. Nor does it require knowledge
        /// of the underlying data access libraries (e.g. SqlClient, OleDb, OracleClient, etc. ).
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CustomerAccountProfileSchema.CustomerMonthlyServiceAmountDataTable></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public CustomerAccountProfileSchema.CustomerMonthlyServiceAmountDataTable GetMonthlyServiceAmount(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }
                string sql = "GETMONTHLYSVCAMT";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAccountNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcMonthlySVCAmtOut", OracleType.Cursor);
                    cmd.Parameters["rfcMonthlySVCAmtOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerMonthlyServiceAmount);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerMonthlyServiceAmount;
                    }
                }
            }
        }

        /// <summary>
        /// Given a siteId (up to 3 digits) and the accountNumber (up to 9 digits), this method 
        /// contructs and fills a strongly typed CustomerEmailDataTable. Using this DataTable 
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
        public string GetCustomerEmail(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }
                string sql = "GETCUSTOMEREMAIL";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("siteId", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("accountNumber", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter customerEmailOut = cmd.Parameters.Add("emailAddress", OracleType.VarChar, 2000);
                    customerEmailOut.Direction = ParameterDirection.Output;
                    OracleParameter errorCodeOut = cmd.Parameters.Add("errorCode", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("errorMessage", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;


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
                    return customerEmailOut.Value.ToString();

                }
            }
        }

        //[02-02-09] Start Changes for Q-matic

        /// <summary>
        /// Gets the account number and site Id matches for the input phoneNumber and street.
        /// </summary>
        /// <param name="phoneNumber10">Phone number passed in</param>
        /// <param name="streetNumber">Street Number</param>
        /// <returns></returns>
        public CustomerAccountProfileSchema.AccountMatchesDataTable GetAccountMatches(string phoneNumber10, string streetNumber)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                string sqlProcedureName = "GETACCOUNTBYPHONEANDSTREET";

                 // build the command object
                using (OracleCommand cmd = new OracleCommand(sqlProcedureName, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("nbrPhoneNbrIn", OracleType.Number,10).Value = Convert.ToUInt64(phoneNumber10);
                    OracleParameter param = new OracleParameter();
                    param.ParameterName = "street";
                    param.OracleType = OracleType.VarChar;
                    param.IsNullable = true;
                    if (!String.IsNullOrEmpty(streetNumber))
                        cmd.Parameters.Add(param).Value = streetNumber;
                    else
                        cmd.Parameters.Add(param).Value = System.DBNull.Value;
                    
                    cmd.Parameters.Add("rfcAccountDetails", OracleType.Cursor);
                    cmd.Parameters["rfcAccountDetails"].Direction = ParameterDirection.Output;

                    OracleParameter errorCodeOut = cmd.Parameters.Add("errorCode", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("errorMessage", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;

                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.AccountMatches);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }
                        // all done, return
                        return ds.AccountMatches;
                    }
                }
            }
        }

        //[02-02-09] End Changes for Q-matic

        /// <summary>
        /// This function gets the customer price lock information from PHASTAGE.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="accountNumber9"></param>
        /// <returns></returns>
        public CustomerAccountProfileSchema.CustomerPriceLockInfoDataTable GetCustomerPriceLockInfo(int siteId, string accountNumber9)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                string sql = "GETCUSTOMERPRICELOCKINFO";

                // build the command object
                using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //add the params
                    cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                    cmd.Parameters.Add("nbrAccountNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                    OracleParameter errorCodeOut = cmd.Parameters.Add("nbrErrorOut", OracleType.Number);
                    errorCodeOut.Direction = ParameterDirection.Output;
                    OracleParameter errorTextOut = cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 2000);
                    errorTextOut.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcCustPriceLockOut", OracleType.Cursor);
                    cmd.Parameters["rfcCustPriceLockOut"].Direction = ParameterDirection.Output;

                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountProfileSchema ds = new CustomerAccountProfileSchema();

                        // now fill it
                        try
                        {
                            da.Fill(ds.CustomerPriceLockInfo);
                        }
                        catch (Exception ex)
                        {
                            throw new DataSourceException(ex);
                        }

                        if (int.Parse(cmd.Parameters["nbrErrorOut"].Value.ToString()) != 0)
                        {
                            throw new DataSourceException(cmd.Parameters["vchErrorOut"].Value.ToString());
                        }

                        // all done, return
                        return ds.CustomerPriceLockInfo;
                    }
                }
            }
        }


        #endregion public methods
    }
}
