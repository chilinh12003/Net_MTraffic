using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyBase.MyWeb;
namespace MyCCare
{
    public partial class LogOut : MyASPXBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Login1.Logout();
            Response.Redirect(Login1.SSOLink_Private + "/SSO/Logout.aspx");
        }
    }
}