using System;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;
using Cox.DataAccess.Account;
using Cox.DataAccess.CustomerAccount;
using Cox.BusinessLogic;
using Cox.BusinessLogic.Validation;
using Cox.BusinessLogic.Exceptions;
using Cox.Validation;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerAccount;
using Cox.BusinessLogic.ConnectionManager;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Text;

namespace Cox.BusinessLogic.CustomerAccount
{
	static class CustomerAccountInfo
	{
		#region constants
		/// <summary>
		/// Placeholder for constants
		/// </summary>
		#endregion constants

		#region member variables

        ///// <summary>
        ///// Member variable containing _accountNumber object.
        ///// </summary>
        //protected CustomerAccountNumber _accountNumber=null;
        ///// <summary>
        ///// Member variable containing siteId
        ///// </summary>
        //protected int _siteId=0;
        ///// <summary>
        ///// Member variable containing siteCode
        ///// </summary>
        //protected string _siteCode=null;
        ///// <summary>
        ///// Member variable containing username.
        ///// </summary>
        //protected string _userName=null;
        ///// <summary>
        ///// Member variable containing phone number.
        ///// </summary>
        //protected string _phoneNumber = null;
		#endregion member variables

		#region ctors
		/// <summary>
		/// The default constructor
		/// </summary>
        //public CustomerAccountAdapter(string phoneNumber)
        //{
        //    // set value
        //    _phoneNumber = phoneNumber;
        //}
		#endregion ctors

		#region public methods

		#region FillMultiple

		/// <summary>
		/// Fills the AccountAddress object - multiple addresses returned.
		/// </summary>
		/// <param name="customerContactInformation"></param>
        /// <param name="phoneNumber10"></param>
        /// <param name="filter"></param>
        public static void MultipleReturns(List<CustomerContactInformation> customerContactInformation, string phoneNumber10, bool getNeverAndFormerAsWell)
		{
			try
			{
                DalCustomerPhone dalCustomerPhone = new DalCustomerPhone();
                CustomerAccountProfileSchema.AccountMatchesDataTable accountMatches = dalCustomerPhone.GetCustomerAccountMatches(phoneNumber10, getNeverAndFormerAsWell);
                //Check if any account matches were found
                if (accountMatches.Rows.Count > 0)
                {
                    //Get CSV string containing pairs of siteids and account numbers
                    string csvSiteIdAndAccountNumbers = GetCSVSiteIdAndAccountNumbers(accountMatches);

                    DalAccount dalAccount = new DalAccount();
                    //Get customer account information by passing above generated csv string
                    CustomerAccountSchema.CustomerAddressesDataTable addresses;

                    //Since we cannot process varchar more than 4000, we have to divide the string appropraitely
                    if (csvSiteIdAndAccountNumbers.Length > 4000)
                    {
                        //create the list of strings to hold the string of max allowed length
                        List<String> csvStringList=new List<String>();

                        //max allowed length must be less than 4000 and should be divisible by 13
                        //13 = 3 digit siteid + 1 for comma + 9 digit account number.
                        int maxAllowedLength = (int)4000 - (4000 % 13);
                       
                        //keep on looping untill all the all the parts of string are added.
                        while (true)
                        {
                            //add max allowed length part of csv string to the list.
                            //maxAllowedLength-2, because since index start from 0 we have to subtract 1 and to remove extra comma at the end we subtract 1 again.
                            csvStringList.Add(csvSiteIdAndAccountNumbers.Substring(0, maxAllowedLength-2));

                            //remove the part added to the list from the csv string
                            csvSiteIdAndAccountNumbers = csvSiteIdAndAccountNumbers.Substring(maxAllowedLength-1);

                            //when the remaining csv string is less than max allowed length, directly add it and exit.
                            if (csvSiteIdAndAccountNumbers.Length < maxAllowedLength-1)
                            {
                                csvStringList.Add(csvSiteIdAndAccountNumbers);
                                break;
                            }
                                                      
                        }


                        addresses = dalAccount.GetAccountByPhoneNumber(csvStringList, getNeverAndFormerAsWell);
                    }
                    else
                    {
                        addresses = dalAccount.GetAccountByPhoneNumber(csvSiteIdAndAccountNumbers, getNeverAndFormerAsWell);
                    }

                    

                    if (addresses == null)
                    {
                        customerContactInformation = null;
                        return;
                    }

                    // if we don't get back an address, then its not a valid accountNumber.
                    foreach (CustomerAccountSchema.CustomerAddress ca in addresses)
                    {
                        CustomerContactInformation aa = new CustomerContactInformation();

                        PopulateAccountInformation(aa, ca);
                        //Check if the match found was cox secondary number or not
                        if (accountMatches.Select("Site_ID='" + aa.SiteId + "' and Account_Number='" + aa.AccountNumber13.Substring(4) + "' and Customer_TN_Flag=1").Length != 0)
                            aa.CustomerTNNumber = Convert.ToUInt64(phoneNumber10);
                        
                        customerContactInformation.Add(aa);
                    }
                }
                //if not match found return null
                else
                {
                    customerContactInformation = null;
                }

			}
			catch (BusinessLogicLayerException)
			{
				// already handled

				throw;
			}
			catch (DataSourceException dse)
			{
				// not handled. need to handle
				throw new DataSourceUnavailableException(dse);
			}
			catch (Exception ex)
			{
				// not handled. need to handle
				throw new UnexpectedSystemException(ex);
			}
		}

