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
using Cox.Security.Cryptography;

namespace Cox.DataAccess.Enterprise
{
	/// <summary>
	/// Provides and caches source information
	/// </summary>
	public class DalUser : Dal
	{
		#region constants
		/// <summary>
		/// Contains the key into the configuration block for retrieving the
		/// underling connection string.
		/// </summary>
		public const string __connectionStringKey ="enterpriseConnectionString";
		#endregion constants

		#region ctors
		/// <summary>Private contructor only called by instance member.</summary>
		public DalUser() : base( __connectionStringKey ){}
		#endregion ctors

		#region public functions
		/// <summary>
		/// Looks up Source Id based on source username provided
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		/// <exception cref="LogonException">Thrown when a problem occurred
		/// trying to connect to the underlying datasource.</exception>
		/// <exception cref="DataSourceException">Thrown when a problem occurred
		/// trying to interact with the underlying datasource after a connection
		/// has been established. Check the inner exception for further meaning
		/// as to why the problem occurred.</exception>
		public int GetUserId(string userName)
		{
			try
			{
				using( SqlConnection sqlConn = new SqlConnection( _connectionString ) )
				{
					// open connection
					try{sqlConn.Open();}
					catch{throw new LogonException();}
					// now exceute command
					using(SqlCommand cmd=new SqlCommand("spGetUserId",sqlConn))
					{
						cmd.CommandType=CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("ReturnValue",
							SqlDbType.Int,4,ParameterDirection.ReturnValue,
							false,0,0,string.Empty,DataRowVersion.Default,null));
						cmd.Parameters.Add("@userName",SqlDbType.VarChar,50).Value=userName;
						cmd.Parameters.Add(new SqlParameter("@userId",
							SqlDbType.Int,4,ParameterDirection.Output,
							false,0,0,string.Empty,DataRowVersion.Default,-1));
						cmd.ExecuteNonQuery();
						int ret=0;
						try{ret=Convert.ToInt32(cmd.Parameters["ReturnValue"].Value);}
						catch{/*do nothing*/}
						if(ret == 1)
						{
							// this should never ever happen, but if it does we need to handle it.
							throw new DataSourceException("Null UserName.");
						}
						try{ret=Convert.ToInt32(cmd.Parameters["@userId"].Value);}
						catch{/*do nothing*/}
						return ret;
					}
				}
			}
			catch(DataSourceException)
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch(Exception ex)
			{
				// DataSourceException.
				throw new DataSourceException(ex);
			}
		}
		/// <summary>
		/// Looks up and returns group information based on userid.
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public Group GetGroupInformation(int userId)
		{
			try
			{
				using( SqlConnection sqlConn = new SqlConnection( _connectionString ) )
				{
					// open connection
					try{sqlConn.Open();}
					catch{throw new LogonException();}
					// now exceute command
					using(SqlCommand cmd=new SqlCommand("spGetGroupInformation",sqlConn))
					{
						cmd.CommandType=CommandType.StoredProcedure;
						cmd.Parameters.Add(new SqlParameter("ReturnValue",
							SqlDbType.Int,4,ParameterDirection.ReturnValue,
							false,0,0,string.Empty,DataRowVersion.Default,null));
						cmd.Parameters.Add("@userId",SqlDbType.Int).Value=userId;
						cmd.Parameters.Add(new SqlParameter("@groupId",
							SqlDbType.Int,4,ParameterDirection.Output,
							false,0,0,string.Empty,DataRowVersion.Default,-1));
						cmd.Parameters.Add(new SqlParameter("@groupName",
							SqlDbType.VarChar,50,ParameterDirection.Output,
							false,0,0,string.Empty,DataRowVersion.Default,-1));
						cmd.ExecuteNonQuery();
						int ret =0;
						try{ret = Convert.ToInt32(cmd.Parameters[ "ReturnValue" ].Value);}
						catch{/*do nothing*/}
						if(ret==1||ret==2)
						{
							// this should never ever happen, but if it does we need to handle it.
							throw new DataSourceException(string.Format("Null/Invalid UserId."));
						}
						int groupId=0;
						string groupName=null;
						try{groupId=Convert.ToInt32(cmd.Parameters["@groupId"].Value);}
						catch{/*do nothing*/}
						try{groupName=Convert.ToString(cmd.Parameters["@groupName"].Value);}
						catch{/*do nothing*/}
						return new Group(groupId,groupName);
					}
				}
			}
			catch(DataSourceException)
			{
				// just rethrow it. it is from our internal code block
				throw;
			}
			catch(Exception ex)
			{
				// DataSourceException.
				throw new DataSourceException(ex);
			}
		}

