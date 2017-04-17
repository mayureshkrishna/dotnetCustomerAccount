using System;
using System.ComponentModel;
using Cox.Validation;
using Cox.BusinessLogic.Validation;

namespace Cox.BusinessLogic
{
	/// <summary>
	/// The purpose of this class is to organize a Cox Communications, Inc.
	/// customer account number into its constituent parts. There are four
	/// distinct parts to a customer account number: statement code, company
	/// number, division number, and account number. These parts are arranged
	/// as follows:<newpara/>
	/// <newpara/>
	/// 123 12 12 1234567 89<newpara/>
	///    ^  ^  ^       ^<newpara/>
	/// SC  CN DN   HN    RN<newpara/>
	///          ^     AN<newpara/>
	/// where:<newpara/>
	/// SC = Statement Code<newpara/>
	/// CN = Company Number<newpara/>
	/// DN = Division Number<newpara/>
	/// HN = House Number<newpara/>
	/// RN = Resident Number<newpara/>
	/// AN = Account Number (9-digit)<newpara/>
	/// <newpara/>
	/// The pieces of the account number suggests the heirarchy of the
	/// different business entities invovled:<newpara/>
	/// <newpara/>
	/// Company1<newpara/>
	///		Division1<newpara/>
	///			Account1<newpara/>
	///				Statement1<newpara/>
	///				Statement2<newpara/>
	///			Account2<newpara/>
	///				Statement1<newpara/>
	///		Division2<newpara/>
	///			Account1<newpara/>
	///				Statement1<newpara/>
	///				Statement2<newpara/>
	///				Statement3<newpara/>
	/// Company2<newpara/>
	/// ...<newpara/>
	/// <newpara/>
	/// </summary>
	/// <exception cref="System.ArgumentNullException">Thrown when... .</exception>
	/// <exception cref="System.ArgumentOutOfRangeException">Thrown when... .</exception>
	[TypeConverter(typeof(CustomerAccountNumberConverter))]
	public class CustomerAccountNumber : Object
	{
		#region constants
		/// <summary>
		/// This specifies the length of a statement code.
		/// </summary>
		public const short knStatementCodeLength = 3;
		/// <summary>
		/// This specifies the length of a company number.
		/// </summary>
		public const short knCompanyNumberLength = 2;
		/// <summary>
		/// This specifies the length of a division number.
		/// </summary>
		public const short knDivisionNumberLength = 2;
		/// <summary>
		/// This specifies the length of the house number.
		/// </summary>
		public const short knHouseNumberLength = 7;
		/// <summary>
		/// This specifies the length of a resident number.
		/// </summary>
		public const short knResidentNumberLength = 2;
		/// <summary>
		/// This specifies the length of a customer account number minus the
		/// granularity of the statement code.
		/// </summary>
		public const short knAccountNumber13Length = 13;
		/// <summary>
		/// This specifies the length of a fully-qualified account number.
		/// </summary>
		public const short knAccountNumberLength = 16;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the statement code.
		/// </summary>
		public const short knStatementCodeStartPos = 0;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the company number.
		/// </summary>
		public const short knCompanyNumberStartPos = 3;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the division number.
		/// </summary>
		public const short knDivisionNumberStartPos = 5;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the company/division-specific account number.
		/// </summary>
		public const short knAccountNumber9StartPos = 7;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the account house number.
		/// </summary>
		public const short knHouseNumberStartPos = 7;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the account resident number.
		/// </summary>
		public const short knResidentNumberStartPos = 14;
		/// <summary>
		/// This specifies the starting position within an account number of
		/// the account number minus the granularity of the statement code.
		/// </summary>
		public const short knAccountNumber13StartPos = 3;
		/// <summary>
		/// The parameter name to use when an exception occurs.
		/// </summary>
		private const string kstrParameterName = "Customer account number";
		/// <summary>
		/// The message to use when a null parameter is encountered.
		/// </summary>
		public const string kstrExceptionMessageNull = kstrParameterName + " cannot be null.";
		/// <summary>
		/// The default message to use when throwing an exception from within this class.
		/// </summary>
		public const string kstrExceptionMessageDefault = kstrParameterName + " is invalid.";
		/// <summary>
		/// This member holds the data for the account number. Any other
		/// properties are based on this member.
		/// </summary>
		private string m_strAccountNumber16 = "";
		#endregion constants

