using System;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Collections.Generic;

using Cox.DataAccess;
using Cox.DataAccess.Exceptions;

namespace Cox.DataAccess.Account
{
    /// <summary>
    /// Summary description for DalWorkOrder.
    /// </summary>
    public class DalIcomsWorkOrder : DalCustomer
    {
        #region ctors
        /// <summary>
        /// The default constructor. It sets the connectionstring to connect
        /// to the oracle account rdbms (typically this is phastage).
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="siteCode"></param>
        public DalIcomsWorkOrder(int siteId, string siteCode) : base(siteId, siteCode) { }
        /// <summary>
        /// Constructor taking a connectionKey.
        /// </summary>
        /// <param name="connectionKey"></param>
        /// <param name="siteId"></param>
        /// <param name="siteCode"></param>
        public DalIcomsWorkOrder(string connectionKey, int siteId, string siteCode)
            : base(connectionKey, siteId, siteCode) { }
        #endregion ctors

        #region public methods
        /// <summary>
        /// This method retrieves a customers workorders based on the supplied parameters.
        /// </summary>
        /// <param name="accountNumber9"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public IcomsWorkOrderSchema.IcomsWOrdersDataTable GetWorkOrders(string accountNumber9, int fromDate)
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
                    sql.Append("select WOCU#,WONUM,WOSTT,WOEDT,WOETM,WOTYC from ");
                    sql.Append(_siteCode);
                    sql.Append("_WORDMPF where WOCU#=");
                    sql.Append(accountNumber9);
                    sql.Append(" and WONROV=");
                    sql.Append(_siteId);
                    sql.Append(" and WOEDT >= ");
                    sql.Append(fromDate);
                    sql.Append(" ORDER BY WOEDT DESC, WOETM DESC");
                    // build the command object
                    OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn);
                    cmd.CommandType = CommandType.Text;
                    // build the dataadapter
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    // create the dataset to fill
                    IcomsWorkOrderSchema ds = new IcomsWorkOrderSchema();
                    // now fill it
                    da.Fill(ds.IcomsWOrders);
                    // all done, return
                    return ds.IcomsWOrders;
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
        /// Retrieves work orders history records and associated work order detail history with product dim information.
        /// </summary>
        /// <param name="accountNumber9"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public IcomsWorkOrderSchema GetCheckedInWorkOrdersWithProductDIMDetail(string accountNumber9, int fromCheckInDate)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }

                    string sql = string.Format(
                        @" select WORK_ORDER_NUMBER, SITE_ID, ACCOUNT_NUMBER, DATE_ENTERED, TIME_ENTERED, WO_COMMENT_LINE_01, WO_COMMENT_LINE_02, WO_COMMENT_LINE_03, INSTALL_COMPLETION_DATE, WO_STATUS, WO_TYPE, CHECK_IN_DATE, CHECK_IN_STATUS, CANCEL_CODE 
                            from {0}_WORK_ORDER_MASTER_HISTORY a where ACCOUNT_NUMBER = :param1 and SITE_ID = :param2 and CHECK_IN_DATE >= :param3 and CHECK_IN_STATUS = 'C'
                            union all
                            select WORK_ORDER_NUMBER, SITE_ID, ACCOUNT_NUMBER, DATE_ENTERED, TIME_ENTERED, WO_COMMENT_LINE_01, WO_COMMENT_LINE_02, WO_COMMENT_LINE_03, INSTALL_COMPLETION_DATE, WO_STATUS, WO_TYPE, CHECK_IN_DATE, CHECK_IN_STATUS, CANCEL_CODE
                            from {0}_WORK_ORDER_MASTER where ACCOUNT_NUMBER = :param1 and SITE_ID = :param2 and CHECK_IN_DATE >= :param3 and CHECK_IN_STATUS = 'C'
                            order by CHECK_IN_DATE"
                        , _siteCode);

