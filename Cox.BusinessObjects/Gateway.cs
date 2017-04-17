using System;
using System.ComponentModel;

namespace Cox.BusinessObjects
{
    #region enumerations
    /// <summary>
    /// These are the gateways available in the web services interfaces.
    /// </summary>
    [TypeConverter(typeof(GatewayConverter))]
    public enum Gateway
    {
        /// <summary>
        /// This is provided when a gateway is not known and cannot be determined.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// This represents the Connection Manager gateway.
        /// </summary>
        ConnectionManager = 1,
        /// <summary>
        /// This represents the Transidiom gateway.
        /// </summary>
        Transidiom = 2
    }
    #endregion enumerations

    #region converter class(es)
    /// <summary>
    /// This class wraps the Gateway enumeration and provides conversions 
    /// to/from Gateway
    /// </summary>
    public class GatewayConverter : EnumConverter
    {
        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public GatewayConverter() : base(typeof(Gateway)) { }
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
            if (value is int)
            {
                // setup default value
                int gateway = (int)value;

                // see if the value is defined w/in the enumeration
                if (Enum.IsDefined(typeof(Gateway), gateway))
                {
                    return (Gateway)gateway;
                }
                else
                {
                    return Gateway.Unknown;
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
            System.Globalization.CultureInfo culture, object value,
            Type destinationType)
        {
            // here we want a chance at it
            if (destinationType == typeof(int) && value is Gateway)
            {
                return (int)value;
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
