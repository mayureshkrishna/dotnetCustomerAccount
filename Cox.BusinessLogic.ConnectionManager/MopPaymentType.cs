using System;
using System.Reflection;
using System.ComponentModel;

using Cox.BusinessLogic.Exceptions;
using Cox.BusinessObjects;
using Cox.Validation;
using Cox.DataAccess.Enterprise;
using Cox.BusinessObjects.CustomerBilling;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.BusinessLogic.ConnectionManager
{
	/// <summary>
	/// This class handles converting an Mop type returned from Icoms
	/// into an ePaymentType value recognized by client applications.
	/// </summary>
	public class MopPaymentType
	{
		#region member variables
		/// <summary>
		/// Internal member holding the underlying MopPaymentType
		/// </summary>
		protected ePaymentType _paymentType = ePaymentType.Unknown;
		/// <summary>
		/// Username used in translation.
		/// </summary>
		protected string _userName = null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// Constructor that takes a string for initialization.
		/// It expects either a single string character or a
		/// match to one of the underlying ePaymentType values.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="mopCode"></param>
		public MopPaymentType( string userName, string mopCode )
		{
			try
			{
				int i = int.Parse( mopCode );
				construct(userName,i);
			}
			catch
			{
				_paymentType=ePaymentType.Unknown;
			}
		}
		/// <summary>
		/// Constructor that takes an integer and converts it
		/// to the underlying ePaymentType.
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="mopCode"></param>
		public MopPaymentType( string userName, int mopCode )
		{
			construct(userName,mopCode);
		}
		/// <summary>
		/// Common initialization routine
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="mopCode"></param>
		protected void construct([RequiredItem()]string userName,int mopCode)
		{
			// default to unknown
			_paymentType=ePaymentType.Unknown;

			// load validator to validate if the call is valid. if it's not
			// do not throw an error, in other words, just use IsValid().
			MethodValidator validator=new MethodValidator(
				MethodBase.GetCurrentMethod(),userName,mopCode);
			if(validator.IsValid())
			{
				_userName=userName;
				// Test for negative value or zero... these are invalid
				if( mopCode > 0 )
				{
					try
					{
						// Now translate it using DAL
						DalMethodOfPayment dalMop = new DalMethodOfPayment();
						// get the integer value
						int paymentType = dalMop.GetPaymentTypeByUserMop( userName, mopCode );
						// set internal enumerated type
						_paymentType=(ePaymentType)TypeDescriptor.GetConverter(typeof(
							ePaymentType)).ConvertFrom(paymentType);;
					} // try
					catch//( Exception ex )
					{
						// Publish to allow support staff to correct the problem
						// throw away exceptions because we have already set 
						// _paymentType = Unknown
						//ExceptionManager.Publish( ex );
					}
				}
			}
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Returns the underlying ePaymentType
		/// </summary>
		public ePaymentType ePaymentType
		{
			get{return _paymentType;}
		}
		/// <summary>
		/// UserName property
		/// </summary>
		public string UserName
		{
			get{return _userName;}
		}
		#endregion properties

		#region operators
		/// <summary>
		/// Operator that returns the underlying transaction type.
		/// </summary>
		/// <param name="mopPaymentType"></param>
		/// <returns></returns>
		public static explicit operator ePaymentType( MopPaymentType mopPaymentType )
		{
			return mopPaymentType.ePaymentType;
		}
		#endregion operators
	}
}
