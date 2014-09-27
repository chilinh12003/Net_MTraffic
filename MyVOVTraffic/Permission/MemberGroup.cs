using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
namespace MyMTraffic.Permission
{
    /// <summary>
    /// Nhóm thành viên
    /// </summary>
    public class MemberGroup
    {
        MyExecuteData mExec = new MyExecuteData();
        MyGetData mGet = new MyGetData();


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
                DataSet mSet = mGet.GetDataSet("Sp_MemberGroup_Select", mPara, mValue);


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
        /// Lấy dữ liệu về Nhóm thành viên (MemberGroup)
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 1: Lấy chi tiết 1 record (Para_1 = MemberGroupID)</para>
        /// <para>Type = 2: Lấy tất cả MemberGroup</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int? Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_MemberGroup_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type">Type = 0</param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Update(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_MemberGroup_Update", mpara, mValue) > 0)
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
        /// Insert dữ liệu và trả về 1 table cần lấy sau khi insert
        /// </summary>
        /// <param name="Type">Type = 0 </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };

                if (mExec.ExecProcedure("Sp_MemberGroup_Insert", mpara, mValue) > 0)
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

        public bool Delete(int Type, string XMLContent)
        {
            try
            {
                string[] mPara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_MemberGroup_Delete", mPara, mValue) > 0)
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
    }
}
