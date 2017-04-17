using System;
using System.ComponentModel;

namespace Cox.BusinessObjects.CustomerBilling
{
	#region enumerations
	/// <summary>
	/// Maintains the different Icoms Statement Statuses.
	/// This enumeration definition is based on code from Kiosk.
	/// </summary>
	[TypeConverter(typeof(StatementStatusConverter))]
	public enum eStatementStatus
	{
		/// <summary>
		/// Unknown
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// Active
		/// </summary>
		Active				= 1,
		/// <summary>
		/// PendingInstall
		/// </summary>
		PendingInstall		= 2,
		/// <summary>
		/// TempDisconnect
		/// </summary>
		TempDisconnect		= 3,
		/// <summary>
		/// NonPayDisconnect
		/// </summary>
		NonPayDisconnect	= 4,
		/// <summary>
		/// PendingReconnect
		/// </summary>
		PendingReconnect	= 5,
		/// <summary>
		/// Disconnect
		/// </summary>
		Disconnect			= 6,
		/// <summary>
		/// PendingDisconnect
		/// </summary>
		PendingDisconnect	= 7,
		/// <summary>
		/// Cancelled
		/// </summary>
		Cancelled			= 8,
		/// <summary>
		/// PendingChange
		/// </summary>
		PendingChange		= 9
	}
	#endregion enumerations

	#region converter class(es)
	/// <summary>
	/// Converter for converting a statement status.
	/// </summary>
	public class StatementStatusConverter:EnumConverter
	{
		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public StatementStatusConverter():base(typeof(eStatementStatus)){}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Overrides the ConvertFrom method of EnumConverter. The only reason
		/// we are deriving from EnumConverter is to handle converting from an
		/// integer to a potentially invalid value.
		/// 
		/// Since the default EnumConverter handles converting from an enumeration
		/// to another type, we will not modify its behavior.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="culture"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object ConvertFrom(ITypeDescriptorContext context, 
			System.Globalization.CultureInfo culture,object value)
		{
			// check if we care about it
			if(value is int||value is string)
			{
				int val=0;
				try{val=Convert.ToInt32(value);}
				catch{/*do nothing*/}
				return Enum.IsDefined(typeof(eStatementStatus),val)?
					(eStatementStatus)val:eStatementStatus.Unknown;
			}
			else
			{
				return base.ConvertFrom(context,culture,value);
			}
		}
		#endregion public methods
	}
	#endregion converter class(es)
}
