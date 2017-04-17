using System;
using System.ComponentModel;

using Request = Cox.ServiceAgent.ConnectionManager.Request;

namespace Cox.ServiceAgent.ConnectionManager
{
    #region enumerations
    /// <summary>
    /// Function actions for spb inline.
    /// </summary>
    public enum eSpbFunctionAction
    {
        /// <summary>
        /// Means to add service codes to customer account.
        /// </summary>
        Add = 2,
        /// <summary>
        /// Means to return (display) service codes for a customer account.
        /// </summary>
        Display = 5
    }
    #endregion enumerations

    #region Common Constants
    /// <summary>
    /// This class exists only to provide modularity to the constants contained within.
    /// </summary>
    public class CmConstant
    {
        /// <summary>
        /// The default constructor fo this class is not accessible. This
        /// prevents anyone from creating an instance of this "class."
        /// </summary>
        private CmConstant() { /*None necessary*/ }

        /// <summary>
        /// This constant provides the common format string for currency
        /// values passed to the system of record.
        /// </summary>
        public const string kstrDefaultCurrencyFormat = "###########0.00";
        /// <summary>
        /// This value is passed to the system of record as a receipt type.
        /// Basically, this implies there is no receipt. The system of
        /// record uses this information as documentation in this case.
        /// </summary>
        public const string kstrDefaultReceiptType = "WEB";
        /// <summary>
        /// This is an arbitrary valie the system of record uses to track 
        /// the terminal used to make a transaction. In this case, the
        /// transaction is electronic and cannot be tracked anyway. 
        /// </summary>
        public const string kstrDefaultWorkstation = "TELNET";
        /// <summary>
        /// This code is primarily used for the payment interface. There does
        /// not seem to be a list of valid task codes readily available.
        /// </summary>
        public const string kstrDefaultTaskCode = "MOP CC CREAT";

        /// <summary>
        /// The account title for a method of payment will be defaulted to 
        /// this value. This is required b/c the interface into the system
        /// of record requires this value in some sites.
        /// </summary>
        public const string kstrDefaultAccountTitle = "Automated Payment";

        /// <summary>
        /// This is the format string for the expiration date in the system
        /// of record.
        /// </summary>
        public const string kstrFormatExpirationDate = "yyyyMMdd";

        /// <summary>
        /// This constant represents an affirmative response to the Connection
        /// Manager interface.
        /// </summary>
        public const string kstrAffirmative = "Y";
        /// <summary>
        /// This constant represents a negative response to the Connection
        /// Manager interface.
        /// </summary>
        public const string kstrNegative = "N";

        /// <summary>
        /// This is the interface representation of the bank account type
        /// Checking.
        /// </summary>
        public const string kstrAccountTypeChecking = "C";
        /// <summary>
        /// This is the interface representation of the bank account type
        /// Savings.
        /// </summary>
        public const string kstrAccountTypeSavings = "S";
    } // struct class

    #endregion // Common Constants

    #region ICOMS Element

    /// <summary>
    /// This class aids in building minimum properties into a Request.ICOMS
    /// instance and allows for cleaner code.
    /// </summary>
    public class IcomsHelper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.ICOMS m_elm = new Request.ICOMS();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="objItem">
        /// An instance of one of the macro classes (i.e. MAC00010,
        /// MAC00027, etc.).
        /// </param>
        public IcomsHelper(object objItem)
        {
            m_elm.Item = objItem;
        } // IcomsHelper() (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.ICOMS Element
        {
            get { return m_elm; }
        } // Request.ICOMS Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.ICOMS(IcomsHelper hlpr)
        {
            return hlpr.m_elm;
        } // Request.ICOMS() (cast to Request.ICOMS)

