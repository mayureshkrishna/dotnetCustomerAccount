using System;
using System.Text;
using System.ComponentModel;
using System.Reflection;

using Cox.BusinessLogic;
using Cox.BusinessLogic.ConnectionManager;
using Cox.BusinessLogic.Validation;
using Cox.BusinessLogic.Exceptions;
using Cox.DataAccess;
using Cox.DataAccess.Account;
using Cox.DataAccess.CustomerBilling;
using Cox.DataAccess.Enterprise;
using Cox.DataAccess.Exceptions;
using Cox.ServiceAgent.ConnectionManager;
using Cox.Validation;
using Request=Cox.ServiceAgent.ConnectionManager.Request;
using Response=Cox.ServiceAgent.ConnectionManager.Response;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerBilling;
using Cox.ActivityLog;
using Cox.ActivityLog.CustomerBilling;
using Microsoft.ApplicationBlocks.ExceptionManagement;
namespace Cox.BusinessLogic.CustomerBilling
{
	/// <summary>
	/// This BusinessLogic class handles retrieving Account and AccountActivity information.
	/// </summary>
	public class AccountActivity : BllConnectionManager
	{
		#region constants
		/// <summary>
		/// Message for InvalidAccountNumberException when no statement code is found for
		/// account number and phone number
		/// </summary>
		protected const string __noStatementCodeForAccountExceptionMessage = 
			"No statement code was found for account '{0}' and phone number '{1}'";
		#region jms
		/// <summary>
		/// Constant value containing the underlying error 
		/// message to use when we are unable to convert the 
		/// SetTopBoxId to a valid AccountNumber.
		/// </summary>
		protected const string __setTopBoxIdToAccountNumberException = 
			"Could not convert setTopBoxId '{0}' to a valid account. A matching record did not exist in the database or invalid data was returned.";
		#endregion jms
		#endregion

		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public AccountActivity(string userName):base(userName){}

        //[23-02-2009] Start Changes for improving performance of CustomerAccount service

        public AccountActivity(string userName, int siteId):base(userName)
        {
            _siteId = siteId;
        }

        //[23-02-2009] End Changes for improving performance of CustomerAccount service

		#endregion ctors

