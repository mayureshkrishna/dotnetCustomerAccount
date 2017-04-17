using System;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
	/// <summary>
	/// Summary description for DalServiceability.
	/// </summary>
	public class DalServiceability  : DalAccountBase
	{
		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalServiceability():this(__accountConnectionKey){}
		/// <summary>
		/// The default constructor.
		/// </summary>
		public DalServiceability(string connectionKey):base(connectionKey){}
		#endregion ctors
		
		#region public methods
		/// <summary>
		/// Searches an ICOMS <siteAbbr>_All_House_Master table for a given address
		/// and returns serviceability for each address that was found
		/// </summary>
		/// <param name="siteAbbr"></param>
		/// <param name="streetNumber"></param>
		/// <param name="streetNumberFraction"></param>
		/// <param name="preDirectional"></param>
		/// <param name="streetName"></param>
		/// <param name="Apt"></param>
		/// <param name="zip5"></param>
		/// <param name="zip4"></param>
		/// <param name="productcodesDT"></param>
		/// <param name="productcodesAC"></param>
		/// <param name="productcodesHSD"></param>
		/// <param name="productcodesDC"></param>
		/// <param name="productcodesST"></param>
		/// <param name="productcodesVT"></param>
		/// <returns></returns>
		public ServiceabilitySchema.ICOMSAddressesDataTable FindAddressAndServiceabilityInICOMS(
			string siteAbbr, string streetNumber, string streetNumberFraction, string preDirectional, 
			string streetName, string Apt, string zip5, string zip4, string productcodesDT, 
			string productcodesAC, string productcodesHSD, string productcodesDC, string productcodesST, 
			string productcodesVT)
		{
			siteAbbr=siteAbbr.Trim();

			try
			{
				using(OracleConnection oracleConn=new OracleConnection(_connectionString))
				{
					// create the sql statement
					StringBuilder sql=new StringBuilder();

					sql.Append("SELECT a.SITE_ID, a.COMPANY_NUMBER, a.DIVISION_NUMBER, a.FRANCHISE_NUMBER, a.HOUSE_NUMBER, "); 
					sql.Append("	a.ADDR_LOCATION, a.FRACTION, a.PRE_DIRECTIONAL, a.STREET, a.ADDR_POST_DIRECTIONAL, a.ADDRESS_LINE_1, a.ADDRESS_LINE_2, a.ADDRESS_LINE_3, a.ADDRESS_LINE_4, a.APARTMENT, a.BUILDING, a.ADDR_CITY, a.ADDR_STATE, a.ADDR_ZIP_5, a.ADDR_ZIP_4, ");
					sql.Append("	a.DWELLING_TYPE, a.HOUSE_RESIDENT_NUMBER, "); 
					sql.Append("	CASE a.HOUSE_STATUS WHEN '2' THEN 1 ELSE 0 END AS HasCustomer, ");	
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN ({1}, {2}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasBasicCable, ", siteAbbr, productcodesAC, productcodesDC);
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN({1}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasDigitalCable, ",siteAbbr, productcodesDC);
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN({1}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasHSI, ",siteAbbr, productcodesHSD);
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN({1}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasTelephony, ", siteAbbr, productcodesDT);
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN({1}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasSwitchedTelephony, ", siteAbbr, productcodesST);
					sql.AppendFormat("	CASE WHEN (SELECT COUNT(*) FROM {0}_HOUSE_MASTER_SRV_CATEGORY b WHERE serviceable_status_code IN({1}) AND a.HOUSE_NUMBER = b.HOUSE_NUMBER) > 0 THEN 1     ELSE 0 END AS HasVoipTelephony ", siteAbbr, productcodesVT);
					sql.AppendFormat("FROM {0}_HOUSE_MASTER a ",siteAbbr);
					sql.Append("WHERE a.ADDR_LOCATION = :pStreetNumber ");
					sql.Append("	AND UPPER(a.STREET) LIKE :pStreetName ");
					sql.Append("	AND a.ADDR_ZIP_5 = :pZip5 ");
					sql.Append("	AND a.UNSERVICEABLE_ADDRESS = 'N' ");
					sql.Append("	AND a.DWELLING_TYPE IN ('A','C','H','L','N','T','U','V','W','1','2','3','4') ");

					// conditional parameters
					if(streetNumberFraction.Trim().Length > 0)
					{
						sql.Append( "AND a.FRACTION = :pStreetFraction ");
					}

					if(preDirectional.Trim().Length > 0)
					{
						sql.Append( "AND a.PRE_DIRECTIONAL LIKE :pPreDirectional ");
					}

					if(Apt.Trim().Length > 0)
					{
						sql.Append( "AND a.APARTMENT LIKE :pApt ");
					}

					if(zip4.Trim().Length > 0)
					{
						sql.Append( "AND a.ADDR_ZIP_4 = :pZip4 ");
					}					
					
					// open connection
					try{oracleConn.Open();}
					catch{throw new LogonException();}	

					// build the command object
					using (OracleCommand cmd=new OracleCommand( sql.ToString(), oracleConn ))
					{
						cmd.CommandType=CommandType.Text;
					
						// build the dataadapter
						using (OracleDataAdapter da=new OracleDataAdapter( cmd ))
						{					
							//add parameters
							string tempParamValue;

							da.SelectCommand.Parameters.Add("pStreetNumber", OracleType.VarChar).Value = streetNumber.Trim().PadLeft(6,' ');
					
							tempParamValue = streetName.ToUpper().Trim() + "%";
							da.SelectCommand.Parameters.Add("pStreetName", OracleType.VarChar).Value = tempParamValue;
					
							da.SelectCommand.Parameters.Add("pZip5", OracleType.VarChar).Value = zip5.Trim();
					
							//add conditional parameters
							if(streetNumberFraction.Trim().Length > 0)
							{
								da.SelectCommand.Parameters.Add("pStreetFraction", OracleType.VarChar).Value = streetNumberFraction.Trim();
							}
					
							if(preDirectional.Trim().Length > 0)
							{
								tempParamValue = "%" + preDirectional.Trim();
								da.SelectCommand.Parameters.Add("pPreDirectional", OracleType.VarChar).Value = tempParamValue;
							}
					
							if(Apt.Trim().Length > 0)
							{
								tempParamValue = "%" + Apt.Trim();
								da.SelectCommand.Parameters.Add("pApt", OracleType.VarChar).Value = tempParamValue;
							}
					
							if(zip4.Trim().Length > 0)
							{
								da.SelectCommand.Parameters.Add("pZip4", OracleType.VarChar).Value = zip4.Trim();
							}					
					
							// create the dataset to fill
							ServiceabilitySchema ds=new ServiceabilitySchema();
					
							// now fill it
							da.Fill( ds.ICOMSAddresses);

							// all done, return
							return ds.ICOMSAddresses;
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
		/// This method determines is a phone number is portable.  The logic uses a rate center for the site/house
		/// from [site]_house_master and then looks for the existance of record in [site]_CMAAREP table.  If a record
		/// exists, the phone number is portable
		/// </summary>
		/// <param name="siteCode"></param>
		/// <param name="houseNumber"></param>
		/// <param name="areaCode"></param>
		/// <param name="exchange"></param>
		/// <returns></returns>
		public bool IsPhoneNumberPortable(string siteCode, string houseNumber, string areaCode, string exchange)
		{
			//variable declaration
			bool returnValue = false;

			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					
					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}
					
					//create sql to look up the portability
					StringBuilder sql = new StringBuilder();
					
					sql.Append("SELECT count(rownum) ");
					sql.AppendFormat("FROM {0}_CMAAREP ", siteCode);
					sql.Append("WHERE AAAAAR = :pRateCenter ");
					sql.Append("AND AAUDNB = :pAreaCode and AAUCNB = :pExchange");
					
					// build the command object
					using( OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn))
					{
						cmd.CommandType = CommandType.Text;
						
						// build the parameters
						using( OracleDataAdapter da = new OracleDataAdapter( cmd ))
						{	
							da.SelectCommand.Parameters.Add("pRateCenter", OracleType.VarChar).Value = this.GetRateCenterId(siteCode, houseNumber);
							da.SelectCommand.Parameters.Add("pAreaCode", OracleType.VarChar).Value = areaCode.Trim();
							da.SelectCommand.Parameters.Add("pExchange", OracleType.VarChar).Value = exchange.Trim();
						
							returnValue = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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
			
			//return 
			return returnValue;
		}
		#endregion public methods

		#region private methods
		/// <summary>
		/// Looks up a rate center for a particular site and house number in the
		/// [site]_HOUSE_MASTER table
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="siteCode"></param>
		/// <param name="houseNumber"></param>
		/// <returns></returns>
		private int GetRateCenterId(string siteCode, string houseNumber)
		{
			//variable declaration
			int returnValue;

			try
			{
				using(OracleConnection oracleConn = new OracleConnection(_connectionString))
				{
					
					// open connection
					try{oracleConn.Open();}
					catch(Exception ex){throw new LogonException(ex);}
			
					//create string builder 
					StringBuilder sql = new StringBuilder();
					
					//look up rate center in house master (not sure what the importance is)
					sql.Append("SELECT DISTINCT(RATE_CENTER_ID) "); 
					sql.AppendFormat("FROM {0}_HOUSE_MASTER ", siteCode); 
					sql.Append(" WHERE HOUSE_NUMBER = :pHouseNumber"); 
		
					// build the command object
					using( OracleCommand cmd = new OracleCommand(sql.ToString(),oracleConn))
					{
						cmd.CommandType = CommandType.Text;
						
						// build the dataadapter
						using ( OracleDataAdapter da = new OracleDataAdapter( cmd ))
						{
							//add parameters
							da.SelectCommand.Parameters.Add("pHouseNumber", OracleType.VarChar).Value = houseNumber.Trim();
						
							returnValue = Convert.ToInt32(cmd.ExecuteScalar());
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

			//return 
			return returnValue;
		}

		#endregion private methods
	}
}