        #endregion // Implicit Operators

    } // class IcomsHelper


    #endregion // ICOMS Element

    #region Macros

    /// <summary>
    /// This class aids in building minimum properties into a Request.ACSUM
    /// instance and allows for cleaner code.
    /// </summary>
    public class AcsumHelper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.ACSUM m_elm = new Request.ACSUM();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        public AcsumHelper(string strSiteId, string strAccountNumber9)
        {
            // Fix up the site id input
            int intSite = 0;
            try
            { intSite = int.Parse(strSiteId); }
            catch
            { /*Don't care*/ }

            m_elm.SITEID = intSite.ToString();
            m_elm.ACCNTNO = strAccountNumber9;
        } // AcsumHelper() (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.ACSUM Element
        {
            get { return m_elm; }
        } // Request.ACSUM Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.ACSUM(AcsumHelper hlpr)
        {
            return hlpr.m_elm;
        } // Request.ACSUM() (cast to Request.ACSUM)

        #endregion // Implicit Operators

    } // class AcsumHelper

    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00010
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00006Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.MAC00006 m_elm = new Request.MAC00006();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId"></param>
        /// <param name="strAccountNumber9"></param>
        /// <param name="strTaskCode"></param>
        /// <param name="strCampaignCode"></param>
        /// <param name="strRetainWorkOrder"></param>
        /// <param name="cancelOrderIfCheckinFails"></param>
        public Mac00006Helper(string strSiteId, string strAccountNumber9,
            string strTaskCode, string strCampaignCode, string strRetainWorkOrder,
            bool cancelOrderIfCheckinFails)
        {
            int intSiteId = 0;
            try { intSiteId = Convert.ToInt32(strSiteId); }
            catch {/*don't care*/}
            m_elm.SITEID = intSiteId.ToString();
            m_elm.ACNTNMBR = strAccountNumber9 != null && strAccountNumber9.Trim().Length > 0
                                    ? strAccountNumber9.Trim() : null;
            m_elm.TASKCODE = strTaskCode != null && strTaskCode.Trim().Length > 0
                                    ? strTaskCode.Trim() : null;
            m_elm.CMPGNNMBR = strCampaignCode != null && strCampaignCode.Trim().Length > 0
                                    ? strCampaignCode.Trim() : null;
            m_elm.RETNWORKORDR = strRetainWorkOrder != null && strRetainWorkOrder.Trim().Length > 0
                                    ? strRetainWorkOrder.Trim() : null;
            m_elm.CNCLWORKORDR = cancelOrderIfCheckinFails == true ? CmConstant.kstrAffirmative : CmConstant.kstrNegative;
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId"></param>
        /// <param name="strAccountNumber9"></param>
        /// <param name="strCampaignCode"></param>
        /// <param name="cancelOrderIfCheckinFails"></param>
        public Mac00006Helper(string strSiteId, string strAccountNumber9,
            string strCampaignCode, bool cancelOrderIfCheckinFails)
        {
            int intSiteId = 0;
            try { intSiteId = Convert.ToInt32(strSiteId); }
            catch {/*don't care*/}
            m_elm.SITEID = intSiteId.ToString();
            m_elm.ACNTNMBR = strAccountNumber9 != null && strAccountNumber9.Trim().Length > 0
                ? strAccountNumber9.Trim() : null;
            m_elm.CMPGNNMBR = strCampaignCode != null && strCampaignCode.Trim().Length > 0
                ? strCampaignCode.Trim() : null;
            m_elm.CNCLWORKORDR = cancelOrderIfCheckinFails == true ? CmConstant.kstrAffirmative : CmConstant.kstrNegative;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.MAC00006 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.MAC00006(Mac00006Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00010
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00010Helper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.MAC00010 m_elm = new Request.MAC00010();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strStatementCode">One of the minimum contained properties.</param>
        public Mac00010Helper(string strSiteId, string strAccountNumber9,
                string strStatementCode)
        {
            // Fix up the site id input
            int intSite = 0;
            try
            { intSite = int.Parse(strSiteId); }
            catch
            { /*Don't care*/ }

            m_elm.SITEID = intSite.ToString();
            m_elm.ACNTNMBR = strAccountNumber9;
            m_elm.STMNTCODE = strStatementCode;
        } // Mac00010Helper( strSiteId, strAccountNumber9, strStatementCode ) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strStatementCode">One of the minimum contained properties.</param>
        /// <param name="intProcessControlNumber">One of the minimum contained properties.</param>
        public Mac00010Helper(string strSiteId, string strAccountNumber9,
                string strStatementCode, int intProcessControlNumber)
            : this(strSiteId, strAccountNumber9, strStatementCode)
        {
            if (0 < intProcessControlNumber)
                m_elm.PRCSCNTRLHEDR = intProcessControlNumber.ToString();
        } // Mac00010Helper() (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.MAC00010 Element
        {
            get { return m_elm; }
        } // Request.MAC00010 Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.MAC00010(Mac00010Helper hlpr)
        {
            return hlpr.m_elm;
        } // Request.MAC00010() (cast to Request.MAC00010)

        #endregion // Implicit Operators

    } // class Mac00010Helper

    /// <summary>
    /// This class aids in building minimum properties into a Request.CURBILL
    /// instance and allows for cleaner code.
    /// </summary>
    public class CurbillHelper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.CURBILL m_elm = new Request.CURBILL();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strStatementCode">One of the minimum contained properties.</param>
        /// <param name="intProcessControlNumber">One of the minimum contained properties.</param>
        public CurbillHelper(string strSiteId, string strAccountNumber9,
                string strStatementCode, int intProcessControlNumber)
        {
            // Fix up the site id input
            int intSite = 0;
            try
            { intSite = int.Parse(strSiteId); }
            catch
            { /*Don't care*/ }

            m_elm.SITEID = intSite.ToString();
            m_elm.ACCNTNO = strAccountNumber9;
            m_elm.STMNTCD = strStatementCode;
            if (0 < intProcessControlNumber)
                m_elm.PRCSSCNTRL = intProcessControlNumber.ToString();
        } // CurbillHelper () (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.CURBILL Element
        {
            get { return m_elm; }
        } // Request.CURBILL Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.CURBILL(CurbillHelper hlpr)
        {
            return hlpr.m_elm;
        } // Request.CURBILL() (cast to Request.CURBILL)

        #endregion // Implicit Operators

    } // class CurbillHelper 

    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00022
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00022Helper
    {
        #region constants
        /// <summary>
        /// OrderSource value to use. E is not part of the documentation but taken from
        /// current implementation. My guess is that E stands for eCommerce.
        /// 
        /// TODO: Evaluate if this is a dynamic value we need to account for. Or if it
        /// is something we should create an enumeration for. Whatever we do, this needs
        /// to be researched further.
        /// </summary>
        protected const string kstrDefaultOrderSource = "E";
        /// <summary>
        /// ActionType to use when creating the order. This should only ever be P for
        /// purchase; however, we may want to create some conversion classes to handle
        /// the different values.
        /// </summary>
        protected const string kstrDefaultActionType = "P";
        #endregion constants

        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.MAC00022 m_elm = new Request.MAC00022();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId"></param>
        /// <param name="strAccountNumber9"></param>
        /// <param name="strTitleCode"></param>
        /// <param name="strShowingNumber"></param>
        /// <param name="strPurchaser"></param>
        /// <param name="strPinNumber"></param>
        public Mac00022Helper(string strSiteId, string strAccountNumber9,
            string strTitleCode, string strShowingNumber, string strPurchaser,
            string strPinNumber)
        {
            // Fix up the site id input
            int intSite = 0;
            try { intSite = int.Parse(strSiteId); }
            catch {/*Don't care*/}
            m_elm.SITEID = intSite.ToString();
            m_elm.ACNTNMBR = strAccountNumber9;
            m_elm.TITLCODE = strTitleCode;
            m_elm.SHWNGNMBR = strShowingNumber;
            // length is up to 10 character. anything more will 
            // cause a -10034 error response code from icoms
            m_elm.PRCHSRID = strPurchaser.Trim().Substring(0, 10);

            // default values
            m_elm.ORDRSORC = kstrDefaultOrderSource;
            m_elm.ACTNTYPE = kstrDefaultActionType;
            if (strPinNumber != null && strPinNumber.Length > 0)
            {
                m_elm.PINNMBR = strPinNumber;
            }
        }
        #endregion ctors

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.MAC00022(Mac00022Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }

    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00027
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00027Helper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.MAC00027 m_elm = new Request.MAC00027();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
                string strTaskCode)
        {
            // Fix up the site id input
            int intSite = 0;
            try
            { intSite = int.Parse(strSiteId); }
            catch
            { /*Don't care*/ }

            m_elm.SITEID = intSite.ToString();
            m_elm.ACNTNMBR = strAccountNumber9;
            m_elm.TASKCODE = strTaskCode;
        } // Mac00027Helper(...) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        /// <param name="inl47">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
                string strTaskCode, Request.INL00047 inl47)
            : this(strSiteId, strAccountNumber9, strTaskCode)
        {
            m_elm.INL00047 = new Request.INL00047[] { (Request.INL00047)inl47 };
        } // Mac00027Helper(...) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        /// <param name="inl72">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
                string strTaskCode, Request.INL00072 inl72)
            : this(strSiteId, strAccountNumber9, strTaskCode)
        {
            m_elm.INL00072 = new Request.INL00072[] { (Request.INL00072)inl72 };
        } // Mac00027Helper(...) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        /// <param name="inl73">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
            string strTaskCode, Request.INL00073 inl73)
            : this(strSiteId, strAccountNumber9, strTaskCode)
        {
            m_elm.INL00073 = new Request.INL00073[] { (Request.INL00073)inl73 };
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        /// <param name="inl74">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
            string strTaskCode, Request.INL00074 inl74)
            : this(strSiteId, strAccountNumber9, strTaskCode)
        {
            m_elm.INL00074 = new Request.INL00074[] { (Request.INL00074)inl74 };
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId">One of the minimum contained properties.</param>
        /// <param name="strAccountNumber9">One of the minimum contained properties.</param>
        /// <param name="strTaskCode">One of the minimum contained properties.</param>
        /// <param name="inl76">One of the minimum contained properties.</param>
        public Mac00027Helper(string strSiteId, string strAccountNumber9,
            string strTaskCode, Request.INL00076 inl76)
            : this(strSiteId, strAccountNumber9, strTaskCode)
        {
            m_elm.INL00076 = new Request.INL00076[] { (Request.INL00076)inl76 };
        }

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.MAC00027 Element
        {
            get { return m_elm; }
        } // Request.MAC00027 Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.MAC00027(Mac00027Helper hlpr)
        {
            return hlpr.m_elm;
        } // Request.MAC00027() (cast to Request.MAC00027)

        #endregion // Implicit Operators

    } // class Mac00027Helper 

    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00054
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00054Helper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.MAC00054 m_elm = new Request.MAC00054();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="taskCode">One of the minimum contained properties.</param>
        /// <param name="siteId">One of the minimum contained properties.</param>
        public Mac00054Helper( string taskCode, int siteId )
        {
            m_elm.TASKCODE  = taskCode;
            m_elm.SITEID    = siteId.ToString();
        } // Mac00054Helper(...) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the Request
        /// namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="taskCode">One of the minimum contained properties.</param>
        /// <param name="siteId">One of the minimum contained properties.</param>
        /// <param name="inl192">One of the minimum contained properties.</param>
        public Mac00054Helper( string taskCode, int siteId, Request.INL00170 inl170 )
            : this( taskCode, siteId )
        {
            m_elm.INL00170 = new Request.INL00170[ 1 ] { ( Request.INL00170 )inl170 };
        } // Mac00054Helper(..., inl170) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the Request
        /// namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="taskCode">One of the minimum contained properties.</param>
        /// <param name="siteId">One of the minimum contained properties.</param>
        /// <param name="inl192">One of the minimum contained properties.</param>
        public Mac00054Helper( string taskCode, int siteId, Request.INL00192 inl192 )
            : this( taskCode, siteId )
        {
            m_elm.INL00192 = new Request.INL00192[ 1 ] { ( Request.INL00192 )inl192 };
        } // Mac00054Helper(..., inl192) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the Request
        /// namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="taskCode">One of the minimum contained properties.</param>
        /// <param name="siteId">One of the minimum contained properties.</param>
        /// <param name="inl193">One of the minimum contained properties.</param>
        public Mac00054Helper( string taskCode, int siteId, Request.INL00193 inl193 )
            : this( taskCode, siteId )
        {
            m_elm.INL00193 = new Request.INL00193[ 1 ] { ( Request.INL00193 )inl193 };
        } // Mac00054Helper(..., inl193) (Constructor)

        /// <summary>
        /// This constructor aids in the creation of the class in the Request
        /// namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="taskCode">One of the minimum contained properties.</param>
        /// <param name="siteId">One of the minimum contained properties.</param>
        /// <param name="inl170">One of the minimum contained properties.</param>
        /// <param name="inl193">One of the minimum contained properties.</param>
        public Mac00054Helper( string taskCode, int siteId, Request.INL00170 inl170, Request.INL00193 inl193 )
            : this( taskCode, siteId, inl193 )
        {
            m_elm.INL00170 = new Request.INL00170[ 1 ] { ( Request.INL00170 )inl170 };
        } // Mac00054Helper(..., inl170, inl193) (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.MAC00054 Element
        {
            get
            {
                return m_elm;
            }
        } // Request.MAC00054 Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.MAC00054( Mac00054Helper hlpr )
        {
            return hlpr.m_elm;
        } // Request.MAC00054() (cast to Request.MAC00054)

        #endregion // Implicit Operators

    } // class Mac00054Helper 

    /// <summary>
    /// This class aids in building minimum properties into a Request.StopPb
    /// instance and allows for cleaner code.
    /// </summary>
    public class StopPbHelper
    {
        #region constants
        private const string __taskCode = "STOPPAPERBIL";
        #endregion constants

        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.STOPPB m_elm = new Request.STOPPB();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strSiteId"></param>
        /// <param name="strAccountNumber9"></param>
        /// <param name="spb"></param>
        public StopPbHelper(string strSiteId, string strAccountNumber9, Request.SPB spb)
        {
            int intSiteId = 0;
            try { intSiteId = Convert.ToInt32(strSiteId); }
            catch {/*don't care*/}
            m_elm.SITEID = intSiteId.ToString();
            m_elm.ACCNTNO = strAccountNumber9 != null
                && strAccountNumber9.Trim().Length > 0
                ? strAccountNumber9.Trim() : null;
            m_elm.TASKCODE = __taskCode;
            m_elm.SPB = new Request.SPB[] { (Request.SPB)spb };
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.STOPPB Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.STOPPB(StopPbHelper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class will cancel a work Order.
    /// </summary>
    public class CancelHelper
    {
        #region constants
        /// <summary>
        /// Internal task code to use when communicating with ICOMS
        /// </summary>
        private const string __taskCode = "CANCEL";
        /// <summary>
        /// Internal Reason code used to define the reason for cancellation.
        /// </summary>
        private const string __cancelReasonCode = "01";
        #endregion constants

        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.CANCEL m_elm = new Request.CANCEL();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="accountNumber9"></param>
        /// <param name="workOrderNumber"></param>
        public CancelHelper(int siteId, string accountNumber9, string workOrderNumber)
        {
            m_elm.SITEID = siteId.ToString();
            //trim it?
            if (accountNumber9 != null) accountNumber9 = accountNumber9.Trim();
            // if no length. set it to null.
            if (accountNumber9.Length == 0) accountNumber9 = null;
            m_elm.ACCNTNO = accountNumber9;
            m_elm.TASKCODE = __taskCode;
            m_elm.WONO = workOrderNumber;
            m_elm.CANCREASCD = __cancelReasonCode;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.CANCEL Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.CANCEL(CancelHelper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }

    /// <summary>
    /// This class aids in building minimum properties into a Request.MAC00053
    /// instance and allows for cleaner code.
    /// </summary>
    public class Mac00053Helper
    {
      #region Members
      /// <summary>
      /// This is the contained element.
      /// </summary>
      private Request.MAC00053 m_elm = new Request.MAC00053();
      #endregion //Members

      #region Construction/Destruction
      /// <summary>
      /// This constructor aids in the creation of the class in the
      /// Request namespace of the Connection Manager service agent.
      /// </summary>
      /// <param name="taskCode">One of the minimum contained properties.</param>
      /// <param name="siteId">One of the minimum contained properties.</param>
      public Mac00053Helper( string taskCode, int siteId )
      {
          m_elm.TASKCODE  = taskCode;
          m_elm.SITEID    = siteId.ToString();
      } // Mac00053Helper(...) (Constructor)

      /// <summary>
      /// This constructor aids in the creation of the class in the Request
      /// namespace of the Connection Manager service agent.
      /// </summary>
      /// <param name="taskCode">One of the minimum contained properties.</param>
      /// <param name="siteId">One of the minimum contained properties.</param>
      /// <param name="inl132">One of the minimum contained properties.</param>
      public Mac00053Helper( string taskCode, int siteId, Request.INL00132 inl132 )
          : this( taskCode, siteId )
      {
          m_elm.INL00132 = new Request.INL00132[1] { ( Request.INL00132 )inl132 };
      } // Mac00053Helper(..., inl132) (Constructor)
      #endregion //Construction/Destruction

      #region Properties
      /// <summary>
      /// This property allows access to the contained element.
      /// </summary>
      public Request.MAC00053 Element
      {
        get{return m_elm;}
      } // Request.MAC00053 Element
      #endregion // Properties

      #region Implicit Operators
      /// <summary>
      /// This operator allows the helper class interact with Connection
      /// Manager functionality without extra manipulation code.
      /// </summary>
      /// <param name="hlpr">An instance of this helper class.</param>
      /// <returns>An instance of the contained class.</returns>
      public static implicit operator Request.MAC00053(Mac00053Helper hlpr)
      {
        return hlpr.m_elm;
      } // Request.MAC00053() (cast to Request.MAC00053)
      #endregion // Implicit Operators

    }//end Mac00053Helper

    public class Mac00011Helper
    {
      #region Members
      /// <summary>
      /// This is the contained element.
      /// </summary>
      private Request.MAC00011 m_elm = new Request.MAC00011();
      #endregion //Members

      #region Construction/Destruction
      
      /// <summary>
      /// This class aids in building minimum properties into a Request.MAC00011
      /// instance and allows for cleaner code.
      /// </summary>
      /// <param name="houseNumber">One of the minimum contained properties.</param>
      /// <param name="siteId">One of the minimum contained properties.</param>
      /// <param name="serviceCategory">One of the minimum contained properties.</param>
      /// <param name="serviceableStatus">The serviceable status</param>
      public Mac00011Helper( string houseNumber, int siteId, string serviceCategory, string serviceableStatus, string functionAction)
      {
        m_elm.HOUSENUM = houseNumber;
        m_elm.SITEID = siteId.ToString();
        m_elm.SRVCATG = serviceCategory;
        m_elm.SRVCEABLESTS = serviceableStatus;
        m_elm.FUNCTACT = functionAction.ToString();
        m_elm.AUTOASSIGN = "N";
      } // Mac00011Helper(...) (Constructor)

      
      #endregion //Construction/Destruction

      #region Properties
      /// <summary>
      /// This property allows access to the contained element.
      /// </summary>
      public Request.MAC00011 Element
      {
        get { return m_elm; }
      } // Request.MAC00011 Element
      #endregion // Properties

      #region Implicit Operators
      /// <summary>
      /// This operator allows the helper class interact with Connection
      /// Manager functionality without extra manipulation code.
      /// </summary>
      /// <param name="hlpr">An instance of this helper class.</param>
      /// <returns>An instance of the contained class.</returns>
      public static implicit operator Request.MAC00011(Mac00011Helper hlpr)
      {
        return hlpr.m_elm;
      } // Request.MAC00011() (cast to Request.MAC00011)
      #endregion // Implicit Operators
    }
    #endregion //Macros

    #region Inlines
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00020
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00020Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00020 m_elm = new Request.INL00020();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intOccurrence"></param>
        /// <param name="strServiceCode"></param>
        /// <param name="eAction"></param>
        public INL00020Helper(int intOccurrence, string strServiceCode, eFunctionAction eAction)
        {
            m_elm.SRVCOCRNC = intOccurrence.ToString();
            m_elm.SRVCCODE = strServiceCode;
            m_elm.IGIFNCTNACTN = (string)TypeDescriptor.GetConverter(
                typeof(eFunctionAction)).ConvertTo(eFunctionAction.Add, typeof(string));
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strServiceCode">One of the minimum contained properties.</param>
        public INL00020Helper(string strServiceCode)
        {
            m_elm.SRVCOCRNC = "1";
            m_elm.SRVCCODE = strServiceCode;
            m_elm.IGIFNCTNACTN = FunctionActionConverter.Add.ToString();
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00020 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00020(INL00020Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00021
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00021Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00021 m_elm = new Request.INL00021();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="dttmDate"></param>
        /// <param name="poolTimeSlot"></param>
        public INL00021Helper(DateTime dttmDate, string poolTimeSlot)
        {
            m_elm.SCHDLDATE8 = dttmDate.ToString(CmConstant.kstrFormatExpirationDate);
            m_elm.POOLTIMESLOT = poolTimeSlot;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00021 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00021(INL00021Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00022
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00022Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00022 m_elm = new Request.INL00022();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intOccurrence"></param>
        /// <param name="strServiceCode"></param>
        /// <param name="eAction"></param>
        public INL00022Helper(int intOccurrence, string strServiceCode, eFunctionAction eAction)
        {
            m_elm.SRVCOCRNC = intOccurrence.ToString();
            m_elm.SRVCCODE = strServiceCode;
            m_elm.IGIFNCTNACTN = (string)TypeDescriptor.GetConverter(
                typeof(eFunctionAction)).ConvertTo(eFunctionAction.Add, typeof(string));
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="strServiceCode">One of the minimum contained properties.</param>
        public INL00022Helper(string strServiceCode)
        {
            m_elm.SRVCOCRNC = "1";
            m_elm.SRVCCODE = strServiceCode;
            m_elm.IGIFNCTNACTN = FunctionActionConverter.Add.ToString();
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00022 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00022(INL00022Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00047
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00047Helper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00047 m_elm = new Request.INL00047();

        #endregion //Members

        #region Construction/Destruction
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="dblTendered">One of the minimum contained properties.</param>
        /// <param name="dblPayAmount">One of the minimum contained properties.</param>
        /// <param name="strBankAccountType">One of the minimum contained properties.</param>
        /// <param name="strCreateIfExists">One of the minimum contained properties.</param>
        /// <param name="strMopVendorNumber">One of the minimum contained properties.</param>
        /// <param name="strMopAccountNumber">One of the minimum contained properties.</param>
        /// <param name="strMopAccountTitle">One of the minimum contained properties.</param>
        /// <param name="intMethodOfPaymentCode">One of the minimum contained properties.</param>
        /// <param name="strPostWithReceipt">One of the minimum contained properties.</param>
        /// <param name="strStatementCode">One of the minimum contained properties.</param>
        /// <param name="strTransactionReceiptType">One of the minimum contained properties.</param>
        /// <param name="strWorkStation">One of the minimum contained properties.</param>
        public INL00047Helper(double dblTendered, double dblPayAmount,
                string strBankAccountType, string strCreateIfExists,
                string strMopVendorNumber, string strMopAccountNumber,
                string strMopAccountTitle, int intMethodOfPaymentCode,
                string strPostWithReceipt, string strStatementCode,
                string strTransactionReceiptType, string strWorkStation)
        {
            // Fix up the statement code input
            int intSC = 0;
            try
            { intSC = int.Parse(strStatementCode); }
            catch
            { /*Don't care*/ }

            m_elm.AMNTTNDRDUSR = dblTendered.ToString(CmConstant.kstrDefaultCurrencyFormat);
            m_elm.AMNTTOAPLYUSR = dblPayAmount.ToString(CmConstant.kstrDefaultCurrencyFormat);
            m_elm.BANKDRFTACNTTYPE = strBankAccountType;
            m_elm.CRETIFEXSTS = strCreateIfExists;
            m_elm.MOPACNTNMBR = strMopAccountNumber;
            m_elm.MOPACNTTITL = strMopAccountTitle;
            m_elm.MOPVNDRNMBR = strMopVendorNumber;
            m_elm.MTHDOFPAYCODE = intMethodOfPaymentCode.ToString();
            m_elm.POSTWITHRCPT = strPostWithReceipt;
            m_elm.STMNTCODE = intSC.ToString();
            m_elm.TRNSCTNRCPTTYP = strTransactionReceiptType;
            m_elm.WORKSTTNATR = strWorkStation;
        } // INL00047Helper() (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00047 Element
        {
            get { return m_elm; }
        } // Request.INL00047 Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00047(INL00047Helper hlpr)
        {
            return hlpr.m_elm;
        } // Request.INL00047() (cast to Request.INL00047)

        #endregion // Implicit Operators

    } // class INL00047Helper 
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00072
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00072Helper
    {
        #region Members

        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00072 m_elm = new Request.INL00072();

        #endregion //Members

        #region Construction/Destruction

        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="dblTendered">One of the minimum contained properties.</param>
        /// <param name="dblPayAmount">One of the minimum contained properties.</param>
        /// <param name="strCreateIfExists">One of the minimum contained properties.</param>
        /// <param name="strMopAccountNumber">One of the minimum contained properties.</param>
        /// <param name="strMopAccountTitle">One of the minimum contained properties.</param>
        /// <param name="dttmExpiration">One of the minimum contained properties.</param>
        /// <param name="intMethodOfPaymentCode">One of the minimum contained properties.</param>
        /// <param name="strPostWithReceipt">One of the minimum contained properties.</param>
        /// <param name="strStatementCode">One of the minimum contained properties.</param>
        /// <param name="strTransactionReceiptType">One of the minimum contained properties.</param>
        /// <param name="strWorkStation">One of the minimum contained properties.</param>
        public INL00072Helper(
                double dblTendered, double dblPayAmount, string strCreateIfExists,
                string strMopAccountNumber, string strMopAccountTitle,
                DateTime dttmExpiration, int intMethodOfPaymentCode,
                string strPostWithReceipt, string strStatementCode,
                string strTransactionReceiptType, string strWorkStation)
        {
            // Fix up the statement code input
            int intSC = 0;
            try
            { intSC = int.Parse(strStatementCode); }
            catch
            { /*Don't care*/ }

            m_elm.AMNTTNDRD = dblTendered.ToString(CmConstant.kstrDefaultCurrencyFormat);
            m_elm.AMNTTOAPLY = dblPayAmount.ToString(CmConstant.kstrDefaultCurrencyFormat);
            m_elm.CRETIFEXSTS = strCreateIfExists;
            m_elm.EXPRTNDATE = dttmExpiration.ToString(CmConstant.kstrFormatExpirationDate);
            m_elm.MOPACNTNMBR = strMopAccountNumber;
            m_elm.MOPACNTTITL = strMopAccountTitle;
            m_elm.MTHDOFPAYCODE = intMethodOfPaymentCode.ToString();
            m_elm.POSTWITHRCPT = strPostWithReceipt;
            m_elm.STMNTCODE = intSC.ToString();
            m_elm.TRNSCTNRCPTTYP = strTransactionReceiptType;
            m_elm.WORKSTTNATR = strWorkStation;
        } // INL00072Helper() (Constructor)

        #endregion // Construction/Destruction

        #region Properties

        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00072 Element
        {
            get { return m_elm; }
        } // Request.INL00072 Element

        #endregion // Properties

        #region Implicit Operators

        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00072(INL00072Helper hlpr)
        {
            return hlpr.m_elm;
        } // Request.INL00072() (cast to Request.INL00072)

        #endregion // Implicit Operators

    } // class INL00072Helper 

    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00073
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00073Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00073 m_elm = new Request.INL00073();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intMethodOfPayCode">One of the minimum contained properties.</param>
        /// <param name="strMopAccountNumber">One of the minimum contained properties.</param>
        /// <param name="strMopAccountTitle">One of the minimum contained properties.</param>
        /// <param name="dttmExpiration">One of the minimum contained properties.</param>
        /// <param name="intStatementCode">One of the minimum contained properties.</param>
        /// <param name="blnCreateIfExists">One of the minimum contained properties.</param>
        public INL00073Helper(int intMethodOfPayCode, string strMopAccountNumber,
            string strMopAccountTitle, DateTime dttmExpiration, int intStatementCode,
            bool blnCreateIfExists)
        {
            m_elm.MTHDOFPAYCODE = intMethodOfPayCode.ToString();
            m_elm.MOPACNTNMBR = strMopAccountNumber;
            m_elm.MOPACNTTITL = strMopAccountTitle;
            m_elm.EXPRTNDATE = dttmExpiration.ToString(CmConstant.kstrFormatExpirationDate);
            m_elm.AUTOASGNSTMNT = intStatementCode.ToString();
            m_elm.CRETIFEXSTS = blnCreateIfExists ?
                                    CmConstant.kstrAffirmative :
                                    CmConstant.kstrNegative;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00073 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00073(INL00073Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00074
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00074Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00074 m_elm = new Request.INL00074();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intMethodOfPayCode">One of the minimum contained properties.</param>
        /// <param name="strMopAccountNumber">One of the minimum contained properties.</param>
        /// <param name="strMopVendorNumber">One of the minimum contained properties.</param>
        /// <param name="strMopAccountTitle">One of the minimum contained properties.</param>
        /// <param name="chBankAccountType">One of the minimum contained properties.</param>
        /// <param name="intStatementCode">One of the minimum contained properties.</param>
        /// <param name="blnCreateIfExists">One of the minimum contained properties.</param>
        public INL00074Helper(int intMethodOfPayCode, string strMopAccountNumber,
            string strMopVendorNumber, string strMopAccountTitle, char chBankAccountType,
            int intStatementCode, bool blnCreateIfExists)
        {
            m_elm.MTHDOFPAYCODE = intMethodOfPayCode.ToString();
            m_elm.MOPACNTNMBR = strMopAccountNumber;
            m_elm.MOPVNDRNMBR = strMopVendorNumber;
            m_elm.MOPACNTTITL = strMopAccountTitle;
            m_elm.BANKDRFTACNTTYPE = chBankAccountType.ToString();
            m_elm.AUTOASGNSTMNT = intStatementCode.ToString();
            m_elm.CRETIFEXSTS = blnCreateIfExists ?
                                        CmConstant.kstrAffirmative :
                                        CmConstant.kstrNegative;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00074 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00074(INL00074Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00076
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00076Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00076 m_elm = new Request.INL00076();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intMethodOfPayCode">One of the minimum contained properties.</param>
        /// <param name="intMopAccountSequence">One of the minimum contained properties.</param>
        /// <param name="intStatementCode">One of the minimum contained properties.</param>
        public INL00076Helper(int intMethodOfPayCode, int intMopAccountSequence,
            int intStatementCode)
        {
            m_elm.MTHDOFPAYCODE = intMethodOfPayCode.ToString();
            m_elm.MOPACNTSQNC = intMopAccountSequence.ToString();
            m_elm.STMNTCODE = intStatementCode.ToString();
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00076 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00076(INL00076Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00022
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00096Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00096 m_elm = new Request.INL00096();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="dttmFromDate"></param>
        public INL00096Helper(DateTime dttmFromDate)
        {
            m_elm.FROMDATE = dttmFromDate.ToString(CmConstant.kstrFormatExpirationDate);
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00096 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00096(INL00096Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00022
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00117Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00117 m_elm = new Request.INL00117();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This is the default constructor. It aids in the creation of the class 
        /// in the Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="workOrderStatus"></param>
        /// <param name="workOrderDateOffset"></param>
        /// <param name="technicianNumber"></param>
        public INL00117Helper(string technicianNumber)
        {
            m_elm.TECHNMBR = technicianNumber;
        }
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="workOrderStatus"></param>
        /// <param name="workOrderDateOffset"></param>
        /// <param name="technicianNumber"></param>
        public INL00117Helper(string workOrderStatus, int workOrderDateOffset, string technicianNumber)
        {
            DateTime startBillingDate = DateTime.Now.AddHours((double)workOrderDateOffset);
            m_elm.STRTBLNGDATE = startBillingDate.ToString(CmConstant.kstrFormatExpirationDate);
            if (workOrderStatus.Length > 0 && workOrderStatus != " ") m_elm.ORDRSTTS = workOrderStatus;
            m_elm.TECHNMBR = technicianNumber;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00117 Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00117(INL00117Helper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00170
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00170Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00170 m_elm = new Request.INL00170();
        #endregion member variables

        #region ctors

        /// <summary>
        /// This is the default constructor. This constructor aids in the
        /// creation of the class in the Request namespace of the Connection
        /// Manager service agent.
        /// </summary>
        /// <param name="strAccountNumber9">
        /// This parameter is required to identify the account for which
        /// information will be set.
        /// </param>
        public INL00170Helper( string strAccountNumber9 )
        {
            m_elm.ACNTNMBR = strAccountNumber9;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00170 Element
        {
            get
            {
                return m_elm;
            }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00170( INL00170Helper hlpr )
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00192
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00192Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00192 m_elm = new Request.INL00192();
        #endregion member variables

        #region ctors

        /// <summary>
        /// This is the default constructor. This constructor aids in the
        /// creation of the class in the Request namespace of the Connection
        /// Manager service agent.
        /// </summary>
        /// <param name="strHouseNumber">
        /// This parameter is required to identify the account for which
        /// information will be set.
        /// </param>
        public INL00192Helper( string strHouseNumber )
        {
            m_elm.HOUSNMBR = strHouseNumber;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00192 Element
        {
            get
            {
                return m_elm;
            }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00192( INL00192Helper hlpr )
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00193
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00193Helper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.INL00193 m_elm = new Request.INL00193();
        #endregion member variables

        #region ctors

        /// <summary>
        /// This is the default constructor. This constructor aids in the
        /// creation of the class in the Request namespace of the Connection
        /// Manager service agent.
        /// </summary>
        /// <param name="strAccountNumber9">
        /// This parameter is required to identify the account for which
        /// information will be set.
        /// </param>
        public INL00193Helper( string strAccountNumber9 )
        {
            m_elm.ACNTNMBR = strAccountNumber9;
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.INL00193 Element
        {
            get
            {
                return m_elm;
            }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.INL00193( INL00193Helper hlpr )
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }
    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00076
    /// instance and allows for cleaner code.
    /// </summary>
    public class SpbHelper
    {
        #region member variables
        /// <summary>
        /// This is the contained element.
        /// </summary>
        private Request.SPB m_elm = new Request.SPB();
        #endregion member variables

        #region ctors
        /// <summary>
        /// This constructor aids in the creation of the class in the
        /// Request namespace of the Connection Manager service agent.
        /// </summary>
        /// <param name="intStatementCode"></param>
        /// <param name="intBillHandlingCode"></param>
        /// <param name="functionAction"></param>
        public SpbHelper(int intStatementCode, int intBillHandlingCode, eSpbFunctionAction functionAction)
        {
            m_elm.STMNTCD = intStatementCode.ToString();
            m_elm.BILLHDLCDE = intBillHandlingCode.ToString();
            m_elm.FUNCACT = ((int)functionAction).ToString();
        }
        #endregion ctors

        #region properties
        /// <summary>
        /// This property allows access to the contained element.
        /// </summary>
        public Request.SPB Element
        {
            get { return m_elm; }
        }
        #endregion properties

        #region operators
        /// <summary>
        /// This operator allows the helper class interact with Connection
        /// Manager functionality without extra manipulation code.
        /// </summary>
        /// <param name="hlpr">An instance of this helper class.</param>
        /// <returns>An instance of the contained class.</returns>
        public static implicit operator Request.SPB(SpbHelper hlpr)
        {
            return hlpr.m_elm;
        }
        #endregion operators
    }

    /// <summary>
    /// This class aids in building minimum properties into a Request.INL00132
    /// instance and allows for cleaner code.
    /// </summary>
    public class INL00132Helper
    {
      #region member variables
      /// <summary>
      /// This is the contained element.
      /// </summary>
      private Request.INL00132 m_elm = new Request.INL00132();
      #endregion member variables

      #region ctors
      /// <summary>
      /// This is the default constructor. This constructor aids in the
      /// creation of the class in the Request namespace of the Connection
      /// Manager service agent.
      /// </summary>
      /// <param name="houseNumber">
      /// This parameter is required to identify the house to be copied      
      /// </param>
      public INL00132Helper( string houseNumber)
      {
        m_elm.HOUSNMBR = houseNumber;        
      }
      #endregion ctors

      #region properties
      /// <summary>
      /// This property allows access to the contained element.
      /// </summary>
      public Request.INL00132 Element
      {
        get { return m_elm; }
      }
      #endregion properties

      #region operators
      /// <summary>
      /// This operator allows the helper class interact with Connection
      /// Manager functionality without extra manipulation code.
      /// </summary>
      /// <param name="hlpr">An instance of this helper class.</param>
      /// <returns>An instance of the contained class.</returns>
      public static implicit operator Request.INL00132(INL00132Helper hlpr)
      {
        return hlpr.m_elm;
      }
      #endregion operators
    }//end INL00132Helper

    #endregion // Inlines
} // namespace Cox.BusinessLogic.ConnectionManager
