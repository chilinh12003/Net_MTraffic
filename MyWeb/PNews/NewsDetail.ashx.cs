using System;
using System.Collections.Generic;
using System.Web;
using MyLoad.LoadStatic;
using MyLoad.LoadNews;
using MyUtility;
using System.Text;
using MyBase.MyWeb;
using MyLoad.LoadNews;

namespace MyWeb.PNews
{
    /// <summary>
    /// Summary description for NewsDetail
    /// </summary>
    public class NewsDetail : MyASHXBase
    {
        private int ArticleID = 0;

        public override void WriteHTML()
        {
            try
            {

                LoadHeader mHeader = new LoadHeader();
                Write(mHeader.GetHTML());

                LoadBanner mBanner = new LoadBanner();
                Write(mBanner.GetHTML());

                if (Request.QueryString["nid"] != null)
                {
                    int.TryParse(Request.QueryString["nid"], out ArticleID);
                }
                LoadContent mContent = new LoadContent();
                mContent.InsertHTML_Change += new LoadContent.InsertHTML(mContent_InsertHTML_Change);
                Write(mContent.GetHTML());
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex, false, MyNotice.EndUserError.LoadDataError, "Chilinh");
                Write(MyNotice.EndUserError.LoadDataError);
            }
            finally
            {
                LoadFooter mFooter = new LoadFooter();
                Write(mFooter.GetHTML());
            }
        }

        string mContent_InsertHTML_Change()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);


                LoadNewsDetail mDetail = new LoadNewsDetail(ArticleID);
                mBuilder.Append(mDetail.GetHTML());

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}