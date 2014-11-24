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
    public partial class Ad_Register : System.Web.UI.Page
    {
        public GetRole mGetRole;
        public int PageIndex = 1;

        Subscriber mSub = new Subscriber();
        Service mService = new Service();
       

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
                    mGetRole = new GetRole(MySetting.AdminSetting.ListPage.Register, Member.MemberGroupID());
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
                }

                 
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }
        protected void tbx_Dereg_Click(object sender, EventArgs e)
        {
            string MSISDN = string.Empty;
            string CommandCode = string.Empty;
            string RequestID = string.Empty;
            string ServiceName = string.Empty;
            string Result = string.Empty;
            string ErrorCode = string.Empty;
            string ErrorDesc = string.Empty;
            string Signature = string.Empty;
            try
            {
                Button mButton = (Button)sender;

                MSISDN = ViewState["MSISDN"] == null ? string.Empty : ViewState["MSISDN"].ToString();
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không đúng hoặc không thuộc mạng Vinaphone");
                    return;
                }
                DataTable mTable_Service = mService.Select(4, null);
                mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mButton.CommandArgument + "'";
                if (mTable_Service.DefaultView.Count < 1)
                {
                    MyMessage.ShowError("Dịch vụ không hợp lệ, xin vui lòng kiểm tra lại.");
                    return;
                }
                CommandCode = mTable_Service.DefaultView[0]["DeregKeyword"].ToString();
               
                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                Signature = MSISDN + "|CMS|" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "|" + mTable_Service.DefaultView[0]["PacketName"].ToString() + "|" + mTable_Service.DefaultView[0]["ServiceID"].ToString();
                Signature = MySecurity.AES.Encrypt(Signature, MySetting.AdminSetting.RegWSKey);
                System.Net.ServicePointManager.Expect100Continue = false;
                //Result = mClient.Dereg_CMD((int)MyConfig.ChannelType.CSKH, Signature, CommandCode);
                Result = mClient.DeReg_VNP(Member.LoginName(), MyCurrent.GetRequestIP, Signature, WS_MTraffic.ChannelType.CSKH);
                string[] Arr_Result = Result.Split('|');

                ErrorCode = Arr_Result[0];
                ErrorDesc = Arr_Result[1];

                if (ErrorCode.Equals("1"))
                {
                    MyMessage.ShowError("Thông tin Hủy đăng ký dịch vụ (" + ServiceName + ") của số điện thoại (" + MSISDN + ") đã được gửi đến hệ thống chờ xử lý.");
                }
                else
                {
                    MyMessage.ShowError("Xin lỗi, đăng ký/hủy đăng ký không thành công, xin vui lòng thử lại sau");
                }


                System.Threading.Thread.Sleep(3000);
                btn_Execute_Click(null, null);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
            finally
            {
                MyLogfile.WriteLogData("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
            }
        }
        protected void tbx_Reg_Click(object sender, EventArgs e)
        {
            string MSISDN = string.Empty;
            string CommandCode = string.Empty;
            string RequestID = string.Empty;
            string ServiceName = string.Empty;
            string Result = string.Empty;
            string ErrorCode = string.Empty;
            string ErrorDesc = string.Empty;
            string Signature = string.Empty;
            try
            {
                Button mButton = (Button)sender;

                MSISDN = ViewState["MSISDN"] == null ? string.Empty : ViewState["MSISDN"].ToString();
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không đúng hoặc không thuộc mạng Vinaphone");
                    return;
                }
                DataTable mTable_Service = mService.Select(4, null);
                mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mButton.CommandArgument + "'";
                if (mTable_Service.DefaultView.Count < 1)
                {
                    MyMessage.ShowError("Dịch vụ không hợp lệ, xin vui lòng kiểm tra lại.");
                    return;
                }
                CommandCode = mTable_Service.DefaultView[0]["RegKeyword"].ToString();
                RequestID = MySecurity.CreateCode(9);
                ServiceName = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                Result = string.Empty; 

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                Signature = MSISDN + "|CMS|" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "|" + mTable_Service.DefaultView[0]["PacketName"].ToString() + "|" + mTable_Service.DefaultView[0]["ServiceID"].ToString();
                Signature = MySecurity.AES.Encrypt(Signature, MySetting.AdminSetting.RegWSKey);
                System.Net.ServicePointManager.Expect100Continue = false;
                //Result = mClient.Reg_CMD((int)MyConfig.ChannelType.CSKH, Signature, CommandCode);
                Result = mClient.Reg_VNP(Member.LoginName(), MyCurrent.GetRequestIP, Signature, WS_MTraffic.ChannelType.CSKH);
                string[] Arr_Result = Result.Split('|');

                ErrorCode = Arr_Result[0];
                ErrorDesc = Arr_Result[1];

                if (ErrorCode.Equals("1"))
                {
                    MyMessage.ShowError("Thông tin Đăng ký dịch vụ (" + ServiceName + ") của số điện thoại (" + MSISDN + ") đã được gửi đến hệ thống chờ xử lý.");
                }
                else
                {
                    MyMessage.ShowError("Xin lỗi, Đăng ký không thành công, xin vui lòng thử lại sau");

                }

                System.Threading.Thread.Sleep(3000);
                btn_Execute_Click(null, null);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
            finally
            {
                MyLogfile.WriteLogData("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
            }
        }
        protected void btn_Execute_Click(object sender, EventArgs e)
        {
            string MSISDN = string.Empty;          
            int PID = 0;
            try
            {

                MSISDN = tbx_MSISDN.Value;
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không đúng hoặc không thuộc mạng Vinaphone");
                    return;
                }

                PID = MyPID.GetPIDByPhoneNumber(MSISDN,MySetting.AdminSetting.MaxPID);

                DataTable mTable_Service = mService.Select(4, null);

                DataTable mTable_Sub = mSub.Select(7,PID.ToString(),MSISDN);
                DataColumn mCol_1 = new DataColumn("ServiceName", typeof(string));
                DataColumn mCol_2 = new DataColumn("RegKeyword", typeof(string));
                DataColumn mCol_3 = new DataColumn("DeregKeyword", typeof(string));

                mTable_Sub.Columns.Add(mCol_1);
                mTable_Sub.Columns.Add(mCol_2);
                mTable_Sub.Columns.Add(mCol_3);

                DataTable mTable_NotSub = mTable_Service.Copy();
                DataColumn mCol_4 = new DataColumn("MSISDN", typeof(string));
                mCol_4.DefaultValue = MSISDN;
                mTable_NotSub.Columns.Add(mCol_4);


                foreach (DataRow mRow in mTable_Sub.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count < 1)
                    {
                        continue;
                    }
                    else
                    {
                        mTable_NotSub.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";
                        if (mTable_NotSub.DefaultView.Count > 0)
                            mTable_NotSub.DefaultView.Delete(0);
                    }

                    mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();

                    mRow["RegKeyword"] = mTable_Service.DefaultView[0]["RegKeyword"].ToString();

                    mRow["DeregKeyword"] = mTable_Service.DefaultView[0]["DeregKeyword"].ToString();
                }
                mTable_Service.DefaultView.RowFilter = string.Empty;
                mTable_NotSub.DefaultView.RowFilter = string.Empty;

                rpt_Data_Reg.DataSource = mTable_Sub;
                rpt_Data_Reg.DataBind();

                rpt_Data_NotReg.DataSource = mTable_NotSub;
                rpt_Data_NotReg.DataBind();

                ViewState["MSISDN"] = MSISDN;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
            finally
            {
                //MyLogfile.WriteLogData("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
            }
        }
    }
}