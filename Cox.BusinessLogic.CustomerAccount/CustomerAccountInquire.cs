using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Cox.ActivityLog;
using Cox.ActivityLog.CustomerAccount;
using Cox.BusinessLogic;
using Cox.BusinessLogic.Exceptions;
using Cox.BusinessLogic.Validation;
using Cox.BusinessObjects;
using Cox.BusinessObjects.CustomerAccount;
using Cox.BusinessLogic.CustomerBilling;
using Cox.BusinessObjects.CustomerBilling;
using Cox.DataAccess;
using Cox.DataAccess.Account;
using Cox.DataAccess.CustomerAccount;
using Cox.DataAccess.CustomerAccountCCRM;
using Cox.DataAccess.Enterprise;
using Cox.DataAccess.Exceptions;
//using Cox.DataAccess.BillNotification;
using Cox.Wotl.ServiceManagementPlatform;
using Cox.Validation;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Cox.BusinessLogic.CustomerAccount
{
    public class CustomerAccountInquire : BllCustomer
    {
        
        #region constants 

        protected const string _multipleMatchesWithPhoneOnly = "Multiple account matches found for the phone number {0}";
        protected const string _noMatchWithPhoneOnly = "No account match found for phone number {0}";
        protected const string _multipleMatchesWithStreet = "Multiple account matches found for the phone number {0} and street {1}";
        protected const string _noMatchWithStreet = "No account match found for phone number {0} and street {1}";

        #endregion constants 

        #region ctors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CustomerAccountInquire(string userName)
            : base(userName)
        { /* None necessary */ }

        #endregion // ctors

        #region Public Methods

        #region GetCustomerAcount

        /// <summary>GetCustomerAcount</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <returns><CategoryActiveOutage></returns>        
        public CustomerAccountProfile GetCustomerAcount(
                   [RequiredItem()][SiteId()]int siteId,
                   [RequiredItem()][RegEx(@"^(\d{7}(?<=\d*?[1-9]{1}\d*?)(?=\d*?[1-9]{1}\d*?)\d{2})?$", RegexOptions.None)]string accountNumber9)
        {
            //create log and begin logging
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(eCustomerAccountActivityType.GetCustomerAccountByAccountNumberAndSiteId, siteId, accountNumber9);
            using (Log log = CreateLog(logEntry))

                try
                {
                   
                    //perform validation
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(), siteId, accountNumber9);
                    validator.Validate();

                    //start changes for activity logging on 4-Feb-2010
                    logEntry.SiteId = siteId;
                    //get a data access obj to work with  
                    CustomerAccountProfile customerAccountProfile = GetCustomerAccountProfile(siteId, accountNumber9);
                    //End changes for activity logging on 4-Feb-2010
                    // Changes for Self Reg **END**//
                    return customerAccountProfile;
                }
                catch (ValidationException vex)
                {
                    logEntry.SetError(new ExceptionFormatter(vex).Format());
                    throw vex;
                }
                catch (BusinessLogicLayerException bllex)
                {
                    logEntry.SetError(new ExceptionFormatter(bllex).Format());
                    throw bllex;
                }
                catch (DataSourceException dse)
                {
                    logEntry.SetError(new ExceptionFormatter(dse).Format());
                    throw new DataSourceUnavailableException(dse);
                }
                catch (Exception e)
                {
                    logEntry.SetError(new ExceptionFormatter(e).Format());
                    //need to convert to bll exception
                    throw new UnexpectedSystemException(e);
                }
        }

        //Added new method on 4-Feb-2010
        /// <summary>
        /// GetCustomerAccountProfile
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="accountNumber9"></param>
        /// <returns></returns>
        private CustomerAccountProfile GetCustomerAccountProfile(int siteId, string accountNumber9)
        {

            /// set the siteid/sitecode information
            PopulateSiteInfo(siteId);
            DalAccount dalAccount = new DalAccount();
            if (!dalAccount.IsAccountNumberValid(siteId, accountNumber9)) throw new Cox.BusinessLogic.Exceptions.InvalidAccountNumberException();
            CompanyDivisionFranchise cdf = new CompanyDivisionFranchise();
            try
            {
                cdf = dalAccount.GetCompanyDivisionFranchise(_siteId,
                        _siteCode, accountNumber9);
            }
            catch (Exception e)
            {
                throw new DataSourceUnavailableException(e);
            }

            // convert to customerAccountNumber object
            CustomerAccountNumber accountNumber = new CustomerAccountNumber(
                    string.Empty, cdf.Company.ToString(), cdf.Division.ToString(),
                    accountNumber9);
            DalCustomerAccount dalCustomerAccount = new DalCustomerAccount();

            // call dal

            //[28-01-2009] Start Changes to reflect services for an account
            CustomerAccountProfileSchema customerAccountSchema = dalCustomerAccount.GetAccountProfile(siteId, accountNumber9);
            CustomerAccountProfileSchema.CustomerAccountDataTable customerAccountDT = customerAccountSchema.CustomerAccount;
            

            CustomerAccountProfileSchema.CustomerServicesDataTable customerServicesDataTable = customerAccountSchema.CustomerServices;
                   
            CustomerAccountProfile customerAccountProfile = new CustomerAccountProfile();

            if (customerAccountDT != null && customerAccountDT.Rows.Count > 0)
            {
                BuildCustomerAccountBase(customerAccountDT, customerAccountProfile);
                BuildCustomerAccountCampaign(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                BuildCustomerAccountStatement(accountNumber9, accountNumber, customerAccountProfile);
                BuildCustomerAccountContract(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                BuildCustomerAccountPhones(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                BuildCustomerAccountPrivacy(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                BuildCustomerAccountCCRMInfo(siteId, accountNumber9, customerAccountProfile);
                BuildContactEmail(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                
                // [17-05-11] Changes starts here for price lock enhancement
                BuildPriceLockDetails(siteId, accountNumber9, dalCustomerAccount, customerAccountProfile);
                // [17-05-11] Changes ends here for price lock enhancement                
            }

            if (customerServicesDataTable != null && customerServicesDataTable.Rows.Count > 0)
            {
                List<AvailableService> services = new List<AvailableService>();

                foreach ( CustomerAccountProfileSchema.CustomerServicesRow row in customerServicesDataTable.Rows )
                {
                    
                    services.Add(new AvailableService(DalServiceCategory.Instance.GetServiceCategoryDesc(row.Service_Category_Code),(ServiceStatus)TypeDescriptor.GetConverter(typeof(ServiceStatus)).ConvertFrom(row.ServiceStatus)));
                            
                }
                customerAccountProfile.Services = services;
            }

            //[28-01-2009] End Changes to reflect services for an account

            // Changes for Self Reg Ernest Griffin **START**//
            //Account account = new Account();

            //AccountActivity accountActivity = new AccountActivity(_userName);
            //account = accountActivity.InquireAccount(accountNumber9, siteId);

            //if (account.AllowOnlineOrdering)
            //{
            //    customerAccountProfile.OnlineOrderDelinquentBalance = false;
            //}
            //else
            //{
            //    customerAccountProfile.OnlineOrderDelinquentBalance = true;
            //}

            //if (account.OnlineOrderingOptOut != 0)
            //{
            //    customerAccountProfile.OnlineOrderBlock = true;
            //}
            //else
            //{
            //    customerAccountProfile.OnlineOrderBlock = false;
            //}
            return customerAccountProfile;
        }

        /// <summary>BuildContactEmail - populates customerAccountProfile.ContactEmailAddress</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildContactEmail(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            customerAccountProfile.ContactEmailAddress = dalCustomerAccount.GetCustomerEmail(siteId, accountNumber9);
        }

        /// <summary>
        /// BuildPriceLockDetails - populates customerAccountProfile.PriceLockDetails
        /// </summary>
        /// <param name="siteId">Site Id for the customer</param>
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        private void BuildPriceLockDetails(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            CustomerAccountProfileSchema.CustomerPriceLockInfoDataTable customerPriceLockInfoDt = dalCustomerAccount.GetCustomerPriceLockInfo(siteId, accountNumber9);
            List<PriceLockDetail> lsPriceLockDetails = new List<PriceLockDetail>();
            foreach (CustomerAccountProfileSchema.CustomerPriceLockInfoRow dr in customerPriceLockInfoDt)
            {
                PriceLockDetail priceLockDetail = new PriceLockDetail();
                priceLockDetail.ServiceCode = dr.IsSERVICE_CODENull() ? string.Empty : dr.SERVICE_CODE;
                priceLockDetail.ServiceOccurrence = dr.IsSERVICE_OCCURRENCENull() ? "0" : dr.SERVICE_OCCURRENCE;
                priceLockDetail.PriceProtectionTermsAndConditionsId = dr.IsTERMS_AND_CONDITIONS_IDNull() ? string.Empty : dr.TERMS_AND_CONDITIONS_ID;
                priceLockDetail.PriceProtectedRate = dr.IsPRICE_PROTECTED_RATENull() ? 0.0 : Convert.ToDouble(dr.PRICE_PROTECTED_RATE);
                priceLockDetail.ServiceCategory = DalServiceCategory.Instance.GetServiceCategoryDesc(dr.SERVICE_CATEGORY_CODE);
                priceLockDetail.PriceProtectionStatus = dr.IsPRICE_PROTECTION_STATUSNull() ? ePriceProtectionStatus.Unknown : (ePriceProtectionStatus)
                                                                                      TypeDescriptor.GetConverter(typeof(ePriceProtectionStatus)).ConvertFrom(dr.PRICE_PROTECTION_STATUS);

                if (!dr.IsACTIVATION_DATENull())
                {
                    priceLockDetail.ActivationDate = new Icoms1900Date(dr.ACTIVATION_DATE);
                    try
                    {
                        if (!dr.IsACTIVATION_TIMENull())
                        {
                            string activationTime = dr.ACTIVATION_TIME.PadLeft(4, '0');
                            double hours = Convert.ToDouble(activationTime.Substring(0, 2));
                            double minutes = Convert.ToDouble(activationTime.Substring(2));
                            priceLockDetail.ActivationDate = priceLockDetail.ActivationDate.AddHours(hours).AddMinutes(minutes);
                        }                     
                    }
                    catch
                    {
                        //Do notthing
                    }
                    
                }
                else
                {
                    priceLockDetail.ActivationDate = DateTime.MinValue;
                }

                if (!dr.IsPRICE_PROTECT_START_DATENull())
                {
                    priceLockDetail.PriceProtectionStartDate = new Icoms1900Date(dr.PRICE_PROTECT_START_DATE);
                }
                else
                {
                    priceLockDetail.PriceProtectionStartDate = DateTime.MinValue;
                }

                if (!dr.IsPRICE_PROTECTION_END_DATENull())
                {
                    priceLockDetail.PriceProtectionEndDate = new Icoms1900Date(dr.PRICE_PROTECTION_END_DATE);
                }
                else
                {
                    priceLockDetail.PriceProtectionEndDate = DateTime.MinValue;
                }

                priceLockDetail.PriceProtectionDescription = dr.IsPRICE_PROTECT_DESCNull() ? string.Empty : dr.PRICE_PROTECT_DESC;
                priceLockDetail.PriceProtectionId = dr.IsPRICE_PROTECTION_IDNull() ? string.Empty : dr.PRICE_PROTECTION_ID;
                lsPriceLockDetails.Add(priceLockDetail);
            }
            customerAccountProfile.PriceLockDetails = lsPriceLockDetails;
        }

        /// <summary>BuildCustomerAccountCCRMInfo - Populates customerAccountProfile.CustomerValueSegmentation</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountCCRMInfo(int siteId, string accountNumber9, CustomerAccountProfile customerAccountProfile)
        {
            DalCustomerAccountCCRM dalCustomerAccountCCRM = new DalCustomerAccountCCRM();
            customerAccountProfile.CustomerValueSegmentation = dalCustomerAccountCCRM.GetCRMAccountProfile(siteId, accountNumber9);
        }

        /// <summary>BuildCustomerAccountPrivacy - Populates customerAccountProfile.Privacies</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountPrivacy(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            // call dal GetCustomerPrivacyInfo
            CustomerAccountProfileSchema.CustomerPrivacyDataTable customerPrivacyDT = dalCustomerAccount.GetCustomerPrivacy(siteId, accountNumber9);
            if (customerPrivacyDT != null && customerPrivacyDT.Rows.Count > 0)
            {
                customerAccountProfile.Privacies = new List<Privacy>();
                for (int i = 0; i < customerPrivacyDT.Count; ++i)
                {
                    customerAccountProfile.Privacies.Add(new Privacy(customerPrivacyDT[i].New_Privacy_Code, customerPrivacyDT[i].Privacy_Code_Description));
                }
            }
        }

        /// <summary>BuildCustomerAccountPhones - Populates customerAccountProfile.Phones</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountPhones(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            // call dal GetAllAccountPhones
           
            CustomerAccountProfileSchema.CustomerPhoneDataTable customerPhoneDT = dalCustomerAccount.GetAllAccountPhones(siteId, accountNumber9);
            if (customerPhoneDT != null && customerPhoneDT.Rows.Count > 0)
            {                
                customerAccountProfile.Phones = new List<PhoneDetail>();
                for (int i = 0; i < customerPhoneDT.Count; ++i)
                {                  
                    customerAccountProfile.Phones.Add(new PhoneDetail(customerPhoneDT[i].Phone_Number, customerPhoneDT[i].Phone_Type, customerPhoneDT[i].Customer_TN_Sequence, customerPhoneDT[i].Customer_TN_Type_Id, customerPhoneDT[i].Wireless_Flag, customerPhoneDT[i].DNC_Flag));
                }
            }
        }

        /// <summary>BuildCustomerAccountContract - Populates customerAccountProfile.ContractDetails</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountContract(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            // call dal GetCustomerContract
            CustomerAccountProfileSchema.CustomerContractDataTable customerContractDT = dalCustomerAccount.GetCustomerContract(siteId, accountNumber9);
            if (customerContractDT != null && customerContractDT.Rows.Count > 0)
            {
                //Changes for adding contract start date starts here
                DateTime contract_End_Date, contract_Start_Date; 
                customerAccountProfile.ContractDetails = new List<ContractDetail>();
                for (int i = 0; i < customerContractDT.Count; ++i)
                {
                    contract_End_Date = DateTime.MinValue;
                    if (customerContractDT[i].Contract_End_Date != null) 
                    {
                      contract_End_Date = new Icoms1900Date(customerContractDT[i].Contract_End_Date);                      
                    }

                    contract_Start_Date = DateTime.MinValue;

                    if (customerContractDT[i].Contract_START_Date != null)
                    {
                        contract_Start_Date = new Icoms1900Date(customerContractDT[i].Contract_START_Date);
                    }

                    List<string> serviceCategory = new List<string>();

                    if (customerContractDT[i].Service_Category_Code != null)
                    {
                        string[] serviceCategoryCode = customerContractDT[i].Service_Category_Code.Split(',');
                        
                        foreach (string s in serviceCategoryCode)
                        {
                            serviceCategory.Add(DalServiceCategory.Instance.GetServiceCategoryDesc(s));
                        }
                    }

                    customerAccountProfile.ContractDetails.Add(new ContractDetail(customerContractDT[i].Contract_Id.ToString(), 
                            customerContractDT[i].Contract_Desc.ToString(), contract_End_Date, contract_Start_Date, 
                            Convert.ToDouble(customerContractDT[i].Early_Term_Assesment_Amt), 
                             serviceCategory));

                    //Changes for adding contract start date ends here
                }
            }
        }

        /// <summary>
        /// BuildCustomerAccountStatement - Populates customerAccountProfile.Statements
        /// and customerAccountProfile.TotalCurrentBalance
        /// </summary>
        /// <param name="accountNumber9">9 digit account number</param>
        /// <param name="accountNumber">full account number</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountStatement(string accountNumber9, CustomerAccountNumber accountNumber, CustomerAccountProfile customerAccountProfile)
        {
            //Get Statements CurrentAmount, EnrolledInEasyPay, EnrolledInStopPaperBill
            Account account = new Account();

            //get a data access obj to coxprod_commerce 
            //DalBillNotification dalBillNotification = new DalBillNotification();

            //// setup adapter and fill account object.
            AccountAdapter adapter = new AccountAdapter(accountNumber, _userName, _siteId, _siteCode);
            adapter.Fill(account);

            double totalCurrentBalance = 0.00;
            double monthlyRecurringRevenue = 0.0;            

            if (account.Statements != null && account.Statements.Count > 0)
            {
                customerAccountProfile.Statements = new List<Cox.BusinessObjects.CustomerAccount.Statement>();

                for (int j = 0; j < account.Statements.Count; j++)
                {
                    bool enrolledInStopPaperBill = false;
                    //bool enrolledInEmailBillReminder = false;
                    
                    if (account.Statements[j].BillingOption == eBillingOption.StopPaper) enrolledInStopPaperBill = true;
                    if (account.Statements[j].BillingOption == eBillingOption.WebStopPaper) enrolledInStopPaperBill = true;
                    //enrolledInEmailBillReminder = dalBillNotification.GetEnrolledInEmailBillReminder(accountNumber9, _siteId, account.Statements[j].AccountNumber16.Substring(4, 4), account.Statements[j].StatementCode);

                    // Call GetMonthlyServiceAmount
                    DalCustomerAccount dalCustomerAccount = new DalCustomerAccount();
                    CustomerAccountProfileSchema.CustomerMonthlyServiceAmountDataTable customerMonthlyServiceAmountDT = dalCustomerAccount.GetMonthlyServiceAmount(_siteId, accountNumber9);

                    if (customerMonthlyServiceAmountDT != null && customerMonthlyServiceAmountDT.Rows.Count > 0)
                    {
                        customerAccountProfile.TotalMonthlyRecurringRevenue = customerMonthlyServiceAmountDT[0].Total_Monthly_SVC_Amount;

                        for (int i = 0; i < customerMonthlyServiceAmountDT.Count; ++i)
                        {
                            int statementCode = 0;
                            int statementCodeDT = 0;
                            statementCode = int.Parse(account.Statements[j].StatementCode);
                            statementCodeDT = int.Parse(customerMonthlyServiceAmountDT[i].Statement_Code);
                            if (statementCode == statementCodeDT)
                            {
                               monthlyRecurringRevenue = customerMonthlyServiceAmountDT[i].StatementCD_Monthly_SVC_Amount;
                            }
                        }
                    }

                    //[05-02-2009] Start Changes for reflecting AR amounts for Q-Matic

                    //customerAccountProfile.Statements.Add(new Cox.BusinessObjects.CustomerAccount.Statement(account.Statements[j].StatementCode, account.Statements[j].AccountNumber16, monthlyRecurringRevenue, account.Statements[j].CurrentBalance, account.Statements[j].EasyPayFlag, enrolledInStopPaperBill, enrolledInEmailBillReminder));
                    
                    customerAccountProfile.Statements.Add(new Cox.BusinessObjects.CustomerAccount.Statement(account.Statements[j].StatementCode, 
                                                       account.Statements[j].AccountNumber16, monthlyRecurringRevenue, account.Statements[j].CurrentBalance,
                                                       account.Statements[j].EasyPayFlag, enrolledInStopPaperBill, 
                                                       account.Statements[j].AR1To30Amount, account.Statements[j].AR31To60Amount, account.Statements[j].AR61To90Amount, account.Statements[j].AR91To120Amount));

                    //[05-02-2009] Start Changes for reflecting AR amounts for Q-Matic

                    totalCurrentBalance += account.Statements[j].CurrentBalance;
                }
                customerAccountProfile.TotalCurrentBalance = totalCurrentBalance;

            }

            //[23-02-2009] Start Changes for improving performance of CustomerAccount service

            AccountActivity accountActivity = new AccountActivity(_userName,_siteId);
            
            accountActivity.SetAllowOnlineOrderingFlag(ref account);

            if (account.AllowOnlineOrdering)
            {
                customerAccountProfile.OnlineOrderDelinquentBalance = false;
            }
            else
            {
                customerAccountProfile.OnlineOrderDelinquentBalance = true;
            }

            if (account.OnlineOrderingOptOut != 0)
            {
                customerAccountProfile.OnlineOrderBlock = true;
            }
            else
            {
                customerAccountProfile.OnlineOrderBlock = false;
            }

            //[23-02-2009] End Changes for improving performance of CustomerAccount service
        }

        /// <summary>BuildCustomerAccountCampaign - Populates customerAccountProfile.CurrentCampaignDetails</summary>
        /// <param name="siteId">Site Id for the customer</param>        
        /// <param name="accountNumber9">9 digit customer account</param>
        /// <param name="dalCustomerAccount">Customer account data access</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountCampaign(int siteId, string accountNumber9, DalCustomerAccount dalCustomerAccount, CustomerAccountProfile customerAccountProfile)
        {
            // call dal GetcustomerCampaign
            CustomerAccountProfileSchema.CustomerCampaignDataTable customerCampaignDT = dalCustomerAccount.GetCustomerCampaign(siteId, accountNumber9);
            if (customerCampaignDT != null && customerCampaignDT.Rows.Count > 0)
            {
                
                customerAccountProfile.CurrentCampaignDetails = new List<CurrentCampaignDetail>();

                //[23-09-2009] Start Changes for Current Campaign Data fetch request

                foreach (CustomerAccountProfileSchema.CustomerCampaignRow row in customerCampaignDT) 
                {
                    CurrentCampaignDetail campaignDetail = new CurrentCampaignDetail();
                    campaignDetail.CurrentCampaignCode = row.IsPromotion_CodeNull() ? null : row.Promotion_Code.ToString();
                    campaignDetail.CurrentCampaignDescription = row.IsPromotion_DescriptionNull() ? null : row.Promotion_Description.ToString();
                    campaignDetail.ServiceCode = row.IsService_CodeNull() ? null : row.Service_Code.ToString();
                    campaignDetail.ServiceOccurrence = row.IsService_OccurrenceNull() ? null : row.Service_Occurrence.ToString();
                    
                    campaignDetail.ServiceStatus = row.IsService_StatusNull() ? ServiceStatus.Unknown : (ServiceStatus)TypeDescriptor.GetConverter(campaignDetail.ServiceStatus).ConvertFrom(row.Service_Status);
                    campaignDetail.ServiceCategory = DalServiceCategory.Instance.GetServiceCategoryDesc(row.Service_Category_Code);
                    campaignDetail.DiscountActive = row.IsDiscount_ActiveNull() ? DiscountActive.Unknown : (DiscountActive)TypeDescriptor.GetConverter(campaignDetail.DiscountActive).ConvertFrom(row.Discount_Active);

                    if (!row.IsDiscount_Begin_DateNull())
                    {
                        campaignDetail.CampaignStartDate = new Icoms1900Date(row.Discount_Begin_Date);
                    }
                    else
                    {
                        campaignDetail.CampaignStartDate = DateTime.MinValue;
                    }

                    if (!row.IsDiscount_End_DateNull())
                    {
                        campaignDetail.CampaignEndDate = new Icoms1900Date(row.Discount_End_Date);
                    }
                    else
                    {
                        campaignDetail.CampaignEndDate = DateTime.MinValue;
                    }
                    
                    customerAccountProfile.CurrentCampaignDetails.Add(campaignDetail);
                }
                //[23-09-2009] End Changes for Current Campaign Data fetch request

            }
        }

        /// <summary>BuildCustomerAccountBase - Populates many customerAccountProfile fields</summary>
        /// <param name="customerAccountDT">Customer Account Data Table</param>
        /// <param name="customerAccountProfile">Customer account profile object</param>
        /// <returns><void></returns>        
        private void BuildCustomerAccountBase(CustomerAccountProfileSchema.CustomerAccountDataTable customerAccountDT, CustomerAccountProfile customerAccountProfile)
        {
            customerAccountProfile.AccountNumber9 = customerAccountDT[0].Account_Number;
            customerAccountProfile.AccountStatus = (AccountStatus)TypeDescriptor.GetConverter(customerAccountProfile.AccountStatus).ConvertFrom(customerAccountDT[0].Customer_Status_Code);
            customerAccountProfile.CompanyNumber = customerAccountDT[0].Company_Number;
            if (customerAccountDT[0].Disconnect_Date != string.Empty) customerAccountProfile.CustomerDisconnectDate = new Icoms1900Date(customerAccountDT[0].Disconnect_Date);
            if (customerAccountDT[0].Connect_Date != string.Empty) customerAccountProfile.CustomerEstablishedDate = new Icoms1900Date(customerAccountDT[0].Connect_Date);
            customerAccountProfile.DivisionNumber = customerAccountDT[0].Division_Number;           
            customerAccountProfile.FirstName = customerAccountDT[0].First_Name;
            customerAccountProfile.FranchiseNumber = customerAccountDT[0].Franchise_Number;
            customerAccountProfile.HouseNumber = customerAccountDT[0].House_Number;
            customerAccountProfile.LanguagePreference = customerAccountDT[0].Language_Code;
            customerAccountProfile.LastName = customerAccountDT[0].Last_Name;
            customerAccountProfile.MiddleInitial = customerAccountDT[0].Middle_Initial;
            customerAccountProfile.ResidentNumber = customerAccountDT[0].House_Resident_Number;
            customerAccountProfile.ServiceAddressLine1 = customerAccountDT[0].Address_Line_1;
            customerAccountProfile.ServiceAddressLine2 = customerAccountDT[0].Address_Line_2;
            customerAccountProfile.ServiceAddressLine3 = customerAccountDT[0].Address_Line_3;
            customerAccountProfile.ServiceAddressLine4 = customerAccountDT[0].Address_Line_4;
            customerAccountProfile.SiteId = customerAccountDT[0].Site_Id;
            customerAccountProfile.Title = customerAccountDT[0].Title;
            customerAccountProfile.ZipCode4 = customerAccountDT[0].ADDR_ZIP_4;
            customerAccountProfile.ZipCode5 = customerAccountDT[0].ADDR_ZIP_5;
            customerAccountProfile.City = customerAccountDT[0].ADDR_CITY;
            customerAccountProfile.State = customerAccountDT[0].ADDR_STATE;
            customerAccountProfile.Building = customerAccountDT[0].Building;
            customerAccountProfile.Apartment = customerAccountDT[0].Apartment;
            customerAccountProfile.CommercialBusinessName = customerAccountDT[0].Customer_Business_Name;
            customerAccountProfile.DwellingType = (DwellingType)TypeDescriptor.GetConverter(customerAccountProfile.DwellingType).ConvertFrom(customerAccountDT[0].Dwelling_Type);
            customerAccountProfile.BillType = (BillType)TypeDescriptor.GetConverter(customerAccountProfile.BillType).ConvertFrom(customerAccountDT[0].Bill_Type_Code);
            customerAccountProfile.CustomerType = (CustomerType)TypeDescriptor.GetConverter(customerAccountProfile.CustomerType).ConvertFrom(customerAccountDT[0].Customer_Type_Code);
            if (customerAccountDT[0].Date_Of_Birth != string.Empty) customerAccountProfile.DateOfBirth = new Icoms1900Date(customerAccountDT[0].Date_Of_Birth);
            customerAccountProfile.DriversLicenseNumber = customerAccountDT[0].Drivers_License_Number;
            customerAccountProfile.SocialSecurityNumber = customerAccountDT[0].Social_Security_Nbr;
            customerAccountProfile.PinNumber = customerAccountDT[0].Pin_Number;
            customerAccountProfile.CustomerComment = customerAccountDT[0].Customer_Comment;
            customerAccountProfile.DoNotAcceptCheck = customerAccountDT[0].DoNotAcceptCheck;
            customerAccountProfile.DoNotAcceptCreditCard = customerAccountDT[0].DoNotAcceptCreditCards;
            customerAccountProfile.HsiAbuse = customerAccountDT[0].CHSI_Abuse_Flag;
            customerAccountProfile.VIPCode = (VIPCode)TypeDescriptor.GetConverter(customerAccountProfile.VIPCode).ConvertFrom(customerAccountDT[0].VIP_Code);
            customerAccountProfile.CustomerValueSegmentation = customerAccountDT[0].Cust_Value_Segmentation;

            //[30-10-2009] Start Changes for including Customer History Data

            customerAccountProfile.CoxUniqueID = customerAccountDT[0].Cox_Unique_ID;
            customerAccountProfile.ContractEligibleFlag = customerAccountDT[0].Contract;
            customerAccountProfile.TotalTenureInMonths = customerAccountDT[0].Tenure_Number_Of_Months;

            //[30-10-2009] Start Changes for including Customer History Data

            customerAccountProfile.ActiveTru2WayDevices = customerAccountDT[0].NumberOfTru2WayActive;

            //customerAccountProfile.OnlineOrderBlock = customerAccountDT[0].OnlineOrderBlock;
            //if (customerAccountDT[0]. .OnlineOrderBlock != 0)
            //{
            //    customerAccountProfile.OnlineOrderBlock = true;
            //}
            //else
            //{
            //    customerAccountProfile.OnlineOrderBlock = false;
            //}
        }

        #endregion GetCustomerAcount


        #region getAccountAddressesbyPhoneNumber
        /// <summary>
        /// This method returns the account address information for the given Phone Number.
        /// </summary>
        /// <param name="phoneNumber10"></param>
        /// <returns></returns>
        public List<CustomerContactInformation> getAccountAddressesbyPhoneNumber(
            [RequiredItem()][StringLength(10, 10)][RegEx(@"\d{10}", RegexOptions.Compiled)]string phoneNumber10,
            bool getNeverAndFormerAsWell)
        {
            //create log and begin logging
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(eCustomerAccountActivityType.GetCustomerAccountInformation, phoneNumber10);
            using (Log log = CreateLog(logEntry))
            {
                try
                {
                    //Assigned note property of logEntry object to log the phonenumber10 on 4-Feb-2010
                    logEntry.Note = "PhoneNumber10:" + phoneNumber10;
                    // validate the parameters.
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(), phoneNumber10);
                    validator.Validate();
                    try
                    {
                        if (Convert.ToUInt64(phoneNumber10) == 0)
                        {
                            throw new Exception();
                        }
                    }
                    catch
                    {
                        throw new ValidationException("Invalid PhoneNumber");
                    }                    
                 
                    // setup the return
                    List<CustomerContactInformation> contactInformation = new List<CustomerContactInformation>();
                    CustomerAccountInfo.MultipleReturns(contactInformation, phoneNumber10, getNeverAndFormerAsWell);
                    return contactInformation;
                }
                catch (ValidationException ve)
                {
                    logEntry.SetError(new ExceptionFormatter(ve).Format());
                    throw;
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(new ExceptionFormatter(blle).Format());
                    throw;
                }
                catch (Exception e)
                {
                    logEntry.SetError(new ExceptionFormatter(e).Format());
                    throw new UnexpectedSystemException(e);
                }
            }
        }
        #endregion getAccountAddressesbyPhoneNumber

        //[02-02-09] Start Changes for Q-matic

        #region GetCustomerAccountByAccountNumber

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNumber13"></param>
        /// <returns></returns>
        public CustomerAccountProfile GetCustomerAccountByAccountNumber([RequiredItem()][StringLength(13, 13)][CustomerAccountNumberAttribute()]string accountNumber13)
        {
            //create log and begin logging
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry();
            using (Log log = CreateLog(logEntry))
            {
                try
                {
                    logEntry.CustomerAccountActivityType =
                        eCustomerAccountActivityType.GetCustomerAccountByAccountNumber;
                    logEntry.CustomerAccountNumber = accountNumber13;
                    // validate the parameters.
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(), accountNumber13);
                    validator.Validate();
                    // convert the accountNumber.
                    CustomerAccountNumber accountNumber = (CustomerAccountNumber)TypeDescriptor.GetConverter(
                        typeof(CustomerAccountNumber)).ConvertFrom(accountNumber13);

                    // get the siteid/sitecode information
                    PopulateSiteInfo(accountNumber);
                    logEntry.SiteId = this.SiteId;
                    string accountNumber9 = accountNumber13.Substring(4);
                    return GetCustomerAccountProfile(SiteId, accountNumber9);

                }
                catch (ValidationException ve)
                {
                    logEntry.SetError(new ExceptionFormatter(ve).Format());
                    throw;
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(new ExceptionFormatter(blle).Format());
                    throw;
                }
                catch (Exception e)
                {
                    logEntry.SetError(new ExceptionFormatter(e).Format());
                    throw new UnexpectedSystemException(e);
                }
            }
        }


        #endregion GetCustomerAccountByAccountNumber

        #region GetCustomerAccountByPhoneNbrAndStreetNbr
        /// <summary>
        /// This method returns the account address information for the given Phone Number.
        /// </summary>
        /// <param name="phoneNumber10"></param>
        /// <param name="streetNumber"></param>
        /// <returns></returns>
        public CustomerAccountProfile GetCustomerAccountByPhoneNbrAndStreetNbr([RequiredItem()][StringLength(10, 10)][RegEx(@"\d{10}", RegexOptions.Compiled)]string phoneNumber10
                                                                            , string streetNumber)
        {
            //create log and begin logging
            CustomerAccountLogEntry logEntry = new CustomerAccountLogEntry(eCustomerAccountActivityType.GetCustomerAccountByPhoneNbrAndStreetNbr, phoneNumber10, streetNumber);
            using (Log log = CreateLog(logEntry))
            {
                try
                {
                    logEntry.Note = "phoneNumber10:" + phoneNumber10 + " and street#:" + streetNumber;
                    // validate the parameters.
                    MethodValidator validator = new MethodValidator(MethodBase.GetCurrentMethod(), phoneNumber10);
                    validator.Validate();
                    try
                    {
                        if (Convert.ToUInt64(phoneNumber10) == 0)
                        {
                            throw new ValidationException();
                        }
                    }
                    catch
                    {
                        throw new ValidationException("Invalid Phonenumber");
                    }

                    //create dal to get customer accounts for given phone number ad street address
                    DalCustomerPhone dalCustomerPhone = new DalCustomerPhone();
                    CustomerAccountProfileSchema.AccountMatchesDataTable accountMatches = dalCustomerPhone.GetCustomerAccountMatches(phoneNumber10, streetNumber);

                    if (String.IsNullOrEmpty(streetNumber))
                    {
                        if (accountMatches.Rows.Count > 1)
                        {
                            throw new MultipleMatchsFoundException(String.Format(_multipleMatchesWithPhoneOnly, phoneNumber10));
                        }
                        else if (accountMatches.Rows.Count == 0)
                        {
                            throw new NoMatchFoundException(String.Format(_noMatchWithPhoneOnly, phoneNumber10));
                        }
                        else
                        {
                            return GetCustomerAccountProfile(accountMatches[0].Site_ID,
                                                        accountMatches[0].Account_Number);
                        }
                    }
                    else
                    {
                        if (accountMatches.Rows.Count > 1)
                        {
                            throw new MultipleMatchsFoundException(String.Format(_multipleMatchesWithStreet, phoneNumber10, streetNumber));
                        }
                        else if (accountMatches.Rows.Count == 0)
                        {
                            throw new NoMatchFoundException(String.Format(_noMatchWithStreet, phoneNumber10, streetNumber));
                        }
                        else
                        {
                            return GetCustomerAccountProfile(accountMatches[0].Site_ID,
                                                           accountMatches[0].Account_Number);
                        }
                    }


                }
                catch (ValidationException ve)
                {
                    logEntry.SetError(new ExceptionFormatter(ve).Format());
                    throw;
                }
                catch (InvalidAccountNumberException)
                {
                    logEntry.SetError(new ExceptionFormatter(new NoMatchFoundException(String.Format(_noMatchWithPhoneOnly, phoneNumber10))).Format());
                    throw new NoMatchFoundException(String.Format(_noMatchWithPhoneOnly, phoneNumber10));
                }
                catch (BusinessLogicLayerException blle)
                {
                    logEntry.SetError(new ExceptionFormatter(blle).Format());
                    throw;
                }
                catch (Exception e)
                {
                    logEntry.SetError(new ExceptionFormatter(e).Format());
                    throw new UnexpectedSystemException(e);
                }
            }

        }

        #endregion GetCustomerAccountByPhoneNbrAndStreetNbr



        //[02-02-09] End Changes for Q-matic


        

        #endregion Public Methods

        
    }
}
