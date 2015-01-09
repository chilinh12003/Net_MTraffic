using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using MyBase.MyWap;
using MyUtility;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;
using MyMTraffic.Service;
using System.Data;
namespace MyWap.VNP
{
    /// <summary>
    /// Summary description for RedirectVNP
    /// </summary>
    public class RedirectVNP : MyWapBase
    {
        /// <summary>
        /// Service ID
        /// </summary>
        int sid = 0;

        /// <summary>
        /// ActionType: 1--> Đăng ký, 2 --> Hủy
        /// </summary>
        int aid = 1;

        public override void WriteHTML()
        {
            try
            {

                if (Request.QueryString["sid"] != null)
                {
                    int.TryParse(Request.QueryString["sid"], out sid);
                }
                if (Request.QueryString["aid"] != null)
                {
                    int.TryParse(Request.QueryString["aid"], out aid);
                }
                // Trả về mã HTML cho header từ template (Fixed)
                MyHeader mHeader = new MyHeader();
                Write(mHeader.GetHTML());
                MyBanner mBanner = new MyBanner(MyBanner.PageSelected.Nothing, MSISDN);
                Write(mBanner.GetHTML());

                MyContent mContent = new MyContent(MSISDN);
                mContent.InsertHTML_Change += new MyContent.InsertHTML(mContent_InsertHTML_Change);

                Write(mContent.GetHTML());



            }
            catch (Exception ex)
            {
                mLog.Error(ex);
                Write(MyNotice.EndUserError.LoadDataError);
            }
            finally
            {
                // Trả về mã HTML cho footer từ template (Fixed)
                MyFooter mFooter = new MyFooter();
                Write(mFooter.GetHTML());
            }
        }
        string mContent_InsertHTML_Change()
        {
            try
            {

                Service mService = new Service();
                DataTable mTable = mService.Select(4, sid.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(MySetting.WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidService).ToString(), false);
                    return string.Empty;
                }

                mTable.DefaultView.RowFilter = "ServiceID = " + sid.ToString();
                if (mTable.DefaultView.Count < 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(MySetting.WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidService).ToString(), false);
                    return string.Empty;
                }

                if(aid == 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(BuildLink_Reg(sid, mTable.DefaultView[0]["PacketName"].ToString()), false);
                    return string.Empty;
                }
                else if(aid == 2)
                {
                    MyCurrent.CurrentPage.Response.Redirect(BuildLink_DeReg(sid, mTable.DefaultView[0]["PacketName"].ToString()), false);
                    return string.Empty;
                }
                else
                {
                    MyCurrent.CurrentPage.Response.Redirect(MySetting.WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidInput).ToString(), false);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string VNPLink_Reg
        {
            get
            {
                string temp = MyConfig.GetKeyInConfigFile("VNPLink_Reg");
                if (string.IsNullOrEmpty(temp))
                {
                    return "http://vinaphone.com.vn/vasgate/reg.jsp";
                }
                else return temp;
            }
        }
        private string VNPLink_Dereg
        {
            get
            {
                string temp = MyConfig.GetKeyInConfigFile("VNPLink_Dereg");
                if (string.IsNullOrEmpty(temp))
                {
                    return "http://vinaphone.com.vn/vasgate/unreg.jsp";
                }
                else return temp;
            }
        }
        private string VNPCPName
        {
            get
            {
                string temp = MyConfig.GetKeyInConfigFile("VNPCPName");
                if (string.IsNullOrEmpty(temp))
                {
                    return "HBCOM";
                }
                else return temp;
            }
        }

        private string VNPSecurepass
        {
            get
            {
                string temp = MyConfig.GetKeyInConfigFile("VNPSecurepass");
                if (string.IsNullOrEmpty(temp))
                {
                    return "hbcom-sBV3K";
                }
                else return temp;
            }
        }
        private string VNPService
        {
            get
            {
                string temp = MyConfig.GetKeyInConfigFile("VNPService");
                if (string.IsNullOrEmpty(temp))
                {
                    return "MTRAFFIC";
                }
                else return temp;
            }
        }

        private string BuildLink_Reg(int ServiceID, string PackageName)
        {
            string requestid = string.Empty;
            string returnurl = string.Empty;
            string backurl = string.Empty;
            string cp = string.Empty;
            string service = string.Empty;
            string package = string.Empty;
            string requestdatetime = string.Empty;
            string channel = string.Empty;
            string securecode = string.Empty;

            string URL = string.Empty;
            string URL_Encode = string.Empty;
            try
            {
                requestid = MySecurity.CreateCode(9);
                returnurl = MyConfig.Domain;
                returnurl = returnurl.ToLower();
                backurl = MyConfig.Domain + "/page/" + ServiceID.ToString() + "/detail.html";
                backurl = backurl.ToLower();
                cp = VNPCPName;
                service = VNPService;
                package = PackageName;
                requestdatetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                channel = "wap";
                securecode = GetMD5Hash(requestid + returnurl + backurl + cp +
                                        service + package + requestdatetime + channel +
                                        VNPSecurepass);

                URL = VNPLink_Reg + "?requestid={0}&returnurl={1}&backurl={2}&cp={3}&service={4}&package={5}&requestdatetime={6}&channel={7}&securecode={8}";
                URL_Encode = string.Format(URL, requestid, HttpUtility.UrlEncode(returnurl),
                                                    HttpUtility.UrlEncode(backurl), cp, service, package, requestdatetime, channel,
                                                    HttpUtility.UrlEncode(securecode));

                return URL_Encode;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mLog.Debug("Redirect VNP", "URL:" + URL_Encode);
            }
        }

        private string BuildLink_DeReg(int ServiceID, string PackageName)
        {
            string requestid = string.Empty;
            string returnurl = string.Empty;
            string backurl = string.Empty;
            string cp = string.Empty;
            string service = string.Empty;
            string package = string.Empty;
            string requestdatetime = string.Empty;
            string channel = string.Empty;
            string securecode = string.Empty;

            string URL = string.Empty;
            string URL_Encode = string.Empty;
            try
            {
                requestid = MySecurity.CreateCode(9);
                returnurl = MyConfig.Domain;
                returnurl = returnurl.ToLower();
                backurl = MyConfig.Domain + "/page/" + ServiceID.ToString() + "/detail.html";
                backurl = backurl.ToLower();
                cp = VNPCPName;
                service = VNPService;
                package = PackageName;
                requestdatetime = DateTime.Now.ToString("yyyyMMddHHmmss");
                channel = "wap";
                securecode = GetMD5Hash(requestid + returnurl + backurl + cp +
                                        service + package + requestdatetime + channel +
                                        VNPSecurepass);
                
                URL = VNPLink_Dereg + "?requestid={0}&returnurl={1}&backurl={2}&cp={3}&service={4}&package={5}&requestdatetime={6}&channel={7}&securecode={8}";
                URL_Encode = string.Format(URL, requestid, HttpUtility.UrlEncode(returnurl),
                                                     HttpUtility.UrlEncode(backurl), cp, service, package, requestdatetime, channel,
                                                     HttpUtility.UrlEncode(securecode));

                return URL_Encode;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mLog.Debug("Redirect VNP", "URL:" + URL_Encode);
            }
        }
        public String GetMD5Hash(String input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new
            System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            String md5String = s.ToString();
            return md5String;
        }
    }
}