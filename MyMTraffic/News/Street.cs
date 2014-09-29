using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyMTraffic.News
{
    public class Street
    {
        MyExecuteData mExec;
        MyGetData mGet;

        public Street()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public Street(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
        }

        /// <summary>
        /// Thời gian cate cho tên dường phố (tính  bằng phút)
        /// </summary>
        public static int StreetCacheTime
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("StreetCacheTime") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("StreetCacheTime"));
                else return 15;
            }
        }
        /// <summary>
        /// Lấy danh sách Đường Hà Nội
        /// </summary>
        public DataTable GetStreetToCache_HN()
        {
            try
            {
                DataTable mTable = new DataTable();
                if (MyUtility.MyCurrent.CurrentPage.Cache["HNStreet"] == null)
                {
                    mTable = Select(2, Position.HaNoi_ID.ToString());
                    MyCurrent.CurrentPage.Cache.Add("HNStreet", mTable, null, DateTime.Now.AddMinutes(StreetCacheTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    mTable = (DataTable)MyUtility.MyCurrent.CurrentPage.Cache["HNStreet"];
                }

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy danh sách Đường TPHCM
        /// </summary>
        public DataTable GetStreetToCache_HCM()
        {
            try
            {
                DataTable mTable = new DataTable();
                if (MyUtility.MyCurrent.CurrentPage.Cache["HCMStreet"] == null)
                {
                    mTable = Select(2, Position.HoChiMinh_ID.ToString());
                    MyCurrent.CurrentPage.Cache.Add("HCMStreet", mTable, null, DateTime.Now.AddMinutes(StreetCacheTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
                else
                {
                    mTable = (DataTable)MyUtility.MyCurrent.CurrentPage.Cache["HCMStreet"];
                }
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_Street_Select", mPara, mValue);
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
        /// Lấy dữ liệu
        /// </summary>
        /// <param name="Type">Cách thức lấy dữ liệu
        /// <para>Type = 1: Lấy thông tin chi tiết 1 Record (Para_1 = StreetID)</para>
        /// <para>Type = 2: Lấy danh sách dường theo PositionID (Para_1 = PositionID)</para>
        /// <para>Type = 4: Lấy tất cả dữ liệu</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Street_Select", mPara, mValue);
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
        /// <para>Type = 6: Lấy chi tiết 1 Record (Para_1 = VNPServiceID, Para_2 = VNPProductID)</para>
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
                return mGet.GetDataTable("Sp_Street_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 7: Lấy chi tiết 1 Record (Para_1 = VNPServiceID, Para_2 = VNPProductID, Para_3 = VNPSPID)</para>
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
                return mGet.GetDataTable("Sp_Street_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_Street_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Street_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Street_Update", mpara, mValue) > 0)
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

        public bool Active(int? Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent", };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Street_Active", mpara, mValue) > 0)
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

        public int TotalRow(int? Type, string SearchContent, int PositionID, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "PositionID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PositionID.ToString(), (IsActive == null ? null : IsActive.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_Street_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int PositionID, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PositionID", "IsActive", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PositionID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_Street_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
