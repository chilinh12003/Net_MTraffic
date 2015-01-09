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

using MyMTraffic.Permission;
namespace MyAdmin.Admin_CCare
{
    public partial class Ad_CheckInfo : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;
        ActionLog mActionLog = new ActionLog();
        MOLog mMOLog = new MOLog();
        ChargeLog mChargeLog = new ChargeLog();

        string MSISDN = string.Empty;
        /// <summary>
        /// = 1: Lịch sử push tin
        /// = 2: Lịch sử Đăng ký/Hủy
        /// = 3: Lịch sử trừ tiền
        /// </summary>
        int HistoryType
        {
            get
            {
                if (ViewState["HistoryType"] == null)
                    return 1;
                else
                    return (int)ViewState["HistoryType"];
            }
            set
            {
                ViewState["HistoryType"] = value;
            }
        }

        string KeyName
        {
            get
            {
                if (HistoryType == 1)
                    return "SortBy1";
                else if (HistoryType == 2)
                {
                    return "SortBy2";
                }
                else
                {
                    return "SortBy3";
                }
            }
        }

        string SortBy
        {
            get
            {
                if (ViewState[KeyName] == null)
                    return "";
                else
                    return ViewState[KeyName].ToString();
            }
            set
            {
                ViewState[KeyName] = value;
            }
        }

