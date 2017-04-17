using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using System.Collections.Specialized;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Account;
using Cox.DataAccess.Exceptions;


namespace Cox.DataAccess.House
{

    #region HouseStatus ENUMERATION
    
    /// <summary>
    /// House Status
    /// </summary>
    public enum HouseStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Never
        /// </summary>
        Never = 1,
        /// <summary>
        /// Active
        /// </summary>
        Active = 2,
        /// <summary>
        /// Former
        /// </summary>
        Former = 3
    } // enum HouseStatus

    #endregion

    /// <summary>
    /// Provides the Data Access Layer for the House
    /// </summary>
    public class DalHouse : Dal
    {
        #region constants

        /// <summary>The connectionKey into the connections configuration section of the config block.</summary>
        protected const string __connectionKey="houseConnectionString";

        #endregion constants

        #region ctors

        /// <summary>
        /// Default constructor. This constructor retreives its 
        /// configuration information from the configuration block.
        /// </summary>
        public DalHouse() : base( __connectionKey )
        { /* None necessary */ }

        #endregion ctors

        #region public methods

        /// <summary>
        /// Gets the address given the house number and site id
        /// </summary>
        /// <param name="siteId">3-digit site id for the customer account that uniquely identifies a geographic location across CCI</param>
        /// <param name="houseNumber">7-digit house number</param>
        /// <returns>Address type containing the address details</returns>
        public Address GetAddressBySiteIdAndHouseNumber( int siteId, string houseNumber )
        {
            Address address = null;

            try
            {        
                using( OracleConnection oracleConn = new OracleConnection( _connectionString ) )
                {
                    // open connection
                    try{ oracleConn.Open(); }
                    catch{ throw new LogonException(); }

                    //stored proc command          
                    OracleCommand command = new OracleCommand( "GETHOUSEADDRESS", oracleConn );
                    command.CommandType = CommandType.StoredProcedure;

                    //Site Id
                    command.Parameters.Add( new OracleParameter( "nbrSIDIn", OracleType.Number ) );
                    command.Parameters["nbrSIDIn"].Value = 
                        siteId.Equals( string.Empty ) ? Convert.DBNull : siteId;

                    //houseNumber
                    command.Parameters.Add( new OracleParameter( "nbrHouseNbrIn", OracleType.Number ) );
                    command.Parameters["nbrHouseNbrIn"].Value = 
                        houseNumber.Equals( string.Empty ) ? Convert.DBNull : houseNumber;

                    //error
                    command.Parameters.Add( new OracleParameter( "nbrErrorOut", OracleType.Number ) );
                    command.Parameters["nbrErrorOut"].Direction = ParameterDirection.InputOutput;

                    //house address out
                    command.Parameters.Add("rfcHouseAddressOut", OracleType.Cursor).Direction = ParameterDirection.Output;
              
                    //Use a datareader to get the data.
                    using (OracleDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    { 

                        //read data
                        if( reader.HasRows )
                        {
                            reader.Read();

                            // Instantiate an Address object
                            address = new Address( string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty );

                            // Populate the address fields
                            address.Location = ( reader[ "addr_location" ] != System.DBNull.Value ) ? reader[ "addr_location" ].ToString() : string.Empty;
                            address.Fraction = ( reader[ "fraction" ] != System.DBNull.Value ) ? reader[ "fraction" ].ToString() : string.Empty;
                            address.Apartment = ( reader[ "apartment" ] != System.DBNull.Value ) ? reader[ "apartment" ].ToString() : string.Empty;
                            address.PreDirectional = ( reader[ "pre_directional" ] != System.DBNull.Value ) ? reader[ "pre_directional" ].ToString() : string.Empty;
                            address.Street = ( reader[ "pre_directional" ] != System.DBNull.Value ) ? reader[ "street" ].ToString() : string.Empty;
                            address.PostDirectional = ( reader[ "addr_post_directional" ] != System.DBNull.Value ) ? reader[ "addr_post_directional" ].ToString() : string.Empty;
                            address.City = ( reader[ "addr_city" ] != System.DBNull.Value ) ? reader[ "addr_city" ].ToString() : string.Empty;
                            address.State = ( reader[ "addr_state" ] != System.DBNull.Value ) ? reader[ "addr_state" ].ToString() : string.Empty;
                            address.Zip5 = ( reader[ "addr_zip_5" ] != System.DBNull.Value ) ? reader[ "addr_zip_5" ].ToString() : string.Empty;
                            address.Zip4 = ( reader[ "addr_zip_4" ] != System.DBNull.Value ) ? reader[ "addr_zip_4" ].ToString() : string.Empty;

                            return address;
                        } // if( reader.HasRows )

                    } // using( OracleDataReader reader... )
                } // using( OracleConnection oracleConn... )
            } // try
            catch( LogonException )
            {
                // just rethrow it. it is from our internal code block
                throw;
            } // catch( LogonException )
            catch( Exception ex )
            {
                // Translate everything else into a daa source exception...
                // given that the only operations in this block to fail would be
                // data related, this is okay. However, dealing with the "data
                // not found" case is vital to ensuring that erroneous errors
                // are not thrown.
                throw new DataSourceException( ex );
            } // catch( Exception ex )

            return address;
        } // GetAddressBySiteIdAndHouseNumber(...)

        /// <summary>
        /// Gets the list of inactive house records given the house address
        /// parts. The purpose here is to find all the houses that may have been
        /// orphaned by managing roommate accounts.
        /// </summary>
        /// <param name="siteId">3-digit site id for the customer account that uniquely identifies a geographic location across CCI</param>
        /// <param name="address">Address parts structure</param>
        /// <returns>A list of house records matching the incoming address</returns>
        public string[] GetOrphanedHouseListBySiteIdAndHouseAddress( int siteId, Address address )
        {
            try
            {
                using( OracleConnection oracleConn = new OracleConnection( _connectionString ) )
                {
                    // open connection
                    try{ oracleConn.Open(); }
                    catch{ throw new LogonException(); }

                    //stored proc command          
                    OracleCommand command = new OracleCommand( "CRM.GetInactiveHouse", oracleConn );
                    command.CommandType = CommandType.StoredProcedure;

                    //Site Id
                    command.Parameters.Add( new OracleParameter( "nbrSIDIn", OracleType.Number ) );
                    command.Parameters[ "nbrSIDIn" ].Value = siteId;

                    //address.Zip5
                    command.Parameters.Add( new OracleParameter( "vchAddrZip5In", OracleType.VarChar ) );
                    command.Parameters[ "vchAddrZip5In" ].Value = address.Zip5;

                    //address.State
                    command.Parameters.Add( new OracleParameter( "vchAddrStateIn", OracleType.VarChar ) );
                    command.Parameters[ "vchAddrStateIn" ].Value = address.State;

                    //address.City
                    command.Parameters.Add( new OracleParameter( "vchAddrCityIn", OracleType.VarChar ) );
                    command.Parameters[ "vchAddrCityIn" ].Value = address.City;

                    //address.Street
                    command.Parameters.Add( new OracleParameter( "vchStreetIn", OracleType.VarChar ) );
                    command.Parameters[ "vchStreetIn" ].Value = address.Street;

                    //address.Location
                    command.Parameters.Add( new OracleParameter( "vchAddrLocationIn", OracleType.VarChar ) );
                    command.Parameters[ "vchAddrLocationIn" ].Value = address.Location;

                    //address.Apartment
                    command.Parameters.Add( new OracleParameter( "vchApartmentIn", OracleType.VarChar ) );
                    command.Parameters[ "vchApartmentIn" ].Value = address.Apartment;

                    //building          
                    command.Parameters.Add( new OracleParameter( "vchBuildingIn", OracleType.VarChar ) );
                    command.Parameters[ "vchBuildingIn" ].Value = "";

                    //address.Fraction
                    command.Parameters.Add( new OracleParameter( "vchFractionIn", OracleType.VarChar ) );
                    command.Parameters[ "vchFractionIn" ].Value = address.Fraction;

                    //address.PreDirectional
                    command.Parameters.Add( new OracleParameter( "vchPreDirectionalIn", OracleType.VarChar ) );
                    command.Parameters[ "vchPreDirectionalIn" ].Value = address.PreDirectional;

                    //address.PostDirectional
                    command.Parameters.Add( new OracleParameter( "vchAddrPostDirectionalIn", OracleType.VarChar ) );
                    command.Parameters[ "vchAddrPostDirectionalIn" ].Value = address.PostDirectional;

                    //error
                    command.Parameters.Add( new OracleParameter( "nbrErrorOut", OracleType.Number ) );
                    command.Parameters[ "nbrErrorOut" ].Direction = ParameterDirection.InputOutput;

                    //house info out
                    //command.Parameters.Add("rfcHouseInfoOut", OracleType.Cursor).Direction = ParameterDirection.Output;
                    OracleParameter prmOutput = command.Parameters.Add( "rfcHouseInfoOut", OracleType.Cursor );
                    prmOutput.Direction = ParameterDirection.Output;

                    //Use a datareader to get the data.
                    using( OracleDataReader reader = command.ExecuteReader( CommandBehavior.CloseConnection ) )
                    {
                        ArrayList orphanedHouses = new ArrayList();

                        //read data
                        while( reader.Read() )
                        {
                            orphanedHouses.Add(reader["house_number"].ToString());
                        } // while( reader.Read() )

                        return (string[])orphanedHouses.ToArray(typeof(string));
                    } // using( OracleDataReader reader... )
                } // using( OracleConnection oracleConn... ) 
            } // try
            catch( LogonException )
            {
                // just rethrow it. it is from our internal code block
                throw;
            } // catch( LogonException )
            catch( Exception ex )
            {
                // Translate everything else into a daa source exception...
                // given that the only operations in this block to fail would be
                // data related, this is okay. However, dealing with the "data
                // not found" case is vital to ensuring that erroneous errors
                // are not thrown.
                throw new DataSourceException( ex );
            } // catch( Exception ex )
        } // GetOrphanedHouseListBySiteIdAndHouseAddress(...)

        /// <summary>
        /// This method pulls service category and Serviceable status code of house by using house number and Site Id
        /// </summary>
        /// <param name="houserNumber">House Number</param>
        /// <param name="siteId">Site id</param>
        /// <returns>List object of HouseMasterServiceCategory type.</returns>
        public List<HouseMasterServiceCategory> GetHouseSrvCategoryBySideIdAndHouseNumber(int houserNumber, int siteId)
        {
            List<HouseMasterServiceCategory> houseSrvCategoryList = new List<HouseMasterServiceCategory>();

            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }

                    //stored proc command          
                    OracleCommand command = new OracleCommand("CRM.GETHOUSESRVCATEGORYANDSTSCODE", oracleConn);
                    command.CommandType = CommandType.StoredProcedure;

                    //House Number
                    command.Parameters.Add(new OracleParameter("nbrHOUSENBRIN", OracleType.Number));
                    command.Parameters["NBRHOUSENBRIN"].Value = houserNumber;

                    //Site Id
                    command.Parameters.Add(new OracleParameter("nbrSIDIn", OracleType.Number));
                    command.Parameters["nbrSIDIn"].Value = siteId;

                    //error
                    command.Parameters.Add(new OracleParameter("nbrErrorOut", OracleType.Number));
                    command.Parameters["nbrErrorOut"].Direction = ParameterDirection.InputOutput;

                    //house Serviceable data out
                    OracleParameter prmOutput = command.Parameters.Add("RFCHOUSECATEGORYOUT", OracleType.Cursor);
                    prmOutput.Direction = ParameterDirection.Output;

                    //Use a datareader to get the data.
                    using (OracleDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        

                        //read data
                        while (reader.Read())
                        {
                            HouseMasterServiceCategory houseSrvCategory = new HouseMasterServiceCategory();
                            houseSrvCategory.SiteId = Convert.ToInt32(reader["Site_ID"]);
                            houseSrvCategory.HouseNumber = Convert.ToInt32(reader["House_number"]);
                            houseSrvCategory.ServiceCategoryCode =Convert.ToString( reader["Service_Category_Code"]);
                            houseSrvCategory.ServiceableStatusCode = Convert.ToString(reader["Serviceable_Status_Code"]);

                            houseSrvCategoryList.Add(houseSrvCategory);

                        } // while( reader.Read() )

                        return houseSrvCategoryList;
                    } // using( OracleDataReader reader... )
                } // using( OracleConnection oracleConn... ) 
            } // try
            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            } // catch( LogonException )
            catch (Exception ex)
            {
                // Translate everything else into a daa source exception...
                // given that the only operations in this block to fail would be
                // data related, this is okay. However, dealing with the "data
                // not found" case is vital to ensuring that erroneous errors
                // are not thrown.
                throw new DataSourceException(ex);
            } // catch( Exception ex )
            
        }
        #endregion //public methods

    } // class DalHouse 

} // namespace Cox.DataAccess.House

