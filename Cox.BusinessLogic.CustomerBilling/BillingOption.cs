using System;
using System.ComponentModel;
using System.Reflection;

using Cox.BusinessLogic;
using Cox.BusinessLogic.ConnectionManager;
using Cox.BusinessLogic.Validation;
using Cox.BusinessLogic.Exceptions;
using Cox.DataAccess;
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
	public class BillingOption:BllConnectionManager
	{
		#region ctors
		/// <summary>
		/// The default constructor.
		/// </summary>
		public BillingOption(string userName):base(userName){}
		#endregion ctors

		#region updateBillingOption
		/// <summary>
		/// This method updates the given account with the correct BillingOption code.
		/// </summary>
		/// <param name="accountNumber16"></param>
		/// <param name="billingOption"></param>
		public void UpdateBillingOption(
			[RequiredItem()][CustomerAccountNumber()]string accountNumber16,
			[RequiredItem()]eBillingOption billingOption)
		{

			BillingLogEntry logEntry = new BillingLogEntry( 
				eBillingActivityType.UpdateBillOption, 
				accountNumber16);
			using(Log log=CreateLog(logEntry))
			{
				try
				{
					// validate the parameters.
					MethodValidator validator=new MethodValidator(MethodBase.GetCurrentMethod(),accountNumber16,billingOption);
					validator.Validate();

					// convert the accountNumber.
					CustomerAccountNumber accountNumber=(CustomerAccountNumber)TypeDescriptor.GetConverter(
						typeof(CustomerAccountNumber)).ConvertFrom(accountNumber16);

					// create the proxy
					CreateProxy(accountNumber);
					logEntry.SiteId = SiteId;

					// invoke it. if an error occurs, one will be thrown.
					Invoke((Request.STOPPB)new StopPbHelper(_siteId.ToString(),
						accountNumber.AccountNumber9,new SpbHelper(
						toInt32(accountNumber.StatementCode),
						(int)billingOption,eSpbFunctionAction.Add)));
				}
				catch( CmErrorException eCm )
				{
					logEntry.SetError( eCm.Message, eCm.ErrorCode );
					throw TranslateCmException( eCm );
				}
				catch( Exception e )
				{
					logEntry.SetError( e.Message );
					throw;
				}
			}
		}
		#endregion updateBillingOption
	}
}
