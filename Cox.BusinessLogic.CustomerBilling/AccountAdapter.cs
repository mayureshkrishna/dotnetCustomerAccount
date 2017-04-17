using System;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;
using Cox.DataAccess.Account;
using Cox.BusinessLogic;
using Cox.BusinessLogic.Validation;
using Cox.BusinessLogic.Exceptions;
using Cox.Validation;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerBilling;
using Cox.BusinessLogic.ConnectionManager;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.BusinessLogic.CustomerBilling
{
	/// <summary>
	/// Converts to a PricingDetail object.
	/// </summary>
	public class AccountAdapter
	{
		#region constants
		/// <summary>
		/// If a statement is not active, this is the # of days back we will evaluate
		/// a statement's laststatementdate to determine if we are going to return it.
		/// </summary>
		protected const int __statementDays=100;
		#endregion constants

		#region member variables

		/// <summary>
		/// Member variable containing _accountNumber object.
		/// </summary>
		protected CustomerAccountNumber _accountNumber=null;
		/// <summary>
		/// Member variable containing siteId
		/// </summary>
		protected int _siteId=0;
		/// <summary>
		/// Member variable containing siteCode
		/// </summary>
		protected string _siteCode=null;
		/// <summary>
		/// Member variable containing username.
		/// </summary>
		protected string _userName=null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
		public AccountAdapter(
			CustomerAccountNumber accountNumber,
			string userName,int siteId,string siteCode)
		{
			// set value
			_accountNumber=accountNumber;
			_userName=userName;
			_siteId=siteId;
			_siteCode=siteCode;
		}
		#endregion ctors

		#region public methods
		/// <summary>
		/// Fills the AccountAddress object.
		/// </summary>
		/// <param name="accountAddress"></param>
		public void Fill(AccountAddress accountAddress)
		{
			try
			{
				// we are going to need this dataAccess assembly to get needed information.
				DalAccount dalAccount=new DalAccount();
					
				// now go get header information
				CustomerAccountSchema.CustomerAddress address=
					dalAccount.GetCustomerAddress(_siteId,_siteCode,
					_accountNumber.AccountNumber9);

				// if we don't get back an address, then its not a valid accountNumber.
				if(address==null) throw new InvalidAccountNumberException();

				accountAddress.AccountNumber13=_accountNumber.AccountNumber13;
				accountAddress.SiteCode=_siteCode;
				accountAddress.SiteNumber=_siteId.ToString();
				accountAddress.AddressLine1 = address.AddressLine1;
				accountAddress.AddressLine2 = address.AddressLine2;
				accountAddress.AddressLine3 = address.AddressLine3;
				accountAddress.AddressLine4 = address.AddressLine4;
				accountAddress.City = address.City;
				accountAddress.State = address.State;
				string zip4=address.Zip4.Trim();
				string zip5=address.Zip5.Trim();
				accountAddress.Zip=zip5;
				if(zip5!=null&&zip5.Length>0&&
					zip4!=null&&zip4.Length>0)
				{
					accountAddress.Zip+="-"+zip4;
				}
				accountAddress.FirstName=address.FirstName;
				accountAddress.MiddleInitial=address.MiddleInitial;
				accountAddress.LastName=address.LastName;
				accountAddress.CustomerName = address.CustomerName;
				accountAddress.HomeTelephoneNumber = 
					(address.IsHomeAreaCodeNull() || address.IsHomeExchangeCodeNull() || address.IsHomeTelephoneNumberNull())?
					string.Empty:FormatPhoneNumber(address.HomeAreaCode,address.HomeExchangeCode,address.HomeTelephoneNumber);
				accountAddress.BusinessTelephoneNumber = (address.IsBusinessAreaCodeNull() || address.IsBusinessExchangeCodeNull() || address.IsBusinessTelephoneNumberNull())?
					string.Empty:FormatPhoneNumber(address.BusinessAreaCode,address.BusinessAreaCode,address.BusinessTelephoneNumber);
				accountAddress.OtherTelephoneNumber = (address.IsOtherAreaCodeNull() || address.IsOtherExchangeCodeNull() || address.IsOtherTelephoneNumberNull())?
					string.Empty:FormatPhoneNumber(address.OtherAreaCode,address.OtherExchangeCode,address.OtherTelephoneNumber);
				accountAddress.AccountNumber13 = _accountNumber.AccountNumber13;
				accountAddress.SiteCode = _siteCode;
				accountAddress.SiteNumber = _siteId.ToString();
				BillType billTypeCode = (BillType)TypeDescriptor.GetConverter(
					typeof(BillType)).ConvertFrom(address.BillType);
				accountAddress.BillTypeCode = billTypeCode;
                accountAddress.OnlineOrderingOptOut = address.ONLINE_ORDERING_OPT_OUT;
				
			}
			catch(BusinessLogicLayerException)
			{
				// already handled
				throw;
			}
			catch(DataSourceException dse)
			{
				// not handled. need to handle
				throw new DataSourceUnavailableException(dse);
			}
			catch(Exception ex)
			{
				// not handled. need to handle
				throw new UnexpectedSystemException(ex);
			}		
		}
		/// <summary>
		/// Fills the AccountAddress object.
		/// </summary>
		/// <param name="account"></param>
		public void Fill(Account account)
		{
			try
			{
				// get address.
				this.Fill((AccountAddress)account);
				DalAccount dalAccount=new DalAccount();
				// now get statement information.
				CustomerAccountSchema.CustomerStatementsDataTable statements=
					dalAccount.GetCustomerStatements(_siteId,_siteCode,_accountNumber.AccountNumber9);
				// get the nsfstatus for the account.
				string nsf=dalAccount.GetNSFCode(_siteId,_siteCode,_accountNumber.AccountNumber9);
				eNSFStatus nsfStatus=(eNSFStatus)TypeDescriptor.GetConverter(
					typeof(eNSFStatus)).ConvertFrom(nsf);
				// now set flags based on NSF.
				account.AcceptChecks=(nsfStatus!=eNSFStatus.BlockDebitAndChecks&&nsfStatus!=eNSFStatus.BlockAll);
				account.AcceptCreditCards=(nsfStatus!=eNSFStatus.BlockCreditCards&&nsfStatus!=eNSFStatus.BlockAll);
													
				//Get Authentication Credentials Status (PIN/SSN)			
				CustomerAuthenticationCredentialsStatus credentialsStatus = 
					dalAccount.GetCustomerAuthenticationCredentialsStatus(_siteId,_siteCode,
					_accountNumber.AccountNumber9);					
				account.PinExists=credentialsStatus.PinExists;
				account.SsnExists=credentialsStatus.SsnExists;
				
				// the customer statement data table returns back 1 record for each statementCode
				// and serviceCode. However, our return sums up these results. this means we need
				// to track when we have a new statement code.
				int previousStatementCode=-1;
				Statement statement=null;
				for(int i=0;i<statements.Count;i++)
				{
					CustomerAccountSchema.CustomerStatement stmnt=statements[i];
					int statementCode=(int)stmnt.StatementCode;
					// these values only change when the statement code changes.
					if(previousStatementCode!=statementCode)
					{
						// create a new statement and add it to the account.
						double amountBilled=(double)stmnt.AmountBilled;
						Icoms1900Date lastStatementDate=new Icoms1900Date((int)stmnt.LastStatementDate);
						eStatementStatus statementStatus=(eStatementStatus)TypeDescriptor.GetConverter(
							typeof(eStatementStatus)).ConvertFrom(stmnt.Status);

						// ok, we care about all statements, except...
						if(statementStatus==eStatementStatus.Cancelled||
							statementStatus==eStatementStatus.Disconnect)
						{
							//we want to show cancelled or disconnected
							//statements that have been billed within the 
							//last 100 days or if the statement still has
							//a balance. Otherwise we get the next statement
							if((!lastStatementDate.SpecialDate
								&&(DateTime.Now-lastStatementDate).Days>__statementDays)
								&&amountBilled<=0)
							{
								continue; //get the next statement
							}
						}
						// set this first off.
						previousStatementCode=statementCode;
						// need this for later.
						string paddedStatementCode=statementCode.ToString().PadLeft(3,'0');
						// create new statement object and add it to collection.
						statement=new Statement();
						account.Statements.Add(statement);
						statement.AccountNumber16=paddedStatementCode+_accountNumber.AccountNumber13;
						statement.StatementCode=paddedStatementCode;
						statement.AmountBilled=amountBilled;
						statement.BillingOption=(eBillingOption)TypeDescriptor.GetConverter(
							typeof(eBillingOption)).ConvertFrom(stmnt.BillHandlingCode);
						// this is the balance outstanding net of any charges not billed.
						Icoms1900Date dueDate=new Icoms1900Date((int)stmnt.DueDate);
						statement.DueOnReceipt=dueDate.SpecialDate;
						statement.DueDate=dueDate.SpecialDate==true?DateTime.MinValue:(DateTime)dueDate;
						int mopCode=(int)stmnt.MopCode;
						statement.EasyPayFlag=mopCode!=0;
						statement.EasyPayMopType=statement.EasyPayFlag ? 
							(ePaymentType)new MopPaymentType(_userName,mopCode) : 
							ePaymentType.Unknown;
						statement.LastStatementDate=new Icoms1900Date((int)stmnt.LastStatementDate);

						// adjustment amounts
						UnappliedAmounts unappliedAmounts=dalAccount.GetUnappliedPaymentAmount(
							_siteId,_siteCode,_accountNumber.AccountNumber9,statementCode);
						statement.UnappliedPaymentAmount=unappliedAmounts.Payments;
						statement.UnappliedAdjustmentAmount=unappliedAmounts.NetAdjustments;
						statement.UnappliedDebitAdjustmentAmount=unappliedAmounts.DebitAdjustments;
						statement.UnappliedCreditAdjustmentAmount=unappliedAmounts.CreditAdjustments;

						// pending amount
						statement.PendingPaymentAmount=dalAccount.GetPendingPaymentAmount(
							_siteId,_siteCode,_accountNumber.AccountNumber9,statementCode);

						// now. let's start setting the fields that are a calculation of 1-N
						// other fields. however, we only want to set that portion of the value
						// that is calculated once per statement (remember GetCustomerStatements()
						// may return multiple records for a single statement we return to a customer)
						// This means if the calculated value of the field depends upon the following
						// items, we want to include it in our calculation here:
						//	a) A PendingPayment
						//	b) UnappliedDebitAdjustment
						//	c) UnappliedCreditAdjustment
						//	d) UnappliedNetAdjustment(net of debit and credit).
						//  e) Any field that includes BALANCE_LAST_STATEMENT
						//	f) Any field that includes LAST_PAYMENT_AMOUNT1,2 or 3

						statement.Status=statementStatus;
						if(stmnt.LastPaymentDate1 > 0)
						{
							statement.RecentPayments.Add(
								new PaymentItem((double)stmnt.LastPaymentAmount1,
								new Icoms1900Date((int)stmnt.LastPaymentDate1)));
						}
						if(stmnt.LastPaymentDate2 > 0)
						{
							statement.RecentPayments.Add(
								new PaymentItem((double)stmnt.LastPaymentAmount2,
								new Icoms1900Date((int)stmnt.LastPaymentDate2)));
						}
						if(stmnt.LastPaymentDate3 > 0)
						{
							statement.RecentPayments.Add(
								new PaymentItem((double)stmnt.LastPaymentAmount3,
								new Icoms1900Date((int)stmnt.LastPaymentDate3)));
						}

						//Get Statement Telephone Numbers
						TelephonySchema.StatementTelephoneNumbersDataTable statementTelephoneNumberData=
							dalAccount.GetStatementTelephoneNumbers(_siteId,_siteCode, statementCode,
							_accountNumber.AccountNumber9);
								
						foreach(TelephonySchema.StatementTelephoneNumbersRow statementTelephoneNumbersRow in statementTelephoneNumberData)
						{	
								string areaCode = statementTelephoneNumbersRow.Area_Code.ToString();
								string exchangeNumber = statementTelephoneNumbersRow.Exchange.ToString();
								string lineNumber = statementTelephoneNumbersRow.Line_Number.ToString();
								
                                // Use Process control for debugging
							    //string process_control = statementTelephoneNumbersRow.Process_Control.ToString();
								
								//Add to Phone Number Collection	 
								statement.StatementPhoneNumbers.Add(new StatementPhoneNumber (FormatPhoneNumber(areaCode,exchangeNumber,lineNumber)));									
														
						}
												
					}
					// HERE WE WANT TO SET THOSE VALUES THAT CHANGE ON A PER RECORD
					// BASIS. REMEMBER THAT GetCustomerStatements() CAN RETURN MULTIPLE
					// RECORDS PER STATEMENT DUE TO AR AGING TRACKING AMOUNTS AT THE
					// PRODUCT LEVEL (E.G. CABLE, DATA, CALLING CARD, TELEPHONE).
						
					// add service code to service code collection.
					statement.ServiceCategories.Add((eServiceCategory)TypeDescriptor.GetConverter(
						typeof(eServiceCategory)).ConvertFrom(stmnt.ServiceCode));
					// modify deposit due.
					statement.DepositDueAmount+=(double)stmnt.DepositDueAmount;
					// next writeoffamount.
					statement.WriteOffAmount+=(double)stmnt.WriteOffAmount;
					// current ar balance.
					statement.CurrentBalance+=(double)stmnt.ArBalanceAmount;
					// next adjust the current bucket.
					statement.CurrentBucket+=(double)stmnt.CurrentArBalanceAmount;
					// are 1-30 bucket.
					statement.AR1To30Amount+=(double)stmnt.Ar1To30Amount;
					// are 31-60 bucket.
					statement.AR31To60Amount+=(double)stmnt.Ar31To60Amount;
					// are 61-90 bucket.
					statement.AR61To90Amount+=(double)stmnt.Ar61To90Amount;
					// are 91-120 bucket.
					statement.AR91To120Amount+=(double)stmnt.Ar91To120Amount;
					// are 121-150 bucket.
					statement.AR121To150Amount+=(double)stmnt.Ar121To150Amount;
					// are 150 plus bucket.
					statement.AR150PlusAmount+=(double)stmnt.Ar150PlusAmount;								
				}
				for(int j=0;j<account.Statements.Count;j++)
				{
					Statement baseStatement=account.Statements[j];
					double totalPending=
						baseStatement.UnappliedPaymentAmount+
						baseStatement.PendingPaymentAmount;

					// set current balance
					baseStatement.CurrentBalance=
						Math.Round(baseStatement.CurrentBalance+
						baseStatement.DepositDueAmount+
						baseStatement.WriteOffAmount+
						baseStatement.UnappliedAdjustmentAmount-
						totalPending,2);

					// now set the different ar aging buckets. NOTE: I know that I am not
					// using all of these fields at this time; however, we are trying to
					// mimic the green screens and our needs may change down the road. So,
					// I am calculating them now so some other poor soul doesn't have to
					// go through it again. Also, I have set XmlIgnoreAttribute() on many
					// of these fields to prevent them from being streamed to our SOA clients.
					// if you want them to be streamed to the client merely change XmlIgnore()
					// to XmlAttribute("NameOfProperty").
					if(totalPending>0)
					{
						double amt=baseStatement.DepositDueAmount;
						bool keepGoing=netOutAmounts(ref amt,ref totalPending);
						baseStatement.DepositDueAmount=Math.Round(amt,2);
						if(keepGoing)
						{
							amt=baseStatement.AR150PlusAmount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR150PlusAmount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							amt=baseStatement.AR121To150Amount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR121To150Amount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							amt=baseStatement.AR91To120Amount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR91To120Amount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							amt=baseStatement.AR61To90Amount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR61To90Amount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							amt=baseStatement.AR31To60Amount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR31To60Amount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							amt=baseStatement.AR1To30Amount;
							keepGoing=netOutAmounts(ref amt,ref totalPending);
							baseStatement.AR1To30Amount=Math.Round(amt,2);
						}
						if(keepGoing)
						{
							baseStatement.CurrentBucket-=totalPending;
						}
					}
					// now modify current bucket by unapplied debit adjustments.
					baseStatement.CurrentBucket=Math.Round(
						baseStatement.CurrentBucket+
						baseStatement.UnappliedDebitAdjustmentAmount,2);
					// now set minimumDue.
					baseStatement.MinimumDue=Math.Round(baseStatement.CurrentBalance-
						baseStatement.CurrentBucket-baseStatement.AR1To30Amount,2);
					// make sure it doesn't go below 0
					if(baseStatement.MinimumDue<0)statement.MinimumDue=0;
				}
			}
			catch(BusinessLogicLayerException)
			{
				// already handled
				throw;
			}
			catch(DataSourceException dse)
			{
				// not handled. need to handle
				throw new DataSourceUnavailableException(dse);
			}
			catch(Exception ex)
			{
				// not handled. need to handle
				throw new UnexpectedSystemException(ex);
			}		
		}
		#endregion public methods

		#region private/protected methods
		/// <summary>
		/// Converts to an int without throwing an exception.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		protected int toInt32(string val)
		{
			int ret=0;
			if(val!=null)
			{
				try{ret=System.Convert.ToInt32(val.Trim());}
				catch{/*None necessary*/}
			}
			return ret;
		}
		/// <summary>
		/// Converts to a double without throwing an exception.
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		protected double toDouble(string val)
		{
			double ret=0;
			if(val!=null)
			{
				try{ret=System.Convert.ToDouble(val.Trim());}
				catch{}
			}
			return ret;
		}
		/// <summary>
		/// This is a helper method that nets out the pending from the amount
		/// field and decrements pending appropriately. if pending is not 0,
		/// it returns true, else false.
		/// </summary>
		/// <param name="amount"></param>
		/// <param name="pending"></param>
		/// <returns></returns>
		private bool netOutAmounts(ref double amount, ref double pending)
		{
			if(amount>=pending)
			{
				amount-=pending;
				pending=0;
				return false;
			}
			else
			{
				pending-=amount;
				amount=0;
				return true;
			}
		}

		/// <summary>
		/// Simple method for formating a phone number.
		/// 
		/// Some might call it a utility, but they would be WRONG!
		/// </summary>
		/// <param name="areaCode"></param>
		/// <param name="exchange"></param>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		private string FormatPhoneNumber(string areaCode, string exchange, string phoneNumber)
		{
			string formattedPhoneNumber = string.Format("({0}) {1}-{2}", areaCode, exchange, phoneNumber.PadLeft(4, '0'));

			//crappy data comes back sometimes...here's a bad way to fix it.
			if (formattedPhoneNumber == "(0) 0-0")
			{
				formattedPhoneNumber = string.Empty;
			}
			return formattedPhoneNumber;
		}
		#endregion private/protected methods
	}
}
