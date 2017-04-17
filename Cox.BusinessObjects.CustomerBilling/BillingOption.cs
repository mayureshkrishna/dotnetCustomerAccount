using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	#region enumerations
	/// <summary>
	/// These describe the method that is used to handle the billingoption on a statement.
	/// </summary>
	[TypeConverter(typeof(BillingOptionConverter))]
	public enum eBillingOption
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// This indicates a paper bill is delivered to a customer.
		/// </summary>
		Paper				= 1,
		/// <summary>
		/// This describes commercial use of the described entity. 
		/// </summary>
		Commercial			= 2,
		/// <summary>
		/// This indciates that the paper bill will not be delivered to the
		/// customer. This may indicate electronic billing.
		/// </summary>
		StopPaper			= 3,
		/// <summary>
		/// This is identical to StopPaper but for the web. If you want to know why we have more than
		/// one enumerated value mean the same thing....don't ask. You are less likely to get a headache.
		/// </summary>
		WebStopPaper		= 4,
		/// <summary>
		/// This means that a bill was sent to the customer but was sent back as undelivered.
		/// </summary>
		DeadLetter			= 5
} // enum eBillingOption

	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class BillingOptionConverter : EnumConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public BillingOptionConverter():base(typeof(eBillingOption)){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Overrides the ConvertFrom method of EnumConverter. The only reason
		/// we are deriving from EnumConverter is to handle converting from an
		/// integer to a potentially invalid value.
		/// 
		/// Since the default EnumConverter handles converting from an enumeration
		/// to another type, we will not modify its behavior.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object value)
		{
			// check if we care about it
			if(value is int||value is string)
			{
				int val=0;
				try{val=Convert.ToInt32(value);}
				catch{/*do nothing*/}
				return Enum.IsDefined(typeof(eBillingOption),val)?
					(eBillingOption)val:eBillingOption.Unknown;
			}
			else
			{
				return base.ConvertFrom(context,culture,value);
			}
		}
		#endregion public methods
	}
	#endregion converter class(es)
}
