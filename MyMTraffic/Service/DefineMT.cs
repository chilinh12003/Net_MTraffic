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
    public class DefineMT
    {

        public enum MTType
        {
            Default = 100, 
            Invalid = 101,
            RegHelp = 102,

            /// <summary>
            /// Đăng ký lai thành công và miễn phí
            /// </summary>
            RegAgainSuccessFree = 200,

            /// <summary>
            /// Đăng ký lại thành công không miễn phí =đăng ký lại nhưng hết thời gian khuyến mại
            /// </summary>
            RegAgainSuccessNotFree = 201,

            /// <summary>
            /// Đăng ký không thành công
            /// </summary>
            RegFail = 202,

            /// <summary>
            /// Đăng ký rồi nhưng lại đăng ký tiếp vần còn trong thời gian khuyến mại
            /// </summary>
            RegRepeatFree = 203,

            /// <summary>
            /// Đắng ký lại trong thời gian hết khuyến mại
            /// </summary>
            RegRepeatNotFree = 204,

            /// <summary>
            /// Đăng ký nhưng tải khoản khách hàng không đủ tiền
            /// </summary>
            RegNotEnoughMoney = 205,

            /// <summary>
            /// Đăng ký mới thành công
            /// </summary>
            RegNewSuccess = 206,

            /// <summary>
            /// Hủy thành công dịch vụ
            /// </summary>
            DeRegSuccess = 300,

            /// <summary>
            /// Hủy không thành công do lỗi hệ thống...
            /// </summary>
            DeRegFail = 301,

            /// <summary>
            /// Huy khi mà chưa đăng ký dịch vụ
            /// </summary>
            DeRegNotRegister = 302,

            /// <summary>
            /// Gia hạn thành công
            /// </summary>
            ExtendSuccess = 400,

            /// <summary>
            /// Gia hạn không thành công
            /// </summary>
            ExtendFail = 401,	

        }
     MyExecuteData mExec;
        MyGetData mGet;

        public DefineMT()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public DefineMT(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_DefineMT_Select", mPara, mValue);
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
        /// <para>Type = 1: Lấy chi tiết 1 Record (Para_1 = DefintMTID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_DefineMT_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_DefineMT_Insert", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_DefineMT_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_DefineMT_Update", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_DefineMT_Active", mpara, mValue) > 0)
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

        public int TotalRow(int? Type, string SearchContent,int MTTypeID, bool? IsActive)
        {
            try
            {
                string[] mPara = { "Type", "SearchContent", "MTTypeID", "IsActive", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, MTTypeID.ToString(),(IsActive == null ? null : IsActive.ToString()), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_DefineMT_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent,int MTTypeID, bool? IsActive, string OrderBy)
        {
            try
            {
                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent","MTTypeID", "IsActive",  "OrderBy","IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, MTTypeID.ToString(), (IsActive == null ? null : IsActive.ToString()), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_DefineMT_Search", mpara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
