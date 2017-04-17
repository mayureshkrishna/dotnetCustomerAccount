using System;
using System.ComponentModel;

namespace Cox.ServiceAgent.ConnectionManager
{
	#region enumeration(s)
	/// <summary>
	/// eFunctionAction defines the different actions used to add a service code to icoms.
	/// </summary>
	[TypeConverterAttribute(typeof(FunctionActionConverter))]
	public enum eFunctionAction
	{
		/// <summary>
		/// Used to define the addtion of a service code.
		/// </summary>
		Add			= 0,
		/// <summary>
		/// Used to define the subtraction of a service code.
		/// </summary>
		Subtract	= 1
	}
	#endregion enumeration(s)

	#region converter class(es)
	/// <summary>
	/// This class wraps the eFunctionAction enumeration and provides conversions 
	/// to/from eFunctionAction
	/// </summary>
	public class FunctionActionConverter : EnumConverter
	{
		#region constants
		/// <summary>
		/// Constant defining addition value.
		/// </summary>
		public const string Add="+";
		/// <summary>
		/// Constant defining subtractoin value.
		/// </summary>
		public const string Subtract="-";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public FunctionActionConverter():base(typeof(eFunctionAction)){}
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
				string localAction = ((string)value).Trim();
				// make sure it's not an empty string.
				if(localAction.Length == 0)
				{
					throw new ArgumentNullException("functionAction");
				}
				else
				{
					// ok...what do we have.
					switch(localAction)
					{
						case Add:
							return eFunctionAction.Add;
						case Subtract:
							return eFunctionAction.Subtract;
						default:
							throw new ArgumentException(string.Format("Invalid functionAction value: {0}",localAction));
					}
				}
			}
			else
			{
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
			if(destinationType==typeof(string) && value is eFunctionAction)
			{
				return (eFunctionAction)value==eFunctionAction.Add?Add:Subtract;
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
