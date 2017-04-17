		using System;
		using System.Text;
		using System.Data;
		using System.Data.OracleClient;

		using Cox.DataAccess;
		using Cox.DataAccess.Exceptions;

		namespace Cox.DataAccess.Account
		{
			/// <summary>
			/// Summary description for DalCustomerDetails.
			/// </summary>
			public class DalCustomerDetailsInfinys : Dal
			{				
				#region Constants
				/// <summary>
				/// The connectionKey into the connections configuration section of the config block.
				/// </summary>
				//protected const string __connectionKey="accountConnectionString";

				protected const string _defaultconnectionKey="accountInfinysConnectionString";
				
				#endregion constants
		
				#region Ctors
				/// <summary>
				/// Default constructor. This constructor retreives its 
				/// connection information from the configuration block.
				/// </summary>
				
				public DalCustomerDetailsInfinys(string siteCode):base(siteCode + _defaultconnectionKey){}
				/// <summary>
				/// Constructor that takes a param for the conn key then retreives its 
				/// connection information from the configuration block.
				/// </summary>
				
				#endregion 

				#region public methods
				
				 /// <summary>
				/// This method retrieves comments about the customer from Infinyse.
				/// </summary>
				/// <param name="siteId">Site ID</param>
				/// <param name="siteCode">Site Code</param>
				/// <param name="accountNumber9">Customer Account Number</param>
				/// <returns>CustomerDetailsSchema.CustomerCommentsDataDataTable</returns>
				public CustomerDetailsSchema.CustomerCommentsInfinysDataDataTable GetCustomerComments(int siteId, string siteCode, string accountNumber9)
				{					
					try
					{
						using(OracleConnection oracleConn=new OracleConnection(_connectionString))
						{
							// open connection
							try{oracleConn.Open();}
							catch{throw new LogonException();}
							
							// build the command object
							using(OracleCommand oracleCMD = new OracleCommand())
							{
								oracleCMD.Connection = oracleConn;
								oracleCMD.CommandText = "WEB_SERVICES_PKG.GET_CUSTOMER_COMMENTS";
								oracleCMD.CommandType = CommandType.StoredProcedure;
								oracleCMD.Parameters.Add("accountnumberIn", OracleType.Number).Value = Convert.ToDecimal(accountNumber9);
								oracleCMD.Parameters.Add("siteidIn", OracleType.Number).Value = siteId;
								oracleCMD.Parameters.Add("ERROR", OracleType.Number).Direction = ParameterDirection.Output;
								oracleCMD.Parameters.Add("p_cursor", OracleType.Cursor).Direction = ParameterDirection.Output;
														
								// build the dataadapter
								using(OracleDataAdapter da = new OracleDataAdapter(oracleCMD ))
								{
									// create the dataset to fill
									CustomerDetailsSchema ds = new CustomerDetailsSchema();
									// now fill it
									da.Fill(ds.CustomerCommentsInfinysData);
									// all done, return
									return ds.CustomerCommentsInfinysData;
								}
							}
						}
					}
					catch( LogonException )
					{
						// just rethrow it. it is from our internal code block
						throw;
					}
					catch( Exception ex )
					{
						// DataSourceException.
						throw new DataSourceException( ex );
					}
				}

				/// <summary>
				/// This method retrieves comments about the customer from Infinyse.
				/// </summary>
				/// <param name="siteId"></param>
				/// <param name="siteCode"></param>
				/// <param name="accountNumber9"></param>
				/// <param name="fromDate"></param>
				/// <param name="toDate"></param>
				/// <returns></returns>
				public CustomerDetailsSchema.CustomerCommentsInfinysDataDataTable GetCustomerComments(int siteId, string siteCode, string accountNumber9, DateTime fromDate, DateTime toDate)
				{					
					try
					{
						using(OracleConnection oracleConn=new OracleConnection(_connectionString))
						{
							// open connection
							try{oracleConn.Open();}
							catch{throw new LogonException();}
							
							// build the command object
							using(OracleCommand oracleCMD = new OracleCommand())
							{
								//NEED NEW PROC!!!!!!
								oracleCMD.Connection = oracleConn;
								oracleCMD.CommandText = "WEB_SERVICES_PKG.GET_CUSTOMER_COMMENTS";
								oracleCMD.CommandType = CommandType.StoredProcedure;
								oracleCMD.Parameters.Add("accountnumberIn", OracleType.Number).Value = Convert.ToDecimal(accountNumber9);
								oracleCMD.Parameters.Add("siteidIn", OracleType.Number).Value = siteId;
								oracleCMD.Parameters.Add("ERROR", OracleType.Number).Direction = ParameterDirection.Output;
								oracleCMD.Parameters.Add("p_cursor", OracleType.Cursor).Direction = ParameterDirection.Output;
														
								// build the dataadapter
								using(OracleDataAdapter da = new OracleDataAdapter(oracleCMD ))
								{
									// create the dataset to fill
									CustomerDetailsSchema ds = new CustomerDetailsSchema();
									// now fill it
									da.Fill(ds.CustomerCommentsInfinysData);
									// all done, return
									return ds.CustomerCommentsInfinysData;
								}
							}
						}
					}
					catch( LogonException )
					{
						// just rethrow it. it is from our internal code block
						throw;
					}
					catch( Exception ex )
					{
						// DataSourceException.
						throw new DataSourceException( ex );
					}
				}

				#endregion public methods
			}
		}

		