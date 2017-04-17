using System;
using System.Data;
using System.Data.SqlClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Enterprise
{
	public class DalUserSetting :Dal
	{

		#region Constants

		/// <summary>
		/// The name of thge connection string key
		/// </summary>
		protected const string __connectionStringKey = "enterpriseConnectionString";

		#endregion
		
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public DalUserSetting() : base(__connectionStringKey){}

		#endregion

		#region Public Methods

		#region Get Methods

		/// <summary>
		/// Gets a user setting with the given name for the a userId and application name
		/// </summary>
		public UserSettingSchema.UserSetting GetUserSetting(string login, int applicationTypeId, string settingName)
		{
			try
			{
				using(SqlConnection connection = new SqlConnection(_connectionString))
				{
					try{connection.Open();}		
					catch{throw new LogonException();}
				
					using(SqlCommand command = new SqlCommand("spGetUserSetting",connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.Add("@login",SqlDbType.VarChar,100).Value = login;
						command.Parameters.Add("@applicationTypeId",SqlDbType.Int).Value = applicationTypeId;
						command.Parameters.Add("@name",SqlDbType.VarChar,100).Value = settingName;
					
						using (SqlDataAdapter adapter = new SqlDataAdapter(command))
						{
							UserSettingSchema.UserSetting userSetting = null;

							UserSettingSchema.UserSettingsDataTable userSettings = new UserSettingSchema.UserSettingsDataTable();
							adapter.Fill(userSettings);
							if(userSettings.Rows.Count >0)
							{
								userSetting = (UserSettingSchema.UserSetting)userSettings.Rows[0];
							}
							return userSetting;
						}
					}
				}
			}
			catch(LogonException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DataSourceException(ex);
			}		
		}


		/// <summary>
		/// Gets all settings for the given user and application name.
		/// </summary>
		public UserSettingSchema.UserSettingsDataTable GetUserSettings(string login, int applicationTypeId)
		{
			try
			{
				using(SqlConnection connection = new SqlConnection(_connectionString))
				{
					try{connection.Open();}		
					catch{throw new LogonException();}
				
					using(SqlCommand command = new SqlCommand("spGetUserSettings",connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						command.Parameters.Add("@login",SqlDbType.VarChar,100).Value = login;
						command.Parameters.Add("@applicationTypeId",SqlDbType.Int).Value = applicationTypeId;
					
						using (SqlDataAdapter adapter = new SqlDataAdapter(command))
						{
							UserSettingSchema.UserSettingsDataTable userSettings = new UserSettingSchema.UserSettingsDataTable();
							adapter.Fill(userSettings);
							return userSettings;
						}
					}
				}
			}
			catch(LogonException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DataSourceException(ex);
			}		
		}

		#endregion

		#region Set Methods

		/// <summary>
		/// Updates a user setting for the user.
		/// </summary>
		public void UpdateUserSetting(string login, int applicationTypeId, string settingName, string value)
		{
			try
			{
				using(SqlConnection connection = new SqlConnection(_connectionString))
				{
					try{connection.Open();}		
					catch{throw new LogonException();}
				
					using(SqlCommand command = new SqlCommand("spUpdateUserSetting",connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						
						command.Parameters.Add("@login",SqlDbType.VarChar,100).Value = login;
						command.Parameters.Add("@applicationTypeId",SqlDbType.Int).Value = applicationTypeId;
						command.Parameters.Add("@settingName",SqlDbType.VarChar,100).Value = settingName;
						command.Parameters.Add("@value",SqlDbType.VarChar,8000).Value = value;
						
						command.ExecuteNonQuery();
					}
				}
			}
			catch(LogonException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DataSourceException(ex);
			}		
		}

		/// <summary>
		/// Inserts a user setting for the user.
		/// </summary>
		public void InsertUserSetting(string login, int applicationTypeId, string settingName, string value)
		{
			try
			{
				using(SqlConnection connection = new SqlConnection(_connectionString))
				{
					try{connection.Open();}		
					catch{throw new LogonException();}
				
					using(SqlCommand command = new SqlCommand("spInsertUserSetting",connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						
						command.Parameters.Add("@login",SqlDbType.VarChar,100).Value = login;
						command.Parameters.Add("@applicationTypeId",SqlDbType.Int).Value = applicationTypeId;
						command.Parameters.Add("@settingName",SqlDbType.VarChar,100).Value = settingName;
						command.Parameters.Add("@value",SqlDbType.VarChar,8000).Value = value;
						
						command.ExecuteNonQuery();
					}
				}
			}
			catch(LogonException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DataSourceException(ex);
			}			
		}

		#endregion

		#region Delete Methods

		public void DeleteUserSetting(string login, int applicationTypeId, string settingName)
		{
			try
			{
				using(SqlConnection connection = new SqlConnection(_connectionString))
				{
					try{connection.Open();}		
					catch{throw new LogonException();}
				
					using(SqlCommand command = new SqlCommand("spDeleteUserSetting",connection))
					{
						command.CommandType = CommandType.StoredProcedure;
						
						command.Parameters.Add("@login",SqlDbType.VarChar,100).Value = login;
						command.Parameters.Add("@applicationTypeId",SqlDbType.Int).Value = applicationTypeId;
						command.Parameters.Add("@settingName",SqlDbType.VarChar,100).Value = settingName;
						
						command.ExecuteNonQuery();
					}
				}
			}
			catch(LogonException)
			{
				throw;
			}
			catch(Exception ex)
			{
				throw new DataSourceException(ex);
			}			
		}

		#endregion
		#endregion
	}
}
