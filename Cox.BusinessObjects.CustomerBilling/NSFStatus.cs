using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	#region enumerations
	/// <summary>
	/// This represents the type of transaction
	/// </summary>
	[TypeConverter(typeof(NSFStatusConverter))]
	public enum eNSFStatus
	{
		/// <summary>
		/// Do not block customer payments.
		/// </summary>
		NotBlocked=1,
		/// <summary>
		/// Block CreditCard payments from customer.
		/// </summary>
		BlockCreditCards=2,
		/// <summary>
		/// Block Debit And Check payments from customer.
		/// </summary>
		BlockDebitAndChecks=3,
		/// <summary>
		/// Block All Payments from customer.
		/// </summary>
		BlockAll=4
	} // enum eNSFStatus
	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class NSFStatusConverter:EnumConverter
	{
		#region constants
		/// <summary>
		/// Value from ICOMS for Blocking CreditCard payments.
		/// </summary>
		public const string BlockCreditCards="C";
		/// <summary>
		/// Value from ICOMS for Blocking Debit and Check payments.
		/// </summary>
		public const string BlockDebitAndChecks="D";
		/// <summary>
		/// Value from ICOMS for Blocking All Payments
		/// </summary>
		public const string BlockAll="A";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public NSFStatusConverter():base(typeof(eNSFStatus)){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Overrides the ConvertFrom method of TypeConverter.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object value)
		{
			// check if we care about it
			if(value as string != null)
			{
				string local = ((string)value).Trim().ToUpper();
				// ok...what do we have.
				switch(local)
				{
					case BlockCreditCards:
						return eNSFStatus.BlockCreditCards;
					case BlockDebitAndChecks:
						return eNSFStatus.BlockDebitAndChecks;
					case BlockAll:
						return eNSFStatus.BlockAll;
					default:
						return eNSFStatus.NotBlocked;
				}
			}
			else
			{
				return base.ConvertFrom(context,culture,value);
			}
		}
		/// <summary>
		/// Overrides the ConvertTo method of TypeConverter.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object value, 
			Type destinationType)
		{
			// here we want a chance at it
			if(destinationType==typeof(string) && value is eNSFStatus)
			{
				switch((eNSFStatus)value)
				{
					case eNSFStatus.BlockCreditCards:
						return BlockCreditCards;
					case eNSFStatus.BlockDebitAndChecks:
						return BlockDebitAndChecks;
					case eNSFStatus.BlockAll:
						return BlockAll;
					default:
						return string.Empty;
				}
			}
			else
			{
				// we don't want it but the base class might.
				return base.ConvertTo(context,culture,value,destinationType);
			}
		}
		#endregion public methods
	}
	#endregion converter class(es)
}
