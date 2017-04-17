using System;

namespace Cox.BusinessObjects.CustomerAccount
{
  public enum AuditCheckCode
  {
    /// <summary>
    /// Blank
    /// </summary>
    Blank = 0,
    /// <summary>
    /// Illegal 1
    /// </summary>
    Illegal1 = 1,
    /// <summary>
    /// Illegal 2
    /// </summary>
    Illegal2 = 2,
    /// <summary>
    /// Disconnected Illegal
    /// </summary>
    DisconnectedIllegal = 3,
    /// <summary>
    /// Added by Audit
    /// </summary>
    AddedByAudit = 'A',
    /// <summary>
    /// Converted
    /// </summary>
    Converted = 'C',
    /// <summary>
    /// Delete - Bad House Record
    /// </summary>
    DeleteBadHouseRecord = 'D',
    /// <summary>
    /// Error
    /// </summary>
    Error = 'E',
    /// <summary>
    /// Verify Hot Drop
    /// </summary>
    VerifyHotDrop = 'F',
    /// <summary>
    /// Verify Cold Drop
    /// </summary>
    VerifyColdDrop = 'G',
    /// <summary>
    /// Trap Illegals
    /// </summary>
    TrapIllegals = 'H',
    /// <summary>
    /// Added Through Installation Work Order
    /// </summary>
    AddedThroughInstallationWorkOrder = 'I',
    /// <summary>
    /// Duplicate Address/Parallel Account
    /// </summary>
    DuplicateAddressOrParallelAccount = 'J',
    /// <summary>
    /// New Build
    /// </summary>
    NewBuild = 'N',
    /// <summary>
    /// Added by New Build - In and On
    /// </summary>
    AddedByNewBuildInAndOn = 'R',
    /// <summary>
    /// Added by Sales
    /// </summary>
    AddedBySales = 'S',
    /// <summary>
    /// Added by Tap Audit
    /// </summary>
    AddedByTapAudit = 'T',
    /// <summary>
    /// Untouched by Audit
    /// </summary>
    UntouchedByAudit = 'U',
    /// <summary>
    /// Verified by Audit
    /// </summary>
    VerifiedByAudit = 'V',
    /// <summary>
    /// Illegal Other
    /// </summary>
    IllegalOther = 'X',
    /// <summary>
    /// ABI Address - Auto Add
    /// </summary>
    AbiAddressAutoAdd = 'Y',
    /// <summary>
    /// ABI Address - Manual Add
    /// </summary>
    AbiAddressManualAdd = 'Z'
  }//enum AuditCheckCode
}// namespace Cox.BusinessObjects.CustomerAccount
