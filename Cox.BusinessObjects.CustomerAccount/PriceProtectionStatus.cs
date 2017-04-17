using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumeration(s)
    /// <summary>
    /// Price Protection status for service code.
    /// </summary>
    [TypeConverter(typeof(PriceProtectionStatusConverter))]
    public enum ePriceProtectionStatus
    {
        /// <summary>
        /// This is provided when a type is not known and cannot be determined.
        /// </summary>
        Unknown = 'U',
        /// <summary>
        /// Pending Reinstate price protection status
        /// </summary>
        PendingReinstate = 'R',
        /// <summary>
        /// Active price protection atatus
        /// </summary>
        Active = 'A',
        /// <summary>
        /// Pending Failure price protection status
        /// </summary>
        PendingFailure = 'F',
        /// <summary>
        /// Pending Install price protection status
        /// </summary>
        PendingInstall = 'I',
        /// <summary>
        /// Pending Disconnect price protection status
        /// </summary>
        PendingDisconnect = 'D',
        /// <summary>
        /// Disconnected price protection status
        /// </summary>
        Disconnected = 'X'

    }

    #endregion enumeration(s)

    #region converter class(es)
    /// <summary>
    /// This class wraps the PriceProtectionStatus enumeration and provides conversions 
    /// to/from ePriceProtectionStatus - EnumConverter
    /// </summary>
    public class PriceProtectionStatusConverter : EnumConverter
    {
        #region constants
        /// <summary>
        /// Price protection status value when status is not known.
        /// </summary>
        public const string Unknown = "U";
        /// <summary>
        /// Value sent to ICOMS for a Pending Reinstate price protection status
        /// </summary>
        public const string PendingReinstate = "R";
        /// <summary>
        /// Value sent to ICOMS for a Active price protection status
        /// </summary>
        public const string Active = "A";
        /// <summary>
        /// Value sent to ICOMS for a Pending Failure price protection status
        /// </summary>
        public const string PendingFailure = "F";
        /// <summary>
        /// Value sent to ICOMS for a Pending Install price protection status
        /// </summary>
        public const string PendingInstall = "I"; 
        /// <summary>
        /// Value sent to ICOMS for a Pending Disconnect price protection status
        /// </summary>
        public const string PendingDisconnect = "D"; 
        /// <summary>
        /// Value sent to ICOMS for a Disconnected price protection status
        /// </summary>
        public const string Disconnected = "X";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public PriceProtectionStatusConverter() : base(typeof(ePriceProtectionStatus)) { }
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
                    case PriceProtectionStatusConverter.Unknown:
                        return ePriceProtectionStatus.Unknown;
                    case PriceProtectionStatusConverter.PendingReinstate:
                        return ePriceProtectionStatus.PendingReinstate;
                    case PriceProtectionStatusConverter.Active:
                        return ePriceProtectionStatus.Active;
                    case PriceProtectionStatusConverter.PendingFailure:
                        return ePriceProtectionStatus.PendingFailure;
                    case PriceProtectionStatusConverter.PendingInstall:
                        return ePriceProtectionStatus.PendingInstall;
                    case PriceProtectionStatusConverter.PendingDisconnect:
                        return ePriceProtectionStatus.PendingDisconnect;
                    case PriceProtectionStatusConverter.Disconnected:
                        return ePriceProtectionStatus.Disconnected;
                    default:
                        return ePriceProtectionStatus.Unknown;
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
            if (destinationType == typeof(string) && value is ePriceProtectionStatus)
            {
                switch ((ePriceProtectionStatus)value)
                {
                    case ePriceProtectionStatus.Unknown:
                        return PriceProtectionStatusConverter.Unknown;
                    case ePriceProtectionStatus.PendingReinstate:
                        return PriceProtectionStatusConverter.PendingReinstate;
                    case ePriceProtectionStatus.Active:
                        return PriceProtectionStatusConverter.Active;
                    case ePriceProtectionStatus.PendingFailure:
                        return PriceProtectionStatusConverter.PendingFailure;
                    case ePriceProtectionStatus.PendingInstall:
                        return PriceProtectionStatusConverter.PendingInstall;
                    case ePriceProtectionStatus.PendingDisconnect:
                        return PriceProtectionStatusConverter.PendingDisconnect;
                    case ePriceProtectionStatus.Disconnected:
                        return PriceProtectionStatusConverter.Disconnected;
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
