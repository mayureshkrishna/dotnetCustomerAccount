using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumeration(s)
    /// <summary>
    /// Phone Type for PhoneDetail.
    /// </summary>
    [TypeConverter(typeof(PhoneTypeConverter))]
    public enum PhoneType
    {
        /// <summary>Home Phone</summary>
        Home = 'H',
        /// <summary>Buisness Phone</summary>
        Business = 'B',
        /// <summary>Other Phone</summary>
        Other = 'O',
        /// <summary>Wireless Phone</summary>
        Wireless = 'W'
    }

    #endregion enumeration(s)

    #region converter class(es)
    /// <summary>
    /// This class wraps the PhoneType enumeration and provides conversions 
    /// to/from PhoneType
    /// </summary>
    public class PhoneTypeConverter : EnumConverter
    {
        #region constants

        /// <summary>Home Phone</summary>
        public const string Home = "H";
        /// <summary>Business Phone</summary>
        public const string Business = "B";
        /// <summary>Other Phone</summary>
        public const string Other = "O";
        /// <summary>Wireless Phone</summary>
        public const string Wireless = "W";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public PhoneTypeConverter() : base(typeof(PhoneType)) { }
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
            if (value as string != null)
            {
                string localValue = ((string)value).Trim().ToUpper();
                //evaluate
                switch (localValue)
                {
                    case Home:
                        return PhoneType.Home;
                    case Business:
                        return PhoneType.Business;
                    case Other:
                        return PhoneType.Other;
                    case Wireless:
                        return PhoneType.Wireless;
                    default:
                        return PhoneType.Home;
                }
            }
            else
            {
                // we don't want it but the base class might.
                return base.ConvertFrom(context, culture, value);
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
            if (destinationType == typeof(string) && value is DwellingType)
            {
                switch ((PhoneType)value)
                {
                    case PhoneType.Home:
                        return Home;
                    case PhoneType.Business:
                        return Business;
                    case PhoneType.Other:
                        return Other;
                    case PhoneType.Wireless:
                        return Wireless;
                    default:
                        return Home;
                }
            }
            else
            {
                // we don't want it but the base class might.
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        #endregion public methods
    }
    #endregion converter class(es)
}
