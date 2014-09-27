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
namespace MyLoad_Wap.LoadService
{
    public class MyRegConfirm : MyLoadBase
    {
        public enum ActionType
        {
            Register = 1,
            Deregister = 2
        }


        Service mService = new Service();
        Subscriber mSub = new Subscriber();

        public string MSISDN = string.Empty;
        public int ServiceID = 0;
        public int KeywordID = 0;
        public string ServiceName = "";
        public MyRegConfirm(string MSISDN, int ServiceID,string ServiceName, int KeywordID)
        {
            this.MSISDN = MSISDN;
            this.ServiceID = ServiceID;
            this.KeywordID = KeywordID;
            this.ServiceName = ServiceName;
            mTemplatePath = "~/Templates/Service/RegConfirm.htm";            
            Init();
        }


        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                //ServiceID|MSISDN|Time
                string Key = MySecurity.AES.Encrypt(ServiceID.ToString() + "|" + KeywordID.ToString() + "|" + MSISDN + "|" + DateTime.Now.ToString("yyyyMMddHHmmss"), WapSetting.PasswordSpecial);

                string Para = System.Web.HttpUtility.UrlEncode(Key);
                string Alert = string.Empty;
                Alert = "Bạn có muốn tiến hành Đăng ký dịch vụ (" + ServiceName + ")?";
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { Alert, Para });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
