using System;

using System.ComponentModel;


namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumeration(s)
    /// <summary>
    /// Account status for customer.
    /// </summary>
    [TypeConverter(typeof(AccountStatusConverter))]
    public enum AccountStatus
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 'U',
        /// <summary>
        /// This is provided when a type is not known and cannot be determined.
        /// </summary>
        Active = 'A',
        /// <summary>
        /// This is used to indicate payment failure.
        /// </summary>
        Former = 'F'

    }

    #endregion enumeration(s)

    #region converter class(es)
    /// <summary>
    /// This class wraps the AccountStatus enumeration and provides conversions 
    /// to/from AccountStatus - EnumConverter
    /// </summary>
    public class AccountStatusConverter : EnumConverter
    {
        #region constants
        /// <summary>
        /// Value sent to ICOMS for a customer whose status is unknown.
        /// </summary>
        public const string Unknown = "U";
        /// <summary>
        /// Value sent to ICOMS for a active customer.
        /// </summary>
        public const string Active = "A";
        /// <summary>
        /// Value sent to ICOMS for a former customer.
        /// </summary>
        public const string Former = "F";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public AccountStatusConverter() : base(typeof(AccountStatus)) { }
        #endregion ctors

        #region public methods

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
            //return base.CanConvertFrom(context, sourceType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return true;
            //return base.IsValid(context, value);
        }

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
                    case AccountStatusConverter.Unknown:
                        return AccountStatus.Unknown;
                    case AccountStatusConverter.Active:
                        return AccountStatus.Active;
                    case AccountStatusConverter.Former:
                        return AccountStatus.Former;
                    default:
                        return AccountStatus.Unknown;
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
            if (destinationType == typeof(string) && value is AccountStatus)
            {
                switch ((AccountStatus)value)
                {
                    case AccountStatus.Unknown:
                        return AccountStatusConverter.Unknown;
                    case AccountStatus.Active:
                        return AccountStatusConverter.Active;
                    case AccountStatus.Former:
                        return AccountStatusConverter.Former;
                    default:
                        return string.Empty;
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