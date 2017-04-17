using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
//DataAccess Stuff
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;


namespace Cox.DataAccess.CustomerAccount
{
    /// <summary>
    /// DataAccessLayer component used to access ODS 
    /// </summary>
   public class DalCustomerPhone : Dal
    {
        #region Constants
        /// <summary>
        /// The connectionKey into the connections configuration section of the config block.
        /// </summary>
        protected const string __connectionKey = "odsConnectionString";
        

        #endregion constants

        #region Ctors
		/// <summary>
		/// Default constructor. This constructor retreives its 
		/// connection information from the configuration block.
		/// </summary>
        public DalCustomerPhone()
            : base(__connectionKey)
        {
          
        }
        #endregion Ctors 

        /// <summary>
        /// Gets the account number and site Id matches for the input phoneNumber and street.
        /// </summary>
        /// <param name="phoneNumber10">Phone number passed in</param>
        /// <param name="streetNumber">Street Number</param>
        /// <returns></returns>
        public CustomerAccountProfileSchema.AccountMatchesDataTable GetCustomerAccountMatches(string phoneNumber10, string streetNumber)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                
                // build the command object
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = oracleConn;
                    cmd.CommandType = CommandType.Text;

                    //Build query
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select    ");
                    sql.Append("Distinct SITE_ID , LPAD(ACCOUNT_NBR,9,'0') as Account_Number, '0' as customer_tn_flag  from CDM.PHONE_MASTER_WS ");
                    sql.Append(" where   " );
                    sql.Append("    (    ( account_status = 'A' " );
                    sql.Append("     and cust_telephone_status IN ( 'AC', 'NA' ) ) or " );
                    sql.Append("    (  cust_telephone_status IN ('Active','Suspended','Inactive') )      )  ");
                    sql.Append("     and phone_nbr = :nbrPhoneNumber  ");

                    //Add street number condition in where clause only if street number is neither empty nor null.
                    if (!String.IsNullOrEmpty(streetNumber.Trim()))
                    {
                        sql.Append("     and trim(street_nbr) = :vchStreetNumber ");
                        //add parameter for street number.
                        cmd.Parameters.Add("vchStreetNumber", OracleType.VarChar, 50).Value = streetNumber.Trim();
                    }

                    cmd.CommandText = sql.ToString();
                    //add parameter for phone number
                    cmd.Parameters.Add("nbrPhoneNumber", OracleType.Number, 10).Value = Convert.ToUInt64(phoneNumber10);
                     
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
                        finally
                        {
                            oracleConn.Close();
                        }
                        // all done, return
                        return ds.AccountMatches;
                    }
                }
            }
        }
            

            

            /// <summary>
        /// Gets the account numbers and site Id matches for the input phoneNumber and street.
        /// </summary>
        /// <param name="phoneNumber10">Phone number passed in</param>
        /// <param name="getNeverAndFormerAsWell">Flag which indicates whether to search former and never accounts or not.</param>
        /// <returns></returns>
       public CustomerAccountProfileSchema.AccountMatchesDataTable GetCustomerAccountMatches(string phoneNumber10, bool getNeverAndFormerAsWell)
        {
            using (OracleConnection oracleConn = new OracleConnection(_connectionString))
            {
                // open connection
                try { oracleConn.Open(); }
                catch (Exception ex)
                {
                    throw new LogonException(ex);
                }

                
                // build the command object
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = oracleConn;
                    cmd.CommandType = CommandType.Text;

                    //Build query
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select    ");
                    sql.Append(" Distinct SITE_ID , LPAD(ACCOUNT_NBR,9,'0') as Account_Number, DECODE(phone_type,'Cox Service Primary',1,'Cox Service Secondary Ring',1,0) customer_tn_flag from CDM.PHONE_MASTER_WS ");
                    sql.Append(" where (    (  ");
                    //Check if we need to get former and never accounts also
                    if (!getNeverAndFormerAsWell)
                        //If we donot need to search for former and never accounts, 
                        //add active account status filter.
                        sql.Append("     account_status = 'A' and ");
                    sql.Append("      cust_telephone_status IN ( 'AC', 'NA') ) or ");
                    sql.Append("    (  cust_telephone_status IN ('Active','Suspended','Inactive') )      )  ");
                    sql.Append("     and phone_nbr = :nbrPhoneNumber  ");

                                     
                    cmd.CommandText = sql.ToString();
                    //add parameter for phone number
                    cmd.Parameters.Add("nbrPhoneNumber", OracleType.Number, 10).Value = Convert.ToUInt64(phoneNumber10);
                     
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
                        finally
                        {
                            oracleConn.Close();
                        }
                        // all done, return
                        return ds.AccountMatches;
                    }
                }
            }
        }
        }
    }

