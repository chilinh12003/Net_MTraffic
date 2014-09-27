using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Service;
using MyMTraffic.Sub;

namespace MyAdmin.Admin_CCare
{
    public partial class SubInfo : System.Web.UI.Page
    {
        MOLog mMOLog = new MOLog();
        ChargeLog mChargeLog = new ChargeLog();
        public int PageIndex_1 = 1;
        public int PageIndex_2 = 1;
        public int PageIndex_3 = 1;

        public string MSISDN = string.Empty;
        public string StatusName = string.Empty;
        public string ServiceName = "";
        public string EffectiveDate = string.Empty;
        public string ExpiryDate = string.Empty;

        DateTime BeginDate_StartServcie = new DateTime(2012, 08, 01);

        protected void Page_Load(object sender, EventArgs e)
        {
            MSISDN = Request.QueryString["msisdn"];

            BindDataSub();

            Admin_Paging_VNP1.rpt_Data = rpt_Reg;
            Admin_Paging_VNP1.GetData_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetData_Callback(Admin_Paging_VNP1_GetTotalPage_Callback_Change);
            Admin_Paging_VNP1.GetTotalPage_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetTotalPage_Callback(Admin_Paging_VNP1_GetData_Callback_Change);

            Admin_Paging_VNP2.rpt_Data = rpt_Renew;
            Admin_Paging_VNP2.GetData_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetData_Callback(Admin_Paging_VNP2_GetTotalPage_Callback_Change);
            Admin_Paging_VNP2.GetTotalPage_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetTotalPage_Callback(Admin_Paging_VNP2_GetData_Callback_Change);

            Admin_Paging_VNP3.rpt_Data = rpt_MOLog;
            Admin_Paging_VNP3.GetData_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetData_Callback(Admin_Paging_VNP3_GetTotalPage_Callback_Change);
            Admin_Paging_VNP3.GetTotalPage_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetTotalPage_Callback(Admin_Paging_VNP3_GetData_Callback_Change);

            Admin_Paging_VNP_Action.rpt_Data = rpt_Action;
            Admin_Paging_VNP_Action.GetData_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetData_Callback(Admin_Paging_VNP_Action_GetTotalPage_Callback_Change);
            Admin_Paging_VNP_Action.GetTotalPage_Callback_Change += new Admin_Control.Admin_Paging_VNP.GetTotalPage_Callback(Admin_Paging_VNP_Action_GetData_Callback_Change);

        }

        private void BindDataSub()
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN))
                    return;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return;
                }
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);

                Subscriber mSub = new Subscriber();
                DataTable mTable = mSub.Select(7, PID.ToString(), MSISDN);


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
                rpt_Sub.DataSource = mTable;
                rpt_Sub.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        int Admin_Paging_VNP1_GetData_Callback_Change()
        {
            try
            {
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                return mChargeLog.TotalRow_SelectType(SearchType, SearchContent, PID, 0, 0, 0, 0, BeginDate, EndDate, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging_VNP1_GetTotalPage_Callback_Change()
        {
            try
            {
                string SortBy = "ChargeLogID DESC";
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }

                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);
                
                PageIndex_1 = (Admin_Paging_VNP1.mPaging.CurrentPageIndex - 1) * Admin_Paging_VNP1.mPaging.PageSize + 1;

                DataTable mTable = mChargeLog.Search_SelectType(SearchType, Admin_Paging_VNP1.mPaging.BeginRow, Admin_Paging_VNP1.mPaging.EndRow, SearchContent, PID,0, 0, 0, 0, BeginDate, EndDate, 1, SortBy);
                DataColumn mCol_1 = new DataColumn("ActionName", typeof(string));
                mTable.Columns.Add(mCol_1);

                foreach (DataRow mRow in mTable.Rows)
                {
                    mRow["ActionName"] = MyEnum.StringValueOf((ChargeLog.ChargeType_Service)int.Parse(mRow["ChargeTypeID"].ToString()));

                    if ((int)mRow["ChargeStatusID"] == 0)
                    {
                        mRow["ChargeStatusName"] = "Thành công";
                    }
                    else
                    {
                        mRow["ChargeStatusName"] = "Không thành công";
                    }
                }
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        int Admin_Paging_VNP2_GetData_Callback_Change()
        {
            try
            {
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);


                return mChargeLog.TotalRow_SelectType(SearchType, SearchContent, PID, 0, 0, 0, 0, BeginDate, EndDate, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging_VNP2_GetTotalPage_Callback_Change()
        {
            try
            {
                string SortBy = "ChargeLogID DESC";
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;


                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                PageIndex_2 = (Admin_Paging_VNP2.mPaging.CurrentPageIndex - 1) * Admin_Paging_VNP2.mPaging.PageSize + 1;

                DataTable mTable = mChargeLog.Search_SelectType(SearchType, Admin_Paging_VNP2.mPaging.BeginRow, Admin_Paging_VNP1.mPaging.EndRow, SearchContent, PID,0, 0, 0, 0, BeginDate, EndDate, 2, SortBy);

                foreach (DataRow mRow in mTable.Rows)
                {
                    if ((int)mRow["ChargeStatusID"] == 0)
                    {
                        mRow["ChargeStatusName"] = "Thành công";
                    }
                    else
                    {
                        mRow["ChargeStatusName"] = "Không thành công";
                    }
                }
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        int Admin_Paging_VNP3_GetData_Callback_Change()
        {
            try
            {
                int SearchType = 0;
                string SearchContent = MSISDN;
                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);


                return mMOLog.TotalRow_MOMT(SearchType, SearchContent, PID,0,  BeginDate, EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging_VNP3_GetTotalPage_Callback_Change()
        {
            try
            {
                string SortBy = "LogID DESC";
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                PageIndex_3 = (Admin_Paging_VNP3.mPaging.CurrentPageIndex - 1) * Admin_Paging_VNP3.mPaging.PageSize + 1;

                DataTable mTable = mMOLog.Search_MOMT(SearchType, Admin_Paging_VNP3.mPaging.BeginRow, Admin_Paging_VNP3.mPaging.EndRow, SearchContent, PID,0, BeginDate, EndDate, SortBy);
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        int Admin_Paging_VNP_Action_GetData_Callback_Change()
        {
            try
            {
                int SearchType = 0;
                string SearchContent = MSISDN;
                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                return mMOLog.TotalRow_Action(SearchType, SearchContent, PID,0, BeginDate, EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging_VNP_Action_GetTotalPage_Callback_Change()
        {
            try
            {
                string SortBy = "LogID DESC";
                int SearchType = 0;

                string SearchContent = MSISDN;

                int PID = 0;

                DateTime BeginDate = BeginDate_StartServcie;
                DateTime EndDate = DateTime.Now;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                PageIndex_3 = (Admin_Paging_VNP_Action.mPaging.CurrentPageIndex - 1) * Admin_Paging_VNP_Action.mPaging.PageSize + 1;

                DataTable mTable = mMOLog.Search_Action(SearchType, Admin_Paging_VNP_Action.mPaging.BeginRow, Admin_Paging_VNP_Action.mPaging.EndRow, SearchContent, PID, 0, BeginDate, EndDate, SortBy);
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}