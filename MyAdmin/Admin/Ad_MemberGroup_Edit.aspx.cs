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
    public partial class Ad_MemberGroup_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        MemberGroup mMemberGroup = new MemberGroup();

        int EditID = 0;

        public string ParentPath = "../Admin/Ad_MemberGroup.aspx";

        private void BindCombo()
        {
            try
            {
                Partner mPartner = new Partner();
                sel_Parnter.DataSource = mPartner.Select(2, string.Empty);
                sel_Parnter.DataTextField = "PartnerName";
                sel_Parnter.DataValueField = "PartnerID";
                sel_Parnter.DataBind();
                sel_Parnter.Items.Insert(0, new ListItem("--Không chọn--", "0"));
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.MemberGroup, Member.MemberGroupID());
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
                mMaster.str_TitleSearchBox = "Thông tin tài khoản:";

                if (!IsPostBack)
                {
                    BindCombo();
                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mMemberGroup.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            DataRow mRow = mTable.Rows[0];
                            tbx_MemberGroupID.Value = mRow["MemberGroupID"].ToString();
                            tbx_MemberGroupName.Value = mRow["MemberGroupName"].ToString();
                            sel_Parnter.SelectedIndex = sel_Parnter.Items.IndexOf(sel_Parnter.Items.FindByValue(mRow["PartnerID"].ToString()));
                        }
                    }
                }
                if (EditID > 0)
                {
                    tbx_MemberGroupID.Disabled = true;
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
           

            mNewRow["MemberGroupID"] = tbx_MemberGroupID.Value;
            mNewRow["MemberGroupName"] = tbx_MemberGroupName.Value;
            if (sel_Parnter.SelectedIndex >= 0 && sel_Parnter.Items.Count > 0)
            {
                mNewRow["PartnerID"] = int.Parse(sel_Parnter.Value);
                mNewRow["PartnerName"] = int.Parse(sel_Parnter.Value);
            }

            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
                DataSet mSet = mMemberGroup.CreateDataSet();
                AddNewRow(ref mSet);

                if (EditID > 0)
                {
                    if (mMemberGroup.Update(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("MemberGroup", ViewState["OldData"].ToString(),mSet.GetXml(), Action, true,string.Empty);

                        #endregion
                        //Update
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
                    if (mMemberGroup.Insert(0,mSet.GetXml()))
                    {
                       #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("MemberGroup", string.Empty,mSet.GetXml(), Action, true,string.Empty);

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
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "AnhTuan");
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
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "AnhTuan");
            }
        }
    }
}
