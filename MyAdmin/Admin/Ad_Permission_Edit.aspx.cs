using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    public partial class Ad_Permission_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        Role mRole = new Role();
        MenuAdmin mMenuAdmin = new MenuAdmin();
        string ParentPath = "../Admin/Ad_Permission.aspx";

        private void BindCombo(int type)
        {
            try
            {
                MemberGroup mGroup = new MemberGroup();
                switch (type)
                {
                    case 1://Bind dữ liệu về nhóm thành viên
                        ddl_MemberGroup.DataSource = mGroup.Select(2, string.Empty);
                        ddl_MemberGroup.DataTextField = "MemberGroupName";
                        ddl_MemberGroup.DataValueField = "MemberGroupID";
                        ddl_MemberGroup.DataBind();
                        break;
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BindData_MenuAdmin()
        {
            try
            {
                DataSet mSet = mMenuAdmin.Select_DataSet(9, ddl_MemberGroup.SelectedValue);
                if (mSet != null && mSet.Tables.Count > 0)
                {
                    DataColumn col_0 = new DataColumn("ParentOrder", typeof(int));
                    col_0.DefaultValue = 0;
                    DataColumn col_1 = new DataColumn("Order", typeof(int));
                    col_1.DefaultValue = 0;
                    DataColumn col_2 = new DataColumn("ChildCount", typeof(int));
                    col_2.DefaultValue = 0;
                    mSet.Tables[0].Columns.Add(col_0);
                    mSet.Tables[0].Columns.Add(col_1);
                    mSet.Tables[0].Columns.Add(col_2);

                    DataColumn col_3 = new DataColumn("ParentOrder", typeof(int));
                    col_3.DefaultValue = 0;
                    DataColumn col_4 = new DataColumn("Order", typeof(int));
                    col_4.DefaultValue = 0;
                    DataColumn col_5 = new DataColumn("ChildCount", typeof(int));
                    col_5.DefaultValue = 0;

                    mSet.Tables[1].Columns.Add(col_3);
                    mSet.Tables[1].Columns.Add(col_4);
                    mSet.Tables[1].Columns.Add(col_5);

                    mSet.AcceptChanges();
                    mSet.Relations.Add("Parent_Child", mSet.Tables[0].Columns["MenuID_1"], mSet.Tables[1].Columns["MenuID_1"]);
                }
                DataTable mTable = mSet.Tables[1].Clone();
                mTable.Clear();
                mTable.AcceptChanges();

                int i = 1;
                foreach (DataRow mRow in mSet.Tables[0].Rows)
                {
                    DataRow[] arr_Row = mRow.GetChildRows("Parent_Child");
                    if (arr_Row.Length < 1)
                        continue;

                    mRow["Order"] = i;
                    mRow["ChildCount"] = arr_Row.Length;
                    mTable.ImportRow(mRow);
                    int j = 1;
                    foreach (DataRow mRow_2 in arr_Row)
                    {
                        mRow_2["ParentOrder"] = i;
                        mRow_2["Order"] = j++;
                        mTable.ImportRow(mRow_2);
                    }

                    i++;
                }

                rpt_MenuAdmin.DataSource = mTable;
                rpt_MenuAdmin.DataBind();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
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
                    lbtn_Accept.Visible = lbtn_Save.Visible = mGetRole.EditRole;
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
                BindCombo(1);
                BindData_MenuAdmin();
            }
        }

        private void AddNewRow(ref DataSet mSet)
        {

            string[] Arr_PageCode = hid_ListPageCode.Value.Split('|');

            for (int i = 0; i < Arr_PageCode.Length; i++)
            {

                DataRow mNewRow = mSet.Tables["Child"].NewRow();
                mNewRow["MemberGroupID"] = int.Parse(ddl_MemberGroup.SelectedValue);
                mNewRow["PageCode"] = Arr_PageCode[i].Replace("`", "");
                mNewRow["AddRole"] = chk_AddRole.Checked;
                mNewRow["DeleteRole"] = chk_DeleteRole.Checked;
                mNewRow["EditRole"] = chk_EditRole.Checked;
                mNewRow["ViewRole"] = chk_ViewRole.Checked;
                mNewRow["PublishRole"] = chk_PublishRole.Checked;
                mNewRow["UnPublishRole"] = chk_UnPublishRole.Checked;
                mNewRow["ExportRole"] = chk_ExportRole.Checked;
                mNewRow["ImportRole"] = chk_ImportRole.Checked;
                mNewRow["ActiveRole"] = chk_ActiveRole.Checked;
                mNewRow["InActiveRole"] = chk_InActiveRole.Checked;
                mSet.Tables["Child"].Rows.Add(mNewRow);
            }
            mSet.AcceptChanges();
        }
        private void Save(bool IsApply)
        {
            try
            {
                //Cho phép tạo lại menu khi dữ liệu menu trong db thay đổi
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;

                if (hid_ListPageCode.Value.Length < 1)
                {
                    MyMessage.ShowError("Xin hãy chọn một trang cần phân quyền.");
                    return;
                }
                if (!chk_AddRole.Checked &&
                    !chk_DeleteRole.Checked &&
                    !chk_EditRole.Checked &&
                    !chk_ViewRole.Checked &&
                    !chk_ActiveRole.Checked &&
                    !chk_InActiveRole.Checked &&
                    !chk_PublishRole.Checked &&
                    !chk_UnPublishRole.Checked &&
                    !chk_ImportRole.Checked &&
                    !chk_ExportRole.Checked)
                {
                    MyMessage.ShowError("Xin hãy chọn ít nhất một Quyền cần cấp.");
                    return;
                }

                DataSet mSet = mRole.CreateDataSet();

                AddNewRow(ref mSet);

                if (mRole.Insert(0, mSet.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                    mLog.Insert("Role", string.Empty, mSet.GetXml(), Action, true, string.Empty);
                    #endregion

                    if (IsApply)
                        MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                    else
                    {
                        Response.Redirect(ParentPath, false);
                    }
                }
                else
                {
                    MyMessage.ShowMessage("Cập nhật dữ liệu (KHÔNG) thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void lbtn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Save(false);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

        protected void lbtn_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                Save(true);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }

        protected void ddl_MemberGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindData_MenuAdmin();
                hid_ListPageCode.Value = string.Empty;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

    }
}
