using System;
using System.Runtime.Serialization;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.Wotl.ServiceManagementPlatform
{
	/// <summary>
	/// SmpProxyException 
	/// </summary>
	[Serializable]
	public abstract class SmpException:BaseApplicationException
	{
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public SmpException(string message):base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpException(string message,Exception inner):base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpException(SerializationInfo info,StreamingContext context):base(info,context){}
	}
	/// <summary>
	/// SmpProxyException 
	/// </summary>
	[Serializable]
	public class SmpProxyException:SmpException
	{
		/// <summary>
		/// Constant value containing the underlying error message to use when not provided
		/// during construction.
		/// </summary>
		protected const string _message="There was an exception in the Smp Proxy. This is a problem with SmpProxy and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SmpProxyException():base(_message){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public SmpProxyException(Exception inner):base(_message,inner){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public SmpProxyException(string message):base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpProxyException(string message,Exception inner):base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpProxyException(SerializationInfo info,StreamingContext context):base(info,context){}
	}
	/// <summary>
	/// This exception occurs when the SMP Proxy is improperly configured.
	/// </summary>
	[Serializable]
	public class SmpProxyConfigurationException:SmpException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected const string _message="The SMP Proxy is not properly configured. This is a problem with the application cofiguration and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SmpProxyConfigurationException():base(_message){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public SmpProxyConfigurationException(Exception inner):base(_message,inner){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public SmpProxyConfigurationException(string message):base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpProxyConfigurationException(string message,Exception inner):base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpProxyConfigurationException(SerializationInfo info,StreamingContext context):base(info,context){}
	}
	/// <summary>
	/// This exception occurs when the SMP server is unresponsive to a web request.
	/// </summary>
	[Serializable]
	public class SmpUnavailableException:SmpProxyException
	{
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when not provided during construction. 
		/// </summary>
		protected new const string _message="The SMP server could not be reached. If the application is configured correctly,then this is a problem with SMP and should be resolved immediately.";
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SmpUnavailableException():base(_message){}
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="inner"></param>
		public SmpUnavailableException(Exception inner):base(_message,inner){}
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="message"></param>
		public SmpUnavailableException(string message):base(message){}
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpUnavailableException(string message,Exception inner):base(message,inner){}
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpUnavailableException(SerializationInfo info,StreamingContext context):base(info,context){}
	}
	/// <summary>
	/// Business Layer Exception defining that a service agent encountered an error.
	/// </summary>
	[Serializable]
	public class SmpErrorException:SmpException
	{
		/// <summary>
		/// Constant value containing the underlying error message to use when not provided
		/// during construction.
		/// </summary>
		protected const string _message="The Smp Service Agent has returned an error.";
		/// <summary>
		/// Constains the error code returned from a failed call to the system of record.
		/// </summary>
		protected int _errorCode=0;
		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="errorCode"></param>
		public SmpErrorException(int errorCode):base(_message)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="inner"></param>
		public SmpErrorException(int errorCode,Exception inner):base(_message,inner)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		public SmpErrorException(int errorCode,string message):base(message)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpErrorException(int errorCode,string message,Exception inner):base(message,inner)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpErrorException(SerializationInfo info,StreamingContext context):base(info,context){}
		/// <summary>
		/// ConnectionManager ErrorCode.
		/// </summary>
		public int ErrorCode
		{
			get{ return _errorCode; }
		}
	}
	/// <summary>
	/// Business Layer Exception defining that a service agent encountered an error.
	/// </summary>
	[Serializable]
	public class SmpSystemException:SmpException
	{
		/// <summary>
		/// Constant value containing the underlying error message to use when not provided
		/// during construction.
		/// </summary>
		protected const string _message="The Smp Service Agent had an internal system error.";
		/// <summary>
		/// Constains the error code returned from a failed call to the system of record.
		/// </summary>
		protected int _errorCode=0;
		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="errorCode"></param>
		public SmpSystemException(int errorCode):base(_message)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor taking an inner exception.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="inner"></param>
		public SmpSystemException(int errorCode,Exception inner):base(_message,inner)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor to an exception with no Inner exception using the given message.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		public SmpSystemException(int errorCode,string message):base(message)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Constructor to create an exception with an Inner exception to add to the
		/// stack trace as well as use the given message for the outermost exception.
		/// </summary>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public SmpSystemException(int errorCode,string message,Exception inner):base(message,inner)
		{ _errorCode=errorCode; }
		/// <summary>
		/// Protected constructor used for serialization.
		/// </summary>
		/// <param name="info">Serialization Information</param>
		/// <param name="context">Given context.</param>
		protected SmpSystemException(SerializationInfo info,StreamingContext context):base(info,context){}
		/// <summary>
		/// ConnectionManager ErrorCode.
		/// </summary>
		public int ErrorCode
		{
			get{ return _errorCode; }
		}
	}
}
