using System;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace Cox.BusinessObjects.CustomerAccount
{
    #region enumerations
    /// <summary>
    /// Bill Type for customer.
    /// </summary>

    [TypeConverter(typeof(BillTypeConverter))]
    public enum BillType
    {
        /// <summary>Commercial</summary>
        Commercial = 'C',
        /// <summary>Multi Family</summary>
        MultiFamily = 'M',
        /// <summary>Single Family</summary>
        SingleFamily = 'S'

    }

    #endregion enumerations

    #region typeconverters

    public class BillTypeConverter : TypeConverter
    {
        // Overrides the CanConvertFrom method of TypeConverter.
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        // Overrides the ConvertFrom method of TypeConverter.
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return (BillType)Enum.Parse(typeof(BillType), value.ToString(), true);
            }
            return base.ConvertFrom(context, culture, value);
        }
        // Overrides the ConvertTo method of TypeConverter.
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return ((BillType)value).ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

    }


    #endregion typeconverters

    //public static class ConvertBillType
    //{

    //    /// <summary>
    //    /// GetBillType
    //    /// </summary>
    //    /// <param name="BillType"></param>
    //    /// <returns></returns>
    //    public static BillType GetBillType(string billType)
    //    {
    //        BillType returnBillType = BillType.SingleFamily;
    //        switch (billType)
    //        {
    //           case "C":
    //               returnBillType = BillType.Commercial;
    //                break;
    //            case "M":
    //                returnBillType = BillType.MultiFamily;
    //                break;
    //            case "S":
    //                returnBillType = BillType.SingleFamily;
    //                break;
    //            default:
    //                break;
    //        }
    //        return returnBillType;
    //    }

    //}

}
