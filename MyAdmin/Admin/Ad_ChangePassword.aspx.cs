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
    public partial class Ad_ChangePassword : System.Web.UI.Page
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
                mGetRole = new GetRole(MySetting.AdminSetting.ListPage.ChangePass, Member.MemberGroupID());
            }
            else
            {
                mGetRole = (GetRole)ViewState["Role"];
            }
            if (!CheckPermission())
                return;

            if (!Member.IsLogined())
            {
                Response.Redirect(MyConfig.URLLogin, false);
            }
            else
            {
                tbx_LoginName.Value = Member.LoginName();
                tbx_LoginName.Disabled = true;
            }
        }

        protected void lbtn_Apply_Click(object sender, EventArgs e)
        {
            try
            {
                Member MyMember = new Member();

                //Lấy mật khẩu cũ của ngưởi dùng
                string str_OldPass = MyMember.Password(Member.MemberID());

                if (str_OldPass != MySecurity.Encrypt_MD5(tbx_OldPass.Value))
                {
                    MyMessage.ShowError("Mật khẩu cũ không chính xác!");
                    return;
                }

                if (tbx_NewPass.Value != tbx_RepeatPass.Value)
                {
                    MyMessage.ShowError("Mật khẩu mới và Nhắc lại mật khẩu không giống nhau!");
                    return;
                }

                //Mất khẩu mới của người dùng
                string str_NewPass = MySecurity.Encrypt_MD5(tbx_NewPass.Value);

                if (MyMember.ChangePassword(Member.MemberID(), MySecurity.Encrypt_MD5(tbx_NewPass.Value)))
                {
                    MyMessage.ShowMessage("Đổi mật khẩu thành công.");
                }
                else
                {
                    MyMessage.ShowMessage("Đổi mật khẩu không thành công");
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.SaveDataError, "Chilinh");
            }
        }
        
    }
}
