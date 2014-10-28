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
using MyMTraffic.Gateway;
namespace MyCCare.Admin_CCare
{
    public partial class Ad_ResendMT : System.Web.UI.Page
    {
        public int PageIndex = 1;
        ActionLog mActionLog = new ActionLog();
        Subscriber mSub = new Subscriber();
        ems_send_queue mQueue = new ems_send_queue(MySetting.AdminSetting.MySQLConnection_Gateway);

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
                mMaster.Title = "GUI - Cài đặt dịch vụ";

                if (!IsPostBack)
                {
                    BindCombo(1);
                    ViewState["SortBy"] = string.Empty;

                    tbx_FromDate.Value = MyConfig.StartDayOfMonth.ToString(MyConfig.ShortDateFormat);
                    tbx_ToDate.Value = DateTime.Now.ToString(MyConfig.ShortDateFormat);
                }

                Admin_Paging1.rpt_Data = rpt_Data;
                Admin_Paging1.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging1_GetData_Callback_Change);
                Admin_Paging1.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging1_GetTotalPage_Callback_Change);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
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
                return mActionLog.TotalRow(SearchType, SearchContent, PID, ServiceID, BeginDate, EndDate);
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

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                int ServiceID = 0;
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                DataTable mTable = mActionLog.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, SearchContent, PID, ServiceID, BeginDate, EndDate, SortBy);

                DataColumn mCol_2 = new DataColumn("ReceiveDate", typeof(DateTime));

                if (!mTable.Columns.Contains("ReceiveDate"))
                {
                    mTable.Columns.Add(mCol_2);
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
                Admin_Paging1.ResetLoadData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        protected void btn_Resend_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn_Resend = (Button)sender;

                string MSISDN = tbx_MSISDN.Value;
                string LogID = btn_Resend.CommandArgument.TrimEnd().TrimStart();
                string RegKeyword = string.Empty;


                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_MSISDN.Value = MSISDN;
                              
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);

                DataTable mTable_ActionLOg = mActionLog.Select(2, PID.ToString(), LogID);
                if(mTable_ActionLOg.Rows.Count < 1)
                {
                    MyMessage.ShowError("Nội dung không hợp lệ, xin vui lòng kiểm tra lại số điện thoại.");
                    return;
                }

                int ServiceID = (int)mTable_ActionLOg.Rows[0]["ServiceID"];
                string MTContent = mTable_ActionLOg.Rows[0]["MT"].ToString();

                DataTable mTable = mSub.Select(2, PID.ToString(), MSISDN, ServiceID.ToString());

                if (mTable.Rows.Count < 1)
                {
                    MyMessage.ShowError("Số điện thoại chưa đăng ký dịch vụ này, nên không thể gửi tin nhắn.");
                    return;
                }

                string REQUEST_ID = MySecurity.CreateCode(9);

                if (SendMT(REQUEST_ID, RegKeyword, MSISDN, MTContent))
                {
                    UpdateMOLog(REQUEST_ID,MSISDN, DefineMT.MTType.Default, ServiceID, string.Empty, MTContent);
                    MyMessage.ShowError("Gửi MT thành công.");
                }
                else
                {
                    MyMessage.ShowError("Gửi MT KHÔNG thành công.");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }
        private void UpdateMOLog(string RequestID, string MSISDN, DefineMT.MTType mMTType, int ServiceID, string LogContent, string MT)
        {
            try
            {
                DataSet mSet = mActionLog.CreateDataSet();
                DataRow mRow = mSet.Tables[0].NewRow();
                mRow["ServiceID"] = ServiceID;
                mRow["MSISDN"] = MSISDN;
                mRow["ActionTypeID"] = (int)ActionLog.ActionType.ResendMT;
                mRow["ActionTypeName"] = ActionLog.ActionType.ResendMT.ToString();
                mRow["LogDate"] = DateTime.Now;
                mRow["LogContent"] = LogContent;
                mRow["MO"] = LogContent;
                mRow["MT"] = MT;
                mRow["RequestID"] = RequestID;
                mRow["LogPID"] = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);

                mSet.Tables[0].Rows.Add(mRow);
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                mActionLog.Insert(0, mSet.GetXml());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendMT(string REQUEST_ID,string COMMAND_CODE, string USER_ID, string MTContent)
        {
            string SERVICE_ID = MySetting.AdminSetting.ShoreCode;
            
            bool Result = false;
            try
            {
                Result = mQueue.Insert(USER_ID, SERVICE_ID, COMMAND_CODE, MTContent, REQUEST_ID);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MyLogfile.WriteLogData("_Resend_MT", "UserName:" + Login1.GetUserName() + "|USER_ID:" + USER_ID + "|COMMAND_CODE:" + COMMAND_CODE + "|REQUEST_ID:" + REQUEST_ID + "|INFO:" + MTContent + "|Result:" + Result.ToString());
            }
        }

    }
}