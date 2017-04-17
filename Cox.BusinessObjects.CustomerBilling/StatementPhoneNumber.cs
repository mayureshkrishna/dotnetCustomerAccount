using System;
using System.Text;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// Summary description for StatementPhoneNumber.
	/// </summary>
	public class StatementPhoneNumber
	{
		#region Member Variables
		/// <summary>
		/// This is the telephone number associated with with this statement.
		/// </summary>
		protected string _phoneNumber=null;	
		
		#endregion

		#region Ctors
		/// <summary>
		/// Default ctor
		/// </summary>
		public StatementPhoneNumber(){}

		/// <summary>
		/// Ctor taking params
		/// </summary>
		/// <param name="phoneNumber"></param>		
		public StatementPhoneNumber(string phoneNumber)
		{
			_phoneNumber = phoneNumber;
			
		}
		#endregion

		#region Properties
		/// <summary>
		/// This property returns the phone number for this statement.
		/// </summary>		
		[XmlAttribute("PhoneNumber")]
		public string PhoneNumber
		{
			get{ return _phoneNumber; }
			set{ _phoneNumber=value; }
		}
		#endregion
	}
}