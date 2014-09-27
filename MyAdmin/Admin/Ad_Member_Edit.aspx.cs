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
namespace Administrator.Admin
{
    public partial class Ad_Member_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        Member mMember = new Member();

        int EditID = 0;

        public string ParentPath = "../Admin/Ad_Member.aspx";

        private void BindCombo()
        {
            MemberGroup mGroup = new MemberGroup();
            DataTable mTable = mGroup.Select(2, string.Empty);
            sel_MemberGroup.DataSource = mTable;
            sel_MemberGroup.DataTextField = "MemberGroupName";
            sel_MemberGroup.DataValueField = "MemberGroupID";
            sel_MemberGroup.DataBind();
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
                    lbtn_Save.Visible = btn_UploadImage.Visible  = lbtn_Accept.Visible = mGetRole.EditRole;
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Member, Member.MemberGroupID());
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

                if (!CheckPermission())
                    return;

                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_TitleSearchBox = "Thông tin tài khoản:";

                if (!IsPostBack)
                {
                    BindCombo();
                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mMember.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                           #region MyRegion
		                    DataRow mRow = mTable.Rows[0];
                            tbx_Address.Value = mRow["Address"].ToString();
                            tbx_Email.Value = mRow["Email"].ToString();
                            tbx_LoginName.Value = mRow["LoginName"].ToString();
                            tbx_MemberName.Value = mRow["MemberName"].ToString();
                            tbx_Phone.Value = mRow["Phone"].ToString();
                            img_Upload.Src = mRow["ImagePath"].ToString();
                            chk_Active.Checked = (bool)mRow["IsActive"];

                            if ((bool)mRow["Sex"])
                                rad_Male.Checked = true;
                            else
                                rad_Female.Checked = true;

                            tbx_LoginName.Disabled = true;

                            //Nếu Member chỉnh sửa tài khoản của người khác thì ko được chỉnh sửa
                            //LoginName và Pass
                            if (Member.LoginName().ToLower() != mRow["LoginName"].ToString().ToLower() && Member.MemberGroupID() != Member.MemberGroupID_Admin())
                            {
                                tbx_RepeatPass.Disabled = true;
                                tbx_Pass.Disabled = true;                                
                            }
                            ViewState["Pass"] = mRow["Password"].ToString();

                            sel_MemberGroup.SelectedIndex = sel_MemberGroup.Items.IndexOf(sel_MemberGroup.Items.FindByValue(mRow["MemberGroupID"].ToString()));
 
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

        private bool UploadFile()
        {
            try
            {
                MyUploadImage mUpload = new MyUploadImage("Member_");
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

        private void AddNewRow(ref DataSet mSet)
        {
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["Child"].NewRow();

            if (EditID > 0)
                mNewRow["MemberID"] = EditID;

            mNewRow["MemberName"] = tbx_MemberName.Value;
            mNewRow["LoginName"] = tbx_LoginName.Value;
            mNewRow["Password"] = MySecurity.Encrypt_MD5(tbx_Pass.Value);
            mNewRow["Sex"] = rad_Female.Checked?false:true;
            mNewRow["Birthday"] = tbx_Birthday.Value.Length > 0 ? DateTime.ParseExact(tbx_Birthday.Value, MyConfig.ShortDateFormat, null).ToString(MyConfig.DateFormat_InsertToDB) :null;
            mNewRow["Email"] = tbx_Email.Value;
            mNewRow["Phone"] = tbx_Phone.Value;
            mNewRow["Address"] = tbx_Address.Value;
            mNewRow["ImagePath"] = tbx_UploadImage.Value;
            mNewRow["MemberGroupID"] = int.Parse(sel_MemberGroup.Value);
            mNewRow["LastLoginDate"] =null;
            mNewRow["CreateDate"] = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
            mNewRow["IsActive"] = chk_Active.Checked;
            mNewRow["UpdateDate"] = null;

            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }
      
        private void Save(bool IsApply)
        {
            try
            {
               #region MyRegion
		        if (tbx_LoginName.Value.Trim().Length < 1)
                {
                    MyMessage.ShowError("Xin hãy nhập tên đăng nhập!");
                    tbx_LoginName.Focus();
                    return;
                }
                if (tbx_Pass.Value.Trim().Length < 1)
                {
                    MyMessage.ShowError("Xin hãy nhập mật khẩu!");
                    tbx_Pass.Focus();
                    return;
                }

                if (tbx_RepeatPass.Value.Trim().Length < 1)
                {
                    MyMessage.ShowError("Xin hãy nhập Nhắc lại mật khẩu!");
                    tbx_RepeatPass.Focus();
                    return;
                }
                if (tbx_RepeatPass.Value != tbx_Pass.Value)
                {
                    MyMessage.ShowError("Mật khẩu và Nhắc lại mật khẩu là không giống nhau, xin hãy xem lại!");
                    tbx_RepeatPass.Focus();
                    return;
                } 
	            #endregion

                if (!UploadFile())
                    return;

                DataSet mSet = mMember.CreateDataSet();
                AddNewRow(ref mSet);
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mMember.Update(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("Member", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
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
                    if (mMember.Insert(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("Member", string.Empty, mSet.GetXml(), Action, true, string.Empty);
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
