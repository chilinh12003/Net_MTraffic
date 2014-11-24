using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Web;
using System.Web.SessionState;
using MyUtility;
namespace MyCCare
{
    /// <summary>
    /// Summary description for Login1
    /// </summary>
    public class Login1 : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Nếu đã đăng nhập thì trả về true. nếu chưa thì chuyển đến trang login
        /// </summary>
        /// <returns></returns>
        public static bool CheckAndRedirectLogin()
        {
            if (MyCurrent.CurrentPage.Session == null ||
                MyCurrent.CurrentPage.Session["Username"] == null ||
                MyCurrent.CurrentPage.Session["Role"] == null)
            {
                MyCurrent.CurrentPage.Response.Redirect(MyConfig.Domain + "/Login.aspx");
                return false;
            }
                
            if (!string.IsNullOrEmpty(MyCurrent.CurrentPage.Session["Username"].ToString()) &&
                !string.IsNullOrEmpty(MyCurrent.CurrentPage.Session["Role"].ToString()))
                return true;
            else
            {
                MyCurrent.CurrentPage.Response.Redirect(MyConfig.Domain + "/Login.aspx");
                return false;
            }
        }
        
        public static bool IsAdmin()
        {
            if (MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session["Role"] != null &&
                MyCurrent.CurrentPage.Session["Role"].ToString().Equals("2",StringComparison.CurrentCultureIgnoreCase))
                return true;
            else return false;
        }
        public static string GetUserName()
        {
            if (MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session["Username"] != null)
                return MyCurrent.CurrentPage.Session["Username"].ToString();
            return string.Empty;
        }
        public static string GetRole()
        {
            if (MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session["Role"] != null)
                return MyCurrent.CurrentPage.Session["Role"].ToString();
            return string.Empty;
        }
        public static void Logout()
        {
            if (MyCurrent.CurrentPage.Session != null)
            {
                MyCurrent.CurrentPage.Session["Role"] = null;
                MyCurrent.CurrentPage.Session["Username"] = null;
            }
        }

        public static string SSOLink
        {
            get
            {
                try
                {
                    string temp = MyConfig.GetKeyInConfigFile("SSOLink");
                    if (string.IsNullOrEmpty(temp))
                    {
                        return "http://10.211.0.250:8080";
                    }
                    else
                    {
                        return temp;
                    }
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError(ex);
                    return "http://10.211.0.250:8080";
                }
            }
        }

        public static string SSOLink_Private
        {
            get
            {
                try
                {
                    string temp = MyConfig.GetKeyInConfigFile("SSOLink_Private");
                    if (string.IsNullOrEmpty(temp))
                    {
                        return "http://10.211.0.250:8080";
                    }
                    else
                    {
                        return temp;
                    }
                }
                catch (Exception ex)
                {
                    MyLogfile.WriteLogError(ex);
                    return "http://10.211.0.250:8080";
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var token = context.Request.QueryString["token"];
            var ketqua = ValidateToken(token);
            context.Response.Write(ketqua);
        }

        //hàm validatetoken, đăng nhập và gán quyền
        public int ValidateToken(string token)
        {
            int ketqua = 0; // nếu user không tồn tại trong hệ thống thì trả về 0
            string sURL = SSOLink_Private+"/SSO/SSOService.svc/user/ValidateTokenUrl?token=" + token + "<@-@>10020";
            var client = new WebClient();
            string html = client.DownloadString(sURL);
            MyLogfile.WriteLogData("html:" + html);
            string role = "";
            dynamic data = JsonConvert.DeserializeObject(html);
            var user = data.ValidateTokenUrlResult.Username;
            var SessionToken = data.ValidateTokenUrlResult.SessionToken;

            MyLogfile.WriteLogData("user:" + user);
            MyLogfile.WriteLogData("SessionToken:" + SessionToken);
            // gán session đănng nhập cho user
            AdminInfo items = new AdminInfo();        
            try
            {
                if (string.IsNullOrEmpty(user.Value) || Guid.Empty.Equals(SessionToken.Value))
                    HttpContext.Current.Session.Add("Username", null);
                else
                {
                    
                    if (HttpContext.Current.Session["Username"] == null ||
                         HttpContext.Current.Session["Username"].ToString() == string.Empty)
                    {
                        role = CheckRole(user.Value);
                        items.Username = user.Value;
                        items.Role = role;
                        MyLogfile.WriteLogData("Username:" + items.Username.ToString());
                        MyLogfile.WriteLogData("role:" + items.Role.ToString());
                        HttpContext.Current.Session.Add("Username", items.Username);
                        HttpContext.Current.Session.Add("Role", items.Role);
                        ketqua = 1;
                    }
                    else
                        ketqua = 1;
                }
            }
            catch (Exception ex)
            {
                ketqua = 0;
                MyLogfile.WriteLogError(ex);
            }
            MyLogfile.WriteLogData("ketqua:" + ketqua.ToString());
            return ketqua;
        }
        //hàm kiểm tra quyền
        public string CheckRole(string Username)
        {
            string sURLRole =SSOLink_Private+ "/Role/ServiceRole.svc/user/CheckRole?username=" + Username;
            var client = new WebClient();
            string role = client.DownloadString(sURLRole);
            MyLogfile.WriteLogData("Role:" + role);
            dynamic data = JsonConvert.DeserializeObject(role);
            long res = data.CheckRoleResult.Value;

            return res.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public class AdminInfo
        {
            #region Members
            public int ID { get; set; }
            public string Username { get; set; }

            public string Role { get; set; }
           
            #endregion

            #region Constructor
            public AdminInfo()
            {
                this.ID = 0;
                this.Username = "";
                this.Role = "";  
            }
            #endregion
        }
    }
}