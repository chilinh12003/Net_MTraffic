using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using MyBase.MyWap;
using MyUtility;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;
namespace MyWap.Page
{
    /// <summary>
    /// Summary description for confirm
    /// </summary>
    public class confirm : MyWapBase
    {
        /// <summary>
        /// Service ID
        /// </summary>
        int sid = 0;

        /// <summary>
        /// ActionType: 1--> Đăng ký, 2 --> Hủy
        /// </summary>
        int aid = 1;

        public override void WriteHTML()
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN))
                {
                    VNP.VNPGetMSISDN mVNPGet = new VNP.VNPGetMSISDN();
                    MSISDN = mVNPGet.GetMSISDN();
                }

                if (Request.QueryString["sid"] != null)
                {
                    int.TryParse(Request.QueryString["sid"], out sid);
                }
                if (Request.QueryString["aid"] != null)
                {
                    int.TryParse(Request.QueryString["aid"], out aid);
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
                MyConfirm mConfirm = new MyConfirm(MSISDN,sid,(MyConfirm.ActionType)aid);

                mBuilder.Append(mConfirm.GetHTML());

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}