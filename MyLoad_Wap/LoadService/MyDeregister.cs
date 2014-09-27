using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyBase.MyLoad;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyUtility;
using MySetting;
using MyLoad_Wap.LoadStatic;

using MyMTraffic.Gateway;

namespace MyLoad_Wap.LoadService
{
    public class MyDeregister : MyLoadBase
    {
        Service mService = new Service();
        Subscriber mSub = new Subscriber();
        sms_receive_queue mQuere = new sms_receive_queue(MySetting.AdminSetting.MySQLConnection_Gateway);

        public string MSISDN = string.Empty;
        public int ServiceID = 0;
        public string Para = string.Empty;
        public string Para_Encode = string.Empty;

        public MyDeregister(string MSISDN, string Para)
        {
            this.MSISDN = MSISDN;
            this.Para = Para;

            mTemplatePath = "~/Templates/Service/Deregister.htm";            
            Init();

        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            string Result = string.Empty;
            string ErrorCode = string.Empty;
            string ErrorDesc = string.Empty;
            string Signature = string.Empty;
            string CommandCode = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(Para))
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidInput).ToString(), false);
                    return string.Empty;
                }
                try
                {
                    Para_Encode = MySecurity.AES.Decrypt(Para, WapSetting.PasswordSpecial);
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError(ex);
                }

                if (string.IsNullOrEmpty(Para_Encode))
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidInput).ToString(), false);
                    return string.Empty;
                }

                DateTime ConfirmDate = DateTime.MinValue;

                //ServiceID|MSISDN|Time
                string[] arr = Para_Encode.Split('|');
                if (arr.Length != 3|| 
                    !int.TryParse(arr[0], out ServiceID) ||
                    !MSISDN.Equals(arr[1],StringComparison.OrdinalIgnoreCase) ||
                    !DateTime.TryParseExact(arr[2],"yyyyMMddHHmmss",null,System.Globalization.DateTimeStyles.None,out ConfirmDate))
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidInput).ToString(), false);
                    return string.Empty;
                }
                TimeSpan Temp = DateTime.Now - ConfirmDate;

                if (Temp.TotalMinutes > 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.ExpireInput).ToString(), false);
                    return string.Empty;
                }

                DataTable mTable = mService.Select(1, ServiceID.ToString());
                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidService).ToString(), false);
                    return string.Empty;
                }
                CommandCode = mTable.Rows[0]["DeregKeyword"].ToString();
                string RequestID = MySecurity.CreateCode(9);

                string Message = string.Empty;

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                Signature = MSISDN + "|WAP|" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                Signature = MySecurity.AES.Encrypt(Signature, MySetting.AdminSetting.RegWSKey);
                System.Net.ServicePointManager.Expect100Continue = false;
                Result = mClient.Dereg_CMD((int)MyConfig.ChannelType.CSKH, Signature, CommandCode);
                string[] Arr_Result = Result.Split('|');

                ErrorCode = Arr_Result[0];
                ErrorDesc = Arr_Result[1];

                if (ErrorCode.Equals("1"))
                {
                    Message = "Thông tin hủy dịch vụ (" + mTable.Rows[0]["ServiceName"].ToString() + ") cho số điện thoại (" + MSISDN + ") đã được gửi thành công đến hệ thống chờ xử lý.";
                }
                else
                {
                    MyMessage.ShowError("Xin lỗi, đăng ký/hủy đăng ký không thành công, xin vui lòng thử lại sau");
                }               

                return mLoadTempLate.LoadTemplateByString(mTemplatePath,Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MyLogfile.WriteLogData("Deregister", "CommandCode:" + CommandCode + "|MSISDN:" + MSISDN + "|Signature:" + Signature + "|Result:" + Result);
            }
        }
    }
}