        private void BindData()
        {
            if (HistoryType == 1)
            {
                SortBy = "LogID DESC";
                Admin_Paging1.ResetLoadData();
            }
            else if (HistoryType == 2)
            {
                Admin_Paging2.ResetLoadData();
            }
            else if (HistoryType == 3)
            {
                Admin_Paging3.ResetLoadData();
            }
             else if (HistoryType == 4)
            {
                Admin_Paging4.ResetLoadData();
            }
        }

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
                        sel_Service.Items.Insert(0, new ListItem("--Dịch vụ--", "0"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckPermission()
        {
            try
            {
                if (mGetRole.ViewRole == false)
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            bool IsRedirect = false;
            try
            {
                //Phân quyền
                if (ViewState["Role"] == null)
                {
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.CheckInfo, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }

                if (!CheckPermission())
                {
                    IsRedirect = true;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
            if (IsRedirect)
            {
                Response.End();
            }
        }

        private void ChangeLog(int HisType)
        {
            try
            {
                btn_PushMT.ForeColor = System.Drawing.Color.Black;
                btn_MOlog.ForeColor = System.Drawing.Color.Black;
                btn_ChargeLog.ForeColor = System.Drawing.Color.Black;
                btn_Action.ForeColor = System.Drawing.Color.Black;
                div_1.Visible = false;
                div_2.Visible = false;
                div_3.Visible = false;
                div_4.Visible = false;

                Admin_Paging1.Visible = false;
                Admin_Paging2.Visible = false;
                Admin_Paging3.Visible = false;
                Admin_Paging4.Visible = false;


                if (HistoryType == 1)
                {
                    btn_PushMT.ForeColor = System.Drawing.Color.Red;
                    div_1.Visible = true;
                    Admin_Paging1.Visible = true;
                }
                else if (HistoryType == 2)
                {
                    btn_MOlog.ForeColor = System.Drawing.Color.Red;
                    div_2.Visible = true;
                    Admin_Paging2.Visible = true;
                }
                else if (HistoryType == 3)
                {
                    btn_ChargeLog.ForeColor = System.Drawing.Color.Red;
                    div_3.Visible = true;
                    Admin_Paging3.Visible = true;
                }
                else if (HistoryType == 4)
                {
                    btn_Action.ForeColor = System.Drawing.Color.Red;
                    div_4.Visible = true;
                    Admin_Paging4.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = mGetRole.PageName;

                MSISDN = tbx_MSISDN.Value;

                if (!IsPostBack)
                {
                    BindCombo(1);
                    HistoryType = 1;
                    ChangeLog(HistoryType);                    

                    tbx_FromDate.Value = MyConfig.StartDayOfMonth.ToString(MyConfig.ShortDateFormat);
                    tbx_ToDate.Value = DateTime.Now.ToString(MyConfig.ShortDateFormat);
                }

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                    MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                    if (mTelco != MyConfig.Telco.Vinaphone)
                    {
                        MyMessage.ShowError("Số điện thoại không hợp lệ, xin vui lòng kiểm tra lại.");
                        return;
                    }
                }

                Admin_Paging1.rpt_Data = rpt_Data_1;
                Admin_Paging1.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging1_GetData_Callback_Change);
                Admin_Paging1.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging1_GetTotalPage_Callback_Change);

                Admin_Paging2.rpt_Data = rpt_Data_2;
                Admin_Paging2.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging2_GetData_Callback_Change);
                Admin_Paging2.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging2_GetTotalPage_Callback_Change);

                Admin_Paging3.rpt_Data = rpt_Data_3;
                Admin_Paging3.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging3_GetData_Callback_Change);
                Admin_Paging3.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging3_GetTotalPage_Callback_Change);

                Admin_Paging4.rpt_Data = rpt_Data_4;
                Admin_Paging4.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging4_GetData_Callback_Change);
                Admin_Paging4.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging4_GetTotalPage_Callback_Change);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        private void BindDataSub()
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN))
                    return;
              
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN,MySetting.AdminSetting.MaxPID);

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

        int Admin_Paging1_GetTotalPage_Callback_Change()
        {
            try
            {
                if (HistoryType != 1)
                    return 0;
                int SearchType = 0;
                int ServiceID = 0;


                DateTime BeginDate = tbx_FromDate.Value.Length > 0 ? DateTime.ParseExact(tbx_FromDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                DateTime EndDate = tbx_ToDate.Value.Length > 0 ? DateTime.ParseExact(tbx_ToDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                EndDate = EndDate.AddDays(1);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);
                return mMOLog.TotalRow_MOMT(SearchType, MSISDN, PID, ServiceID, BeginDate, EndDate);
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
                if (HistoryType != 1)
                    return new DataTable();
                int SearchType = 0;

                int ServiceID = 0;

                DateTime BeginDate = tbx_FromDate.Value.Length > 0 ? DateTime.ParseExact(tbx_FromDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                DateTime EndDate = tbx_ToDate.Value.Length > 0 ? DateTime.ParseExact(tbx_ToDate.Value, "dd/MM/yyyy", null) : DateTime.MinValue;
                EndDate = EndDate.AddDays(1);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);
                return mMOLog.Search_MOMT(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, MSISDN, PID, ServiceID, BeginDate, EndDate, " LogDate DESC ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        int Admin_Paging2_GetTotalPage_Callback_Change()
        {
            try
            {
                if (HistoryType != 2)
                    return 0;

                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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
                PID = MyPID.GetPIDByPhoneNumber(SearchContent,MySetting.AdminSetting.MaxPID);

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                return mChargeLog.TotalRow_SelectType(SearchType, SearchContent, PID, ServiceID, 0, 0, 0, BeginDate, EndDate, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging2_GetData_Callback_Change()
        {
            try
            {
                if (HistoryType != 2)
                    return new DataTable();

                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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
                PID = MyPID.GetPIDByPhoneNumber(SearchContent,MySetting.AdminSetting.MaxPID);
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                PageIndex = (Admin_Paging2.mPaging.CurrentPageIndex - 1) * Admin_Paging2.mPaging.PageSize + 1;

                DataTable mTable = mChargeLog.Search_SelectType(SearchType, Admin_Paging2.mPaging.BeginRow, Admin_Paging2.mPaging.EndRow, SearchContent, PID, ServiceID, 0, 0, 0, BeginDate, EndDate, 1, SortBy);

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

        int Admin_Paging3_GetTotalPage_Callback_Change()
        {
            try
            {
                if (HistoryType != 3)
                    return 0;
                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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
                PID = MyPID.GetPIDByPhoneNumber(SearchContent,MySetting.AdminSetting.MaxPID);
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }


                return mChargeLog.TotalRow_SelectType(SearchType, SearchContent, PID, ServiceID, 0, 0, 0, BeginDate, EndDate, 2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging3_GetData_Callback_Change()
        {
            try
            {
                if (HistoryType != 3)
                    return new DataTable();

                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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
                PID = MyPID.GetPIDByPhoneNumber(SearchContent,MySetting.AdminSetting.MaxPID);

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                PageIndex = (Admin_Paging3.mPaging.CurrentPageIndex - 1) * Admin_Paging3.mPaging.PageSize + 1;

                return mChargeLog.Search_SelectType(SearchType, Admin_Paging3.mPaging.BeginRow, Admin_Paging3.mPaging.EndRow, SearchContent, PID, ServiceID, 0, 0, 0, BeginDate, EndDate, 2, SortBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        int Admin_Paging4_GetTotalPage_Callback_Change()
        {
            try
            {
                if (HistoryType != 4)
                    return 0;
                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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
                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }


                return mMOLog.TotalRow_Action(SearchType, SearchContent, PID, ServiceID, BeginDate, EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging4_GetData_Callback_Change()
        {
            try
            {
                if (HistoryType != 4)
                    return new DataTable();

                int SearchType = 0;

                int ServiceID = 0;
                string SearchContent = MSISDN;

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

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }
                PageIndex = (Admin_Paging4.mPaging.CurrentPageIndex - 1) * Admin_Paging4.mPaging.PageSize + 1;

                DataTable mTable = mMOLog.Search_Action(SearchType, Admin_Paging4.mPaging.BeginRow, Admin_Paging4.mPaging.EndRow, SearchContent, PID, ServiceID, BeginDate, EndDate, SortBy);
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        protected void btn_PushMT_Click(object sender, EventArgs e)
        {
            HistoryType = 1;
            ChangeLog(HistoryType);
            BindData();
        }

        protected void btn_MOlog_Click(object sender, EventArgs e)
        {
            HistoryType = 2;
            ChangeLog(HistoryType);
            BindData();
        }

        protected void btn_ChargeLog_Click(object sender, EventArgs e)
        {
            HistoryType = 3;
            ChangeLog(HistoryType);
            BindData();
        }
        protected void btn_Action_Click(object sender, EventArgs e)
        {
            HistoryType = 4;
            ChangeLog(HistoryType);
            BindData();
        }
        protected void lbtn_Sort_Click(object sender, EventArgs e)
        {
            try
            {
                //lbtn_Sort_1.CssClass = "Sort";
                //lbtn_Sort_2.CssClass = "Sort";
                //lbtn_Sort_3.CssClass = "Sort";
                //lbtn_Sort_4.CssClass = "Sort";
                //lbtn_Sort_5.CssClass = "Sort";
                //lbtn_Sort_6.CssClass = "Sort";
                //lbtn_Sort_7.CssClass = "Sort";

                LinkButton mLinkButton = (LinkButton)sender;
                SortBy = mLinkButton.CommandArgument;

                if (mLinkButton.CommandArgument.IndexOf(" ASC") >= 0)
                {
                    mLinkButton.CssClass = "SortActive_Up";
                    mLinkButton.CommandArgument = mLinkButton.CommandArgument.Replace(" ASC", " DESC");
                }
                else
                {
                    mLinkButton.CssClass = "SortActive_Down";
                    mLinkButton.CommandArgument = mLinkButton.CommandArgument.Replace(" DESC", " ASC");
                }

                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SortError, "Chilinh");
            }
        }
        protected void tbx_Search_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataSub();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SortError, "Chilinh");
            }
        }
       
    }
}