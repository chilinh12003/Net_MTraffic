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
using MyUtility.UploadFile;
using MyMTraffic.Permission;
using MyMTraffic.News;
namespace MyAdmin.Admin_Service
{
    public partial class Ad_Service_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        Service mService = new Service();

        int EditID = 0;

        public string ParentPath = "../Admin_Service/Ad_Service.aspx";

        private void BindCombo(int type)
        {
            try
            {
                Category mCate = new Category();

                switch (type)
                {

                    case 1:
                        ServiceGroup mGroup = new ServiceGroup();
                        sel_ServiceGroup.DataSource = mGroup.Select(2, string.Empty);
                        sel_ServiceGroup.DataTextField = "ServiceGroupName";
                        sel_ServiceGroup.DataValueField = "ServiceGroupID";
                        sel_ServiceGroup.DataBind();
                        sel_ServiceGroup.Items.Insert(0, new ListItem("--Nhóm--", "0"));
                        break;
                    case 2:
                        sel_ServiceType.DataSource = MyEnum.CrateDatasourceFromEnum(typeof(Service.ServiceType));
                        sel_ServiceType.DataValueField = "ID";
                        sel_ServiceType.DataTextField = "TEXT";
                        sel_ServiceType.DataBind();
                        break;
                    case 3:
                        Partner mPather = new Partner();
                        sel_Partner.DataSource = mPather.Select(2,string.Empty);
                        sel_Partner.DataTextField = "PartnerName";
                        sel_Partner.DataValueField = "PartnerID";
                        sel_Partner.DataBind();
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Service, Member.MemberGroupID());
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
                    BindCombo(3);

                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mService.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            #region MyRegion
                            DataRow mRow = mTable.Rows[0];                         

                            if (mRow["ServiceGroupID"] != DBNull.Value)
                                sel_ServiceGroup.SelectedIndex = sel_ServiceGroup.Items.IndexOf(sel_ServiceGroup.Items.FindByValue(mRow["ServiceGroupID"].ToString()));

                            if (mRow["PartnerID"] != DBNull.Value)
                                sel_Partner.SelectedIndex = sel_Partner.Items.IndexOf(sel_Partner.Items.FindByValue(mRow["PartnerID"].ToString()));

                            if (mRow["ServiceTypeID"] != DBNull.Value)
                                sel_ServiceType.SelectedIndex = sel_ServiceType.Items.IndexOf(sel_ServiceType.Items.FindByValue(mRow["ServiceTypeID"].ToString()));

                            tbx_ServiceName.Value = mRow["ServiceName"].ToString();
                            tbx_RegKeyword.Value = mRow["RegKeyword"].ToString();
                            tbx_DeregKeyword.Value = mRow["DeregKeyword"].ToString();

                            tbx_Description.Text = mRow["Description"].ToString();

                            tbx_UploadImage_1.Value = img_Upload_1.Src = mRow["ImagePath_1"].ToString();
                            tbx_UploadImage_2.Value = img_Upload_2.Src = mRow["ImagePath_2"].ToString();
                            tbx_Price.Value = mRow["Price"].ToString();
                            tbx_TableName.Value = mRow["TableName"].ToString();
                            tbx_PushTime.Value = mRow["PushTime"].ToString();
                            tbx_MTNumber.Value = mRow["MTNumber"].ToString();

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
        private void AddNewRow(ref DataSet mSet)
        {
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            DataRow mNewRow = mSet.Tables["Child"].NewRow();

            if (EditID > 0)
                mNewRow["ServiceID"] = EditID;

            mNewRow["ServiceName"] = tbx_ServiceName.Value;
            mNewRow["Description"] = tbx_Description.Text;           

            if (sel_Partner.SelectedIndex >= 0 && sel_Partner.Items.Count > 0)
            {
                mNewRow["PartnerID"] = int.Parse(sel_Partner.Value);
                mNewRow["PartnerName"] = sel_Partner.Items[sel_Partner.SelectedIndex].Text;
            }

            if (sel_ServiceGroup.SelectedIndex >= 0 && sel_ServiceGroup.Items.Count > 0)
            {
                mNewRow["ServiceGroupID"] = int.Parse(sel_ServiceGroup.Value);
                mNewRow["ServiceGroupName"] = sel_ServiceGroup.Items[sel_ServiceGroup.SelectedIndex].Text;
            }

            if (sel_ServiceType.SelectedIndex >= 0 && sel_ServiceType.Items.Count > 0)
            {
                mNewRow["ServiceTypeID"] = int.Parse(sel_ServiceType.Value);
                mNewRow["ServiceTypeName"] = sel_ServiceType.Items[sel_ServiceType.SelectedIndex].Text;
            }

            mNewRow["ServiceName"] = tbx_ServiceName.Value;
            mNewRow["RegKeyword"] = tbx_RegKeyword.Value;
            mNewRow["DeregKeyword"] = tbx_DeregKeyword.Value;

            mNewRow["Description"] = tbx_Description.Text;

            mNewRow["ImagePath_1"] = tbx_UploadImage_1.Value;
            mNewRow["ImagePath_2"] = tbx_UploadImage_2.Value;
            mNewRow["Price"] = tbx_Price.Value;
            mNewRow["TableName"] = tbx_TableName.Value;
            mNewRow["PushTime"] = tbx_PushTime.Value;
            mNewRow["MTNumber"] = tbx_MTNumber.Value;


            int Priority = 0;
            if (int.TryParse(tbx_Priority.Value, out Priority))
            {
                mNewRow["Priority"] = Priority;
            }
            mNewRow["IsActive"] = chk_Active.Checked;

            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
                if (!UploadImage())
                    return;

                DataSet mSet = mService.CreateDataSet();
                AddNewRow(ref mSet);
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mService.Update(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("Service", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
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
                    if (mService.Insert(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("Service", string.Empty, mSet.GetXml(), Action, true, string.Empty);
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
    }
}