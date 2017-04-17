using System;

namespace Cox.BusinessObjects
{
	/// <summary>
	/// The first 6 digits of your credit card number (including the initial
	/// MII digit) form the Issuer Identifier. This represents the entity
	/// which issued your credit card. These are common types with which
	/// this interface interacts.
	/// </summary>
	[Serializable]
	public enum IssuerType
	{
		/// <summary>
		/// This is provided when a type is not known and cannot be determined.
		/// </summary>
		Unknown				= 0,
		/// <summary>
		/// This represents the Diner's Club/Carte Blanche as an issuer.
		/// </summary>
		DinersClub			= 1,
		/// <summary>
		/// This represents American Express as an issuer.
		/// </summary>
		AmericanExpress		= 2,
		/// <summary>
		/// This represents VISA as an issuer.
		/// </summary>
		Visa				= 3,
		/// <summary>
		/// This represents MasterCard as an issuer.
		/// </summary>
		Mastercard			= 4,
		/// <summary>
		/// This represents Discover as an issuer.
		/// </summary>
		Discover			= 5
	}

}
