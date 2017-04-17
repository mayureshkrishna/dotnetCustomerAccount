using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Cox.Web.Exceptions;

using Microsoft.ApplicationBlocks.ExceptionManagement;
namespace Cox.BusinessLogic.Exceptions
{
    /// <summary>
    /// Generic abstract exception class from which all other BLL exceptions
    /// are defined. This enable the ability to let exceptions bubble up within
    /// a method if it has already been handled. For example:
    /// try
    /// {
    /// }
    /// catch(BusinessLogicLayerException)
    /// {
    ///		// its been handled
    ///		throw;
    /// }
    /// catch(Exception e)
    /// {
    ///		// handle it.
    ///		throw new SomeBLLException(e);
    /// }
    /// </summary>
    public abstract class BusinessLogicLayerException : BaseApplicationException
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public BusinessLogicLayerException() : base() { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public BusinessLogicLayerException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public BusinessLogicLayerException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected BusinessLogicLayerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Business Layer Exception defining that the underlying datasource is unavailable.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "DataSourceUnavailable")]
    public class DataSourceUnavailableException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "A requested datasource is unavailable.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public DataSourceUnavailableException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public DataSourceUnavailableException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public DataSourceUnavailableException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public DataSourceUnavailableException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected DataSourceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Business Layer Exception defining that the underlying ServiceAgent is unavailable.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ServiceAgentUnavailable")]
    public class ServiceAgentUnavailableException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "A requested Service Agent is unavailable.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ServiceAgentUnavailableException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ServiceAgentUnavailableException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ServiceAgentUnavailableException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ServiceAgentUnavailableException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ServiceAgentUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid SetTopBoxId
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidSetTopBoxId")]
    public class InvalidSetTopBoxIdException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified SetTopBoxId is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidSetTopBoxIdException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidSetTopBoxIdException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidSetTopBoxIdException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidSetTopBoxIdException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidSetTopBoxIdException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid HouseNumber
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails( SoapFaultCodeType.Client, "InvalidHouseNumber" )]
    public class InvalidHouseNumberException: BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified HouseNumber is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidHouseNumberException() : base( _message ) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidHouseNumberException( Exception inner ) : base( _message, inner ) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidHouseNumberException( string message ) : base( message ) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidHouseNumberException( string message, Exception inner ) : base( message, inner ) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidHouseNumberException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
    }
    /// <summary>
    /// Account already exists
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails( SoapFaultCodeType.Client, "AccountAlreadyExists" )]
    public class AccountAlreadyExistsException: BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified house number already has an active account associated.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public AccountAlreadyExistsException() : base( _message ) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public AccountAlreadyExistsException( Exception inner ) : base( _message, inner ) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public AccountAlreadyExistsException( string message ) : base( message ) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AccountAlreadyExistsException( string message, Exception inner ) : base( message, inner ) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected AccountAlreadyExistsException( SerializationInfo info, StreamingContext context ) : base( info, context ) { }
    }
    /// <summary>
    /// Invalid AccountNumber
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidAccountNumber")]
    public class InvalidAccountNumberException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified AccountNumber is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidAccountNumberException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidAccountNumberException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidAccountNumberException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidAccountNumberException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidAccountNumberException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// MOP Authorization Failure
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "MopAuthorizationFailed")]
    public class MopAuthorizationFailedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified method of payment information could not be authorized.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public MopAuthorizationFailedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public MopAuthorizationFailedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public MopAuthorizationFailedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public MopAuthorizationFailedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected MopAuthorizationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// MOP Already Authorized
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "MopAlreadyAuthorized")]
    public class MopAlreadyAuthorizedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified method of payment information has already been authorized.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public MopAlreadyAuthorizedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public MopAlreadyAuthorizedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public MopAlreadyAuthorizedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public MopAlreadyAuthorizedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected MopAlreadyAuthorizedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Bank routing number is not valid
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidBankRoutingNumber")]
    public class InvalidBankRoutingNumberException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified bank routing number is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidBankRoutingNumberException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidBankRoutingNumberException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidBankRoutingNumberException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidBankRoutingNumberException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidBankRoutingNumberException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Method of Payment account information is incorrect
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidMopAccountInformation")]
    public class InvalidMopAccountInformationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified method of payment information is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidMopAccountInformationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidMopAccountInformationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidMopAccountInformationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidMopAccountInformationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidMopAccountInformationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// No or invalid previous MOP information was found.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPreviousMOPInformation")]
    public class InvalidPreviousMOPInformationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The previous MOP information is either invalid or does not exist.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPreviousMOPInformationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPreviousMOPInformationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPreviousMOPInformationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPreviousMOPInformationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPreviousMOPInformationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// The supplied pin did not match the pin of the saved MOP.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPreviousMOPPin")]
    public class InvalidPreviousMOPPinException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The pin provided does not match the pin for the saved previous MOP information.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPreviousMOPPinException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPreviousMOPPinException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPreviousMOPPinException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPreviousMOPPinException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPreviousMOPPinException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Method of Payment accoun information is incorrect
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InquiryNoStatements")]
    public class InquiryNoStatementsException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified customer account does not have any active statements.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InquiryNoStatementsException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InquiryNoStatementsException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InquiryNoStatementsException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InquiryNoStatementsException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InquiryNoStatementsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Method of Payment account information is incorrect
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidStatementCode")]
    public class InvalidStatementCodeException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified statement code is not valid for the associated customer account number.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidStatementCodeException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidStatementCodeException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidStatementCodeException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidStatementCodeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidStatementCodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Unexpected system error.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "UnexpectedSystemError")]
    public class UnexpectedSystemException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "An unexpected system exception occurred.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UnexpectedSystemException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public UnexpectedSystemException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public UnexpectedSystemException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnexpectedSystemException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected UnexpectedSystemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Method of Payment accoun information is incorrect
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPpvEvent")]
    public class InvalidPpvEventException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The requested PayPerView Event is not valid for the associated customer account number.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPpvEventException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPpvEventException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPpvEventException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPpvEventException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPpvEventException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid PinNumber
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPinNumber")]
    public class InvalidPinNumberException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified PinNumber is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPinNumberException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPinNumberException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPinNumberException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPinNumberException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPinNumberException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid PinNumber
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPassword")]
    public class InvalidPasswordException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified password is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPasswordException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPasswordException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPasswordException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPasswordException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPasswordException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid PinNumber
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PinNumberRequired")]
    public class PinNumberRequiredException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "A PinNumber is required.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PinNumberRequiredException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PinNumberRequiredException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PinNumberRequiredException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PinNumberRequiredException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PinNumberRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// AccountNumber failed the credit check.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "CreditCheckFailed")]
    public class CreditCheckFailedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The customer account failed the credit check.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public CreditCheckFailedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public CreditCheckFailedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public CreditCheckFailedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CreditCheckFailedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected CreditCheckFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// PpvEventRestricted Exception. Viewing the PpvEvent is restricted for this account.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PpvEventRestricted")]
    public class PpvEventRestrictedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The requested PayPerView Event is restricted for the given customer account.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PpvEventRestrictedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PpvEventRestrictedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PpvEventRestrictedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PpvEventRestrictedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PpvEventRestrictedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Event already ordered exception.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PpvEventAlreadyOrdered")]
    public class PpvEventAlreadyOrderedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The requested PayPerView Event has already been ordered.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PpvEventAlreadyOrderedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PpvEventAlreadyOrderedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PpvEventAlreadyOrderedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PpvEventAlreadyOrderedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PpvEventAlreadyOrderedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Customer does not have the necessary equipment.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "EquipmentException")]
    public class EquipmentException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The equipment on record does not support the requested action.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public EquipmentException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public EquipmentException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public EquipmentException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public EquipmentException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected EquipmentException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Method of Payment accoun information is incorrect
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PpvExpired")]
    public class PpvExpiredException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The time to order the requested PayPerView Event has passed.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PpvExpiredException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PpvExpiredException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PpvExpiredException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PpvExpiredException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PpvExpiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidSalesReason")]
    public class InvalidSalesReasonException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The sales reason was invalid or not specified.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidSalesReasonException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidSalesReasonException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidSalesReasonException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidSalesReasonException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidSalesReasonException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "AccountInUse")]
    public class AccountInUseException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The account being accessed is already in use and therefore cannot be modified.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public AccountInUseException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public AccountInUseException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public AccountInUseException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AccountInUseException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected AccountInUseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidCampaignCode")]
    public class InvalidCampaignCodeException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified Campaign Code is invalid,inactive or does not meet requirements for its use.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidCampaignCodeException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidCampaignCodeException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidCampaignCodeException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidCampaignCodeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidCampaignCodeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PrerequisitesNotMet")]
    public class PrerequisitesNotMetException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Unable to apply a service code to an account because the prerequisites for the account were not met.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PrerequisitesNotMetException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PrerequisitesNotMetException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PrerequisitesNotMetException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PrerequisitesNotMetException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PrerequisitesNotMetException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "WorkOrderCheckinError")]
    public class WorkOrderCheckInException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Unable to check-in the WorkOrder. One or more of the requirements were not met; however, the workorder was created.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public WorkOrderCheckInException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public WorkOrderCheckInException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public WorkOrderCheckInException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public WorkOrderCheckInException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected WorkOrderCheckInException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidSiteId")]
    public class InvalidSiteIdException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified siteid was invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidSiteIdException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidSiteIdException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidSiteIdException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidSiteIdException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidSiteIdException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidCatalogInformation")]
    public class InvalidCatalogInformationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified catalog information was invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidCatalogInformationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidCatalogInformationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidCatalogInformationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidCatalogInformationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidCatalogInformationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidOrderCriteria")]
    public class InvalidOrderCriteriaException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The specified OrderCriteria information was invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidOrderCriteriaException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidOrderCriteriaException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidOrderCriteriaException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidOrderCriteriaException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidOrderCriteriaException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "CannotAllocateTimeSlot")]
    public class CannotAllocateTimeSlotException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Unable to allocate a time slot for WorkOrder.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public CannotAllocateTimeSlotException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public CannotAllocateTimeSlotException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public CannotAllocateTimeSlotException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public CannotAllocateTimeSlotException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected CannotAllocateTimeSlotException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// The requested catalog/offer information was not properly configured.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "ProductConfiguration")]
    public class ProductConfigurationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The requested catalog item was not properly configured.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ProductConfigurationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ProductConfigurationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ProductConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ProductConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ProductConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid or not specified sales reason.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "ProductConfiguration")]
    public class RuleEngineException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "An exception occurred working with Rule Engine.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public RuleEngineException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public RuleEngineException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public RuleEngineException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public RuleEngineException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected RuleEngineException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid WorkOrder Status.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidWorkOrderStatus")]
    public class InvalidWorkOrderStatusException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "An error occurred setting the status of the Work Order in the System of Record. The order was created but needs follow up by a CSR.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidWorkOrderStatusException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidWorkOrderStatusException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidWorkOrderStatusException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidWorkOrderStatusException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidWorkOrderStatusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Field locked.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "InvalidWorkOrderStatus")]
    public class FieldLockedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "A field locked exception occurred. This typically occurs when an attempt to add a new work was made with a one-time service charge and the one-time service charge already existed on an open work order for that customer account.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public FieldLockedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public FieldLockedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public FieldLockedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public FieldLockedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected FieldLockedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid Email Account.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidEmailAddress")]
    public class InvalidEmailAddressException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Email Account is not active.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidEmailAddressException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidEmailAddressException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidEmailAddressException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidEmailAddressException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidEmailAddressException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid Phone Number.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidPhoneNumber")]
    public class InvalidPhoneNumberException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Phone Number is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidPhoneNumberException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidPhoneNumberException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidPhoneNumberException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidPhoneNumberException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidPhoneNumberException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid DNIS.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidDnis")]
    public class InvalidDnisException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Dnis is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidDnisException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidDnisException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidDnisException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidDnisException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidDnisException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Promise to Pay Denied
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "PromiseToPayDenied")]
    public class PromiseToPayDeniedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Promise to Pay is denied.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public PromiseToPayDeniedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public PromiseToPayDeniedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public PromiseToPayDeniedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public PromiseToPayDeniedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected PromiseToPayDeniedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Work order prerequist error
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "WorkOrderPrerequisiteError")]
    public class WorkOrderPrerequisitesException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The work order could not be created due to a pre-requisite conflict on the account.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public WorkOrderPrerequisitesException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public WorkOrderPrerequisitesException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public WorkOrderPrerequisitesException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public WorkOrderPrerequisitesException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected WorkOrderPrerequisitesException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Work order not found
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "WorkOrderNotFound")]
    public class WorkOrderNotFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Work Order Number entered could not be found.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public WorkOrderNotFoundException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public WorkOrderNotFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public WorkOrderNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public WorkOrderNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected WorkOrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Attribute configuration exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "AttributeConfiguration")]
    public class AttributeConfigurationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The attribute is improperly configured.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public AttributeConfigurationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public AttributeConfigurationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public AttributeConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public AttributeConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected AttributeConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Attribute configuration exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "OfferConfiguration")]
    public class OfferConfigurationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The offer is improperly configured.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public OfferConfigurationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public OfferConfigurationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public OfferConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OfferConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected OfferConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Order not found exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "OrderNotFound")]
    public class OrderNotFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The order was not found.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public OrderNotFoundException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public OrderNotFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public OrderNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OrderNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected OrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Order data missing exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "OrderDataMissing")]
    public class OrderDataMissingException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Required data for the order is missing or could not be found.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public OrderDataMissingException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public OrderDataMissingException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public OrderDataMissingException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OrderDataMissingException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected OrderDataMissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// OrderFooNotFound exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "OrderFooNotFound")]
    public class OrderFooNotFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The foo for the order was not found.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public OrderFooNotFoundException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public OrderFooNotFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public OrderFooNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public OrderFooNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected OrderFooNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// DowngradeNotAllowed exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "DowngradeNotAllowed")]
    public class DowngradeNotAllowedException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The requested action would result in a downgrade.  The action cannot be performed.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public DowngradeNotAllowedException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public DowngradeNotAllowedException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public DowngradeNotAllowedException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public DowngradeNotAllowedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected DowngradeNotAllowedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// UnknownServiceFeature exception
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "UnknownServiceFeature")]
    public class UnknownServiceFeatureException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The Service Feature Location is Unknown.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UnknownServiceFeatureException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public UnknownServiceFeatureException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public UnknownServiceFeatureException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnknownServiceFeatureException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected UnknownServiceFeatureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Debit card number is invalid.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidDebitCardNumber")]
    public class InvalidDebitCardNumberException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The debit card number provided was not found in the BIN file and is therefore invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidDebitCardNumberException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidDebitCardNumberException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidDebitCardNumberException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidDebitCardNumberException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidDebitCardNumberException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid channel profile.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "InvalidChannelProfile")]
    public class InvalidChannelProfileException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The channel profile is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidChannelProfileException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidChannelProfileException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidChannelProfileException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidChannelProfileException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidChannelProfileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Profile filter error.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ProfileFilterError")]
    public class ProfileFilterException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Profile filter failed to evaluate.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ProfileFilterException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ProfileFilterException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ProfileFilterException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ProfileFilterException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ProfileFilterException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Channel profile not found.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ProfileNotFound")]
    public class ProfileNotFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The channel profile was not found.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ProfileNotFoundException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ProfileNotFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ProfileNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ProfileNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ProfileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Error in the object factory.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ObjectFactoryError")]
    public class ObjectFactoryException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "There was an error with the object factory.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ObjectFactoryException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ObjectFactoryException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ObjectFactoryException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ObjectFactoryException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ObjectFactoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Error in the list handler.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ListProfileHandlerError")]
    public class ListProfileHandlerException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "There was an error with the list handler.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ListProfileHandlerException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ListProfileHandlerException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ListProfileHandlerException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ListProfileHandlerException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ListProfileHandlerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    /// Invalid hierarchy type.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidHierarchyTypeError")]
    public class InvalidHierarchyTypeException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Invalid hierarchy type.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidHierarchyTypeException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidHierarchyTypeException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidHierarchyTypeException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidHierarchyTypeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidHierarchyTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// User setting config error.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "UserSettingConfigurationError")]
    public class UserSettingConfigurationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The user settings confiuration is invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UserSettingConfigurationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public UserSettingConfigurationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public UserSettingConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UserSettingConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected UserSettingConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// User setting config error.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "UnsupportedScriptTypeException")]
    public class UnsupportedScriptTypeException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Script type is unsupported by this script handler";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public UnsupportedScriptTypeException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public UnsupportedScriptTypeException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public UnsupportedScriptTypeException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnsupportedScriptTypeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected UnsupportedScriptTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// User setting config error.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Server, "ScriptExecutionException")]
    public class ScriptExecutionException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "Script type is unsupported by this script handler";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public ScriptExecutionException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public ScriptExecutionException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public ScriptExecutionException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public ScriptExecutionException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected ScriptExecutionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// Invalid channel profile.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "InvalidChannelType")]
    public class InvalidChannelTypeException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The channel type is unknown or invalid.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public InvalidChannelTypeException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public InvalidChannelTypeException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidChannelTypeException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public InvalidChannelTypeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected InvalidChannelTypeException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// The requested catalog/offer information was not properly configured.
    /// </summary>
    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "SystemConfiguration")]
    public class SystemConfigurationException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when not provided
        /// during construction.
        /// </summary>
        protected const string _message = "The system is not properly configured to run.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public SystemConfigurationException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public SystemConfigurationException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public SystemConfigurationException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public SystemConfigurationException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected SystemConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    //[03-02-09] Start Changes for Q-matic requirement

    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "MultipleMatchsFound")]
    public class MultipleMatchsFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when only
        /// phone number. 
        /// </summary>
        protected const string _message = "Multiple account matches found for the input criteria.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public MultipleMatchsFoundException() : base (_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public MultipleMatchsFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public MultipleMatchsFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public MultipleMatchsFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected MultipleMatchsFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    [CoxSoapExceptionDetails(SoapFaultCodeType.Client, "NoMatchFound")]
    public class NoMatchFoundException : BusinessLogicLayerException
    {
        /// <summary>
        /// Constant value containing the underlying error message to use when only
        /// phone number. 
        /// </summary>
        protected const string _message = "No account match found for the input criteria.";
        /// <summary>
        /// The default constructor.
        /// </summary>
        public NoMatchFoundException() : base(_message) { }
        /// <summary>
        /// Constructor taking an inner exception.
        /// </summary>
        /// <param name="inner"></param>
        public NoMatchFoundException(Exception inner) : base(_message, inner) { }
        /// <summary>
        /// Constructor to an exception with no Inner exception using the given message.
        /// </summary>
        /// <param name="message"></param>
        public NoMatchFoundException(string message) : base(message) { }
        /// <summary>
        /// Constructor to create an exception with an Inner exception to add to the
        /// stack trace as well as use the given message for the outermost exception.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public NoMatchFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Protected constructor used for serialization.
        /// </summary>
        /// <param name="info">Serialization Information</param>
        /// <param name="context">Given context.</param>
        protected NoMatchFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    //[03-02-09] Start Changes for Q-matic requirement
}
