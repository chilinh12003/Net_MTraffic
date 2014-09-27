using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyMTraffic;
using MyMTraffic.News;
using MyMTraffic.Service;
using MyMTraffic.Permission;
using MyMTraffic.News;
namespace MyAdmin.Admin_News
{
    public partial class Ad_News : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;

        News mNews = new News();

        private void BindCombo(int type)
        {
            try
            {
                Category mCate = new Category();

                switch (type)
                {
                    case 1:
                        ServiceGroup mServiceGroup = new ServiceGroup();
                        ddl_ServiceGroup.DataSource = mServiceGroup.Select(2, string.Empty);
                        ddl_ServiceGroup.DataTextField = "ServiceGroupName";
                        ddl_ServiceGroup.DataValueField = "ServiceGroupID";
                        ddl_ServiceGroup.DataBind();
                        ddl_ServiceGroup.Items.Insert(0, new ListItem("--Nhóm dịch vụ--", "0"));
                        break;
                     case 2:
                        Service mService = new Service();
                        
                        sel_Service.DataSource = mService.Select(3, ddl_ServiceGroup.SelectedItem.Value);
                        sel_Service.DataTextField = "ServiceName";
                        sel_Service.DataValueField = "ServiceID";
                        sel_Service.DataBind();
                        sel_Service.Items.Insert(0, new ListItem("--Dịch vụ--", "0"));
                        break;
                   
                    case 3:
                        sel_Status.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(News.Status),true);
                        sel_Status.DataTextField = "Text";
                        sel_Status.DataValueField = "ID";
                        sel_Status.DataBind();
                        sel_Status.Items.Insert(0, new ListItem("--Tình trạng--", "0"));
                        break;
                    case 4:
                        sel_NewsType.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(News.NewsType),true);
                        sel_NewsType.DataTextField = "Text";
                        sel_NewsType.DataValueField = "ID";
                        sel_NewsType.DataBind();
                        sel_NewsType.Items.Insert(0, new ListItem("--Loại tin--", "0"));
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

                link_Add.Visible = mGetRole.AddRole;
                link_Edit.Visible = mGetRole.EditRole;
                lbtn_Active.Visible = mGetRole.PublishRole;
                lbtn_UnActive.Visible = mGetRole.PublishRole;
                lbtn_Delete.Visible = mGetRole.DeleteRole;

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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.News, Member.MemberGroupID());
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
                    BindCombo(4);
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
                int? SearchType = null;
                string str_SearchContent = null;
                bool? IsActive = null;
                string SortBy = ViewState["SortBy"].ToString();
                int ServiceID = 0;
                int StatusID = 0;
                int NewsTypeID = 0;
                int StreetID = 0;
                SearchType = int.Parse(sel_SearchType.Value);

                str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);

                if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }

                if (sel_Status.SelectedIndex > 0)
                {
                    int.TryParse(sel_Status.Value, out StatusID);
                }
                if (sel_NewsType.SelectedIndex > 0)
                {
                    int.TryParse(sel_NewsType.Value, out NewsTypeID);
                }

                if (sel_Active.Value == "1")
                    IsActive = true;

                if (sel_Active.Value == "2")
                    IsActive = false;

                return mNews.TotalRow(SearchType, str_SearchContent, ServiceID, StreetID, (News.Status)StatusID, (News.NewsType)NewsTypeID, IsActive);
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
                int? SearchType = null;
                string str_SearchContent = null;
                bool? IsActive = null;
                string SortBy = ViewState["SortBy"].ToString();
                int ServiceID = 0;
                int StatusID = 0;
                int NewsTypeID = 0;
                int StreetID = 0;
                SearchType = int.Parse(sel_SearchType.Value);

                str_SearchContent = tbx_Search.Value.Length < 1 ? null : MyText.ValidSearchContent(tbx_Search.Value);

              if (sel_Service.SelectedIndex > 0)
                {
                    int.TryParse(sel_Service.Value, out ServiceID);
                }

                if (sel_Status.SelectedIndex > 0)
                {
                    int.TryParse(sel_Status.Value, out StatusID);
                }
                if (sel_NewsType.SelectedIndex > 0)
                {
                    int.TryParse(sel_NewsType.Value, out NewsTypeID);
                }
                if (sel_Active.Value == "1")
                    IsActive = true;

                if (sel_Active.Value == "2")
                    IsActive = false;

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mNews.Search(SearchType, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, str_SearchContent, ServiceID, StreetID, (News.Status)StatusID, (News.NewsType)NewsTypeID, IsActive, SortBy);
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
                lbtn_Sort_2.CssClass = "Sort";
                lbtn_Sort_3.CssClass = "Sort";
                lbtn_Sort_4.CssClass = "Sort";
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

        protected void lbtn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                char[] key_1 = { '|' };

                string[] arr_1 = hid_ListCheckAll.Value.Split(key_1);


                DataSet dds_Parent = new DataSet("Parent");
                DataTable tbl_Child = new DataTable("Child");
                DataColumn col_1 = new DataColumn("ID", typeof(int));
                tbl_Child.Columns.Add(col_1);

                for (int i = 0; i < arr_1.Length; i++)
                {
                    DataRow mRow = tbl_Child.NewRow();

                    mRow["ID"] = int.Parse(arr_1[i]);

                    tbl_Child.Rows.Add(mRow);
                }
                tbl_Child.AcceptChanges();

                dds_Parent.Tables.Add(tbl_Child);
                dds_Parent.AcceptChanges();

                if (mNews.Delete(0, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("News", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
                    #endregion

                    MyMessage.ShowMessage("Xóa dữ liệu thành công.");
                    BindData();
                }
                else
                {
                    MyMessage.ShowMessage("Xóa dữ liệu KHÔNG thành công!");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.DeleteDataError, "Chilinh");
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        private void Active(bool IsActive)
        {
            try
            {
                char[] key_1 = { '|' };

                string[] arr_1 = hid_ListCheckAll.Value.Split(key_1);


                DataSet dds_Parent = new DataSet("Parent");
                DataTable tbl_Child = new DataTable("Child");
                DataColumn col_1 = new DataColumn("ID", typeof(int));
                tbl_Child.Columns.Add(col_1);

                for (int i = 0; i < arr_1.Length; i++)
                {
                    DataRow mRow = tbl_Child.NewRow();

                    mRow["ID"] = int.Parse(arr_1[i]);

                    tbl_Child.Rows.Add(mRow);
                }
                tbl_Child.AcceptChanges();

                dds_Parent.Tables.Add(tbl_Child);
                dds_Parent.AcceptChanges();

                if (mNews.Active(0, IsActive, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = IsActive ? MemberLog.ActionType.Active : MemberLog.ActionType.InActive;
                    mLog.Insert("News", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
                    #endregion
                    MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                    BindData();
                }
                else
                {
                    MyMessage.ShowMessage("Cập nhật dữ liệu KHÔNG thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lbtn_Active_Click(object sender, EventArgs e)
        {

            try
            {
                Active(true);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.ActiveError, "Chilinh");
            }
        }

        protected void lbtn_UnActive_Click(object sender, EventArgs e)
        {

            try
            {
                Active(false);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.ActiveError, "Chilinh");
            }
        }

        protected void ddl_ServiceGroup_IndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
