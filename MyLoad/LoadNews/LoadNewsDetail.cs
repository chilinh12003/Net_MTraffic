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
    public class LoadNewsDetail : MyLoadBase
    {
        public int ArticleID = 0;
        public LoadNewsDetail(int ArticleID)
        {
            this.ArticleID = ArticleID;
            mTemplatePath = "~/Templates/News/NewsDetail.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                LoadOtherNews mOther = new LoadOtherNews(ArticleID);

                Article mArticle = new Article();
                DataTable mTable = mArticle.Select(1, ArticleID.ToString());
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                if (mTable == null || mTable.Rows.Count < 1)
                    return string.Empty;
                DataRow mRow = mTable.Rows[0];

                string[] arr = {mRow["Articlename"].ToString(),mRow["Description"].ToString(),
                                          mRow["Content"].ToString(),
                                            mOther.GetHTML()};                    
              
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}