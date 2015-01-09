using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyUtility;using MyBase.MyWeb;
namespace MyCCare
{
    public partial class Default : MyASPXBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Login1.GetUserName()))
                Response.Redirect(MyConfig.Domain + "/Login.aspx");
            else
            {
                Response.Redirect(MyConfig.Domain + "/Admin_CCare/Ad_SubInfo.aspx");
            }
        }
    }
}