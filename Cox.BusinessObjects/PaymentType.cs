using System;
using System.ComponentModel;

namespace Cox.BusinessObjects
{
	#region enumerations
	/// <summary>
	/// These are the payment types available in the web services interfaces.
	/// </summary>
	[TypeConverter(typeof(PaymentTypeConverter))]
	public enum ePaymentType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// This represents the credit card type Mastercard.
		/// </summary>
		MasterCard			= 1,
		/// <summary>
		/// This represents the credit card type Discover.
		/// </summary>
		Discover			= 2,
		/// <summary>
		/// This represents the credit card type American Express.
		/// </summary>
		AmericanExpress		= 3,
		/// <summary>
		/// This represents the credit card type Visa.
		/// </summary>
		Visa				= 4,
		/// <summary>
		/// This type is used in some electronic banking transactions.
		/// </summary>
		OneTimeDirectDebit	= 5,
		/// <summary>
		/// This type is used in some electronic banking transactions.
		/// </summary>
		ElectronicCheck		= 6,
		/// <summary>
		/// This type is used in for recurring direct debit transactions.
		/// </summary>
		RecurringDirectDebit = 7,
		/// <summary>
		/// This type is used for pinless debit (ATM) transactions.
		/// </summary>
		PinlessDebit = 8
	}
	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class PaymentTypeConverter:EnumConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public PaymentTypeConverter ():base(typeof(ePaymentType)){}
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
			if(value is int)
			{
				// setup default value
				int paymentType=(int)value;
				// see if the value is defined w/in the enumeration
				if(Enum.IsDefined(typeof(ePaymentType),paymentType))
				{
					return (ePaymentType)paymentType;
				}
				else
				{
					return ePaymentType.Unknown;
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
			if(destinationType==typeof(int) && value is ePaymentType)
			{
				return (int)value;
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
