using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Cox.DataAccess;
using Cox.DataAccess.Exceptions;


namespace Cox.DataAccess.Enterprise
{

	/// <summary>
	/// This class implemnts the Data Access Layer for anything related to 
	/// a method of payment. This handles translation of MOPs from the
	/// internal system of record numbers (arbitrary) to a integer type
	/// representation.
	/// </summary>
	public class DalServiceFeatureLocation: Dal
	{

		#region Constants

		/// <summary>
		/// This constant indicates the key used to lookup the database
		/// connection string for this component.
		/// </summary>
		private const string kstrConnectionStringKey = "enterpriseConnectionString";

		#endregion // Constants


		#region Construction/Destruction

		/// <summary>
		/// This the default contructor for this class.
		/// </summary>
		public DalServiceFeatureLocation()
			: base( kstrConnectionStringKey )
		{ /* None necessary */ }

		#endregion // Construction/Destruction


		#region Methods
		/// <summary>
		/// This method looks up an Service Feature Location using a valid siteid.
		/// </summary>
		/// <param name="siteId">
		/// The siteid for customer account.
		/// </param>
		/// <param name="serviceFeatureId"></param>
		/// <returns>
		/// The ServiceFeatureLocation found within the system of record is 
		/// returned.
		/// </returns>
		public int GetServiceFeatureLocation( int siteId,int serviceFeatureId)
		{	
			int seviceFeatureLocationId = -1;
	 
			try
			{
				using( SqlConnection con = new SqlConnection( _connectionString ) )
				{
					// build the command object
					using (SqlCommand cmd = new SqlCommand( "spGetServiceFeatureLocation", con ))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						// Parameters					
						cmd.Parameters.Add( "@intSiteId", SqlDbType.Int ).Value = siteId;
						cmd.Parameters.Add( "@intServiceFeatureId", SqlDbType.Int ).Value = serviceFeatureId;
						cmd.Parameters.Add( "@intServiceFeatureLocationId", SqlDbType.Int ).Direction = ParameterDirection.Output;
						
						// Open connection and execute proc
						try
						{
							con.Open();
						}
						catch
						{
							throw new LogonException();
						}
						cmd.ExecuteNonQuery();
					
						// Get the output variable(s)
						
						try
						{
							seviceFeatureLocationId = Convert.ToInt32(cmd.Parameters[ "@intServiceFeatureLocationId" ].Value);
						}
						catch {}
						
						// Set to Unknown
						if(seviceFeatureLocationId==0)
						{
							seviceFeatureLocationId = -1;
						}
						return seviceFeatureLocationId;
					}	
					
				} // using( SqlConnection con =... )
			} // try
			catch( DataSourceException )
			{
				throw;
			} // catch( DataSourceException excData )
			catch( Exception exc )
			{
				throw new DataSourceException( exc );
			} // catch( Exception exc )
			
		} // GetServiceFeatureLocation()
		

		#endregion // Methods

	} // class DalIcomsAPIService

} // namespace Cox.DataAccess.Enterprise

