using System;
using System.ComponentModel;

namespace Cox.BusinessObjects
{
    #region enumerations
    /// <summary>
    /// These are the data centers available in the web services interfaces.
    /// </summary>
    [TypeConverter(typeof(DataCenterConverter))]
    public enum DataCenter
    {
        /// <summary>
        /// This is provided when a data center is not known and cannot be determined.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// This represents the DATA1 AS400.
        /// </summary>
        DATA1 = 1,
        /// <summary>
        /// This represents the DATA2 AS400.
        /// </summary>
        DATA2 = 2,
        /// <summary>
        /// This represents the CENTRAL AS400.
        /// </summary>
        CENTRAL = 3
    }
    #endregion enumerations

    #region converter class(es)
    /// <summary>
    /// This class wraps the DataCenter enumeration and provides conversions 
    /// to/from DataCenter
    /// </summary>
    public class DataCenterConverter : EnumConverter
    {
        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public DataCenterConverter() : base(typeof(DataCenter)) { }
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
                int dataCenter = (int)value;

                // see if the value is defined w/in the enumeration
                if (Enum.IsDefined(typeof(DataCenter), dataCenter))
                {
                    return (DataCenter)dataCenter;
                }
                else
                {
                    return DataCenter.Unknown;
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
            if (destinationType == typeof(int) && value is DataCenter)
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
