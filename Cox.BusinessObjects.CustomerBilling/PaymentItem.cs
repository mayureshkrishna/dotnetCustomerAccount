using System;
using System.Xml.Serialization;

namespace Cox.BusinessObjects.CustomerBilling
{
	/// <summary>
	/// Contains not only an amount but the date of a payment as well.
	/// </summary>
	public class PaymentItem
	{
		#region member variables
		/// <summary>
		/// Member variable containing the amount of the payment.
		/// </summary>
		protected double _amount=0.0d;
		/// <summary>
		/// Member variable containing the date the payment occurred.
		/// </summary>
		protected DateTime _date=DateTime.MinValue;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public PaymentItem(){}
		/// <summary>
		/// Parameterized constructor
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="date"></param>
		public PaymentItem(double amount, DateTime date)
		{
			_amount=amount;
			_date=date;
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Amount of payment.
		/// </summary>
		[XmlAttribute("Amount")]
		public double Amount
		{
			get{return _amount;}
			set{_amount=value;}
		}
		/// <summary>
		/// Date of payment.
		/// </summary>
		[XmlElement("Date")]
		public DateTime Date
		{
			get{return _date;}
			set{_date=value;}
		}
		#endregion properties
	}
}
