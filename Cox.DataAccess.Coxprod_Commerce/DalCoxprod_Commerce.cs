using System;
using System.Diagnostics;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Coxprod_Commerce
{
    public class DalCoxprod_Commerce : Dal
	{
		#region constants
		/// <summary>
		/// Contains the key into the configuration block for retrieving the
		/// underling connection string.
		/// </summary>
		protected const string __connectionStringKey ="coxProdCommerceConnectionString";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
        public DalCoxprod_Commerce() : base(__connectionStringKey) { }
		#endregion ctors

		#region public methods
		/// <summary>
        /// GetEnrolledInEmailBillReminder - returns true if the customer is enrolled in email bill reminders
		/// </summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="companyDiv">Company Division</param>
        /// <param name="statementCode">Statement Code</param>
        /// <returns><bool></returns>
        /// <exception cref="LogonException">Thrown when a problem occurred
        /// trying to connect to the underlying datasource.</exception>
        /// <exception cref="DataSourceException">Thrown when a problem occurred
        /// trying to interact with the underlying datasource after a connection
        /// has been established. Check the inner exception for further meaning
        /// as to why the problem occurred.</exception>
        public bool GetEnrolledInEmailBillReminder(string accountNumber9, int siteId, string companyDiv, string statementCode)
		{
            using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
            {
                try { sqlConnection.Open(); }
                catch (Exception e) { throw new LogonException(e); }

                using (SqlCommand cmd = new SqlCommand("spGetEnrolledInSPBRequestsByAccountNumberandSiteID", sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@accountNumber", SqlDbType.VarChar).Value = accountNumber9;
                    cmd.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;
                    cmd.Parameters.Add("@companyDiv", SqlDbType.NVarChar).Value = companyDiv;
                    cmd.Parameters.Add("@statementCode", SqlDbType.NVarChar).Value = statementCode;

                    object count;

                    try
                    {
                        count = cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new DataSourceException(ex);
                    }

                    return Convert.ToInt32(count) > 0;
                }
            
            }
		}
		#endregion public methods
	}
}
