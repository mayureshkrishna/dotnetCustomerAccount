using System;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.ApplicationBlocks.ExceptionManagement;

using Cox.BusinessLogic.Exceptions;
using Cox.BusinessObjects;
using Cox.DataAccess.Enterprise;
using Cox.DataAccess.Exceptions;
using Cox.ServiceAgent.ConnectionManager;
using Request = Cox.ServiceAgent.ConnectionManager.Request;
using Response = Cox.ServiceAgent.ConnectionManager.Response;
using Cox.Validation;

namespace Cox.BusinessLogic.ConnectionManager
{

	/// <summary>
	/// This derived class appends the functionality for accessing
	/// Connection Manager service agent into Business Logic Layer. This
	/// common functionality is accessible with the protected member function
	/// CreateProxy().
	/// </summary>
	public class BllConnectionManager : BllCustomer
	{
		#region constants
		/// <summary>
		/// In certain situations ConnectionManager returns back an errorNumber of 1...meaning
		/// Unspecified Error. When this happens there are certain situations where the text
		/// can be evaluated to determine if a more specific exception can be thrown. This
		/// string defines the situation where the campaign code specified was invalid.
		/// </summary>
		protected const string __campaignCodeInvalid="Campaign is invalid for sales date.";
		/// <summary>
		/// In certain situations ConnectionManager returns back an errorNumber of 1...meaning
		/// Unspecified Error. When this happens there are certain situations where the text
		/// can be evaluated to determine if a more specific exception can be thrown. This
		/// string defines the situation where the campaign code specified was invalid.
		/// </summary>
		protected const string __campaignInvalid="Invalid campaign.";
		/// <summary>
		/// In certain situations ConnectionManager returns back an errorNumber of 1...meaning
		/// Unspecified Error. When this happens there are certain situations where the text
		/// can be evaluated to determine if a more specific exception can be thrown. This
		/// string defines the situation where the authorization request failed because the amount
		/// already exists for the given account.
		/// </summary>
		protected const string __authorizationRequestFailed="Authorization Request Already Exists For Amount";
		/// <summary>
		/// A service code in a icoms servicebundle cannot be applied to the customer account because
		/// it would not meet the prerequisite requirements.
		/// </summary>
		protected const string __prerequisitesNotMet="requires \"force\" service.\"";
		/// <summary>
		/// A service code in a icoms servicebundle cannot be applied to the customer account because
		/// it would not meet the prerequisite requirements.
		/// </summary>
		protected const string __serviceRequirementError="Service Requirement Error. Please revisit the W/O and correct the errors";
		/// <summary>
		/// Certain combinations of service codes are not allowed. Some of these cross occurrences. For 
		/// example if you try to add different CHSI speeds on different occurrences in San Diego it will
		/// cause this error. 
		/// </summary>
		protected const string __prerequisitesError="does not meet the requirement";
        /// <summary>
        /// Error codes form the API are associated with a subfile select. For
        /// error number 1025, this can mean an invalid account number was
        /// passed or it can mean a new customer account cannot be created
        /// because an active account already exists on the house number. The
        /// string error message determines the context. In this case, we want
        /// to know the difference between a bad account number exception and
        /// unable to create the account number.
        /// </summary>
        protected const string __cannotIncrementOccupant = "Unable to Increment Occupant";
        #endregion constants

		#region Members

		/// <summary>
		/// Internal member to track Connection Manager class.
		/// </summary>
		internal ConnectionManagerProxy m_proxy = null;

		#endregion // Members

		#region Construction/Destruction

		/// <summary>
		/// The default constructor is should not be available.
		/// </summary>
		private BllConnectionManager()
			: base( null )
		{ /* None necessary */ }

		/// <summary>
		/// This is the main constructor for this component. 
		/// </summary>
		/// <param name="strUserName">
		/// The username that will be matched against CM credentials.
		/// </param>
		public BllConnectionManager( string strUserName )
			: base( strUserName )
		{
			// NOTE: Credentials are never persisted in memory. They are
			// queried on every proxy creation to Connection Manager.
		} // BllConnectionManager( string userName )

		#endregion Construction/Destruction

		#region Methods

