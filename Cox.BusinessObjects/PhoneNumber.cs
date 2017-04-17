using System;
using System.Text;
using System.Xml.Serialization;

namespace Cox.BusinessObjects
{
	/// <summary>
	/// A phone number for a customer.
	/// </summary>
	public class PhoneNumber
	{
		#region Member Variables
		/// <summary>The area code</summary>
		protected int _areaCode = 0;
		/// <summary>The exchange code</summary>
		protected int _exchange = 0;
		/// <summary>The number</summary>
		protected int _number = 0;
		#endregion
		
		#region Ctors
		/// <summary>
		/// Default ctor
		/// </summary>
		public PhoneNumber(){}

		/// <summary>
		/// Ctor taking params
		/// </summary>
		/// <param name="areaCode"></param>
		/// <param name="exchange"></param>
		/// <param name="number"></param>
		public PhoneNumber(int areaCode, int exchange, int number)
		{
			_areaCode = areaCode;
			_exchange = exchange;
			_number = number;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Property enabling the getting and setting of AreaCode.
		/// </summary>
		[XmlAttribute( "AreaCode" )]
		public int AreaCode
		{
			get{return _areaCode;}
			set{_areaCode=value;}
		}
		/// <summary>
		/// Property enabling the getting and setting of Exchange.
		/// </summary>
		[XmlAttribute( "Exchange" )]
		public int Exchange
		{
			get{return _exchange;}
			set{_exchange=value;}
		}
		/// <summary>
		/// Property enabling the getting and setting of Number.
		/// </summary>
		[XmlAttribute( "Number" )]
		public int Number
		{
			get{return _number;}
			set{_number=value;}
		}
		/// <summary>
		/// Property enabling the getting of PhoneNumber.
		/// </summary>
		[XmlAttribute( "PhoneNumberString" )]
		public string PhoneNumberString
		{
			get
			{
				//i could have used a string formater, but i don't feel like looking it up.
				StringBuilder phoneNumber = new StringBuilder();
				phoneNumber.Append("(" + _areaCode.ToString().PadLeft(3,'0') + ")");
				phoneNumber.Append(_exchange.ToString().PadLeft(3,'0'));
				phoneNumber.Append("-" + _number.ToString().PadLeft(4, '0'));
 
				return phoneNumber.ToString();
			}
		}
		#endregion
	}
}