                    OracleCommand cmd = new OracleCommand(sql, oracleConn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("param1", accountNumber9);
                    cmd.Parameters.AddWithValue("param2", _siteId);
                    cmd.Parameters.AddWithValue("param3", fromCheckInDate);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    IcomsWorkOrderSchema ds = new IcomsWorkOrderSchema();
                    da.Fill(ds.WorkOrderMaster);

                    sql = string.Format(
                        @"select a.WORK_ORDER_NUMBER, a.SERVICE_CODE, b.PRODUCT_LINE_CD, a.WORK_ORDER_OCCURRENCE, a.FROM_QUANTITY, a.TO_QUANTITY
                            from {0}_WORK_ORDER_DETAIL_HISTORY a inner join CRM.PRODUCT_DIM b on a.SERVICE_CODE = b.PRODUCT_CD
                            and a.ACCOUNT_NUMBER = :param1
                            union all
                            select a.WORK_ORDER_NUMBER, a.SERVICE_CODE, b.PRODUCT_LINE_CD, a.WORK_ORDER_OCCURRENCE, a.FROM_QUANTITY, a.TO_QUANTITY
                            from {0}_WORK_ORDER_DETAIL a inner join CRM.PRODUCT_DIM b on a.SERVICE_CODE = b.PRODUCT_CD
                            and a.ACCOUNT_NUMBER = :param1 
                            order by SERVICE_CODE , WORK_ORDER_OCCURRENCE"
                        , _siteCode);
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("param1", accountNumber9);
                    da.Fill(ds.WorkOrderDetail);

                    sql = string.Format(
                        @"select a.WORK_ORDER_NUMBER, a.PROMOTION_CODE, a.PROMOTION_GROUP_CODE, a.PROMOTION_TYPE
                            from {0}_WO_CAMPAIGN_PACKAGE a 
                            inner join {0}_WORK_ORDER_MASTER_HISTORY c on a.WORK_ORDER_NUMBER = c.WORK_ORDER_NUMBER and c.ACCOUNT_NUMBER = :param1 and c.SITE_ID = :param2 and c.CHECK_IN_DATE >= :param3 and c.CHECK_IN_STATUS = 'C'
                            union all
                            select a.WORK_ORDER_NUMBER, a.PROMOTION_CODE, a.PROMOTION_GROUP_CODE, a.PROMOTION_TYPE
                            from {0}_WO_CAMPAIGN_PACKAGE a 
                            inner join {0}_WORK_ORDER_MASTER c on a.WORK_ORDER_NUMBER = c.WORK_ORDER_NUMBER and c.ACCOUNT_NUMBER = :param1 and c.SITE_ID = :param2 and c.CHECK_IN_DATE >= :param3 and c.CHECK_IN_STATUS = 'C'     
                        "
                        , _siteCode);
                    cmd.CommandText = sql;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("param1", accountNumber9);
                    cmd.Parameters.AddWithValue("param2", _siteId);
                    cmd.Parameters.AddWithValue("param3", fromCheckInDate);
                    da.Fill(ds.WorkOrderCampaign);

                    return ds;
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }

