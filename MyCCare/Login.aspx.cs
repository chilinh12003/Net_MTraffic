using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace MyCCare
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Login1.GetUserName()))
            {
                Response.Redirect("~/Admin_CCare/Ad_SubInfo.aspx");
            }
        }
    }
}