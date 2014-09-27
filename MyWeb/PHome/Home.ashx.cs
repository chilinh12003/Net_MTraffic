using System;
using System.Collections.Generic;
using System.Web;
using MyLoad.LoadStatic;
using MyLoad.LoadNews;
using MyUtility;
using System.Text;
using MyBase.MyWeb;
using MyLoad.LoadService;

namespace MyWeb.PHome
{
    /// <summary>
    /// Summary description for Home
    /// </summary>
    public class Home : MyASHXBase
    {
        public override void WriteHTML()
        {
            try
            {
                LoadHeader mHeader = new LoadHeader();           
                Write(mHeader.GetHTML());

                LoadBanner mBanner = new LoadBanner();
                Write(mBanner.GetHTML());

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
                LoadHomeService mHomeService = new LoadHomeService();
                mBuilder.Append(mHomeService.GetHTML());

                LoadNewsSlide mNewsSlide = new LoadNewsSlide();
                mBuilder.Append(mNewsSlide.GetHTML());

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}