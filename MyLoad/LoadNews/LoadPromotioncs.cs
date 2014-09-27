using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic;
using System.Data;
using MyMTraffic.News;
namespace MyLoad.LoadNews
{
    public class LoadPromotioncs : MyLoadBase
    {
        public int PageIndex = 0;
        public int CateID = Article.Article_Promo_CateID;

        public LoadPromotioncs(int PageIndex)
        {
            this.PageIndex = PageIndex;
            mTemplatePath = "~/Templates/News/Promotion.htm";
            mTemplatePath_Repeat = "~/Templates/News/Promotion_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                Article mArticle = new Article();

                //Tạo đối tượng để phân trang
                LoadStatic.LoadPaging mPaging = new LoadStatic.LoadPaging();
                mPaging.PageSize = 4;

                mPaging.PageLink = "Promotion.html?cid=" + CateID.ToString();
                mPaging.CurrentPageIndex = PageIndex;

                mPaging.TotalRow = mArticle.TotalRow(0, string.Empty, CateID, true);

                DataTable mTable = mArticle.Search(0, mPaging.BeginRow, mPaging.EndRow, string.Empty, CateID, true, " Priority DESC, ArticleID DESC ");

                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] arr_repeat = {mRow["ArticleID"].ToString(),MyText.CreateRewriteURL(mRow["ArticleName"].ToString()),
                                            mRow["ArticleName"].ToString(), ((DateTime)mRow["CreateDate"]).ToString(MyConfig.LongDateFormat),
                                            MyImage.GetFullPathImage(mRow,1),mRow["Description"].ToString()};

                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, arr_repeat));
                }
                string[] arr = { mBuilder.ToString(),mPaging.GetHTML() };
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
