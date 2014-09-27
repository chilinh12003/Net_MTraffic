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

namespace MyAdmin.Admin_Report
{
    public partial class Ad_MTHistory : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;
        ActionLog mActionLog = new ActionLog();

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
                    case 2:
                        sel_Day.DataSource = MyEnum.GetDataFromTime(2, "Ngày ", string.Empty);
                        sel_Day.DataTextField = "Text";
                        sel_Day.DataValueField = "ID";
                        sel_Day.DataBind();
                        sel_Day.Items.Insert(0, new ListItem("--Ngày--", "0"));
                        break;
                    case 3:
                        sel_Month.DataSource = MyEnum.GetDataFromTime(1, "Tháng ", string.Empty);
                        sel_Month.DataTextField = "Text";
                        sel_Month.DataValueField = "ID";
                        sel_Month.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindData()
        {
            Admin_Paging1.ResetLoadData();
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.ReportMTHisTory, Member.MemberGroupID());
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = mGetRole.PageName;

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    BindCombo(1);
                    BindCombo(2);
                    BindCombo(3);

                    sel_Month.SelectedIndex = sel_Month.Items.IndexOf(sel_Month.Items.FindByValue(DateTime.Now.Month.ToString()));
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
                int ServiceID = 0;
                string SearchContent = tbx_SearchContent.Value;

                int LogPID = int.Parse(sel_Month.Value) -1;               
                DateTime BeginDate   =  DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 0;
                }                

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }

                if (sel_Day.SelectedIndex > 0)
                {
                    BeginDate = new DateTime(DateTime.Now.Year, int.Parse(sel_Month.Value), int.Parse(sel_Day.Value));
                    EndDate = BeginDate.AddDays(1);
                }

                return mActionLog.TotalRow(SearchType, SearchContent, LogPID, ServiceID, BeginDate,EndDate);
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
                int ServiceID = 0;
                string SearchContent = tbx_SearchContent.Value;

                int LogPID = int.Parse(sel_Month.Value) - 1;
                DateTime BeginDate = DateTime.MinValue;
                DateTime EndDate = DateTime.MinValue;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return new DataTable();
                }

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }

                if (sel_Day.SelectedIndex > 0)
                {
                    BeginDate = new DateTime(DateTime.Now.Year, int.Parse(sel_Month.Value), int.Parse(sel_Day.Value));
                    EndDate = BeginDate.AddDays(1);
                }

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mActionLog.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, SearchContent, LogPID, ServiceID, BeginDate,EndDate, SortBy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtn_Sort_Click(object sender, EventArgs e)
        {
            try
            {
                lbtn_Sort_1.CssClass = "Sort";
                //lbtn_Sort_2.CssClass = "Sort";
                //lbtn_Sort_3.CssClass = "Sort";
                //lbtn_Sort_4.CssClass = "Sort";
                //lbtn_Sort_5.CssClass = "Sort";
                //lbtn_Sort_6.CssClass = "Sort";
                //lbtn_Sort_7.CssClass = "Sort";

                LinkButton mLinkButton = (LinkButton)sender;
                ViewState["SortBy"] = mLinkButton.CommandArgument;

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

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string MSISDN = tbx_SearchContent.Value;
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_SearchContent.Value = MSISDN;
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

    }
}