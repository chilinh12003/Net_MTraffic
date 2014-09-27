using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyMTraffic;
using System.Data;
using MyUtility;
using MyMTraffic.News;
namespace MyLoad.LoadNews
{
    public class LoadNewsSlide : MyLoadBase
    {
        public LoadNewsSlide()
        {
            mTemplatePath = "~/Templates/News/NewsSlide.htm";
            mTemplatePath_Repeat = "~/Templates/News/NewsSlide_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {                
                Article mArticle = new Article();
                DataTable mTable = mArticle.Search(0, 0, 10, string.Empty, Article.Article_CateID, true, " CreateDate DESC");
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] arr_repeat = {mRow["ArticleID"].ToString(),MyText.CreateRewriteURL(mRow["ArticleName"].ToString()),
                                            MyImage.GetFullPathImage(mRow,1),mRow["ArticleName"].ToString()};
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