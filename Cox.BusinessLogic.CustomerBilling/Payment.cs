using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Cox.BusinessLogic;
using Cox.BusinessLogic.ConnectionManager;
using Cox.BusinessLogic.Exceptions;
using Cox.BusinessLogic.Validation;

using Cox.ActivityLog;
using Cox.ActivityLog.CustomerBilling;

using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerBilling;

using Cox.DataAccess;
using Cox.DataAccess.Account;
using Cox.DataAccess.CustomerBilling;
using Cox.DataAccess.Exceptions;
using Cox.DataAccess.Enterprise;

using Cox.ServiceAgent.ConnectionManager;
using Request=Cox.ServiceAgent.ConnectionManager.Request;
using Response=Cox.ServiceAgent.ConnectionManager.Response;

using Cox.Validation;


namespace Cox.BusinessLogic.CustomerBilling
{
	/// <summary>
	/// This Business Logic Layer facilitates the payment of customer
	/// statements.
	/// </summary>
	public class Payment : BllConnectionManager
	{
		#region Constants
		/// <summary>Error message for stopped check</summary>
		private const string __stoppedCheckErrorMessage="The bank information provided cannot be used to make payments because the administrator has placed a stop on it.";
		/// <summary>Error message for nsf check</summary>
		private const string __nsfErrorMessage="The given payment type is not supported for this customer.";
		/// <summary>Debit card number not found in BIN file</summary>
		private const string __debitCardNotFoundInBinFileMessage="The debit card number provided was not found in the BIN file.";
		/// <summary>Date used to send across to ICOMS for transaction not trulyexpecting a date. This is required when the DTD specifies a requiredfield for a date that is not used in ICOMS.</summary>
		private readonly DateTime __icomsSpecialDate=new DateTime( 2099, 12, 31 );
		#endregion
		
		#region Members
		/// <summary>
		/// This member tracks the customer account number used for the 
		/// current set of transactions.
		/// </summary>
		private CustomerAccountNumber m_can=null;
		#endregion Members

		#region Construction/Destruction
		/// <summary>
		/// The default contructor should not be visible.
		/// </summary>
		private Payment():base(null){}
		/// <summary>
		/// This is the primary constructor for this class. The username is
		/// passed to the base class BllConnectionManager to allow for CM
		/// credentials population. 
		/// </summary>
		/// <param name="strUserName">
		/// This username is passed to the base class BllConnection Manager
		/// to populate CM credentials.
		/// </param>
		/// <param name="strAccountNumber16">
		/// This account number is used to query site information by the base 
		/// class BllCustomer.
		/// </param>
		public Payment([RequiredItem()]string strUserName,
			[RequiredItem()][StringLength(16,16)][CustomerAccountNumber()]string strAccountNumber16)
			:base(strUserName)
		{
			// validate the parameters.
			ConstructorValidator validator=new ConstructorValidator(ConstructorInfo.GetCurrentMethod(),strUserName, strAccountNumber16);
			validator.Validate();
						
			// convert the accountNumber.
			m_can=(CustomerAccountNumber)TypeDescriptor.GetConverter(
				typeof(CustomerAccountNumber)).ConvertFrom(strAccountNumber16);
			// NOTE: This is a fix to work around a Connection Manager bug.
			// When an account number has statement code equal to "000," this
			// causes Connection Manger to blow up for the entire site and 
			// return the error message similar to "Cannot start IGI
			// interface". We are now kicking these references out here.
			if( "000" == m_can.StatementCode )throw new InvalidStatementCodeException();
			// Initialize the service agent
			CreateProxy(m_can);
		}
		#endregion Construction/Destruction

