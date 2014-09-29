using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyConnect.SQLServer;
using MyUtility;
using System.Data.SqlClient;
using System.ComponentModel;

namespace MyMTraffic.News
{
    public class Article
    {
        public static int Article_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Article_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Article_CateID_1"));
                else return 0;
            }
        }

        public static int Article_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Article_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Article_CateID"));
                else return 0;
            }
        }

        public static int Article_Promo_CateID_1
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Article_Promo_CateID_1") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Article_Promo_CateID_1"));
                else return 0;
            }
        }

        public static int Article_Promo_CateID
        {
            get
            {
                if (MyConfig.GetKeyInConfigFile("Article_Promo_CateID") != string.Empty)
                    return int.Parse(MyConfig.GetKeyInConfigFile("Article_Promo_CateID"));
                else return 0;
            }
        }

        MyExecuteData mExec;
        MyGetData mGet;

        public Article()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public Article(string KeyConnect_InConfig)
        {
            mExec = new MyExecuteData(KeyConnect_InConfig);
            mGet = new MyGetData(KeyConnect_InConfig);
        }

        public int TotalRow(int? Type, string SearchContent, int? CateID, bool? IsActive)
        {
            try
            {
                string[] mpara = { "Type", "SearchContent", "CateID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, CateID.ToString(), (IsActive == null ? null : IsActive.ToString()), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_Article_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int? CateID, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "CateID", "IsActive", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, CateID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_Article_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo 1 Dataset mẫu
        /// </summary>
        /// <returns></returns>
        public DataSet CreateDataSet()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                DataSet mSet = mGet.GetDataSet("Sp_Article_Select", mPara, mValue);
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
        /// Lấy dữ liệu về Article
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy dữ liệu mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (Para_1 = ArticleID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mpara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Article_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu Article
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 2: Lấy danh sách Article theo CateID (Para_1 = CateID, Para_2 = RowCount)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1, string Para_2)
        {
            try
            {
                string[] mpara = { "Type", "Para_1", "Para_2" };
                string[] mValue = { Type.ToString(), Para_1, Para_2 };
                return mGet.GetDataTable("Sp_Article_Select", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert dữ liệu
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Article_Insert", mpara, mValue) > 0)
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
        /// 
        /// </summary>
        /// <param name="Type">Type = 0: </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Update(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Article_Update", mpara, mValue) > 0)
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
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Article_Delete", mpara, mValue) > 0)
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
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="IsActive"></param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Active(int? Type, bool IsActive, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "IsActive", "XMLContent", };
                string[] mValue = { Type.ToString(), IsActive.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Article_Active", mpara, mValue) > 0)
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

    }
}