		#region inquireAccount
		/// <summary>
		/// This method returns account and statement information for the given account.
		/// </summary>
		/// <param name="accountNumber13"></param>
		/// <returns></returns>
		public Account InquireAccount([RequiredItem()][StringLength(13,13)][CustomerAccountNumberAttribute()]string accountNumber13)
		{
			BillingLogEntry logEntry = new BillingLogEntry(eBillingActivityType.AccountInquiry, accountNumber13!=null?accountNumber13.PadLeft(16,'0'):string.Empty);
			using(Log log = CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber13);
					validator.Validate();
					// convert the accountNumber.
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

					// get the siteid/sitecode information
					PopulateSiteInfo(accountNumber);
					logEntry.SiteId=this.SiteId;

					// setup return
					Account account=new Account();

					// setup adapter and fill account object.
					AccountAdapter adapter=new AccountAdapter(accountNumber,_userName,_siteId,_siteCode);
					adapter.Fill(account);

					//set the AllowOnlineOrdering flag
					SetAllowOnlineOrderingFlag(ref account);

					// all done.
					return account;
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(new ExceptionFormatter(ve).Format());
					throw;
				}
				catch(BusinessLogicLayerException blle)
				{
					logEntry.SetError(new ExceptionFormatter(blle).Format());
					throw;
				}
				catch(Exception e)
				{
					logEntry.SetError( new ExceptionFormatter(e).Format() );
					throw new UnexpectedSystemException(e);
				}
			}
		}
		/// <summary>
		/// This method returns account and statement information for the given account.
		/// </summary>
		/// <param name="accountNumber9"></param>
		/// <param name="siteId"></param>
		/// <returns></returns>
		public Account InquireAccount([RequiredItem()][StringLength(9,9)]string accountNumber9, [RequiredItem()]int siteId)
		{
			BillingLogEntry logEntry = new BillingLogEntry(eBillingActivityType.AccountInquiry, accountNumber9==null?string.Empty:accountNumber9.PadLeft(16,'0'));
			using(Log log = CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber9, siteId);
					validator.Validate();
				
					//log the site id
					logEntry.SiteId = siteId;

					//look up the company and division for this account
					string siteCode = DalSiteCode.Instance.GetSiteCode(siteId);
					DalAccount dalAccount = new DalAccount();
					CompanyDivisionFranchise companyDivisionFranchise = dalAccount.GetCompanyDivisionFranchise(siteId, siteCode, accountNumber9);
									
					//turn accountNumber9 into accountNumber13
					string accountNumber13 = companyDivisionFranchise.Company.ToString().PadLeft(2,'0') + companyDivisionFranchise.Division.ToString().PadLeft(2,'0') + accountNumber9;
					
					// convert to CustomerAccountNumber
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

					PopulateSiteInfo(accountNumber);

					//log the account number
					logEntry.CustomerAccountNumber = accountNumber.AccountNumber16;

					// setup return
					Account account=new Account();

					// setup adapter and fill account object.
					AccountAdapter adapter=new AccountAdapter(accountNumber,_userName,_siteId,_siteCode);
					adapter.Fill(account);

					//set the AllowOnlineOrdering flag
					SetAllowOnlineOrderingFlag(ref account);

					// all done.
					return account;
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(ve.Message);
					throw;
				}
				catch(InvalidAccountNumberException ie)
				{
					logEntry.SetError(ie.Message);
					throw;
				}
				catch(DataSourceException de)
				{
					logEntry.SetError(de.Message);
					throw new DataSourceUnavailableException(de);
				}
				catch(UnexpectedSystemException ue)
				{
					logEntry.SetError(ue.Message);
					throw;
				}
				catch(Exception e)
				{
					logEntry.SetError( e.Message );
					throw new UnexpectedSystemException(e);
				}
			}
		}

		/// <summary>
		/// This method returns account and statement information for the given account.
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="setTopBoxId"></param>
		/// <returns></returns>
		public Account InquireAccount([RequiredItem()]int siteId,[RequiredItem()][StringLength(1,16)]string setTopBoxId)
		{
			BillingLogEntry logEntry = new BillingLogEntry(eBillingActivityType.AccountInquiry, siteId, setTopBoxId);
			using(Log log = CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),siteId, setTopBoxId);
					validator.Validate();

					//look up the sitecode 
					string siteCode = DalSiteCode.Instance.GetSiteCode(siteId);

					//get account number
					string accountNumber9 = null;
					DalEquipment dalEquipment = new DalEquipment();
					accountNumber9 = dalEquipment.GetAccountFromSetTopBoxId(siteId, siteCode, 
						setTopBoxId).PadLeft(9, '0');

					if(accountNumber9==null || accountNumber9==string.Empty)
					{
						throw new InvalidSetTopBoxIdException(string.Format( 
							__setTopBoxIdToAccountNumberException,setTopBoxId) );
					}

					//look up division for this account
					DalAccount dalAccount = new DalAccount();
					CompanyDivisionFranchise companyDivisionFranchise = dalAccount.GetCompanyDivisionFranchise(siteId, 
						siteCode, accountNumber9);
									
					//turn accountNumber9 into accountNumber13
					string accountNumber13 = companyDivisionFranchise.Company.ToString().PadLeft(2,'0') + companyDivisionFranchise.Division.ToString().PadLeft(2,'0') + accountNumber9;
					
					// convert to CustomerAccountNumber
					CustomerAccountNumber accountNumber = (CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

					// get the siteid/sitecode information
					PopulateSiteInfo(accountNumber);
					logEntry.CustomerAccountNumber = accountNumber.AccountNumber16;

					// setup return
					Account account=new Account();

					// setup adapter and fill account object.
					AccountAdapter adapter = new AccountAdapter(accountNumber, _userName, _siteId, _siteCode);
					adapter.Fill(account);

					//set the AllowOnlineOrdering flag
					SetAllowOnlineOrderingFlag(ref account);

					// all done.
					return account;
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(ve.Message);
					throw;
				}
				catch(BusinessLogicLayerException blle)
				{
					logEntry.SetError(new ExceptionFormatter(blle).Format());
					throw;
				}
				catch(DataSourceException de)
				{
					logEntry.SetError(de.Message);
					throw new DataSourceUnavailableException(de);
				}
				catch(Exception e)
				{
					logEntry.SetError( e.Message );
					throw new UnexpectedSystemException(e);
				}
			}
		}
		#endregion inquireAccount

		#region inquireStatementActivity
		/// <summary>
		/// This method returns the StatementActivity for the given account
		/// since the last statement.
		/// </summary>
		/// <param name="accountNumber16"></param>
		/// <returns></returns>
		public StatementActivity InquireStatementActivity(
			[RequiredItem()][StringLength(16,16)]
			[CustomerAccountNumber()]string accountNumber16)
		{
			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.StatementActivity, accountNumber16 );
			using( Log log = CreateLog( logEntry ) )
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber16);
					validator.Validate();
					// convert the accountNumber.
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber16);

					// setup the return
					StatementActivity statementActivity = new StatementActivity();
					statementActivity.AccountNumber16 = accountNumber.AccountNumber16;
					statementActivity.StatementCode = accountNumber.StatementCode;

					// create proxy
					CreateProxy( accountNumber );

					// get the sitecode/siteId information
					// NOTE: CreateProxy above populates enough data for this
					// purpose as well
					logEntry.SiteId = SiteId;

					// get ACSUM from proxy
					Response.MAC00010 mac10Output = 
						(Response.MAC00010) this.Invoke(
						(Request.MAC00010)
						new Mac00010Helper(
						SiteId.ToString(),
						accountNumber.AccountNumber9,
						accountNumber.StatementCode ) );

					if( mac10Output.Items != null )
					{
						foreach( object item in mac10Output.Items )
						{
							if( item is Response.INL00008 )
							{
								Response.INL00008 inline8 = (Response.INL00008)item;
								Transaction transaction = new Transaction();
								transaction.Amount=toDouble(inline8.TRNSCTNAMNT);
								transaction.Description = inline8.DETLDSCRPTN;
								transaction.FromDate=new IcomsDate(inline8.TRNSCTNFROMDATE8).Date;
								transaction.ToDate=new IcomsDate(inline8.TRNSCTNTODATE8).Date;
								transaction.TransactionType=(eTransactionType)TypeDescriptor.GetConverter(typeof(
									eTransactionType)).ConvertFrom(inline8.TRNSCTNTYPE);;
								transaction.ServiceCategory=(eServiceCategory)TypeDescriptor.GetConverter(typeof(
									eServiceCategory)).ConvertFrom(inline8.SRVCCTGRY);;
								statementActivity.Transactions.Add( transaction );
							}
						}
					}
					// all done.
					return statementActivity;
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(new ExceptionFormatter(ve).Format());
					throw;
				}
				catch(BusinessLogicLayerException blle)
				{
					logEntry.SetError(new ExceptionFormatter(blle).Format());
					throw;
				}
				catch( CmErrorException eCm )
				{
					logEntry.SetError( eCm.Message, eCm.ErrorCode );
					throw TranslateCmException( eCm );
				}
				catch(Exception e)
				{
					logEntry.SetError( new ExceptionFormatter(e).Format() );
					throw new UnexpectedSystemException(e);
				}
			}
		}
		#endregion inquireStatementActivity

		#region inquireAccountAddress
		/// <summary>
		/// This method returns the account address information for the given account.
		/// </summary>
		/// <param name="accountNumber13"></param>
		/// <returns></returns>
		public AccountAddress InquireServiceAddress(
			[RequiredItem()][StringLength(13,13)]
			[CustomerAccountNumber()]string accountNumber13)
		{
			BillingLogEntry logEntry = new BillingLogEntry(
				eBillingActivityType.AccountAddress,
				accountNumber13.PadLeft(16,'0'));
			using(Log log=CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber13);
					validator.Validate();

					// convert the accountNumber.
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

					// setup site information.
					PopulateSiteInfo(accountNumber);
					logEntry.SiteId = SiteId;

					// setup the return
					AccountAddress accountAddress = new AccountAddress();
					AccountAdapter adapter=new AccountAdapter(accountNumber,_userName,_siteId,_siteCode);
					adapter.Fill(accountAddress);
					return accountAddress;
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(new ExceptionFormatter(ve).Format());
					throw;
				}
				catch(BusinessLogicLayerException blle)
				{
					logEntry.SetError(new ExceptionFormatter(blle).Format());
					throw;
				}
				catch(Exception e)
				{
					logEntry.SetError( new ExceptionFormatter(e).Format() );
					throw new UnexpectedSystemException(e);
				}
			}
		}
		#endregion inquireStatementActivity

		#region inquireStatementCode
		/// <summary>
		/// This method returns the telephone numbers for digital telephone service for a statement.
		/// </summary>
		/// <param name="accountNumber13"></param>
		/// <param name="phoneNumber"></param>
		/// <returns></returns>
		public string InquireStatementCode(
			[RequiredItem()][StringLength(13,13)][CustomerAccountNumber()]string accountNumber13,
			[RequiredItem()][StringLength(4,4)]string phoneNumber)
		{
			BillingLogEntry logEntry = new BillingLogEntry(eBillingActivityType.StatementCodeInquiry, accountNumber13, phoneNumber);
			using(Log log = CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber13,phoneNumber);
					validator.Validate();
					
					// convert the accountNumber.
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

					// get the siteid/sitecode information
					PopulateSiteInfo(accountNumber);
					logEntry.SiteId=this.SiteId;

					// use dal to get statement code
					string statementCode = null;
					DalAccount dalAccount = new DalAccount();
					statementCode = dalAccount.GetStatementCode(SiteId, SiteCode, accountNumber.AccountNumber9, phoneNumber);

					// if no statement code is found throw ex
					if (statementCode == null || statementCode == string.Empty)
					{
						throw new InvalidAccountNumberException(
							string.Format(__noStatementCodeForAccountExceptionMessage,accountNumber.AccountNumber13,phoneNumber));
					}
					return statementCode.PadLeft(3,'0');
				}
				catch(ValidationException ve)
				{
					logEntry.SetError(new ExceptionFormatter(ve).Format());
					throw;
				}
				catch(BusinessLogicLayerException blle)
				{
					logEntry.SetError(new ExceptionFormatter(blle).Format());
					throw;
				}
				catch(Exception e)
				{
					logEntry.SetError( new ExceptionFormatter(e).Format() );
					throw new UnexpectedSystemException(e);
				}
			}
		}
		#endregion

        /// <summary>
        /// This method discovers which SqlStatementBas to use and returns its
        /// underlying index. 999999 out of 1000000 it will be 0.
        /// </summary>
        /// <param name="icomsStatement"></param>
        /// <returns></returns>
        protected int getIcomsStatementIndex(Response.SQLSTMNT icomsStatement)
        {
            // if there is only 1, then return the 0 offset.
            //if (icomsStatement.SQLSTMNTBAS.Length == 1) return 0;

            //int i = 0, processControlId = 0, basProcessControlId = 0;
            //int length = icomsStatement.SQLSTMNTBAS.Length;
            //for (; i < length; i++)
            //{
            //    Response.SQLSTMNTBAS bas = icomsStatement.SQLSTMNTBAS[i];
            //    try { basProcessControlId = Convert.ToInt32(bas.PRCSSCNTRL); }
            //    catch { basProcessControlId = 0; }
            //    if (basProcessControlId > processControlId)
            //    {
            //        processControlId = basProcessControlId;
            //    }
            //}
            //return i;
            // Return below added due to the changes in this version of ICOMS
            return 0;
        }

		/// <summary>
		/// Checks if we need to set the allow online ordering flag to false
		/// </summary>
		/// <param name="account"></param>
		public void SetAllowOnlineOrderingFlag(ref Account account)
		{
			double AR1To30PastDueAmount = 0.00;
			double AR31To60PastDueAmount = 0.00;
			double AR61To90PastDueAmount = 0.00;
			double AR91To120PastDueAmount = 0.00;
			double AR121To150PastDueAmount = 0.00;
			double AR151PlusPastDueAmount = 0.00;
			double accountBalance = 0.00;
			for(int i=0;i<account.Statements.Count;i++)
			{
				//add up the past due amounts for all statements
				AR1To30PastDueAmount += account.Statements[i].AR1To30Amount;
				AR31To60PastDueAmount += account.Statements[i].AR31To60Amount;
				AR61To90PastDueAmount += account.Statements[i].AR61To90Amount;
				AR91To120PastDueAmount += account.Statements[i].AR91To120Amount;
				AR121To150PastDueAmount += account.Statements[i].AR121To150Amount;
				AR151PlusPastDueAmount += account.Statements[i].AR150PlusAmount;

				//add up the account balance for all statments
				accountBalance += account.Statements[i].CurrentBalance;
			}

			//check if we need to set the allow online ordering flag
			//set to true by default
			if(AR1To30PastDueAmount > 0.00 || AR31To60PastDueAmount > 0.00 || AR61To90PastDueAmount > 0.00 || 
				AR91To120PastDueAmount > 0.00 || AR121To150PastDueAmount > 0.00 || AR151PlusPastDueAmount > 0.00 ||
				accountBalance > 0.00)
			{
				//roll up the past due amounts 
				AR1To30PastDueAmount += AR31To60PastDueAmount + AR61To90PastDueAmount + AR91To120PastDueAmount + AR121To150PastDueAmount + AR151PlusPastDueAmount;
				AR31To60PastDueAmount += AR61To90PastDueAmount + AR91To120PastDueAmount + AR121To150PastDueAmount + AR151PlusPastDueAmount;
				AR61To90PastDueAmount += AR91To120PastDueAmount + AR121To150PastDueAmount + AR151PlusPastDueAmount;
				AR91To120PastDueAmount += AR121To150PastDueAmount + AR151PlusPastDueAmount;
				AR121To150PastDueAmount += AR151PlusPastDueAmount;
											
				//set the allow online ordering flag
				DalOnlineOrdering dalOnlineOrdering = new DalOnlineOrdering();
				OnlineOrderingSchema.OnlineOrderingRow onlineOrderingRow = dalOnlineOrdering.GetOnlineOrderingInfo(_siteId);

				//check each amount and see if it is over the max
				if(onlineOrderingRow != null)
				{
					if(!onlineOrderingRow.IsMaxCurrentBalanceNull() && accountBalance > (double)onlineOrderingRow.MaxCurrentBalance)
					{
						account.AllowOnlineOrdering = false;
					}
					if(!onlineOrderingRow.IsMaxAR1To30BalanceNull() && AR1To30PastDueAmount > (double)onlineOrderingRow.MaxAR1To30Balance)
					{
						account.AllowOnlineOrdering = false;
					}
					else if(!onlineOrderingRow.IsMaxAR31To60BalanceNull() && AR31To60PastDueAmount > (double)onlineOrderingRow.MaxAR31To60Balance)
					{
						account.AllowOnlineOrdering = false;
					}
					else if(!onlineOrderingRow.IsMaxAR61To90BalanceNull() && AR61To90PastDueAmount > (double)onlineOrderingRow.MaxAR61To90Balance)
					{
						account.AllowOnlineOrdering = false;
					}
					else if(!onlineOrderingRow.IsMaxAR91To120BalanceNull() && AR91To120PastDueAmount > (double)onlineOrderingRow.MaxAR91To120Balance)
					{
						account.AllowOnlineOrdering = false;
					}
					else if(!onlineOrderingRow.IsMaxAR121to150BalanceNull() && AR121To150PastDueAmount > (double)onlineOrderingRow.MaxAR121to150Balance)
					{
						account.AllowOnlineOrdering = false;
					}
					else if(!onlineOrderingRow.IsMaxAR150PlusBalanceNull() && AR151PlusPastDueAmount > (double)onlineOrderingRow.MaxAR150PlusBalance)
					{
						account.AllowOnlineOrdering = false;
					}
					//already set to true by default
				}
			}
		}

	}
}
