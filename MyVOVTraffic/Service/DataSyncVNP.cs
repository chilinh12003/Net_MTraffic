using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyMTraffic.Service
{
    public class DataSyncVNP
    {

        public class DataSyncVNPObject
        {
            public string ProductOrderKey = string.Empty;
            public string MSISDN = string.Empty;
            public int MSISDNType = 0;
            public string SPID = string.Empty;
            public string ProductID = string.Empty;
            public string ServiceID = string.Empty;
            public string ServiceList = string.Empty;
            public int UpdateType = 0;
            public DateTime UpdateTime = DateTime.Now;
            public string UpdateDesc = string.Empty;
            public DateTime EffectiveTime = DateTime.MinValue;
            public DateTime ExpiryTime = DateTime.MinValue;
            public int PID = 0;

            public bool IsNull
            {
                get
                {
                    if (string.IsNullOrEmpty(this.ProductOrderKey))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }

            public static DataSyncVNPObject Convert(DataTable mTable)
            {
                try
                {
                    List<DataSyncVNPObject> mList = MyConvert.ConvertTable2Object<DataSyncVNPObject>(mTable);
                    if (mList.Count > 0)
                        return mList[0];

                    return new DataSyncVNPObject();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static DataTable Convert(DataSyncVNPObject mObject)
            {
                try
                {
                    List<DataSyncVNPObject> mList = new List<DataSyncVNPObject>();
                    mList.Add(mObject);

                    DataTable mTable = MyConvert.ConvertObject2Table(mList);
                    return mTable;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Các kết quả trả về cho VNP
        /// </summary>
        public enum Result
        {
            //0  Success
            //7001  Field value is invalid(not expected or error)
            //7999  System internal error 
            //7201  The subscription already exists
            //7219  The subscription does not exist
            //7203  The product does not exist
            //7209  The service is not open(Its status is abnormal)
            //#  SDP sẽ xem là không thành công
            [DescriptionAttribute("Success")]
            Success = 0,
            [DescriptionAttribute("Field value is invalid(not expected or error)")]
            ValueInvalid = 7001,
            [DescriptionAttribute("System internal error")]
            SystemError = 7999,
            [DescriptionAttribute("The subscription already exists")]
            SubExist = 7201,
            [DescriptionAttribute("The subscription does not exist")]
            SubNotExist = 7219,
            [DescriptionAttribute("The product does not exist")]
            ProductNotExist = 7203,
            [DescriptionAttribute("The service is not open(Its status is abnormal)")]
            ServiceInvalid = 7209,
            [DescriptionAttribute("Not Success")]
            Fail = 100,
            [DescriptionAttribute("Deregister not success, may be this MSISDN not register before.")]
            DeregisterNotSuccess = 101,
           
        }

        MyExecuteData mExec;
        MyGetData mGet;

        public DataSyncVNP()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public DataSyncVNP(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_DataSyncVNP_Select", mPara, mValue);
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
                return mGet.GetDataTable("Sp_DataSyncVNP_Select", mPara, mValue);
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
        /// <para>Type = 2: Lấy lịch sử đăng ký, hủy của 1 thuê bao (@Para_1 = PID, @Para_2 = MSISDN, Order By LastUpdate DESC</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mPara = { "Type", "Para_1","Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_DataSyncVNP_Select", mPara, mValue);
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
        /// <para>Type = 3: Lay theo ngay thang (Para_1 = PID,Para_2 = RowCount, Para_3 = LastUpdate)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2, string Para_3)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3 };
                return mGet.GetDataTable("Sp_DataSyncVNP_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_DataSyncVNP_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_DataSyncVNP_Delete", mpara, mValue) > 0)
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

        /// <summary>
        /// Sync dữ liệu từ VNP chuyền sang.
        /// <para>Nếu đã tồn tại productOrderKey thì update, còn không thì insert</para>
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool SyncData(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_DataSyncVNP_Sync", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_DataSyncVNP_Update", mpara, mValue) > 0)
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

        public int TotalRow(int? Type, string SearchContent, int PID, int ServiceID)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_DataSyncVNP_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), OrderBy, false.ToString() };
                DataTable mTable=  mGet.GetDataTable("Sp_DataSyncVNP_Search", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                DataColumn mCol_2 = new DataColumn("UpdateTypeName", typeof(string));
                mTable.Columns.Add(mCol_1);
                mTable.Columns.Add(mCol_2);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    if ((int)mRow["UpdateType"] == 1)
                    {
                        mRow["UpdateTypeName"] = "Đăng ký";
                    }
                    else
                    {
                        mRow["UpdateTypeName"] = "Hủy đăng ký";
                    }
                    mTable_Service.DefaultView.RowFilter = "VNPProductID = '" + mRow["ProductID"].ToString() + "'";
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
