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
    public class DalAuthorizationUser : Dal
    {
        #region Constants
        /// <summary>
        /// Contains the key into the configuration block for retrieving the
        /// underling connection string.
        /// </summary>
        protected const string __connectionStringKey = "enterpriseConnectionString";
        /// <summary>
        /// Contains the name for the get procedure for AuthorizationUsers.
        /// </summary>
        protected const string __getAuthorizationUsersSPName = "spGetAuthorizationUsers";
        /// <summary>
        /// Error message for when the user id is not found for the login.
        /// </summary>
        protected const string __unableToFindUserIdErrorMessage = "Unable to find a user id for login '{0}'.";
        /// <summary>
        /// Error message for when the role association id is not found.
        /// </summary>
        protected const string __unableToFindRoleAssociationIdErrorMessage = "Unable to find role association id for channel id '{0}', business unit id '{1}' and role id '{2}'";
        /// <summary>
        /// Error message for when the login already exists.
        /// </summary>
        protected const string __loginAlreadyExistsErrorMessage = "The login '{0}' already exists in the database.";
        /// <summary>
        /// Error message for when a required parameter is null.
        /// </summary>
        protected const string __parameterCannotBeNullErrorMessage = "The parameter '{0}' cannot be null.";
        /// <summary>
        /// Error message for when records cannot be deleted.
        /// </summary>
        protected const string __couldNotDeleteRecordErrorMessage = "Could not delete records from table '{0}' for login '{1}'}";
        /// <summary>
        /// Error message for when something unknown went wrong trying to delete a config record.
        /// </summary>
        protected const string __unknownErrorMessage = "Something went wrong, but we don't why.";
        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public DalAuthorizationUser() : base(__connectionStringKey) { }
        #endregion ctors

        #region Public Methods

        #region Authorization Methods

        #region IsAuthorized
        /// <summary>
        /// Checks if the user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskName"></param>
        /// <param name="siteId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public bool IsAuthorized(string userId, string taskName, int siteId, int channelId)
        {
            return IsUserAuthorized(userId, taskName, siteId, channelId);
        }
        #endregion IsAuthorized

        #region IsAuthorized(2)
        /// <summary>
        /// Checks if the user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskName"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool IsAuthorized(string userId, string taskName, int siteId)
        {
            return IsUserAuthorized(userId, taskName, siteId, -1);
        }
        #endregion IsAuthorized(2)

        #region IsAuthorized(3)
        /// <summary>
        /// Checks if the user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskName"></param>
        /// <returns></returns>
        public bool IsAuthorized(string userId, string taskName)
        {
            return IsUserAuthorized(userId, taskName, -1, -1);
        }
        #endregion IsAuthorized(3)

        #endregion

        #region Get Methods

        #region GetAuthorizationUsers
        /// <summary>
        /// Gets a list of all AuthorizationUsers.
        /// </summary>
        /// <param name="applicationTypeId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public AuthorizationUserSchema.AuthorizationUserDataTable GetAuthorizationUsers(int? channelId, int? siteId, bool? isActive)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionString))
                {
                    // open connection
                    try { sqlConn.Open(); }
                    catch { throw new LogonException(); }
                    // now execute command
                    using (SqlCommand command = new SqlCommand(__getAuthorizationUsersSPName, sqlConn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (channelId.HasValue) command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        if (siteId.HasValue) command.Parameters.Add("@businessUnitId", SqlDbType.Int).Value = siteId;
                        if (isActive.HasValue) command.Parameters.Add("@isActive", SqlDbType.Int).Value = isActive;

                        //Build the dataadapter
                        using (SqlDataAdapter da = new SqlDataAdapter(command))
                        {
                            //Create the datatable to fill
                            AuthorizationUserSchema.AuthorizationUserDataTable dt = new AuthorizationUserSchema.AuthorizationUserDataTable();

                            //Fill the datatable
                            da.Fill(dt);

                            //Return DataTable.
                            return dt;
                        }
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
        #endregion GetAuthorizationUsers

        #region GetBusinessUnits
        /// <summary>
        /// Default method that assumes cox.com channel
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AuthorizationUserSchema.BusinessUnitDataTable GetBusinessUnits(string userId)
        {
            //user cox.com channel
            return GetBusinessUnitsForUser(userId, 1);
        }
        #endregion GetBusinessUnits

        #region GetBusinessUnits(2)
        /// <summary>
        /// Get the business units for a user and channel
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public AuthorizationUserSchema.BusinessUnitDataTable GetBusinessUnits(string userId, int channelId)
        {
            return GetBusinessUnitsForUser(userId, channelId);
        }
        #endregion GetBusinessUnits(2)

        #region GetRoles
        /// <summary>
        /// Gets a list roles for a user, channel, and business unit (site for now)
        /// </summary>
        /// <param name="login"></param>
        /// <param name="channelId"></param>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public AuthorizationUserSchema.RoleDataTable GetRoles(string login, int channelId, int businessUnit)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spGetRolesForUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("@businessUnitId", SqlDbType.Int).Value = businessUnit;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            AuthorizationUserSchema authorizationUserSchema = new AuthorizationUserSchema();
                            adapter.Fill(authorizationUserSchema.Role);
                            return authorizationUserSchema.Role;
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

        #region GetRoles
        /// <summary>
        /// Gets a list roles for a channel, and business unit (site for now)
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="businessUnit"></param>
        /// <returns></returns>
        public AuthorizationUserSchema.RoleDataTable GetRoles(int channelId, int businessUnit)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spGetRoles", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("@businessUnitId", SqlDbType.Int).Value = businessUnit;

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            AuthorizationUserSchema authorizationUserSchema = new AuthorizationUserSchema();
                            adapter.Fill(authorizationUserSchema.Role);
                            return authorizationUserSchema.Role;
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
        #endregion GetBusinessUnitsForUser

        #endregion

        #region Insert Methods

        #region CreateUserRoleAssociation
        /// <summary>
        /// Creates an association between a user and a role.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="channelId"></param>
        /// <param name="businessUnitId"></param>
        /// <returns></returns>
        public void CreateUserRoleAssociation(string login, int channelId, int businessUnitId, int roleId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spInsertUserRoleAssociation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("@businessUnitId", SqlDbType.Int).Value = businessUnitId;
                        command.Parameters.Add("@roleId", SqlDbType.Int).Value = roleId;
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
                                // returnValue code == 1 -- could not find the user id for the login
                                case 1:
                                    errorMessage = string.Format(__unableToFindUserIdErrorMessage, login);
                                    break;
                                // returnValue code == 2 -- could not find the role association.
                                case 2:
                                    errorMessage = string.Format(__unableToFindRoleAssociationIdErrorMessage, channelId, businessUnitId);
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

        #endregion

        #region SaveMethods

        #region SaveAuthorizationUser
        /// <summary>
        /// Saves user information to the db.
        /// </summary>
        /// <param name="authorizationUserId"></param>
        /// <param name="login"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="isActive"></param>
        public void SaveAuthorizationUser(ref int authorizationUserId, string login, string firstName, string middleName, string lastName, string emailAddress, bool isActive)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string procedureName = isNew(authorizationUserId) ? "dbo.spInsertAuthorizationUser" : "dbo.spUpdateAuthorizationUser";

                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@authorizationUserId", SqlDbType.Int).Value = authorizationUserId;
                        command.Parameters["@authorizationUserId"].Direction = isNew(authorizationUserId) ? ParameterDirection.Output : ParameterDirection.Input;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
                        command.Parameters.Add("@firstName", SqlDbType.VarChar, 50).Value = firstName;
                        command.Parameters.Add("@middleName", SqlDbType.VarChar, 50).Value = middleName;
                        command.Parameters.Add("@lastName", SqlDbType.VarChar, 50).Value = lastName;
                        command.Parameters.Add("@emailAddress", SqlDbType.VarChar, 50).Value = emailAddress;
                        command.Parameters.Add("@isActive", SqlDbType.TinyInt).Value = isActive;
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
                                // returnValue code == 1 -- login already exists
                                case 1:
                                    errorMessage = string.Format(__loginAlreadyExistsErrorMessage, login);
                                    break;
                                // returnValue code == 3 -- login was null
                                case 3:
                                    errorMessage = string.Format(__parameterCannotBeNullErrorMessage, "login");
                                    break;
                                // returnValue code == 4 -- first name was null
                                case 4:
                                    errorMessage = string.Format(__parameterCannotBeNullErrorMessage, "first Name");
                                    break;
                                // returnValue code == 5 -- last name was null
                                case 5:
                                    errorMessage = string.Format(__parameterCannotBeNullErrorMessage, "last name");
                                    break;
                                // returnValue code == 6 -- email address was null
                                case 6:
                                    errorMessage = string.Format(__parameterCannotBeNullErrorMessage, "email address");
                                    break;
                                // something else went wrong
                                default:
                                    errorMessage = __unknownErrorMessage;
                                    break;
                            }
                            throw new DataSourceException(errorMessage);
                        }

                        // set id
                        if (isNew(authorizationUserId))
                        {
                            try { authorizationUserId = Convert.ToInt32(command.Parameters["@authorizationUserId"].Value); }
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

        #region DeleteUserRoleAssociation
        /// <summary>
        /// Deletes an association between a user and a role.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="channelId"></param>
        /// <param name="businessUnitId"></param>
        /// <returns></returns>
        public void DeleteUserRoleAssociation(string login, int channelId, int businessUnitId, int roleId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spDeleteUserRoleAssociation", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("@businessUnitId", SqlDbType.Int).Value = businessUnitId;
                        command.Parameters.Add("@roleId", SqlDbType.Int).Value = roleId;
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
                                // returnValue code == 1 -- could not find the user id for the login
                                case 1:
                                    errorMessage = string.Format(__unableToFindUserIdErrorMessage, login);
                                    break;
                                // returnValue code == 2 -- could not find the role association
                                case 2:
                                    errorMessage = string.Format(__unableToFindRoleAssociationIdErrorMessage, channelId, businessUnitId);
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

        #region DeleteAuthorizationUser
        /// <summary>
        /// Deletes a user record.
        /// </summary>
        /// <param name="login"></param>
        public void DeleteAuthorizationUser(string login)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("dbo.spDeleteAuthorizationUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = login;
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
                                // returnValue code == 1 -- could not find id for login
                                case 1:
                                    errorMessage = string.Format(__unableToFindUserIdErrorMessage, login);
                                    break;
                                // returnValue code == 2 -- could not delete config hierachy records
                                case 2:
                                    errorMessage = string.Format(__couldNotDeleteRecordErrorMessage, "Configuration Hierarchy", login);
                                    break;
                                // returnValue code == 3 -- could not delete config records
                                case 3:
                                    errorMessage = string.Format(__couldNotDeleteRecordErrorMessage, "Configuration Env.", login);
                                    break;
                                // returnValue code == 4 -- could not delete role association records
                                case 4:
                                    errorMessage = string.Format(__couldNotDeleteRecordErrorMessage, "Role Association", login);
                                    break;
                                // returnValue code == 5 -- could not delete user records
                                case 5:
                                    errorMessage = string.Format(__couldNotDeleteRecordErrorMessage, "Authorization User", login);
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

        #endregion

        #endregion

        #region Protected Methods

        #region IsUserAuthorized
        /// <summary>
        /// Checks if the user is authorized
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskName"></param>
        /// <param name="siteId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        protected bool IsUserAuthorized(string userId, string taskName, int siteId, int channelId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spIsAuthorized", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = userId;
                        command.Parameters.Add("@taskName", SqlDbType.VarChar, 50).Value = taskName;
                        command.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        command.Parameters.Add("ReturnValue", SqlDbType.TinyInt);
                        command.Parameters["ReturnValue"].Direction = ParameterDirection.ReturnValue;

                        command.ExecuteNonQuery();

                        return Convert.ToBoolean(command.Parameters["ReturnValue"].Value);
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
        #endregion IsUserAuthorized

        #region GetBusinessUnitsForUser
        /// <summary>
        /// Gets a list of business units for a user and tasks
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        protected AuthorizationUserSchema.BusinessUnitDataTable GetBusinessUnitsForUser(string userId, int channelId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand("spGetBusinessUnitsForUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = userId;
                        command.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;


                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            AuthorizationUserSchema authorizationUserSchema = new AuthorizationUserSchema();
                            adapter.Fill(authorizationUserSchema.BusinessUnit);
                            return authorizationUserSchema.BusinessUnit;
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
        #endregion GetBusinessUnitsForUser

        #region isNew
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

        #endregion
    }
}
