
////CREATE INFO///////////////////////////////////////////////////
//  Author:     Chilinh                                         //
//  CreateDate: 12/03/2009                                      //
//////////////////////////////////////////////////////////////////

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using MyConnect.SQLServer;
using System.Data.SqlClient;
using MyUtility;
using MySetting;
namespace MyMTraffic.Permission
{
    /// <summary>
    /// Dùng để phân quyền cho từng trang
    /// </summary>
    [Serializable()]
    public class GetRole
    {

        public string PageCode = "";
        public string PageName = "";
        public int MemberGroupID = 0;
        /// <summary>
        /// Quyền thêm dữ liệu
        /// </summary>
        public bool AddRole = false;
        /// <summary>
        /// Quyền xóa
        /// </summary>
        public bool DeleteRole = false;
        /// <summary>
        /// Quyền chỉnh sửa
        /// </summary>
        public bool EditRole = false;
        public bool ViewRole = false;
        public bool ExportRole = false;
        /// <summary>
        /// Quyền xuất bản
        /// </summary>
        public bool PublishRole = false;
        /// <summary>
        /// Quyền huyể xuất bản
        /// </summary>
        public bool UnPublishRole = false;
        public bool ImportRole = false;
        /// <summary>
        /// Quyền kích hoạt
        /// </summary>
        public bool ActiveRole = false;
        /// <summary>
        /// Quyền Hủy kích hoạt
        /// </summary>
        public bool InActiveRole = false;

        public bool ExistRole = false;

        public string Message = "";

        public string URLNotView = MyConfig.URLLogin;


        public GetRole()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        /// <summary>
        /// Tạo lớp Role với Những quyền mà người dùng đăng nhập vào được nhận
        /// </summary>
        /// <param name="PageCode"></param>
        /// <param name="MemberGroupID"></param>
        public GetRole(AdminSetting.ListPage PageCode, int MemberGroupID)
        {
            //Lấy trang hiện tại
            URLNotView += "?PrevURL=" + MyCurrent.CurrentPage.Request.Url.ToString();

            this.PageCode = PageCode.ToString();
            this.PageName = MyEnum.StringValueOf(PageCode);

            if (MemberGroupID <= 0)
                return;

            this.AddRole = false;
            this.DeleteRole = false;
            this.EditRole = false;
            this.ViewRole = false;
            this.ExportRole = false;
            this.PublishRole = false;
            this.UnPublishRole = false;
            this.ImportRole = false;
            this.ActiveRole = false;
            this.InActiveRole = false;

            DataTable mTable = MyMTraffic.Permission.Role.Select_Role(MemberGroupID, this.PageCode, this.PageName);
            if (mTable.Rows.Count <= 0)
            {
                this.Message = "Thành viên chưa được phân quyền cho trang trang (" + this.PageName + ")";
                this.ExistRole = false;
                return;
            }
            else
                this.ExistRole = true;

            this.AddRole = (bool)mTable.Rows[0]["AddRole"];
            this.DeleteRole = (bool)mTable.Rows[0]["DeleteRole"];
            this.EditRole = (bool)mTable.Rows[0]["EditRole"];
            this.ViewRole = (bool)mTable.Rows[0]["ViewRole"];
            this.ExportRole = (bool)mTable.Rows[0]["ExportRole"];
            this.PublishRole = (bool)mTable.Rows[0]["PublishRole"];
            this.UnPublishRole = (bool)mTable.Rows[0]["UnPublishRole"];
            this.ImportRole = (bool)mTable.Rows[0]["ImportRole"];
            this.ActiveRole = (bool)mTable.Rows[0]["ActiveRole"];
            this.InActiveRole = (bool)mTable.Rows[0]["InActiveRole"];
        }
    }
}