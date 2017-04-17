using System;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

using Cox.ActivityLog;
using Cox.ActivityLog.CustomerAccount;
using Cox.BusinessLogic;
using Cox.BusinessLogic.ConnectionManager;
using Cox.BusinessLogic.Exceptions;
using Cox.BusinessLogic.Validation;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerAccount;
using Cox.DataAccess;
using Cox.DataAccess.Enterprise;
using Cox.DataAccess.Exceptions;
using Cox.DataAccess.House;
using Cox.ServiceAgent.ConnectionManager;
using Request = Cox.ServiceAgent.ConnectionManager.Request;
using Response = Cox.ServiceAgent.ConnectionManager.Response;
using Cox.Validation;
using System.Collections.Generic;
using System.Configuration;
using System.Text;


namespace Cox.BusinessLogic.CustomerAccount
{
    public class CustomerAccountManager : BllConnectionManager
    {
        #region constants

        protected const string TASKCODE_GENERIC_CUSTOMER = "CUSTOMER";
        protected const string TASKCODE_ADD_CUSTOMER = "ADDCUST";
        protected const string TASKCODE_WORK_WITH_HOUSES = "HOUSE";
        protected const int FUNCTION_ACTION_UPDATE = 2;
        protected const int FUNCTION_ACTION_INSERT = 1;
        private const string _ssnRegularExpression = @"^(\d{9})?$";
        private const string _pinRegularExpression = @"^(\d{4})?$";
        private const string _dobRegularExpression = @"^((([0]?[1-9]|1[0-2])/([0-2]?[0-9]|3[0-1])/[1-2]\d{3})?)?$";
        private const RegexOptions _options = RegexOptions.None;
        private const int _ssnSize = 9;
        private const int _pinSize = 4;
        private const int _mindriverLicenseSize = 0;
        private const int _maxdriverLicenseSize = 25;
        private const string _validationErrorMessage = "The following validation errors occurred:   ";
        private const string _CPNIErrorMessage = "{0} contains an invalid value. It must be between '{1}' and '{2}'.";
        private const string _dobErrorMessage = "{0} contains an invalid value.It must be MM/DD/YYYY format";

        #endregion // constants

        #region ctors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CustomerAccountManager(
            [RequiredItem()]string userName,
            [RequiredItem()][SiteId()]int siteID)
            : base(userName)
        {
            // validate the parameters.
            ConstructorValidator validator = new ConstructorValidator(
                ConstructorInfo.GetCurrentMethod(), userName, siteID);
            validator.Validate();

            // Initialize the service agent
            CreateProxy(siteID);
        } // CustomerAccountManager()  (Constructor)

        #endregion // ctors

        #region ModifyCustomerAccountByHouseNumberAndSiteId

