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
    public class MyConfirm : MyLoadBase
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
        public ActionType mActionType = ActionType.Register;
        public MyConfirm(string MSISDN, int ServiceID, ActionType mActionType)
        {
            this.MSISDN = MSISDN;
            this.ServiceID = ServiceID;
            this.mActionType = mActionType;
            mTemplatePath = "~/Templates/Service/Confirm.htm";            
            Init();
        }


        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                DataTable mTable_Sub = new DataTable();
                DataTable mTable = mService.Select(1, ServiceID.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidService).ToString(), false);
                    return string.Empty;
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidMSISDN).ToString(), false);
                    return string.Empty;
                }

                int PID = MyPID.GetPIDByPhoneNumber(MSISDN, WapSetting.MaxPID);
                mTable_Sub = mSub.Select(2, PID.ToString(), MSISDN, ServiceID.ToString());

                if (mActionType == ActionType.Register && mTable_Sub.Rows.Count > 0)
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.RegExistSub).ToString(), false);
                    return string.Empty;
                }
                if (mActionType == ActionType.Deregister && mTable_Sub.Rows.Count <1 )
                {
                    MyCurrent.CurrentPage.Response.Redirect(WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.DeregNotReg).ToString(), false);
                    return string.Empty;
                }

                //ServiceID|MSISDN|Time
                string Key = string.Empty;

              

                    Key = MySecurity.AES.Encrypt(ServiceID.ToString() + "|" + MSISDN + "|" + DateTime.Now.ToString("yyyyMMddHHmmss"), WapSetting.PasswordSpecial);

                string Para = System.Web.HttpUtility.UrlEncode( Key);
                string Link = string.Empty;
                string Alert = string.Empty;
                if (mActionType == ActionType.Register)
                {
                    Alert = "Bạn có muốn tiến hành Đăng ký dịch vụ (" + mTable.Rows[0]["ServiceName"].ToString() + ") cho số điện thoại (" + MSISDN + ")?";
                    Link = MyConfig.Domain + "/page/reg.html?para=" + Para;
                }
                else if (mActionType == ActionType.Deregister)
                {
                    Alert = "Bạn có muốn tiến hành Hủy dịch vụ (" + mTable.Rows[0]["ServiceName"].ToString() + ") cho số điện thoại (" + MSISDN + ")?";
                    Link = MyConfig.Domain + "/page/dereg.html?para=" + Para;
                }

                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { Alert, Link });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
