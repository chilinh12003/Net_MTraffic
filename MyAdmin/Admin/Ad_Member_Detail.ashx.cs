using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using MyUtility;
using MyMTraffic;

using MyMTraffic.Permission;
namespace MyAdmin.Admin
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>

    public class Ad_Member_Detail : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int MemberID = context.Request.QueryString["ID"] == null ? 0 : int.Parse(context.Request.QueryString["ID"]);

            System.Text.StringBuilder str_HTML = new System.Text.StringBuilder(string.Empty);

            try
            {
                System.Data.DataTable mTable = new System.Data.DataTable();
                Member mMember = new Member();

                mTable = mMember.Select(1, MemberID.ToString());

                if (mTable.Rows.Count < 1)
                {
                    context.Response.Write("<div class='FaceBoxAlert_Warning'>KHÔNG CÓ DỮ LIỆU!</div>");
                    return;
                }

                System.Data.DataRow mRow = mTable.Rows[0];

                str_HTML.Append("<div class='ViewDetail'><div class ='ViewDetail_Header'>TÔNG TIN CHI TIẾT VỀ THÀNH VIÊN</div>" +
                             "<div class='ViewDetail_Left'>");

                string[] arrTitile = {  "Ảnh đại diện", "Mà thành viên","Nhóm thành viên",
                                        "Họ và Tên","Tên đăng nhập","Email",
                                        "Phone", "Địa chỉ","Ngày tạo",
                                        "Ngày ĐN cuối","Ngày chỉnh sửa","Kích hoạt" };

                string NgayTao = (mRow["CreateDate"] == DBNull.Value ? string.Empty : ((DateTime)mRow["CreateDate"]).ToString(MyConfig.LongDateFormat));
                string NgayDangNhapCuoi = (mRow["LastLoginDate"] == DBNull.Value ? string.Empty : ((DateTime)mRow["LastLoginDate"]).ToString(MyConfig.LongDateFormat));
                string NgayChinhSua = (mRow["UpdateDate"] == DBNull.Value ? string.Empty : ((DateTime)mRow["UpdateDate"]).ToString(MyConfig.LongDateFormat));

                string DuongDanAnh = mRow["ImagePath"] == DBNull.Value || mRow["ImagePath"].ToString().Trim().Length < 1 ? string.Empty : "<img class='ViewDetail_Anh' src='" + mRow["ImagePath"].ToString() + "' atl=''/>";
                
                string[] arrValue ={DuongDanAnh,mRow["MemberID"].ToString(),mRow["MemberGroupName"].ToString(),
                               mRow["MemberName"].ToString(),mRow["LoginName"].ToString(),mRow["Email"].ToString(),
                               mRow["Phone"].ToString(),mRow["Address"].ToString(),NgayTao,
                               NgayDangNhapCuoi,NgayChinhSua,((bool)mRow["IsActive"]).ToString()};

                int k = DuongDanAnh == string.Empty ? 0 : 3;
                for (int i = 0; i < arrTitile.Length; i++)
                {
                    str_HTML.Append("<div class='ViewDetail_Line'>" +
                                     "<span class='ViewDetail_Title'>" +
                                        arrTitile[i] +
                                     "</span>" +
                                     "<span class='ViewDetail_Content'>" +
                                        arrValue[i] +
                                     "</span>" +
                                "</div>");
                    if (i == arrValue.Length / 2 - k)
                        str_HTML.Append("</div><div class='ViewDetail_Left'>");

                }
                str_HTML.Append("</div></div>");
                
                context.Response.Write(str_HTML.ToString());
            }
            catch (Exception ex)
            {
                context.Response.Write("<div class='FaceBoxAlert_UnSuccess'>Có lỗi trong quá trình tải dữ liệu!</div>");
                MyLogfile.WriteLogError(ex, true, MyNotice.AdminError.LoadDataError, "Chilinh");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
