using System;
using System.Data;
using System.Data.SqlClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Enterprise
{
    public class DalConfigurationEnvironment : Dal
    {
        #region Constants
        /// <summary>
        /// The name of thge connection string key
        /// </summary>
        protected const string __connectionStringKey = "enterpriseConnectionString";
        /// <summary>
        /// Error message for when the user id is not found for the login.
        /// </summary>
        protected const string __unableToFindUserIdErrorMessage = "Unable to find a user id for login '{0}'.";
        /// <summary>
        /// Error message for when something goes wrong trying to delete child config hierarchy records.
        /// </summary>
        protected const string __couldNotDeleteChildRecordsErrorMessage = "Could not delete configuration heirarchy records for configuration environment id '{0}'";
        /// <summary>
        /// Error message for when something goes wrong when trying to delete a config envrionment record.
        /// </summary>
        protected const string __couldNotDeleteParentRecordErrorMessage = "Could not delete configuration environment records for configuration environment id '{0}'";
        /// <summary>
        /// Error message for when something unknown went wrong trying to delete a config environment record.
        /// </summary>
        protected const string __unknownErrorMessage = "Something went wrong, but we don't why.";
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DalConfigurationEnvironment() : base(__connectionStringKey){}
        #endregion

        #region Public Methods

        #region Get Methods
        /// <summary>
        /// Gets configuration environment information for the given name for the a userId and application name
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public ConfigurationEnvironmentSchema GetConfigurationEnvironment(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spGetConfigurationEnvironment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login == null ? DBNull.Value : (object)login;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            ConfigurationEnvironmentSchema configEnvironmentDataSet = new ConfigurationEnvironmentSchema();

                            //get the config environment
                            adapter.Fill(configEnvironmentDataSet.ConfigurationEnvironment);

                            //get the config hierarchy
                            command.CommandText = "spGetConfigurationHierarchy";
                            adapter.Fill(configEnvironmentDataSet.ConfigurationHierarchy);

                            return configEnvironmentDataSet;
                        }
                    }
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }      
        #endregion

        #region Save Methods

        #region SaveConfigurationEnvironment
        /// <summary>
        /// Saves configuration environment information.
        /// </summary>
        /// <param name="configurationEnvironmentId"></param>
        /// <param name="login"></param>
        /// <param name="channelId"></param>
        /// <param name="siteId"></param>
        public void SaveConfigurationEnvironment(ref int configurationEnvironmentId, string login, 
            int channelId, int siteId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string procedureName = isNew(configurationEnvironmentId) ? "dbo.spInsertConfigurationEnvironment" : "dbo.spUpdateConfigurationEnvironment";
                    
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@configurationEnvironmentId", SqlDbType.Int).Value = configurationEnvironmentId;
                        command.Parameters["@configurationEnvironmentId"].Direction = isNew(configurationEnvironmentId) ? ParameterDirection.Output : ParameterDirection.Input;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;
                        command.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int,
                            4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty,
                            DataRowVersion.Default, null));

                        command.ExecuteNonQuery();

                        int returnValue = 0;
                        try { returnValue = Convert.ToInt32(command.Parameters["ReturnValue"].Value); }
                        catch {/*do nothing*/}

                        if (returnValue > 0)
                        {
                            //returnValue code == 1 -- could not find user id for login
                            throw new DataSourceException(string.Format(__unableToFindUserIdErrorMessage, login));
                        }
                        
                        if (isNew(configurationEnvironmentId))
                        {
                            try { configurationEnvironmentId = Convert.ToInt32(command.Parameters["@configurationEnvironmentId"].Value); }
                            catch{/*do nothing*/}
                        }
                    }
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (DataSourceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }
        #endregion

        #region SaveConfigurationHierarchy
        /// <summary>
        /// Saves configuration hierarchy information.
        /// </summary>
        /// <param name="configurationHierarchyId"></param>
        /// <param name="configurationEnvironmentId"></param>
        /// <param name="hierarchyId"></param>
        /// <param name="hierarchyTypeId"></param>
        /// <param name="affiliateId"></param>
        /// <param name="isDefault"></param>
        public void SaveConfigurationHierarchy(ref int configurationHierarchyId, int configurationEnvironmentId, 
            int hierarchyId, int hierarchyTypeId, int affiliateId,
            bool isDefault)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string procedureName = isNew(configurationHierarchyId) ? "dbo.spInsertConfigurationHierarchy" : "dbo.spUpdateConfigurationHierarchy";

                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@configurationHierarchyId", SqlDbType.Int).Value = configurationHierarchyId;
                        command.Parameters["@configurationHierarchyId"].Direction = isNew(configurationHierarchyId) ? ParameterDirection.Output : ParameterDirection.Input;
                        command.Parameters.Add("@configurationEnvironmentId", SqlDbType.Int).Value = configurationEnvironmentId; 
                        command.Parameters.Add("@hierarchyId", SqlDbType.Int).Value = hierarchyId;
                        command.Parameters.Add("@hierarchyTypeId", SqlDbType.Int).Value = hierarchyTypeId;
                        command.Parameters.Add("@affiliateId", SqlDbType.Int).Value = affiliateId;
                        command.Parameters.Add("@isDefault", SqlDbType.TinyInt).Value = isDefault;
                        
                        command.ExecuteNonQuery();

                        if (isNew(configurationHierarchyId))
                        {
                            try { configurationHierarchyId = Convert.ToInt32(command.Parameters["@configurationHierarchyId"].Value); }
                            catch {/*do nothing*/}
                        }
                    }
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (DataSourceException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }
        #endregion

        #endregion

        #region Delete Methods

        #region DeleteConfigurationEnvironment
        /// <summary>
        /// Deletes configuration environment record.
        /// </summary>
        /// <param name="configurationEnvironmentId"></param>
        public void DeleteConfigurationEnvironment(int configurationEnvironmentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("dbo.spDeleteConfigurationEnvironment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@configurationEnvironmentId", SqlDbType.Int).Value = configurationEnvironmentId;
                        command.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int,
                            4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty,
                            DataRowVersion.Default, null));
                    
                        command.ExecuteNonQuery();

                        int returnValue = 0;
                        try { returnValue = Convert.ToInt32(command.Parameters["ReturnValue"].Value); }
                        catch {/*do nothing*/}

                        if (returnValue > 0)
                        {
                            string errorMessage = string.Empty;
                            switch (returnValue)
                            {
                                // returnValue code == 1 -- could not delete child records
                                case 1:
                                    errorMessage = string.Format(__couldNotDeleteChildRecordsErrorMessage, configurationEnvironmentId);
                                    break;
                                // returnValue code == 2 -- could not delete parent record
                                case 2:
                                    errorMessage = string.Format(__couldNotDeleteParentRecordErrorMessage, configurationEnvironmentId);
                                    break;
                                // something else went wrong
                                default:
                                    errorMessage = __unknownErrorMessage;
                                    break;
                            }
                            throw new DataSourceException(errorMessage);
                        }
                    }
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }
        #endregion

        #region DeleteConfigurationHierarchy
        /// <summary>
        /// Deletes configuration environment record.
        /// </summary>
        /// <param name="configurationEnvironmentId"></param>
        public void DeleteConfigurationHierarchy(int configurationHierarchyId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("dbo.spDeleteConfigurationHierarchy", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@configurationHierarchyId", SqlDbType.Int).Value = configurationHierarchyId;

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }
        #endregion

        #endregion

        #endregion

        #region Protected Methods
        /// <summary>
        /// Returns true if the id is considered a "New" id. This is important
        /// for saving a record so that it calls the appropriate stored procedure.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected bool isNew(int id)
        {
            return id == -1 || id == 0;
        }
        #endregion 	
    }
}