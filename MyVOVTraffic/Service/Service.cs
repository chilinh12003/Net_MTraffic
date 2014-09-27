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
    public class Service
    {
        public class ServiceObject
        {
            public int ServiceID = 0;
            public string VNPSPID = string.Empty;
            public string VNPServiceID = string.Empty;
            public string VNPProductID = string.Empty;
            public string ServiceName = string.Empty;
            public int ServiceGroupID = 0;
            public string ServiceGroupName = string.Empty;
            public string RegKeyword = string.Empty;
            public string DeregKeyword = string.Empty;
            public string Description = string.Empty;
            public string ImagePath_1 = string.Empty;
            public string ImagePath_2 = string.Empty;
            public double Price = 0;
            public int PartnerID = 0;
            public string PartnerName = string.Empty;
            public bool IsActive = false;
            public int Priority = 0;
            public string TableName = string.Empty;
            public string PushTime = string.Empty;
            public int MTNumber = 0;

            public bool IsNull
            {
                get
                {
                    if (ServiceID < 1)
                        return true;
                    else
                        return false;
                }
            }

            public static ServiceObject Convert(DataTable mTable)
            {
                try
                {
                    List<ServiceObject> mList = MyConvert.ConvertTable2Object<ServiceObject>(mTable);

                    if (mList.Count < 1)
                        return new ServiceObject();
                    else
                        return mList[0];
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public static DataTable Convert(ServiceObject mObject)
            {
                try
                {
                    List<ServiceObject> mList = new List<ServiceObject>();

                    mList.Add(mObject);
                    return MyConvert.ConvertObject2Table(mList);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        public enum ServiceType
        {
            [DescriptionAttribute("Loại dịch vụ")]
            Nothing = 0,
            [DescriptionAttribute("Trả MT ngay khi có tin")]
            PushNow = 1,
            [DescriptionAttribute("Trả MT theo khung giờ")]
            PushByTimer = 2,

            [DescriptionAttribute("Trả MT theo khung giờ và tin HOT")]
            PushByTimerAndHot = 3,

        }

        /// <summary>
        /// Là dịch vụ liên quan đến tên đường
        /// </summary>
        public static int StreetServiceID
        {
            get
            {
                string Value = MyConfig.GetKeyInConfigFile("StreetServiceID");
                Value = Value.Trim();
                if (string.IsNullOrEmpty(Value))
                    return 0;
                else
                    return int.Parse(Value);
            }
    }
         MyExecuteData mExec;
        MyGetData mGet;

        public Service()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public Service(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_Service_Select", mPara, mValue);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 1: Lấy chi tiết 1 Record (Para_1 = MSISDN)</para>
        /// <para>Type = 3: Lấy theo ServiceGroupID (Para_1 = ServiceGroupID)</para>
        /// <para>TYpe = 4: Lất tất cả các Record dã đã được active</para>
        /// <para>TYpe = 6: Lất tất cả các Record</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };

                DataTable mTable = new DataTable();
                if (Type == 4)
                {
                    if (MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session[MySetting.AdminSetting.ParaSave.Service] != null)
                        mTable = ((DataTable)MyCurrent.CurrentPage.Session[MySetting.AdminSetting.ParaSave.Service]).Copy();
                    else
                    {
                        mTable = mGet.GetDataTable("Sp_Service_Select", mPara, mValue);
                        MyCurrent.CurrentPage.Session[MySetting.AdminSetting.ParaSave.Service] = mTable;
                    }
                }
                else
                {
                    mTable = mGet.GetDataTable("Sp_Service_Select", mPara, mValue);
                }
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 2: Lấy chi tiết 1 Record (Para_1 = VNPServiceID, Para_2 = VNPProductID)</para>
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
                return mGet.GetDataTable("Sp_Service_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_Service_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Service_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Service_Update", mpara, mValue) > 0)
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
    
        public bool Active(int Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent" };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Service_Active", mpara, mValue) > 0)
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

        public int TotalRow(int? Type, string SearchContent,int ServiceGroupID, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "ServiceGroupID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, ServiceGroupID.ToString(),(IsActive == null ? null : IsActive.ToString()), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_Service_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent,int ServiceGroupID, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent","ServiceGroupID", "IsActive",  "OrderBy","IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, ServiceGroupID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_Service_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
