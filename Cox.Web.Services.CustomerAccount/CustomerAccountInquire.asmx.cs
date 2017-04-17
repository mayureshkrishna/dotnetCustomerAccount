using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using Cox.Web.Services;
using Cox.Web.Exceptions;
using Cox.BusinessLogic;
using Cox.BusinessLogic.CustomerAccount;
//using Cox.BusinessObjects.ProductOffer;
using Cox.BusinessObjects.CustomerAccount;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Xml.Serialization;


namespace Cox.Web.Services.CustomerAccount
{
    [WebService( Namespace = "http://webservices.cox.com/CustomerAccount" )]
    [WebServiceBinding( ConformsTo = WsiProfiles.BasicProfile1_1 )]
    public class CustomerAccountInquire: Cox.Web.Services.SecureWebService
    {
        public CustomerAccountInquire()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod( Description = "Returns customer account by account number and site id." )]
        public CustomerAccountProfile GetCustomerAccountByAccountNumberAndSiteId( int siteId, string accountNumber9 )
        {
            try
            {
                Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire customerAccountInquire = new Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire( this.userName );
                return customerAccountInquire.GetCustomerAcount( siteId, accountNumber9 );
            }
            catch( Exception e )
            {
                ExceptionManager.Publish( e );
                throw CoxSoapException.Create( e );
            }
        }

        #region GetAccountByPhoneNumber
        /// <summary>
        /// Return the latest account address information from the specified phone number.
        /// </summary>
        /// <param name="phoneNumber10"></param>
        /// <returns></returns>
        [WebMethod(Description = "Return the latest account address information from the specified phone number.")]
        public CustomerContactInformation[] GetAccountByPhoneNumber(
            [XmlElement(IsNullable = true)] string phoneNumber10, 
            bool getNeverAndFormerAsWell, [XmlIgnore()] bool getNeverAndFormerAsWellSpecified)
        {
            try
            {
                
                Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire customerAccountInquire = new Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire(this.userName);
                return customerAccountInquire.getAccountAddressesbyPhoneNumber(phoneNumber10, getNeverAndFormerAsWell).ToArray();
            }

            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                throw CoxSoapException.Create(e);
            }
        }
        #endregion GetAccountByPhoneNumber

        //[02-02-09] Start Changes for Q-matic

        #region GetCustomerAccountByAccountNumber

        /// <summary>
        /// Returns the latest customer account information by specified 13 digit customer account number.
        /// </summary>
        /// <param name="accountNumber13"></param>
        /// <returns></returns>
        [WebMethod(Description = "Returns the latest customer account information by specified 13 digit customer account number.")]
        public CustomerAccountProfile GetCustomerAccountByAccountNumber(string accountNumber13)
        {
            try
            {
                Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire customerAccountInquire = new Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire(this.userName);
                return customerAccountInquire.GetCustomerAccountByAccountNumber(accountNumber13);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                throw CoxSoapException.Create(e);
            }
        }

        #endregion GetCustomerAccountByAccountNumber


        #region GetCustomerAccountByPhoneNbrAndStreetNbr
        /// <summary>
        /// Finds the potential customer account by specified phone number and street information.
        /// </summary>
        /// <param name="phoneNumber10"></param>
        /// <param name="streetNumber"></param>
        /// <returns></returns>
        [WebMethod(Description = "Finds the potential customer account by specified phone number and street information.")]
        public CustomerAccountProfile GetCustomerAccountByPhoneNbrAndStreetNbr(string phoneNumber10, string streetNumber)
        {
            try
            {
                Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire customerAccountInquire = new Cox.BusinessLogic.CustomerAccount.CustomerAccountInquire(this.userName);
                return customerAccountInquire.GetCustomerAccountByPhoneNbrAndStreetNbr(phoneNumber10, streetNumber);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                throw CoxSoapException.Create(e);
            }
        }

        #endregion GetCustomerAccountByPhoneNbrAndStreetNbr

        //[02-02-09] End Changes for Q-matic


    }
}
