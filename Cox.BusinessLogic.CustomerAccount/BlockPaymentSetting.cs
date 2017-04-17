using System;


namespace Cox.BusinessObjects.CustomerAccount
{
    /// <summary>
    /// Within the ICOMS customer account, there are several values to flag an
    /// account for blocked payemnts.
    /// </summary>
    public enum BlockPaymentSetting
    {
        Blank   = 0,
        /// <summary>Don't block payments.</summary>
        None    = ' ',
        /// <summary>Block only credit card payments.</summary>
        Credit  = 'C',
        /// <summary>Block only debit (i.e. debit card, check) payments.</summary>
        Debit   = 'D',
        /// <summary>Block all payment types.</summary>
        All     = 'A'
    } // enum BlockPaymentSetting

} // namespace Cox.BusinessObjects.CustomerAccount

