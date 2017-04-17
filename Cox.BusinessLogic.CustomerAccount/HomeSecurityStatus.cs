using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerAccount
{
    /// <summary>
    /// List of Cable Serviceable Status
    /// </summary>
    [TypeConverter(typeof(HomeSecurityServiceStatusConverter))]
    [XmlTypeAttribute("HomeSecurityServiceStatus")]
    public enum HomeSecurityServiceStatus
    {
        /// <summary>
        /// Unknown 
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Home Security Serviceable = 'HY'
        /// </summary>
        HomeSecurityServiceable = 1,
        /// <summary>
        /// Home Security Not Serviceable = 'HN'
        /// </summary>
        HomeSecurityNotServiceable = 2
    }//enum HomeSecurityServiceStatus

    #region converter class(es)
    /// <summary>
    /// This class wraps the HomeSecurityServiceStatus enumeration and provides conversions 
    /// to/from HomeSecurityServiceStatus
    /// </summary>
    public class HomeSecurityServiceStatusConverter : EnumConverter
    {
        #region constants
        /// <summary>
        /// Unknown
        /// </summary>
        public const string Unknown = "Unknown";
        /// <summary>
        /// Home Security Serviceable = 'HY'
        /// </summary>
        public const string HomeSecurityServiceable = "HY";
        /// <summary>
        /// HomeSecurity Not Serviceable = 'HN'
        /// </summary>
        public const string HomeSecurityNotServiceable = "HN";
        

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public HomeSecurityServiceStatusConverter() : base(typeof(HomeSecurityServiceStatus)) { }
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
                                            System.Globalization.CultureInfo culture,
                                            object value)
        {
            if (value as string != null)
            {
                string localCategory = ((string)value).Trim().ToUpper();

                switch (localCategory)
                {
                    case HomeSecurityServiceable:
                        return HomeSecurityServiceStatus.HomeSecurityServiceable;
                    case HomeSecurityNotServiceable:
                        return HomeSecurityServiceStatus.HomeSecurityNotServiceable;
                    default:
                        return HomeSecurityServiceStatus.Unknown;
                }
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
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
                                          System.Globalization.CultureInfo culture,
                                          object value,
                                          Type destinationType)
        {
            if (destinationType == typeof(string) && value is HomeSecurityServiceStatus)
            {
                switch ((HomeSecurityServiceStatus)value)
                {
                    case HomeSecurityServiceStatus.HomeSecurityServiceable:
                        return HomeSecurityServiceable;
                    case HomeSecurityServiceStatus.HomeSecurityNotServiceable:
                        return HomeSecurityNotServiceable;
                    default:
                        return string.Empty;
                }
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        #endregion public methods
    }
    #endregion converter class(es)
}//namespace Cox.BusinessObjects.CustomerAccount

