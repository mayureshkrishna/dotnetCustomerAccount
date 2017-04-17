using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
    /// <summary>
    /// Summary description for DalWorkOrder.
    /// </summary>
    public class DalDetailedWorkOrder : DalAccountBase
    {
        #region ctors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public DalDetailedWorkOrder() : base(__accountConnectionKey) { }
        #endregion ctors

        #region public methods
        /// <summary>
        /// This method retrieves workorder's info based on the supplied parameters.
        /// </summary>
        /// <param name="siteId">Site id</param>
        /// <param name="siteCode">Site code</param>
        /// <param name="workOrderNumber">Work order number</param>
        /// <returns>DetailedWorkOrderSchema.WorkOrderRow</returns>
        public DetailedWorkOrderSchema.WorkOrderRow GetWorkOrderInfo(int siteId, string siteCode, string workOrderNumber)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }
                    // create the sql statement
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select wo.work_order_number, wo.account_number, wo.date_entered, wo.salesman_number, ");
                    sql.Append("wo.wo_comment_line_01,wo.wo_comment_line_02, wo.wo_comment_line_03, wo.sales_reason, wo.q_code, wo.problem_code_01, ");
                    sql.Append("wo.problem_code_02, wo.problem_code_03, wo.problem_code_04, wo.problem_code_05, wo.schedule_date, wo.time_slot, wo.stage_code, ");
                    sql.Append("wo.campaign_code, cm.first_name, cm.last_name, cm.monthly_amount, hm.addr_location, hm.street, hm.addr_city, ");
                    sql.Append("hm.addr_state, hm.addr_zip_5, hm.apartment, hm.dwelling_type, hm.signal_access_code, hm.unserviceable_address, hm.replace_drop_date ");
                    sql.AppendFormat("from {0}_work_order_master wo, {1}_customer_master cm, {2}_house_master hm ", siteCode, siteCode, siteCode);
                    sql.Append("where wo.account_number = cm.account_number and cm.house_number = hm.house_number and wo.work_order_number = ");
                    sql.AppendFormat(workOrderNumber);
                    sql.Append(" and wo.site_id = ");
                    sql.AppendFormat(siteId.ToString());
                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            DetailedWorkOrderSchema ds = new DetailedWorkOrderSchema();
                            // now fill it
                            da.Fill(ds.WorkOrder);
                            // all done, return
                            return ds.WorkOrder.Count == 1 ? ds.WorkOrder[0] : null;
                        }
                    }
                }
            }
            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
        }

        /// <summary>
        /// This method retrieves workorder's detail info based on the supplied parameters.
        /// </summary>
        /// <param name="siteId">Site id</param>
        /// <param name="siteCode">Site code</param>
        /// <param name="workOrderNumber">Work order number</param>
        /// <returns>DetailedWorkOrderSchema.WorkOrderDetailDataTable</returns>
        public DetailedWorkOrderSchema.WorkOrderDetailDataTable GetWorkOrderDetails(int siteId, string siteCode, string workOrderNumber)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }
                    // create the sql statement
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select wd.work_order_number, wd.service_code, sc.short_description, sc.long_description, wd.category, ");
                    sql.Append("wd.old_rate, wd.new_rate, wd.work_order_occurrence, wd.from_quantity, wd.to_quantity, wd.campaign_code, wd.employee_number ");
                    sql.AppendFormat("from {0}_work_order_detail wd, {0}_CF_service_codes sc ", siteCode);
                    sql.Append("where wd.service_code = sc.service_code and wd.site_id = sc.site_id ");
                    sql.AppendFormat("and wd.work_order_number = {0} and wd.site_id = {1}", workOrderNumber, siteId);

                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            DetailedWorkOrderSchema ds = new DetailedWorkOrderSchema();
                            // now fill it
                            da.Fill(ds.WorkOrderDetail);
                            // all done, return
                            return ds.WorkOrderDetail;
                        }
                    }
                }
            }
            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
        }

        /// <summary>
        /// This method retrieves workorder's detail info based on the supplied parameters
        /// from Oracle Phastage ICOMS 7.1  
        /// </summary>
        /// <param name="siteId">Site id</param>
        /// <param name="siteCode">Site code</param>
        /// <param name="workOrderNumber">Work order number</param>
        /// <returns>DetailedWorkOrderSchema.WorkOrderDetailDataTable</returns>
        public DetailedWorkOrderSchema.WorkOrderDetailDataTable GetWorkOrderDetailsFromIcoms7Dot1(int siteId, string siteCode, string workOrderNumber)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    // open connection
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }
                    // create the sql statement
                    StringBuilder sql = new StringBuilder();
                    sql.Append("select distinct wd.work_order_number, wd.service_code, sc.short_description, sc.long_description, wd.category, ");
                    sql.Append("wd.old_rate, wd.new_rate, wd.work_order_occurrence, wd.from_quantity, wd.to_quantity, wddt.iybic1 as campaign_code, wd.employee_number ");
                    sql.AppendFormat("from {0}_work_order_detail wd join {0}_CF_Service_Codes sc on (wd.Service_code = sc.Service_code and wd.Site_id = sc.Site_id) ", siteCode);
                    sql.AppendFormat("left join {0}_cgiycpp wddt ", siteCode);
                    sql.Append("on (wd.site_id = wddt.iynrov and wd.work_order_number = wddt.iywonr and wd.account_number = wddt.iycnbr and wd.service_code = wddt.iysvcd ");
                    sql.Append("and wd.service_category_code = wddt.iycek6 and wddt.iycama = 'Y' and wddt.iybrcn = 'C')");
                    sql.Append("where wd.service_code = sc.service_code and wd.site_id = sc.site_id ");
                    sql.AppendFormat("and wd.work_order_number = {0} and wd.site_id = {1} ", workOrderNumber, siteId);
                    sql.AppendFormat("order by wd.Service_code, wd.Work_order_occurrence");
                    // build the command object
                    using (OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn))
                    {
                        cmd.CommandType = CommandType.Text;
                        // build the dataadapter
                        using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                        {
                            // create the dataset to fill
                            DetailedWorkOrderSchema ds = new DetailedWorkOrderSchema();
                            // now fill it
                            da.Fill(ds.WorkOrderDetail);
                            // all done, return
                            return ds.WorkOrderDetail;
                        }
                    }
                }
            }
            catch (LogonException)
            {
                // just rethrow it. it is from our internal code block
                throw;
            }
            catch (Exception ex)
            {
                // DataSourceException.
                throw new DataSourceException(ex);
            }
        }
        #endregion public methods
    }
}
