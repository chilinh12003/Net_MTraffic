using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyCCare.MasterPages
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        public string Title = "GUI - MTRAFFIC";
        protected void Page_Init(object sender, EventArgs e)
        {
            Login1.CheckAndRedirectLogin();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}