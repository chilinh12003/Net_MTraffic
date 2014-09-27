using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using MyConnect.SQLServer;
using MyUtility;
using System.Web;
using System.ComponentModel;

namespace MyVOVTraffic.Sub
{
    public class Sub_TrafficStreet
    {
        MyExecuteData mExec;
        MyGetData mGet;

        public Sub_TrafficStreet()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public Sub_TrafficStreet(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_Sub_TrafficStreet_Select", mPara, mValue);
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
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Sub_TrafficStreet_Select", mPara, mValue);
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
        /// <para>Type = 2: Lấy chi tiết 1 Record (Para_1 = PID, Para_2 = MSISDN)</para>
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
                return mGet.GetDataTable("Sp_Sub_TrafficStreet_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_Sub_TrafficStreet_Insert", mpara, mValue) > 0)
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
        /// <para>Type = 0 : Xóa theo số điện thoại MSISDN</para>
        /// <para>Type = 1 : Xóa theo số điện thoại và PID (PID,MSISDN) </para>
        /// </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Sub_TrafficStreet_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Sub_TrafficStreet_Update", mpara, mValue) > 0)
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
        /// Đồng bộ dữ liệu với dữ liệu của VNP chuyền sang
        /// </summary>
        /// <param name="IsExist"></param>
        /// <param name="mDataSyncVNPObject"></param>
        /// <param name="mServiceOject"></param>
        /// <returns></returns>
        public bool SyncData(bool IsExist, Service.DataSyncVNP.DataSyncVNPObject mDataSyncVNPObject, Service.Service.ServiceObject mServiceOject, int StreetID)
        {
            try
            {
                int PID = MyConfig.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);
                DataSet mSet = new DataSet("Parent");

                DataTable mTable_Sub = Select(2, PID.ToString(), mDataSyncVNPObject.MSISDN);

                if (mTable_Sub != null && mTable_Sub.Rows.Count > 0)
                    IsExist = true;
                else
                    IsExist = false;

                mSet = CreateDataSet();

                if (IsExist)
                {
                    mSet.Tables[0].ImportRow(mTable_Sub.Rows[0]);
                    mSet.Tables[0].Rows[0]["EffectiveTime"] = mDataSyncVNPObject.EffectiveTime;
                    mSet.Tables[0].Rows[0]["ExpiryTime"] = mDataSyncVNPObject.ExpiryTime;
                }
                else
                {
                    DataRow mRow = mSet.Tables[0].NewRow();
                    mRow["MSISDN"] = mDataSyncVNPObject.MSISDN;
                    mRow["ProductOrderKey"] = mDataSyncVNPObject.ProductOrderKey;
                    mRow["CreateDate"] = DateTime.Now;
                    mRow["EffectiveTime"] = mDataSyncVNPObject.EffectiveTime;
                    mRow["ExpiryTime"] = mDataSyncVNPObject.ExpiryTime;
                    mRow["PID"] = PID;
                    mSet.Tables[0].Rows.Add(mRow);
                }

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                bool Result = false;

                if (IsExist)
                {
                    Result = Update(0, mSet.GetXml());
                }
                else
                {
                    Result = Insert(0, mSet.GetXml());
                }

                //Insert tên đường đã đăng ký vào danh sách đăng ký
                if (Result)
                {
                    InsertToRegisterStreet(mDataSyncVNPObject, mServiceOject, StreetID);
                }

                return Result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private bool InsertToRegisterStreet(Service.DataSyncVNP.DataSyncVNPObject mDataSyncVNPObject, Service.Service.ServiceObject mServiceOject, int StreetID)
        {
            try
            {
                //Insert vào danh sách tên đường đã đăng ký
                RegisterStreet mRegStreet = new RegisterStreet();
                DataSet mSet = mRegStreet.CreateDataSet();
                DataRow mRow = mSet.Tables[0].NewRow();
                mRow["MSISDN"] = mDataSyncVNPObject.MSISDN;
                mRow["StreetID"] = StreetID;
                mRow["CreateDate"] = DateTime.Now;
                mRow["PID"] = MyConfig.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);

                mSet.Tables[0].Rows.Add(mRow);

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                return mRegStreet.Insert(1, mSet.GetXml());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
