using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumeration(s)
    /// <summary>
    /// Customer Type for customer.
    /// </summary>
    [TypeConverter(typeof(CustomerTypeConverter))]
    public enum CustomerType
    {
        /// <summary>Default customer type</summary>
        Default = 1,
        /// <summary>SSI Recipient</summary>
        SSIRecipient = 2,
        /// <summary>Senior Citizen</summary>
        SeniorCitizen = 3,
        /// <summary>Memo-Employee</summary>
        MemoEmployee = 4,
        /// <summary>Memo-VIP/Management</summary>
        MemoVIPManagement = 5,
        /// <summary>Memo Apartment Manager</summary>
        MemoAptManager = 6,
        /// <summary>Memo-Govt/Public</summary>
        MemoGovernmentPublic = 7,
        /// <summary>Memo-Reciprocal</summary>
        MemoReciprocal = 8,
        /// <summary>Special Use</summary>
        SpecialUse = 9,
        /// <summary>Sprint Client</summary>
        SprintClient = '@',
        /// <summary>Military Rates</summary>
        MilitaryRates = 'A',
        /// <summary>Commerical</summary>
        Commerical = 'C',
        /// <summary>Credit Risk/Refus</summary>
        CreditRiskRefus = 'N',
        /// <summary>ZPDI</summary>
        ZPDI = 'Q',
        /// <summary>Roommate</summary>
        Roommate = 'R',
        /// <summary>Employee</summary>
        Employee = 'V',
        /// <summary>VIP/Management</summary>
        VIPManagement = 'W',
        /// <summary>Apartment Manger</summary>
        AptManager = 'X',
        /// <summary>Government</summary>
        Government = 'Y',
        /// <summary>Reciprocal</summary>
        Reciprocal = 'Z'

    }

    #endregion enumeration(s)

    #region converter class(es)
    /// <summary>
    /// This class wraps the CustomerType enumeration and provides conversions 
    /// to/from CustomerType
    /// </summary>
    public class CustomerTypeConverter : EnumConverter
    {
        #region constants
        /// <summary>Default customer type</summary>
        public const string Default = "1";
        /// <summary>SSI Recipient customer type</summary>
        public const string SSIRecipient = "2";
        /// <summary>Senior Citizen customer type</summary>
        public const string SeniorCitizen = "3";
        /// <summary>Memo Employee customer type</summary>
        public const string MemoEmployee = "4";
        /// <summary>Memo VIP Management customer type</summary>
        public const string MemoVIPManagement = "5";
        /// <summary>Memo Appartment Manager customer type</summary>
        public const string MemoAptManager = "6";
        /// <summary>Memo Government Public customer type</summary>
        public const string MemoGovernmentPublic = "7";
        /// <summary>Memo Reciprocal customer type</summary>
        public const string MemoReciprocal = "8";
        /// <summary>Special Use customer type</summary>
        public const string SpecialUse = "9";
        /// <summary>Sprint Client customer type</summary>
        public const string SprintClient = "@";
        /// <summary>Militar yRates customer type</summary>
        public const string MilitaryRates = "A";
        /// <summary>Commerical customer type</summary>
        public const string Commerical = "C";
        /// <summary>Credit Risk Refuse customer type</summary>
        public const string CreditRiskRefus = "N";
        /// <summary>ZPDI customer type</summary>
        public const string ZPDI = "Q";
        /// <summary>Roommate customer type</summary>
        public const string Roommate = "R";
        /// <summary>Employee customer type</summary>
        public const string Employee = "V";
        /// <summary>VIP Management customer type</summary>
        public const string VIPManagement = "W";
        /// <summary>Apartment Manager customer type</summary>
        public const string AptManager = "X";
        /// <summary>Government customer type</summary>
        public const string Government = "Y";
        /// <summary>Reciprocal customer type</summary>
        public const string Reciprocal = "Z";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public CustomerTypeConverter() : base(typeof(CustomerType)) { }
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
                    case Default:
                        return CustomerType.Default;
                    case SSIRecipient:
                        return CustomerType.SSIRecipient;
                    case SeniorCitizen:
                        return CustomerType.SeniorCitizen;
                    case MemoEmployee:
                        return CustomerType.MemoEmployee;
                    case MemoVIPManagement:
                        return CustomerType.MemoVIPManagement;
                    case MemoAptManager:
                        return CustomerType.MemoAptManager;
                    case MemoGovernmentPublic:
                        return CustomerType.MemoGovernmentPublic;
                    case MemoReciprocal:
                        return CustomerType.MemoReciprocal;
                    case SpecialUse:
                        return CustomerType.SpecialUse;
                    case SprintClient:
                        return CustomerType.SprintClient;
                    case MilitaryRates:
                        return CustomerType.MilitaryRates;
                    case Commerical:
                        return CustomerType.Commerical;
                    case CreditRiskRefus:
                        return CustomerType.CreditRiskRefus;
                    case ZPDI:
                        return CustomerType.ZPDI;
                    case Roommate:
                        return CustomerType.Roommate;
                    case Employee:
                        return CustomerType.Employee;
                    case VIPManagement:
                        return CustomerType.VIPManagement;
                    case AptManager:
                        return CustomerType.AptManager;
                    case Government:
                        return CustomerType.Government;
                    case Reciprocal:
                        return CustomerType.Reciprocal;
                    default:
                        return CustomerType.Default;
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
                switch ((CustomerType)value)
                {
                    case CustomerType.Default:
                        return Default;
                    case CustomerType.SSIRecipient:
                        return SSIRecipient;
                    case CustomerType.SeniorCitizen:
                        return SeniorCitizen;
                    case CustomerType.MemoEmployee:
                        return MemoEmployee;
                    case CustomerType.MemoVIPManagement:
                        return MemoVIPManagement;
                    case CustomerType.MemoAptManager:
                        return MemoAptManager;
                    case CustomerType.MemoGovernmentPublic:
                        return MemoGovernmentPublic;
                    case CustomerType.MemoReciprocal:
                        return MemoReciprocal;
                    case CustomerType.SpecialUse:
                        return SpecialUse;
                    case CustomerType.SprintClient:
                        return SprintClient;
                    case CustomerType.MilitaryRates:
                        return MilitaryRates;
                    case CustomerType.Commerical:
                        return Commerical;
                    case CustomerType.CreditRiskRefus:
                        return CreditRiskRefus;
                    case CustomerType.ZPDI:
                        return ZPDI;
                    case CustomerType.Roommate:
                        return Roommate;
                    case CustomerType.Employee:
                        return Employee;
                    case CustomerType.VIPManagement:
                        return VIPManagement;
                    case CustomerType.AptManager:
                        return AptManager;
                    case CustomerType.Government:
                        return Government;
                    case CustomerType.Reciprocal:
                        return Reciprocal;
                    default:
                        return Default;
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
