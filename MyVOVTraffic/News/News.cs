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
    public class News
    {
        public enum NewsType
        {
            [DescriptionAttribute("Tất cả")]
            Nothing = 0,
            [DescriptionAttribute("Bình thường")]
            Normal = 1,
            [DescriptionAttribute("Tin Hot")]
            HOT = 2,
        }
        public enum Status
        {
            [DescriptionAttribute("Tất cả")]
            Nothing = 0,
            [DescriptionAttribute("Tin mới")]
            New = 1,
            [DescriptionAttribute("Tin gửi")]
            Sending = 2,
            [DescriptionAttribute("Hoàn thành")]
            Complete = 3,
        }

      
        MyExecuteData mExec;
        MyGetData mGet;

        public News()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public News(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
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
        /// <param name="Type">
        /// <para>Type = 6: lấy dữ liệu mẫu cho 2 table News, NewsStreet</para>
        /// </param>
        /// <returns></returns>
        public DataSet CreateDataSet(int Type)
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { Type.ToString() };
                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 2)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "News";
                    mSet.Tables[1].TableName = "NewsStreet";
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
        /// <param name="Type">
        /// <para>Type = 5: lấy dữ liệu News, NewsStreet theo NewsID (Para_1 = NewsID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataSet SelectDataSet(int Type, string Para_1)
        {
            try
            {
               string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                

                DataSet mSet = mGet.GetDataSet("Sp_News_Select", mPara, mValue);
                if (mSet != null && mSet.Tables.Count >= 2)
                {
                    mSet.DataSetName = "Parent";
                    mSet.Tables[0].TableName = "News";
                    mSet.Tables[1].TableName = "NewsStreet";
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
        /// <para>Type = 1: Lấy thông tin chi tiết 1 Record (Para_1 = NewsID)</para>
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
                return mGet.GetDataTable("Sp_News_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_News_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_News_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_News_Update", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_News_Active", mpara, mValue) > 0)
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

        public int TotalRow(int? Type, string SearchContent,int ServiceID, int StreetID, Status StatusID, NewsType NewsTypeID, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent","ServiceID", "StreetID","StatusID","NewsTypeID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent,ServiceID.ToString(), StreetID.ToString(),((int)StatusID).ToString(),((int)NewsTypeID).ToString(), (IsActive == null ? null : IsActive.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_News_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent,int ServiceID, int StreetID, Status StatusID, NewsType NewsTypeID, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent","ServiceID", "StreetID", "StatusID", "NewsTypeID", "IsActive", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent,ServiceID.ToString(), StreetID.ToString(), ((int)StatusID).ToString(), ((int)NewsTypeID).ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_News_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
