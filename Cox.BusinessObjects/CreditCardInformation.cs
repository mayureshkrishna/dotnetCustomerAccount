using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using Cox.Security.Cryptography;

namespace Cox.BusinessObjects
{
	/// <summary>
	/// Summary description for CreditCard.
	/// </summary>
	[Serializable]
	public class CreditCardInformation
	{
		#region member variables
		/// <summary>
		/// Member variable containing customer name.
		/// </summary>
		protected string _customerName=null;
		/// <summary>
		/// Member variable containing issuerType.
		/// </summary>
		protected IssuerType _issuerType=IssuerType.Unknown;
		/// <summary>
		/// Member variable containing credit card number.
		/// </summary>
		protected string _creditCardNumber=null;
		/// <summary>
		/// Member variable containing expiration month.
		/// </summary>
		protected int _expirationMonth=0;
		/// <summary>
		/// Member variable containing year.
		/// </summary>
		protected int _expirationYear=0;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CreditCardInformation(){}
		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="customerName"></param>
		/// <param name="issuerType"></param>
		/// <param name="creditCardNumber"></param>
		/// <param name="expirationMonth"></param>
		/// <param name="expirationYear"></param>
		public CreditCardInformation(string customerName, IssuerType issuerType, string creditCardNumber,
			int expirationMonth, int expirationYear)
		{
			_customerName = customerName;
			_issuerType = issuerType;
			this.CreditCardNumber = creditCardNumber;
			_expirationMonth = expirationMonth;
			_expirationYear = expirationYear;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Custoemr Name.
		/// </summary>
		[XmlAttribute("CustomerName")]
		public string CustomerName
		{
			get{return _customerName;}
			set{_customerName = value;}
		}
		/// <summary>
		/// issuerType
		/// </summary>
		[XmlElement("IssuerType")]
		public IssuerType IssuerType
		{
			get{return _issuerType;}
			set{_issuerType= value;}
		}
		/// <summary>
		/// Credit card number. We always store it encrypted.
		/// </summary>
		[XmlAttribute("CreditCardNumber")]
		public string CreditCardNumber
		{
			get
			{
				if(_creditCardNumber!=null)
				{
					SimpleCrypto encryptor = new SimpleCrypto();
					return encryptor.Decrypt(_creditCardNumber);
				}
				else
				{
					return null;
				}
			}
			set
			{
				if(value!=null)
				{
					// always ecnrypt it.
					SimpleCrypto encryptor = new SimpleCrypto();
					_creditCardNumber = encryptor.Encrypt(value);
				}
				else
				{
					_creditCardNumber=value;
				}
			}
		}
		/// <summary>
		/// Expiration Month
		/// </summary>
		[XmlAttribute("ExpirationMonth")]
		public int ExpirationMonth
		{
			get{return _expirationMonth;}
			set{_expirationMonth = value;}
		}
		/// <summary>
		/// Expiration Year
		/// </summary>
		[XmlAttribute("ExpirationYear")]
		public int ExpirationYear
		{
			get{return _expirationYear;}
			set{_expirationYear = value;}
		}
		#endregion properties

		#region GetEncryptedCreditCardNumber
		/// <summary>
		/// Returns back the internal credit card number encrypted.
		/// </summary>
		/// <returns></returns>
		public string GetEncryptedCreditCardNumber()
		{
			return _creditCardNumber;
		}
		#endregion GetEncryptedCreditCardNumber

		#region SetEncryptedCreditCardNumber
		/// <summary>
		/// Returns back the internal credit card number encrypted.
		/// </summary>
		/// <returns></returns>
		public void SetEncryptedCreditCardNumber(string encryptedCreditCardNumber)
		{
			_creditCardNumber = encryptedCreditCardNumber;
		}
		#endregion SetEncryptedCreditCardNumber

	}
}
