using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MyMTraffic;
using MyUtility;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    public partial class Ad_Login : System.Web.UI.Page
    {
        string PrevURL = string.Empty;
        public string AlertMessage = "Xin hãy kiểm tra lại Tăng đăng nhập hoặc Mật khẩu.";

        protected string GetIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!MySetting.AdminSetting.DisableCheckIP)
                {
                    MyIP mMyIP = new MyIP();
                    mMyIP.ImportFromXML(MySetting.AdminSetting.AllowIPFile);

                    string RequestIP = GetIPAddress();
                    if (!mMyIP.CheckNumber(RequestIP) && mMyIP.GetListIPCount() > 0)
                    {
                        Response.Redirect("Ad_Alert.aspx?id=2");
                        return;
                    }
                }
                if (Member.IsLogined())
                {
                    Response.Redirect(MyConfig.URLNotice + "?ID=" + ((int)MyAdmin.Admin.Ad_Alert.AlertType.NotAccessRule).ToString(), false);
                    return;
                }

                PrevURL = HttpContext.Current.Server.UrlDecode(Request.QueryString["PrevURL"]);

            }
            catch(Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
               
                if(Session["Captcha"] != null)
                {
                    string Key = Session["Captcha"].ToString();
                    Key = MySecurity.AES.Decrypt(Key, MyAdmin.Captcha.KeyEncode);
                    if (tbx_Captcha.Value.ToUpper() != Key.ToUpper())
                    {
                        div_Loi.Visible = true;
                        AlertMessage = "Mã xác thực không chính xác, xin vui lòng nhập lại.";
                            return;
                    }
                }
                if (Member.Login(tbx_LoginName.Value, tbx_Password.Value, false, true, false, ref AlertMessage))
                {
                    #region Log member
                    MemberLog mLog = new MemberLog();
                    MemberLog.ActionType Action = MemberLog.ActionType.Login;
                    mLog.Insert("Member", string.Empty, string.Empty, Action, true, tbx_LoginName.Value);
                    #endregion

                    if (!string.IsNullOrEmpty(PrevURL))
                        Response.Redirect(PrevURL, false);
                    else
                        Response.Redirect(MyConfig.URLLoginSuccess, false);
                }
                else
                {
                    div_Loi.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoginError, "Chilinh");
            }
        }
    }
}
