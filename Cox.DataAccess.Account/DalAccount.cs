using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;
using System.Collections.Generic;

namespace Cox.DataAccess.Account
{

	/// <summary>
	/// DataAccessLayer component used to access the CustomerAccount 
	/// information from its system of record. 
	/// </summary>
	public class DalAccount : DalAccountBase
	{
		#region constants
		/// <summary>
		/// The ssn is masked in the database with X's
		/// </summary>
		protected const string __ssnMaskedFormat = "XXXXX{0}";
		#endregion

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalAccount():this(__accountConnectionKey){}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalAccount(string connectionKey):base(connectionKey){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// This method returns the three digit statement codes for a 9 digit account number.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public string[] GetStatementCodeList(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try
					{
						oracleConn.Open();
					}
					catch
					{
						throw new LogonException();
					}

					// create the sql statement
					StringBuilder sb = new StringBuilder();
					sb.Append("SELECT TO_CHAR(STATEMENT_CODE) ");
					sb.AppendFormat("FROM {0}_CUSTOMER_STATEMENTS ", siteCode);
					sb.AppendFormat("WHERE SITE_ID = {0} ", siteId);
					sb.AppendFormat("AND ACCOUNT_NUMBER = {0}", accountNumber9);

					//Create the command object
					OracleCommand cmd = new OracleCommand(sb.ToString(), oracleConn);
					cmd.CommandType = CommandType.Text;


					//Use a datareader to get the data.
					using(OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						ArrayList statementList = new ArrayList();
						while(reader.Read())
						{
							statementList.Add(reader.GetString(0).PadLeft(3, '0'));
						}
						return (string[])statementList.ToArray(typeof(string));
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
		/// This method returns back customer account information based on the siteId/siteCode
		/// and 9 digit accountNumber
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CustomerAccountSchema.CustomerAddress GetCustomerAddress(int siteId,
			string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					// create the houseNumber
					string houseNumber=accountNumber9.Substring(0,accountNumber9.Length-2);
					// create the sql statement
                    //Start of Changes for Self Reg - Ernest Griffin
                    //StringBuilder sql=new StringBuilder();
                    //sql.Append( "select a.FIRST_NAME,a.MIDDLE_INITIAL,a.LAST_NAME,a.CUSTOMER_NAME,a.HOME_AREA_CODE,a.HOME_EXCHANGE_NUMBER,a.HOME_TELEPHONE_NUMBER," );
                    //sql.Append( "a.BUSINESS_AREA_CODE,a.BUSINESS_EXCHANGE_NUMBER,a.BUSINESS_TELEPHONE_NUMBER,a.OTHER_AREA_CODE,a.OTHER_EXCHANGE_NUMBER,a.OTHER_PHONE," );
                    //sql.Append( "b.ADDRESS_LINE_1,b.ADDRESS_LINE_2,b.ADDRESS_LINE_3,b.ADDRESS_LINE_4,ADDR_CITY,ADDR_STATE,ADDR_ZIP_4,ADDR_ZIP_5,BILL_TYPE_CODE from " );
                    //sql.Append( siteCode );
                    //sql.Append( "_CUSTOMER_MASTER a," );
                    //sql.Append( siteCode );
                    //sql.Append( "_HOUSE_MASTER b where a.SITE_ID=b.SITE_ID and a.ACCOUNT_NUMBER='" );
                    //sql.Append( accountNumber9 );
                    //sql.Append( "' and a.SITE_ID='" );
                    //sql.Append( siteId );
                    //sql.Append( "' and b.SITE_ID='" );
                    //sql.Append( siteId );
                    //sql.Append( "' and b.HOUSE_NUMBER='" );
                    //sql.Append( houseNumber );
                    //sql.Append( "'" );
                    string sql = String.Format(@"select a.FIRST_NAME,a.MIDDLE_INITIAL,a.LAST_NAME,a.CUSTOMER_NAME,a.HOME_AREA_CODE,
                    a.HOME_EXCHANGE_NUMBER,a.HOME_TELEPHONE_NUMBER,
                    a.BUSINESS_AREA_CODE,a.BUSINESS_EXCHANGE_NUMBER,a.BUSINESS_TELEPHONE_NUMBER,a.OTHER_AREA_CODE,a.OTHER_EXCHANGE_NUMBER,a.OTHER_PHONE,
                    b.ADDRESS_LINE_1,b.ADDRESS_LINE_2,b.ADDRESS_LINE_3,b.ADDRESS_LINE_4,ADDR_CITY,ADDR_STATE,ADDR_ZIP_4,ADDR_ZIP_5,BILL_TYPE_CODE, 
                    NVL((SELECT (CASE WHEN (COMPLAINT_TYPE IS NULL or COMPLAINT_TYPE='YES') then 0 else 1 end) 
                                    from (SELECT MAX(LAST_CHANGE_DATE), COMPLAINT_CATEGORY_CODE, COMPLAINT_SUBTOCATEGORY, 
                                    COMPLAINT_TYPE 
                                    FROM {0}_CUSTOMER_COMPLAINT 
                                    WHERE COMPLAINT_CATEGORY_CODE='AUT' 
                                        and COMPLAINT_SUBTOCATEGORY='SVC' 
                                        and ROWNUM<2 
                                        and SITE_ID = {1} 
                                        and ACCOUNT_NUMBER={2} 
                                    GROUP BY COMPLAINT_CATEGORY_CODE,COMPLAINT_SUBTOCATEGORY,COMPLAINT_TYPE ORDER BY 1 DESC)), 0) ONLINE_ORDERING_OPT_OUT,
                                    b.Franchise_number, agc.grouping_number Pricing_Group, a.customer_status_code AccountStatus
                    from {0}_CUSTOMER_MASTER a 
                    inner join {0}_HOUSE_MASTER b 
                        on a.SITE_ID=b.SITE_ID 
                            and a.house_number = b.house_number
                            and a.house_resident_number = b.house_resident_number
                    left join (select distinct cm.Site_id, nvl(decode(trim(KOSGA6), '',null,KOSGA6), RVSGA1) grouping_number, cm.account_number
                            from {0}_customer_master cm
                                inner join {0}_house_master hm
                                    on cm.house_number=hm.house_number
                                        and cm.house_resident_number=hm.HOUSE_RESIDENT_NUMBER
                                inner join icoms.{0}_CBRVREP fg
                                    on fg.RVCMPY = cm.company_number
                                        and fg.RVDIVN = cm.division_number
                                        and fg.RVFMAJ = hm.franchise_number
                                        and fg.RVSIU2 = '02'
                                left join icoms.{0}_CMKOREP pg 
                                    ON fg.RVNROV = pg.KONROV 
                                        and  KOSUJL ='02'
                                        and pg.KOCNBR = cm.account_number
                                where cm.CUSTOMER_STATUS_CODE <> 'F' and 
                                    cm.account_number = {2}
                            ) agc 
                        on a.site_id = agc.site_id
                            and a.account_number = agc.account_number
                    where a.ACCOUNT_NUMBER={2}
                        and a.SITE_ID={1}", siteCode, siteId, accountNumber9);

                    //End of changes for Self Reg - Ernest Griffin

					// build the command object
					OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn );
					cmd.CommandType=CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da=new OracleDataAdapter( cmd );
					// create the dataset to fill
					CustomerAccountSchema ds=new CustomerAccountSchema();
					// now fill it
					da.Fill( ds.CustomerAddresses );
					// all done, return
					return ds.CustomerAddresses.Count == 1 ? ds.CustomerAddresses[0] : null;
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
        /// This method returns back customer account information based on the siteId/siteCode
        /// and 9 digit accountNumber
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="siteCode"></param>
        /// <param name="accountNumber9"></param>
        /// <returns></returns>
        public CustomerAccountSchema.CustomerAddressesDataTable GetCustomerAddress(string phoneNumber10)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }
                    // build the command object
                    OracleCommand cmd = new OracleCommand("GETACCOUNTPROFILEBYPHONENBR", oracleConn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("nbrPhoneNbrIn", OracleType.Number).Value = phoneNumber10;
                    cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("vchErrorOut", OracleType.VarChar, 1).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("rfcAccountProfileOut", OracleType.Cursor).Direction = ParameterDirection.Output;
                    // build the dataadapter
                    using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                    {
                        // create the dataset to fill
                        CustomerAccountSchema ds = new CustomerAccountSchema();
                        // now fill it
                        da.Fill(ds.CustomerAddresses);
                        // all done, return
                        return ds.CustomerAddresses;
                    }
                }
            }
            catch (LogonException)
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

		/// <summary>
		/// This method returns back the customer name for a given account.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CustomerAccountSchema.CustomerName GetCustomerName(int siteId,
			string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					// create the houseNumber
					string houseNumber=accountNumber9.Substring(0,accountNumber9.Length-2);
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select nvl(a.FIRST_NAME,'') as FIRST_NAME,nvl(a.MIDDLE_INITIAL,'') as MIDDLE_INITIAL,nvl(a.LAST_NAME,'') as LAST_NAME " );
					sql.AppendFormat("from {0}_CUSTOMER_MASTER a,{0}_HOUSE_MASTER b ",siteCode);
					sql.AppendFormat("where a.SITE_ID=b.SITE_ID and a.ACCOUNT_NUMBER={0} ",accountNumber9);
					sql.AppendFormat("and a.SITE_ID={0} and b.SITE_ID={0} and b.HOUSE_NUMBER={1} ",siteId,houseNumber);
					// build the command object
					OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn );
					cmd.CommandType=CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da=new OracleDataAdapter( cmd );
					// create the dataset to fill
					CustomerAccountSchema ds=new CustomerAccountSchema();
					// now fill it
					da.Fill(ds.CustomerNames);
					// all done, return
					return ds.CustomerNames.Count == 1?ds.CustomerNames[0]:null;
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
		/// Retrieves the NSF Code for the given customer account.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public string GetNSFCode(int siteId,string siteCode,string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.AppendFormat("select DSCEDJ from {0}_DEMOGPF where DSNROV={1} and DSCNBR={2}",
						siteCode,siteId,accountNumber9);
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					// execute,check results and return.
					object ret=cmd.ExecuteScalar();
					// check results.
					return ret==null?string.Empty:ret.ToString();
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
		/// This method returns the serviceCodes associated with a particular account
		/// and not defined in the given serviceBundle.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="serviceCode"></param>
		/// <returns></returns>
		public int GetServiceCodeOccurrence(int siteId,string siteCode,
			string accountNumber9,string serviceCode)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the tableName
					string tableName=string.Format("{0}_SRVRPF a",siteCode);
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.AppendFormat("select nvl(max(AOSYP8),0) from {0} where AONROV={1} and AOCNBR={2} and AOSVCD='{3}'",
						tableName,siteId,accountNumber9,serviceCode);
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					return Convert.ToInt32(cmd.ExecuteScalar());
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
		/// Retrieves customer service code information.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CustomerAccountSchema.CustomerServiceCodesDataTable
			GetCustomerServiceCodes(int siteId,string siteCode,
			string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select SITE_ID,ACCOUNT_NUMBER,SERVICE_CATEGORY_CODE,SERVICE_OCCURRENCE,SERVICE_CODE, (SERVICE_QUANTITY + SERVICE_QUANTITY_TO_FREE) SERVICE_QUANTITY" );
					sql.AppendFormat(" from {0}_CUSTOMER_SERVICES where SITE_ID = :param1 and ACCOUNT_NUMBER = :param2 and (SERVICE_QUANTITY + SERVICE_QUANTITY_TO_FREE) > 0 AND SERVICE_STATUS = 'A' ", siteCode );
					sql.Append(" order by SERVICE_CODE,SERVICE_OCCURRENCE");
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					cmd.Parameters.AddWithValue( "param1", siteId );
					int accountNumber = 0;
					try{accountNumber = Convert.ToInt32( accountNumber9 );}
					catch{/* do not care */}
					cmd.Parameters.AddWithValue( "param2", accountNumber );
					// build the dataadapter
					OracleDataAdapter da=new OracleDataAdapter(cmd);
					// create the dataset to fill
					CustomerAccountSchema ds=new CustomerAccountSchema();
					// now fill it
					da.Fill(ds.CustomerServiceCodes);
					// all done, return
					return ds.CustomerServiceCodes;
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
		/// This method returns back the total pending payment amount for a given customer.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="statementCode"></param>
		/// <returns></returns>
		public double GetPendingPaymentAmount(int siteId,string siteCode,
			string accountNumber9,int statementCode)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select NVL(SUM(OTVANV),0) from " );
					sql.Append( siteCode );
					sql.Append( "_CBOTCPP " );
					sql.Append( "where OTSINF='1' and OTNROV=" );
					sql.Append( siteId );
					sql.Append( " and OTCNBR=" );
					sql.Append( accountNumber9 );
					sql.Append( " and OTNUO9=" );
					sql.Append(statementCode);
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					return Convert.ToDouble(cmd.ExecuteScalar());
				}
			}
			catch(LogonException)
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
		/// This method returns back the total pending payment amount for a given customer.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="statementCode"></param>
		/// <returns></returns>
		public UnappliedAmounts GetUnappliedPaymentAmount(int siteId,
			string siteCode,string accountNumber9,int statementCode)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select NVL(CWDCK$,0),NVL(CWDAJ$,0) from " );
					sql.Append( siteCode );
					sql.Append( "_CSAJDTPF " );
					sql.Append( "where CWDROV=" );
					sql.Append( siteId );
					sql.Append( " and CWDCU#=" );
					sql.Append( accountNumber9 );
					sql.Append( " and CWDUO9=" );
					sql.Append(statementCode);
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					double payment=0.0d;
					double debitAdjustment=0.0d;
					double creditAdjustment=0.0d;
					using(OracleDataReader reader=cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						// read to get first (and only) record.
						while(reader.Read())
						{
							payment+=reader.GetDouble(0);
							double tmpAmount=reader.GetDouble(1);
							if(tmpAmount>0)
							{
								debitAdjustment+=tmpAmount;
							}
							else
							{
								creditAdjustment+=Math.Abs(tmpAmount);
							}
						}
						// return a new object instance.
						return new UnappliedAmounts(payment,
							debitAdjustment,creditAdjustment);
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
		/// Retrieves customer statements.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CustomerAccountSchema.CustomerStatementsDataTable GetCustomerStatements(int siteId,
			string siteCode, string accountNumber9)
		{

            //[02-03-2009] Start Changes to move the inline query to Stored Procedure
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }

                    using (OracleCommand cmd = new OracleCommand())
                    {

                        cmd.CommandText = "GetCustomerStatements";
                        cmd.Connection = oracleConn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                        cmd.Parameters.Add("nbrAccountNbrIn", OracleType.Number).Value = Convert.ToInt32(accountNumber9);
                        cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Direction = ParameterDirection.InputOutput;
                        cmd.Parameters.Add("rfcCustomerStatements", OracleType.Cursor).Direction =
                            ParameterDirection.Output;

                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            CustomerAccountSchema ds = new CustomerAccountSchema();
                            da.TableMappings.Add("Table", "CustomerStatement");
                            da.Fill(ds);
                            return ds.CustomerStatements;
                        }
                    }

                }
            }

            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
            //[02-03-2009] End Changes to move the inline query to Stored Procedure
		}

		/// <summary>
		/// Retrieves customer statements telephone numbers.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="statementCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public TelephonySchema.StatementTelephoneNumbersDataTable GetStatementTelephoneNumbers(int siteId, string siteCode, 
			int statementCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					accountNumber9=accountNumber9.Trim();
					siteCode=siteCode.Trim();
					string statementTelephoneTable=string.Format("{0}_stmt_exp_type_01 s",siteCode);
					string statementTelephoneTable2=string.Format("{0}_stmt_exp_type_01",siteCode);
					string customerTelephoneTable=string.Format("{0}_customer_tn t",siteCode);
										
										
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append("select s.SITE_ID,s.PROCESS_CONTROL,s.ACCOUNT_NUMBER,s.STATEMENT_CODE,t.NPA_STD as AREA_CODE,t.EXCHANGE,s.PHONE_NUMBER as LINE_NUMBER");					
					sql.AppendFormat(" from {0},",statementTelephoneTable);
					sql.AppendFormat(" {0} where",customerTelephoneTable); 
					sql.AppendFormat(" s.SITE_ID={0} and s.ACCOUNT_NUMBER={1} and s.statement_code={2}",siteId,accountNumber9,statementCode);
					sql.Append(" and t.SITE_ID=s.SITE_ID and t.ACCOUNT_NUMBER=s.ACCOUNT_NUMBER");
					sql.Append(" and s.Phone_Number = t.TN_LINE_NUMBER");
					sql.AppendFormat(" and s.process_control = (select max(process_control) as process_control from {0} where account_number = {1} and statement_code = {2})",statementTelephoneTable2,accountNumber9,statementCode);
					
					// build the command object
					OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn );
					cmd.CommandType=CommandType.Text;
					// build the dataadapter
					OracleDataAdapter da=new OracleDataAdapter( cmd );
					// create the dataset to fill
					TelephonySchema ds=new TelephonySchema();
					// now fill it
					da.Fill(ds.StatementTelephoneNumbers);
					// all done, return
					return ds.StatementTelephoneNumbers;
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
		/// Gets the company and division numbers for an account.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CompanyDivisionFranchise GetCompanyDivisionFranchise(int siteId, string siteCode, string accountNumber9)
		{
			//It would have been nice just to get the company and division from the company master
			//table, but the data is not there yet.  Once that data is replicated to PHAStage
			//then we should update this query to get it directly from there.  For now, we have
			//to get the data from franchise master by joining on franchise number and site id 
			//to house master and using the house number as the filter.  
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					string accountNumber7=accountNumber9.Trim().Substring(0,7);
					siteCode=siteCode.Trim();
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append("select distinct a.company_number, a.division_number,a.franchise_number from ");
					sql.Append(siteCode);
					sql.Append("_FRANCHISE_MASTER a, ");
					sql.Append(siteCode);
					sql.Append("_HOUSE_MASTER b ");
					sql.Append("where a.FRANCHISE_NUMBER=b.FRANCHISE_NUMBER ");
					sql.Append("and a.SITE_ID=b.SITE_ID ");
					sql.Append("and b.SITE_ID=");
					sql.Append(siteId);
					sql.Append(" and b.HOUSE_NUMBER=");
					sql.Append(accountNumber7);

					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					int company=0;
					int division=0;
					int franchise=0;
					using(OracleDataReader reader=cmd.ExecuteReader(CommandBehavior.CloseConnection))
					{
						// read to get first (and only) record.
						while(reader.Read())
						{
							company=reader.GetInt32(0);
							division=reader.GetInt32(1);
							franchise=reader.GetInt32(2);
						}
						// return a new object instance.
						return new CompanyDivisionFranchise(company,division,franchise);
					}
				}
			}
			catch( DataSourceException )
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
		/// Returns true if one of the serviceCodes in the list is on an
		/// open workorder; false otherwise.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="serviceCodes"></param>
		/// <returns></returns>
		public bool AreServiceCodesOnAnOpenOrder(int siteId,string siteCode,
			string accountNumber9,string[] serviceCodes)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.AppendFormat("select count(rownum) from {0}_WORK_ORDER_DETAIL d,{0}_WORK_ORDER_MASTER m ",siteCode);
					sql.Append("where d.SITE_ID=m.SITE_ID and d.WORK_ORDER_NUMBER=m.WORK_ORDER_NUMBER and d.ACCOUNT_NUMBER=m.ACCOUNT_NUMBER ");
					sql.AppendFormat("and d.ACCOUNT_NUMBER={0} and m.ACCOUNT_NUMBER={0}",accountNumber9);
					sql.AppendFormat("and d.SITE_ID={0} and m.SITE_ID={0}",siteId);
					sql.Append("and WO_TYPE in ('DW','IN','SR','TC','UP','RS') and (m.WO_STATUS is null or m.WO_STATUS in ('',' ','IC','ND','FB','NX'))");
					for(int i=0;i<serviceCodes.Length;i++)
					{
						if(i==0)
							sql.AppendFormat("and d.SERVICE_CODE in ('{0}'",serviceCodes[i]);
						else
							sql.AppendFormat(",'{0}'",serviceCodes[i]);
					}
					// now close it
					if(serviceCodes.Length>0)sql.Append(")");
					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;
					object count=cmd.ExecuteScalar();
					return Convert.ToInt32(count)>0;
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
		/// Gets the status of the authentication credentials from the database
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public CustomerAuthenticationCredentialsStatus GetCustomerAuthenticationCredentialsStatus(int siteId,string siteCode,string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					siteCode=siteCode.Trim();
					StringBuilder sql=new StringBuilder();
					sql.Append("select SOCIAL_SECURITY_NBR, PIN_NUMBER ");
					sql.AppendFormat("from {0}_CUSTOMER_MASTER ", siteCode);
					sql.AppendFormat("where SITE_ID = {0} and ACCOUNT_NUMBER = {1} ",siteId,accountNumber9);

					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}

					bool pinExists=false;
					bool ssnExists=false;
					
					// build the command object
					using(OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn))
					{
						cmd.CommandType=CommandType.Text;					
						using(OracleDataReader reader=cmd.ExecuteReader())
						{
							// read to get first (and only) record.
							while(reader.Read())
							{
								//check if values exist for the ssn and pin fields
								ssnExists=(!reader.IsDBNull(0) && (reader.GetString(0) != string.Empty && reader.GetString(0) != " "))?true:false;
								pinExists=(!reader.IsDBNull(1) && (reader.GetString(1) != string.Empty && reader.GetString(1) != " "))?true:false;							
							}
							// return a new object instance.
							return new CustomerAuthenticationCredentialsStatus(pinExists, ssnExists);
						}
					}
				}
			}
			catch( DataSourceException )
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

		public bool IsAccountAuthenticated(int siteId,string siteCode,string accountNumber9,string securityCode)
		{			
			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append("select count(rownum) ");
					sql.AppendFormat("from {0}_CUSTOMER_MASTER ", siteCode);
					sql.AppendFormat("where SITE_ID = {0} and ACCOUNT_NUMBER = {1} ",siteId,accountNumber9);
					sql.AppendFormat("and (SOCIAL_SECURITY_NBR = '{0}' or PIN_NUMBER = '{1}' )", string.Format(__ssnMaskedFormat,securityCode), securityCode);
			
					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}
			
					// build the command object
					using( OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn))
					{
						cmd.CommandType=CommandType.Text;
						return Convert.ToInt32(cmd.ExecuteScalar())>0;
					}
				}
			}
			catch( DataSourceException )
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

        public bool GetAccountByPhoneNumber(int siteId, string accountNumber9, string phoneNumber)
        {
            try
            {

                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    string sql = "VERIFYCUSTBYPHONENBR";

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //add the params
                        cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                        cmd.Parameters.Add("nbrTel#In", OracleType.Number).Value = phoneNumber;
                        cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Value = 0;
                        cmd.Parameters["nbrErrorOut"].Direction = ParameterDirection.InputOutput;               
                        cmd.Parameters.Add("rfcCustMstrVerifyOut", OracleType.Cursor);
                        cmd.Parameters["rfcCustMstrVerifyOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("rfcCustTelVerifyOut", OracleType.Cursor);
                        cmd.Parameters["rfcCustTelVerifyOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("nbrAccountNBRIn", OracleType.Number).Value = accountNumber9;
                        
                        // open connection
                        try { oracleConn.Open(); }
                        catch (Exception ex) { throw new LogonException(ex); }

                        // build the datareader
                        using (OracleDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            return reader.HasRows;
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

		public CustomerAccountSchema.CustomerAccountsDataTable GetAccountByPhoneNumber(int siteId, string phoneNumber)
		{
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {                                        
                    string sql = "VERIFYCUSTBYPHONENBR";

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //add the params
                        cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
                        cmd.Parameters.Add("nbrTel#In", OracleType.Number).Value = phoneNumber;
                        cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Value = 0;
                        cmd.Parameters["nbrErrorOut"].Direction = ParameterDirection.InputOutput;
                        cmd.Parameters.Add("rfcCustMstrVerifyOut", OracleType.Cursor);
                        cmd.Parameters["rfcCustMstrVerifyOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("rfcCustTelVerifyOut", OracleType.Cursor);
                        cmd.Parameters["rfcCustTelVerifyOut"].Direction = ParameterDirection.Output;
                        
                        // open connection
                        try { oracleConn.Open(); }
                        catch (Exception ex) { throw new LogonException(ex); }

                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            CustomerAccountSchema ds = new CustomerAccountSchema();
                            // now fill it
                            da.Fill(ds.CustomerAccounts);
                            // all done, return
                            return ds.CustomerAccounts.Count > 0 ? ds.CustomerAccounts : null;
                        }
                    }

                }
            }
            catch (DataSourceException)
            {
                // just rethrow it
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
		}
		/// <summary>
		/// Gets the statement code for the given information
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		public string GetStatementCode(int siteId, string siteCode, 
			string accountNumber9, string phoneNumber)
		{
			try
			{
				int phoneNumberLastFourDigits = 0;
				try{phoneNumberLastFourDigits = Convert.ToInt32(phoneNumber);}
				catch{/*don't care*/}

				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql=new StringBuilder();
					sql.Append( "select STATEMENT_CODE " );
					sql.AppendFormat( "from {0}_STMT_EXP_TYPE_01 ", siteCode.Trim() );
					sql.AppendFormat("where SITE_ID = {0} ",siteId);
					sql.AppendFormat("and ACCOUNT_NUMBER = {0} and PHONE_NUMBER = {1} ", accountNumber9,phoneNumberLastFourDigits);
					sql.Append( "order by PROCESS_CONTROL desc ");

					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}
					
					// build the command object
					using (OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn ))
					{
						cmd.CommandType=CommandType.Text;

						// there might be many records that match the query.
						// we just want the one with the latest process control number.
						// the query has them ordered so that the record with the highest 
						// process control number is returned first.
						return Convert.ToString(cmd.ExecuteScalar());
					}
				}
			}
			catch( DataSourceException )
			{
				// just rethrow it
				throw;
			}
			catch( Exception ex )
			{
				// DataSourceException.
				throw new DataSourceException( ex );
			}
		}

		/// <summary>
		/// This method returns back the dwelling type based on the siteId/siteCode
		/// and 9 digit accountNumber
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public string GetDwellingType(int siteId, string siteCode, string accountNumber9)
		{
			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}

					accountNumber9 = accountNumber9.Trim();
					siteCode = siteCode.Trim();

					// create the houseNumber
					string houseNumber = accountNumber9.Substring(0, accountNumber9.Length-2);

					// create the sql statement
					StringBuilder sql = new StringBuilder();
					sql.Append( "select DWELLING_TYPE from " );
					sql.Append( siteCode );
					sql.Append( "_HOUSE_MASTER where SITE_ID='" );
					sql.Append( siteId );
					sql.Append( "' and HOUSE_NUMBER='" );
					sql.Append( houseNumber );
					sql.Append( "'" );

					// build the command object
					OracleCommand cmd=new OracleCommand(sql.ToString(),oracleConn);
					cmd.CommandType=CommandType.Text;

					return Convert.ToString(cmd.ExecuteScalar());
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
		/// This method returns a bool indicating whether the account exists in the
		/// ICOMS system
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="accountNumber9"></param>
		/// <returns></returns>
		public bool IsAccountNumberValid(int siteId, string accountNumber9)
		{
			try
			{
				int accountNumber;
				try
				{
					accountNumber = Convert.ToInt32(accountNumber9);
				}
				catch
				{
					return false;
				}

				using (OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					string sql = "AccountNumberExists";

					// open connection
					try { oracleConn.Open(); }
					catch (Exception ex) { throw new LogonException(ex); }

					// build the command object
					using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
					{
						cmd.CommandType = CommandType.StoredProcedure;

						//add the params
						cmd.Parameters.Add("nbrSIDIn", OracleType.Number).Value = siteId;
						cmd.Parameters.Add("nbrAcctNbrIN", OracleType.Number).Value = accountNumber;
						cmd.Parameters.Add("nbrAccountExistsOut", OracleType.Number);
						cmd.Parameters["nbrAccountExistsOut"].Direction = ParameterDirection.Output;

						cmd.ExecuteNonQuery();
						if (!Convert.IsDBNull(cmd.Parameters["nbrAccountExistsOut"].Value) && Convert.ToInt32(cmd.Parameters["nbrAccountExistsOut"].Value) == 1)
						{
							return true;
						}
						else
						{
							return false;
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
        public bool IsAccountValidByZipCode(int siteId, string siteCode, string accountNumber9, string zipCode)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // create the sql statement
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select count(rownum) ");                    
                    sql.AppendFormat("from {0}_CUSTOMER_MASTER a, {0}_HOUSE_MASTER b ", siteCode);
                    sql.Append("where a.SITE_ID = b.SITE_ID ");
                    sql.Append("and a.HOUSE_NUMBER = b.HOUSE_NUMBER ");
                    sql.AppendFormat("and a.SITE_ID = {0} and a.ACCOUNT_NUMBER = {1} ", siteId, accountNumber9);
                    sql.AppendFormat("and b.addr_zip_5= '{0}' ", zipCode);
                    sql.Append("and a.CUSTOMER_STATUS_CODE = 'A' ");
                    sql.Append("and a.CUSTOMER_type_code = 'C'");
                    
                    // open connection
                    try { oracleConn.Open(); }
                    catch (Exception ex) { throw new LogonException(ex); }

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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

        public CustomerAccountSchema.CustomerAddressesDataTable GetAccountByPhoneNumber(string csvSiteIdAccountNumber, bool getNeverAndFormerAsWell)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    string sql = "GETACCOUNTBYSITEANDACCNBRSINFO";

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //add the params
                        cmd.Parameters.Add("arVchSiteAndAccountNbrsIn", OracleType.VarChar, 4000).Value = csvSiteIdAccountNumber;
                        cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Value = 0;
                        cmd.Parameters["nbrErrorOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("vchErrorOut", OracleType.VarChar).Value = 0;
                        cmd.Parameters["vchErrorOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("rfcAccountProfileOut", OracleType.Cursor);
                        cmd.Parameters["rfcAccountProfileOut"].Direction = ParameterDirection.Output;

                        //Check if we need to search for former and never accounts also
                        if (getNeverAndFormerAsWell)
                            //If search for former and never account is enabled, 
                            //set account status filter value as A,F and N.
                            cmd.Parameters.Add("vchAccountStatus", OracleType.VarChar, 100).Value = "'A','F','N'";
                        else
                            //If search for former and never account is disabled,
                            //set account status filter value as A.
                            cmd.Parameters.Add("vchAccountStatus", OracleType.VarChar, 100).Value = "'A'";


                        // open connection
                        try { oracleConn.Open(); }
                        catch (Exception ex) { throw new LogonException(ex); }

                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            CustomerAccountSchema ds = new CustomerAccountSchema();

                            // now fill it
                            da.Fill(ds.CustomerAddresses);

                            // all done, return
                            return ds.CustomerAddresses.Count > 0 ? ds.CustomerAddresses : null;
                        }
                    }

                }
            }
            catch (DataSourceException)
            {
                // just rethrow it
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
        }

        public CustomerAccountSchema.CustomerAddressesDataTable GetAccountByPhoneNumber(List<string> csvSiteIdAccountNumberList, bool getNeverAndFormerAsWell)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    string sql = "WES.GETACCOUNTBYSITEANDACCNBRSINFO";

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql, oracleConn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        //add the params
                        cmd.Parameters.Add("arVchSiteAndAccountNbrsIn", OracleType.VarChar, 5200);
                        cmd.Parameters.Add("nbrErrorOut", OracleType.Number).Value = 0;
                        cmd.Parameters["nbrErrorOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("vchErrorOut", OracleType.VarChar,100).Value = 0;
                        cmd.Parameters["vchErrorOut"].Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("rfcAccountProfileOut", OracleType.Cursor);
                        cmd.Parameters["rfcAccountProfileOut"].Direction = ParameterDirection.Output;

                        //Check if we need to search for former and never accounts also
                        if (getNeverAndFormerAsWell)
                            //If search for former and never account is enabled, 
                            //set account status filter value as A,F and N.
                            cmd.Parameters.Add("vchAccountStatus", OracleType.VarChar, 100).Value = "'A','F','N'";
                        else
                            //If search for former and never account is disabled,
                            //set account status filter value as A.
                            cmd.Parameters.Add("vchAccountStatus", OracleType.VarChar, 100).Value = "'A'";

                        // open connection
                        try { oracleConn.Open(); }
                        catch (Exception ex) { throw new LogonException(ex); }

                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            CustomerAccountSchema ds = new CustomerAccountSchema();

                            foreach (String csvString in csvSiteIdAccountNumberList)
                            {
                                //set the csv string as input parameter to stored proc
                                cmd.Parameters["arVchSiteAndAccountNbrsIn"].Value = csvString;
                                // now fill it
                                da.Fill(ds.CustomerAddresses);
                            }

                           

                            // all done, return
                            return ds.CustomerAddresses.Count > 0 ? ds.CustomerAddresses : null;
                        }
                    }

                }
            }
            catch (DataSourceException)
            {
                // just rethrow it
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
        }

		#endregion public methods
	}
	/// <summary>
	/// Class containing unappliedAmounts
	/// </summary>
	public class UnappliedAmounts
	{
		#region member variables
		/// <summary>
		/// Member variable containing sum of payment amounts
		/// </summary>
		protected double _payments=0.00d;
		/// <summary>
		/// Member variable containing sum of debit adjustment amounts
		/// </summary>
		protected double _debitAdjustments=0.00d;
		/// <summary>
		/// Member variable containing sum of credit adjustment amounts
		/// </summary>
		protected double _creditAdjustments=0.00d;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public UnappliedAmounts(){}
		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="payments"></param>
		/// <param name="debitAdjustments"></param>
		/// <param name="creditAdjustments"></param>
		public UnappliedAmounts(double payments, 
			double debitAdjustments, double creditAdjustments)
		{
			_payments=payments;
			_debitAdjustments=debitAdjustments;
			_creditAdjustments=creditAdjustments;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Property containing sum of payments.
		/// </summary>
		public double Payments
		{
			get{return _payments;}
			set{_payments=value;}
		}
		/// <summary>
		/// Property containing sum of debit adjustments.
		/// </summary>
		public double DebitAdjustments
		{
			get{return _debitAdjustments;}
			set{_debitAdjustments=value;}
		}
		/// <summary>
		/// Property containing sum of debit adjustments.
		/// </summary>
		public double CreditAdjustments
		{
			get{return _creditAdjustments;}
			set{_creditAdjustments=value;}
		}
		/// <summary>
		/// Returns the net adjustments amount (e.g. Debit-Credit)
		/// </summary>
		public double NetAdjustments
		{
			get{return _debitAdjustments-_creditAdjustments;}
		}
		#endregion properties
	}

	/// <summary>
	/// Simple structure for company, division and franchise
	/// </summary>
	public struct CompanyDivisionFranchise
	{
		#region member variables
		/// <summary>The company number</summary>
		private int _company;
		/// <summary>The division number</summary>
		private int _division;
		/// <summary>The franchise number</summary>
		private int _franchise;
		#endregion member variables

		#region properties
		/// <summary>
		/// Gets or sets the company number
		/// </summary>
		public int Company
		{
			get{return _company;}
			set{_company=value;}
		}
		/// <summary>
		/// Gets or sets the division number
		/// </summary>
		public int Division
		{
			get{return _division;}
			set{_division=value;}
		}
		/// <summary>
		/// Gets or sets the franchise number
		/// </summary>
		public int Franchise
		{
			get{return _franchise;}
			set{_franchise=value;}
		}
		#endregion

		#region ctors
		/// <summary>
		/// Ctor taking params
		/// </summary>
		/// <param name="company"></param>
		/// <param name="division"></param>
		/// <param name="franchise"></param>
		public CompanyDivisionFranchise(int company,int division,int franchise)
		{
			_company=company;
			_division=division;
			_franchise=franchise;
		}
		#endregion
	}

	/// <summary>
	/// Simple structure to indicate the existance of Authentication credetials for an account.
	/// </summary>
	public struct CustomerAuthenticationCredentialsStatus
	{
		#region member variables
		/// <summary>Indicates if a pin is setup for the account</summary>
		private bool _pinExists;
		/// <summary>Indicates if a ssn is setup for the account</summary>
		private bool _ssnExists;
		#endregion member variables

		#region properties
		/// <summary>
		/// Gets or sets if the pin exists
		/// </summary>
		public bool PinExists
		{
			get{return _pinExists;}
			set{_pinExists=value;}
		}
		/// <summary>
		/// Gets or sets if the ssn exists
		/// </summary>
		public bool SsnExists
		{
			get{return _ssnExists;}
			set{_ssnExists=value;}
		}
		#endregion

		#region ctors
		/// <summary>
		/// Ctor taking params
		/// </summary>
		/// <param name="company"></param>
		/// <param name="division"></param>
		/// <param name="franchise"></param>
		public CustomerAuthenticationCredentialsStatus(bool pinExists,bool ssnExists)
		{
			_pinExists=pinExists;
			_ssnExists=ssnExists;
		}
		#endregion
	}
}
