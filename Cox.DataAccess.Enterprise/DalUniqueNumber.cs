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

namespace Cox.DataAccess.Enterprise
{
    /// <summary>
    /// Summary description for DalBroadcastMessage.
    /// </summary>
    public class DalUniqueNumber : Dal
    {
        #region constants
        /// <summary>
        /// Contains the key into the configuration block for retrieving the
        /// underling connection string.
        /// </summary>
        protected const string __connectionStringKey = "enterpriseConnectionString";
        /// <summary>
        /// Message to use when a null/empty name parameter specified for spGetNextNumber.
        /// </summary>
        protected const string __invalidNameParameter = "Invalid/empty name for generating a unique number.";
        /// <summary>
        /// Message to use when an unexpected error occurred trying to insert/update or delete a broadcast message.
        /// </summary>
        protected const string __unexpected = "An unexpected error occurred with Generating a Unique Number.";
        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public DalUniqueNumber() : base(__connectionStringKey) { }
        #endregion ctors

        #region GenerateUniqueNumber
        /// <summary>
        /// Generates a unique identity like value for the given name.
        /// </summary>
        /// <returns></returns>
        public int GenerateUniqueNumber(string name)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionString))
                {
                    // open connection
                    try { sqlConn.Open(); }
                    catch { throw new LogonException(); }
                    // now execute command
                    using (SqlCommand cmd = new SqlCommand("spGetNextNumber", sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // setup return variable
                        cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int,
                            4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty,
                            DataRowVersion.Default, null));
                        cmd.Parameters.Add("@name", SqlDbType.VarChar, 50).Value = name;

                        cmd.ExecuteNonQuery();
                        int ret = 0;
                        try { ret = Convert.ToInt32(cmd.Parameters["ReturnValue"].Value); }
                        catch {/*do nothing*/}

                        if (ret <= 0)
                        {
                            switch (ret)
                            {
                                case -1:
                                    throw new DataSourceException(__invalidNameParameter);
                                default:
                                    throw new DataSourceException(__unexpected);

                            }
                        }
                        return ret;
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
        #endregion GenerateUniqueNumber
    }
}