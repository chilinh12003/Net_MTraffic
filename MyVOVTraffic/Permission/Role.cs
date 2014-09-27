using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
namespace MyMTraffic.Permission
{
    public class Role
    {
        MyExecuteData mExec = new MyExecuteData();
        MyGetData mGet = new MyGetData();

        /// <summary>
        /// Lấy Quyền trên 1 trang
        /// </summary>
        /// <param name="MaNhomThanhVien"></param>
        /// <param name="MaTrang"></param>
        /// <returns></returns>
        public static DataTable Select_Role(int MemberGroupID, string PageCode, string PageName)
        {
            try
            {
                MyGetData mGet = new MyGetData();
                string[] mPara = { "Type", "MemberGroupID", "PageCode", "PageName" };
                string[] mValue = { "0", MemberGroupID.ToString(), PageCode, PageName };
                return mGet.GetDataTable("Sp_Role_SelectRole", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable CreateTable()
        {
            try
            {
                string[] mPara = { "Type" };
                string[] mValue = { "0" };
                return mGet.GetDataTable("Sp_Role_Select", mPara, mValue);
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
                DataSet mSet = mGet.GetDataSet("Sp_Role_Select", mPara, mValue);

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
        /// Lấy dữ liệu table Role
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Lấy table Mẫu</para>
        /// <para>Type = 1: Lấy thông tin chi tiết 1 record (Para_1= RoleID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Role_Select", mPara, mValue);
            }
            catch (Exception ex)
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
        public bool Insert(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Role_Insert", mpara, mValue) > 0)
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
        public bool Update(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Role_Update", mpara, mValue) > 0)
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
        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mPara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Role_Delete", mPara, mValue) > 0)
                {
                    return true;
                }
                else { return false; }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng số dòng
        /// </summary>
        /// <returns></returns>
        public int TotalRow(int? MemberGroupID, string SearchContent, string OrderBy)
        {
            try
            {
                string[] mPara = { "Type", "MemberGroupID", "SearchContent", "OrderBy" };
                string[] mValue = { "0", MemberGroupID.ToString(), SearchContent, OrderBy };

                return (int)mGet.GetExecuteScalar("Sp_Role_Search", mPara, mValue);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu về phân trang
        /// </summary>
        /// <param name="BeginRow"></param>
        /// <param name="EndRow"></param>
        /// <param name="MemberGroupID"></param>
        /// <param name="SearchContent"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public DataTable Search(int? Type,int BeginRow, int EndRow, int? MemberGroupID, string SearchContent, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "MemberGroupID", "SearchContent", "OrderBy" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), MemberGroupID.ToString(), SearchContent, OrderBy };
                return mGet.GetDataTable("Sp_Role_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