		/// <summary>
		/// This method creates a Connection Manager proxy according to the 
		/// account number provided. The account number is used to query site
		/// infomration using the base class functionality. The internal
		/// username is also used to query the Connection Manager credentials.
		/// </summary>
		/// <param name="can">
		/// The customer account number from which site information is
		/// garnered.
		/// </param>
		protected void CreateProxy( CustomerAccountNumber can )
		{
			// get the siteid/sitecode information
			PopulateSiteInfo( can );

			// Don't keep this in exposed memory
			string strLogon, strPassword;
			PopulateCmCredentials( out strLogon, out strPassword );

			try
			{
				m_proxy = new ConnectionManagerProxy(
					SiteId, SiteCode, strLogon, strPassword );
			} // try
			catch( CmErrorException )
			{
				// Let the containing layer have a crack at this information
				throw;
			} // catch( CmErrorException excCm )
			catch( Exception e )
			{
				throw new ServiceAgentUnavailableException( e );
			} // catch( Exception e )

		} // CreateProxy()

        /// <summary>
        /// This method creates a Connection Manager proxy according to the 
        /// account number provided. The account number is used to query site
        /// information using the base class functionality. The internal
        /// username is also used to query the Connection Manager credentials.
        /// </summary>
        /// <param name="siteID">
        /// The siteID from which site information is garnered.
        /// </param>
        protected void CreateProxy( int siteID )
        {
            // get the siteid/sitecode information
            PopulateSiteInfo( siteID );

            // Don't keep this in exposed memory
            string strLogon, strPassword;
            PopulateCmCredentials( out strLogon, out strPassword );

            try
            {
                m_proxy = new ConnectionManagerProxy(
                    SiteId, SiteCode, strLogon, strPassword );
            } // try
            catch( CmErrorException )
            {
                // Let the containing layer have a crack at this information
                throw;
            } // catch( CmErrorException excCm )
            catch( Exception e )
            {
                throw new ServiceAgentUnavailableException( e );
            } // catch( Exception e )

        } // CreateProxy()

		/// <summary>
		/// This method wraps the invocation of a Connection Manager call. 
		/// An ICOMS tag is wrapped around the passed instance before the
		/// call is made. This function removes as much common functionality
		/// as possible form a CM call.
		/// </summary>
		/// <param name="obj">
		/// The child tag indicating a proper Connection Manager macro
		/// complete with attributes.
		/// </param>
		/// <returns>
		/// The output of this method is the corresponding output tag to the 
		/// input. Of course, this is effectively managed by Connection
		/// Manager. 
		/// </returns>
		protected object Invoke( object obj )
		{
			// Setup a request instance
			Request.ICOMS icomsRequest = new IcomsHelper( obj );
			// Setup and response instance holder
			Response.ICOMS icomsResponse = null;

			try
			{
				icomsResponse = m_proxy.CmMethod( (Request.ICOMS)icomsRequest );
			} // try
			catch( CmErrorException )
			{
				// Let the containing layer have a crack at this information
				throw;
			} // catch( CmErrorException excCm )
			catch( Exception exc )
			{
				throw new ServiceAgentUnavailableException( exc );
			} // catch( Exception exc )

			// NOTE: If Response.ERROR came back from Connection Manager, the 
			// proxy would already have dealt with that above.
			
			return icomsResponse.Item;

		} // Invoke( Request.ICOMS )

		#endregion // Methods

		#region Helper Functions

        /// <summary>
        /// The intent of this function is to populate the Connection Manager
        /// credentials into this class instance. This modularity allows for 
        /// simple maintenance of this functionality when the call comes to
        /// change it.
        /// </summary>
        /// <param name="strLogon">
        /// This is the username used to access Connection Manager.
        /// </param>
        /// <param name="strPassword">
        /// This is the password used to access Connection Manager.
        /// </param>
        internal void PopulateCmCredentials(out string strLogon, out string strPassword)
        {
            // get the gateway id - in this case ConnectionManager
            int gatewayId = (int)TypeDescriptor.GetConverter(
                typeof(Gateway)).ConvertTo(Gateway.ConnectionManager, typeof(int));

            try
            {
                DalUser dalUser = new DalUser();
                IcomsCredential icomsCredential = dalUser.GetIcomsCredentials(gatewayId, _siteId, _userId);

                strLogon = icomsCredential.LoginName;
                strPassword = icomsCredential.Password;

                if (strLogon.Length < 1)
                {
                    throw new RecordDoesNotExistException();
                }
            }
            catch (DataSourceException dse)
            {
                throw new DataSourceUnavailableException(dse);
            }
            catch (RecordDoesNotExistException rdnee)
            {
                throw new RecordDoesNotExistException(string.Format("CM Icoms credentials do not exist for site id '{0}' and user id '{1}'", _siteId, _userId), rdnee);
            }
            catch (Exception e)
            {
                throw new UnexpectedSystemException(e);
            }
        } // PopulateCmCredentials()

