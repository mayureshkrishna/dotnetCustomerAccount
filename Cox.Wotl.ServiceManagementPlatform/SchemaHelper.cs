using System;

namespace Cox.Wotl.ServiceManagementPlatform
{
	/// <summary>
	/// This class aids in building minimum properties into a SMP request
	/// instance and allows for cleaner code.
	/// </summary>
	public class SmpHelper
	{
		#region Members
		/// <summary>
		/// This is the contained element.
		/// </summary>
		private ITVRequest _element = new ITVRequest();

		#endregion //Members

		#region Construction/Destruction
		/// <summary>
		/// This constructor aids in the creation of the class in the
		/// SMP service agent.
		/// </summary>
		/// <param name="objItem">
		/// An instance of one of the request classes (i.e. GetEmailInfo,
		/// ProvisionPin, etc.).
		/// </param>
		public SmpHelper( object item )
		{
			_element.Item = item;
		} 
		#endregion 

		#region Properties

		/// <summary>
		/// This property allows access to the contained element.
		/// </summary>
		public ITVRequest Element
		{
			get{ return _element; }
		} 

		#endregion

		#region Implicit Operators
		/// <summary>
		/// This operator allows the helper class interact with 
		/// SMP functionality without extra manipulation code.
		/// </summary>
		/// <param name="hlpr">An instance of this helper class.</param>
		/// <returns>An instance of the contained class.</returns>
		public static implicit operator ITVRequest(SmpHelper helper) 
		{
			return helper._element;
		} 
		#endregion 
	} 	
	/// <summary>
	/// This class aids in building minimum properties into a GetEmailInfo request
	/// instance and allows for cleaner code.
	/// </summary>
	public class GetEmailInformationHelper
	{
		#region Members
		/// <summary>
		/// This is the contained element.
		/// </summary>
		private ITVRequestGetEmailInfo2 _element = new ITVRequestGetEmailInfo2();
		#endregion 

		#region Construction/Destruction

		public GetEmailInformationHelper(string accountNumber9, int siteId)
		{
			string accountNumber13 = siteId.ToString() + accountNumber9;
			_element.IcomsID = accountNumber13;
		} 
		#endregion 

		#region Properties

		/// <summary>
		/// This property allows access to the contained element.
		/// </summary>
		public ITVRequestGetEmailInfo2 Element
		{
			get{ return _element; }
		} 
		#endregion // Properties

		#region Implicit Operators

		/// <summary>
		/// This operator allows the helper class interact with Connection
		/// Manager functionality without extra manipulation code.
		/// </summary>
		/// <param name="hlpr">An instance of this helper class.</param>
		/// <returns>An instance of the contained class.</returns>
		public static implicit operator ITVRequestGetEmailInfo2(GetEmailInformationHelper helper) 
		{
			return helper._element;
		} 
		#endregion 

	} 

	/// <summary>
	/// This class aids in building minimum properties into a ProvisionPin request
	/// instance and allows for cleaner code.
	/// </summary>
	public class ProvisionPinHelper
	{
		#region Members
		/// <summary>
		/// This is the contained element.
		/// </summary>
		private ITVRequestProvisionPin _element = new ITVRequestProvisionPin();
		#endregion 

		#region Construction/Destruction
		/// <summary>
		/// Need this for serialization.
		/// </summary>
		public ProvisionPinHelper(){}
		/// <summary>
		/// main constructor.
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <param name="emailPassword"></param>
		/// <param name="pin"></param>
		public ProvisionPinHelper(string emailAddress, string emailPassword, string pin)
		{
			_element.EmailAddress = emailAddress;
			_element.EmailPassword = emailPassword;
			_element.PinNumber = pin;
		} 
		#endregion 

		#region Properties

		/// <summary>
		/// This property allows access to the contained element.
		/// </summary>
		public ITVRequestProvisionPin Element
		{
			get{ return _element; }
		} 
		#endregion // Properties

		#region Implicit Operators

		/// <summary>
		/// This operator allows the helper class interact with Connection
		/// Manager functionality without extra manipulation code.
		/// </summary>
		/// <param name="hlpr">An instance of this helper class.</param>
		/// <returns>An instance of the contained class.</returns>
		public static implicit operator ITVRequestProvisionPin(ProvisionPinHelper helper) 
		{
			return helper._element;
		} 
		#endregion 
	} 

	/// <summary>
	/// This class aids in building minimum properties into a ProvisionPin request
	/// instance and allows for cleaner code.
	/// </summary>
	public class GetEmailPropertiesHelper
	{
		#region Members
		/// <summary>
		/// This is the contained element.
		/// </summary>
		private ITVRequestGetEmailProperties _element = new ITVRequestGetEmailProperties();
		#endregion 

		#region Construction/Destruction
		/// <summary>
		/// Need this for serialization.
		/// </summary>
		public GetEmailPropertiesHelper(){}
		/// <summary>
		/// Main method for constructions
		/// </summary>
		/// <param name="emailAddress"></param>
		/// <param name="pin"></param>
		public GetEmailPropertiesHelper(string emailAddress, string pin)
		{
			_element.EmailAddress = emailAddress;
			_element.PinNumber = pin;
		} 
		#endregion 

		#region Properties

		/// <summary>
		/// This property allows access to the contained element.
		/// </summary>
		public ITVRequestGetEmailProperties Element
		{
			get{ return _element; }
		} 
		#endregion // Properties

		#region Implicit Operators

		/// <summary>
		/// This operator allows the helper class interact with Connection
		/// Manager functionality without extra manipulation code.
		/// </summary>
		/// <param name="hlpr">An instance of this helper class.</param>
		/// <returns>An instance of the contained class.</returns>
		public static implicit operator ITVRequestGetEmailProperties(GetEmailPropertiesHelper helper) 
		{
			return helper._element;
		} 
		#endregion 
	} 	
}
