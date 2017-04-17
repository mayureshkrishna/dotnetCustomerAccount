using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
//NOTE: THIS IS THE SAME CLASS AS THE ONE IN COX.SERIALIZATION WITH SOME MINOR MODIFICATIONS
//		TO ALLOW YOU TO PASS IN THE ENCODING TYPE FOR SERIALIZATION.
//		THE NEXT TIME THE FRAMEWORK IS UPDATED, THE CLASS IN COX.SERIALIZATION SHOULD BE 
//		REPLACED WITH THIS CLASS, AND ALL PROJECTS USING THIS CODE SHOULD BE UPDATED TO USE
//		THIS CLASS
//		...BE SURE TO CHANGE THE NAMESPACE...AND THE CRAPPY CLASS NAME
namespace Cox.Wotl.ServiceManagementPlatform
{
	/// <summary>
	/// A simple class that serializes and deserializes an 
	/// object as an XmlStream for you.
	/// </summary>
	public sealed class ObjectSerializerWithEncoding
	{
		#region ctors
		/// <summary>
		/// Private constructor.
		/// </summary>
		private ObjectSerializerWithEncoding(){}

		#endregion ctors

		#region methods

		/// <summary>
		/// Serializes an object for you.
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		/// <returns>Xml String of serialized object.</returns>
		public static string Serialize( object obj )
		{
			StringWriter stringWriter = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				stringWriter = new StringWriter();
				serializer.Serialize(stringWriter, obj);
				return stringWriter.ToString();
			}
			catch
			{
				throw;
			}
			finally
			{
				if(stringWriter != null)
				{
					stringWriter.Close();
				}
			}
		}
		
		/// <summary>
		/// Serializes an object for you.
		/// </summary>
		/// <param name="obj">Object to serialize</param>
		/// <returns>Xml String of serialized object.</returns>
		/// <param name="encoding">The type of encoding for the xml...utf-8 is recomended</param>
		/// <returns></returns>
		public static string Serialize( object obj, System.Text.Encoding encoding )
		{
			StringWriterWithEncoding stringWriter = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(obj.GetType());
				stringWriter = new StringWriterWithEncoding(encoding);				
				serializer.Serialize(stringWriter, obj);			
                return stringWriter.ToString();
			}
			catch
			{
				throw;
			}
			finally
			{
				if( stringWriter != null )
				{
					stringWriter.Close();
				}
			}
		}
		/// <summary>
		/// Takes an Xml String and the object type and 
		/// returns the represented object.
		/// </summary>
		/// <param name="xml">Xml String of a previously serialized object.</param>
		/// <param name="typeOf">System type. Used to stream the xml back into object form.</param>
		/// <returns></returns>
		public static object Deserialize( string xml, System.Type typeOf )
		{
			StringReader reader = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer( typeOf );
				reader = new StringReader( xml );
				return serializer.Deserialize( reader );
			}
			catch
			{
				throw;
			}
			finally
			{
				if( reader != null )
				{
					reader.Close();
				}
			}
		}
		#endregion methods
	}
	
	/// <summary>
	/// This class with create a string writer with the supplied encoding.
	/// </summary>
	public class StringWriterWithEncoding : StringWriter 
	{ 
		#region Members
		/// <summary>The encoding type to use</summary>
		private System.Text.Encoding _encoding; 
		#endregion

		#region Ctors
		/// <summary>
		/// Default ctor that takes encoding type
		/// </summary>
		/// <param name="encoding"></param>
		public StringWriterWithEncoding (System.Text.Encoding encoding) 
		{ 
			_encoding = encoding; 
		} 
		#endregion

		#region Properties
		/// <summary>
		/// Gets the encoding used by the string writer
		/// </summary>
		public override System.Text.Encoding Encoding 
		{ 
			get {return _encoding;} 
		} 
		#endregion
	} 

}