		/// <summary>
		/// Translates an outgoing Connection Manager exception into something
		/// BLL compatible.
		/// </summary>
		/// <param name="exc">
		/// The CmErrorException type thrown by the service agent.
		/// </param>
		/// <returns>
		/// The outgoing exception with the appropriate exception key for the
		/// soap detail block.
		/// </returns>
		protected Exception TranslateCmException( Exception exc )
		{
			Exception excReturn = exc;

			CmErrorException excCm = exc as CmErrorException;
			if( null != excCm )
				excReturn = TranslateCmException(
					excCm.ErrorCode, exc.Message, excCm );

			return excReturn;
		} // TranslateCmException()

		/// <summary>
		/// Translates an outgoing Connection Manager exception into something
		/// BLL compatible using the return code only.
		/// </summary>
		/// <param name="intErrorCode">
		/// The return code from a Connection Manager invocation.
		/// </param>
		/// <param name="strMessage">
		/// Optional message parameter to allow clients to change output
		/// slightly.
		/// </param>
		/// <param name="excInner">
		/// Optional exception parameter to allow the attachment of the
		/// provided exception as an inner exception to the returned value.
		/// </param>
		/// <returns>
		/// The outgoing exception with the appropriate exception key for the
		/// soap detail block.
		/// </returns>
		protected Exception TranslateCmException(
			int intErrorCode, string strMessage, Exception excInner )
		{
			// Default to whatever comes in just in case something falls
			// through inadvertently
			Exception excReturn = excInner;

			// Use the error code to determine the return type
			switch(Math.Abs(intErrorCode))
			{
				case 1:
					// an error code of 1 means that its an unspecified error. However,
					// we can evaluate the text to see if we can handle the error with
					// a more specific exception then "UnexpectedSystemException". So, we
					// look at the string and act accordingly. Notice that when looking at
					// the message text we are walking the error stack looking for a match.
					Exception inner=excInner;
					bool handled=false;
					while(inner!=null)
					{
						// check for an invalid campaign code.
						if(inner.Message.IndexOf(__campaignCodeInvalid)>=0)
						{
							excReturn = new InvalidCampaignCodeException(excInner);
							handled=true;
							break;
						}
						else if (inner.Message.IndexOf(__campaignInvalid) >= 0) 
						{
							excReturn = new InvalidCampaignCodeException(excInner);
							handled = true;
							break;
						}
							// no match, so check for an already authorized amount failure.
						else if(inner.Message.IndexOf(__authorizationRequestFailed)>=0)
						{
							excReturn = new MopAlreadyAuthorizedException(strMessage,excInner);
							handled=true;
							break;
						}
							// no match, so check for an already authorized amount failure.
						else if(inner.Message.IndexOf(__prerequisitesNotMet)>=0)
						{
							//excReturn = new PrerequisitesNotMetException(excInner);
							excReturn = new WorkOrderPrerequisitesException(excInner);
							handled=true;
							break;
						}
						else if(inner.Message.IndexOf(__serviceRequirementError)>=0)
						{
							excReturn = new WorkOrderCheckInException(excInner);
							handled=true;
							break;
						}
						else if(inner.Message.IndexOf(__prerequisitesError)>=0)
						{
							excReturn = new WorkOrderPrerequisitesException(excInner);
							handled=true;
							break;
						}
						// no match, lets grab the inner exception so we can walk the error stack.
						inner=inner.InnerException;
					}
					// we were unable to handle the exception based on the message text value
					// therefore, throw an unexpected system exception.
					if(!handled)
					{
						excReturn = new UnexpectedSystemException(strMessage,excInner);
					}
					break;
				case 503:
					excReturn = new AccountInUseException(strMessage,excInner);
					break;
				case 525: case 526:
					excReturn = new MopAuthorizationFailedException(strMessage,excInner);
					break;
				case 12: case 3101: case 3102: // these last 2 were added for Ppv
					excReturn = new InvalidAccountNumberException(strMessage,excInner);
					break;
                case 1025:
                    if( null != excInner )
                    {
                        Exception excTemp = excInner.InnerException;
                        if( ( null != excTemp ) && ( excTemp.Message.IndexOf( __cannotIncrementOccupant ) >= 0 ) )
                        {
                            excReturn = new AccountAlreadyExistsException( strMessage, excInner );
                            break;
                        } // ...(__cannotIncrementOccupant)...
                        else
                        {
                            excReturn = new InvalidAccountNumberException( strMessage, excInner );
                            break;
                        } // else
                    } // if( null != excInner )
                    else
                    {
                        excReturn = new InvalidAccountNumberException( strMessage, excInner );
                    } // else( null != excInner )
                    break;
                case 1545:
                    excReturn = new InvalidHouseNumberException( strMessage, excInner );
                    break;
                case 1655:
					excReturn = new InvalidBankRoutingNumberException(strMessage,excInner);
					break;
				case 1640:
					excReturn = new InvalidMopAccountInformationException(strMessage,excInner);
					break;
				case 1714: // added for OrderFulfillment
					excReturn = new CannotAllocateTimeSlotException(strMessage,excInner);
					break;
				case 1715: // added for Ppv
					excReturn = new InvalidPinNumberException(strMessage,excInner);
					break;
				case 1770: // added for OrderFullfillment
					excReturn = new FieldLockedException(strMessage,excInner);
					break;
				case 1780: // added for OrderFullfillment
					excReturn = new InvalidSalesReasonException(strMessage,excInner);
					break;
					/* 
				 * added for Ppv. Legacy was tracking this exception for PayPerView. Normally
				 * I do not see a need for this because it doesn't look like a Ppv Exception;
				 * however it doesn't hurt because the specific exception we are throwing does
				 * make sense for this error.
				 */
				case 1822: case 3113: 
					excReturn = new InvalidSetTopBoxIdException(strMessage,excInner);
					break;
				case 1835:
					// rate master not found. however, the rate master could not be
					// found due to the service code not being mapped to a rate master.
					// this means that the service code is probably invalid.
					excReturn = new ProductConfigurationException(strMessage,excInner);
					break;
				case 1855:
					excReturn = new InvalidSiteIdException(strMessage,excInner);
					break;
				case 1870:
					excReturn = new InvalidStatementCodeException(strMessage,excInner);
					break;
					/* 
					 * added for Ppv...again legacy was tracking it so I am. not sure it is needed
					 * for ppv but it doesn't hurt because the exception we are throwing is specific
					 * and valid for the error response code from icoms.
					 */
				case 2065: // added for OrderFullfillment
					excReturn = new InvalidWorkOrderStatusException(strMessage,excInner);
					break;
				case 3100: 
					excReturn = new CreditCheckFailedException(strMessage,excInner);
					break;
				case 3109: case 3126: // these 2 added for Ppv
					excReturn = new PpvEventRestrictedException(strMessage,excInner);
					break;
				case 3114: // added for Ppv
					excReturn = new PpvEventAlreadyOrderedException(strMessage,excInner);
					break;
				case 3121: // added for Ppv
					excReturn = new EquipmentException(strMessage,excInner);
					break;
				case 3125: // added for Ppv
					excReturn = new PinNumberRequiredException(strMessage,excInner);
					break;
				case 3122: // added for Ppv
					excReturn = new PpvExpiredException(strMessage,excInner);
					break;
				case 3133: // added for Ppv
					excReturn = new InvalidPpvEventException(strMessage,excInner);
					break;
				case 90000: case 90001: case 90002:
				case 90003: case 90004: case 90005:
				case 90006: case 90007: case 90008:
				case 90009:
					excReturn = new ServiceAgentUnavailableException(strMessage,excInner);
					break;
				default:
					excReturn = new UnexpectedSystemException(strMessage,excInner);
					break;
			} // switch( intErrorCode )

			return excReturn;
		} // TranslateCmException()

