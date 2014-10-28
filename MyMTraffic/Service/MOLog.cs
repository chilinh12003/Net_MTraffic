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
    public class MOLog
    {
        public string ConvertMTTypeIDToDescription(DefineMT.MTType mMTType, MyConfig.ChannelType mChannelType)
        {
            try
            {
                if (mChannelType == MyConfig.ChannelType.WAP)
                {
                   if ( mMTType == DefineMT.MTType.RegAgainSuccessFree ||
                        mMTType == DefineMT.MTType.RegAgainSuccessNotFree ||
                        mMTType == DefineMT.MTType.RegFail ||
                        mMTType == DefineMT.MTType.RegHelp ||
                        mMTType == DefineMT.MTType.RegNewSuccess ||
                        mMTType == DefineMT.MTType.RegNotEnoughMoney ||
                        mMTType == DefineMT.MTType.RegRepeatFree ||
                        mMTType == DefineMT.MTType.RegRepeatNotFree)
                    {
                        return "Đăng ký từ WAP";
                    }
                    else if (mMTType == DefineMT.MTType.DeRegFail ||
                        mMTType == DefineMT.MTType.DeRegNotRegister ||
                        mMTType == DefineMT.MTType.DeRegSuccess)
                    {
                        return "Hủy đăng ký từ WAP";
                    }
                    else
                       return "Truy cập WAP";
                }
                else
                {
                    if (mMTType == DefineMT.MTType.Invalid)
                        return "Gửi MO sai cú pháp";
                    else if (mMTType == DefineMT.MTType.RegAgainSuccessFree ||
                            mMTType == DefineMT.MTType.RegAgainSuccessNotFree ||
                        mMTType == DefineMT.MTType.RegFail ||
                        mMTType == DefineMT.MTType.RegHelp ||
                        mMTType == DefineMT.MTType.RegNewSuccess ||
                        mMTType == DefineMT.MTType.RegNotEnoughMoney ||
                        mMTType == DefineMT.MTType.RegRepeatFree ||
                        mMTType == DefineMT.MTType.RegRepeatNotFree)
                    {
                        return "Gửi MO DK dịch vụ";
                    }
                    else if (mMTType == DefineMT.MTType.DeRegFail ||
                        mMTType == DefineMT.MTType.DeRegNotRegister ||
                        mMTType == DefineMT.MTType.DeRegSuccess)
                    {
                        return "Gửi MO HUY dịch vụ";
                    }
                    else
                        return "Gửi MO";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ConvertMTTypeIDToActionName(DefineMT.MTType mMTType, MyConfig.ChannelType mChannelType)
        {
            try
            {
                if (mChannelType == MyConfig.ChannelType.WAP)
                {
                    if (mMTType == DefineMT.MTType.RegAgainSuccessFree ||
                            mMTType == DefineMT.MTType.RegAgainSuccessNotFree ||
                        mMTType == DefineMT.MTType.RegFail ||
                        mMTType == DefineMT.MTType.RegHelp ||
                        mMTType == DefineMT.MTType.RegNewSuccess ||
                        mMTType == DefineMT.MTType.RegNotEnoughMoney ||
                        mMTType == DefineMT.MTType.RegRepeatFree ||
                        mMTType == DefineMT.MTType.RegRepeatNotFree)
                        return "Đăng ký từ WAP";
                    else if (mMTType == DefineMT.MTType.DeRegFail ||
                            mMTType == DefineMT.MTType.DeRegNotRegister ||
                        mMTType == DefineMT.MTType.DeRegSuccess)
                        return "Hủy từ WAP";
                    else
                        return "Truy cập WAP";
                }
                else 
                {
                    if (mMTType == DefineMT.MTType.Invalid)
                        return "Gửi MO sai cú pháp";
                    else if (mMTType == DefineMT.MTType.RegAgainSuccessFree ||
                            mMTType == DefineMT.MTType.RegAgainSuccessNotFree ||
                        mMTType == DefineMT.MTType.RegFail ||
                        mMTType == DefineMT.MTType.RegHelp ||
                        mMTType == DefineMT.MTType.RegNewSuccess ||
                        mMTType == DefineMT.MTType.RegNotEnoughMoney ||
                        mMTType == DefineMT.MTType.RegRepeatFree ||
                        mMTType == DefineMT.MTType.RegRepeatNotFree)
                    {
                        return "Gửi MO Đăng ký";
                    }
                    else if (mMTType == DefineMT.MTType.DeRegFail ||
                        mMTType == DefineMT.MTType.DeRegNotRegister ||
                        mMTType == DefineMT.MTType.DeRegSuccess)
                    {
                        return "Gửi MO Hủy";
                    }
                    else
                        return "Gửi MO";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MyExecuteData mExec;
        MyGetData mGet;

        public MOLog()
        {
            mExec = new MyExecuteData();
            mGet = new MyGetData();
        }

        public MOLog(string KeyConnect_InConfig)
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
                DataSet mSet = mGet.GetDataSet("Sp_MOLog_Select", mPara, mValue);
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
                if (mExec.ExecProcedure("Sp_MOLog_Insert", mpara, mValue) > 0)
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


        public int TotalRow(int? Type, string SearchContent, int PID, int ServiceID, int MTTypeID, int ChannelTypeID, DateTime BeginDate, DateTime EndDate)
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
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID", "MTTypeID", "ChannelTypeID", "BeginDate", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), MTTypeID.ToString(),  ChannelTypeID.ToString(), str_BeginDate, str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_MOLog_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, int MTTypeID,  int ChannelTypeID, DateTime BeginDate, DateTime EndDate, string OrderBy)
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

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "MTTypeID",  "ChannelTypeID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), MTTypeID.ToString(),  ChannelTypeID.ToString(), str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_MOLog_Search", mpara, mValue);
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


        public int TotalRow_Action(int? Type, string SearchContent, int PID, int ServiceID, DateTime BeginDate, DateTime EndDate)
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
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID", "BeginDate", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(),  str_BeginDate, str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_MOLog_Search_Action", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_Action(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, DateTime BeginDate, DateTime EndDate, string OrderBy)
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

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID", "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(),  str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_MOLog_Search_Action", mpara, mValue);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                mTable.Columns.Add(mCol_1);

                DataColumn mCol_2 = new DataColumn("ActionName", typeof(string));
                mTable.Columns.Add(mCol_2);

                DataColumn mCol_3 = new DataColumn("Description", typeof(string));
                mTable.Columns.Add(mCol_3);

                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mRow["ActionName"] = ConvertMTTypeIDToActionName((DefineMT.MTType)int.Parse(mRow["MTTypeID"].ToString()), (MyConfig.ChannelType)int.Parse(mRow["ChannelTypeID"].ToString()));
                    mRow["Description"] = ConvertMTTypeIDToDescription((DefineMT.MTType)int.Parse(mRow["MTTypeID"].ToString()), (MyConfig.ChannelType)int.Parse(mRow["ChannelTypeID"].ToString()));

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

        public int TotalRow_MOMT(int? Type, string SearchContent, int PID, int ServiceID,  DateTime BeginDate, DateTime EndDate)
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
                string[] mPara = { "Type", "SearchContent", "PID", "ServiceID",  "BeginDate", "EndDate", "IsTotalRow" };
                string[] mValue = { Type.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(), str_BeginDate, str_EndDate, true.ToString() };

                return (int)mGet.GetExecuteScalar("Sp_MOMTLog_Search", mPara, mValue);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Search_MOMT(int? Type, int BeginRow, int EndRow, string SearchContent, int PID, int ServiceID, DateTime BeginDate, DateTime EndDate, string OrderBy)
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

                string[] mpara = { "Type", "BeginRow", "EndRow", "SearchContent", "PID", "ServiceID",  "BeginDate", "EndDate", "OrderBy", "IsTotalRow" };
                string[] mValue = { Type.ToString(), BeginRow.ToString(), EndRow.ToString(), SearchContent, PID.ToString(), ServiceID.ToString(),  str_BeginDate, str_EndDate, OrderBy, false.ToString() };
                DataTable mTable = mGet.GetDataTable("Sp_MOMTLog_Search", mpara, mValue);
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

    }
}
