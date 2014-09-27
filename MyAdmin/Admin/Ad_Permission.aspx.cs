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
using System.Text;
using System.Collections.Generic;
using MyMTraffic.Permission;
using MyMTraffic.News;

namespace Administrator.Admin
{
    public partial class Ad_Permission : System.Web.UI.Page
    {

        public int PageIndex = 1;
        public GetRole mGetRole;
        Role mRole = new Role();

        private void BindData()
        {
            BindCombo(2);
            Admin_Paging1.ResetLoadData();
        }

        private void BindCombo(int type)
        {
            try
            {
                MemberGroup mGroup = new MemberGroup();
                MenuAdmin mMenuAdmin = new MenuAdmin();
                switch (type)
                {
                    case 1://Bind dữ liệu về nhóm thành viên
                        sel_MemberGroup.DataSource = mGroup.Select(2,string.Empty);
                        sel_MemberGroup.DataTextField = "MemberGroupName";
                        sel_MemberGroup.DataValueField = "MemberGroupID";
                        sel_MemberGroup.DataBind();
                       
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
                if (mGetRole.ViewRole == false && Member.MemberGroupID() != 1)
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }
                if (Member.MemberGroupID() != 1)
                {
                    link_Add.Visible = mGetRole.AddRole;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Phân quyền
                if (ViewState["Role"] == null)
                {
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Permission, Member.MemberGroupID());
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
            try
            {
                string SearchContent = null;

                int? MemberGroupID = 0;

                string SortBy = ViewState["SortBy"].ToString();

                if (btn_CancelSearch.Visible)
                {
                    MemberGroupID = sel_MemberGroup.SelectedIndex > 0 ? (int?)int.Parse(sel_MemberGroup.Value) : null;
                    SearchContent = tbx_Search.Value.Length < 1 ? null : tbx_Search.Value;

                }
                return mRole.TotalRow(MemberGroupID, SearchContent, SortBy);
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
                string SearchContent = null;

                int? MemberGroupID = 0;

                string SortBy = ViewState["SortBy"].ToString();

                if (btn_CancelSearch.Visible)
                {
                    MemberGroupID = sel_MemberGroup.SelectedIndex > 0 ? (int?)int.Parse(sel_MemberGroup.Value) : null;
                    SearchContent = tbx_Search.Value.Length < 1 ? null : tbx_Search.Value;
                }

                PageIndex = (Admin_Paging1.mPaging.CurrentPageIndex - 1) * Admin_Paging1.mPaging.PageSize + 1;

                return mRole.Search(1, Admin_Paging1.mPaging.BeginRow, Admin_Paging1.mPaging.EndRow, MemberGroupID, SearchContent, SortBy);
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
                lbtn_Sort_5.CssClass = "Sort";
                lbtn_Sort_6.CssClass = "Sort";
                lbtn_Sort_7.CssClass = "Sort";
                lbtn_Sort_8.CssClass = "Sort";
                lbtn_Sort_9.CssClass = "Sort";

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

                if (mRole.Delete(0, dds_Parent.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("Role", string.Empty, dds_Parent.GetXml(), Action, true, string.Empty);
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
                 
    }
}
