using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyBase.MyWeb;
namespace MyCCare.Admin_CCare
{
    public partial class Ad_ServiceInfo : MyASPXBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
            mMaster.Title = "GUI - Thông tin dịch vụ";
        }
    }
}