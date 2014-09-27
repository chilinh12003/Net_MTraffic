using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    public partial class Ad_MenuAdmin : System.Web.UI.Page
    {
        public GetRole mGetRole;

        public int PageIndex = 1;

        MenuAdmin mMenuAdmin = new MenuAdmin();

        private bool CheckPermission()
        {
            try
            {
                if (mGetRole.ViewRole == false && Member.MemberGroupID() != Member.MemberGroupID_Admin())
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }
                if (Member.MemberGroupID() != Member.MemberGroupID_Admin())
                {
                    link_Add.Visible = mGetRole.AddRole;
                    link_Edit.Visible = mGetRole.EditRole;
                    lbtn_Active.Visible = mGetRole.ActiveRole;
                    lbtn_UnActive.Visible = mGetRole.InActiveRole;
                    lbtn_Delete.Visible = mGetRole.DeleteRole;
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        private void BindCombo(int type)
        {
            switch (type)
            {
                case 1: //Bind dữ liệu thể loại 1
                    ddl_MenuID_1.DataSource = mMenuAdmin.Select(0, null);
                    ddl_MenuID_1.DataValueField = "MenuID_1";
                    ddl_MenuID_1.DataTextField = "MenuName";
                    ddl_MenuID_1.DataBind();
                    ddl_MenuID_1.Items.Insert(0, new ListItem("--Menu cấp 1--", "0"));
                    ddl_MenuID_2.Items.Clear();
                    sel_MenuID_3.Items.Clear();
                    ddl_MenuID_2.Items.Insert(0, new ListItem("--Menu cấp 2--", "0"));
                    sel_MenuID_3.Items.Insert(0, new ListItem("--Menu cấp 3--", "0"));
                    break;
                case 2:
                    ddl_MenuID_2.DataSource = mMenuAdmin.Select(2, ddl_MenuID_1.SelectedValue);
                    ddl_MenuID_2.DataValueField = "MenuID_2";
                    ddl_MenuID_2.DataTextField = "MenuName";
                    ddl_MenuID_2.DataBind();
                    ddl_MenuID_2.Items.Insert(0, new ListItem("--Menu cấp 2--", "0"));
                    sel_MenuID_3.Items.Clear();
                    sel_MenuID_3.Items.Insert(0, new ListItem("--Menu cấp 3--", "0"));
                    break;
                case 3:
                    sel_MenuID_3.DataSource = mMenuAdmin.Select(3, ddl_MenuID_2.SelectedValue);
                    sel_MenuID_3.DataValueField = "MenuID_3";
                    sel_MenuID_3.DataTextField = "MenuName";
                    sel_MenuID_3.DataBind();
                    sel_MenuID_3.Items.Insert(0, new ListItem("--Menu cấp 3--", "0"));
                    break;
            }
        }

        private void BindData()
        {
            Admin_Paging1.ResetLoadData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Phân quyền
                if (ViewState["Role"] == null)
                {
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.MenuAdmin, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }
                if (!CheckPermission())
                    return;

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    BindCombo(1);
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
            string str_SearchContent = null;
            int? MenuID_1 = 0;
            int? MenuID_2 = 0;
            int? MenuID_3 = 0;
            bool? IsActive = null;

            if (btn_CancelSearch.Visible)
            {
                MenuID_1 = int.Parse(ddl_MenuID_1.SelectedValue);
                MenuID_2 = int.Parse(ddl_MenuID_2.SelectedValue);
                MenuID_3 = int.Parse(sel_MenuID_3.Value);

                str_SearchContent = tbx_Search.Value.Length < 1 ? null : tbx_Search.Value;
                if (rad_Active.Checked)
                    IsActive = true;
                if (rad_UnActive.Checked)
                    IsActive = false;              
            }
            return mMenuAdmin.TotalRow(str_SearchContent, MenuID_1, MenuID_2, MenuID_3, IsActive);
        }

        DataTable Admin_Paging1_GetData_Callback_Change()
        {
            string str_SearchContent = null;
            bool? IsActive = null;
            int? MenuID_1 = 0;
            int? MenuID_2 = 0;
            int? MenuID_3 = 0;

            string SortBy = ViewState["SortBy"].ToString();

            if (btn_CancelSearch.Visible)
            {
                MenuID_1 = int.Parse(ddl_MenuID_1.SelectedValue);
                MenuID_2 = int.Parse(ddl_MenuID_2.SelectedValue);
                MenuID_3 = int.Parse(sel_MenuID_3.Value);

                str_SearchContent = tbx_Search.Value.Length < 1 ? null : tbx_Search.Value;
                if (rad_Active.Checked)
                    IsActive = true;
                if (rad_UnActive.Checked)
                    IsActive = false;
             
            }

            PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

            return mMenuAdmin.Search(Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, str_SearchContent, MenuID_1, MenuID_2, MenuID_3, IsActive, SortBy);
        }

        protected void lbtn_Sort_Click(object sender, EventArgs e)
        {
            try
            {
                lbtn_Sort_1.CssClass = "Sort";
                lbtn_Sort_2.CssClass = "Sort";
                lbtn_Sort_3.CssClass = "Sort";
                lbtn_Sort_4.CssClass = "Sort";
                lbtn_Sort_5.CssClass = "Sort";
                lbtn_Sort_6.CssClass = "Sort";


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
                //Cho phép tạo lại menu khi dữ liệu menu trong db thay đổi
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;

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

                if (mMenuAdmin.Delete(0, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("MenuAdmin", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
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

                btn_CancelSearch.Visible = true;
                BindData();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        protected void btn_CancelSearch_Click(object sender, EventArgs e)
        {
            try
            {
                btn_CancelSearch.Visible = false;
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
                //Cho phép tạo lại menu khi dữ liệu menu trong db thay đổi
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;

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

                if (mMenuAdmin.Active(0, IsActive, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action =IsActive? MemberLog.ActionType.Active:MemberLog.ActionType.InActive;
                    mLog.Insert("MenuAdmin", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
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
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.DeActiveError, "Chilinh");
            }
        }

        protected void ddl_MenuID_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(2);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        protected void ddl_MenuID_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(3);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        protected void ddl_MenuID_1_DataBound(object sender, EventArgs e)
        {

        }
    }
}
