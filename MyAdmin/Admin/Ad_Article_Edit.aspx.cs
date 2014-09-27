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
    public partial class Ad_Article_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        Article mArticle = new Article();

        int EditID = 0;

        public string ParentPath = "../Admin/Ad_Article.aspx";

        private void BindCombo(int type)
        {
            try
            {
                Category mCate = new Category();

                switch (type)
                {
                    case 1: //Thể loại mức 1
                        ddl_CateID_1.DataSource = mCate.Select(2, null);
                        ddl_CateID_1.DataTextField = "CateName";
                        ddl_CateID_1.DataValueField = "CateID_1";
                        ddl_CateID_1.DataBind();
                        ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(Article.Article_CateID_1.ToString()));
                        ddl_CateID_1.Enabled = false;
                        break;
                    case 2: //Thể loại mức 2
                        ddl_CateID_2.DataSource = mCate.Select(3, ddl_CateID_1.SelectedValue);
                        ddl_CateID_2.DataTextField = "CateName";
                        ddl_CateID_2.DataValueField = "CateID_2";
                        ddl_CateID_2.DataBind();
                        ddl_CateID_2.Items.Insert(0, new ListItem("--Thể loại mức 2--", "0"));
                        break;
                    case 3: //Thể loại mức 3
                        sel_CateID_3.DataSource = mCate.Select(4, ddl_CateID_2.SelectedValue);
                        sel_CateID_3.DataTextField = "CateName";
                        sel_CateID_3.DataValueField = "CateID_3";
                        sel_CateID_3.DataBind();
                        sel_CateID_3.Items.Insert(0, new ListItem("--Thể loại mức 3--", "0"));
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
                if (EditID > 0)
                {
                    lbtn_Save.Visible = lbtn_Accept.Visible = mGetRole.EditRole;
                    link_Add.Visible = mGetRole.AddRole;
                }
                else
                {
                    lbtn_Save.Visible = lbtn_Accept.Visible = link_Add.Visible = mGetRole.AddRole;
                }
                chk_Active.Disabled = !mGetRole.PublishRole;

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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Article, Member.MemberGroupID());
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
                

                //Lấy memberID nếu là trước hợp Sửa
                EditID = Request.QueryString["ID"] == null ? 0 : int.Parse(Request.QueryString["ID"]);

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = mGetRole.PageName;
                mMaster.str_TitleSearchBox = "Thông tin về " + mGetRole.PageName;

                if (!IsPostBack)
                {
                    BindCombo(1);
                    BindCombo(2);

                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mArticle.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            #region MyRegion
                            DataRow mRow = mTable.Rows[0];

                            ddl_CateID_1.SelectedIndex = ddl_CateID_1.Items.IndexOf(ddl_CateID_1.Items.FindByValue(mRow["CateID_1"].ToString()));

                            if (mRow["CateID_2"] != null)
                            {
                                BindCombo(2);
                                ddl_CateID_2.SelectedIndex = ddl_CateID_2.Items.IndexOf(ddl_CateID_2.Items.FindByValue(mRow["CateID_2"].ToString()));
                                if (mRow["CateID_3"] != null)
                                {
                                    BindCombo(3);
                                    sel_CateID_3.SelectedIndex = sel_CateID_3.Items.IndexOf(sel_CateID_3.Items.FindByValue(mRow["CateID_3"].ToString()));
                                }
                            }
                         
                            tbx_UploadImage_1.Value = img_Upload_1.Src = mRow["ImagePath_1"].ToString();
                            tbx_UploadImage_2.Value = img_Upload_2.Src = mRow["ImagePath_2"].ToString();

                            tbx_Description.Value = mRow["Description"].ToString();
                            tbx_Content.Text = mRow["Content"].ToString();
                            tbx_ArticleName.Value = mRow["ArticleName"].ToString();

                            tbx_Priority.Value = mRow["Priority"].ToString();
                            chk_Active.Checked = (bool)mRow["IsActive"];

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }

        }
        protected void ddl_CateID_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindCombo(2);
                BindCombo(3);
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


        private bool UploadFile()
        {
            try
            {
                MyUploadImage mUpload = new MyUploadImage("Article_");
                bool IsSuccess = true;
                string Message = string.Empty;

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

        private void AddNewRow(ref DataSet mSet)
        {
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["Child"].NewRow();

            if (EditID > 0)
                mNewRow["ArticleID"] = EditID;
          
            //Lấy CateID,CateName dựa vào CateID_1,2,3,4
            Category mCate = new Category();
            DataTable tblCate = mCate.Select(10, ddl_CateID_1.SelectedValue, ddl_CateID_2.SelectedValue, sel_CateID_3.Value, string.Empty);
            if (tblCate != null && tblCate.Rows.Count > 0)
            {
                mNewRow["CateID"] = tblCate.Rows[0]["CateID"];
                mNewRow["CateName"] = tblCate.Rows[0]["CateName"];
            }

            mNewRow["ArticleName"] = tbx_ArticleName.Value;
            mNewRow["ImagePath_1"] = tbx_UploadImage_1.Value;
            mNewRow["ImagePath_2"] = tbx_UploadImage_2.Value;
            mNewRow["Description"] = tbx_Description.Value;
            mNewRow["Content"] = tbx_Content.Text.Replace("src=\"" + MyConfig.GetKeyInConfigFile("cuteEditor_UploadNews"), "src=\"" + MyConfig.GetKeyInConfigFile("EditorUploadResource") + MyConfig.GetKeyInConfigFile("cuteEditor_UploadNews"));
            mNewRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);

            mNewRow["IsActive"] = chk_Active.Checked;
            int Priority = 0;
            if (int.TryParse(tbx_Priority.Value, out Priority))
            {
                mNewRow["Priority"] = Priority;
            }          
            
            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
                if (!UploadFile())
                    return;

                DataSet mSet = mArticle.CreateDataSet();
                AddNewRow(ref mSet);
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mArticle.Update(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("Article", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
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
                    if (mArticle.Insert(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("Article", string.Empty, mSet.GetXml(), Action, true, string.Empty);
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
    }
}


