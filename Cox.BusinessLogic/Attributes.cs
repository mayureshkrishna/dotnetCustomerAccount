using System;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Cox.DataAccess;
using Cox.DataAccess.Enterprise;
using Cox.DataAccess.Exceptions;
using Cox.Validation;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.BusinessLogic.Validation
{
	/// <summary>
	/// This class provides the rules for a valid creditcard number.
	/// </summary>
	public class CreditCardNumberAttribute:BaseItemAttribute
	{
		#region constants
		/// <summary>
		/// Constant defining error message for this class.
		/// </summary>
		private const string __errorMessage="{0} contains an invalid CreditCardNumber value of '{1}'.";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CreditCardNumberAttribute():this(null,__errorMessage){}
		/// <summary>
		/// Constructor when you are only setting the name.
		/// </summary>
		/// <param name="name"></param>
		public CreditCardNumberAttribute(string name):this(name,__errorMessage){}
		/// <summary>
		/// Protected constructor when you wish to override the errorMessage.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="errorMessage"></param>
		protected CreditCardNumberAttribute(string name,string errorMessage)
			:base( name,errorMessage){}
		#endregion ctors

		#region methods
		/// <summary>
		/// Return true, if the object value is valid, false otherwise.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool IsValid(object obj)
		{
			return IsValid(Convert.ToString(obj));
		}
		/// <summary>
		/// Return true, if the string value is a valid credit card number, false otherwise.
		/// </summary>
		/// <param name="creditCardNumber"></param>
		/// <returns></returns>
		public bool IsValid(string creditCardNumber)
		{
			try
			{
				return CreditCardNumber.ValidateAccountNumber(creditCardNumber);
			}
			catch
			{
				return false;
			}
		}
		/// <summary>
		/// Throws an exception if it fails validation; else it does nothing.
		/// </summary>
		/// <param name="val"></param>
		public virtual void Validate(object val)
		{
			if(!IsValid(val))throw new ValidationException(GetFormattedError(val));
		}
		#endregion methods
	}
	/// <summary>
	/// This class provides the rules for a valid creditCard expiration date.
	/// </summary>
	public class ValidCCDateAttribute:ValidDateTimeAttribute
	{
		#region constants/readonly
		/// <summary>
		/// A default format included by default with each instance.
		/// </summary>
		protected static readonly string __defaultFormat1="MM/yy";
		/// <summary>
		/// A default format included by default with each instance.
		/// </summary>
		protected static readonly string __defaultFormat2="MM/yyyy";
		/// <summary>
		/// A default format included by default with each instance.
		/// </summary>
		protected static readonly string __defaultFormat3="MM/dd/yy";
		/// <summary>
		/// A default format included by default with each instance.
		/// </summary>
		protected static readonly string __defaultFormat4="MM/dd/yyyy";
		#endregion constants/readonly

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public ValidCCDateAttribute():this(null){}
		/// <summary>
		/// The default constructor.
		/// </summary>
		/// <param name="name"></param>
		public ValidCCDateAttribute(string name)
			:base(name,new string[]{__defaultFormat1,
			__defaultFormat2,__defaultFormat3,__defaultFormat4}){}
		#endregion ctors

		#region operators
		/// <summary>
		/// Converts to a date.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime ToDate(string date)
		{
			return convert(date,new string[]{__defaultFormat1,__defaultFormat2,__defaultFormat3,__defaultFormat4});
		}
		#endregion operators
	}
	/// <summary>
	/// This class provides the rules for a valid customerAccount number.
	/// </summary>
	public class CustomerAccountNumberAttribute:BaseItemAttribute
	{
		#region constants
		/// <summary>
		/// Constant defining error message for this class.
		/// </summary>
		private const string __errorMessage="{0} contains an invalid CustomerAccountNumber value of '{1}'.";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public CustomerAccountNumberAttribute():this(null,__errorMessage){}
		/// <summary>
		/// Constructor when you are only setting the name.
		/// </summary>
		/// <param name="name"></param>
		public CustomerAccountNumberAttribute(string name):
			base(name,__errorMessage){}
		/// <summary>
		/// Protected constructor when you wish to override the errorMessage.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="errorMessage"></param>
		protected CustomerAccountNumberAttribute(string name,string errorMessage)
			:base(name,errorMessage){}
		#endregion ctors

		#region methods
		/// <summary>
		/// Return true, if the object value is valid, false otherwise.
		/// </summary>
		public override bool IsValid(object accountNumber)
		{
			return IsValid(Convert.ToString(accountNumber));
		}
		/// <summary>
		/// Return true, if the string value is a valid 
		/// customeraccountnumber, false otherwise.
		/// </summary>
		/// <param name="accountNumber"></param>
		/// <returns></returns>
		public bool IsValid(string accountNumber)
		{
			try
			{
				return CustomerAccountNumber.ValidateAccountNumber(
					accountNumber.Trim().PadLeft(
					CustomerAccountNumber.knAccountNumberLength,'0'));
			}
			catch
			{
				return false;
			}
		}
		/// <summary>
		/// Throws an exception if it fails validation; else it does nothing.
		/// </summary>
		/// <param name="val"></param>
		public virtual void Validate(object val)
		{
			if(!IsValid(val))throw new ValidationException(GetFormattedError(val));
		}
		#endregion methods
	}
	/// <summary>
	/// This class provides the rules for a valid emailAddress.
	/// </summary>
	public class EmailAddressAttribute:RegExAttribute
	{
		#region constants
		/// <summary>
		/// RegularExpression to use to validate email address.
		/// </summary>
		private const string __emailExpression=@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
		/// <summary>
		/// RegularExpression option to use to validate email address.
		/// </summary>
		private const RegexOptions __options=RegexOptions.None;
		/// <summary>
		/// Constant containing message when given an invalid email address.
		/// </summary>
		protected const string __invalidEmailAddress="Item '{0}' contains an invalid EmailAddress of '{1}'.";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public EmailAddressAttribute():base(null,__emailExpression,__options,__invalidEmailAddress){}
		/// <summary>
		/// Constructor taking name, expression, options and value.
		/// </summary>
		/// <param name="name"></param>
		public EmailAddressAttribute(string name):base(name,__emailExpression,__options,__invalidEmailAddress){}
		/// <summary>
		/// Constructor taking name, expression, options and value.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="errorMessage"></param>
		public EmailAddressAttribute(string name,string errorMessage)
			:base(name,__emailExpression,__options,errorMessage){}
		#endregion ctors
	}
	/// <summary>
	/// This class provides the rules for a valid customerAccount number.
	/// </summary>
	public class SiteIdAttribute:BaseItemAttribute
	{
		#region constants
		/// <summary>
		/// Constant defining error message for this class.
		/// </summary>
		private const string __errorMessage="{0} contains an invalid SiteId value of '{1}'.";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public SiteIdAttribute():this(null,__errorMessage){}
		/// <summary>
		/// Constructor when you are only setting the name.
		/// </summary>
		/// <param name="name"></param>
		public SiteIdAttribute(string name):
			base(name,__errorMessage){}
		/// <summary>
		/// Protected constructor when you wish to override the errorMessage.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="errorMessage"></param>
		protected SiteIdAttribute(string name,string errorMessage)
			:base(name,errorMessage){}
		#endregion ctors

		#region methods
		/// <summary>
		/// Return true, if the object value is valid, false otherwise.
		/// </summary>
		public override bool IsValid(object siteId)
		{
			int id=0;
			// if we cannot convert to int, then its invalid.
			try{id=Convert.ToInt32(siteId);}
			catch{return false;}
			return IsValid(id);
		}
		/// <summary>
		/// Return true, if the string value is a valid 
		/// customeraccountnumber, false otherwise.
		/// </summary>
		/// <returns></returns>
		public bool IsValid(int siteId)
		{
			// if its outside of this range, then its invalid.
			if(siteId<1||siteId>999)return false;
			try
			{
				string siteCode=DalSiteCode.Instance.GetSiteCode(siteId);
				if(siteCode=="N/A")return false;
			}
			catch{return false;}
			return true;
		}
		/// <summary>
		/// Throws an exception if it fails validation; else it does nothing.
		/// </summary>
		/// <param name="val"></param>
		public virtual void Validate(object val)
		{
			if(!IsValid(val))throw new ValidationException(GetFormattedError(val));
		}
		#endregion methods
	}
	/// <summary>
	/// This class provides the rules for a valid emailAddress.
	/// </summary>
	public class NumericAttribute:RegExAttribute
	{
		#region constants
		/// <summary>
		/// RegularExpression to use to validate for numeric characters.
		/// </summary>
		private const string __numericExpression=@"^[-+]?[0-9]\d*\.?[0]*$";
		/// <summary>
		/// RegularExpression option to use to validate.
		/// </summary>
		private const RegexOptions __options=RegexOptions.None;
		/// <summary>
		/// Constant containing message when given an invalid character.
		/// </summary>
		protected const string __errorMessage="Item '{0}' contains one or more non-numeric characters in '{1}'.";
		#endregion constants

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public NumericAttribute():base(null,__numericExpression,__options,__errorMessage){}
		/// <summary>
		/// Constructor taking name, expression, options and value.
		/// </summary>
		/// <param name="name"></param>
		public NumericAttribute(string name):base(name,__numericExpression,__options,__errorMessage){}
		/// <summary>
		/// Constructor taking name, expression, options and value.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="errorMessage"></param>
		public NumericAttribute(string name,string errorMessage)
			:base(name,__numericExpression,__options,errorMessage){}
		#endregion ctors
	}
}
