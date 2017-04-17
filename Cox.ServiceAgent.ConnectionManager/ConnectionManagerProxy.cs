using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

using Cox.Serialization;

using Cox.ServiceAgent.ConnectionManager;
using Request=Cox.ServiceAgent.ConnectionManager.Request;
using Response=Cox.ServiceAgent.ConnectionManager.Response;


namespace Cox.ServiceAgent.ConnectionManager
{
	/// <summary>
	/// This class is responsible for communication with the Connection
	/// Manager utility provided by Convergys. The tool responds to HTTP
	/// POST requests containing an XML document with the ICOMS tag as
	/// the root element.
	/// </summary>
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	public class ConnectionManagerProxy : HttpPostClientProtocol
	{

		#region Constants

		/// <summary>Connection Manager default timeout is 60 seconds</summary>
		protected const int kintDefaultTimeoutValue		= 60000;
		/// <summary>Connection Manager default url is localhost:8080 for local debug proxy. This is innocuous otherwise.</summary>
		protected const string kstrDefaultCmUrl			= "http://localhost:8080";
		/// <summary>This si the default error text used for CM exceptions</summary>
		protected const string kstrErrorMessageDefault	= "A requested Service Agent has returned an error.";
		#endregion // Constants


		#region Members

		/// <summary>This is the site id used only to build the CM key</summary>
		protected int		m_siteId			= 0;
		/// <summary>This contains the environment string CM uses to configure routing.</summary>
		protected string	m_strEnvironment	= string.Empty;
		/// <summary>This represents the username CM uses to connect to the AS/400</summary>
		protected string	m_strUsername		= string.Empty;
		/// <summary>This is the matching password for the above.</summary>
		protected string	m_strPassword		= string.Empty;

		#endregion // Members


		#region Construction/Destruction

		/// <summary>
		/// This constructor allows for a common construction of the proxy
		/// instance, but is not exposed publicly.
		/// </summary>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		protected ConnectionManagerProxy() : base()
		{
			string strUrlSetting = ConfigurationManager.AppSettings["Cox.ServiceAgent.ConnectionManager"];
			if( null != strUrlSetting )
				Url = strUrlSetting;
			else 
				Url = kstrDefaultCmUrl;

			// Set miscellaneous properties
			RequestEncoding = Encoding.ASCII;
			UnsafeAuthenticatedConnectionSharing = false;

			// UNDONE: timeout value configurable?
			// QUESTION: Should this be configurable by function???
			// QUESTION: Should this be mapped as seconds in config (i.e. 60 instead of 60000)???
			Timeout = kintDefaultTimeoutValue;
		} // ConnectionManagerProxy() (Constructor)

		/// <summary>
		/// Construction of a proper CM proxy occurs here. 
		/// </summary>
		/// <param name="siteId">The site id used for the cm key.</param>
		/// <param name="environment">The string used for routing (e.g. SAN).</param>
		/// <param name="username">The CM username used for authentication (e.g. JoeSmoe).</param>
		/// <param name="password">The matching passwrod for above (e.g. SmoeJoe)</param>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		public ConnectionManagerProxy( int siteId, string environment, string username, string password )
			: this()
		{
			m_siteId			= siteId;
			m_strEnvironment	= environment;
			m_strUsername		= username;
			m_strPassword		= password;
		} // ConnectionManagerProxy( strEnvironment, strUsername, strPassword ) (Constructor)

		#endregion // Construction/Destruction


		#region Properties

		/// <summary>
		/// Returns the environment value used by the connectionManager
		/// </summary>
		public string Environment
		{
			get{return m_strEnvironment;}
		} // string Environment

		/// <summary>
		/// Returns the username value used by the connectionManager
		/// </summary>
		public string UserName
		{
			get{return m_strUsername;}
		} // string UserName

		/// <summary>
		/// Returns the password value used by the connectionManager
		/// </summary>
		public string Password
		{
			get{return m_strPassword;}
		} // string Password

		#endregion Properties


		#region Synchronous Methods

		/// <summary>
		/// This is the main method invocation to CM. Notice the namespaces
		/// on both input and output are different, yet the contained elements
		/// SEEM the same. The namespaces are SEVERLY different and they will
		/// conflict one another if combined. This is a fault in the CM schemas.
		/// </summary>
		/// <param name="reqicoms">An ICOMS element from the Request namespace.</param>
		/// <returns>The return from this method is an ICOMS element from the Response namespace.</returns>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		public Response.ICOMS CmMethod( Request.ICOMS reqicoms ) 
		{
			// Set routing information from constructor of this proxy
			reqicoms.ENVIRONMENT = m_strEnvironment;
			reqicoms.USERID = m_strUsername;
			reqicoms.PASSWORD = m_strPassword;

			WebRequest req = GetWebRequest( new Uri( Url ) );
			SendRequest( req, reqicoms );
			
			WebResponse rsp = GetWebResponse( req );
			Response.ICOMS rspicoms = ReceiveResponse( rsp );
			rsp.Close();

			// Parse the standard errors
			string strErrorText = null;
			int intErrorCode = 0;
			FindError( rspicoms.Item, out intErrorCode, out strErrorText );
			if( 0 != intErrorCode )
				throw new CmErrorException( intErrorCode,
					kstrErrorMessageDefault, new CmProxyException( strErrorText ) );

			// If the output key was not returned intact, then we are getting
			// a crisscrossed message.
			if( rspicoms.KEY != reqicoms.KEY )
				throw new CmKeyMismatchException();

			return rspicoms;
		} // CmMethod()

		#endregion // Synchronous Methods


		#region Asynchronous Methods

		// UNDONE: Asynchronous Methods are not on the priority list

		#endregion // Asynchronous Methods


		#region HttpWebClientProtocol Overrides

		/// <summary>
		/// This is an internal helper function to create a proper web
		/// request specific to CM's unique property settings. Of note,
		/// the KeepAlive property will cause major intermittent problems
		/// if not set. Also, the ContentType and Method properties are 
		/// mandatory.
		/// </summary>
		/// <param name="uri">Essentially, the URL to be connected.</param>
		/// <returns>The return value is a proper web request suitable for connection.</returns>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		protected override WebRequest GetWebRequest( Uri uri )
		{
			WebRequest req = null;

			try
			{
				req = base.GetWebRequest( uri );
			} // try
			catch( InvalidOperationException excInvOp )
			{
				Debug.WriteLine( excInvOp );
				throw new CmProxyConfigurationException( excInvOp );
			} // catch( InvalidOperationException excInvOp )
			catch( Exception exc )
			{
				throw new CmProxyException( exc );
			} // catch( Exception exc )

			// Clear the headers first thing...any extras will make CM barf
			req.Headers.Clear();
			// Set particular web request properties that CM understands
			req.ContentType = "text/xml";
			req.Method = "POST";

			// Set properties particular to the HttpWebRequest
			HttpWebRequest reqHttp = req as HttpWebRequest;
			if( null != reqHttp )
			{
				reqHttp.KeepAlive = false;
			} // if( null != reqHttp )

			return req;
		} // GetWebRequest()

		#endregion // HttpWebClientProtocol Overrides


		#region Stream Handling

		/// <summary>
		/// This helper function controls the actual formatting of the data
		/// written to the web request stream. Notice, the KEY property of
		/// the ICOMS object is set here to ensure uniqueness on every send.
		/// </summary>
		/// <param name="req">
		/// A web request instance properly configured for CM.
		/// </param>
		/// <param name="reqicoms">
		/// An ICOMS element woith a proper child element to indicate a task.
		/// </param>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		protected virtual void SendRequest( WebRequest req, Request.ICOMS reqicoms )
		{
			Stream strm = null;
			XmlTextWriter wtr = null;

			// Create a new unique key on the input stream
			string strKey = m_siteId.ToString() + DateTime.Now.ToString( "yyyyMMddHHmmssffffff" );
			reqicoms.KEY = strKey;

			try
			{
				// TODO: Trace request???

				strm = req.GetRequestStream();
				wtr = new XmlTextWriter( strm, this.RequestEncoding );
				wtr.WriteRaw( ObjectSerializer.Serialize( reqicoms ) );
			} // try
			catch( WebException excWeb )
			{
				Debug.WriteLine( excWeb );
				throw new CmUnavailableException( excWeb );
			} // catch( WebException excWeb )
			catch( Exception exc )
			{
				Debug.WriteLine( exc );
				throw new CmProxyException( exc );
			} // catch( Exception exc )
			finally
			{
				if( null != wtr ) wtr.Close();
				if( null != strm ) strm.Close();
			} // finally

		} // SendRequest()

		/// <summary>
		/// This method reads the response stream and serializes the results
		/// into a proper ICOMS element from the Response namespace.
		/// </summary>
		/// <param name="rsp">
		/// A standard web response object obtained from the above web request
		/// instance.
		/// </param>
		/// <returns>
		/// A Response.ICOMS element matched to the input Request.ICOMS
		/// </returns>
		[System.Diagnostics.DebuggerStepThroughAttribute()]
		protected virtual Response.ICOMS ReceiveResponse( WebResponse rsp )
		{
			Response.ICOMS rspicoms = null;
			Stream strm = null;
			StreamReader rdr = null;

			try
			{
				strm = rsp.GetResponseStream();
				// NOTE: Cannot use XmlTextReader. It complains and gives
				// an empty string. instead use the generic StreamReader
				rdr = new StreamReader( strm );
				rspicoms = (Response.ICOMS) ObjectSerializer.Deserialize(
					rdr.ReadToEnd(), typeof(Response.ICOMS) );

				// TODO: Trace response???

			} // try
			catch( IOException excIo )
			{
				Debug.WriteLine( excIo );
				throw new CmUnavailableException( excIo );
			} // catch( IOException excIo )
			catch( Exception exc )
			{
				Debug.WriteLine( exc );
				throw new CmProxyException( exc );
			} // catch( Exception exc )
			finally
			{
				if( null != rdr ) rdr.Close();
				if( null != strm ) strm.Close();
			} // finally

			return rspicoms;
		} // ReceiveResponse()

		#endregion // Stream Handling


		#region Helper Functions

		/// <summary>
		/// Given a known ERROR instance, return the error code.
		/// </summary>
		/// <param name="objError">The suppposed ERROR instance.</param>
		/// <returns>The error code from the ERROR input parameter.</returns>
		public int getErrorNumber( object objError )
		{
			int intReturn = 0;

			if( ( null != objError ) && ( objError is Response.ERROR ) )
			{
				Response.ERROR error = objError as Response.ERROR;
				try
				{ intReturn = int.Parse( error._RC ); }
				catch( Exception exc )
				{ Debug.WriteLine( exc ); }
			} // if( ( null != objError ) && ( objError is ERROR ) )

			return intReturn;
		} // getErrorText()

		/// <summary>
		/// Given a known ERROR instance, return the message text.
		/// </summary>
		/// <param name="objError">The suppposed ERROR instance.</param>
		/// <returns>The message text from the ERROR input parameter.</returns>
		public string getErrorText( object objError )
		{
			string strReturn = null;

			if( ( null != objError ) && ( objError is Response.ERROR ) )
			{
				Response.ERROR error = objError as Response.ERROR;
				strReturn = error._MSGTEXT;
			} // if( ( null != objError ) && ( objError is ERROR ) )

			return strReturn;
		} // getErrorText()


		/// <summary>
		/// Gets the field value from the FielInfo and the target object.
		/// </summary>
		/// <param name="fi">FieldInfo from which to get teh value.</param>
		/// <param name="objTarget">Target object from which came the FieldInfo</param>
		/// <returns>The rewuested object value.</returns>
		public object getFieldValue( FieldInfo fi, object objTarget )
		{
			object objReturn = null;

			try
			{ objReturn = fi.GetValue( objTarget ); }
			catch( Exception exc )
			{ Debug.WriteLine( exc ); }

			return objReturn;
		} // getFieldValue()
		/// <summary>
		/// Check for the child or grandchild ERROR instance.
		/// </summary>
		/// <param name="obj">The child instance to Response.ICOMS.</param>
		/// <param name="intErrorCode">
		/// The error code returned from Connection Manager.
		/// </param>
		/// <param name="strErrorText">
		/// The error text returned form Connection Manager.
		/// </param>
		public void FindError( object obj,
			out int intErrorCode, out string strErrorText )
		{
			// Initialize outgoiing variables
			intErrorCode = 0;
			strErrorText = null;

			if( null != obj )
			{
				// Root member is ERROR tag
				intErrorCode = getErrorNumber( obj );
				strErrorText = getErrorText( obj );

				Type typ = obj.GetType();

				// ERROR member
				FieldInfo fi = typ.GetField( "ERROR" );
				if( null != fi )
				{
					object aobj = getFieldValue( fi, obj );
					// MCN: bug. this was checking obj should be checking aobj
					if( null != aobj )
					{
						// MCN: bug. this was checking obj should be checking aobj
						intErrorCode = getErrorNumber( aobj );
						strErrorText = getErrorText( aobj );
					} // if( null != aobj )
				} // if( null != fi )

				// ERROR embedded in Items[]
				fi = typ.GetField( "Items" );
				if( null != fi ) 
				{
					object[] aobj = getFieldValue( fi, obj ) as object[];
					if( ( null != aobj ) && ( aobj.Length > 0 ) )
					{
						intErrorCode = getErrorNumber( aobj[0] );
						strErrorText = getErrorText( aobj[0] );
					} // if( ( null != aobj ) && ( aobj.Length > 0 ) )
				} // if( null != fi )
			} // if( null != obj )

		} // FindErrorNumber()

		#endregion // Helper Functions

	} // class ConnectionManagerProxy

} // namespace Cox.ConnectionManager
