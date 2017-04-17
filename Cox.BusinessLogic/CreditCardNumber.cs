using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

using Cox.BusinessObjects;
using Cox.Validation;
using Cox.BusinessLogic.Validation;

namespace Cox.BusinessLogic
{

	#region Enumerations
	/// <summary>
	/// The first digit of your credit card number is the Major Industry
	/// Identifier (MII). This represents the category of entity which
	/// issued your credit card.
	/// </summary>
	public enum MajorIndustryType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown					= -1,
		/// <summary>
		/// This indicates ISO/TC 68 and other industry assignments.
		/// </summary>
		ISO_TC68Other			= 0,
		/// <summary>
		/// This indicates the Airlines industry.
		/// </summary>
		Airlines				= 1,
		/// <summary>
		/// This indicates Airlines and other industry assignments.
		/// </summary>
		AirlinesOther			= 2,
		/// <summary>
		/// This indicates the travel and entertainment industries.
		/// </summary>
		TravelEntertainment		= 3,
		/// <summary>
		/// This indicates the banking and financial industry.
		/// </summary>
		BankingFinancial1		= 4,
		/// <summary>
		/// This indicates the banking and financial industry.
		/// </summary>
		BankingFinancial2		= 5,
		/// <summary>
		/// This indicates the merchandizing and banking industries.
		/// </summary>
		MerchandizingBanking	= 6,
		/// <summary>
		/// This indicates the Petroleum industry.
		/// </summary>
		Petroleum				= 7,
		/// <summary>
		/// This indicates Telecommunications and other industry assignments.
		/// </summary>
		TelecomOther			= 8,
		/// <summary>
		/// This indicates a National assignment.
		/// </summary>
		NationalAssign			= 9
	}
	#endregion Enumerations

	#region Credit Card Number Class
	/// <summary>
	/// The purpose of this class is to organize credit card number
	/// into its constituent parts.	
	/// </summary>
	[TypeConverter(typeof(CreditCardNumberConverter))]
	public class CreditCardNumber : Object
	{
		#region Constants
		/// <summary>
		/// This specifies the length of the Major Industry Identifier.
		/// </summary>
		private const int		kintMajorIndustryLength			= 1;
		/// <summary>
		/// This indicates the starting position within the credit card
		/// number of the Major Industry Identifier.
		/// </summary>
		private const int		kintMajorIndustryStartPos		= 0;
		/// <summary>
		/// This specifies the length of the Issuer Identifier.
		/// </summary>
		private const int		kintIssuerLength				= 6;
		/// <summary>
		/// This indicates the starting position within the credit card
		/// number of the Issuer Identifier.
		/// </summary>
		private const int		kintIssuerStartPos				= 0;
		/// <summary>
		/// This indicates the starting position within the credit card
		/// number of the acocunt number.
		/// </summary>
		private const int		kintAccountStartPos				= 6;
		/// <summary>
		/// This indicates the starting position from the end within the
		/// credit card number of the account number.
		/// </summary>
		private const int		kintAccountPosFromEnd			= 1;
		/// <summary>
		/// This indicates the starting position from the end within the
		/// credit card number of the check digit.
		/// </summary>
		private const int		kintCheckDigitPosFromEnd		= 0;
		/// <summary>
		/// The parameter name to use when an exception occurs.
		/// </summary>
		private const string	kstrParameterName				= "Credit card number";
		/// <summary>
		/// The message to use when a null parameter is encountered.
		/// </summary>
		public const string		kstrExceptionMessageNull		= kstrParameterName + " cannot be null.";
		/// <summary>
		/// The default message to use when throwing an exception from within this class.
		/// </summary>
		public const string		kstrExceptionMessageDefault		= kstrParameterName + " is not valid.";

		#endregion // Constants

		#region Members
		/// <summary>
		/// Digits 7 to (n - 1) of your credit card number are your
		/// individual account identifier.
		/// </summary>
		private	string				m_strAccountNumber	= null;
		/// <summary>
		/// The first digit of your credit card number is the Major
		/// Industry Identifier (MII), which represents the category
		/// of entity which issued your credit card.
		/// </summary>
		private	MajorIndustryType	m_emit				= MajorIndustryType.Unknown;
		/// <summary>
		/// The first 6 digits of your credit card number (including the
		/// initial MII digit) form the issuer identifier.
		/// </summary>
		private	IssuerType			m_eit				= IssuerType.Unknown;
		/// <summary>
		/// This is a colleciton of parsers that allow this class to
		/// implement the properties provided.
		/// </summary>
		internal static CCNumberParser[] m_apars =
			new CCNumberParser[]
			{
				new DinersClubParser(),
				new AmexParser(),
				new VisaParser(),
				new MastercardParser(),
				new DiscoverParser() 
			}; // CCNumberParser

		#endregion // Members

		#region ctors
		/// <summary>
		/// This is the default constructor for this class. The protection
		/// level prevents anyonw from instantiating a blank instance.
		/// </summary>
		private CreditCardNumber()
			: base()
		{ /* None necessary */ }

		/// <summary>
		/// This is the main constructor for this class. This ensures that
		/// a proper number will be provided upon instantiation.
		/// </summary>
		/// <param name="strAccountNumber">
		/// The credit card number to pull apart.
		/// </param>
		public CreditCardNumber( string strAccountNumber )
		{
			// Set the property and validate there
			AccountNumber = strAccountNumber;
		} // CreditCardNumber( string ) (Constructor)

		#endregion ctors

		#region Properties
		/// <summary>
		/// This property returns the fully-qualified credit card number
		/// contained in this class instance. NOTE: the set accessor for
		/// this property performs validation on input.
		/// </summary>
		public string AccountNumber
		{
			get{ return m_strAccountNumber; }
			set
			{
				// Remove whitespace
				string str = RemoveNonDigits( value );
				if( false == __ValidateAccountNumber( str ) )
				{
					throw new ArgumentOutOfRangeException(
						kstrParameterName,
						value,
						kstrExceptionMessageDefault );
				} // if( false == __ValidateAccountNumber( str ) )
				m_strAccountNumber = str;
				m_eit = ParsIssuerType( str );
			} // set
		} // string AccountNumber

		/// <summary>
		/// This property returns the major industry identifier for this
		/// credit card number instance. This is read-only.
		/// </summary>
		public string MajorIndustryIdentifier
		{
			get
			{
				return m_strAccountNumber.Substring(
					kintMajorIndustryStartPos, kintMajorIndustryLength );
			} // get
		} // string MajorIndustryIdnetifier

		/// <summary>
		/// This property returns the issuer identifier for this
		/// credit card number instance. This is read-only.
		/// </summary>
		public string IssuerIdentifier
		{
			get
			{
				return m_strAccountNumber.Substring(
					kintIssuerStartPos, kintIssuerLength );
			} // get
		} // string IssuerIdentifier

		/// <summary>
		/// This property returns the individual account identifier for this
		/// credit card number instance. This is read-only.
		/// </summary>
		public string AccountIdentifier
		{
			get
			{
				return m_strAccountNumber.Substring( kintAccountStartPos,
					m_strAccountNumber.Length - kintAccountPosFromEnd );
			} // get
		} // string AccountIdentifier

		/// <summary>
		/// This property returns the Major Industry Identifier enumerated
		/// type for this class instance.
		/// </summary>
		public MajorIndustryType MajorIndustry
		{
			get{ return m_emit; }
		} // eMajorIndustryIdentifier MajorIndustry

		/// <summary>
		/// This property returns the Issuer Identifier enumerated type
		/// for this class instance.
		/// </summary>
		public IssuerType Issuer
		{
			get{ return m_eit; }
		} // eMajorIndustryIdentifier Issuer

		/// <summary>
		/// Convert the internal issuer type to a valid payment type we
		/// expect in other interfaces.
		/// </summary>
		public ePaymentType PaymentType
		{
			get
			{
				ePaymentType ept = ePaymentType.Unknown;
				switch( Issuer )
				{
					case IssuerType.Mastercard:
						ept = ePaymentType.MasterCard;
						break;
					case IssuerType.Discover:
						ept = ePaymentType.Discover;
						break;
					case IssuerType.AmericanExpress:
						ept = ePaymentType.AmericanExpress;
						break;
					case IssuerType.Visa:
						ept = ePaymentType.Visa;
						break;
				} // switch( Issuer )

				return ept;
			} // get

		} // ePaymentType PaymentType

		#endregion // Properties

		#region Helper Functions
		/// <summary>
		/// Remove any white space characters fro the input string.
		/// </summary>
		/// <param name="str">This is the string to be cleaned.</param>
		/// <returns>
		/// The output is a string containind no non-digit characters.
		/// </returns>
		internal static string RemoveNonDigits( string str )
		{
			string strReturn = "";
			foreach( char ch in str )
			{
				if( Char.IsNumber( ch ) ) 
					strReturn += ch;
			} // foreach( char ch in str )

			return strReturn;
		} // RemoveNonDigits()

		/// <summary>
		/// This function parses the issuer identifier from the string credit
		/// card number. 
		/// </summary>
		/// <param name="strAccountNumber">
		/// The input fully-qualified credit card number.
		/// </param>
		/// <returns>
		/// The substring representation of the issuer identifier.
		/// </returns>
		internal static string ParseIssueIdentifier( string strAccountNumber )
		{
			string str = string.Empty;
			if( strAccountNumber.Length >
					CreditCardNumber.kintIssuerStartPos +
					CreditCardNumber.kintIssuerLength )
				str = strAccountNumber.Substring(
					CreditCardNumber.kintIssuerStartPos,
					CreditCardNumber.kintIssuerLength );
			return str;
		} // ParseIssueIdentifier()

		/// <summary>
		/// This function parses the enumerated issuer type from the string
		/// credit card number. 
		/// </summary>
		/// <param name="strAccountNumber">
		/// The input fully-qualified credit card number.
		/// </param>
		/// <returns>
		/// The substring representation of the issuer identifier.
		/// </returns>
		internal static IssuerType ParsIssuerType( string strAccountNumber )
		{
			IssuerType eit = IssuerType.Unknown;

			foreach( CCNumberParser pars in m_apars )
			{
				if( pars.Parse( strAccountNumber.Length,
					CreditCardNumber.ParseIssueIdentifier( strAccountNumber ) ) )
				{
					eit = pars.IssuerType;
					// Assumption: mutually exclusive types
					break;
				} // if( Parser.Parse()
			} // foreach( CCNumberParser pars in m_apars )

			return eit;
		} // ParsIssuerType()

		/// <summary>
		/// This function uses the industry standard Luhn Method to test for a
		/// valid credit card number.
		/// </summary>
		/// <param name="strAccountNumber">
		/// The input fully-qualified credit card number.
		/// </param>
		/// <returns>
		/// This function returns true if the input passed the validation
		/// check. Otherwise, false.
		/// </returns>
		internal static bool ValidateUsingLuhnMethod( string strAccountNumber )
		{
			int intSum = 0;
			int intDigit = 0;
			int intAddend = 0;
			// First occurrence is not alteranting digit
			bool blnAlternateDigit = false;

			// Loop through the digits from next to last to first
			// Example:
			//		1234567890123345 6
			//					<---^
			// NOTE: Start with next to last digit, but avoid off-by-one error
			for( int i = strAccountNumber.Length - 1; i >= 0; i-- )
			{
				// Obtain the value of the current digit
				intDigit = Int32.Parse( strAccountNumber.Substring( i, 1) );
				// If this is an alternating digit...
				if( blnAlternateDigit )
				{
					// ...multiply by two and normalize to characters '0'-'9'
					intAddend = intDigit * 2;
					if( intAddend > 9 ) 
						intAddend -= 9;
				} // if( blnAlternateDigit )
				else
					// ... otherwise, simply use the value
					intAddend = intDigit;
				// Add the last calculated digit
				intSum += intAddend;
				// Reset the alteranting digit flag
				blnAlternateDigit = ! blnAlternateDigit;
			} // for( i >= 0 )

			// Calculate the modulus for this account number
			int intModulus = intSum % 10;

			// Return success if the modulus is zero
			return( intModulus == 0 );
		} // ValidateUsingLuhnMethod()


		/// <summary>
		/// This internal function throws an null parameter exception as part
		/// of the validation routine.
		/// </summary>
		/// <param name="strAccountNumber">
		/// The input fully-qualified credit card number.
		/// </param>
		/// <returns>
		/// This function returns true if the input passed the validation
		/// check. Otherwise, false.
		/// </returns>
		internal static bool __ValidateAccountNumber( string strAccountNumber )
		{
			bool blnReturn = false;

			if( null == strAccountNumber )
			{
				throw new ArgumentNullException(
					kstrParameterName,
					kstrExceptionMessageNull );
			} // if( null == strAccountNumber )

			// Parse the issuer and test against unknown and
			// validate with Luhn method
			if( ( IssuerType.Unknown != ParsIssuerType( strAccountNumber ) ) &&
				( CreditCardNumber.ValidateUsingLuhnMethod( strAccountNumber ) ) )
				blnReturn = true;

			return blnReturn;
		} // __ValidateAccountNumber()

		#endregion // Helper Functions

		#region Methods

		/// <summary>
		/// This function validates the credit card number using
		/// __ValidateAccountNumber, but passes the string minus any white
		/// space as expected for an externally available interface.
		/// </summary>
		/// <param name="strAccountNumber">
		/// The input fully-qualified credit card number. This parameter could
		/// contain "dirty" content (i.e. white space characters).
		/// </param>
		/// <returns>
		/// This function returns true if the input passed the validation
		/// check. Otherwise, false.
		/// </returns>
		public static bool ValidateAccountNumber( string strAccountNumber )
		{
			return __ValidateAccountNumber( RemoveNonDigits( strAccountNumber ) );
		} // ValidateAccountNumber()

		#endregion // Methods
	}
	#endregion Credit Card Number Class

	#region Parser Classes

	#region Abstract Base Parser

	/// <summary>
	/// This is the base interface for any parser used in this schema.
	/// </summary>
	public abstract class CCNumberParser
	{

		#region Members

		/// <summary>
		/// This member holds the Major Industry Identifier.
		/// </summary>
		protected IssuerType m_eit = IssuerType.Unknown;
		/// <summary>
		/// This member holds valid ranges used for identification.
		/// </summary>
		protected ArrayList m_lstRanges = new ArrayList();
		/// <summary>
		/// This member holds valid lengths used for identification.
		/// </summary>
		protected ArrayList m_lstLengths = new ArrayList();

		#endregion // Members


		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public CCNumberParser()
		{ /* None necessary */ }

		#endregion // Construction/Destruction


		#region Methods

		/// <summary>
		/// This function initiates the parse of a number given the length
		/// and issuer identifier of the credit card number.
		/// </summary>
		/// <param name="intLength">
		/// This specifies the length of the credit card number.
		/// </param>
		/// <param name="strIssuer">
		/// This specifies the issuer identifier of the credit card number.
		/// </param>
		/// <returns>
		/// If this credit card number matches intended issuer type, valid
		/// lengths,a nd valid ranges for this parser, then this function
		/// returns true. Otherwise, false.
		/// </returns>
		public bool Parse( int intLength, string strIssuer )
		{
			// Assume false and match one of the length values
			bool blnLengthValid = false;
			// Assume true and match one of the range values
			bool blnIssuerValid = false;

			foreach( int n in m_lstLengths )
			{
				if( n == intLength )
				{
					blnLengthValid = true;
					break;
				} // 
			} // foreach( string str in m_lstLengths )

			foreach( Int64RangeAttribute vldtr in m_lstRanges )
			{
				Int64 intIssuer = Int64.Parse( strIssuer );
				if( vldtr.IsValid( intIssuer ) )
				{
					blnIssuerValid = true;
					break;
				} // if( vldtr.IsValid( ... ) )
			} // foreach( Int64RangeValidator rng in m_lstRanges )

			return ( blnLengthValid && blnIssuerValid );
		} // Parse( intLength, strIssuer )

		#endregion // Methods


		#region Properties

		/// <summary>
		/// TODO: Property description
		/// </summary>
		public IssuerType IssuerType
		{
			get { return m_eit; }
		} // IssuerType IssuerType

		#endregion // Properties

	} // class CCNumberParser

	#endregion // Abstract Base Parser


	/// <summary>
	/// This parser derivative defines a diners club card.
	/// </summary>
	public class DinersClubParser : CCNumberParser
	{

		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public DinersClubParser()
			: base()
		{
			// Assign issuing type
			m_eit = IssuerType.DinersClub;
			// Valid ranges
			// 300xxx-305xxx
			// 36xxxx-38xxxx
			m_lstRanges.Add(new Int64RangeAttribute(300000,305999));
			m_lstRanges.Add(new Int64RangeAttribute(360000,389999));
			// Valid Lengths
			m_lstLengths.Add( 14 );
		} // DinersClubParser() (Constructor)

		#endregion // Construction/Destruction

	} // class DinersClubParser

	/// <summary>
	/// AmericanExpress card number parser
	/// </summary>
	public class AmexParser : CCNumberParser
	{

		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public AmexParser()
			: base()
		{
			// Assign issuing type
			m_eit = IssuerType.AmericanExpress;
			// Valid ranges
			// 33xxxx-37xxxx
			m_lstRanges.Add(new Int64RangeAttribute(330000,379999));
			// Valid Lengths
			m_lstLengths.Add( 15 );
		} // AmexParser() (Constructor)

		#endregion // Construction/Destruction

	} // class AmexParser

	/// <summary>
	/// Visa card number parser
	/// </summary>
	public class VisaParser : CCNumberParser
	{

		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public VisaParser()
			: base()
		{
			// Assign issuing type
			m_eit = IssuerType.Visa;
			// Valid ranges
			// 4xxxxx
			m_lstRanges.Add(new Int64RangeAttribute(400000,499999));
			// Valid Lengths
			m_lstLengths.Add(13);
			m_lstLengths.Add(16);
		} // VisaParser() (Constructor)

		#endregion // Construction/Destruction

	} // class VisaParser

	/// <summary>
	/// Mastercard card number parser
	/// </summary>
	public class MastercardParser : CCNumberParser
	{

		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public MastercardParser()
			: base()
		{
			// Assign issuing type
			m_eit = IssuerType.Mastercard;
			// Valid ranges
			// 51xxxx - 55xxxx
			m_lstRanges.Add(new Int64RangeAttribute(510000,559999));
			// Valid Lengths
			m_lstLengths.Add( 16 );
		} // MastercardParser() (Constructor)

		#endregion // Construction/Destruction

	} // class MastercardParser

	/// <summary>
	/// Discover card number parser
	/// </summary>
	public class DiscoverParser : CCNumberParser
	{

		#region Construction/Destruction

		/// <summary>
		/// This is the default constructor for this class.
		/// </summary>
		public DiscoverParser()
			: base()
		{
			// Assign issuing type
			m_eit = IssuerType.Discover;
			// Valid ranges
			// 6011xx
			m_lstRanges.Add(new Int64RangeAttribute(601100,601199));
			// Valid Lengths
			m_lstLengths.Add( 16 );
		} // DiscoverParser() (Constructor)

		#endregion // Construction/Destruction

	} // class DiscoverParser


	#endregion // Parser Classes

	#region converter class(es)
	/// <summary>
	/// This class wraps the eTaskCode enumeration and provides conversions 
	/// to/from eTaskCode
	/// </summary>
	public class CreditCardNumberConverter:TypeConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public CreditCardNumberConverter():base(){}
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
				// For example, CreditCardNumberAttribute calls into CreditCardNumber
				// in order to validate. We do this in order to have a common exception. We
				// discussed the fact that some duplication of effort was going to occur and
				// we are ok with this. We felt it was best to provide clear cut and concise
				// classes as opposed to going for pure performance. This will lead to more
				// maintainable code.
				// ============================================================================
				// trim it
				string creditCardNumber=Convert.ToString(value).Trim();
				// we want to throw the exception defined in the attribute validator.
				CreditCardNumberAttribute attribute=new CreditCardNumberAttribute();
				attribute.Validate(creditCardNumber);
				// if here we are still good.
				return new CreditCardNumber(creditCardNumber);
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
			if (destinationType == typeof(CreditCardNumber))return true;
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
			if(destinationType==typeof(string)&&value==typeof(CreditCardNumber))
			{
				return ((CreditCardNumber)value).ToString();
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
