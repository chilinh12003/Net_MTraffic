using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using MyUtility;
using MyMTraffic;
using MyMTraffic.News;
namespace MyAdmin.Admin
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class Ad_Article_Detail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string ID = context.Request.QueryString["ID"] == null ? string.Empty : context.Request.QueryString["ID"];

            System.Text.StringBuilder str_HTML = new System.Text.StringBuilder(string.Empty);

            try
            {
                System.Data.DataTable mTable = new System.Data.DataTable();
                Article mArticle = new Article();

                mTable = mArticle.Select(1, ID.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    context.Response.Write("<div class='FaceBoxAlert_Warning'>KHÔNG CÓ DỮ LIỆU!</div>");
                    return;
                }

                System.Data.DataRow mRow = mTable.Rows[0];

                str_HTML.Append("<div class='ViewDetail'><div class ='ViewDetail_Header'>TÔNG TIN CHI TIẾT</div>" +
                             "<div class='ViewDetail_Left'>");

                #region MyRegion
                string[] ColumnName = { 
                                        
                                        "ImagePath_1",
                                        "ImagePath_2",
                                        "CateID",
                                        "CateName",                                       
                                        "CreateDate",
                                        "IsActive",
                                        "Priority"};
                #endregion

                string[] arrTitile = {  
                                        "ImagePath_1",
                                        "ImagePath_2",
                                        "CateID",
                                        "CateName",
                                        "CreateDate",
                                        "IsActive",
                                        "Priority"};

                string[] arrValue = MyConvert.ConvertDataRowToArray(mRow, ColumnName);

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
                    if (i == arrValue.Length / 2)
                        str_HTML.Append("</div><div class='ViewDetail_Left'>");

                }
                str_HTML.Append("</div>");
                str_HTML.Append("<div class='ViewDetail_Split'>Tiêu đề</div><div class='ViewDetail_NoiDung'>" + mRow["ArticleName"].ToString() + "</div>");
                str_HTML.Append("<div class='ViewDetail_Split'>Mổ tả (trích dẫn)</div><div class='ViewDetail_NoiDung'>" + mRow["Description"].ToString() + "</div>");
                str_HTML.Append("<div class='ViewDetail_Split'>Nội dung bài viết</div><div class='ViewDetail_NoiDung'>" + mRow["Content"].ToString() + "</div>");
                str_HTML.Append("</div>");

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
