using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyUtility;
namespace MyAdmin.Admin
{
    public partial class Ad_Alert : System.Web.UI.Page
    {
        public string str_Alert = string.Empty;
        public int iAlertType = 0;

        public enum AlertType
        {
            NotAccessRule = 1,
            InvalidIP = 2,
            InvalidRequest = 3,
        }

        protected string GetIPAddress()
        {
            string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            ((MyAdmin.MasterPages.Admin)Page.Master).ShowToolBox = false;
            ((MyAdmin.MasterPages.Admin)Page.Master).str_TitleSearchBox = "Thông báo:";

            if (Request.QueryString["ID"] != null)
            {
                int.TryParse(Request.QueryString["ID"], out iAlertType);
            }

            switch ((AlertType)iAlertType)
            {
                case AlertType.NotAccessRule:
                    str_Alert = "HIỆN TẠI BẠN KHÔNG CÓ QUYỀN TRUY CẬP VÀO TRANG NÀY, XIN HÃY LIÊN HỆ VỚI ADMIN!";
                    break;
                case AlertType.InvalidIP:
                    str_Alert = "IP CỦA BẠN KHÔNG ĐƯỢC CẤP PHÉP ĐỂ TRUY CẬP VÀO TRANG NÀY, XIN VUI LÒNG LIÊN HỆ VỚI ADMIN!";
                    str_Alert += "<br/>(IP CỦA BẠN:" + GetIPAddress() + ")";
                    break;
                case AlertType.InvalidRequest:
                    str_Alert = "YÊU CẦU CỦA BẠN KHÔNG HỢP LỆ, XIN VUI LÒNG ĐĂNG NHẬP LẠI!";
                    break;
                default:
                    str_Alert = "HIỆN TẠI BẠN KHÔNG CÓ QUYỀN TRUY CẬP VÀO TRANG NÀY, XIN HÃY LIÊN HỆ VỚI ADMIN!";
                    break;
            }
        }
    }
}
