using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyConnect.MySQL;
using MyUtility;
using System.Collections;
using System.ComponentModel;
using MyConnect.MySQL;


namespace MyMTraffic.Gateway
{
    public class KPI
    {
         MySQLExecuteData mExec;
        MySQLGetData mGet;
        public KPI()
        {
            mExec = new MySQLExecuteData();
            mGet = new MySQLGetData();
        }

        public KPI(string KeyConnect_InConfig)
        {
            mExec = new MySQLExecuteData(KeyConnect_InConfig);
            mGet = new MySQLGetData(KeyConnect_InConfig);
        }


        /// <summary>
        /// Lấy tổng MT gửi xuống
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public int GetTotalMT(string sTableTime, string FromDate, string ToDate)
        {
            try
            {
                int Total = 0;
                string sSQL = "SELECT COUNT(1) AS CountRow " +
                              "FROM ems_send_log" + sTableTime.Trim() + " " +
                              "WHERE Submit_Date Between '" + FromDate.Trim() + "' AND '" + ToDate.Trim() + "';";
                DataTable tbl = mGet.GetDataTableByQuery(sSQL);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    int.TryParse(tbl.Rows[0]["CountRow"].ToString(), out Total);
                }
                return Total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tổng số MT gửi xuống thành công
        /// </summary>
        /// <param name="sTableTime"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public int GetTotalMTSuccess(string sTableTime, string FromDate, string ToDate)
        {
            try
            {
                int Total = 0;
                string sSQL = "SELECT COUNT(1) AS CountRow " +
                              "FROM ems_send_log" + sTableTime.Trim() + " " +
                              "WHERE process_result=1 AND (Submit_Date Between '" + FromDate.Trim() + "' AND '" + ToDate.Trim() + "');";
                DataTable tbl = mGet.GetDataTableByQuery(sSQL);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    int.TryParse(tbl.Rows[0]["CountRow"].ToString(), out Total);
                }
                return Total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng MO gửi xuống
        /// </summary>
        /// <param name="sTable"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public int GetTotalMO(string sTableTime, string FromDate, string ToDate)
        {
            try
            {
                int Total = 0;
                string sSQL = "SELECT COUNT(1) AS CountRow " +
                              "FROM sms_receive_log" + sTableTime.Trim() + " " +
                              "WHERE Receive_Date Between '" + FromDate.Trim() + "' AND '" + ToDate.Trim() + "' ;";
                DataTable tbl = mGet.GetDataTableByQuery(sSQL);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    int.TryParse(tbl.Rows[0]["CountRow"].ToString(), out Total);
                }
                return Total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tổng số MO gửi xuống thành công
        /// </summary>
        /// <param name="sTableTime"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public int GetTotalMOSuccess(string sTableTime, string FromDate, string ToDate)
        {
            try
            {
                int Total = 0;
                string sSQL = "SELECT COUNT(1) AS CountRow " +
                              "FROM sms_receive_log" + sTableTime.Trim() + " " +
                              "WHERE Command_Code != 'INV' AND (Receive_Date Between '" + FromDate.Trim() + "' AND '" + ToDate.Trim() + "');";
                DataTable tbl = mGet.GetDataTableByQuery(sSQL);
                if (tbl != null && tbl.Rows.Count > 0)
                {
                    int.TryParse(tbl.Rows[0]["CountRow"].ToString(), out Total);
                }
                return Total;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
