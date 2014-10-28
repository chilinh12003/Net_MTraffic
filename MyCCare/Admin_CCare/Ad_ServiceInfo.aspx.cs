using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MyCCare.Admin_CCare
{
    public partial class Ad_ServiceInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
            mMaster.Title = "GUI - Thông tin dịch vụ";
        }
    }
}