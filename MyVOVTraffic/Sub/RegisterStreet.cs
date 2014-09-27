using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyMTraffic.Sub
{
    public class RegisterStreet
    {
        MyExecuteData mExec;
        MyGetData mGet;

        public RegisterStreet()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public RegisterStreet(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_RegisterStreet_Select", mPara, mValue);
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
        /// <para>Type = 1: Lấy chi tiết 1 Record (Para_1 = MSISDN, Para_2 = StreetID)</para>
        /// <para>Type = 2: Lấy chi tiết 1 Record (Para_1 = PID, Para_2 = MSISDN, Para_3 = StreetID)</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_RegisterStreet_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Insert dữ liệu
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 0: Insert bình thường</para>
        /// <para>Type = 1: chỉ insert khi dữ liệu không tồn tại trong table</para>
        /// </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RegisterStreet_Insert", mpara, mValue) > 0)
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
        /// Xóa dữ liệu
        /// </summary>
        /// <param name="Type">
        /// <para>Type = 0: Xóa theo MSISDN và StreetID</para>
        /// <para>Type = 1: Xóa theo MSISDN</para>
        /// <para>Type = 2: Xóa theo PID và MSISDN</para>
        /// <para>Type = 3: Xóa theo PID, MSISDN, StreetID</para>
        /// </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_RegisterStreet_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_RegisterStreet_Update", mpara, mValue) > 0)
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
