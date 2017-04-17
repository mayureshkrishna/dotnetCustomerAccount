using System;

namespace Cox.BusinessObjects
{
	/// <summary>
	/// The Service Feature Location 	
	/// </summary>
	public enum ServiceFeatureLocation
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown				= -1,
		/// <summary>
		/// This represents the Oracle Phastage location.
		/// </summary>
		Phastage			= 1,
		/// <summary>
		/// This represents the ICOMS Infinys location.
		/// </summary>
		Infinys				= 2,
		
	}

}
