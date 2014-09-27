using System;
using System.Collections.Generic;
using System.Web;
using MyBase.MyWap;
using MyUtility;
using System.Text;
using MyBase.MyLoad;
using MyMTraffic.Service;
using System.Data;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;
using MySetting;
namespace MyWap.Reg
{
    /// <summary>
    /// Summary description for Deny
    /// </summary>
    public class Deny : MyWapBase
    {

        Keyword mKeyword = new Keyword();

        public override void WriteHTML()
        {
            try
            {
                if (string.IsNullOrEmpty(MSISDN))
                {
                    VNP.VNPGetMSISDN mVNPGet = new VNP.VNPGetMSISDN();
                    MSISDN = mVNPGet.GetMSISDN();
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
                MyNotify mNote = new MyNotify("Bạn đã không đồng ý đăng ký dịch vụ, chân thành cảm ơn.");
                return mNote.GetHTML();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}