using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cox.BusinessObjects
{
	#region enumerations
	/// <summary>
	/// Service categories describe the category of service on a statement or account.
	/// </summary>
	[TypeConverter(typeof(ServiceCategoryConverter))]
	[XmlTypeAttribute("ServiceCategory")]
	public enum eServiceCategory
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown			= -1,
		/// <summary>
		/// This represents standard or digital cable services.
		/// </summary>
		Cable			= 1,
		/// <summary>
		/// This represents high speed data services.
		/// </summary>
		Data			= 2,
		/// <summary>
		/// This represents telephony services.
		/// </summary>
		Telephone		= 3,
		/// <summary>
		/// This represents calling card services.
		/// </summary>
		CallingCard		= 4,
        /// <summary>
        /// This represents home security services.
        /// </summary>
        // Feb 2012 CHS service category addition project
        HomeSecurity = 5,
	}
	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// This class wraps the eServiceCategory enumeration and provides conversions 
	/// to/from eServiceCategory
	/// </summary>
	public class ServiceCategoryConverter:EnumConverter
	{
		#region constants
		/// <summary>
		/// Value sent to ICOMS for.....a ServiceCategory of Cable
		/// </summary>
		public const string Cable="C";
		/// <summary>
		/// Value sent to ICOMS for.....a ServiceCategory of Data
		/// </summary>
		public const string Data="D";
		/// <summary>
		/// Value sent to ICOMS for.....a ServiceCategory of Telephone
		/// </summary>
		public const string Telephone="T";
		/// <summary>
		/// Value sent to ICOMS for.....a ServiceCategory of CallingCard
		/// </summary>
		public const string CallingCard="L";
        /// <summary>
        /// Value sent to ICOMS for.....a ServiceCategory of HomeSecurity
        /// </summary>
        // Feb 2012 CHS service category addition project
        public const string HomeSecurity = "H";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public ServiceCategoryConverter ():base(typeof(eServiceCategory)){}
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
				string localCategory = ((string)value).Trim().ToUpper();
				// ok...what do we have.
				switch(localCategory)
				{
					case Cable:
						return eServiceCategory.Cable;
					case Data:
						return eServiceCategory.Data;
					case Telephone:
						return eServiceCategory.Telephone;
					case CallingCard:
						return eServiceCategory.CallingCard;
                    // Feb 2012 CHS service category addition project
                    case HomeSecurity:
                        return eServiceCategory.HomeSecurity;
                    // End of changes CHS
					default:
						return eServiceCategory.Unknown;
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
			if(destinationType==typeof(string) && value is eServiceCategory)
			{
				switch((eServiceCategory)value)
				{
					case eServiceCategory.Cable:
						return Cable;
					case eServiceCategory.Data:
						return Data;
					case eServiceCategory.Telephone:
						return Telephone;
					case eServiceCategory.CallingCard:
						return CallingCard;
                    // Feb 2012 CHS service category addition project
                    case eServiceCategory.HomeSecurity:
                        return HomeSecurity;
                    // End of changes CHS
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
