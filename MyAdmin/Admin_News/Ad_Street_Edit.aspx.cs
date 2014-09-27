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
using MyMTraffic.Permission;

namespace MyAdmin.Admin_News
{
    public partial class Ad_Street_Edit : System.Web.UI.Page
    {
        public GetRole mGetRole;
        Street mStreet = new Street();

        int EditID = 0;

        public string ParentPath = "../Admin_News/Ad_Street.aspx";

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1://Bind Position
                        Position mPos = new Position();
                        sel_Position.DataSource = mPos.Select(3, string.Empty);
                        sel_Position.DataValueField = "PositionID";
                        sel_Position.DataTextField = "PositionName";
                        sel_Position.DataBind();
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Street, Member.MemberGroupID());
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

                    //Nếu là Edit
                    if (EditID > 0)
                    {
                        DataTable mTable = mStreet.Select(1, EditID.ToString());

                        //Lưu lại thông tin OldData để lưu vào MemberLog
                        ViewState["OldData"] = MyXML.GetXML(mTable);

                        if (mTable != null && mTable.Rows.Count > 0)
                        {
                            #region MyRegion
                            DataRow mRow = mTable.Rows[0];
                            tbx_StreetName.Value = mRow["StreetName"].ToString();

                            tbx_Priority.Value = mRow["Priority"].ToString();
                            chk_Active.Checked = (bool)mRow["IsActive"];

                            if (mRow["PositionID"] != DBNull.Value)
                                sel_Position.SelectedIndex = sel_Position.Items.IndexOf(sel_Position.Items.FindByValue(mRow["PositionID"].ToString()));

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
                mNewRow["StreetID"] = EditID;

            mNewRow["StreetName"] = tbx_StreetName.Value;

            mNewRow["StreetName_EN"] = MyText.RemoveSignVietnameseString(tbx_StreetName.Value);

            mNewRow["IsActive"] = chk_Active.Checked;

            int Priority = 0;
            if (int.TryParse(tbx_Priority.Value, out Priority))
            {
                mNewRow["Priority"] = Priority;
            }
            if (sel_Position.SelectedIndex >= 0 && sel_Position.Items.Count > 0)
            {
                mNewRow["PositionID"] = int.Parse(sel_Position.Value);
                mNewRow["PositionName"] = sel_Position.Items[sel_Position.SelectedIndex].Text;
            }
            mNewRow["ContentSearch"] = MyConvert.ConvertDataToSearchText(mNewRow, new string[] { "StreetName", "PositionID", "PositionName"});
            mSet.Tables["Child"].Rows.Add(mNewRow);
            mSet.AcceptChanges();
        }

        private void Save(bool IsApply)
        {
            try
            {
                DataSet mSet = mStreet.CreateDataSet();
                AddNewRow(ref mSet);
                //Nếu là Edit
                if (EditID > 0)
                {
                    if (mStreet.Update(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Update;
                        mLog.Insert("Street", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty);
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
                    if (mStreet.Insert(0, mSet.GetXml()))
                    {
                        #region Log member
                        MemberLog mLog = new MemberLog();
                        MemberLog.ActionType Action = MemberLog.ActionType.Insert;
                        mLog.Insert("Street", string.Empty, mSet.GetXml(), Action, true, string.Empty);
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