		#endregion FillMultiple

		#region PopulateAccountInformation

		/// <summary>
		/// Fills the AccountAddress object.
		/// </summary>
		/// <param name="account"></param>
		private static void PopulateAccountInformation(CustomerContactInformation accountAddress, CustomerAccountSchema.CustomerAddress address)
		{
			if (address == null) throw new InvalidAccountNumberException();
			accountAddress.AccountNumber13 = address.Account_Number13.PadLeft(12,'0');
            accountAddress.SiteId = int.Parse(address.Site_ID);
            accountAddress.AccountStatus = address.AccountStatus;
            accountAddress.BillTypeCode = address.BillType;
            accountAddress.FranchiseNumber = int.Parse(address.Franchise_Number);

			accountAddress.ServiceAddressLine1 = address.AddressLine1;
			accountAddress.ServiceAddressLine2 = address.AddressLine2;
			accountAddress.ServiceAddressLine3 = address.AddressLine3;
			accountAddress.ServiceAddressLine4 = address.AddressLine4;
			accountAddress.City = address.City;
			accountAddress.State = address.State;
			accountAddress.ZipCode4 = address.Zip4.Trim();
			accountAddress.ZipCode5 = address.Zip5.Trim();
			accountAddress.FirstName = address.FirstName;
			accountAddress.MiddleInitial = address.MiddleInitial;
			accountAddress.LastName = address.LastName;
			accountAddress.CustomerName = address.CustomerName;
			accountAddress.PhoneNumber10 = address.HomeTelephoneNumberFull;
			accountAddress.BusinessPhoneNumber = address.BusinessTelephoneNumberFull;
			accountAddress.OtherPhoneNumber = address.OtherTelephoneNumberFull;
            
		}
		#endregion PopulateAccountInformation

		#endregion public methods

		#region private/protected methods
        /// <summary>
        /// Convert the datatable consisting of siteids and account numbers into csv string.
        /// </summary>
        /// <param name="customerAccounts"></param>
        public static string GetCSVSiteIdAndAccountNumbers(CustomerAccountProfileSchema.AccountMatchesDataTable customerAccounts)
        {
            ArrayList arSiteidAccountNumberList = new ArrayList();
            //Create a string containing siteids and account numbers separeted by comma.
            foreach (CustomerAccountProfileSchema.AccountMatchesRow dr in customerAccounts)
            {
                //Add each row to arraylist
                arSiteidAccountNumberList.Add(dr.Site_ID.ToString().PadLeft(3,'0'));
                arSiteidAccountNumberList.Add(dr.Account_Number);
            }

            //return csv string
            return string.Join(",", (String[])arSiteidAccountNumberList.ToArray(Type.GetType("System.String")));
        }
		#endregion private/protected methods
	}
}
