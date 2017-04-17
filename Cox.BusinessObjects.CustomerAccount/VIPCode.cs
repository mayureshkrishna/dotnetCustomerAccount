using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerAccount
{

    #region enumerations
    /// <summary>
    /// These are the VIP Codes available in the web services interfaces.
    /// </summary>
    [TypeConverter(typeof(VIPCodeConverter))]
    public enum VIPCode
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// This represents Pending Bankrupty.
        /// </summary>
        PendingBankRuptcy = 'B',
        /// <summary>
        /// This represents City Official.
        /// </summary>
        CityOfficial = 'C',
        /// <summary>
        /// This represents Developers.
        /// </summary>
        Developers = 'D',
        /// <summary>
        /// This represents Commercial.
        /// </summary>
        Commercial = 'E',
        /// <summary>
        /// This represents Military.
        /// </summary>
        Military = 'F',
        /// <summary>
        /// This represents Home Owners Association.
        /// </summary>
        HomeOwnersAssoc = 'H',
        /// <summary>
        /// This represents CBI/CCI Internal Account.
        /// </summary>
        CBSCCIInternalAccount = 'I',
        /// <summary>
        /// This represents Customer Disputed Toll.
        /// </summary>
        CustomerDisputedToll = 'J',
        /// <summary>
        /// This represents Special Use.
        /// </summary>
        SpecialUse = 'L',
        /// <summary>
        /// This represents Mayor.
        /// </summary>
        Mayor = 'M',
        /// <summary>
        /// This represents National Accounts.
        /// </summary>
        NationalAccounts = 'N',
        /// <summary>
        /// This represents City Official.
        /// </summary>
        CityOfficials = 'O',
        /// <summary>
        /// This represents Profession Courtesy.
        /// </summary>
        ProfessionalCourtesy = 'P',
        /// <summary>
        /// This represents Digital Beta Tester.
        /// </summary>
        DigitalBetaTester = 'R',
        /// <summary>
        /// This represents HSD Beta Tester.
        /// </summary>
        HSDBetaTester = 'S',
        /// <summary>
        /// This represents Telephone Beta Tester.
        /// </summary>
        TelephoneBetaTester = 'T',
        /// <summary>
        /// This represents Digital Telephone Beta Tester.
        /// </summary>
        DigitalTelephoneBetaTester = 'U',
        /// <summary>
        /// This represents VIP.
        /// </summary>
        VIP = 'V',
    }
    #endregion enumerations

    #region converter class(es)
    /// <summary>
    /// This class wraps the VIPCode enumeration and provides conversions 
    /// to/from VIPCode - EnumConverter
    /// </summary>
    public class VIPCodeConverter : EnumConverter
    {
        #region constants
        /// <summary>
        /// Value sent to ICOMS for a customer whose status is unknown.
        /// </summary>
        public const string Unknown = "-1";
        /// <summary>
        /// Value sent to ICOMS for a customer whose status is Pending BankRuptcy.
        /// </summary>
        public const string PendingBankRuptcy = "B";
        /// <summary>
        /// Value sent to ICOMS for City Official.
        /// </summary>
        public const string CityOfficial = "C";
        /// <summary>
        /// Value sent to ICOMS for Developer.
        /// </summary>
        public const string Developers = "D";
        /// <summary>
        /// Value sent to ICOMS for Commercial.
        /// </summary>
        public const string Commercial = "E";
        /// <summary>
        /// Value sent to ICOMS for Military.
        /// </summary>
        public const string Military = "F";
        /// <summary>
        /// Value sent to ICOMS for Home Owners Assoc.
        /// </summary>
        public const string HomeOwnersAssoc = "H";
        /// <summary>
        /// Value sent to ICOMS for CBS/CCI Internal Account.
        /// </summary>
        public const string CBSCCIInternalAccount = "I";
        /// <summary>
        /// Value sent to ICOMS for Customer Disputed Toll.
        /// </summary>
        public const string CustomerDisputedToll = "J";
        /// <summary>
        /// Value sent to ICOMS for Special Use.
        /// </summary>
        public const string SpecialUse = "L";
        /// <summary>
        /// Value sent to ICOMS for Mayor.
        /// </summary>
        public const string Mayor = "M";
        /// <summary>
        /// Value sent to ICOMS for National Accounts.
        /// </summary>
        public const string NationalAccounts = "N";
        /// <summary>
        /// Value sent to ICOMS for City Official.
        /// </summary>
        public const string CityOfficials = "O";
        /// <summary>
        /// Value sent to ICOMS for Professional Courtesy.
        /// </summary>
        public const string ProfessionalCourtesy = "P";
        /// <summary>
        /// Value sent to ICOMS for Digital Beta Tester.
        /// </summary>
        public const string DigitalBetaTester = "R";
        /// <summary>
        /// Value sent to ICOMS for HSD Beta Tester.
        /// </summary>
        public const string HSDBetaTester = "S";
        /// <summary>
        /// Value sent to ICOMS for Telephone Beta Tester.
        /// </summary>
        public const string TelephoneBetaTester = "T";
        /// <summary>
        /// Value sent to ICOMS for Digital Telephone Beta Tester.
        /// </summary>
        public const string DigitalTelephoneBetaTester = "U";
        /// <summary>
        /// Value sent to ICOMS for VIP.
        /// </summary>
        public const string VIP = "V";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public VIPCodeConverter() : base(typeof(VIPCode)) { }
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
                    case PendingBankRuptcy:
                        return VIPCode.PendingBankRuptcy;
                    case CityOfficial:
                    case CityOfficials:
                        return VIPCode.CityOfficial;
                    case Developers:
                        return VIPCode.Developers;
                    case Commercial:
                        return VIPCode.Commercial;
                    case Military:
                        return VIPCode.Military;
                    case HomeOwnersAssoc:
                        return VIPCode.HomeOwnersAssoc;
                    case CBSCCIInternalAccount:
                        return VIPCode.CBSCCIInternalAccount;
                    case CustomerDisputedToll:
                        return VIPCode.CustomerDisputedToll;
                    case SpecialUse:
                        return VIPCode.SpecialUse;
                    case Mayor:
                        return VIPCode.Mayor;
                    case NationalAccounts:
                        return VIPCode.NationalAccounts;
                    //case CityOfficials:
                      //  return VIPCode.CityOfficials;
                    case ProfessionalCourtesy:
                        return VIPCode.ProfessionalCourtesy;
                    case DigitalBetaTester:
                        return VIPCode.DigitalBetaTester;
                    case HSDBetaTester:
                        return VIPCode.HSDBetaTester;
                    case TelephoneBetaTester:
                        return VIPCode.TelephoneBetaTester;
                    case DigitalTelephoneBetaTester:
                        return VIPCode.DigitalTelephoneBetaTester;
                    case VIP:
                        return VIPCode.VIP;
                    default:
                        return VIPCode.Unknown;
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
            if (destinationType == typeof(string) && value is VIPCode)
            {
                switch ((VIPCode)value)
                {
                    case VIPCode.PendingBankRuptcy:
                        return PendingBankRuptcy;
                    case VIPCode.CityOfficial:
                        return CityOfficial;
                    case VIPCode.Developers:
                        return Developers;
                    case VIPCode.Commercial:
                        return Commercial;
                    case VIPCode.Military:
                        return Military;
                    case VIPCode.HomeOwnersAssoc:
                        return HomeOwnersAssoc;
                    case VIPCode.CBSCCIInternalAccount:
                        return CBSCCIInternalAccount;
                    case VIPCode.CustomerDisputedToll:
                        return CustomerDisputedToll;
                    case VIPCode.SpecialUse:
                        return SpecialUse;
                    case VIPCode.Mayor:
                        return Mayor;
                    case VIPCode.NationalAccounts:
                        return NationalAccounts;
                    case VIPCode.CityOfficials:
                        return CityOfficials;
                    case VIPCode.ProfessionalCourtesy:
                        return ProfessionalCourtesy;
                    case VIPCode.DigitalBetaTester:
                        return DigitalBetaTester;
                    case VIPCode.HSDBetaTester:
                        return HSDBetaTester;
                    case VIPCode.TelephoneBetaTester:
                        return TelephoneBetaTester;
                    case VIPCode.DigitalTelephoneBetaTester:
                        return DigitalTelephoneBetaTester;
                    case VIPCode.VIP:
                        return VIP;
                    default:
                        return Unknown;
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
