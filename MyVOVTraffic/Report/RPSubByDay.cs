using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;
using MyMTraffic.Service;
namespace MyMTraffic.Report
{
    public class RPSubByDay
    {
        MyExecuteData mExec;
        MyGetData mGet;

        public RPSubByDay()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public RPSubByDay(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
        }

        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_RPSubByDay_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 1)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "Child";
                }
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_RPSubByDay_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">Cách thức lấy dữ liệu
        /// <para>Type = 2: Lấy chi tiết 1 Record (Para_1 = ServiceID, Para_2 = ReportDay)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_RPSubByDay_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RPSubByDay_Insert", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RPSubByDay_Delete", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool Update(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RPSubByDay_Update", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool Sync(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RPSubByDay_Sync", mpara, mValue) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int TotalRow(int? Type, string SearchContent,  int ServiceID, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent","ServiceID","BeginDate","EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, ServiceID.ToString(),BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_RPSubByDay_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int ServiceID, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "ServiceID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, ServiceID.ToString(), BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB), OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_RPSubByDay_Search", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                DataColumn mCol_2 = new DataColumn("UpdateTypeName", typeof(string));
                mTable.Columns.Add(mCol_1);
                mTable.Columns.Add(mCol_2);

                MyMTraffic.Service.Service mService = new MyMTraffic.Service.Service();

                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {

                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
