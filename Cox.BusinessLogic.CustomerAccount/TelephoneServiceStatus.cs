using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerAccount
{
    /// <summary>
    /// List of Cable Serviceable Status
    /// </summary>
    [TypeConverter(typeof(TelephoneServiceStatusConverter))]
    [XmlTypeAttribute("TelephoneServiceStatus")]
    public enum TelephoneServiceStatus
    {
        /// <summary>
        /// Unknown 
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// Telephone not serviceable = 'TN'
        /// </summary>
        TelephoneNotServiceable = 1,
        /// <summary>
        /// Telephone Hybrid = 'TH'
        /// </summary>
        TelephoneHybrid = 2,
        /// <summary>
        /// Telephone Serviceable = 'TY'
        /// </summary>
        TelephoneServiceable = 3,
        /// <summary>
        /// Telephone VOIP serviceable = 'VY'
        /// </summary>
        TelephoneVOIPServiceable = 4
        
    }//enum TelephoneServiceStatus

    #region converter class(es)
    /// <summary>
    /// This class wraps the TelephoneServiceStatus enumeration and provides conversions 
    /// to/from TelephoneServiceStatus
    /// </summary>
    public class TelephoneServiceStatusConverter : EnumConverter
    {
        #region constants
        /// <summary>
        /// Unknown
        /// </summary>
        public const string Unknown = "Unknown";
        /// <summary>
        /// Telephone Not Serviceable = 'TN'
        /// </summary>
        public const string TelephoneNotServiceable = "TN";
        /// <summary>
        /// Telephone Hybrid = 'TH'
        /// </summary>
        public const string TelephoneHybrid = "TH";
        /// <summary>
        /// Telephone Serviceable = 'TY'
        /// </summary>
        public const string TelephoneServiceable = "TY";
        /// <summary>
        /// Telephone VOIP Serviceable = 'VY'
        /// </summary>
        public const string TelephoneVOIPServiceable = "VY";
        
        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public TelephoneServiceStatusConverter() : base(typeof(TelephoneServiceStatus)) { }
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
                    case TelephoneNotServiceable:
                        return TelephoneServiceStatus.TelephoneNotServiceable;
                    case TelephoneHybrid:
                        return TelephoneServiceStatus.TelephoneHybrid;
                    case TelephoneServiceable:
                        return TelephoneServiceStatus.TelephoneServiceable;
                    case TelephoneVOIPServiceable:
                        return TelephoneServiceStatus.TelephoneVOIPServiceable;
                    default:
                        return TelephoneServiceStatus.Unknown;
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
            if (destinationType == typeof(string) && value is TelephoneServiceStatus)
            {
                switch ((TelephoneServiceStatus)value)
                {
                    case TelephoneServiceStatus.TelephoneNotServiceable:
                        return TelephoneNotServiceable;
                    case TelephoneServiceStatus.TelephoneHybrid:
                        return TelephoneHybrid;
                    case TelephoneServiceStatus.TelephoneServiceable:
                        return TelephoneServiceable;
                    case TelephoneServiceStatus.TelephoneVOIPServiceable:
                        return TelephoneVOIPServiceable;
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

