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
    public class ChargeLog
    {
        public enum ChargeType
        {
            NOTHING = 0,
            [DescriptionAttribute("Đăng ký")]
            REG = 1,
            [DescriptionAttribute("Gia hạn")]
            RENEW = 2,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG = 3,
        }

        public enum ChargeType_Service
        {
            NOTHING = 0,
            [DescriptionAttribute("Đăng ký")]
            REG = 1,
            [DescriptionAttribute("Gia hạn")]
            RENEW = 2,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG = 3,

            [DescriptionAttribute("Đăng ký")]
            REG_GTHN_WEEKLY = 100,
            [DescriptionAttribute("Gia hạn")]
            RENEW_GTHN_WEEKLY = 101,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_GTHN_WEEKLY = 102,

            [DescriptionAttribute("Đăng ký")]
            REG_GTHCM_WEEKLY = 200,
            [DescriptionAttribute("Gia hạn")]
            RENEW_GTHCM_WEEKLY = 201,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_GTHCM_WEEKLY = 202,

            [DescriptionAttribute("Đăng ký")]
            REG_DN_WEEKLY = 300,
            [DescriptionAttribute("Gia hạn")]
            RENEW_DN_WEEKLY = 301,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_DN_WEEKLY = 302,

            [DescriptionAttribute("Đăng ký")]
            REG_KP_WEEKLY = 400,
            [DescriptionAttribute("Gia hạn")]
            RENEW_KP_WEEKLY = 401,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_KP_WEEKLY = 402,

            [DescriptionAttribute("Đăng ký")]
            REG_LGT_WEEKLY = 500,
            [DescriptionAttribute("Gia hạn")]
            RENEW_LGT_WEEKLY = 501,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_LGT_WEEKLY = 502,

            [DescriptionAttribute("Đăng ký")]
            REG_TV_WEEKLY = 600,
            [DescriptionAttribute("Gia hạn")]
            RENEW_TV_WEEKLY = 601,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_TV_WEEKLY = 602,

            [DescriptionAttribute("Đăng ký")]
            REG_HS_WEEKLY = 700,
            [DescriptionAttribute("Gia hạn")]
            RENEW_HS_WEEKLY = 701,
            [DescriptionAttribute("Hủy đăng ký")]
            UNREG_HS_WEEKLY = 702

        }

        public static string GetListChargeType_Service(ChargeType mChargeType)
        {
            try
            {
                string List = string.Empty;
                switch (mChargeType)
                {
                    case ChargeType.REG:
                        List = "1,100,200,300,400,500,600,700";
                        break;
                    case ChargeType.RENEW:
                        List = "2,101,201,301,401,501,601,701";
                        break;
                    case ChargeType.UNREG:
                        List = "3,102,202,302,402,502,602,702";
                        break;
                    default:
                        List = string.Empty;
                        break;
                }
                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public enum ChargeStatus
        {
            ChargeSuccess=0, 
            BlanceTooLow=1, 
            WrongUserAndPassword=2, 
            ChargeNotComplete=3, 
            OtherError=4, 
            WrongSubscriberNumber=5, 
            SubDoesNotExist=6, 
            OverChargeLimit=7,
            OverChargeLimit2=17, 
            ServerInternalError=8, 
            ConfigError=9, 
            RequestIDIsNull=10, 
            InvalidSubscriptionState = 11,
            UnknowIP=99, 
            SynctaxXMLError=100, 
            UnknownRequest=500,
            VNPAPIError = -1,

        }

        MyExecuteData mExec;
        MyGetData mGet;

        public ChargeLog()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public ChargeLog(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_ChargeLog_Select", mPara, mValue);
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

        public bool Insert(int? Type, string XMLContent)
        {
            try
            {
                string[] mpara = { "Type", "XMLContent" };
                string[] mValue = { Type.ToString(), XMLContent };
                if (mExec.ExecProcedure("Sp_ChargeLog_Insert", mpara, mValue) > 0)
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
        /// <param name="Type">
        /// <para>Type = 8: Lay số lượng thuê bao theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = ChargeStatusID, Para_4 = ChannelTypeID, Para_5 = BeginDate, Para_6 = EndDate</para>
        /// <para>Type = 11: Lấy tổng tiền theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = ChargeStatusID, Para_4 = ChannelTypeID, Para_5 = BeginDate, Para_6 = EndDate</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <param name="Para_4"></param>
        /// <param name="Para_5"></param>
        /// <param name="Para_6"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2, string Para_3, string Para_4, string Para_5, string Para_6)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3", "Para_4", "Para_5", "Para_6" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3, Para_4, Para_5, Para_6 };
                return mGet.GetDataTable("Sp_ChargeLog_Select", mPara, mValue);
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
        /// <para>Type = 7: Lay số lượng thuê bao theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = ChargeStatusID, Para_4 = BeginDate, Para_5 = EndDate</para>
        /// <para>Type = 10: Lấy tổng tiền theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = ChargeStatusID, Para_4 = BeginDate, Para_5 = EndDate</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <param name="Para_4"></param>
        /// <param name="Para_5"></param>
        /// <param name="Para_6"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2, string Para_3, string Para_4, string Para_5)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3", "Para_4", "Para_5" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3, Para_4, Para_5};
                return mGet.GetDataTable("Sp_ChargeLog_Select", mPara, mValue);
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
        /// <para>Type = 2: Lấy tổng số lệnh charge (Para_1 = ChargeStatusID,Para_2= ChargeTypeID, Para_3=BeginDate, Para_4 = EndDate)</para>
        /// <para>Type = 6: Lay số lượng thuê bao theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = BeginDate, Para_4 = EndDate</para>
        /// <para>Type = 9: Lấy tổng tiền theo từng dịch vụ và đối tác (Para_1 = PID, Para_2 = ChargeTypeID,Para_3 = BeginDate, Para_4 = EndDate</para>
        /// </param>
        /// <param name="Para_1"></param>
        /// <param name="Para_2"></param>
        /// <param name="Para_3"></param>
        /// <returns></returns>
        public DataTable Select(int Type, string Para_1, string Para_2, string Para_3, string Para_4)
        {
            try
            {
                string[] mPara = { "Type", "Para_1", "Para_2", "Para_3", "Para_4" };
                string[] mValue = { Type.ToString(), Para_1, Para_2, Para_3, Para_4 };
                return mGet.GetDataTable("Sp_ChargeLog_Select", mPara, mValue);
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
        /// <para>Type = 3: Lấy tổng số lệnh charge (Para_1= ChargeTypeID, Para_2=BeginDate, Para_3 = EndDate)</para>
        ///<para>Type = 4: Lấy tổng số lệnh charge (Para_1 = ChargeStatusID, Para_2=BeginDate, Para_3 = EndDate)</para>
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
                return mGet.GetDataTable("Sp_ChargeLog_Select", mPara, mValue);
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
        /// <para>Type = 5: Lấy tổng số lệnh charge (Para_1 = BeginDate, Para_2 = EndDate)</para>
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
                return mGet.GetDataTable("Sp_ChargeLog_Select", mPara, mValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tổng lệnh charge theo tình trạng charge và theo loại charge
        /// </summary>
        /// <param name="mStatus"></param>
        /// <param name="mType"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public int GetTotal(ChargeStatus mStatus, ChargeType mType, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                DataTable mTable = Select(2, ((int)mStatus).ToString(), ((int)mType).ToString(), BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                if (mTable == null || mTable.Rows.Count < 1)
                    return 0;

                return int.Parse(mTable.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng lệnh charge theo kiểu charge
        /// </summary>
        /// <param name="mType"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public int GetTotal(ChargeType mType, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                DataTable mTable = Select(3, ((int)mType).ToString(), BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                if (mTable == null || mTable.Rows.Count < 1)
                    return 0;

                return int.Parse(mTable.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng lệnh charge theo tình trạng
        /// </summary>
        /// <param name="mStatus"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public int GetTotal(ChargeStatus mStatus, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                DataTable mTable = Select(4, ((int)mStatus).ToString(), BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                if (mTable == null || mTable.Rows.Count < 1)
                    return 0;

                return int.Parse(mTable.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tổng lệnh charge
        /// </summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public int GetTotal(DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                DataTable mTable = Select(5, BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                if (mTable == null || mTable.Rows.Count < 1)
                    return 0;

                return int.Parse(mTable.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow(int? Type, string SearchContent, int PID, int ServiceID,int ChargeTypeID, int ChargeStatusID, int ChannelTypeID,  DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID","ChargeTypeID","ChargeStatusID","ChannelTypeID", "BeginDate", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(),ChargeTypeID.ToString(),ChargeStatusID.ToString(),ChannelTypeID.ToString(),str_BeginDate,str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, int ChargeTypeID, int ChargeStatusID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "ChargeTypeID", "ChargeStatusID", "ChannelTypeID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), ChannelTypeID.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_ByDay(int? Type, int ServiceID, int ChargeTypeID, int ChargeStatusID, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type","ServiceID", "ChargeTypeID", "ChargeStatusID", "BeginDate", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(),ServiceID.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), str_BeginDate, str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search_ByDay", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_ByDay(int? Type, int BeginRow, int EndRow, int ServiceID, int ChargeTypeID, int ChargeStatusID, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "ServiceID","ChargeTypeID", "ChargeStatusID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(),ServiceID.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search_ByDay", mpara, mValue);

                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_ByDay_Price(int? Type, int ServiceID, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "BeginDate","ServiceID", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), str_BeginDate,ServiceID.ToString(), str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search_ByDay_Price", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_ByDay_Price(int? Type, int BeginRow, int EndRow, int ServiceID, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow","ServiceID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(),ServiceID.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search_ByDay_Price", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_SelectType(int? Type, string SearchContent, int PID, int ServiceID, int ChargeTypeID, int ChargeStatusID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate, int SelectType)
        {
            try
            {

                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID", "ChargeTypeID", "ChargeStatusID", "ChannelTypeID", "BeginDate", "EndDate","SelectType", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), ChannelTypeID.ToString(), str_BeginDate, str_EndDate,SelectType.ToString(), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_SelectType(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, int ChargeTypeID, int ChargeStatusID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate, int SelectType, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "ChargeTypeID", "ChargeStatusID", "ChannelTypeID", "BeginDate", "EndDate","SelectType", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), ChannelTypeID.ToString(), str_BeginDate, str_EndDate,SelectType.ToString(), OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_Renew(int? Type, string SearchContent, int PID, int ServiceID, ChargeType mChargeType, int ChargeStatusID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate, int SelectType)
        {
            try
            {

                string str_BeginDate = null;
                string str_EndDate = null;
                string ListChargeTypeID = string.Empty;

                ListChargeTypeID = GetListChargeType_Service(mChargeType);
                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID", "ChargeTypeID", "ChargeStatusID", "ListChargeTypeID", "BeginDate", "EndDate", "SelectType", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), ListChargeTypeID, ChargeStatusID.ToString(), ChannelTypeID.ToString(), str_BeginDate, str_EndDate, SelectType.ToString(), true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_Renew(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, ChargeType mChargeType, int ChargeStatusID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate, int SelectType, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;
                string ListChargeTypeID = string.Empty;
                ListChargeTypeID = GetListChargeType_Service(mChargeType);

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "ListChargeTypeID", "ChargeStatusID", "ChannelTypeID", "BeginDate", "EndDate", "SelectType", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), ListChargeTypeID, ChargeStatusID.ToString(), ChannelTypeID.ToString(), str_BeginDate, str_EndDate, SelectType.ToString(), OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                        continue;

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_ByDay_Partner(int? Type, int ChargeTypeID, int ChargeStatusID, int PartnerID, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "ChargeTypeID", "ChargeStatusID", "BeginDate", "EndDate", "IsTotalRow", "PartnerID" };
                string[] mValue = { Type.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), str_BeginDate, str_EndDate, true.ToString(), PartnerID.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search_ByDay_Partner", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_ByDay_Partner(int? Type, int BeginRow, int EndRow, int ChargeTypeID, int ChargeStatusID, int PartnerID, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "ChargeTypeID", "ChargeStatusID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow", "PartnerID" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), ChargeTypeID.ToString(), ChargeStatusID.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString(), PartnerID.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search_ByDay_Partner", mpara, mValue);

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int TotalRow_ByDay_Price_Partner(int? Type, int PartnerID, DateTime BeginDate, DateTime EndDate)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }
                string[] mPara = { "Type", "BeginDate", "EndDate", "IsTotalRow", "PartnerID" };
                string[] mValue = { Type.ToString(), str_BeginDate, str_EndDate, true.ToString(), PartnerID.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_ChargeLog_Search_ByDay_Price_Partner", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_ByDay_Price_Partner(int? Type, int PartnerID, int BeginRow, int EndRow, DateTime BeginDate, DateTime EndDate, string OrderBy)
        {
            try
            {
                string str_BeginDate = null;
                string str_EndDate = null;

                if (BeginDate != DateTime.MinValue && BeginDate != DateTime.MaxValue &&
                    EndDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                {
                    str_BeginDate = BeginDate.ToString(MyConfig.DateFormat_InsertToDB);
                    str_EndDate = EndDate.ToString(MyConfig.DateFormat_InsertToDB);
                }

                string[] mpara = { "Type", "BeginRow", "EndRow", "BeginDate", "EndDate", "OrderBy", "IsTotalRow", "PartnerID" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString(), PartnerID.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_ChargeLog_Search_ByDay_Price_Partner", mpara, mValue);

                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
