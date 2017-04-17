using System;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace Cox.Wotl.ServiceManagementPlatform
{
	/// <summary>
	/// This class is a proxy for the Waste Of Time Layer (WOTL) (or Web Object Transaction Layer 
	/// depending on who you are talking to) Instead of letting us call the stored procs for the SAMP database
	/// someone had to go and wrap it with XML and call it an API.  Now we have to worry about 
	/// formatting the messages and working with the "API" instead of just using a DAL with
	/// stored procs.
	/// </summary>
	public class SmpProxy : HttpPostClientProtocol
	{
		#region Constants
		/// <summary>Default timeout is 15 seconds</summary>
		protected const int __defaultTimeoutValue=15000;
		/// <summary>Default url</summary>
		protected const string __defaultSmpUrl="http://localhost:8181";
		/// <summary>This is the default error text used for SMP exceptions</summary>
		protected const string __defaultErrorMessage="The SMP Service Agent has returned an error.";
		/// <summary>This is the error text to use when we cannot translate the errorcode.</summary>
		protected const string __cannotTranslateErrorCode="The SMP service agent returned an invalid response.";
		/// <summary>This is the text used to retrieve the returncode from the wotl.</summary>
		protected const string __returnCode="ReturnCode";
		/// <summary>This is the text used to retrieve the returnmessage from the wotl.</summary>
		protected const string __returnMessage="ReturnMsg";
		#endregion // Constants

		#region Ctors
		/// <summary>
		/// Protected ctor for setting comm vals
		/// </summary>
		public SmpProxy() : base()
		{
			// read url from config file.
			string strUrlSetting=ConfigurationManager.AppSettings["Cox.ServiceAgent.Smp"];
			Url=strUrlSetting!=null?strUrlSetting:__defaultSmpUrl;
			// read the timeout from the config file.
			Timeout = __defaultTimeoutValue;
			try{Timeout=Convert.ToInt32(ConfigurationManager.AppSettings["Cox.ServiceAgent.Smp.Timeout"]);}
			catch{/*do nothing*/}
			// Set miscellaneous properties
			RequestEncoding = Encoding.ASCII;
			UnsafeAuthenticatedConnectionSharing = false;			
		} 
		#endregion

		#region Public Methods
		/// <summary>
		/// Sends in the request and returns back the inner response.
		/// </summary>
		/// <param name="smpRequest"></param>
		/// <returns></returns>
		public object Send(object smpRequest)
		{
			try
			{
				//wrap request in SMP request xml
				ITVRequest request = new SmpHelper(smpRequest);
				//create web request to send
				WebRequest webRequest = GetWebRequest(new Uri(Url));
				//send
				SendRequest(webRequest, request);
				//get response back
				HttpWebResponse webResponse = (HttpWebResponse)GetWebResponse(webRequest);
				//get smpResponse out of response
				ITVResponse smpResponse = ReceiveResponse(webResponse);
				webResponse.Close();
				// this checks for ITV Errors
				checkForSystemException(smpResponse);
				// this checks for Errors in Item Response.
				checkForResponseException(smpResponse.Item);
				// no errors, all responses should be in good shape.
				return smpResponse.Item;
			}
			catch(Exception ex)
			{
				throw new SmpUnavailableException(ex);
			}
		}
		#endregion

		#region HttpWebClientProtocol Overrides

		/// <summary>
		/// This is an internal helper function to create a proper web
		/// request specific to Smp's unique property settings. Of note,
		/// the KeepAlive property will cause major intermittent problems
		/// if not set. Also, the ContentType and Method properties are 
		/// mandatory.
		/// </summary>
		/// <param name="uri">Essentially, the URL to be connected.</param>
		/// <returns>The return value is a proper web request suitable for connection.</returns>
		protected override WebRequest GetWebRequest(Uri uri)
		{
			//create a new web request to send
			WebRequest webRequest = null;
			try
			{
				webRequest = base.GetWebRequest(uri);
			} 
			catch(InvalidOperationException invOpEx)
			{
				throw new SmpProxyConfigurationException(invOpEx);
			} 
			catch(Exception ex)
			{
				throw new SmpProxyException(ex);
			} 
			// Clear the headers 
			webRequest.Headers.Clear();
			// Set particular web request properties 
			webRequest.ContentType = "text/xml";
			webRequest.Method = "POST";
			// Set properties particular to the HttpWebRequest
			HttpWebRequest reqHttp = webRequest as HttpWebRequest;
			if(null != reqHttp)reqHttp.KeepAlive = false;
			return webRequest;
		} 
		#endregion 

		#region Stream Handling

		/// <summary>
		/// This helper function controls the actual formatting of the data
		/// written to the web request stream. 
		/// </summary>
		/// <param name="request">A web request instance properly configured for SMP.</param>
		/// <param name="reqicoms">An ICOMS element woith a proper child element to indicate a task.
		/// </param>
		protected virtual void SendRequest( WebRequest webRequest, ITVRequest smpRequest)
		{
			Stream stream = null;
			XmlTextWriter xmlTextWriter = null;

			try
			{
				stream = webRequest.GetRequestStream();
				xmlTextWriter = new XmlTextWriter(stream, this.RequestEncoding);
				xmlTextWriter.WriteRaw(ObjectSerializerWithEncoding.Serialize(smpRequest, Encoding.UTF8));
			} 
			catch(WebException webEx)
			{
				throw new SmpUnavailableException(webEx);
			} 
			catch(Exception ex)
			{
				throw new SmpProxyException(ex);
			} 
			finally
			{
				//clean up writer and stream
				if(null != xmlTextWriter) xmlTextWriter.Close();
				if(null != stream) stream.Close();
			} 
		}

		/// <summary>
		/// This method reads the response stream and serializes the results
		/// into a proper SMP Response.
		/// </summary>
		/// <param name="response">
		/// A standard web response object obtained from the above web request
		/// instance.
		/// </param>
		/// <returns>
		/// An ITVResponse with the appropriate elements set
		/// </returns>
		protected virtual ITVResponse ReceiveResponse(HttpWebResponse response)
		{
			//check http response before we try to deserialize
			if(response.StatusCode != HttpStatusCode.OK)
			{
				throw new SmpUnavailableException(
					string.Format("The call to the WOTL failed with return code '{0}' and message '{1}'",response.StatusCode.ToString(),response.StatusDescription));
			}
			
			ITVResponse smpResponse = null;
			Stream stream = null;
			StreamReader streamReader = null;

			try
			{
				stream = response.GetResponseStream();
				// NOTE: Cannot use XmlTextReader. It complains and gives
				// an empty string. instead use the generic StreamReader
				streamReader = new StreamReader(stream);
				smpResponse = (ITVResponse)ObjectSerializerWithEncoding.Deserialize(streamReader.ReadToEnd(), typeof(ITVResponse));
			}
			catch( IOException ioEx)
			{
				throw new SmpUnavailableException(ioEx);
			}
			catch( InvalidOperationException ivoEx)
			{
				throw new SmpUnavailableException(ivoEx);
			}
			catch(Exception ex)
			{
				throw new SmpProxyException(ex);
			} 
			finally
			{
				//clean up stream reader and stream
				if(null != streamReader) streamReader.Close();
				if(null != stream) stream.Close();
			} 
			return smpResponse;
		} 
		#endregion 

		#region error handling functions
		/// <summary>
		/// This method translates error information from the response objects.
		/// 
		/// Note: Per BusinessEdge, it is not expected that error information 
		/// will go more then 2 levels deep (e.g. one level of ITVReturnCode
		/// and a single level of ReturnCode in child Item element). If this
		/// ever changes, then we need to revisit how this is done. However,
		/// at this time, ITV is driving the requirements and none is expected.
		/// </summary>
		/// <param name="response"></param>
		protected void checkForSystemException(ITVResponse itvResponse)
		{
			// set to defaults
			int errorCode=0;
			string errorText=null;

			if(itvResponse!=null&&itvResponse.ITVReturnCode!=null)
			{
				try
				{
					errorCode=Convert.ToInt32(itvResponse.ITVReturnCode.Trim());
				}
				catch
				{
					// eat the error and set return to 2 
					errorCode=2;
					errorText=__cannotTranslateErrorCode;
				}
				try{errorText=itvResponse.ITVReturnMsg;}
				catch{/*do nothing*/}
				if(errorCode!=0)
				{
					throw new SmpSystemException(errorCode,errorText);
				}
			}
		}
		/// <summary>
		/// This method translates error information from the response objects.
		/// 
		/// Note: Per BusinessEdge, it is not expected that error information 
		/// will go more then 2 levels deep (e.g. one level of ITVReturnCode
		/// and a single level of ReturnCode in child Item element). If this
		/// ever changes, then we need to revisit how this is done. However,
		/// at this time, ITV is driving the requirements and none is expected.
		/// </summary>
		/// <param name="response"></param>
		protected void checkForResponseException(object response)
		{
			// set to defaults
			int errorCode=0;
			string errorText=null;
			// time to reflect...
			Type type=response.GetType();
			FieldInfo fiReturnCode=type.GetField(__returnCode);
			if(fiReturnCode!=null)
			{
				try
				{
					// get error code
					object val=fiReturnCode.GetValue(response);
					if(val!=null)
					{
						errorCode=Convert.ToInt32(val);
					}
					// if i have an error code, then get its message.
					if(errorCode>0)
					{
						FieldInfo fiErrorMessage=type.GetField(__returnMessage);
						try{errorText=Convert.ToString(fiErrorMessage.GetValue(response));}
						catch{/*do nothing*/}
					}
				}
				catch
				{
					// eat the error and set return to 2 
					errorCode=2;
					errorText=__cannotTranslateErrorCode;
				}
				if(errorCode!=0)
				{
					throw new SmpErrorException(errorCode,errorText);
				}
			}
		}
		#endregion error handling functions
	}
}