        /// <summary>
        /// Modifies a customer account record for the given site identifier and
        /// account number in the system of record.
        /// </summary>
        /// <param name="accountNumber9">The nine digit account number from the system of record.</param>
        /// <param name="siteID">The three digit site identifier from the system of record.</param>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName">Customer business name.</param>
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
        /// <param name="blockPaymentFlag">Flag indicating can accept a check on this account.</param>
        public void ModifyCustomerAccountByAccountNumberAndSiteId(
            [RequiredItem()][RegEx(@"^(\d{7}(?<=\d*?[1-9]{1}\d*?)(?=\d*?[1-9]{1}\d*?)\d{2})?$", RegexOptions.None)]
            string accountNumber9,
            [RequiredItem()][SiteId()]int siteID,
            [RegEx(@"^((([0]?[1-9]|1[0-2])/([0-2]?[0-9]|3[0-1])/[1-2]\d{3})?)?$", RegexOptions.None)]
            string dateOfBirth,
            [StringLength(0, 25)]string driverLicenseNumber,
            [StringLength(0, 32)]string businessName,
            [StringLength(0, 4)]string title,
            [StringLength(0, 10)]string firstName,
            [StringLength(0, 15)]string lastName,
            [StringLength(0, 1)]string middleInitial,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string homeTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string businessTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string otherTelephone,
            [RegEx(@"^(\d{9})?$", RegexOptions.None)]string socialSecurityNumber,
            [RegEx(@"^(\d{4})?$", RegexOptions.None)]string pin,
            [RegEx(@"^(\w+([-+.]\w+)*@(?!.*?@.*?)\w+([-.]\w+)*\.\w+([-.]\w+)*)?$", RegexOptions.None)]
            string emailAddress,
            [StringLength(0, 30)]string comments,
            BlockPaymentSetting blockPaymentFlag)
        {
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(
                eCustomerAccountActivityType.ModifyInformation, siteID);
            using (Log log = CreateLog(logEntry))
            {
                try
                {

                    if (dateOfBirth.Length > 0 && !new Regex(_dobRegularExpression, _options).IsMatch(dateOfBirth))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_dobErrorMessage, "dateOfBirth"));
                    }
                    // Method validator can not mask CPNI in message, due to this reason we have implemented this validation before Method Validation
                    if (socialSecurityNumber.Length > 0 && !new Regex(_ssnRegularExpression, _options).IsMatch(socialSecurityNumber))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "socialSecurityNumber", _ssnSize, _ssnSize));
                    }
                    if (pin.Length > 0 && !new Regex(_pinRegularExpression, _options).IsMatch(pin))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "pin", _pinSize, _pinSize));
                    }
                    if (!(driverLicenseNumber.Length >= _mindriverLicenseSize && driverLicenseNumber.Length <= _maxdriverLicenseSize))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "driverLicenseNumber", _mindriverLicenseSize, _maxdriverLicenseSize));
                    }
                    // perform validation
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(),
                        accountNumber9, siteID, dateOfBirth,
                        driverLicenseNumber, businessName,
                        title, firstName, lastName, middleInitial,
                        homeTelephone, businessTelephone, otherTelephone,
                        socialSecurityNumber, pin,
                        emailAddress, comments);
                    validator.Validate();

                    // Validate parameters where the validation framework does
                    // not handle
                    ValidateExtra(dateOfBirth);

                    // Validate parameters where the validation framework does
                    // not handle
                    ValidateExtra(socialSecurityNumber, driverLicenseNumber, dateOfBirth);

                    // Populate log information as you would expect
                    logEntry.CustomerAccountNumber = accountNumber9;
                    logEntry.Note = CreateLogNote(dateOfBirth,
                        driverLicenseNumber, businessName,
                        title, firstName, lastName, middleInitial,
                        homeTelephone, businessTelephone, otherTelephone,
                        socialSecurityNumber, pin,
                        emailAddress, comments, blockPaymentFlag);

                    // Set the siteid/sitecode information
                    PopulateSiteInfo(siteID);
                    logEntry.SiteId = this.SiteId;

                    // Create necessary inlines for this macro invocation
                    Request.INL00170 inl170req = CreateINL00170(
                        accountNumber9, dateOfBirth, driverLicenseNumber,
                        blockPaymentFlag);
                    Request.INL00193 inl193req = CreateINL00193(
                        accountNumber9, title, firstName, middleInitial,
                        lastName, businessName, emailAddress, comments,
                        homeTelephone, businessTelephone, otherTelephone,
                        pin, socialSecurityNumber);

                    // Invoke the customer information macro
                    Mac00054Helper mac54req = new Mac00054Helper(
                        TASKCODE_GENERIC_CUSTOMER,
                        this.SiteId, inl170req, inl193req);
                    this.Invoke(mac54req.Element);
                } // try
                catch (ValidationException ve)
                {
                    logEntry.SetError(ve.Message);
                    throw;
                }
                catch (DataSourceException dse)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(dse.Message);
                    throw new DataSourceUnavailableException(dse);
                }
                catch (CmErrorException excCm)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(excCm.Message, excCm.ErrorCode);
                    throw TranslateCmException(excCm);
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(blle.Message);
                    throw;
                }
                catch (Exception e)
                {
                    // Log and flag as unexpected
                    logEntry.SetError(e.Message);
                    throw new UnexpectedSystemException(e);
                }
            } // using( Log log = CreateLog( logEntry ) )

        } // ModifyCustomerAccountByHouseNumberAndSiteId( ... )

        #endregion // ModifyCustomerAccountByHouseNumberAndSiteId

        #region CreateCustomerAccountByHouseNumberAndSiteId

        /// <summary>
        /// Creates a customer account record for the given site identifier and
        /// house number in the system of record. This will fail if an active
        /// account currently exists on the given house.
        /// </summary>
        /// <param name="houseNumber">The seven digit house number from the system of record.</param>
        /// <param name="siteID">The three digit site identifier from the system of record.</param>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName">Customer business name.</param>
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
        /// <param name="blockPaymentFlag">Flag indicating can accept a check on this account.</param>
        /// <returns>The resulting new customer account number.</returns>
        public string CreateCustomerAccountByHouseNumberAndSiteId(
            [RequiredItem()][RegEx(@"^(\d{7}(?<=\d*?[1-9]{1}\d*?))?$", RegexOptions.None)]
            string houseNumber,
            [RequiredItem()][SiteId()]int siteID,
            [RegEx(@"^((([0]?[1-9]|1[0-2])/([0-2]?[0-9]|3[0-1])/[1-2]\d{3})?)?$", RegexOptions.None)]
            string dateOfBirth,
            [StringLength(0, 25)]string driverLicenseNumber,
            [StringLength(0, 32)]string businessName,
            [StringLength(0, 4)]string title,
            [RequiredItem()][StringLength(0, 10)]string firstName,
            [RequiredItem()][StringLength(0, 15)]string lastName,
            [StringLength(0, 1)]string middleInitial,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string homeTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string businessTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string otherTelephone,
            [RegEx(@"^(\d{9})?$", RegexOptions.None)]
            string socialSecurityNumber,
            [RegEx(@"^(\d{4})?$", RegexOptions.None)]string pin,
            [RegEx(@"^(\w+([-+.]\w+)*@(?!.*?@.*?)\w+([-.]\w+)*\.\w+([-.]\w+)*)?$", RegexOptions.None)]
            string emailAddress,
            [StringLength(0, 30)]string comments,
            BlockPaymentSetting blockPaymentFlag,
            string oldAccountNumber9)
        {
            string strAccountNumber9 = null;

            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(
                eCustomerAccountActivityType.CreateAccount, siteID);
            using (Log log = CreateLog(logEntry))
            {
                try
                {
                    if (dateOfBirth.Length > 0 && !new Regex(_dobRegularExpression, _options).IsMatch(dateOfBirth))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_dobErrorMessage, "dateOfBirth"));
                    }
                    // Method validator can not mask CPNI in message, due to this reason we have implemented this validation before Method Validation
                    if (socialSecurityNumber.Length > 0 && !new Regex(_ssnRegularExpression, _options).IsMatch(socialSecurityNumber))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "socialSecurityNumber", _ssnSize, _ssnSize));
                    }
                    if (pin.Length > 0 && !new Regex(_pinRegularExpression, _options).IsMatch(pin))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "pin", _pinSize, _pinSize));
                    }
                    if (!(driverLicenseNumber.Length >= _mindriverLicenseSize && driverLicenseNumber.Length <= _maxdriverLicenseSize))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "driverLicenseNumber", _mindriverLicenseSize, _maxdriverLicenseSize));
                    }

                    // perform validation
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(),
                        houseNumber, siteID, dateOfBirth,
                        driverLicenseNumber, businessName,
                        title, firstName, lastName, middleInitial,
                        homeTelephone, businessTelephone, otherTelephone,
                        socialSecurityNumber, pin,
                        emailAddress, comments);
                    validator.Validate();

                    // Validate parameters where the validation framework does
                    // not handle
                    ValidateExtra(dateOfBirth);

                    // Validate parameters where the validation framework does
                    // not handle
                    ValidateExtra(socialSecurityNumber, driverLicenseNumber, dateOfBirth);

                    // Populate log information as you would expect
                    logEntry.CustomerAccountNumber = houseNumber;
                    logEntry.Note = CreateLogNote(dateOfBirth,
                        driverLicenseNumber, businessName,
                        title, firstName, lastName, middleInitial,
                        homeTelephone, businessTelephone, otherTelephone,
                        socialSecurityNumber, pin,
                        emailAddress, comments, blockPaymentFlag);

                    // Set the siteid/sitecode information
                    PopulateSiteInfo(siteID);
                    logEntry.SiteId = this.SiteId;

                    Request.INL00192 inl192req = CreateINL00192(
                        houseNumber, title, firstName, middleInitial,
                        lastName, businessName, emailAddress, comments,
                        homeTelephone, businessTelephone, otherTelephone,
                        pin, socialSecurityNumber);

                    // Invoke the customer information macro
                    Mac00054Helper mac54req = new Mac00054Helper(
                        TASKCODE_ADD_CUSTOMER, this.SiteId, inl192req);
                    Response.MAC00054 mac54resp = (Response.MAC00054)this.Invoke(mac54req.Element);

                    // Parse to get account number!!!
                    strAccountNumber9 = getAccountNumberFromINL00192Response(mac54resp);

                    // Create necessary inlines for this macro invocation
                    Request.INL00170 inl170req = CreateINL00170(
                        strAccountNumber9, dateOfBirth, driverLicenseNumber,
                        blockPaymentFlag);

                    // Build and invoke the customer information macro again to
                    // change demographics information
                    mac54req = new Mac00054Helper(
                        TASKCODE_ADD_CUSTOMER, this.SiteId, inl170req);
                    this.Invoke(mac54req.Element);

                    //Begin Nov 2012 changes to save the Parent/Roommate Account Numbers to the associated accounts
                    #region Add Parent/Roommate Account Comments

                    //Check to see if the account creation was called with the previous account number
                    //if an old account number was passed in, then the new account is a roommate of the old
                    if (oldAccountNumber9 != null)
                    {
                        // Get Today's date to save to the comments
                        string creationDate = DateTime.Today.ToString().Substring(0, 10);


                        //Create the comment string containing the account number and date
                        string addComment = string.Format("PARENT:{0}({1})", oldAccountNumber9, creationDate);

                        // build the inline command for updating the roommate account
                        Request.INL00193 inl193req = CreateINL00193(strAccountNumber9, null, null, null, null, null, null, addComment, null, null, null, null, null);

                        // Build and invoke the macro to update the roommate account with comment
                        mac54req = new Mac00054Helper(
                            TASKCODE_ADD_CUSTOMER, this.SiteId, inl193req);
                        this.Invoke(mac54req.Element);


                        //Create the comment string containing the account number and date
                        addComment = string.Format("ROOMMATE:{0}({1})", strAccountNumber9, creationDate);

                        // build the inline command for updating the parent account
                        inl193req = CreateINL00193(oldAccountNumber9, null, null, null, null, null, null, addComment, null, null, null, null, null);

                        // Build and invoke the macro to update the parent account with comment
                        mac54req = new Mac00054Helper(
                            TASKCODE_ADD_CUSTOMER, this.SiteId, inl193req);
                        this.Invoke(mac54req.Element);
                    }
                    #endregion
                    //End changes for Parent/Roommate comment

                } // try
                catch (ValidationException ve)
                {
                    logEntry.SetError(ve.Message);
                    throw;
                }
                catch (DataSourceException dse)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(dse.Message);
                    throw new DataSourceUnavailableException(dse);
                }
                catch (CmErrorException excCm)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(excCm.Message, excCm.ErrorCode);
                    throw TranslateCmException(excCm);
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(blle.Message);
                    throw;
                }
                catch (Exception e)
                {
                    // Log and flag as unexpected
                    logEntry.SetError(e.Message);
                    throw new UnexpectedSystemException(e);
                }
            } // using( Log log = CreateLog( logEntry ) )

            return strAccountNumber9;
        } // CreateCustomerAccountByHouseNumberAndSiteId()

        #endregion // CreateCustomerAccountByHouseNumberAndSiteId

        #region CreateRoommateAccountByAccountNumberAndSiteId
        /// <summary>
        /// This method creates a new customer roommate account in the system of record against
        /// the provided account number and site id.  The other parameters are used to edit the house 
        /// details ans cable serviceability status
        /// </summary>
        /// <param name="accountNumber9">The 9-digit account identifier for the customer account where the roommate will be created</param>
        /// <param name="siteID">the 3-digit site id for the customer</param>        
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="middleInitial">Middle initial</param>
        /// <param name="title">Title (e.g. Mr., Mrs., Dr.)</param>
        /// <param name="dateOfBirth">Date of birth</param>
        /// <param name="driverLicenseNumber">Driver's license number</param>
        /// <param name="socialSecurityNumber">Social security number</param>
        /// <param name="businessName">Business name</param>
        /// <param name="homeTelephone">Home telephone number</param>
        /// <param name="businessTelephone">Business telephone number</param>
        /// <param name="otherTelephone">Other telephone number</param>
        /// <param name="pin">Pin</param>
        /// <param name="emailAddress">Email address</param>
        /// <param name="comments">Customer comments</param>
        /// <param name="complex">Complex</param>
        /// <param name="auditCheckCode">Indicates the audit check code</param>
        /// <param name="cableServiceStatus">Indicates the cable service status</param>
        /// <returns>The resulting new roommate account number</returns>
        public string CreateRoommateAccountByAccountNumberAndSiteId(
            [RequiredItem()][RegEx(@"^(\d{7}(?<=\d*?[1-9]{1}\d*?)(?=\d*?[1-9]{1}\d*?)\d{2})?$", RegexOptions.None)]
                string accountNumber9,
            [RequiredItem()][SiteId()]int siteId,
            [RequiredItem()][StringLength(0, 10)]string firstName,
            [RequiredItem()][StringLength(0, 15)]string lastName,
            [StringLength(0, 1)]string middleInitial,
            [StringLength(0, 4)]string title,
            [RegEx(@"^((([0]?[1-9]|1[0-2])/([0-2]?[0-9]|3[0-1])/[1-2]\d{3})?)?$", RegexOptions.None)]
                string dateOfBirth,
            [StringLength(0, 25)]string driverLicenseNumber,
            [RegEx(@"^(\d{9})?$", RegexOptions.None)]
                string socialSecurityNumber,
            [StringLength(0, 32)]string businessName,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string homeTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string businessTelephone,
            [RegEx(@"^(\d{10})?$", RegexOptions.None)]string otherTelephone,
            [RegEx(@"^(\d{4})?$", RegexOptions.None)]string pin,
            [RegEx(@"^(\w+([-+.]\w+)*@(?!.*?@.*?)\w+([-.]\w+)*\.\w+([-.]\w+)*)?$", RegexOptions.None)]
                string emailAddress,
            [StringLength(0, 30)]string comments,
            [RequiredItem()][StringLength(0, 25)]string complex,
            AuditCheckCode auditCheckCode,
            CableServiceStatus cableServiceStatus)
        {
            string newHouseNumber = null;

            //log entry
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(
            eCustomerAccountActivityType.CreateAccount, siteId);
            using (Log log = CreateLog(logEntry))
            {
                try
                {
                    if (dateOfBirth.Length > 0 && !new Regex(_dobRegularExpression, _options).IsMatch(dateOfBirth))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_dobErrorMessage, "dateOfBirth"));
                    }
                    // Method validator can not mask CPNI in message, due to this reason we have implemented this validation before Method Validation
                    if (socialSecurityNumber.Length > 0 && !new Regex(_ssnRegularExpression, _options).IsMatch(socialSecurityNumber))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "socialSecurityNumber", _ssnSize, _ssnSize));
                    }
                    if (pin.Length > 0 && !new Regex(_pinRegularExpression, _options).IsMatch(pin))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "pin", _pinSize, _pinSize));
                    }
                    if (!(driverLicenseNumber.Length >= _mindriverLicenseSize && driverLicenseNumber.Length <= _maxdriverLicenseSize))
                    {
                        throw new Exception(_validationErrorMessage + string.Format(_CPNIErrorMessage, "driverLicenseNumber", _mindriverLicenseSize, _maxdriverLicenseSize));
                    }

                    //create instance of MethodValidator then validate method parameters
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(),
                                                                        accountNumber9,
                                                                        siteId,
                                                                        firstName,
                                                                        lastName,
                                                                        middleInitial,
                                                                        title,
                                                                        dateOfBirth,
                                                                        driverLicenseNumber,
                                                                        socialSecurityNumber,
                                                                        businessName,
                                                                        homeTelephone,
                                                                        businessTelephone,
                                                                        otherTelephone,
                                                                        pin,
                                                                        emailAddress,
                                                                        comments,
                                                                        complex);
                    validator.Validate();
                    // Validate parameters that do not fit into the validation framework properly
                    ValidateExtra(dateOfBirth);

                    // Validate parameters where the validation framework does
                    // not handle
                    ValidateExtra(socialSecurityNumber, driverLicenseNumber, dateOfBirth);

                    //populate log information
                    logEntry.CustomerAccountNumber = accountNumber9;
                    logEntry.Note = CreateLogNote(dateOfBirth,
                                                    driverLicenseNumber,
                                                    businessName,
                                                    title,
                                                    firstName,
                                                    lastName,
                                                    middleInitial,
                                                    homeTelephone,
                                                    businessTelephone,
                                                    otherTelephone,
                                                    socialSecurityNumber,
                                                    pin,
                                                    emailAddress,
                                                    comments,
                                                    auditCheckCode,
                                                    cableServiceStatus);

                    // Set the siteid/sitecode information
                    PopulateSiteInfo(siteId);

                    //log siteId
                    logEntry.SiteId = this.SiteId;

                    // Because we are creating multiple house records for the
                    // same address, it is possible that orphaned records can be
                    // created. Scenario: Two roommates exist, one moves out,
                    // another moves in. This next bit of code attempts to
                    // recover these orphaned records instead of blindly
                    // creating a house record every time.
                    string reusableHouseNumber = CheckForReusableHouseNumber(accountNumber9);
                    if ((null != reusableHouseNumber) && (0 < reusableHouseNumber.Length))
                    {
                        newHouseNumber = reusableHouseNumber.PadLeft(7, '0');
                    } // if( ...reusableHouseNumber not empty )
                    else
                    {
                        // Create a new house number by copying the house
                        // details of the provided account number

                        // Copy the house
                        newHouseNumber = CopyHouse(
                            accountNumber9.Substring(0, accountNumber9.Length - 2),
                            complex,
                            auditCheckCode);
                        // Pad with zeros
                        newHouseNumber = newHouseNumber.PadLeft(7, '0');
                    } // else( ...reusableHouseNumber not empty )

                    if (!((null != newHouseNumber) && (!newHouseNumber.Equals("0000000"))))
                        throw new InvalidHouseNumberException();

                    // Edit the cable service status of the new house
                    EditCableServiceStatus(newHouseNumber, cableServiceStatus);


                    //Begin Aug 2014 Update Serviceable status code for new house (roommate house).
                    #region Update Serviceable status code

                    // Get parent's available Service category code and Service status code
                    List<HouseMasterServiceCategory> houserSrvCategoryList = GetHouseSrvCategoryBySideIdAndHouseNumber(Convert.ToInt32(accountNumber9.Substring(0, accountNumber9.Length - 2)), this.SiteId);

                    // Get roommate's available Service category code and Service status code
                    List<HouseMasterServiceCategory> roomMatehouserSrvCategoryList = GetHouseSrvCategoryBySideIdAndHouseNumber(Convert.ToInt32(newHouseNumber), this.SiteId);

                    bool matchRecord = false;

                    //

                    bool updateRecord = false;
                    // Set Serviceable status code "HN" for Service category Code='H' in roommate's record 
                    foreach (HouseMasterServiceCategory serviceCategoryStatus in houserSrvCategoryList)
                    {
                        //Service Category='H'
                        if (serviceCategoryStatus.ServiceCategoryCode.Equals(TypeDescriptor.GetConverter(eServiceCategory.HomeSecurity).
                                                                  ConvertTo(eServiceCategory.HomeSecurity, typeof(string)).ToString(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Set "HN" Serviceable status for roommate account if Service category is home Security
                            EditServiceStatus(newHouseNumber, ServiceCategoryConverter.HomeSecurity, TypeDescriptor.GetConverter(HomeSecurityServiceStatus.HomeSecurityNotServiceable).
                                                                  ConvertTo(HomeSecurityServiceStatus.HomeSecurityNotServiceable, typeof(string)).ToString());
                            updateRecord = true;
                            break;
                        }


                    }
                    // Check Service Category 'H' exists and updated in previous statement
                    if (!updateRecord)
                    {
                        // Creating a new record for roommate account for category code 'H' and Service Status code 'HN'
                        CreateServiceStatus(newHouseNumber, ServiceCategoryConverter.HomeSecurity, TypeDescriptor.GetConverter(HomeSecurityServiceStatus.HomeSecurityNotServiceable).
                                                                  ConvertTo(HomeSecurityServiceStatus.HomeSecurityNotServiceable, typeof(string)).ToString());
                    }

                    // applying loop on available parent's Service category
                    foreach (HouseMasterServiceCategory houserSrvCategory in houserSrvCategoryList)
                    {

                        if (houserSrvCategory.ServiceCategoryCode.Equals(ServiceCategoryConverter.Telephone, StringComparison.InvariantCultureIgnoreCase))
                        {

                            // Category 'T' AND Service Status = 'TY' or 'TH'
                            if (houserSrvCategory.ServiceCategoryCode.Equals(ServiceCategoryConverter.Telephone, StringComparison.InvariantCultureIgnoreCase)
                                    && (houserSrvCategory.ServiceableStatusCode.Equals(TypeDescriptor.GetConverter(TelephoneServiceStatus.TelephoneServiceable).ConvertTo(TelephoneServiceStatus.TelephoneServiceable, typeof(string)).ToString(), StringComparison.InvariantCultureIgnoreCase)
                                    || houserSrvCategory.ServiceableStatusCode.Equals(TypeDescriptor.GetConverter(TelephoneServiceStatus.TelephoneHybrid).ConvertTo(TelephoneServiceStatus.TelephoneHybrid, typeof(string)).ToString(), StringComparison.InvariantCultureIgnoreCase)))
                            {
                                // This is notel site list object
                                List<string> nortelMarketsSiteIdList = new List<string>();
                                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NortelMarketsSiteIdList"]))
                                {
                                    string[] tempArray = ConfigurationManager.AppSettings["NortelMarketsSiteIdList"].ToString().Split(new char[] { ',' });
                                    foreach (string temp in tempArray)
                                        nortelMarketsSiteIdList.Add(temp);
                                }

                                // Site Id of parent record should be available in nortelMarketList and Status Code is TH
                                if (nortelMarketsSiteIdList.Contains(this.SiteId.ToString())
                                    && houserSrvCategory.ServiceableStatusCode.Equals(TypeDescriptor.GetConverter(TelephoneServiceStatus.TelephoneServiceable).ConvertTo(TelephoneServiceStatus.TelephoneServiceable, typeof(string)).ToString(), StringComparison.InvariantCultureIgnoreCase))
                                {
                                    // Set service status "TH" for roommate house
                                    EditServiceStatus(newHouseNumber, ServiceCategoryConverter.Telephone, TypeDescriptor.GetConverter(TelephoneServiceStatus.TelephoneHybrid).ConvertTo(TelephoneServiceStatus.TelephoneHybrid, typeof(string)).ToString());
                                }

                            }

                        }
                    }
                    #endregion Update Serviceable status code

                } // try
                catch (ValidationException ve)
                {
                    logEntry.SetError(ve.Message);
                    throw;
                }
                catch (DataSourceException dse)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(dse.Message);
                    throw new DataSourceUnavailableException(dse);
                }
                catch (CmErrorException excCm)
                {
                    // Log and translate to BLL exception
                    logEntry.SetError(excCm.Message, excCm.ErrorCode);
                    throw TranslateCmException(excCm);
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(blle.Message);
                    throw;
                }
                catch (Exception e)
                {
                    // Log and flag as unexpected
                    logEntry.SetError(e.Message);
                    throw new UnexpectedSystemException(e);
                }
            } //using (Log log = CreateLog(logEntry))

            //create new roommate account then return it to the caller of the method;
            //use the new house number
            return CreateCustomerAccountByHouseNumberAndSiteId(newHouseNumber,
                                                                this.SiteId,
                                                                dateOfBirth,
                                                                driverLicenseNumber,
                                                                businessName,
                                                                title,
                                                                firstName,
                                                                lastName,
                                                                middleInitial,
                                                                homeTelephone,
                                                                businessTelephone,
                                                                otherTelephone,
                                                                socialSecurityNumber,
                                                                pin,
                                                                emailAddress,
                                                                comments,
                                                                BlockPaymentSetting.Blank,
                                                                accountNumber9);
        } // CreateRoommateAccountByAccountNumberAndSiteId( ... )
        #endregion // CreateRoommateAccountByAccountNumberAndSiteId

        #region Helper Functions

        /// <summary>
        /// This method pulls service category and Serviceable status code of house by using house number and Site Id
        /// </summary>
        /// <param name="houserNumber">House Number</param>
        /// <param name="siteId">Site id</param>
        /// <returns>List object of HouseMasterServiceCategory type.</returns>
        private List<HouseMasterServiceCategory> GetHouseSrvCategoryBySideIdAndHouseNumber(int houserNumber, int siteId)
        {
            // Here we will create House Data Access Layer Object and call GetHouseSrvCategoryBySideIdAndHouseNumber method
            //instantiate DalHouse
            DalHouse dalHouse = new DalHouse();

            //get address of passed account number
            List<HouseMasterServiceCategory> returnCollection = dalHouse.GetHouseSrvCategoryBySideIdAndHouseNumber(houserNumber, siteId);

            //return collection
            return returnCollection;

        }


        /// <summary>
        /// Creates the customer demographic inline (INL00170) for use in
        /// MAC00054.
        /// </summary>
        /// <param name="accountNumber9">The nine digit account number from the system of record.</param>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="blockPaymentFlag">Flag indicating can accept payment types on this account.</param>
        /// <returns>The properly configured customer demographics inline (INL00170).</returns>
        private Request.INL00170 CreateINL00170(string accountNumber9, string dateOfBirth, string driverLicenseNumber, BlockPaymentSetting blockPaymentFlag)
        {
            bool blnDobEmpty = (null != dateOfBirth) ? (0 == dateOfBirth.Trim().Length) : true;

            INL00170Helper inl170req = new INL00170Helper(accountNumber9);
            // NOTE: we must be guaranteed a valid date here
            inl170req.Element.DATEOFBIRTH = (!blnDobEmpty) ? (new IcomsDate(DateTime.Parse(dateOfBirth))).ToString() : MakeNullIfEmpty(dateOfBirth);
            inl170req.Element.DATEOFBRTHCNTRL = MakeControlClearIfEmpty(dateOfBirth);
            inl170req.Element.DRVRSLCNSNMBR = MakeNullIfEmpty(driverLicenseNumber);
            inl170req.Element.DRVRSLCNSNMBRCNTRL = MakeControlClearIfEmpty(driverLicenseNumber);

            switch (blockPaymentFlag)
            {
                case BlockPaymentSetting.None:
                    // Clear the value using the control parameter
                    inl170req.Element.VCRDATACNTRL = " ";
                    break;
                case BlockPaymentSetting.Credit:
                case BlockPaymentSetting.Debit:
                case BlockPaymentSetting.All:
                    inl170req.Element.VCRDATA = Convert.ToChar(blockPaymentFlag).ToString();
                    break;
                default:
                    // Just leave everything null for these settings:
                    //      * BlockPaymentSetting.Blank
                    //      * Anything not above
                    break;
            } // switch( blockPaymentFlag )

            return (Request.INL00170)inl170req;
        } // CreateINL00170( ... )

        /// <summary>
        /// Creates the modify customer information inline (INL00192) for use in
        /// MAC00054.
        /// </summary>
        /// <param name="houseNumber">The seven digit house number from the system of record.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="businessName">Customer business name.</param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="pin">Customer PIN.</param>
        /// <param name="socialSecurityNumber">Customer social security number.</param>
        /// <returns>The properly configured modify customer information inline (INL00193).</returns>
        private Request.INL00192 CreateINL00192(
            string houseNumber, string title,
            string firstName, string middleInitial, string lastName,
            string businessName, string emailAddress, string comments,
            string homeTelephone, string businessTelephone, string otherTelephone,
            string pin, string socialSecurityNumber)
        {
            INL00192Helper inl192req = new INL00192Helper(houseNumber);
            // NOTE: Channel lineup ID is not required under the 7.1
            // interface... this parameter is no longer necessary and we don't
            // have to worry about finding the default channel lineup id.
            //inl192req.Element.LINPID = "4101";
            // NOTE: The default customer type (i.e. 1 - DEFAULT) exists for all
            // sites... using this is the best plan for managing residential
            // accounts. When business accounts are added, maybe this should be
            // changed.
            inl192req.Element.CSTMRTYPECODE = "1";
            // NOTE: The default customer category (i.e. 1 - PERMANENT RESIDENT)
            // exists for all sites... using this is the best plan for managing
            // residential accounts. When business accounts are added, maybe
            // this should be changed.
            inl192req.Element.CSTMRCTGRY = "1";
            inl192req.Element.CSTMRBSNSNAME2 = MakeNullIfEmpty(businessName);
            inl192req.Element.CSTMRCMNT = MakeNullIfEmpty(comments);
            inl192req.Element.EMALADRS = MakeNullIfEmpty(emailAddress);
            inl192req.Element.FRSTNAME = MakeNullIfEmpty(firstName);
            inl192req.Element.HOMETLPHNNMBR = MakeNullIfEmpty(homeTelephone);
            inl192req.Element.LASTNAME = MakeNullIfEmpty(lastName);
            inl192req.Element.MIDLINTL = MakeNullIfEmpty(middleInitial);
            inl192req.Element.OTHRTLPHNNMBR = MakeNullIfEmpty(otherTelephone);
            inl192req.Element.BSNSTLPHNNMBR = MakeNullIfEmpty(businessTelephone);
            inl192req.Element.PIN = MakeNullIfEmpty(pin);
            inl192req.Element.SOCLSCRTYNMBR = MakeNullIfEmpty(socialSecurityNumber);
            inl192req.Element.TITL = MakeNullIfEmpty(title);

            return (Request.INL00192)inl192req;
        } // CreateINL00192( ... )

        /// <summary>
        /// Creates the modify customer information inline (INL00193) for use in
        /// MAC00054.
        /// </summary>
        /// <param name="accountNumber9">The nine digit account number from the system of record.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="businessName">Customer business name.</param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="pin">Customer PIN.</param>
        /// <param name="socialSecurityNumber">Customer social security number.</param>
        /// <returns>The properly configured modify customer information inline (INL00193).</returns>
        private Request.INL00193 CreateINL00193(
            string accountNumber9, string title,
            string firstName, string middleInitial, string lastName,
            string businessName, string emailAddress, string comments,
            string homeTelephone, string businessTelephone, string otherTelephone,
            string pin, string socialSecurityNumber)
        {
            INL00193Helper inl193req = new INL00193Helper(accountNumber9);
            inl193req.Element.CSTMRBSNSNAME2 = MakeNullIfEmpty(businessName);
            inl193req.Element.CSTMRBSNSNAME2CNTRL = MakeControlClearIfEmpty(businessName);
            inl193req.Element.CSTMRCMNT = MakeNullIfEmpty(comments);
            inl193req.Element.CSTMRCMNTCNTRL = MakeControlClearIfEmpty(comments);
            inl193req.Element.EMALADRS = MakeNullIfEmpty(emailAddress);
            inl193req.Element.EMALADRSCNTRL = MakeControlClearIfEmpty(emailAddress);
            inl193req.Element.FRSTNAME = MakeNullIfEmpty(firstName);
            inl193req.Element.FRSTNAMECNTRL = MakeControlClearIfEmpty(firstName);
            inl193req.Element.HOMETLPHNNMBR = MakeNullIfEmpty(homeTelephone);
            inl193req.Element.HOMETLPHNNMBRCNTRL = MakeControlClearIfEmpty(homeTelephone);
            inl193req.Element.LASTNAME = MakeNullIfEmpty(lastName);
            inl193req.Element.LASTNAMECNTRL = MakeControlClearIfEmpty(lastName);
            inl193req.Element.MIDLINTL = MakeNullIfEmpty(middleInitial);
            inl193req.Element.MIDLINTLCNTRL = MakeControlClearIfEmpty(middleInitial);
            inl193req.Element.OTHRTLPHNNMBR = MakeNullIfEmpty(otherTelephone);
            inl193req.Element.OTHRTLPHNNMBRCNTRL = MakeControlClearIfEmpty(otherTelephone);
            inl193req.Element.BSNSTLPHNNMBR = MakeNullIfEmpty(businessTelephone);
            inl193req.Element.BSNSTLPHNNMBRCNTRL = MakeControlClearIfEmpty(businessTelephone);
            inl193req.Element.PIN = MakeNullIfEmpty(pin);
            inl193req.Element.PINCNTRL = MakeControlClearIfEmpty(pin);
            inl193req.Element.SOCLSCRTYNMBR = MakeNullIfEmpty(socialSecurityNumber);
            inl193req.Element.SOCLSCRTYNMBRCNTRL = MakeControlClearIfEmpty(socialSecurityNumber);
            inl193req.Element.TITL = MakeNullIfEmpty(title);
            inl193req.Element.TITLCNTRL = MakeControlClearIfEmpty(title);

            return (Request.INL00193)inl193req;
        } // CreateINL00193( ... )

        /// <summary>
        /// Given a call to the MAC00054 macro using INL00192 inline, parse the
        /// account number from the response.
        /// </summary>
        /// <param name="mac54resp">The surrounding MAC00054 response element.</param>
        /// <returns>
        /// The account number field as it was returned in the inline from the
        /// macro response.
        /// </returns>
        private string getAccountNumberFromINL00192Response(Response.MAC00054 mac54resp)
        {
            string strAccountNumber = string.Empty;

            foreach (object objElement in mac54resp.Items)
            {
                if (objElement is Response.INL00192)
                {
                    Response.INL00192 inl00192resp = objElement as Response.INL00192;
                    strAccountNumber = inl00192resp.ACNTNMBR;
                } // if( objElement is Response.INL00192 )

            } // foreach( object objElement in mac54resp.Items )

            return strAccountNumber.PadLeft(9, '0');
        } // getAccountNumberFromINL00192Response()

        /// <summary>
        /// Creates a log entry note for the transaction.
        /// </summary>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName">Customer business name.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="socialSecurityNumber">
        /// Customer social security number. IMPORTANT: Currently, this is NOT
        /// logged for security reasons.
        /// </param>
        /// <param name="pin">
        /// Customer PIN. IMPORTANT: Currently, this is NOT logged for security
        /// reasons.
        /// </param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="blockPaymentFlag">Flag indicating can accept a check on this account.</param>
        /// <returns>
        /// A formatted abbrviated string representing only the information
        /// changed in this transaction.
        /// </returns>
        protected string CreateLogNote(
            string dateOfBirth, string driverLicenseNumber,
            string businessName, string title, string firstName,
            string lastName, string middleInitial,
            string homeTelephone, string businessTelephone,
            string otherTelephone, string socialSecurityNumber, string pin,
            string emailAddress, string comments,
            BlockPaymentSetting blockPaymentFlag)
        {
            System.Text.StringBuilder strbld = new System.Text.StringBuilder();
            strbld.Append("Fields edited: ");
            if (null != dateOfBirth)
                strbld.Append("DOB, ");
            if (null != driverLicenseNumber)
                strbld.Append("DL, ");
            if (null != businessName)
                strbld.Append("BN, ");
            if (null != title)
                strbld.Append("TTL, ");
            if (null != firstName)
                strbld.Append("FN, ");
            if (null != middleInitial)
                strbld.Append("MN, ");
            if (null != lastName)
                strbld.Append("LN, ");
            if (null != homeTelephone)
                strbld.Append("HT, ");
            if (null != businessTelephone)
                strbld.Append("BT, ");
            if (null != otherTelephone)
                strbld.Append("OT, ");
            if (null != socialSecurityNumber)
                strbld.Append("SSN, ");
            if (null != pin)
                strbld.Append("PIN, ");
            if (null != emailAddress)
                strbld.Append("EML, ");
            if (null != comments)
                strbld.Append("COM, ");
            if (BlockPaymentSetting.Blank != blockPaymentFlag)
                strbld.Append("BPS, ");

            return strbld.ToString();
        } // CreateLogNote()

        /// <summary>
        /// Creates a log entry note for the transaction.
        /// </summary>
        /// <param name="dateOfBirth">Customer date of birth.</param>
        /// <param name="driverLicenseNumber">Customer driver's license number.</param>
        /// <param name="businessName">Customer business name.</param>
        /// <param name="title">Customer title (i.e. Mr., Mrs., Dr., etc.)</param>
        /// <param name="firstName">Customer first name.</param>
        /// <param name="lastName">Customer last name.</param>
        /// <param name="middleInitial">Customer middle initial.</param>
        /// <param name="homeTelephone">Customer home telephone number.</param>
        /// <param name="businessTelephone">Customer business telephone number.</param>
        /// <param name="otherTelephone">Customer other telephone number.</param>
        /// <param name="socialSecurityNumber">
        /// Customer social security number. IMPORTANT: Currently, this is NOT
        /// logged for security reasons.
        /// </param>
        /// <param name="pin">
        /// Customer PIN. IMPORTANT: Currently, this is NOT logged for security
        /// reasons.
        /// </param>
        /// <param name="emailAddress">Customer email address.</param>
        /// <param name="comments">Customer comments.</param>
        /// <param name="auditChecCode">Audit check code.</param>
        /// <param name="cableServiceStatus">Cable service status.</param>
        /// <returns>
        /// A formatted abbrviated string representing only the information
        /// changed in this transaction.
        /// </returns>
        protected string CreateLogNote(
            string dateOfBirth, string driverLicenseNumber,
            string businessName, string title, string firstName,
            string lastName, string middleInitial,
            string homeTelephone, string businessTelephone,
            string otherTelephone, string socialSecurityNumber, string pin,
            string emailAddress, string comments,
            AuditCheckCode auditCheckCode, CableServiceStatus cableServiceStatus)
        {
            System.Text.StringBuilder strbld = new System.Text.StringBuilder();
            strbld.Append("Fields edited: ");
            if (null != dateOfBirth)
                strbld.Append("DOB, ");
            if (null != driverLicenseNumber)
                strbld.Append("DL, ");
            if (null != businessName)
                strbld.Append("BN, ");
            if (null != title)
                strbld.Append("TTL, ");
            if (null != firstName)
                strbld.Append("FN, ");
            if (null != middleInitial)
                strbld.Append("MN, ");
            if (null != lastName)
                strbld.Append("LN, ");
            if (null != homeTelephone)
                strbld.Append("HT, ");
            if (null != businessTelephone)
                strbld.Append("BT, ");
            if (null != otherTelephone)
                strbld.Append("OT, ");
            if (null != socialSecurityNumber)
                strbld.Append("SSN, ");
            if (null != pin)
                strbld.Append("PIN, ");
            if (null != emailAddress)
                strbld.Append("EML, ");
            if (null != comments)
                strbld.Append("COM, ");
            if (AuditCheckCode.Blank != auditCheckCode)
                strbld.Append("ACC, ");
            if (CableServiceStatus.Unknown != cableServiceStatus)
                strbld.Append("CSS, ");

            return strbld.ToString();
        } // CreateLogNote()

        /// <summary>
        /// Translate the input string to null if empty. NOTE: Spaces are fine.
        /// </summary>
        /// <param name="strValue">The input value to translate.</param>
        /// <returns>
        /// If string.Empty or null, returns null. Otherwise, the input string
        /// is returned.
        /// </returns>
        protected string MakeNullIfEmpty(string strValue)
        {
            if (null != strValue)
                strValue = strValue.Trim();
            return ((null != strValue) && (0 < strValue.Length)) ? strValue : null;
        } // MakeNullIfEmpty()

        /// <summary>
        /// If the value passed in is empty vs. null, then return the control
        /// value clear setting.
        /// </summary>
        /// <param name="strValue">The corresponding input value.</param>
        /// <returns>
        /// If string.Empty, returns a space (universal clear value). Otherwise,
        /// null is returned.
        /// </returns>
        protected string MakeControlClearIfEmpty(string strValue)
        {
            return ((null != strValue) && (0 == strValue.Length)) ? " " : null;
        } // MakeControlClearIfEmpty()

        /// <summary>
        /// Validates what the incoming valdation framework cannot handle.
        /// </summary>
        /// <param name="strDateOfBirth">Date of birth string to be validated.</param>
        protected void ValidateExtra(string dateOfBirth)
        {
            // Parse the date/time from dateOfBirth parameter
            // NOTE: This parameter may be null or a valid date (e.g.
            // 10/02/1970). The regular expression validates the basic
            // date format; however, there are still edge cases (e.g.
            // 2/31/2006) which must simply be parsed out here. Make
            // sure we throw the proper validation exception.
            try
            {
                dateOfBirth = MakeNullIfEmpty(dateOfBirth);
                if (null != dateOfBirth)
                    DateTime.Parse(dateOfBirth);
            } // try
            catch (Exception)
            {
                // Mimic the validation exception here.
                throw new ValidationException(string.Format(
                    "{0} contains an invalid value of '{1}'.",
                    "dateOfBirth",
                    dateOfBirth));
            } // catch( Exception )
        } // ValidateDate( ... )

        /// <summary>
        /// Validates what the incoming valdation framework cannot handle.
        /// </summary>
        /// <param name="socialSecurityNumber">Social security number</param>
        /// <param name="driverLicenseNumber">Driver License Number</param>
        /// <param name="strDateOfBirth">Date of birth</param>
        protected void ValidateExtra(string socialSecurityNumber, string driverLicenseNumber, string dateOfBirth)
        {
            ///Changes for making Driving License Number and Date of birth mandatory if SSN is not provided starts here.
            if (String.IsNullOrEmpty(socialSecurityNumber) && (String.IsNullOrEmpty(driverLicenseNumber) || String.IsNullOrEmpty(dateOfBirth)))
            {
                //Check if only Date of birth was provided
                if (String.IsNullOrEmpty(socialSecurityNumber) && String.IsNullOrEmpty(driverLicenseNumber) && !String.IsNullOrEmpty(dateOfBirth))
                {
                    //If only date of birth was provided, exception stating either SSN or Driver license number is required must be thrown.
                    throw new ValidationException("SSN or Driver license number is required");
                }
                //Check if only Driver License Number was provided
                else if (String.IsNullOrEmpty(socialSecurityNumber) && !String.IsNullOrEmpty(driverLicenseNumber) &&
                          String.IsNullOrEmpty(dateOfBirth))
                {
                    //If only Driver license number was provided, exception stating either SSN or date of birth is required must be thrown.
                    throw new ValidationException("SSN or Date of birth is required");
                }
                else
                {
                    //If none of the three were provided, following exception will be thrown.
                    throw new ValidationException("If SSN is not provided Driver license number and Date of birth are mandatory");
                }
            }
            //Changes for making Driving License Number and Date of birth mandatory if SSN is not provided ends here.
        } // ValidateExtra( ... )

        /// <summary>
        /// This method facilitates in copying a house.
        /// </summary>
        /// <param name="houseNumber">The 7-digit house number from the system of record</param>        
        /// <param name="complex">The complex</param>
        /// <param name="auditCheckCode">Indicates the audit check code</param>
        /// <returns>The new house number</returns>
        private string CopyHouse(string houseNumber, string complex, AuditCheckCode auditCheckCode)
        {
            string newHouseNumber = string.Empty;

            Request.INL00132 inl132req = CreateINL00132(houseNumber, complex, auditCheckCode);

            try
            {
                //invoke MAC53 'Work With Houses Macro'
                Mac00053Helper mac53req = new Mac00053Helper(TASKCODE_WORK_WITH_HOUSES, this.SiteId, inl132req);
                Response.MAC00053 mac53resp = (Response.MAC00053)this.Invoke(mac53req.Element);

                //parse the house number 
                newHouseNumber = GetHouseNumberFromINL00132Response(mac53resp);
            } // try
            catch (CmErrorException exc)
            {
                // 1310 = bad company error => invalid house number in this macro
                if (1310 != exc.ErrorCode)
                    throw exc;
            } // catch( CmErrorException )

            return newHouseNumber;
        } // CopyHouse(...)

        /// <summary>
        /// This method facilitates in modifying the cable serviceable status code of a house
        /// </summary>
        /// <param name="houseNumber">The 7-digit house number from the system of record</param>        
        /// <param name="cableServiceStatus">The cable serviceable status</param>
        private void EditCableServiceStatus(string houseNumber, CableServiceStatus cableServiceStatus)
        {
            // Only change the status if a change is necessary... there is no
            // unknown status in ICOMS, so just ignore that one and let the
            // default flow through.
            if (cableServiceStatus != CableServiceStatus.Unknown)
            {
                Mac00011Helper mac11req = new Mac00011Helper(houseNumber,
                                                                this.SiteId,
                                                                TypeDescriptor.GetConverter(eServiceCategory.Cable).
                                                                  ConvertTo(eServiceCategory.Cable, typeof(string)).ToString(),
                                                                TypeDescriptor.GetConverter(cableServiceStatus).
                                                                  ConvertTo(cableServiceStatus, typeof(string)).ToString(),
                                                                FUNCTION_ACTION_UPDATE.ToString());

                //invoke MAC11 'Maintain House Serviceable Status'
                Response.MAC00011 mac11resp = (Response.MAC00011)this.Invoke(mac11req.Element);
            } // if( cableServiceStatus != CableServiceStatus.Unknown )
        }//end EditCableServiceStatus(...)

        /// <summary>
        /// This method facilitates in modifying the serviceable status code of a house
        /// </summary>
        /// <param name="houseNumber">The 7-digit house number from the system of record</param>        
        /// <param name="cableServiceStatus">The cable serviceable status</param>
        private void EditServiceStatus(string houseNumber, String serviceCategory, string serviceableStatusCode)
        {

            Mac00011Helper mac11req = new Mac00011Helper(houseNumber, this.SiteId, serviceCategory,
                                                            serviceableStatusCode,
                                                            FUNCTION_ACTION_UPDATE.ToString());

            //invoke MAC11 'Maintain House Serviceable Status'
            Response.MAC00011 mac11resp = (Response.MAC00011)this.Invoke(mac11req.Element);

        }//end EditServiceStatus(...)

        /// <summary>
        /// This method facilitates in modifying the serviceable status code of a house
        /// </summary>
        /// <param name="houseNumber">The 7-digit house number from the system of record</param>        
        /// <param name="cableServiceStatus">The cable serviceable status</param>
        private void CreateServiceStatus(string houseNumber, String serviceCategory, string serviceableStatusCode)
        {

            //Creating Mac00011Helper object. We are passing blank space in "functionAction" variable. 
            // When we passed 1 in functionAction, it will not create new record in ICOMS. NetCracker Team resolve this issue and as per NetCracket team's suggestion, we are passing " " value.
            Mac00011Helper mac11req = new Mac00011Helper(houseNumber, this.SiteId, serviceCategory, serviceableStatusCode, " ");
            //invoke MAC11 'Maintain House Serviceable Status'
            Response.MAC00011 mac11resp = (Response.MAC00011)this.Invoke(mac11req.Element);

        }//end CreateServiceStatus(...)

        /// <summary>
        /// Creates the copy/modify house information inline (INL00132) for use in
        /// MAC00053.
        /// </summary>
        /// <param name="houseNumber">The seven digit house number from the system of record.</param>
        /// <param name="complex">Complex</param>
        /// <param name="auditCheckCode">Audit check code</param>
        /// <returns></returns>
        private Request.INL00132 CreateINL00132(string houseNumber,
            string complex, AuditCheckCode auditCheckCode)
        {
            INL00132Helper inl132req = new INL00132Helper(houseNumber);
            // NOTE: During a copy operation, this field is used to match
            // against other house records. Apparently, records with the same
            // complex name are hard-linked and cannot be undone. This makes
            // it impossible to set any properies in the INL170 call later.
            if (null != complex)
            {
                // Use the date as the unique key... only one-digit year is
                // necessary for our purposes
                string strAppend = DateTime.Now.ToString("yMMddHHmmss");
                // Don't go beyond max 25 chars
                //[07-01-2009] Start Changes for Production Issue where max length for complex field is being exceeded
                int intTrimLength = (13 > complex.Length) ? complex.Length : 13;
                //[07-01-2009] End Changes for Production Issue where max length for complex field is being exceeded 
                complex = complex.Substring(0, intTrimLength) + strAppend;
            } // if( null != complex )
            inl132req.Element.CMPLX = MakeNullIfEmpty(complex);
            //if auditCheckCode equals AuditCheckCode.Blank use null          
            inl132req.Element.AUDTCHCKCODE =
                (auditCheckCode == AuditCheckCode.Blank) ?
                    null : Convert.ToChar(auditCheckCode).ToString();

            return (Request.INL00132)inl132req;
        } // CreateINL00132( ... )

        /// <summary>
        /// Parse the house number from the MAC00053 macro response using INL00132
        /// </summary>
        /// <param name="mac53resp">The surrounding MAC00053 response element.</param>
        /// <returns>
        /// The house number field as it was returned in the inline from the 
        /// macro response
        /// </returns>
        private string GetHouseNumberFromINL00132Response(Response.MAC00053 mac53resp)
        {
            string newHouseNumber = string.Empty;

            foreach (object objElement in mac53resp.Items)
            {
                if (objElement is Response.INL00132)
                {
                    Response.INL00132 inl00132resp = objElement as Response.INL00132;
                    newHouseNumber = inl00132resp.NEWHOUSNMBR;
                } // if( objElement is Response.INL00132 )
            } // foreach( object objElement in mac53resp.Items )

            return newHouseNumber;
        } // GetHouseNumberFromINL00132Response()

        /// <summary>
        /// Checks for reusable house numbers
        /// </summary>
        /// <param name="accountNumber9">The account number</param>
        /// <returns></returns>
        private string CheckForReusableHouseNumber(string accountNumber9)
        {
            // Assume no orphaned houses
            string strReturn = string.Empty;

            //parse house number from accountNumber9
            string houseNumber = accountNumber9.Substring(0, accountNumber9.Length - 2);

            //instantiate DalHouse
            DalHouse dalHouse = new DalHouse();

            //get address of passed account number
            Address address = dalHouse.GetAddressBySiteIdAndHouseNumber(this.SiteId, houseNumber);
            if (null != address)
            {
                //get orphaned houses
                string[] orphanedHouses = dalHouse.GetOrphanedHouseListBySiteIdAndHouseAddress(this.SiteId, address);

                //return first orphaned house if there is any
                if (orphanedHouses.Length > 0)
                    strReturn = orphanedHouses[0].ToString();
            } // if( null != address )

            return strReturn;
        } // CheckForReusableHouseNumber(...)

        
        #endregion // Helper Functions

    } // class CustomerAccountManager

} // namespace Cox.BusinessLogic.CustomerAccount
