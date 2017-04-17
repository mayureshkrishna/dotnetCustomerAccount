using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;

//Framework stuff
using Cox.Web.Services;
using Cox.Web.Exceptions;

//BO stuff
using Cox.BusinessObjects.CustomerAccount;

//BLL stuff
using Cox.BusinessLogic;
using Cox.BusinessLogic.Exceptions;
using Cox.BusinessLogic.CustomerAccount;

//MS App blocks
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.Web.Services.CustomerAccount
{
    /// <summary>
    /// Main webservice's class for retrieving/updating CustomerAccount information
    /// </summary>
    [WebService( Namespace = "http://webservices.cox.com/CustomerAccount" )]
    public class CustomerAccountService: Cox.Web.Services.SecureWebService
    {
        #region ctors

        /// <summary>
        /// The default constructor.
        /// </summary>
        public CustomerAccountService()
            : base()
        {
            InitializeComponent();
        } // CustomerAccountService()  (Constructor)

        #endregion // ctors

        #region Web Methods

        /// <summary>
        /// Modifies a customer account record for the given site identifier and
        /// account number in the system of record.
        /// </summary>
        /// <param name="accountNumber">The nine digit account number from the system of record.</param>
        /// <param name="siteID">The three digit site identifier from the system of record.</param>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName1">Customer first business name.</param>
        /// <param name="businessName2">Customer second business name.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="socialSecurityNumber">Customer social security number.</param>
        /// <param name="pin">Customer PIN.</param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="paymentSetting">Flag indicating can accept a check on this account.</param>
        [WebMethod( Description = "This method creates a customer account record for the given site identifier and house number in the system of record." )]
        public void ModifyCustomerAccountByAccountNumberAndSiteId(
            string accountNumber, int siteID, string dateOfBirth,
            string driverLicenseNumber, string businessName,
            string title, string firstName,
            string lastName, string middleInitial, string homeTelephone,
            string businessTelephone, string otherTelephone,
            string socialSecurityNumber, string pin, string emailAddress,
            string comments, BlockPaymentSetting paymentSetting )
        {
            try
            {
#if( !STUBS )

                CustomerAccountManager customerAccountManager = new CustomerAccountManager( this.userName, siteID );

                customerAccountManager.ModifyCustomerAccountByAccountNumberAndSiteId(
                    accountNumber, siteID, dateOfBirth, driverLicenseNumber,
                    businessName,
                    title, firstName, lastName, middleInitial,
                    homeTelephone, businessTelephone, otherTelephone,
                    socialSecurityNumber, pin, emailAddress,
                    comments, paymentSetting );

#endif
            } // try
            catch( Exception exc )
            {
                ExceptionManager.Publish( exc );
                throw CoxSoapException.Create( exc );
            } // catch( Exception exc )
        } // ModifyCustomerAccountByHouseNumberAndSiteId()

        /// <summary>
        /// Creates a customer account record for the given site identifier and
        /// house number in the system of record. This will fail if an active
        /// account currently exists on the given house.
        /// </summary>
        /// <param name="houseNumber">The seven digit house number from the system of record.</param>
        /// <param name="siteID">The three digit site identifier from the system of record.</param>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName1">Customer first business name.</param>
        /// <param name="businessName2">Customer second business name.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="socialSecurityNumber">Customer social security number.</param>
        /// <param name="pin">Customer PIN.</param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="paymentSetting">Flag indicating can accept a check on this account.</param>
        [WebMethod( Description = "This method creates a customer account record for the given site identifier and house number in the system of record." )]
        public string CreateCustomerAccountByHouseNumberAndSiteId(
            string houseNumber, int siteID, string dateOfBirth,
            string driverLicenseNumber, string businessName,
            string title, string firstName,
            string lastName, string middleInitial, string homeTelephone,
            string businessTelephone, string otherTelephone,
            string socialSecurityNumber, string pin, string emailAddress,
            string comments, BlockPaymentSetting paymentSetting )
        {
            try
            {
#if( !STUBS )

                CustomerAccountManager customerAccountManager = new CustomerAccountManager( this.userName, siteID );

                return customerAccountManager.CreateCustomerAccountByHouseNumberAndSiteId(
                    houseNumber, siteID, dateOfBirth, driverLicenseNumber,
                    businessName, title, firstName, lastName, middleInitial,
                    homeTelephone, businessTelephone, otherTelephone,
                    socialSecurityNumber, pin, emailAddress,
                    comments, paymentSetting,null );

#else
                return "000000101";
#endif
            } // try
            catch( Exception exc )
            {
                ExceptionManager.Publish( exc );
                throw CoxSoapException.Create( exc );
            } // catch( Exception exc )
        } // CreateCustomerAccountByHouseNumberAndSiteId()


        /// <summary>
        /// This method creates a new customer roommate account in the system of record against
        /// the provided account number and site id.  The other parameters are used to edit the house 
        /// details ans cable serviceability status
        /// </summary>
        /// <param name="accountNumber9">9 digit account number</param>
        /// <param name="siteId">3 digit site id</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="middleInitial">Middle initial</param>
        /// <param name="title">Title ie Mr, Ms</param>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <param name="driverLicenseNumber">Driver license number</param>
        /// <param name="socialSecurityNumber">Social security number</param>
        /// <param name="businessName">Business name</param>
        /// <param name="homeTelephone">Home telephone</param>
        /// <param name="businessTelephone">Business telephone</param>
        /// <param name="otherTelephone">Other telephone</param>
        /// <param name="pin">Pin</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="comments">Comments</param>
        /// <param name="complex">Complex</param>
        /// <param name="auditCheckCode">Audit check code</param>
        /// <param name="cableServiceStatus">Cable service status</param>
        /// <returns>The created roommate account number</returns>
        [WebMethod( Description = "This method creates a new customer roommate account in the system of record against the provided account number and site id.  The other parameters are used to edit the house details ans cable serviceability status" )]
        public string CreateRoommateAccountByAccountNumberAndSiteId( string accountNumber9
                                                                  , int siteId
                                                                  , string firstName
                                                                  , string lastName
                                                                  , string middleInitial
                                                                  , string title
                                                                  , string dateOfBirth
                                                                  , string driverLicenseNumber
                                                                  , string socialSecurityNumber
                                                                  , string businessName
                                                                  , string homeTelephone
                                                                  , string businessTelephone
                                                                  , string otherTelephone
                                                                  , string pin
                                                                  , string emailAddress
                                                                  , string comments
                                                                  , string complex
                                                                  , AuditCheckCode auditCheckCode
                                                                  , CableServiceStatus cableServiceStatus )
        {
            //account number to be returned
            string accountNumber = string.Empty;
            try
            {
#if( !STUBS )
                CustomerAccountManager customerAccountManager = new CustomerAccountManager( this.userName, siteId );

                accountNumber = customerAccountManager.CreateRoommateAccountByAccountNumberAndSiteId( accountNumber9
                                                                                     , siteId
                                                                                     , firstName
                                                                                     , lastName
                                                                                     , middleInitial
                                                                                     , title
                                                                                     , dateOfBirth
                                                                                     , driverLicenseNumber
                                                                                     , socialSecurityNumber
                                                                                     , businessName
                                                                                     , homeTelephone
                                                                                     , businessTelephone
                                                                                     , otherTelephone
                                                                                     , pin
                                                                                     , emailAddress
                                                                                     , comments
                                                                                     , complex
                                                                                     , auditCheckCode
                                                                                     , cableServiceStatus );
#endif

            }//try
            catch( Exception exc )
            {
                ExceptionManager.Publish( exc );
                throw CoxSoapException.Create( exc );
            } // catch( Exception exc )

            return accountNumber;

        }//CreateRoommateAccountByAccountNumberAndSiteId()

        #endregion // Web Methods

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing && components != null )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        } // Dispose()

        #endregion // Component Designer generated code

    } // class CustomerAccountService 

} // namespace Cox.Web.Services.CustomerAccount

