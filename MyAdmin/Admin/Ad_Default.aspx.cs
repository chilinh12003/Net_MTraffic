using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    public partial class Ad_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Member.IsLogined())
            {
                Response.Redirect(MyConfig.URLLogin);
            }
        }
    }
}
