using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;using MyBase.MyWeb;
using MyMTraffic;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyUtility.UploadFile;

namespace MyCCare.Admin_CCare
{
    public partial class Ad_RegDereg : MyASPXBase
    {
        public int PageIndex = 1;
        Subscriber mSub = new Subscriber();
        UnSubscriber mUnSub = new UnSubscriber();
        Service mService = new Service();

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
                        sel_Service.Items.Insert(0, new ListItem("--Chọn gói cước--", "0"));
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
                mMaster.Title = "GUI - Cài đặt dịch vụ";

                if (!IsPostBack)
                {
                    BindCombo(1);
                    ViewState["SortBy"] = string.Empty;
                    tbx_MSISDN.Value = MySetting.AdminSetting.MSISDN;

                    if(!string.IsNullOrEmpty(tbx_MSISDN.Value))
                    {
                        btn_Search_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.LoadDataError, true, ex);
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
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

                tbx_MSISDN.Value = MSISDN;
                MySetting.AdminSetting.MSISDN = MSISDN;
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

                rpt_Data_Sub.DataSource = mTable_Sub;
                rpt_Data_Sub.DataBind();

                rpt_Data_UnSub.DataSource = mTable_NotSub;
                rpt_Data_UnSub.DataBind();

                ViewState["MSISDN"] = MSISDN;
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.LoadDataError, true, ex);
            }
            finally
            {
                //mLog.Debug("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
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
                LinkButton mButton = (LinkButton)sender;

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
                ServiceName = mTable_Service.DefaultView[0]["ServiceName"].ToString();

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                Signature = MSISDN + "|CMS|" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "|" + mTable_Service.DefaultView[0]["PacketName"].ToString() + "|" + mTable_Service.DefaultView[0]["ServiceID"].ToString();
                Signature = MySecurity.AES.Encrypt(Signature, MySetting.AdminSetting.RegWSKey);
                System.Net.ServicePointManager.Expect100Continue = false;
                Result = mClient.DeReg_VNP(Login1.GetUserName(), MyCurrent.GetRequestIP, Signature, WS_MTraffic.ChannelType.CSKH);
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
                btn_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SaveDataError, true, ex);
                
            }
            finally
            {
                mLog.Debug("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
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
                LinkButton mButton = (LinkButton)sender;

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
                Result = mClient.Reg_VNP(Login1.GetUserName(),MyCurrent.GetRequestIP,Signature,WS_MTraffic.ChannelType.CSKH);
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
                btn_Search_Click(null, null);
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SaveDataError, true, ex);
            }
            finally
            {
                mLog.Debug("Register", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|RequestID:" + RequestID + "|Result:" + Result);
            }
        }

        protected void rpt_Data_UnSub_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
             if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
             {
                 var lbtn_Reg = e.Item.FindControl("lbtn_Reg");
                 if(lbtn_Reg != null)
                 {
                     ((LinkButton)lbtn_Reg).Enabled = Login1.IsAdmin();
                     if (!Login1.IsAdmin())
                         ((LinkButton)lbtn_Reg).OnClientClick = string.Empty;
                 }
             }
        }

        protected void rpt_Data_Sub_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var lbtn_Dereg = e.Item.FindControl("lbtn_Dereg");
                if (lbtn_Dereg != null)
                {
                    ((LinkButton)lbtn_Dereg).Enabled = Login1.IsAdmin();
                    if (!Login1.IsAdmin())
                        ((LinkButton)lbtn_Dereg).OnClientClick = string.Empty;
                }
                var lbtn_Reset = e.Item.FindControl("lbtn_Reset");
                if (lbtn_Reset != null)
                {
                    ((LinkButton)lbtn_Reset).Enabled = Login1.IsAdmin();
                    if (!Login1.IsAdmin())
                        ((LinkButton)lbtn_Reset).OnClientClick = string.Empty;
                }
            }
        }


        protected void lbtn_DeregFile_Click(object sender, EventArgs e)
        {
            try
            {
                string FilePathSave = string.Empty;
                MyUploadFile mUpload = new MyUploadFile("Huy_");
                mUpload.AutoGeneralFileName = true;
                bool IsSuccess = true;
                string Message = string.Empty;


                if (!string.IsNullOrEmpty(file_UploadText.PostedFile.FileName))
                {
                    mUpload.mPostedFile = file_UploadText.PostedFile;

                    if (mUpload.Upload())
                    {
                        FilePathSave = mUpload.PhysicalUploadedPath;
                    }
                    else
                    {
                        Message += mUpload.Message;
                        IsSuccess = false;
                    }
                }
                else
                {
                    MyMessage.ShowError("Xin hãy chọn file danh sách thuê bao cần hủy.");
                    return;
                }

                if (!IsSuccess)
                {
                    MyMessage.ShowError(Message);
                    return;
                }

                var mList = MyFile.ReadFile_Collection(FilePathSave);
                if (mList.Count < 1)
                {
                    MyMessage.ShowError("File dữ liệu không chính xác, xin vui lòng thử lại");
                    return;
                }

                int SuccessCount = 0;
                int FailCount = 0;
                int NotRegCount = 0;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                DataTable mTable_Service = mService.Select(4, null);
                mTable_Service.DefaultView.RowFilter = "ServiceID = '" + sel_Service.Value + "'";

                if (mTable_Service.DefaultView.Count < 1)
                {
                    MyMessage.ShowError("Dịch vụ không hợp lệ, xin vui lòng kiểm tra lại.");
                    return;
                }
                string CommandCode = mTable_Service.DefaultView[0]["DeregKeyword"].ToString();
                string ServiceName = mTable_Service.DefaultView[0]["ServiceName"].ToString();

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
             
                foreach (string PhoneNumber in mList)
                {

                    string MSISDN = PhoneNumber;

                    if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                    {
                        FailCount++;
                        continue;
                    }
                    string Signature = MSISDN + "|CMS|" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "|" + mTable_Service.DefaultView[0]["PacketName"].ToString() + "|" + mTable_Service.DefaultView[0]["ServiceID"].ToString();
                    Signature = MySecurity.AES.Encrypt(Signature, MySetting.AdminSetting.RegWSKey);
                    System.Net.ServicePointManager.Expect100Continue = false;
                    string Result = mClient.DeReg_VNP(Login1.GetUserName(), MyCurrent.GetRequestIP, Signature, WS_MTraffic.ChannelType.CSKH);
                    string[] Arr_Result = Result.Split('|');

                    string ErrorCode = Arr_Result[0];
                    string ErrorDesc = Arr_Result[1];

                    if (ErrorCode.Equals("1"))
                    {
                        SuccessCount++;
                    }
                    else if (ErrorCode.Equals("6"))
                    {
                        NotRegCount++;
                    }
                    else
                    {
                        FailCount++;
                    }
                }

                string mess = "Đã hủy thành công (" + SuccessCount.ToString() + "), thất bại (" + FailCount.ToString() + "), chưa đăng ký dv (" + NotRegCount.ToString() + ") thuê bao.";
                mLog.Info(mess);
                MyMessage.ShowError(mess);

            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SaveDataError, true, ex);
            }
        }
     
    }
}