		#region One-time Payment Methods
		/// <summary>
		/// This method provides functionality to pay a statement using
		/// electronic check as the method of payment.
		/// </summary>
		/// <param name="strCustomerName"></param>
		/// <param name="strBankRouteNumber"></param>
		/// <param name="strBankAccountNumber"></param>
		/// <param name="ebatAccountType"></param>
		/// <param name="dblAmount"></param>
		/// <returns></returns>
		public PaymentReceipt PayUsingElectronicCheck(
			string strCustomerName,
			[RequiredItem()][StringLength(1,9)][NumericAttribute()]string strBankRouteNumber,
			[RequiredItem()][StringLength(1,20)]string strBankAccountNumber,
			[RequiredItem()]eBankAccountType ebatAccountType,
			[RequiredItem()][DoubleRange(0.00,9999999.99)]double dblAmount)
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.PayCheck, 
				this.m_can.AccountNumber16, dblAmount );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),
						strCustomerName,strBankRouteNumber,strBankAccountNumber,ebatAccountType,dblAmount);
					validator.Validate();

					// In this case, we are hard-coding the payment type
					ePaymentType ept = ePaymentType.ElectronicCheck;
					logEntry.PaymentType = ept;

					// set the siteId information					
					logEntry.SiteId = SiteId;

					//use stopped check dal to verfiy the account does not have a stop on it
					DalStoppedCheck dalSC = new DalStoppedCheck();
					if(dalSC.IsStoppedCheck(strBankRouteNumber,strBankAccountNumber))
					{
						//the account has a stop placed on it
						throw new MopAuthorizationFailedException(string.Format(__stoppedCheckErrorMessage + "\nBank Routing Number: {0} \nBank Account Number: {1}", strBankRouteNumber, strBankAccountNumber));
					}
					
					checkNSFStatus(false);

					// Create a DAL to transalate Mop codes
					DalMethodOfPayment dal = new DalMethodOfPayment();
					DalPaymentReceipt dalPaymentReceipt = new DalPaymentReceipt();

					string paymentReceiptType = dalPaymentReceipt.GetPaymentReceiptType(_userId);
					paymentReceiptType = paymentReceiptType == string.Empty?CmConstant.kstrDefaultReceiptType:paymentReceiptType;

					PaymentReceipt rcpt = new PaymentReceipt();

					// need to get the customer's name.
					if(strCustomerName!=null)strCustomerName=strCustomerName.Trim();
					if(strCustomerName==null||strCustomerName.Length==0)
					{
						DalAccount dalAccount=new DalAccount();
						CustomerAccountSchema.CustomerName custName=
							dalAccount.GetCustomerName(_siteId,_siteCode,m_can.AccountNumber9);
						if(custName==null) throw new InvalidAccountNumberException();
						strCustomerName=(string)new CustomerName(custName.FirstName,custName.MiddleInitial,custName.LastName);
						if(strCustomerName==null||strCustomerName.Length==0)strCustomerName=CmConstant.kstrDefaultAccountTitle;
					}

					// assure that the length of the customer name does NOT exceed 32 characters!
					if (strCustomerName.Length >= 32)strCustomerName=strCustomerName.Substring(0, 31);

				// Build input elements
				Request.INL00047 inl47 = new INL00047Helper(dblAmount,dblAmount,
						( eBankAccountType.Checking == ebatAccountType )?
						CmConstant.kstrAccountTypeChecking:
						CmConstant.kstrAccountTypeSavings,
						CmConstant.kstrNegative,strBankRouteNumber,strBankAccountNumber,
						strCustomerName,dal.GetMopByUserPaymentType(
						UserName,(int)ept),CmConstant.kstrNegative,m_can.StatementCode,
						paymentReceiptType,CmConstant.kstrDefaultWorkstation );

					Request.MAC00027 mac27 =
						new Mac00027Helper(SiteId.ToString(),m_can.AccountNumber9,
						CmConstant.kstrDefaultTaskCode,inl47 );

					// Use inherited functions to get a response
					Response.MAC00027 mac27Response = (Response.MAC00027)
						this.Invoke( (Request.MAC00027) mac27 );
					Response.INL00047 inl47Response = mac27Response.Items[0] as Response.INL00047;

					int intErrorCode = toInt32( inl47Response.IGIRTRNCODE );
					if( intErrorCode > 0 )
						throw TranslateCmException(
							intErrorCode,
							string.Format( "Authorization failed with error - ErrorCode: {0} ErrorText: {1}",
							inl47Response.IGIRTRNCODE,
							inl47Response.IGIMESGTEXT ),
							null );

					rcpt.AccountNumber16	= m_can.AccountNumber16;
					rcpt.AmountPaid			= ( inl47Response.AMNTTOAPLYUSR.Length > 0 ) ? Double.Parse( inl47Response.AMNTTOAPLYUSR ): 0.00;
					rcpt.PaymentType		= ePaymentType.ElectronicCheck;
					rcpt.Status				= ePaymentStatus.Success;
					rcpt.TransactionDate	= new IcomsDate( inl47Response.ATHRZTNDATE ).Date;

					return rcpt;
				} // try
				catch(BusinessLogicLayerException blle)
				{
					//the dal threw an exception
					logEntry.SetError(blle.Message);
					throw;
				}
				catch (DataSourceException excDSE)
				{
					//the dal threw an exception
					logEntry.SetError( excDSE.Message );
					throw new DataSourceUnavailableException(excDSE);
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				} // catch( CmErrorException excCm )
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				} // catch( Exception exc )

			} // using( Log log =  )

		} // PayUsingElectronicCheck()

		/// <summary>
		/// This method provides functionality to pay a statement using
		/// a credit card as the method of payment.
		/// </summary>
		/// <param name="strCreditCardNumber">
		/// Thie customer credit card number used to pay the bill.
		/// </param>
		/// <param name="strNameOnCard">
		/// The name on the customer credit card.
		/// </param>
		/// <param name="strExpirationDate">
		/// The expiration date of the customer credit card.
		/// </param>
		/// <param name="dblAmount">
		/// The amount to be paid to the customer service account.
		/// </param>
		/// <returns>
		/// A payment receipt instance is returned with the results from
		/// the requested payment.
		/// </returns>
		public PaymentReceipt PayUsingCreditCard(
			[RequiredItem()][CreditCardNumberAttribute()]string strCreditCardNumber,
			[RequiredItem()]string strNameOnCard,
			[RequiredItem()][ValidCCDate()]string strExpirationDate,
			[RequiredItem()][DoubleRange(0.00,9999999.99)]double dblAmount)
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.PayCredit, 
				this.m_can.AccountNumber16, dblAmount );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),
						strCreditCardNumber,strNameOnCard,strExpirationDate,dblAmount);
					validator.Validate();

					// convert the accountNumber.
					CreditCardNumber creditCardNumber=(CreditCardNumber)TypeDescriptor.GetConverter(
						typeof(CreditCardNumber)).ConvertFrom(strCreditCardNumber);

					// Credit card validation
					logEntry.PaymentType = creditCardNumber.PaymentType;
					DateTime dttmExpirationDate=ValidCCDateAttribute.ToDate(strExpirationDate);

					// set the siteId information					
					logEntry.SiteId = SiteId;

					checkNSFStatus(true);

					// Create a DAL to transalate Mop codes
					DalMethodOfPayment dal = new DalMethodOfPayment();
					DalPaymentReceipt dalPaymentReceipt = new DalPaymentReceipt();

					string paymentReceiptType = dalPaymentReceipt.GetPaymentReceiptType(_userId);
					paymentReceiptType = paymentReceiptType == string.Empty?CmConstant.kstrDefaultReceiptType:paymentReceiptType;

					PaymentReceipt rcpt = new PaymentReceipt();

					// assure that the length of the customer name does NOT exceed 32 characters!
					if (strNameOnCard.Length >= 32)strNameOnCard=strNameOnCard.Substring(0, 31);

					// Build input elements
					Request.INL00072 inl72 = 
						new INL00072Helper(
							dblAmount,
							dblAmount,
							CmConstant.kstrNegative,
							creditCardNumber.AccountNumber,
							strNameOnCard,
							dttmExpirationDate,
							dal.GetMopByUserPaymentType(UserName,(int)creditCardNumber.PaymentType),
							CmConstant.kstrNegative,
							m_can.StatementCode,
							paymentReceiptType,
							CmConstant.kstrDefaultWorkstation );

					Request.MAC00027 mac27 =
						new Mac00027Helper(
							SiteId.ToString(),
							m_can.AccountNumber9,
							CmConstant.kstrDefaultTaskCode,
							inl72 );

					// Use inherited functions to get a response
					Response.MAC00027 mac27Response = (Response.MAC00027)
						this.Invoke( (Request.MAC00027) mac27 );
					Response.INL00072 inl72Response = mac27Response.Items[0] as Response.INL00072;

					int intErrorCode = toInt32( inl72Response.IGIRTRNCODE );
					if( intErrorCode > 0 )
						throw TranslateCmException(
							intErrorCode,
							string.Format( "Authorization failed with error - ErrorCode: {0} ErrorText: {1}",
							inl72Response.IGIRTRNCODE,
							inl72Response.IGIMESGTEXT ),
							null );

					rcpt.AccountNumber16	= m_can.AccountNumber16;
					rcpt.AmountPaid			= ( inl72Response.AMNTTOAPLY.Length > 0 ) ? Double.Parse( inl72Response.AMNTTOAPLY ): 0.00;
					rcpt.PaymentType		= (ePaymentType)new MopPaymentType( this.UserName, inl72Response.MTHDOFPAYCODE );
					rcpt.Status				= ePaymentStatus.Success;
					rcpt.TransactionDate	= new IcomsDate( inl72Response.ATHRZTNDATE ).Date;

					return rcpt;
				} // try
				catch(BusinessLogicLayerException blle)
				{
					//the dal threw an exception
					logEntry.SetError(blle.Message);
					throw;
				}
				catch (DataSourceException excDSE)
				{
					//the dal threw an exception
					logEntry.SetError( excDSE.Message );
					throw new DataSourceUnavailableException(excDSE);
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				} // catch( CmErrorException excCm )
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				} // catch( Exception exc )

			} // using( Log log = CreateLog( logEntry ) )
		} // PayUsingCreditCard()

		/// <summary>
		/// This method provides functionality to pay a statement using
		/// a pinless debit card as the method of payment.
		/// </summary>
		/// <param name="debitCardNumber">
		/// The customer debit card number used to pay the bill.
		/// </param>
		/// <param name="nameOnCard">
		/// The name on the customer debit card.
		/// </param>
		/// <param name="amount">
		/// The amount to be paid to the customer service account.
		/// </param>
		/// <returns>
		/// A payment receipt instance is returned with the results from
		/// the requested payment.
		/// </returns>
		public PaymentReceipt PayUsingPinlessDebitCard(
			[RequiredItem()][RegEx(@"^\d{12,19}$", RegexOptions.None)]string debitCardNumber,
			[RequiredItem()][StringLength(1,32)] string nameOnCard,
			[RequiredItem()][DoubleRange(0.00,9999999.99)]double amount )
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.PayPinlessDebit, 
				this.m_can.AccountNumber16, amount );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{
					MethodValidator validator=new MethodValidator(
						MethodBase.GetCurrentMethod(),
						debitCardNumber, nameOnCard, amount );
					validator.Validate();
	
					// See if debit card number exists in BIN file.
					// If not, it's invalid...need to throw an
					// exception.
					if( ! _IsDebitCardNumberValid( debitCardNumber ) )
					{
						throw new InvalidDebitCardNumberException( __debitCardNotFoundInBinFileMessage );
					}

					// Debit card validation
					logEntry.PaymentType = ePaymentType.PinlessDebit;

					// set the siteId information					
					logEntry.SiteId = SiteId;

					// Set to false since it's a pinless debit transaction
					checkNSFStatus( false );

					// Create a DAL to transalate Mop codes
					DalMethodOfPayment dal = new DalMethodOfPayment();
					DalPaymentReceipt dalPaymentReceipt = new DalPaymentReceipt();

					string paymentReceiptType = dalPaymentReceipt.GetPaymentReceiptType(_userId);
					paymentReceiptType = paymentReceiptType == string.Empty?CmConstant.kstrDefaultReceiptType:paymentReceiptType;

					// assure that the length of the customer name does NOT exceed 32 characters!
					if( nameOnCard.Length >= 32 )
						nameOnCard = nameOnCard.Substring( 0, 31 );

					// Build input elements
					Request.INL00072 inl72 = 
						new INL00072Helper(
						amount,
						amount,
						CmConstant.kstrNegative,
						debitCardNumber,
						nameOnCard,
						__icomsSpecialDate,
						dal.GetMopByUserPaymentType( UserName,(int) ePaymentType.PinlessDebit ),
						CmConstant.kstrNegative,
						m_can.StatementCode,
						paymentReceiptType,
						CmConstant.kstrDefaultWorkstation );

					Request.MAC00027 mac27 = new Mac00027Helper(
                        SiteId.ToString(),
						m_can.AccountNumber9,
						CmConstant.kstrDefaultTaskCode,
						inl72 );

					// Use inherited functions to get a response
					Response.MAC00027 mac27Response = (Response.MAC00027)
						this.Invoke( (Request.MAC00027) mac27 );
					Response.INL00072 inl72Response = mac27Response.Items[0] as Response.INL00072;

					int intErrorCode = toInt32( inl72Response.IGIRTRNCODE );
					if( intErrorCode > 0 )
						throw TranslateCmException(
							intErrorCode,
							string.Format( "Authorization failed with error - ErrorCode: {0} ErrorText: {1}",
							inl72Response.IGIRTRNCODE,
							inl72Response.IGIMESGTEXT ),
							null );

					PaymentReceipt rcpt		= new PaymentReceipt();
					rcpt.AccountNumber16	= m_can.AccountNumber16;
					rcpt.AmountPaid			= ( inl72Response.AMNTTOAPLY.Length > 0 ) ? Double.Parse( inl72Response.AMNTTOAPLY ): 0.00;
					rcpt.PaymentType		= (ePaymentType) new MopPaymentType( this.UserName, inl72Response.MTHDOFPAYCODE );
					rcpt.Status				= ePaymentStatus.Success;
					rcpt.TransactionDate	= new IcomsDate( inl72Response.ATHRZTNDATE ).Date;

					return rcpt;
				}
				catch( BusinessLogicLayerException blle )
				{
					//the dal threw an exception
					logEntry.SetError( blle.Message );
					throw;
				}
				catch( DataSourceException excDSE )
				{
					//the dal threw an exception
					logEntry.SetError( excDSE.Message );
					throw new DataSourceUnavailableException( excDSE );
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				}
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				}
			}
		}

		#endregion // One-time Payment Methods

		#region BIN file validation method

		/// <summary>
		/// Validates the debit card number against the BIN file
		/// </summary>
		/// <param name="debitCardNumber"></param>
		/// <returns></returns>
		public bool IsDebitCardNumberValid(
			[RequiredItem()][RegEx(@"^\d{12,19}$", RegexOptions.None)]string debitCardNumber)
		{
			//Validate the arguments
			MethodValidator validator = new MethodValidator(
				MethodBase.GetCurrentMethod(), debitCardNumber);
			validator.Validate();

			return _IsDebitCardNumberValid( debitCardNumber );
		}

		#endregion BIN file validation method

		#region recurring payment methods
		/// <summary>
		/// Sets up the CustomerAccount to pay for services on a recurring basis 
		/// using a credit card.
		/// </summary>
		/// <param name="strCreditCardNumber"></param>
		/// <param name="strNameOnCard"></param>
		/// <param name="strExpirationDate"></param>
		public void ActivateRecurringUsingCreditCard(
			[RequiredItem()][CreditCardNumber()] string strCreditCardNumber,
			[RequiredItem()] string strNameOnCard,
			[RequiredItem()][ValidCCDate()]string strExpirationDate)
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.RecurringCredit, 
				this.m_can.AccountNumber16 );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{

					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),
						strCreditCardNumber,strNameOnCard,strExpirationDate);
					validator.Validate();

					// convert the accountNumber.
					CreditCardNumber creditCardNumber=(CreditCardNumber)TypeDescriptor.GetConverter(
						typeof(CreditCardNumber)).ConvertFrom(strCreditCardNumber);

					// Credit card validation
					logEntry.PaymentType = creditCardNumber.PaymentType;
					DateTime dttmExpirationDate=ValidCCDateAttribute.ToDate(strExpirationDate);
					// set the siteId information					
					logEntry.SiteId = SiteId;

					// Create a DAL to transalate Mop codes
					DalMethodOfPayment dal = new DalMethodOfPayment();

					int intStatementCode=0;
					try{intStatementCode=int.Parse(m_can.StatementCode);}catch{/*don't care*/}

					// check nsf status
					checkNSFStatus(true);

					// assure that the length of the customer name does NOT exceed 32 characters!
					if (strNameOnCard.Length >= 32)strNameOnCard=strNameOnCard.Substring(0, 31);

					// Build input elements
					Request.INL00073 inl73 = new INL00073Helper(dal.GetMopByUserPaymentType(
						UserName,(int)creditCardNumber.PaymentType),creditCardNumber.AccountNumber,
						strNameOnCard,dttmExpirationDate,intStatementCode,false);
					Request.MAC00027 mac27 = new Mac00027Helper(SiteId.ToString(),
						m_can.AccountNumber9,CmConstant.kstrDefaultTaskCode,inl73 );

					// invoke and get the response object.
					this.Invoke( (Request.MAC00027)mac27 );
				}
				catch(BusinessLogicLayerException blle)
				{
					//the dal threw an exception
					logEntry.SetError(blle.Message);
					throw;
				}
				catch (DataSourceException excDSE)
				{
					//the dal threw an exception
					logEntry.SetError( excDSE.Message );
					throw new DataSourceUnavailableException(excDSE);
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				}
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				}
			}
		}
		/// <summary>
		/// Sets up the CustomerAccount to pay for services on a recurring basis 
		/// using a credit card.
		/// </summary>
		/// <param name="strCustomerName"></param>
		/// <param name="strBankRouteNumber"></param>
		/// <param name="strBankAccountNumber"></param>
		/// <param name="ebatAccountType"></param>
		public void ActivateRecurringUsingDirectDebit(
			string strCustomerName,
			[RequiredItem()][StringLength(1,9)][NumericAttribute()]string strBankRouteNumber,
			[RequiredItem()][StringLength(1,20)]string strBankAccountNumber,
			[RequiredItem()]eBankAccountType ebatAccountType)
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.RecurringCheck, 
				this.m_can.AccountNumber16 );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),
						strCustomerName,strBankRouteNumber,strBankAccountNumber,ebatAccountType);
					validator.Validate();

					// In this case, we are hard-coding the payment type
					ePaymentType ept=ePaymentType.RecurringDirectDebit;
					logEntry.PaymentType=ept;

					// set the siteId information					
					logEntry.SiteId = SiteId;

					int intStatementCode=0;
					try{intStatementCode=int.Parse(m_can.StatementCode);}
					catch{/*don't care*/}

					//use stopped check dal to verfiy the account does not have a stop on it
					DalStoppedCheck dalSC = new DalStoppedCheck();
					if(dalSC.IsStoppedCheck(strBankRouteNumber,strBankAccountNumber))
					{
						//the account has a stop placed on it
						throw new MopAuthorizationFailedException(string.Format(__stoppedCheckErrorMessage + "\nBank Routing Number: {0} \nBank Account Number: {1}", strBankRouteNumber, strBankAccountNumber));
					}
					
					checkNSFStatus(false);

					// Create a DAL to transalate Mop codes
					DalMethodOfPayment dal = new DalMethodOfPayment();

					// need to get the customer's name.
					if(strCustomerName!=null)strCustomerName=strCustomerName.Trim();
					if(strCustomerName==null||strCustomerName.Length==0)
					{
						DalAccount dalAccount=new DalAccount();
						CustomerAccountSchema.CustomerName custName=
							dalAccount.GetCustomerName(_siteId,_siteCode,m_can.AccountNumber9);
						if(custName==null) throw new InvalidAccountNumberException();
						strCustomerName=(string)new CustomerName(custName.FirstName,custName.MiddleInitial,custName.LastName);
						if(strCustomerName==null||strCustomerName.Length==0)strCustomerName=CmConstant.kstrDefaultAccountTitle;
					}

					// assure that the length of the customer name does NOT exceed 32 characters!
					if (strCustomerName.Length >= 32)strCustomerName=strCustomerName.Substring(0, 31);

					// Build input elements
					Request.INL00074 inl74 =
						new INL00074Helper(dal.GetMopByUserPaymentType(UserName,(int)ept),
						strBankAccountNumber,strBankRouteNumber,strCustomerName,
						(char)TypeDescriptor.GetConverter(typeof(eBankAccountType)).ConvertTo(ebatAccountType,typeof(char)),
						intStatementCode,false);
					Request.MAC00027 mac27 =new Mac00027Helper(SiteId.ToString(),
						m_can.AccountNumber9,CmConstant.kstrDefaultTaskCode,inl74 );

					this.Invoke((Request.MAC00027)mac27);
				}
				catch(BusinessLogicLayerException blle)
				{
					//the dal threw an exception
					logEntry.SetError(blle.Message);
					throw;
				}
				catch (DataSourceException excDSE)
				{
					//the dal threw an exception
					logEntry.SetError( excDSE.Message );
					throw new DataSourceUnavailableException(excDSE);
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				}
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				}
			}
		}
		/// <summary>
		/// Turn off EasyPay (Recurring Payments for the specified account.
		/// </summary>
		public void DeactivateRecurring()
		{
			// setup logging
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.DeactivateRecurring, 
				this.m_can.AccountNumber16 );
			using( Log log = CreateLog( logEntry ) )
			{
				// declare needed variables
				int intStatementCode=0,intMopAccountSequence=0,intMopCode=0;
				// no need to wrap in try/catch (its already been validated).
				int intAccountStatementCode=int.Parse(m_can.StatementCode);
				// set the siteId information					
				logEntry.SiteId = SiteId;

				try
				{
					// execute ACSUM to get needed information
					Response.ACSUM acsumOutput = (Response.ACSUM) Invoke((Request.ACSUM)
						new AcsumHelper(SiteId.ToString(),m_can.AccountNumber9) );

					// format the output
					if( acsumOutput.Items != null )
					{
						// we need to look for the correct statement.
						foreach( object item in acsumOutput.Items )
						{
							if( item is Response.SQLSTMNT )
							{
								// cast to correct type.
								Response.SQLSTMNT icomsStatement = (Response.SQLSTMNT)item;

								// now convert statementCode to an integer.
								int intLocalStatementCode=0;
								try{intLocalStatementCode=int.Parse(icomsStatement.STMNTCD);}
								catch{/*don't care*/}

								// if we did not find the correct statement code then break now.
								if(intLocalStatementCode==intAccountStatementCode)
								{
									// set this.
									intStatementCode=intLocalStatementCode;
									try{intMopAccountSequence=int.Parse(icomsStatement.SQLSTMNTBAS[0].MOPACNTSQNC);}
									catch{/*don't care*/}

									try{intMopCode=int.Parse(icomsStatement.SQLSTMNTBAS[0].MOPCD);}
									catch{/*don't care*/}
									break;
								}
							}
						}
					}

					// now let's validate to see if we found the statement.
					if(intStatementCode==0) throw new InvalidStatementCodeException();

					// if we have a statementCode but no MopCode, then recurring 
					// payments are not setup and there is no point in going further
					if(intMopCode!=0)
					{
						// Build input elements
						Request.INL00076 inl76 = new INL00076Helper(intMopCode,
							intMopAccountSequence,intStatementCode);

						// build mac27 call.
						Request.MAC00027 mac27 =new Mac00027Helper(SiteId.ToString(),
							m_can.AccountNumber9,CmConstant.kstrDefaultTaskCode,inl76 );

						// now invoke.
						this.Invoke((Request.MAC00027)mac27);
					}
				}
				catch( CmErrorException excCm )
				{
					logEntry.SetError( excCm.Message, excCm.ErrorCode );
					throw TranslateCmException( excCm );
				}
				catch( Exception exc )
				{
					logEntry.SetError( exc.Message );
					throw;
				}
			}
		}
		#endregion recurring payment methods

		#region private/protected methods
		/// <summary>
		/// This method checks the customers NSFStatus and throws the BLL Exception
		/// PaymentTypeNotAcceptedException if the paymenttype is not accepted for the customer.
		/// 
		/// if param is true, we check for credit card. if it is false, we check for debit card/check.
		/// </summary>
		/// <param name="creditCard"></param>
		protected void checkNSFStatus(bool creditCard)
		{
			// check nsf flag to see if we accept payment type by customer.
			DalAccount dalAccount=new DalAccount();
			string nsf=dalAccount.GetNSFCode(_siteId,_siteCode,m_can.AccountNumber9);
			eNSFStatus nsfStatus=(eNSFStatus)TypeDescriptor.GetConverter(
				typeof(eNSFStatus)).ConvertFrom(nsf);
			if((!creditCard&&(nsfStatus==eNSFStatus.BlockDebitAndChecks||nsfStatus==eNSFStatus.BlockAll))
				||
				(creditCard&&(nsfStatus==eNSFStatus.BlockCreditCards||nsfStatus==eNSFStatus.BlockAll)))
			{
				throw new MopAuthorizationFailedException(__nsfErrorMessage);
			}
		}

		/// <summary>
		/// This internal method validates the debit card number against the BIN
		/// file. This is common functionality between the
		/// IsDebitCardNumberValid() and PayUsingPinlessDebitCard() service
		/// calls. This prevents the scenario of performing double validation on
		/// the card number.
		/// </summary>
		/// <param name="debitCardNumber"></param>
		/// <returns></returns>
		protected bool _IsDebitCardNumberValid( string debitCardNumber )
		{
			try
			{
				DalPinlessDebit dal = new DalPinlessDebit();
				return dal.IsPinlessDebitCard( debitCardNumber );
			}
			catch( DataSourceException exc )
			{
				throw new DataSourceUnavailableException( exc );
			}
			// NOTE: Without the need to log, there is no reason to catch any
			// other exceptions here. Doing so will create a large performance
			// hit over allowing the exception to fall out.
		}


		#endregion private/protected methods

	} // class Payment

} // namespace Cox.BusinessLogic.CustomerBilling

