using System;


namespace Cox.BusinessObjects.CustomerBilling
{

	#region Enumerations

	public enum ePaymentType
	{
		Unknown				= -1,
		AmericanExpress		= 1,
		Discover			= 2,
		MasterCard			= 3,
		Visa				= 4,
		ElectronicCheck		= 5,
		DirectDebit			= 6,
		DirectDebitOneTime	= 7
		// TODO: Payment types
	} // enum ePaymentType

	#endregion // Enumerations

} // namespace Cox.BusinessObjects.CustomerBilling