		#region ctors
		/// <summary>
		/// A simple constructor is not possible - see the qualified
		/// constructor(s) for more information.
		/// </summary>
		private CustomerAccountNumber():base(){}
		/// <summary>
		/// The constructor will simply store the value and cause validation
		/// of the supplied data.
		/// </summary>
		/// <param name="strAccountNumber16">Account number - A fully-
		/// qualified account number should be supplied to ensure that proper
		/// verification can be performed before any other access to the
		/// properties. This number is specific to a particular customer
		/// statement.
		/// </param>
		public CustomerAccountNumber( string strAccountNumber16 ):base()
		{
			this.AccountNumber16 = strAccountNumber16;
		}
		/// <summary>
		/// This contructor is added for convenience. A scenario exists that
		/// requires the building of the constituent parts of an accoutn
		/// number into the whole.
		/// </summary>
		/// <param name="strStatementCode">Statement Code - specifies the
		/// granularity at statement level.
		/// </param>
		/// <param name="intCompanyNumber">Company Number - specifies part
		/// of the heirarchy relating to a location in the system of record.
		/// </param>
		/// <param name="intDivisionNumber">Division Number - specifies part
		/// of the heirarchy relating to a location in the system of record.
		/// </param>
		/// <param name="strAccountNumber9">Accoutn Number - relates the
		/// location specific account number for a customer record... these
		/// numbers are NOT unique across locations.
		/// </param>
		public CustomerAccountNumber( string strStatementCode,int intCompanyNumber,
			int intDivisionNumber,string strAccountNumber9 ):base()
		{
			// Create the full data picture
			string str = string.Format( "{0}{1}{2}{3}",
				new object [] { FormatSC( strStatementCode ),
								  FormatCN( intCompanyNumber.ToString() ),
								  FormatDN( intDivisionNumber.ToString() ),
								  FormatAN9( strAccountNumber9 ) } );
			// Set the internal data
			this.AccountNumber16 = str;
		}
		/// <summary>
		/// This contructor is added for convenience. A scenario exists that
		/// requires the building of the constituent parts of an account
		/// number into the whole.
		/// </summary>
		/// <param name="strStatementCode">Statement Code - specifies the
		/// granularity at statement level.
		/// </param>
		/// <param name="strCompanyNumber">Company Number - specifies part
		/// of the heirarchy relating to a location in the system of record.
		/// </param>
		/// <param name="strDivisionNumber">Division Number - specifies part
		/// of the heirarchy relating to a location in the system of record.
		/// </param>
		/// <param name="strAccountNumber9">Accoutn Number - relates the
		/// location specific account number for a customer record... these
		/// numbers are NOT unique across locations.
		/// </param>
		public CustomerAccountNumber( string strStatementCode,string strCompanyNumber,
			string strDivisionNumber,string strAccountNumber9 ):base()
		{
			// Create the full data picture
			string str = string.Format( "{0}{1}{2}{3}",
				new object [] { FormatSC( strStatementCode ),
								  FormatCN( strCompanyNumber ),
								  FormatDN( strDivisionNumber ),
								  FormatAN9( strAccountNumber9 ) } );
			// Set the internal data
			this.AccountNumber16 = str;
		}
		/// <summary>
		/// This contructor is added for convenience. A scenario exists that
		/// requires the building of the constituent parts of an accoutn
		/// number into the whole.
		/// </summary>
		/// <param name="strStatementCode">Statement Code - specifies the
		/// granularity at statement level
		/// </param>
		/// <param name="strAccountNumber13">Account Number - relates the
		/// location generic account number for a customer record... these
		/// numbers are unique across locations... This number is NOT specific
		/// to a particular statement.
		/// </param>
		public CustomerAccountNumber(string strStatementCode,string strAccountNumber13):base()
		{
			// Create the full data picture
			string str = string.Format( "{0}{1}",
				new object [] { strStatementCode, strAccountNumber13 } );
			// Set the internal data
			this.AccountNumber16 = str;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// A property specifying a particular statement unique only within a
		/// fully-qualified account number.
		/// </summary>
		public string StatementCode
		{
			get { return m_strAccountNumber16.Substring( knStatementCodeStartPos, knStatementCodeLength ); }
		} // string StatementCode
		/// <summary>
		/// A property specifying the account number minus the statement
		/// code granularity.
		/// </summary>
		public string AccountNumber9
		{
			get { return m_strAccountNumber16.Substring( knAccountNumber9StartPos ); }
		} // string AccountNumber9
		/// <summary>
		/// A fully qualified account number including all location and
		/// statement information as necessary for routing and payment
		/// services. This property is not destructive - validation is
		/// performed before storage.
		/// </summary>
		/// <summary>
		/// A property specifying the account number minus the statement
		/// code granularity.
		/// </summary>
		public string AccountNumber13
		{
			get { return m_strAccountNumber16.Substring( knAccountNumber13StartPos, knAccountNumber13Length ); }
		} // string AccountNumber13
		/// <summary>
		/// A fully qualified account number including all location and
		/// statement information as necessary for routing and payment
		/// services. This property is not destructive - validation is
		/// performed before storage.
		/// </summary>
		public string AccountNumber16
		{
			get { return m_strAccountNumber16.Substring( 0, knAccountNumberLength ); }
			set
			{
				string str = value.Trim();
				if( false == ValidateAccountNumber( str ) )
				{
					throw new ArgumentOutOfRangeException(
						kstrParameterName,
						value, 
						kstrExceptionMessageDefault );
				} // if( false == ValidateAccountNumber( str ) )
				m_strAccountNumber16 = str;
			} // set
		} // string AccountNumber16
		/// <summary>
		/// A number specifying part of the location heirarchy in the system
		/// of record.
		/// </summary>
		public int CompanyNumber
		{
			get { return Int32.Parse( m_strAccountNumber16.Substring( knCompanyNumberStartPos, knCompanyNumberLength ) ); }
		} // int CompanyNumber
		/// <summary>
		/// A number specifying part of the location heirarchy in the system
		/// of record.
		/// </summary>
		public int DivisionNumber
		{
			get { return Int32.Parse( m_strAccountNumber16.Substring( knDivisionNumberStartPos, knDivisionNumberLength ) ); }
		} // int DivisionNumber
		/// <summary>
		/// A number assigned to a unique physical location (house).
		/// </summary>
		public string HouseNumber
		{
			get { return m_strAccountNumber16.Substring( knHouseNumberStartPos, knHouseNumberLength ); }
		} // string HouseNumber
		/// <summary>
		/// A number specifying the current resident within the house.
		/// </summary>
		public string ResidentNumber
		{
			get { return m_strAccountNumber16.Substring( knResidentNumberStartPos, knResidentNumberLength ); }
		} // string ResidentNumber
		#endregion properties

		#region methods
		/// <summary>
		/// This operation creates a distinct copy of the account number object.
		/// </summary>
		/// <returns>
		/// A copy of the account number instance to be used elsewhere without
		/// destructuve results on this instance.
		/// </returns>
		public CustomerAccountNumber Copy()
		{
			return new CustomerAccountNumber( m_strAccountNumber16 );
		} // Copy()
		/// <summary>
		/// This function validates the client-side validation rules for a
		/// fully-qualified account number. To date this is simply a
		/// validation of the length of the account number and assumes the
		/// contained data is valid. That is, no connection to the server for
		/// verification of account number data in this class is performed.
		/// </summary>
		/// <param name="strAccountNumber16">
		/// Account Number - a fully-qualified account number
		/// </param>
		/// <returns>
		/// True - indicates a valid account number.
		/// False - indicates an invalid account number.
		/// </returns>
		public static bool ValidateAccountNumber( string strAccountNumber16 )
		{
			bool blnReturn = false;

			// Validate proper input parameter
			if( null == strAccountNumber16 )
			{
				throw new ArgumentNullException(
					kstrParameterName,
					kstrExceptionMessageNull );
			} // if( null == strAccountNumber16 )

			// Remove trailing and leading white space
			string strLocal = strAccountNumber16.Trim();

			// Validate length
			if( knAccountNumberLength == strLocal.Length )
			{
				// Test the content of the string for alpha characters
				try
				{
					Int64.Parse( strLocal );
					// At this point, success!
					blnReturn = true;
				} // try
				catch
				{
					// Something is invalid in the parse
					blnReturn = false;
				} // catch all
			} // if( knAccountNumberLength == strLocal.Length )

			return blnReturn;
		} // ValidateAccountNumber()
		/// <summary>
		/// This formats a statement code string as expected... a three-digit
		/// string padded with zeros on the left side as necessary.
		/// </summary>
		/// <param name="strStatementCode"></param>
		/// <returns></returns>
		public static string FormatSC( string strStatementCode )
		{
			string strReturn =
				( null != strStatementCode ) ?
				strStatementCode.Trim():
				string.Empty;
			strReturn = strReturn.PadLeft( knStatementCodeLength, '0' );
			return strReturn;
		} // FormatSC()
		/// <summary>
		/// This formats a company number string as expected... a two-digit
		/// string padded with zeros on the left side as necessary.
		/// </summary>
		/// <param name="strCompanyNumber"></param>
		/// <returns></returns>
		public static string FormatCN( string strCompanyNumber )
		{
			string strReturn =
				( null != strCompanyNumber ) ?
				strCompanyNumber.Trim():
				string.Empty;
			strReturn = strReturn.PadLeft( knCompanyNumberLength, '0' );
			return strReturn;
		} // FormatCN()
		/// <summary>
		/// This formats a division number string as expected... a two-digit
		/// string padded with zeros on the left side as necessary.
		/// </summary>
		/// <param name="strDivisionNumber"></param>
		/// <returns></returns>
		public static string FormatDN( string strDivisionNumber )
		{
			string strReturn =
				( null != strDivisionNumber ) ?
				strDivisionNumber.Trim():
				string.Empty;
			strReturn = strReturn.PadLeft( knDivisionNumberLength, '0' );
			return strReturn;
		} // FormatDN()
		/// <summary>
		/// This formats a location-non-specific account number as expected...
		/// a nine-digit string padded with zeros on the left side as
		/// necessary. Also, there is no punctuation.
		/// </summary>
		/// <param name="strAccountNumber9"></param>
		/// <returns></returns>
		public static string FormatAN9( string strAccountNumber9 )
		{
			string strReturn =
				( null != strAccountNumber9 ) ?
				strAccountNumber9.Trim():
				string.Empty;
			strReturn = strReturn.Replace( "-", "" );
			strReturn = strReturn.PadLeft( 9, '0' );
			return strReturn;
		} // FormatAN9()

        /// <summary>
        /// Returns an AccountNumber9 string given a house number and resident number.
        /// </summary>
        /// <param name="houseNumber"></param>
        /// <param name="residentNumber"></param>
        /// <returns></returns>
        public static string CreateAN9(int houseNumber, int residentNumber)
        {
            return houseNumber.ToString().PadLeft(7, '0') + residentNumber.ToString().PadLeft(2, '0');
        }

		/// <summary>
		/// This function was overriden to allow for inclusion in string
		/// output operations within other classes. Mostly, this is
		/// implemented for debugging and logging.
		/// </summary>
		/// <returns>A string containing the fully-qualified account number.</returns>
		public override string ToString()
		{
			return m_strAccountNumber16;
		} // ToString()
		#endregion methods
	}

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class CustomerAccountNumberConverter:TypeConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public CustomerAccountNumberConverter():base(){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Overridden method to test if we can convert from the sourceType.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="sourceType"></param>
		/// <returns></returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context,Type sourceType)
		{
			if(sourceType==typeof(string))return true;
			return base.CanConvertFrom(context,sourceType);
		}
		/// <summary>
		/// Overrides the ConvertFrom method of TypeConverter.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture, object value)
		{
			// check if we care about it
			if(value as string != null)
			{
				// ============================================================================
				// Note: I know that there is some duplication of work going on here.
				// For example, CustomerAccountNumberAttribute calls into CustomerAccountNumber
				// in order to validate. We do this in order to have a common exception. We
				// discussed the fact that some duplication of effort was going to occur and
				// we are ok with this. We felt it was best to provide clear cut and concise
				// classes as opposed to going for pure performance. This will lead to more
				// maintainable code.
				// ============================================================================
				// trim it
				string accountNumber=Convert.ToString(value).Trim();
				// we want to throw the exception defined in the attribute validator.
				CustomerAccountNumberAttribute attribute=new CustomerAccountNumberAttribute();
				attribute.Validate(accountNumber);
				// if here we are still good.
				return new CustomerAccountNumber(accountNumber.PadLeft(
					CustomerAccountNumber.knAccountNumberLength,'0'));
			}
			else
			{
				return base.ConvertFrom(context,culture,value);
			}
		}
		/// <summary>
		/// Overridden method to test if we can convert to the type specified.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="destinationType"></param>
		/// <returns></returns>
		public override bool CanConvertTo(ITypeDescriptorContext context,Type destinationType)
		{
			if (destinationType==typeof(CustomerAccountNumber))return true;
			return base.CanConvertTo(context,destinationType);
		}
		/// <summary>
		/// Overrides the ConvertTo method of TypeConverter.
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
			if(destinationType==typeof(string) && value is CustomerAccountNumber)
			{
				return ((CustomerAccountNumber)value).ToString();
			}
			else
			{
				// we don't want it but the base class might.
				return base.ConvertTo(context,culture,value,destinationType);
			}
		}
		#endregion public methods
	}
	#endregion converter class(es)
}