		/// <summary>
		/// Helper function that converts strings to ints without 
		/// throwing an exception. It returns 0 if it cannot convert.
		/// NOTE: This function belongs in a common conversion class.
		/// </summary>
		/// <param name="strConvert">
		/// A string value that needs to be converted.
		/// </param>
		/// <returns>
		/// An integer representation of the input parameter
		/// </returns>
		protected int toInt32( string strConvert )
		{
			int intReturn = 0;
			if( null != strConvert )
			{
				try
				{ intReturn = Convert.ToInt32( strConvert.Trim() ); }
				catch
				{ /*None necessary*/ }
			} // if( null != strConvert )
			return intReturn;
		} // toInt32()

		/// <summary>
		/// Helper function that converts strings to doubles without 
		/// throwing an exception. It returns 0 if it cannot convert.
		/// NOTE: This function belongs in a common conversion class.
		/// </summary>
		/// <param name="strConvert">
		/// A string value that needs to be converted.
		/// </param>
		/// <returns>
		/// An double representation of the input parameter
		/// </returns>
		protected double toDouble( string strConvert )
		{
			double dblReturn = 0;
			if( null != strConvert )
			{
				try
				{ dblReturn = Convert.ToDouble( strConvert.Trim() ); }
				catch
				{ /*None necessary*/ }
			} // if( null != strConvert )
			return dblReturn;
		} // toDouble()

		#endregion // Helper Functions

	} // class BllConnectionManager

} // Cox.BusinessLogic.ConnectionManager
