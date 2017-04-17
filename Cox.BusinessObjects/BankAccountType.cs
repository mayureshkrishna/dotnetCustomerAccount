using System;
using System.ComponentModel;

namespace Cox.BusinessObjects
{
	#region enumerations
	/// <summary>
	/// Bank account types are used in check payment transactions.
	/// </summary>
	[TypeConverter(typeof(BankAccountTypeConverter))]
	public enum eBankAccountType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown		= -1,
		/// <summary>
		/// This indicates a bank checking account.
		/// </summary>
		Checking	= 1,
		/// <summary>
		/// This represents a bank savings account.
		/// </summary>
		Savings		= 2
	}
	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eBankAccountType enumeration
	/// and provides conversions to/from eBankAccountType
	/// 
	/// Because we must decorate the enumeration with the 
	/// TypeConverterAttribute, it must exist in the same
	/// namespace...a good tradeoff.
	/// </summary>
	public class BankAccountTypeConverter : EnumConverter
	{
		#region constants
		/// <summary>
		/// Icoms constant representing checking.
		/// </summary>
		public const string Checking="C";
		/// <summary>
		/// Icoms constant representing savings.
		/// </summary>
		public const string Savings="S";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public BankAccountTypeConverter():base(typeof(eBankAccountType)){}
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
			if(value.GetType()==typeof(string)||value.GetType()==typeof(char))
			{
				string localType=Convert.ToString(value).Trim().ToUpper();
				// here, we are always returning checking or savings.
				return localType==Savings?
					eBankAccountType.Savings:
					eBankAccountType.Checking;
			}
			else
			{
				return base.ConvertFrom(context,culture,value.ToString().Trim());
			}
		}
		/// <summary>
		/// <para>
		/// Overrides the ConvertTo method of TypeConverter for eBankAccountType
		/// </para>
		/// <para>
		/// If the value passed in is null and its destinationType is either string
		/// or char, then we will return a "C" or 'C' respectively to denote checking.
		/// </para>
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="val"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override object ConvertTo(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object val, 
			Type destinationType)
		{
			if((destinationType==typeof(string)
				||destinationType==typeof(char))
				&&val is eBankAccountType)
			{
				string ret=null;
				// determine what value to return
				if(eBankAccountType.Savings==(eBankAccountType)val) ret=Savings;
				else ret=Checking;
				// determine how to return it...as a char or a string
				if(destinationType==typeof(char)) return ret[0];
				else return ret;
			}
			else
			{
				return base.ConvertTo(context,culture,val,destinationType);
			}
		}
		#endregion public methods
	}
	#endregion converter class(es)
}
