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
using MyUtility.UploadFile;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    
    public partial class Ad_MenuAdmin_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;

        MenuAdmin mMenuAdmin = new MenuAdmin();
        int EditID = 0;
        int MenuID_1 = 0;
        int MenuID_2 = 0;
        int MenuID_3 = 0;
        int MenuID_4 = 0;

        public string ParentPath = "../Admin/Ad_MenuAdmin.aspx";

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1: //Bind dữ liệu thể loại 1
                        ddl_MenuID_1.DataSource = mMenuAdmin.Select(0, null);
                        ddl_MenuID_1.DataValueField = "MenuID_1";
                        ddl_MenuID_1.DataTextField = "MenuName";
                        ddl_MenuID_1.DataBind();
                        ddl_MenuID_1.Items.Insert(0, new ListItem("--Menu cấp 1--", "0"));
                        ddl_MenuID_2.Items.Insert(0, new ListItem("--Menu cấp 2--", "0"));
                        sel_MenuID_3.Items.Insert(0, new ListItem("--Menu cấp 3--", "0"));
                        break;
                    case 2:
                        ddl_MenuID_2.DataSource = mMenuAdmin.Select(2, ddl_MenuID_1.SelectedValue);
                        ddl_MenuID_2.DataValueField = "MenuID_2";
                        ddl_MenuID_2.DataTextField = "MenuName";
                        ddl_MenuID_2.DataBind();
                        ddl_MenuID_2.Items.Insert(0, new ListItem("--Menu cấp 2--", "0"));
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                    if (EditID > 0)
                    {
                        lbtn_Save.Visible = btn_UploadImage.Visible = lbtn_Accept.Visible = mGetRole.EditRole;
                        link_Add.Visible = mGetRole.AddRole;
                    }
                    else
                    {
                        lbtn_Save.Visible = lbtn_Accept.Visible = btn_UploadImage.Visible = link_Add.Visible = mGetRole.AddRole;
                    }
                    chk_Active.Disabled = !mGetRole.PublishRole;
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.MenuAdmin, Member.MemberGroupID());
                }
                else
                {
                    mGetRole = (GetRole)ViewState["Role"];
                }

                //Lấy memberID nếu là trước hợp Sửa
                EditID = Request.QueryString["ID"] == null ? 0 : int.Parse(Request.QueryString["ID"]);

                if (!CheckPermission())
                    return;

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_TitleSearchBox = "Thông tin thể loại media:";

               

                if (!IsPostBack)
                {
                    #region MyRegion
                    BindCombo(1);
                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        #region MyRegion
                        DataTable mTable = mMenuAdmin.Select(1, EditID.ToString());
                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            DataRow mRow = mTable.Rows[0];
                            tbx_CateName.Value = mRow["MenuName"].ToString();

                            MenuID_1 = mRow["MenuID_1"] != DBNull.Value ? (int)mRow["MenuID_1"] : 0;
                            MenuID_2 = mRow["MenuID_2"] != DBNull.Value ? (int)mRow["MenuID_2"] : 0;
                            MenuID_3 = mRow["MenuID_3"] != DBNull.Value ? (int)mRow["MenuID_3"] : 0;
                            MenuID_4 = mRow["MenuID_4"] != DBNull.Value ? (int)mRow["MenuID_4"] : 0;

                            ViewState["MenuID_1"] = MenuID_1;
                            ViewState["MenuID_2"] = MenuID_2;
                            ViewState["MenuID_3"] = MenuID_3;
                            ViewState["MenuID_4"] = MenuID_4;

                            tbx_UploadImage.Value = img_Upload.Src = mRow["ImagePath"].ToString();

                            tbx_Link.Value  = mRow["Link"].ToString();
                            tbx_PageCode.Value = mRow["PageCode"].ToString();
                       
                            chk_Active.Checked = (bool)mRow["IsActive"];

                            if (mRow["MenuID_4"] != DBNull.Value)
                            {
                                ddl_MenuID_1.SelectedIndex = ddl_MenuID_1.Items.IndexOf(ddl_MenuID_1.Items.FindByValue(mRow["MenuID_1"].ToString()));
                                BindCombo(2);
                                ddl_MenuID_2.SelectedIndex = ddl_MenuID_2.Items.IndexOf(ddl_MenuID_2.Items.FindByValue(mRow["MenuID_2"].ToString()));
                                BindCombo(3);
                                sel_MenuID_3.SelectedIndex = sel_MenuID_3.Items.IndexOf(sel_MenuID_3.Items.FindByValue(mRow["MenuID_3"].ToString()));
                            }
                            else if (mRow["MenuID_3"] != DBNull.Value)
                            {
                                ddl_MenuID_1.SelectedIndex = ddl_MenuID_1.Items.IndexOf(ddl_MenuID_1.Items.FindByValue(mRow["MenuID_1"].ToString()));
                                BindCombo(2);
                                ddl_MenuID_2.SelectedIndex = ddl_MenuID_2.Items.IndexOf(ddl_MenuID_2.Items.FindByValue(mRow["MenuID_2"].ToString()));
                            }
                            else if (mRow["MenuID_2"] != DBNull.Value)
                            {
                                ddl_MenuID_1.SelectedIndex = ddl_MenuID_1.Items.IndexOf(ddl_MenuID_1.Items.FindByValue(mRow["MenuID_1"].ToString()));
                            }
                        }
                                               
                        #endregion
                    }
                  
                    #endregion
                }

                if (EditID > 0)
                {
                    MenuID_1 = (int)ViewState["MenuID_1"];
                    MenuID_2 = (int)ViewState["MenuID_2"];
                    MenuID_3 = (int)ViewState["MenuID_3"];
                    MenuID_4 = (int)ViewState["MenuID_4"];

                    ddl_MenuID_1.Enabled = false;
                    ddl_MenuID_2.Enabled = false;
                    sel_MenuID_3.Disabled = true;
                }
                else
                {
                    MenuID_1 = int.Parse(ddl_MenuID_1.SelectedValue);
                    MenuID_2 = int.Parse(ddl_MenuID_2.SelectedValue);
                    MenuID_3 = int.Parse(sel_MenuID_3.Value);
                    MenuID_4 = 0;

                    ddl_MenuID_1.Enabled = true;
                    ddl_MenuID_2.Enabled = true;
                    sel_MenuID_3.Disabled = false;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }

        }
        private bool UploadFile()
        {
            try
            {
                MyUploadImage mUpload = new MyUploadImage("Menu_");
                bool IsSuccess = true;
                string Message = string.Empty;

                if (!string.IsNullOrEmpty(file_UploadImage.PostedFile.FileName))
                {
                    mUpload.mPostedFile = file_UploadImage.PostedFile;

                    if (mUpload.Upload())
                    {
                        img_Upload.Src = mUpload.UploadedPathFile;
                        tbx_UploadImage.Value = mUpload.UploadedPathFile;

                    }
                    else
                    {
                        Message += mUpload.Message;
                        IsSuccess = false;
                    }
                }

                if (!IsSuccess)
                {
                    MyMessage.ShowError(Message);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        protected void btn_UploadImage_Click(object sender, EventArgs e)
        {
            try
            {
                UploadFile();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.UploadFileError, "Chilinh");
            }
        }

        private void Save(bool IsApply)
        {
            try
            {
                //Cho phép tạo lại menu khi dữ liệu menu trong db thay đổi
                MyCurrent.CurrentPage.Session["Menu_1"] = null;
                MyCurrent.CurrentPage.Session["Menu_2"] = null;

                if (!UploadFile())
                    return;
              
                int Priority = tbx_Priority.Value.Length < 1 ? 0 : int.Parse(tbx_Priority.Value);

                img_Upload.Src = tbx_UploadImage.Value;
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mMenuAdmin.Update(0, EditID, MenuID_1, MenuID_2, MenuID_3, MenuID_4, tbx_CateName.Value,
                                        tbx_UploadImage.Value, Priority, chk_Active.Checked,
                                        tbx_PageCode.Value,
                                        tbx_Link.Value))
                    {
                        if (IsApply)
                        {
                            #region Log member
                            MemberLog mLog = new MemberLog();
                            MemberLog.ActionType Action = MemberLog.ActionType.Update;
                            mLog.Insert("MenuAdmin", string.Empty, string.Empty, Action, true, string.Empty);
                            #endregion
                            MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                        }
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
                else
                {
                    if (mMenuAdmin.Insert(0, MenuID_1, MenuID_2, MenuID_3,MenuID_4, tbx_CateName.Value,
                                        tbx_UploadImage.Value, Priority, chk_Active.Checked,
                                        tbx_PageCode.Value,
                                        tbx_Link.Value))
                    {
                        if (IsApply)
                        {
                            #region Log member
                            MemberLog mLog = new MemberLog();
                            MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                            mLog.Insert("MenuAdmin", string.Empty, string.Empty, Action, true, string.Empty);
                            #endregion
                            MyMessage.ShowMessage("Cập nhật dữ liệu thành công.");
                        }
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
    }
}
