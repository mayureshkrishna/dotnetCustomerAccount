using System;
using Cox.BusinessLogic.Exceptions;
using Cox.BusinessObjects.CustomerBilling;

namespace Cox.BusinessLogic.ConnectionManager
{
	/// <summary>
	/// This class wraps the eBillingOption enumeration
	/// and provides conversions to/from eBillingOption
	/// </summary>
	public class BillingOption
	{
		#region member variables
		/// <summary>
		/// Internal member holding the underlying transaction type.
		/// </summary>
		protected eBillingOption _billingOption = eBillingOption.Unknown;
		#endregion member variables

		#region ctors
		/// <summary>
		/// Constructor that takes a eBillingOption for initialization.
		/// </summary>
		/// <param name="billingOption"></param>
		public BillingOption( eBillingOption billingOption )
		{
			_billingOption=billingOption;
		}
		/// <summary>
		/// Constructor that takes a string for initialization.
		/// It expects either a single string character or a
		/// match to one of the underlying eBillingOption values.
		/// </summary>
		/// <param name="billingOption"></param>
		public BillingOption( string billingOption )
		{
			construct( billingOption );
		}
		/// <summary>
		/// Constructor that takes a string for initialization.
		/// It expects either a single string character or a
		/// match to one of the underlying eBillingOption values.
		/// </summary>
		/// <param name="billingOption"></param>
		protected void construct( string billingOption )
		{
			try
			{ 
				int val = int.Parse( billingOption );
				if( Enum.IsDefined( typeof( eBillingOption ), val ) )
					_billingOption = (eBillingOption) val;
			}
			catch { /* Ignore me */ }
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Returns the underlying transaction type.
		/// </summary>
		public eBillingOption eBillingOption
		{
			get{return _billingOption;}
		}
		#endregion properties

		#region operators
		/// <summary>
		/// operator that takes a string and returns
		/// the underlying eBillingOption value.
		/// </summary>
		/// <param name="billingOption"></param>
		/// <returns></returns>
		public static explicit operator BillingOption( string billingOption )
		{
			return new BillingOption( billingOption );
		}
		/// <summary>
		/// Operator that returns the underlying transaction type.
		/// </summary>
		/// <param name="billingOption"></param>
		/// <returns></returns>
		public static explicit operator eBillingOption( BillingOption billingOption )
		{
			return billingOption.eBillingOption;
		}
		#endregion operators
	}
}
