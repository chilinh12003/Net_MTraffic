using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using MyBase.MyWap;
using MyUtility;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;


namespace MyWap
{
    /// <summary>
    /// Summary description for Error
    /// </summary>
    public class Error : MyWapBase
    {
        int NotifyID = 9;

        public override void WriteHTML()
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN))
                {
                    VNP.VNPGetMSISDN mVNPGet = new VNP.VNPGetMSISDN();
                    MSISDN = mVNPGet.GetMSISDN();
                }

                if (Request.QueryString["nid"] != null)
                {
                    int.TryParse(Request.QueryString["nid"], out NotifyID);

                }
                // Trả về mã HTML cho header từ template (Fixed)
                MyHeader mHeader = new MyHeader();
                Write(mHeader.GetHTML());

                MyBanner mBanner = new MyBanner(MyBanner.PageSelected.Nothing, MSISDN);
                Write(mBanner.GetHTML());

                MyContent mContent = new MyContent(MSISDN);
                mContent.InsertHTML_Change += new MyContent.InsertHTML(mContent_InsertHTML_Change);

                Write(mContent.GetHTML());
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex, false, MyNotice.EndUserError.LoadDataError, "Chilinh");
                Write(MyNotice.EndUserError.LoadDataError);
            }

            // Trả về mã HTML cho footer từ template (Fixed)
            MyFooter mFooter = new MyFooter();
            Write(mFooter.GetHTML());
        }

        string mContent_InsertHTML_Change()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                MyError mError = new MyError();
                mBuilder.Append(mError.GetHTML());

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}