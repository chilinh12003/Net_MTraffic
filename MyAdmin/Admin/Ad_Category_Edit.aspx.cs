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
using MyMTraffic.News;
namespace MyAdmin.Admin
{
    public partial class Ad_Category_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;

        Category mCate = new Category();
        int EditID = 0;
        int CateID_1 = 0;
        int CateID_2 = 0;
        int CateID_3 = 0;
        int CateID_4 = 0;

        public string ParentPath = "../Admin/Ad_Category.aspx";
        private void BindCombo(int type)
        {
            switch (type)
            {
                case 1: //Bind dữ liệu thể loại 1
                    ddl_CateID_1.DataSource = mCate.Select(2, null);
                    ddl_CateID_1.DataValueField = "CateID_1";
                    ddl_CateID_1.DataTextField = "CateName";
                    ddl_CateID_1.DataBind();
                    ddl_CateID_1.Items.Insert(0, new ListItem("--Thể loại mức 1--", "0"));
                    ddl_CateID_2.Items.Insert(0, new ListItem("--Thể loại mức 2--", "0"));
                    sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
                    break;
                case 2:
                    ddl_CateID_2.DataSource = mCate.Select(3, ddl_CateID_1.SelectedValue);
                    ddl_CateID_2.DataValueField = "CateID_2";
                    ddl_CateID_2.DataTextField = "CateName";
                    ddl_CateID_2.DataBind();
                    ddl_CateID_2.Items.Insert(0, new ListItem("--Thể loại mức 2--", "0"));
                    sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
                    break;
                case 3:
                    sel_CateID_3.DataSource = mCate.Select(4, ddl_CateID_2.SelectedValue);
                    sel_CateID_3.DataValueField = "CateID_3";
                    sel_CateID_3.DataTextField = "CateName";
                    sel_CateID_3.DataBind();                   
                    sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
                    break;
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
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Category, Member.MemberGroupID());
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
                        DataTable mTable = mCate.Select(1, EditID.ToString());
                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            DataRow mRow = mTable.Rows[0];
                            tbx_CateName.Value = mRow["CateName"].ToString();
                            tbx_Description.Value = mRow["Description"].ToString();

                            CateID_1 = mRow["CateID_1"] != DBNull.Value ? (int)mRow["CateID_1"] : 0;
                            CateID_2 = mRow["CateID_2"] != DBNull.Value ? (int)mRow["CateID_2"] : 0;
                            CateID_3 = mRow["CateID_3"] != DBNull.Value ? (int)mRow["CateID_3"] : 0;
                            CateID_4 = mRow["CateID_4"] != DBNull.Value ? (int)mRow["CateID_4"] : 0;

                            ViewState["CateID_1"] = CateID_1;
                            ViewState["CateID_2"] = CateID_2;
                            ViewState["CateID_3"] = CateID_3;
                            ViewState["CateID_4"] = CateID_4;

                            tbx_UploadImage_1.Value = img_Upload_1.Src = mRow["ImagePath_1"].ToString();
                            tbx_UploadImage_2.Value = img_Upload_2.Src = mRow["ImagePath_2"].ToString();
                            tbx_UploadImage_3.Value = img_Upload_3.Src = mRow["ImagePath_3"].ToString();

                            tbx_PrefixCode.Value = mRow["PrefixCode"].ToString();

                            chk_Active.Checked = (bool)mRow["IsActive"];

                            if (mRow["CateID_4"] != DBNull.Value)
                            {
                                ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(mRow["CateID_1"].ToString()));
                                BindCombo(2);
                                ddl_CateID_2.SelectedIndex = ddl_CateID_2.Items.IndexOf(ddl_CateID_2.Items.FindByValue(mRow["CateID_2"].ToString()));
                                BindCombo(3);
                                sel_CateID_3.SelectedIndex = sel_CateID_3.Items.IndexOf(sel_CateID_3.Items.FindByValue(mRow["CateID_3"].ToString()));
                            }
                            else if (mRow["CateID_3"] != DBNull.Value)
                            {
                                ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(mRow["CateID_1"].ToString()));
                                BindCombo(2);
                                ddl_CateID_2.SelectedIndex = ddl_CateID_2.Items.IndexOf(ddl_CateID_2.Items.FindByValue(mRow["CateID_2"].ToString()));
                            }
                            else if (mRow["CateID_2"] != DBNull.Value)
                            {
                                ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(mRow["CateID_1"].ToString()));
                            }
                        }
                                               
                        #endregion
                    }
                  
                    #endregion
                }

                if (EditID > 0)
                {
                    CateID_1 = (int)ViewState["CateID_1"];
                    CateID_2 = (int)ViewState["CateID_2"];
                    CateID_3 = (int)ViewState["CateID_3"];
                    CateID_4 = (int)ViewState["CateID_4"];

                    ddl_CateID_1.Enabled = false;
                    ddl_CateID_2.Enabled = false;
                    sel_CateID_3.Disabled = true;
                }
                else
                {
                    CateID_1 = int.Parse(ddl_CateID_1.SelectedValue);
                    CateID_2 = int.Parse(ddl_CateID_2.SelectedValue);
                    CateID_3 = int.Parse(sel_CateID_3.Value);
                    CateID_4 = 0;

                    ddl_CateID_1.Enabled = true;
                    ddl_CateID_2.Enabled = true;
                    sel_CateID_3.Disabled = false;
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }

        }

        private bool UploadImage()
        {
            try
            {
                MyUploadImage mUpload = new MyUploadImage("Cate");

                bool IsSuccess = true;
                string Message = string.Empty;

                //Image 1
                #region MyRegion
                if (!string.IsNullOrEmpty(file_UploadImage_1.PostedFile.FileName))
                {
                    mUpload.mPostedFile = file_UploadImage_1.PostedFile;

                    if (mUpload.Upload())
                    {

                        img_Upload_1.Src = mUpload.UploadedPathFile;
                        tbx_UploadImage_1.Value = mUpload.UploadedPathFile;

                    }
                    else
                    {
                        Message += mUpload.Message;
                        IsSuccess = false;
                    }
                }
                #endregion

                //Image 2
                #region MyRegion
                if (!string.IsNullOrEmpty(file_UploadImage_2.PostedFile.FileName))
                {
                    mUpload.mPostedFile = file_UploadImage_2.PostedFile;

                    if (mUpload.Upload())
                    {

                        img_Upload_2.Src = mUpload.UploadedPathFile;
                        tbx_UploadImage_2.Value = mUpload.UploadedPathFile;

                    }
                    else
                    {
                        Message += mUpload.Message;
                        IsSuccess = false;
                    }
                }
                #endregion

                //Image 3
                #region MyRegion

                if (!string.IsNullOrEmpty(file_UploadImage_3.PostedFile.FileName))
                {
                    mUpload.mPostedFile = file_UploadImage_3.PostedFile;

                    if (mUpload.Upload())
                    {

                        img_Upload_3.Src = mUpload.UploadedPathFile;
                        tbx_UploadImage_3.Value = mUpload.UploadedPathFile;

                    }
                    else
                    {
                        Message += mUpload.Message;
                        IsSuccess = false;
                    }
                }
                #endregion

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
                UploadImage();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.UploadFileError, "Chilinh");
            }
        }
        private void AddNewRow(ref DataSet mSet)
        {
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["Child"].NewRow();

            if (EditID > 0)
                mNewRow["CateID"] = EditID;
            mNewRow["CateID_1"] = CateID_1;
            mNewRow["CateID_2"] = CateID_2;
            mNewRow["CateID_3"] = CateID_3;
            mNewRow["CateID_4"] = CateID_4;
            mNewRow["ImagePath_1"] = tbx_UploadImage_1.Value;
            mNewRow["ImagePath_2"] = tbx_UploadImage_2.Value;
            mNewRow["ImagePath_3"] = tbx_UploadImage_3.Value;
            mNewRow["CateName"] = tbx_CateName.Value;
            mNewRow["Description"] = tbx_Description.Value;
            mNewRow["IsActive"] = chk_Active.Checked;
            int Priority = tbx_Priority.Value.Length < 1 ? 0 : int.Parse(tbx_Priority.Value);
            mNewRow["Priority"] = Priority;

            mNewRow["IsHasChild"] = false;
            mNewRow["PrefixCode"] = tbx_PrefixCode.Value;
           

            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {               
                if (!UploadImage())
                    return;

                img_Upload_1.Src = tbx_UploadImage_1.Value;
                img_Upload_2.Src = tbx_UploadImage_2.Value;
                img_Upload_3.Src = tbx_UploadImage_3.Value;

                DataSet mSet = mCate.CreateDataSet();
                AddNewRow(ref mSet);

                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mCate.Update(0, mSet.GetXml()))
                    {
                         #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("Category", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
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
                else
                {
                    if (mCate.Insert(0, mSet.GetXml()))
                    {
                         #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("Category", string.Empty, mSet.GetXml(), Action, true, string.Empty);
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

        protected void ddl_CateID_1_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void ddl_CateID_2_SelectedIndexChanged(object sender, EventArgs e)
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
