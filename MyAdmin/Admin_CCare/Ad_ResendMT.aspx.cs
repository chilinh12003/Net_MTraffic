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
using MyMTraffic.Sub;
using System.Net;
using System.IO;
using MyMTraffic.Gateway;
using MyMTraffic.Permission;
namespace MyAdmin.Admin_CCare
{
    public partial class Ad_ResendMT : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;

        Subscriber mSub = new Subscriber();
        ems_send_queue mQueue = new ems_send_queue(MySetting.AdminSetting.MySQLConnection_Gateway);

        private void BindCombo(int type)
        {
            try
            {
                switch (type)
                {
                    case 1:
                        Service mService = new Service();
                        sel_Service.DataSource = mService.Select(4, string.Empty);
                        sel_Service.DataTextField = "ServiceName";
                        sel_Service.DataValueField = "ServiceID";
                        sel_Service.DataBind();
                        sel_Service.Items.Insert(0, new ListItem("--Dịch vụ--", "0"));
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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.ResendMT, Member.MemberGroupID());
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
                MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
                mMaster.str_PageTitle = mGetRole.PageName;

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    BindCombo(1);


                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        private bool GetServiceInfo(int ServiceID, ref int MTNumber, ref string ServiceName, ref string RegKeyword, ref string DeregKeyword)
        {
            try
            {
                Service mService = new Service();
                DataTable mTable = mService.Select(1, ServiceID.ToString());
                if (mTable == null || mTable.Rows.Count < 1)
                    return false;
                if (mTable.Rows[0]["MTNumber"] == DBNull.Value)
                {
                    MTNumber = 20;
                    return false;
                }
                MTNumber = (int)mTable.Rows[0]["MTNumber"];
                ServiceName = mTable.Rows[0]["ServiceName"].ToString();
                RegKeyword = mTable.Rows[0]["RegKeyword"].ToString();
                DeregKeyword = mTable.Rows[0]["DeregKeyword"].ToString();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_SendMT_Click(object sender, EventArgs e)
        {
            try
            {
                string MSISDN = tbx_MSISDN.Value;
                string MTContent = tbx_MT.Value.TrimEnd().TrimStart();
                int ServiceID = 0;
                int MTNumber = 20;
                string ServiceName = string.Empty;
                string RegKeyword = string.Empty;
                string DeregKeyword = string.Empty;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_MSISDN.Value = MSISDN;

                int.TryParse(sel_Service.Value, out ServiceID);
                if (sel_Service.SelectedIndex == 0 || ServiceID == 0)
                {
                    MyMessage.ShowError("Xin hãy chọn một dịch vụ bạn muốn gửi tin.");
                    return;
                }

                if (!GetServiceInfo(ServiceID, ref MTNumber, ref ServiceName, ref RegKeyword, ref DeregKeyword))
                {
                    MyMessage.ShowError("Dịch vụ không chính xác, xin vui lòng kiểm tra lại.");
                    return;
                }

                if (string.IsNullOrEmpty(MTContent))
                {
                    MyMessage.ShowError("Xin hãy nhập  nội dung MT cần gửi.");
                    return;
                }

                int PID = MyPID.GetPIDByPhoneNumber(MSISDN,MySetting.AdminSetting.MaxPID);

                DataTable mTable = mSub.Select(2, PID.ToString(), MSISDN, ServiceID.ToString());

                if (mTelco == null || mTable.Rows.Count < 1)
                {
                    MyMessage.ShowError("Số điện thoại chưa đăng ký dịch vụ này, nên không thể gửi tin nhắn.");
                    return;
                }
                int MTCount = MTContent.Length / 160;

                if (MTContent.Length % 160 > 0)
                    MTCount++;


                int TotalMTByDay_Update = MTCount;

                if (mTable.Rows[0]["LastUpdate"] != DBNull.Value)
                {
                    DateTime LastUpdate = (DateTime)mTable.Rows[0]["LastUpdate"];
                    if (DateTime.Now.Day == LastUpdate.Day &&
                        DateTime.Now.Month == LastUpdate.Month &&
                        DateTime.Now.Year == LastUpdate.Year)
                    {
                        if (mTable.Rows[0]["TotalMTByDay"] != DBNull.Value)
                        {
                            int TotalMTByDay = (int)mTable.Rows[0]["TotalMTByDay"];

                            if (TotalMTByDay + MTCount > MTNumber)
                            {
                                MyMessage.ShowError("Số lượng MT vượt quá số lượng MT cho phép trong 1 ngày.");
                                return;
                            }
                            TotalMTByDay_Update = TotalMTByDay + MTCount;
                        }
                    }
                }

                if (SendMT(RegKeyword, MSISDN, MTContent))
                {
                    mTable.Rows[0]["TotalMTByDay"] = TotalMTByDay_Update;
                    mTable.Rows[0]["LastUpdate"] = DateTime.Now;
                    UpdateToSub(mTable);

                    UpdateActionLog(ServiceID, MSISDN, ActionLog.ActionType.PushMT_Retry, string.Empty, MTContent);
                    MyMessage.ShowMessage("Gửi MT thành công.");
                }
                else
                {
                    MyMessage.ShowMessage("Gửi MT KHÔNG thành công.");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SeachError, "Chilinh");
            }
        }

        private void UpdateActionLog(int ServiceID, string MSISDN, ActionLog.ActionType mType, string LogContent, string MT)
        {
            try
            {

                ActionLog mALog = new ActionLog();

                DataSet mSet = mALog.CreateDataSet();
                DataRow mRow = mSet.Tables[0].NewRow();
                mRow["ServiceID"] = ServiceID.ToString();
                mRow["MSISDN"] = MSISDN;
                mRow["LogDate"] = DateTime.Now;
                mRow["ActionTypeID"] = ((int)mType).ToString();
                mRow["ActionTypeName"] = mType.ToString();
                mRow["LogContent"] = LogContent;
                mRow["MT"] = MT;
                mRow["LogPID"] = MyPID.GetPIDByDate(DateTime.Now);

                mSet.Tables[0].Rows.Add(mRow);
                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                mALog.Insert(0, mSet.GetXml());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateToSub(DataTable mTable_Sub)
        {
            try
            {
                DataSet mSet = new DataSet("Parent");
                DataTable mTable = mTable_Sub.Copy();
                mTable.TableName = "Child";
                mSet.Tables.Add(mTable);

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);
                if (mSub.Update(1, mSet.GetXml()))
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SendMT(string COMMAND_CODE, string USER_ID, string MTContent)
        {
            string SERVICE_ID = MySetting.AdminSetting.ShoreCode;
            string REQUEST_ID = MySecurity.CreateCode(9);
            bool Result = false;
            try
            {
                Result = mQueue.Insert(USER_ID, SERVICE_ID, COMMAND_CODE, MTContent, REQUEST_ID);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MyLogfile.WriteLogData("_Resend_MT", "UserID:" + Member.MemberID().ToString() + "|USER_ID:" + USER_ID + "|COMMAND_CODE:" + COMMAND_CODE + "|REQUEST_ID:" + REQUEST_ID + "|INFO:" + MTContent + "|Result:" + Result.ToString());
            }
        }

    }
}