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
    public class DalBroadcastMessage : Dal
    {
        #region constants
        /// <summary>
        /// Contains the key into the configuration block for retrieving the
        /// underling connection string.
        /// </summary>
        protected const string __connectionStringKey = "enterpriseConnectionString";
        /// <summary>
        /// Contains the name for the insert procedure for a broadcast message.
        /// </summary>
        protected const string __insertBroadcastMessageSPName = "spInsertBroadcastMessage";
        /// <summary>
        /// Contains the name for the update procedure for a broadcast message.
        /// </summary>
        protected const string __updateBroadcastMessageSPName = "spUpdateBroadcastMessage";
        /// <summary>
        /// Contains the name for the delete procedure for a broadcast message.
        /// </summary>
        protected const string __deleteBroadcastMessageSPName = "spDeleteBroadcastMessage";
        /// <summary>
        /// Contains the name for the get procedure for all broadcast messages.
        /// </summary>
        protected const string __getBroadcastMessagesSPName = "spGetBroadcastMessages";
        /// <summary>
        /// Message to use when login is invalid/blank.
        /// </summary>
        protected const string __invalidLogin = "Invalid/Blank Login specified.";
        /// <summary>
        /// Message to use when BroadcastMessageId is null.
        /// </summary>
        protected const string __invalidBroadcastMessageId = "Invalid/Blank BroadcastMessageId specified.";
        /// <summary>
        /// Message to use when ApplicationTypeId is null.
        /// </summary>
        protected const string __invalidApplicationType = "Invalid/Blank ApplicationType specified.";
        /// <summary>
        /// Message to use when ChannelId is null.
        /// </summary>
        protected const string __invalidChannel = "Invalid/Blank Channel specified.";
        /// <summary>
        /// Message to use when Value is blank.
        /// </summary>
        protected const string __valueCannotBeNull = "The Broadcast Message value cannot be blank.";
        /// <summary>
        /// Message to use when an unexpected error occurred trying to insert/update or delete a broadcast message.
        /// </summary>
        protected const string __unexpected = "An unexpected error occurred inserting/updating/deleting the BroadcastMessage.";
        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor.
        /// </summary>
        public DalBroadcastMessage() : base(__connectionStringKey) { }
        #endregion ctors

        #region GetBroadcastMessages
        /// <summary>
        /// Gets all broadcast messages
        /// </summary>
        /// <returns></returns>
        public BroadcastMessageSchema.BroadcastMessageDataTable GetBroadcastMessages(int applicationTypeId, int channelId)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionString))
                {
                    // open connection
                    try { sqlConn.Open(); }
                    catch { throw new LogonException(); }
                    // now execute command
                    using (SqlCommand cmd = new SqlCommand(__getBroadcastMessagesSPName, sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@applicationTypeId", SqlDbType.Int).Value = applicationTypeId;
                        cmd.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;

                        //Build the dataadapter
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        //Create the datatable to fill
                        BroadcastMessageSchema.BroadcastMessageDataTable dt = new BroadcastMessageSchema.BroadcastMessageDataTable();

                        //Fill the datatable
                        da.Fill(dt);

                        //Return DataTable.
                        return dt;
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
        #endregion GetBroadcastMessages

        #region AddBroadcastMessage
        /// <summary>
        /// Saves a new message to the DB
        /// </summary>
        /// <param name="applicationTypeId"></param>
        /// <param name="channelId"></param>
        /// <param name="login"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="message"></param>
        /// <param name="broadcastMessageId"></param>
        /// <param name="createDate"></param>
        public void AddBroadcastMessage(int applicationTypeId, int channelId, string login, DateTime startDate, DateTime endDate, string message, ref int broadcastMessageId, ref DateTime createDate)
        {
            saveBroadcastMessage(true, applicationTypeId, channelId, login, startDate, endDate, message, ref broadcastMessageId, ref createDate);
        }
        #endregion AddBroadcastMessage

        #region UpdateBroadcastMessage
        /// <summary>
        /// Updates an existing message to the DB
        /// </summary>
        /// <param name="applicationTypeId"></param>
        /// <param name="channelId"></param>
        /// <param name="login"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="message"></param>
        /// <param name="broadcastMessageId"></param>
        public void UpdateBroadcastMessage(int applicationTypeId, int channelId, string login, DateTime startDate, DateTime endDate, string message, int broadcastMessageId)
        {
            DateTime createDate = DateTime.MinValue;
            int messageId = broadcastMessageId;
            saveBroadcastMessage(false, applicationTypeId, channelId, login, startDate, endDate, message, ref messageId, ref createDate);
        }
        #endregion UpdateBroadcastMessage

        #region DeleteBroadcastMessage
        /// <summary>
        /// Deletes a BroadcastMessage
        /// </summary>
        /// <param name="broadcastMessageId"></param>
        public void DeleteBroadcastMessage(int broadcastMessageId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    try { connection.Open(); }
                    catch { throw new LogonException(); }

                    using (SqlCommand command = new SqlCommand(__deleteBroadcastMessageSPName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@broadcastMessageId", SqlDbType.Int).Value = broadcastMessageId;

                        command.ExecuteNonQuery();
                    }
                }
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
        #endregion DeleteBroadcastMessage

        #region saveBroadcastMessage
        /// <summary>
        /// Internal method that handles inserts/updates of a broadcast message.
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="applicationTypeId"></param>
        /// <param name="channelId"></param>
        /// <param name="login"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="message"></param>
        /// <param name="broadcastMessageId"></param>
        /// <param name="createDate"></param>
        protected void saveBroadcastMessage(bool isNew, int applicationTypeId, int channelId, string login, DateTime startDate, DateTime endDate, string message, ref int broadcastMessageId, ref DateTime createDate)
        {
            // check for null.
            if (message == null) throw new DataSourceException(__valueCannotBeNull);

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_connectionString))
                {
                    // open connection
                    try { sqlConn.Open(); }
                    catch { throw new LogonException(); }
                    // now execute command
                    string procedureName = isNew ? __insertBroadcastMessageSPName : __updateBroadcastMessageSPName;
                    using (SqlCommand cmd = new SqlCommand(procedureName, sqlConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // setup return variable
                        cmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Int,
                            4, ParameterDirection.ReturnValue, false, 0, 0, string.Empty,
                            DataRowVersion.Default, null));

                        cmd.Parameters.Add("@applicationTypeId", SqlDbType.Int).Value = applicationTypeId;
                        cmd.Parameters.Add("@channelId", SqlDbType.Int).Value = channelId;
                        cmd.Parameters.Add("@login", SqlDbType.VarChar, 100).Value = (login == null ? string.Empty : login);

                        if (startDate == DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                        }

                        if (endDate == DateTime.MinValue)
                        {
                            cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;
                        }

                        cmd.Parameters.Add("@value", SqlDbType.VarChar, 8000).Value = message;

                        SqlParameter pCreateDate = null;
                        if (isNew)
                        {
                            pCreateDate = cmd.Parameters.Add("@createDate", SqlDbType.DateTime);
                            pCreateDate.Direction = ParameterDirection.InputOutput;
                            pCreateDate.Value = DBNull.Value;
                        }

                        SqlParameter pBroadcastMessageId = cmd.Parameters.Add("@broadcastMessageId", SqlDbType.Int);
                        pBroadcastMessageId.Direction = isNew ? ParameterDirection.InputOutput : ParameterDirection.Input;
                        pBroadcastMessageId.Value = broadcastMessageId;

                        cmd.ExecuteNonQuery();

                        int ret = 0;
                        try { ret = Convert.ToInt32(cmd.Parameters["ReturnValue"].Value); }
                        catch {/*do nothing*/}

                        if (ret == 0 && isNew)
                        {
                            try { broadcastMessageId = Convert.ToInt32(pBroadcastMessageId.Value); }
                            catch {/*do nothing*/}
                            try { createDate = Convert.ToDateTime(pCreateDate.Value); }
                            catch {/*do nothing*/}
                        }
                        else if (ret > 0)
                        {
                            switch (ret)
                            {
                                case 1:
                                case 2:
                                    throw new RecordDoesNotExistException(__invalidLogin);
                                case 3:
                                    throw new DataSourceException(__invalidBroadcastMessageId);
                                case 4:
                                    throw new DataSourceException(__invalidApplicationType);
                                case 5:
                                    throw new DataSourceException(__invalidChannel);
                                case 6:
                                    throw new DataSourceException(__valueCannotBeNull);
                                default:
                                    throw new DataSourceException(__unexpected);
                            }
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
        #endregion saveBroadcastMessage
    }
}
