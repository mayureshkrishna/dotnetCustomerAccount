using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	#region enumerations
	/// <summary>
	/// Payment status is used to indicate server status when making
	/// a payment.
	/// </summary>
	[TypeConverter(typeof(PaymentStatusConverter))]
	public enum ePaymentStatus
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// This is used to indicate payment failure.
		/// </summary>
		Failure				= 0,
		/// <summary>
		/// This is used to indicate payment success.
		/// </summary>
		Success				= 1
	} // enum ePaymentStatus

	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class PaymentStatusConverter:EnumConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public PaymentStatusConverter():base(typeof(ePaymentStatus)){}
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
			System.Globalization.CultureInfo culture,object value)
		{
			// check if we care about it
			if(value is int||value is string)
			{
				int val=0;
				try{val=Convert.ToInt32(value);}
				catch{/*do nothing*/}
				return Enum.IsDefined(typeof(ePaymentStatus),val)?
					(ePaymentStatus)val:ePaymentStatus.Unknown;
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
