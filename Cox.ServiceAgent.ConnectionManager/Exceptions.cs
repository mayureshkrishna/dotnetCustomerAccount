using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Cox.Web.Exceptions;

namespace Cox.ServiceAgent.ConnectionManager
{
	/// <summary>
	/// This is a generic exception from the Connection Manager proxy.
	/// </summary>
	[Serializable]
	public class CmProxyException : BaseApplicationException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected const string _message = "The Connection Manager proxy caused an unexpected error. This is a problem with ConnectionManagerProxy and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CmProxyException() : base( _message ){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public CmProxyException( Exception inner ) : base( _message, inner ){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public CmProxyException(string message) : base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmProxyException(string message, Exception inner) : base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmProxyException(SerializationInfo info, StreamingContext context) : base(info, context){}
	}
	
	/// <summary>
	/// This exception occurs when the output key from a 
	/// ConnectionManager call does not match the input key.
	/// </summary>
	[Serializable]
	public class CmKeyMismatchException : CmProxyException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected new const string _message = "The Request.ICOMS.KEY did not correspond to Response.ICOMS.KEY. These two should match exactly. This is a problem with ConnectionManager and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CmKeyMismatchException() : base( _message ){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public CmKeyMismatchException( Exception inner ) : base( _message, inner ){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public CmKeyMismatchException(string message) : base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmKeyMismatchException(string message, Exception inner) : base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmKeyMismatchException(SerializationInfo info, StreamingContext context) : base(info, context){}
	}

	/// <summary>
	/// This exception occurs when Connection Manager is unresponsive to a 
	/// web request.
	/// </summary>
	[Serializable]
	public class CmUnavailableException : CmProxyException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected new const string _message = "The Connection Manager could not be reached. If the application is configured correctly, then this is a problem with ConnectionManager and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CmUnavailableException() : base( _message ){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public CmUnavailableException( Exception inner ) : base( _message, inner ){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public CmUnavailableException(string message) : base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmUnavailableException(string message, Exception inner) : base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context){}
	}

	/// <summary>
	/// This exception occurs when the return data from a Connection Manager
	/// call is invalid or cannot be interpreted.
	/// </summary>
	[Serializable]
	public class CmInvalidResultsException : CmProxyException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected new const string _message = "The Connection Manager result was not well-formed XML. This is a problem with ConnectionManager and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CmInvalidResultsException() : base( _message ){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public CmInvalidResultsException( Exception inner ) : base( _message, inner ){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public CmInvalidResultsException(string message) : base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmInvalidResultsException(string message, Exception inner) : base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmInvalidResultsException(SerializationInfo info, StreamingContext context) : base(info, context){}
	}

	/// <summary>
	/// Business Layer Exception defining that a service agent encountered an error.
	/// </summary>
	[Serializable]
	public class CmErrorException : BaseApplicationException
	{
		/// <summary>
		/// Constant value containing the underlying error message to use when not provided
		/// during construction.
		/// </summary>
		protected const string _message = "A requested Service Agent has returned an error.";
		/// <summary>
		/// Constains the error code returned from a failed call to the system of record.
		/// </summary>
		protected int m_intErrorCode = 0;
		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="intErrorCode"></param>
		public CmErrorException( int intErrorCode ) : base( _message )
		{ m_intErrorCode = intErrorCode; }
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="intErrorCode"></param>
		/// <param name="inner"></param>
		public CmErrorException(int intErrorCode, Exception inner ) : base( _message, inner )
		{ m_intErrorCode = intErrorCode; }
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="intErrorCode"></param>
		/// <param name="message"></param>
		public CmErrorException(int intErrorCode, string message) : base(message)
		{ m_intErrorCode = intErrorCode; }
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="intErrorCode"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmErrorException(int intErrorCode, string message, Exception inner) : base(message,inner)
		{ m_intErrorCode = intErrorCode; }
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmErrorException(SerializationInfo info, StreamingContext context) : base(info, context){}
		/// <summary>
		/// ConnectionManager ErrorCode.
		/// </summary>
		public int ErrorCode
		{
			get{ return m_intErrorCode; }
		} // int Error
	}

	/// <summary>
	/// This exception occurs when the Connection Manager Proxy is improperly
	/// configured.
	/// </summary>
	[Serializable]
	public class CmProxyConfigurationException : CmProxyException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected new const string _message = "The Connection Manager Proxy is not properly configured. This is a problem with the application cofiguration and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CmProxyConfigurationException() : base( _message ){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public CmProxyConfigurationException( Exception inner ) : base( _message, inner ){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public CmProxyConfigurationException(string message) : base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CmProxyConfigurationException(string message, Exception inner) : base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected CmProxyConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context){}
	}

}
