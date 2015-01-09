using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;using MyBase.MyWeb;
using MyMTraffic;
using MyMTraffic.Service;
using MyMTraffic.Sub;

namespace MyCCare.Admin_CCare
{
    public partial class Ad_HistoryUsing : MyASPXBase
    {
        public int PageIndex = 1;
        MOLog mMOLog = new MOLog();

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        Service mService = new Service();
                        sel_Service.DataSource = mService.Select(4, string.Empty);
                        sel_Service.DataTextField = "ServiceName";
                        sel_Service.DataValueField = "ServiceID";
                        sel_Service.DataBind();
                        sel_Service.Items.Insert(0, new ListItem("--Chọn gói cước--", "0"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
                mMaster.Title = "GUI - Lịch sử sử dụng";

                if (!IsPostBack)
                {
                    BindCombo(1);

                    ViewState["SortBy"] = string.Empty;
                    tbx_MSISDN.Value = MySetting.AdminSetting.MSISDN;

                    tbx_FromDate.Value = MySetting.AdminSetting.BeginDate;
                    tbx_ToDate.Value = MySetting.AdminSetting.EndDate;
                }
                else
                {
                    MySetting.AdminSetting.BeginDate = tbx_FromDate.Value;
                    MySetting.AdminSetting.EndDate = tbx_ToDate.Value;
                }
                Admin_Paging1.rpt_Data = rpt_Data;
                Admin_Paging1.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging1_GetData_Callback_Change);
                Admin_Paging1.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging1_GetTotalPage_Callback_Change);
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.LoadDataError, true, ex);
            }
        }

        int Admin_Paging1_GetTotalPage_Callback_Change()
        {
            try
            {
                int SearchType = 0;
                string SortBy = ViewState["SortBy"].ToString();
                string SearchContent = tbx_MSISDN.Value;

                int PID = 0;

                DateTime BeginDate = tbx_FromDate.Value.Length > 0 ? DateTime.ParseExact(tbx_FromDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                DateTime EndDate = tbx_ToDate.Value.Length > 0 ? DateTime.ParseExact(tbx_ToDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                EndDate = EndDate.AddDays(1);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);
                int ServiceID = 0;
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                return mMOLog.TotalRow(SearchType, SearchContent, PID,ServiceID, 0, 0, BeginDate, EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging1_GetData_Callback_Change()
        {
            try
            {
                int SearchType = 0;
                string SortBy = ViewState["SortBy"].ToString();
                string SearchContent = tbx_MSISDN.Value;

                int PID = 0;

                DateTime BeginDate = tbx_FromDate.Value.Length > 0 ? DateTime.ParseExact(tbx_FromDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                DateTime EndDate = tbx_ToDate.Value.Length > 0 ? DateTime.ParseExact(tbx_ToDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                EndDate = EndDate.AddDays(1);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                int ServiceID = 0;
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                DataTable mTable = mMOLog.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, SearchContent, PID,ServiceID, 0, 0, BeginDate, EndDate, SortBy);

                DataColumn mCol_2 = new DataColumn("ReceiveDate", typeof(DateTime));
                DataColumn mCol_1 = new DataColumn("ActionName", typeof(string));

                if (!mTable.Columns.Contains("ReceiveDate"))
                {
                    mTable.Columns.Add(mCol_2);
                }
                if (!mTable.Columns.Contains("ActionName"))
                {
                    mTable.Columns.Add(mCol_1);
                }
                foreach (DataRow mRow in mTable.Rows)
                {

                    DateTime mDate_Receive = (DateTime)mRow["LogDate"];
                    DateTime mDate_SendDate = new DateTime(mDate_Receive.Year, mDate_Receive.Month, mDate_Receive.Day, mDate_Receive.Hour, mDate_Receive.Minute, mDate_Receive.Second);

                    Random mRandom = new Random();
                    int Delay = 5;
                    mDate_Receive = mDate_Receive.AddSeconds(-Delay);

                    mRow["LogDate"] = mDate_SendDate;
                    mRow["ReceiveDate"] = mDate_Receive;
                    mRow["ActionName"] = mMOLog.ConvertMTTypeIDToActionName((DefineMT.MTType)int.Parse(mRow["MTTypeID"].ToString()), (MyConfig.ChannelType)int.Parse(mRow["ChannelTypeID"].ToString()));
                }
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string MSISDN = tbx_MSISDN.Value;
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_MSISDN.Value = MSISDN;
                MySetting.AdminSetting.MSISDN = MSISDN;
                Admin_Paging1.ResetLoadData();
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SeachError, true, ex);
            }
        }

    }
}