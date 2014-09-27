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
    public class LoadOtherNews : MyLoadBase
    {
        public int ArticleID = 0;
        public int CateID = Article.Article_CateID;

        public LoadOtherNews(int ArticleID)
        {
            this.ArticleID = ArticleID;
            mTemplatePath = "~/Templates/News/OtherNews.htm";
            mTemplatePath_Repeat = "~/Templates/News/OtherNews_Repeat.htm";
            Init();
        }
        public LoadOtherNews(int ArticleID,int CateID):this(ArticleID)
        {            
            this.CateID = CateID;           
        }
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                Article mArticle = new Article();
                DataTable mTable = mArticle.Select(2, CateID.ToString(), "6");
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                mTable.DefaultView.RowFilter = " ArticleID NOT IN ( " + ArticleID.ToString() + " )";

                foreach (DataRowView mRow in mTable.DefaultView)
                {
                    string[] arr_repeat = {mRow["ArticleID"].ToString(),MyText.CreateRewriteURL(mRow["ArticleName"].ToString()),
                                            mRow["ArticleName"].ToString()};

                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, arr_repeat));
                }
                string[] arr = { mBuilder.ToString() };
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}