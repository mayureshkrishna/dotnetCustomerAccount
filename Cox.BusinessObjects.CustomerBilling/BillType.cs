using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// ICOMS documents four types of dwellings for billing purposes. 
	/// These are based represented by the letters 'C' (commercial) 
	/// 'M' (multi-family dwelling) and 'S' (single family dwelling)
	/// </summary>
	[TypeConverter(typeof(BillTypeConverter))]
	public enum BillType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown = -1,
		/// <summary>
		/// This represents commercial establishments.
		/// </summary>
		Commercial = 1,
		/// <summary>
		/// This represents multi-family dwellings.
		/// </summary>
		MultiFamily = 2,
		/// <summary>
		/// This represents single-family dwellings.
		/// </summary>
		SingleFamily = 3
	}
	#region converter class(es)
	/// <summary>
	/// This class wraps the BillType enumeration and provides conversions 
	/// to/from BillType
	/// </summary>
	public class BillTypeConverter :EnumConverter
	{
		#region constants
		/// <summary>
		/// Value sent to ICOMS for a commercial dwelling type.
		/// </summary>
		public const string Commercial="C";
		/// <summary>
		/// Value sent to ICOMS for a multi-family dwelling type.
		/// </summary>
		public const string MultiFamily="M";
		/// <summary>
		/// Value sent to ICOMS for a single-family dwelling type.
		/// </summary>
		public const string SingleFamily="S";

		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public BillTypeConverter():base(typeof(BillType)){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Overrides the ConvertFrom method of EnumConverter.
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
				string localBillType = ((string)value).Trim().ToUpper();
				//evaluate
				switch(localBillType)
				{
						case Commercial:
							return BillType.Commercial;
						case MultiFamily:
							return BillType.MultiFamily;
						case SingleFamily:
							return BillType.SingleFamily;
						default:
							return BillType.Unknown;
				}
			}
			else
			{
				// we don't want it but the base class might.
				return base.ConvertFrom(context,culture,value);
			}
		}
		/// <summary>
		/// Overrides the ConvertTo method of EnumConverter.
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
			if(destinationType==typeof(string) && value is BillType)
			{
				switch((BillType)value)
				{
					case BillType.Commercial:
						return Commercial;
					case BillType.MultiFamily:
						return MultiFamily;
					case BillType.SingleFamily:
						return SingleFamily;
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
