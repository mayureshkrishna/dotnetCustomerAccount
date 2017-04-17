using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	#region enumerations
	/// <summary>
	/// This represents the type of transaction
	/// </summary>
	[TypeConverter(typeof(TransactionTypeConverter))]
	public enum eTransactionType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown		= -1,
		/// <summary>
		/// This indicates a trasaction is a payment.
		/// </summary>
		Payment		= 1,
		/// <summary>
		/// This indicates a trasaction is an adjustment.
		/// </summary>
		Adjustment	= 2,
		/// <summary>
		/// This indicates a trasaction is recurring in nature.
		/// </summary>
        Recurring	= 3,
		/// <summary>
		/// This indicates a trasaction is related to a service.
		/// </summary>
		Service		= 4
	} // enum eTransactionType

	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class TransactionTypeConverter:EnumConverter
	{
		#region constants
		/// <summary>
		/// Value sent to ICOMS for.....a TransactionType of Payment
		/// </summary>
		public const string Payment="P";
		/// <summary>
		/// Value sent to ICOMS for.....a TransactionType of Adjustment
		/// </summary>
		public const string Adjustment="A";
		/// <summary>
		/// Value sent to ICOMS for.....a TransactionType of Recurring
		/// </summary>
		public const string Recurring="R";
		/// <summary>
		/// Value sent to ICOMS for.....a TransactionType of Service
		/// </summary>
		public const string Service="S";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public TransactionTypeConverter():base(typeof(eTransactionType)){}
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
					case Payment:
						return eTransactionType.Payment;
					case Adjustment:
						return eTransactionType.Adjustment;
					case Recurring:
						return eTransactionType.Recurring;
					case Service:
						return eTransactionType.Service;
					default:
						return eTransactionType.Unknown;
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
			if(destinationType==typeof(string) && value is eTransactionType)
			{
				switch((eTransactionType)value)
				{
					case eTransactionType.Payment:
						return Payment;
					case eTransactionType.Adjustment:
						return Adjustment;
					case eTransactionType.Recurring:
						return Recurring;
					case eTransactionType.Service:
						return Service;
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
