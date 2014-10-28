using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using MyBase.MyWap;
using MyUtility;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;
using MyMTraffic.Service;
using System.Data;

namespace MyWap.VNP
{
    /// <summary>
    /// Summary description for ReturnVNP
    /// </summary>
    public class ReturnVNP : MyWapBase
    {
        /// <summary>
        /// Service ID
        /// </summary>
        int sid = 0;


        public override void WriteHTML()
        {
            try
            {
                if (Request.QueryString["sid"] != null)
                {
                    int.TryParse(Request.QueryString["sid"], out sid);
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

                Service mService = new Service();
                DataTable mTable = mService.Select(4, sid.ToString());

                if (mTable == null || mTable.Rows.Count < 1)
                {
                    MyCurrent.CurrentPage.Response.Redirect(MySetting.WapSetting.NotifyURL + "?nid=" + ((int)MyNotify.NotifyType.InvalidService).ToString(), false);
                    return string.Empty;
                }

                MyNotify mNote = new MyNotify("Chúc mừng bạn đã đăng ký thành công dịch vụ " + mTable.Rows[0]["ServiceName"].ToString());
                mBuilder.Append(mNote.GetHTML());

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}