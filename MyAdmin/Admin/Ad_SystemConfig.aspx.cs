using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    public partial class Ad_SystemConfig : System.Web.UI.Page
    {
        public GetRole mGetRole;

        private bool CheckPermission()
        {
            try
            {
                if (mGetRole.ViewRole == false)
                {
                    Response.Redirect(mGetRole.URLNotView, false);
                    return false;
                }            

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.CheckPermissionError, "Chilinh");
                return false;
            }
            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Phân quyền
            if (ViewState["Role"] == null)
            {
                mGetRole = new GetRole(MySetting.AdminSetting.ListPage.SystemConfig, Member.MemberGroupID());
            }
            else
            {
                mGetRole = (GetRole)ViewState["Role"];
            }
            if (!CheckPermission())
                return;

            MyAdmin.MasterPages.Admin mMaster = (MyAdmin.MasterPages.Admin)Page.Master;
            mMaster.str_PageTitle = "Cấu hình hệ thông";
            mMaster.str_TitleSearchBox = "Thông tin cấu hình";
        }
    }
}
