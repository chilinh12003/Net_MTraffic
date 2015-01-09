using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyMTraffic.Permission;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyBase.MyWeb;
namespace MyCCare.Admin_CCare
{
    public partial class Ad_ConfigService : MyASPXBase
    {
        public int PageIndex = 1;
        Subscriber mSub = new Subscriber();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                btn_Save.Enabled = Login1.IsAdmin();
                MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
                mMaster.Title = "GUI - Cài đặt dịch vụ";

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    tbx_MSISDN.Value = MySetting.AdminSetting.MSISDN;
                }

                if(string.IsNullOrEmpty( tbx_MSISDN.Value))
                {
                    ViewState["Data"] = null;
                }
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.LoadDataError, true, ex);
            }
        }
        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string MSISDN = tbx_MSISDN.Value;
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_MSISDN.Value = MSISDN;
                MySetting.AdminSetting.MSISDN = MSISDN;
                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, MySetting.AdminSetting.MaxPID);
                DataTable mTable = mSub.Select(2, PID.ToString(), MSISDN);
                if(mTable == null || mTable.Rows.Count < 1)
                {
                    ViewState["Data"] = null;
                    MyMessage.ShowError("Thuê bao chưa đăng ký dịch vụ này, xin vui lòng thử lại với thuê bao khác.");
                    return;
                }

                ViewState["Data"] = mTable;

                if(mTable.Rows[0]["IsNotify"] != DBNull.Value && ((bool)mTable.Rows[0]["IsNotify"]))
                {
                    sel_IsNotify.SelectedIndex = 0;
                }
                else
                {
                    sel_IsNotify.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SeachError, true, ex);
                
            }
        }

        public bool IsShow()
        {
            if (ViewState["Data"] != null && ((DataTable)ViewState["Data"]).Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
        protected void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if(!IsShow())
                {
                    btn_Search_Click(null,null);
                }

                DataTable mTable = (DataTable)ViewState["Data"];
                ViewState["OldData"] = MyXML.GetXML(mTable);

                if (mTable.Rows[0]["MSISDN"].ToString() != tbx_MSISDN.Value)
                {
                    btn_Search_Click(null, null);
                }
                mTable.Rows[0]["NotifyDate"] = DateTime.Now;
                if(sel_IsNotify.Items[sel_IsNotify.SelectedIndex].Value == "0")
                {
                    mTable.Rows[0]["IsNotify"] = true;
                }
                else
                {
                    mTable.Rows[0]["IsNotify"] = false;
                }

                DataSet mSet = new DataSet("Parent");
                mTable.TableName = "Child";
                mSet.Tables.Add(mTable.Copy());

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                if(mSub.Update(3, mSet.GetXml()))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Delete;
                    mLog.Insert("Subscriber", ViewState["OldData"].ToString(), mSet.GetXml(), Action, true, string.Empty,0,Login1.GetUserName());
                    #endregion

                    MyMessage.ShowError("Cập nhật thông tin thành công.");
                }
                else
                {
                    MyMessage.ShowError("Cập nhật thông tin Không thành công.");
                }

            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SeachError, true, ex);
            }
        }
    }
}