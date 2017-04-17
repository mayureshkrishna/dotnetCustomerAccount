using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerAccount
{ 
  /// <summary>
  /// List of Cable Serviceable Status
  /// </summary>
  [TypeConverter(typeof(CableServiceStatusConverter))]
  [XmlTypeAttribute("CableServiceStatus")]
  public enum CableServiceStatus
  {
    /// <summary>
    /// Unknown 
    /// </summary>
    Unknown = -1,
    /// <summary>
    /// Addressable Tap = 'AT'
    /// </summary>
    AddressableTap = 1,
    /// <summary>
    /// Cable Collections Account = 'CC'
    /// </summary>
    CableCollectionsAccount = 2,
    /// <summary>
    /// Cable Future = 'CF'
    /// </summary>
    CableFuture = 3,
    /// <summary>
    /// Cable Hub Zone Fixes = 'CH'
    /// </summary>
    CableHubZoneFixes = 4,
    /// <summary>
    /// Cable MDU/No Agreement = 'CM'
    /// </summary>
    CableMduNoAgreement = 5,
    /// <summary>
    /// Cable Not Serviceable = 'CN'
    /// </summary>
    CableNotServiceable = 6,
    /// <summary>
    /// Cable Serviceable = 'CY'
    /// </summary>
    CableServiceable = 7,
    /// <summary>
    /// Digital Compression = 'DC'
    /// </summary>
    DigitalCompression = 8,
    /// <summary>
    /// Digital Serviceable-Motorola = 'DM'
    /// </summary>
    DigitalServiceable_M = 9,
    /// <summary>
    /// Digital Serviceable = 'DS'
    /// </summary>
    DigitalServiceable = 10,
    /// <summary>
    /// Digital Serviceable-Telco = 'DT'
    /// </summary>
    DigitalServiceable_Telco = 11
  }//enum CableServiceStatus

  #region converter class(es)
  /// <summary>
  /// This class wraps the CableServiceStatus enumeration and provides conversions 
  /// to/from CableServiceStatus
  /// </summary>
  public class CableServiceStatusConverter : EnumConverter
  {
    #region constants
    /// <summary>
    /// Unknown
    /// </summary>
    public const string Unknown = "Unknown";
    /// <summary>
    /// Addressable Tap = 'AT'
    /// </summary>
    public const string AddressableTap = "AT";
    /// <summary>
    /// Cable Collections Account = 'CC'
    /// </summary>
    public const string CableCollectionsAccount = "CC";
    /// <summary>
    /// Cable Future = 'CF'
    /// </summary>
    public const string CableFuture = "CF";
    /// <summary>
    /// Cable Hub Zone Fixes = 'CH'
    /// </summary>
    public const string CableHubZoneFixes = "CH";
    /// <summary>
    /// Cable MDU/No Agreement = 'CM'
    /// </summary>
    public const string CableMduNoAgreement = "CM";
    /// <summary>
    /// Cable Not Serviceable = 'CN'
    /// </summary>
    public const string CableNotServiceable = "CN";
    /// <summary>
    /// Cable Serviceable = 'CY'
    /// </summary>
    public const string CableServiceable = "CY";
    /// <summary>
    /// Digital Compression = 'DC'
    /// </summary>
    public const string DigitalCompression = "DC";
    /// <summary>
    /// Digital Serviceable-Motorola = 'DM'
    /// </summary>
    public const string DigitalServiceable_M = "DM";
    /// <summary>
    /// Digital Serviceable = 'DS'
    /// </summary>
    public const string DigitalServiceable = "DS";
    /// <summary>
    /// Digital Serviceable-Telco = 'DT'
    /// </summary>
    public const string DigitalServiceable_Telco = "DT";
    #endregion constants

    #region ctors
    /// <summary>
    /// The default constructor
    /// </summary>
    public CableServiceStatusConverter() : base(typeof(CableServiceStatus)) { }
    #endregion ctors

    #region public methods
    /// <summary>
    /// Overrides the ConvertFrom method of TypeConverter.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="culture"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public override object ConvertFrom( ITypeDescriptorContext context,
                                        System.Globalization.CultureInfo culture,
                                        object value)
    {      
      if (value as string != null)
      {
        string localCategory = ((string)value).Trim().ToUpper();
        
        switch (localCategory)
        {
          case AddressableTap:
            return CableServiceStatus.AddressableTap;
          case CableCollectionsAccount:
            return CableServiceStatus.CableCollectionsAccount;
          case CableFuture:
            return CableServiceStatus.CableFuture;
          case CableHubZoneFixes:
            return CableServiceStatus.CableHubZoneFixes;
          case CableMduNoAgreement:
            return CableServiceStatus.CableMduNoAgreement;
          case CableNotServiceable:
            return CableServiceStatus.CableNotServiceable;
            case CableServiceable:
            return CableServiceStatus.CableServiceable;
          case DigitalCompression:
            return CableServiceStatus.DigitalCompression;
            case DigitalServiceable:
            return CableServiceStatus.DigitalServiceable;
          case DigitalServiceable_M:
            return CableServiceStatus.DigitalServiceable_M;
          case DigitalServiceable_Telco:
            return CableServiceStatus.DigitalServiceable_Telco;
          default:
            return CableServiceStatus.Unknown;
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
    public override object ConvertTo( ITypeDescriptorContext context,
                                      System.Globalization.CultureInfo culture,
                                      object value,
                                      Type destinationType)
    {      
      if (destinationType == typeof(string) && value is CableServiceStatus)
      {
        switch ((CableServiceStatus)value)
        {
          case CableServiceStatus.AddressableTap:
            return AddressableTap;
          case CableServiceStatus.CableCollectionsAccount:
            return CableCollectionsAccount;
          case CableServiceStatus.CableFuture:
            return CableFuture;
          case CableServiceStatus.CableHubZoneFixes:
            return CableHubZoneFixes;
          case CableServiceStatus.CableMduNoAgreement:
            return CableMduNoAgreement;
          case CableServiceStatus.CableNotServiceable:
            return CableNotServiceable;
          case CableServiceStatus.CableServiceable:
            return CableServiceable;
          case CableServiceStatus.DigitalCompression:
            return DigitalCompression;
          case CableServiceStatus.DigitalServiceable:
            return DigitalServiceable;
          case CableServiceStatus.DigitalServiceable_M:
            return DigitalServiceable_M;
          case CableServiceStatus.DigitalServiceable_Telco:
            return DigitalServiceable_Telco;          
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