        /// <summary>
        /// Retrieves work orders records and associated work order detail with product dim information.
        /// </summary>
        /// <param name="accountNumber9"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public IcomsWorkOrderSchema GetPendingWorkOrdersWithProductDIMDetail(string accountNumber9, int fromCreateDate)
        {
            try
            {
                using (OracleConnection oracleConn = new OracleConnection(_connectionString))
                {
                    try { oracleConn.Open(); }
                    catch { throw new LogonException(); }

                    string sql = string.Format(
                        @" select WORK_ORDER_NUMBER, SITE_ID, ACCOUNT_NUMBER, DATE_ENTERED, TIME_ENTERED, WO_COMMENT_LINE_01, WO_COMMENT_LINE_02, WO_COMMENT_LINE_03, INSTALL_COMPLETION_DATE, WO_STATUS, WO_TYPE, CHECK_IN_DATE, CHECK_IN_STATUS, CANCEL_CODE
                            from {0}_WORK_ORDER_MASTER where ACCOUNT_NUMBER = :param1 and SITE_ID = :param2 and DATE_ENTERED >= :param3 and CHECK_IN_STATUS = 'P' ORDER BY DATE_ENTERED"
                        , _siteCode);

                    OracleCommand cmd = new OracleCommand(sql, oracleConn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("param1", accountNumber9);
                    cmd.Parameters.AddWithValue("param2", _siteId);
                    cmd.Parameters.AddWithValue("param3", fromCreateDate);

                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    IcomsWorkOrderSchema ds = new IcomsWorkOrderSchema();
                    da.Fill(ds.WorkOrderMaster);

                    sql = string.Format(
                        @"select a.WORK_ORDER_NUMBER, a.SERVICE_CODE, b.PRODUCT_LINE_CD, a.WORK_ORDER_OCCURRENCE, a.FROM_QUANTITY, a.TO_QUANTITY
                            from {0}_WORK_ORDER_DETAIL a inner join CRM.PRODUCT_DIM b on a.SERVICE_CODE = b.PRODUCT_CD
                            inner join {0}_WORK_ORDER_MASTER c on a.WORK_ORDER_NUMBER = c.WORK_ORDER_NUMBER and c.ACCOUNT_NUMBER = :param1 and c.SITE_ID = :param2 and c.DATE_ENTERED >= :param3 and c.CHECK_IN_STATUS = 'P' ORDER BY a.SERVICE_CODE , a.WORK_ORDER_OCCURRENCE"
                        , _siteCode);
                    cmd.CommandText = sql;
                    da.Fill(ds.WorkOrderDetail);

                    sql = string.Format(
                        @"select a.WORK_ORDER_NUMBER, a.PROMOTION_CODE, a.PROMOTION_GROUP_CODE, a.PROMOTION_TYPE
                            from {0}_WO_CAMPAIGN_PACKAGE a 
                            inner join {0}_WORK_ORDER_MASTER c on a.WORK_ORDER_NUMBER = c.WORK_ORDER_NUMBER and c.ACCOUNT_NUMBER = :param1 and c.SITE_ID = :param2 and c.DATE_ENTERED >= :param3 and c.CHECK_IN_STATUS = 'P' "
                        , _siteCode);
                    cmd.CommandText = sql;
                    da.Fill(ds.WorkOrderCampaign);

                    return ds;
                }
            }
            catch (LogonException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataSourceException(ex);
            }
        }

        /// <summary>
        /// Retrieves the list of timeslots from oracle.
        /// </summary>
        /// <param name="houseNumber"></param>
        /// <returns></returns>
        public IcomsWorkOrderSchema.CFTimeSlotsDataTable GetTimeSlots(string houseNumber)
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
                    sql.Append("select b.SITE_ID,b.POOL,POOL_TIME_SLOT,SUN_START_TIME,SUN_END_TIME,MON_START_TIME,");
                    sql.Append("MON_END_TIME,TUE_START_TIME,TUE_END_TIME,WED_START_TIME,WED_END_TIME,THU_START_TIME,");
                    sql.Append("THU_END_TIME,FRI_START_TIME,FRI_END_TIME,SAT_START_TIME,SAT_END_TIME from ");
                    sql.AppendFormat("{0}_CF_TIME_SLOTS a,{0}_HOUSE_MASTER b where a.SITE_ID={1} and a.SITE_ID=b.SITE_ID ", _siteCode, _siteId);
                    sql.AppendFormat("and a.POOL=b.POOL and b.HOUSE_NUMBER={0} order by b.POOL,POOL_TIME_SLOT", houseNumber);
                    // build the command object
                    OracleCommand cmd = new OracleCommand(sql.ToString(), oracleConn);
                    cmd.CommandType = CommandType.Text;
                    // build the dataadapter
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    // create the dataset to fill
                    IcomsWorkOrderSchema ds = new IcomsWorkOrderSchema();
                    // now fill it
                    da.Fill(ds.CFTimeSlots);
                    // all done, return
                    return ds.CFTimeSlots;
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
