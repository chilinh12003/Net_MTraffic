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
    public class Subscriber
    {

        public enum Status
        {
            [DescriptionAttribute("Hoạt động")]
            Active = 1,
            [DescriptionAttribute("Không hoạt động")]
            Deactive = 2,
            [DescriptionAttribute("Retry Charge")]
            ChargeFail = 3,
            [DescriptionAttribute("Hoạt động")]
            ConfirmDeregister = 4,
        }

        MyExecuteData mExec;
        MyGetData mGet;

        public Subscriber()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();

        }

        public Subscriber(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_Subscriber_Select", mPara, mValue);
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
        /// <para>Type = 11: Số lần đăng ký cho 1 đối tác (Para_1 = PartnerID)</para>
        /// <para>Type = 12: Lấy tổng số thuê bao nhóm theo ServiceId, PartnerID</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1)
        {
            try
            {
                string[] mPara = { "Type", "Para_1" };
                string[] mValue = { Type.ToString(), Para_1 };
                return mGet.GetDataTable("Sp_Subscriber_Select", mPara, mValue);
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
        /// <para>Type = 5: Lấy thông tin chi tiết 1 Recodr theo (Para_1 = PID, Para_2 = ProductOrderKey)</para>
        /// <para>Type = 7: Lất tất cả dịch vụ đã đăng ký của 1 số điện thoại (Para_1 = PID, Para_2 = MSISDN)</para>
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
                return mGet.GetDataTable("Sp_Subscriber_Select", mPara, mValue);
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
        /// <para>Type = 2: Lấy chi tiết 1 Record (Para_1 = PID, Para_2 = MSISDN, Para_3 = ServiceID)</para>        
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2, string Para_3)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3 };
                return mGet.GetDataTable("Sp_Subscriber_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_Subscriber_Insert", mpara, mValue) > 0)
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
        /// <para>Type = 0: Xóa theo MSISDN và ServiceID</para>
        /// <para>Type = 1: Xóa theo MSISDN</para>
        /// <para>Type = 2: Xóa theo PID và MSISDN</para>
        /// <para>Type = 3: Xóa theo PID, MSISDN, ServiceID</para>
        /// <para>Type = 4: Xóa theo số điện thoại và PID và ServiceID (PID, MSISDN, ServiceID), đồng thời them vao table Cancel</para>
        /// </param>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public bool Delete(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_Subscriber_Delete", mpara, mValue) > 0)
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
                if (mExec.ExecProcedure("Sp_Subscriber_Update", mpara, mValue) > 0)
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
        /// Count số lượng thuê bao cho dịch vụ
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Count cho tất cả các dịch vụ</para>
        /// <para>Type = 1: Count cho 1 dịch vụ duy nhất</para>
        /// </param>
        /// <param name="ServiceID"></param>
        /// <returns></returns>
        public DataTable Search_Count(int Type, int ServiceID, string OrderBy)
        {
            try
            {
                string[] mPara = { "Type", "ServiceID", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), ServiceID.ToString(), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_Subscriber_Search_Count", mPara, mValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Total_Count(int Type, int ServiceID)
        {
            try
            {
                string[] mPara = { "Type", "ServiceID", "IsTotalRow" };
                string[] mValue = { Type.ToString(), ServiceID.ToString(), true.ToString() };                
                return (int)mGet.GetExecuteScalar("Sp_Subscriber_Search_Count", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Count số lượng thuê bao cho dịch vụ
        /// </summary>
        /// <param name="Type">Cách thức lấy
        /// <para>Type = 0: Count cho tất cả các dịch vụ</para>
        /// <para>Type = 1: Count cho 1 dịch vụ duy nhất</para>
        /// </param>
        /// <param name="ServiceID"></param>
        /// <returns></returns>
        public DataTable Search_Count_Partner(int Type, int ServiceID, int PartnerID, string OrderBy)
        {
            try
            {
                string[] mPara = { "Type", "ServiceID","PartnerID", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), ServiceID.ToString(),PartnerID.ToString(), OrderBy, false.ToString() };
                return mGet.GetDataTable("Sp_Subscriber_Search_Count_Partner", mPara, mValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Total_Count_Partner(int Type, int ServiceID, int PartnerID)
        {
            try
            {
                string[] mPara = { "Type", "ServiceID","PartnerID", "IsTotalRow" };
                string[] mValue = { Type.ToString(), ServiceID.ToString(),PartnerID.ToString(), true.ToString() };
                return (int)mGet.GetExecuteScalar("Sp_Subscriber_Search_Count_Partner", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
