using System;
using System.Text;

namespace Cox.BusinessLogic.CustomerBilling
{
	/// <summary>
	/// Summary description for CustomerName.
	/// </summary>
	public class CustomerName
	{
		#region member variables
		/// <summary/>
		protected string _firstName=null;
		/// <summary/>
		protected string _middleName=null;
		/// <summary/>
		protected string _lastName=null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		public CustomerName(string firstName,string lastName)
		{
			_firstName=makeNullIfEmpty(firstName);
			_lastName=makeNullIfEmpty(lastName);
		}
		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="firstName"></param>
		/// <param name="middleName"></param>
		/// <param name="lastName"></param>
		public CustomerName(string firstName,string middleName,string lastName)
		{
			_firstName=makeNullIfEmpty(firstName);
			_middleName=makeNullIfEmpty(middleName);
			_lastName=makeNullIfEmpty(lastName);
		}
		#endregion ctors

		#region properties
		/// <summary>
		/// Get/set FirstName
		/// </summary>
		public string FirstName
		{
			get{return _firstName;}
			set{_firstName=value;}
		}
		/// <summary>
		/// Get/set MiddleName
		/// </summary>
		public string MiddleName
		{
			get{return _middleName;}
			set{_middleName=value;}
		}
		/// <summary>
		/// Get/set LastName
		/// </summary>
		public string LastName
		{
			get{return _lastName;}
			set{_lastName=value;}
		}
		#endregion properties

		#region private/protected functions
		/// <summary>
		/// This method converts an empty string to null so that we can have
		/// consistency defining empty and null values (e.g. both will be null).
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		public string makeNullIfEmpty(string val)
		{
			if(val!=null)
			{
				val=val.Trim();
				return val.Length>0?val:null;
			}
			return null;
		}
		/// <summary>
		/// Returns whether or not the firstName is null.
		/// </summary>
		protected bool isFirstNameNull()
		{
			return _firstName==null;
		}
		/// <summary>
		/// Returns whether or not the middleName is null.
		/// </summary>
		protected bool isMiddleNameNull()
		{
			return _middleName==null;
		}
		/// <summary>
		/// Returns whether or not the lastName is null.
		/// </summary>
		protected bool isLastNameNull()
		{
			return _lastName==null;
		}
		#endregion private/protected functions

		#region operators
		/// <summary>
		/// Converts to a string
		/// </summary>
		/// <param name="customerName"></param>
		/// <returns></returns>
		public static implicit operator string(CustomerName customerName)
		{
			StringBuilder name=new StringBuilder();
			if(!customerName.isFirstNameNull())
				name.Append(customerName.FirstName);
			if(!customerName.isMiddleNameNull())
			{
				if(name.Length>0)
					name.AppendFormat(" {0}",customerName.MiddleName);
				else
					name.Append(customerName.MiddleName);
			}
			if(!customerName.isLastNameNull())
			{
				if(name.Length>0)
					name.AppendFormat(" {0}",customerName.LastName);
				else
					name.Append(customerName.LastName);
			}
			return name.ToString();
		}
		#endregion operators
	}
}
