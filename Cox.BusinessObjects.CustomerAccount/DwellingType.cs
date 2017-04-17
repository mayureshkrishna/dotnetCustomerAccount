using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumeration(s)
    /// <summary>
    /// House Dwelling Type for customer.
    /// </summary>
    [TypeConverter(typeof(DwellingTypeConverter))]
    public enum DwellingType
    {
        /// <summary>Rev ShareApt</summary>
        RevShareApt = 1,
        /// <summary>Rev Share Condo</summary>
        RevShareCondo = 2,
        /// <summary>Rev Share Moble Home</summary>
        RevShareMobleHome = 3,
        /// <summary>Rev Share MDU</summary>
        RevShareMDU = 4,
        /// <summary>Rev Share BUS</summary>
        RevShareBUS = 5,
        /// <summary>Apartment</summary>
        Apartment = 'A',
        /// <summary>Business</summary>
        Business = 'B',
        /// <summary>Condominium</summary>
        Condominium = 'C',
        /// <summary>Hospital</summary>
        Hospital = 'D',
        /// <summary>Church</summary>
        Church = 'E',
        /// <summary>Retirement Home</summary>
        RetirementHome = 'F',
        /// <summary>Government/Public</summary>
        GovtPublic = 'G',
        /// <summary>House</summary>
        House = 'H',
        /// <summary>Organization</summary>
        Organization = 'I',
        /// <summary>Golf Facility</summary>
        GolfFacility = 'J',
        /// <summary>Fire Station</summary>
        FireStation = 'K',
        /// <summary>Slip</summary>
        Slip = 'L',
        /// <summary>Motel / Hotel</summary>
        MotelHotel = 'M',
        /// <summary>Dorm / Barracks</summary>
        DormBarracks = 'N',
        /// <summary>Nursing Home</summary>
        NursingHome = 'O',
        /// <summary>Police</summary>
        Police = 'P',
        /// <summary>Tavern / Bar</summary>
        TavernBar = 'Q',
        /// <summary>Restaurant</summary>
        Restaurant = 'R',
        /// <summary>School</summary>
        School = 'S',
        /// <summary>Mobile Home</summary>
        MobileHome = 'T',
        /// <summary>Townhouse</summary>
        Townhouse = 'U',
        /// <summary>Vacant Lot</summary>
        VacantLot = 'V',
        /// <summary>Home / Office</summary>
        HomeOffice = 'W',
        /// <summary>Airport</summary>
        Airport = 'Y'

    }


    //[23-09-2009] Start Changes for Current Campaign Data fetch request

    /// <summary>
    ///  Service status
    /// </summary>
    [TypeConverter(typeof(ServiceStatusConverter))]
    public enum ServiceStatus
    {
        /// <summary>Unknown</summary>
        Unknown = 0,
        /// <summary>Service is Active</summary>
        Active = 1,
        /// <summary>Service is Disconnected</summary>
        Disconnected = 2
        
    }

    /// <summary>
    ///  Discount Active or not
    /// </summary>
    [TypeConverter(typeof(DiscountActiveConverter))]
    public enum DiscountActive
    {
        /// <summary>Unknown</summary>
        Unknown = 0,
        /// <summary>Discount is active</summary>
        Yes = 1,
        /// <summary>Discount is not active </summary>
        No = 2,
        
    }

    //[23-09-2009] End Changes for Current Campaign Data fetch request

    #endregion enumeration(s)

    #region converter class(es)
    /// <summary>
    /// This class wraps the AccountStatus enumeration and provides conversions 
    /// to/from AccountStatus
    /// </summary>
    public class DwellingTypeConverter : EnumConverter
    {
        #region constants
        /// <summary>Rev ShareApt</summary>
        public const string RevShareApt = "1";
        /// <summary>Rev Share Condo</summary>
        public const string RevShareCondo = "2";
        /// <summary>Rev Share Moble Home</summary>
        public const string RevShareMobleHome = "3";
        /// <summary>Rev Share MDU</summary>
        public const string RevShareMDU = "4";
        /// <summary>Rev Share BUS</summary>
        public const string RevShareBUS = "5";
        /// <summary>Apartment</summary>
        public const string Apartment = "A";
        /// <summary>Business</summary>
        public const string Business = "B";
        /// <summary>Condominium</summary>
        public const string Condominium = "C";
        /// <summary>Hospital</summary>
        public const string Hospital = "D";
        /// <summary>Church</summary>
        public const string Church = "E";
        /// <summary>Retirement Home</summary>
        public const string RetirementHome = "F";
        /// <summary>GovtPublic</summary>
        public const string GovtPublic = "G";
        /// <summary>House</summary>
        public const string House = "H";
        /// <summary>Organization</summary>
        public const string Organization = "I";
        /// <summary>Golf Facility</summary>
        public const string GolfFacility = "J";
        /// <summary>Fire Station</summary>
        public const string FireStation = "K";
        /// <summary>Slip</summary>
        public const string Slip = "L";
        /// <summary>Motel or Hotel</summary>
        public const string MotelHotel = "M";
        /// <summary>Dorm or Barracks</summary>
        public const string DormBarracks = "N";
        /// <summary>Nursing Home</summary>
        public const string NursingHome = "O";
        /// <summary>Police</summary>
        public const string Police = "P";
        /// <summary>Tavern or Bar</summary>
        public const string TavernBar = "Q";
        /// <summary>Restaurant</summary>
        public const string Restaurant = "R";
        /// <summary>School</summary>
        public const string School = "S";
        /// <summary>Mobile Home</summary>
        public const string MobileHome = "T";
        /// <summary>Townhouse</summary>
        public const string Townhouse = "U";
        /// <summary>Vacant Lot</summary>
        public const string VacantLot = "V";
        /// <summary>Home Office</summary>
        public const string HomeOffice = "W";
        /// <summary>Airport</summary>
        public const string Airport = "Y";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public DwellingTypeConverter() : base(typeof(DwellingType)) { }
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
                    case RevShareApt:
                        return DwellingType.RevShareApt;
                    case RevShareCondo:
                        return DwellingType.RevShareCondo;
                    case RevShareMobleHome:
                        return DwellingType.RevShareMobleHome;
                    case RevShareMDU:
                        return DwellingType.RevShareMDU;
                    case RevShareBUS:
                        return DwellingType.RevShareBUS;
                    case Apartment:
                        return DwellingType.Apartment;
                    case Business:
                        return DwellingType.Business;
                    case Condominium:
                        return DwellingType.Condominium;
                    case Hospital:
                        return DwellingType.Hospital;
                    case Church:
                        return DwellingType.Church;
                    case RetirementHome:
                        return DwellingType.RetirementHome;
                    case GovtPublic:
                        return DwellingType.GovtPublic;
                    case Organization:
                        return DwellingType.Organization;
                    case GolfFacility:
                        return DwellingType.GolfFacility;
                    case FireStation:
                        return DwellingType.FireStation;
                    case Slip:
                        return DwellingType.Slip;
                    case MotelHotel:
                        return DwellingType.MotelHotel;
                    case DormBarracks:
                        return DwellingType.DormBarracks;
                    case NursingHome:
                        return DwellingType.NursingHome;
                    case Police:
                        return DwellingType.Police;
                    case TavernBar:
                        return DwellingType.TavernBar;
                    case Restaurant:
                        return DwellingType.Restaurant;
                    case School:
                        return DwellingType.School;
                    case MobileHome:
                        return DwellingType.MobileHome;
                    case Townhouse:
                        return DwellingType.Townhouse;
                    case VacantLot:
                        return DwellingType.VacantLot;
                    case HomeOffice:
                        return DwellingType.HomeOffice;
                    case Airport:
                        return DwellingType.Airport;
                    default:
                        return DwellingType.House;
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
                switch ((DwellingType)value)
                {
                    case DwellingType.RevShareApt:
                        return RevShareApt;
                    case DwellingType.RevShareCondo:
                        return RevShareCondo;
                    case DwellingType.RevShareMobleHome:
                        return RevShareMobleHome;
                    case DwellingType.RevShareMDU:
                        return RevShareMDU;
                    case DwellingType.RevShareBUS:
                        return RevShareBUS;
                    case DwellingType.Apartment:
                        return Apartment;
                    case DwellingType.Business:
                        return Business;
                    case DwellingType.Condominium:
                        return Condominium;
                    case DwellingType.Hospital:
                        return Hospital;
                    case DwellingType.Church:
                        return Church;
                    case DwellingType.RetirementHome:
                        return RetirementHome;
                    case DwellingType.GovtPublic:
                        return GovtPublic;
                    case DwellingType.Organization:
                        return Organization;
                    case DwellingType.GolfFacility:
                        return GolfFacility;
                    case DwellingType.FireStation:
                        return FireStation;
                    case DwellingType.Slip:
                        return Slip;
                    case DwellingType.MotelHotel:
                        return MotelHotel;
                    case DwellingType.DormBarracks:
                        return DormBarracks;
                    case DwellingType.NursingHome:
                        return NursingHome;
                    case DwellingType.Police:
                        return Police;
                    case DwellingType.TavernBar:
                        return TavernBar;
                    case DwellingType.Restaurant:
                        return Restaurant;
                    case DwellingType.School:
                        return School;
                    case DwellingType.MobileHome:
                        return MobileHome;
                    case DwellingType.Townhouse:
                        return Townhouse;
                    case DwellingType.VacantLot:
                        return VacantLot;
                    case DwellingType.HomeOffice:
                        return HomeOffice;
                    case DwellingType.Airport:
                        return Airport;
                    default:
                        return House;
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

    //[23-09-2009] Start Changes for Current Campaign Data fetch request

    public class ServiceStatusConverter : EnumConverter
    {
        #region constants
        /// <summary>Active</summary>
        public const string Active = "A";
        /// <summary>Disconnected</summary>
        public const string Disconnected = "D";
        /// <summary>Unknown</summary>
        public const string Unknown = "";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public ServiceStatusConverter() : base(typeof(ServiceStatus)) { }
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
                    case Active:
                        return ServiceStatus.Active;
                    case Disconnected:
                        return ServiceStatus.Disconnected;
                    default:
                        return ServiceStatus.Unknown;
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
            if (destinationType == typeof(string) && value is ServiceStatus)
            {
                switch ((ServiceStatus)value)
                {
                    case ServiceStatus.Active:
                        return Active;
                    case ServiceStatus.Disconnected:
                        return Disconnected;
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


    public class DiscountActiveConverter : EnumConverter
    {
        #region constants
        /// <summary>Yes</summary>
        public const string Yes = "Y";
        /// <summary>No</summary>
        public const string No = "N";
        /// <summary>Unknown</summary>
        public const string Unknown = "";

        #endregion constants

        #region ctors
        /// <summary>
        /// The default constructor
        /// </summary>
        public DiscountActiveConverter() : base(typeof(DiscountActive)) { }
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
                    case Yes:
                        return DiscountActive.Yes;
                    case No:
                        return DiscountActive.No;
                    default:
                        return DiscountActive.Unknown;
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
            if (destinationType == typeof(string) && value is DiscountActive)
            {
                switch ((DiscountActive)value)
                {
                    case DiscountActive.Yes:
                        return Yes;
                    case DiscountActive.No:
                        return No;
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

    //[23-09-2009] End Changes for Current Campaign Data fetch request

    #endregion converter class(es)

}