        /// <summary>
        /// Gets ICOMS credentials for a given gateway, site and user.
        /// </summary>
        /// <param name="gatewayId"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IcomsCredential GetIcomsCredentials(int gatewayId, int siteId, int userId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(_connectionString))
                {
                    try { sqlConnection.Open(); }
                    catch (Exception) { throw new LogonException(); }

                    using (SqlCommand cmd = new SqlCommand("spGetIcomsCredentialByGatewaySiteUser", sqlConnection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@gatewayId", SqlDbType.Int).Value = gatewayId;
                        cmd.Parameters.Add("@siteId", SqlDbType.Int).Value = siteId;
                        cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

                        cmd.Parameters.Add(new SqlParameter("@loginName",
                            SqlDbType.VarChar, 50, ParameterDirection.Output,
                            false, 0, 0, string.Empty, DataRowVersion.Default, -1));

                        cmd.Parameters.Add(new SqlParameter("@password",
                            SqlDbType.VarChar, 50, ParameterDirection.Output,
                            false, 0, 0, string.Empty, DataRowVersion.Default, -1));

                        cmd.ExecuteNonQuery();

                        string loginName = null;
                        string password = null;

                        try { loginName = Convert.ToString(cmd.Parameters["@loginName"].Value); }
                        catch {/*do nothing*/}

                        try { password = Convert.ToString(cmd.Parameters["@password"].Value); }
                        catch {/*do nothing*/}

                        return new IcomsCredential(loginName, password);
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
		#endregion public functions
	}

	#region public structures
	/// <summary>
	/// Simple structure for user/group information.
	/// </summary>
	public struct Group
	{
		#region member variables
		/// <summary>The groupId</summary>
		private int _groupId;
		/// <summary>The groupName</summary>
		private string _groupName;
		#endregion member variables

		#region properties
		/// <summary>
		/// Gets or sets the GroupId
		/// </summary>
		public int GroupId
		{
			get{return _groupId;}
			set{_groupId=value;}
		}
		/// <summary>
		/// Gets or sets the GroupName
		/// </summary>
		public string GroupName
		{
			get{return _groupName;}
			set{_groupName=value;}
		}
		#endregion properties

		#region ctors
		/// <summary>
		/// The parameter constructor.
		/// </summary>
		/// <param name="groupId"></param>
		/// <param name="groupName"></param>
		public Group(int groupId,string groupName)
		{
			_groupId=groupId;
			_groupName=groupName;
		}
		#endregion ctors
	}

    /// <summary>
    /// Simple structure for ICOMS Credential information.
    /// </summary>
    public struct IcomsCredential
    {
        #region member variables
        /// <summary>The ICOMS login name</summary>
        private string _loginName;
        /// <summary>The password</summary>
        private string _password;
        #endregion member variables

        #region properties
        /// <summary>
        /// Gets or sets the LoginName
        /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = value; }
        }
        /// <summary>
        /// Gets or sets the Password
        /// </summary>
        public string Password
        {
            get
            {
                if (_password != null)
                {
                    // decrypt it
                    SimpleCrypto encryptor = new SimpleCrypto();
                    return encryptor.Decrypt(_password);
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value != null)
                {
                    // encrypt it
                    SimpleCrypto encryptor = new SimpleCrypto();
                    _password = encryptor.Encrypt(value);
                }
                else
                {
                    _password = value;
                }
            }
        }
        #endregion properties

        #region ctors
        /// <summary>
        /// The parameter constructor.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupName"></param>
        public IcomsCredential(string loginName, string password)
        {
            _loginName = loginName;
            _password = password;
        }
        #endregion ctors
    }
	#endregion public structures
}
