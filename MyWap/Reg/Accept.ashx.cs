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
    /// Summary description for Accept
    /// </summary>
    public class Accept : MyWapBase
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
                mLog.Error(ex);
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
                return Register();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string Register()
        {
            string Para = string.Empty;
            string ServiceName = string.Empty;
            int ServiceID = 0;
            int KeywordID = 0;
            string Keyword = string.Empty;

            string ErrorCode = string.Empty;
            string ErrorDesc = string.Empty;
            DateTime ConfirmDate = DateTime.Now.AddDays(-1);
            try
            {
                Para = Request.QueryString["para"];


                if (!string.IsNullOrEmpty(Para))
                {
                    string Para_Decode = MySecurity.AES.Decrypt(Para, WapSetting.PasswordSpecial);
                    if (!string.IsNullOrEmpty(Para_Decode))
                    {
                        string[] arr = Para_Decode.Split('|');
                        if (arr.Length == 4)
                        {
                            int.TryParse(arr[0], out ServiceID);

                            int.TryParse(arr[1], out KeywordID);
                            MSISDN = arr[2];
                            string strDate = arr[3];

                        }
                    }
                }
                else
                {
                    MyNotify mNote = new MyNotify("Thông tin không hợp lệ, xin vui lòng thử lại với thông tin khác.");

                    return mNote.GetHTML();
                }

                //kiểm tra nếu truy cập quá nhanh
                if (Session["RequestTime_Accept"] != null)
                {
                    DateTime RequestTime = (DateTime)Session["RequestTime_Accept"];
                    TimeSpan Interval_Time = DateTime.Now - RequestTime;
                    if (Interval_Time.TotalSeconds < 60)
                    {
                        MyNotify mNote = new MyNotify("Bạn đang truy cập lặp lại quá nhanh, xin vui lòng chờ trong 1 phút và hãy thử lại.");

                        return mNote.GetHTML();
                    }
                }

                Session["RequestTime_Accept"] = DateTime.Now;

                DataTable mTable_Keyword = mKeyword.Select(1, KeywordID.ToString(), string.Empty);

                if (mTable_Keyword.Rows.Count < 1)
                {
                    MyNotify mNote = new MyNotify("Thông tin của đối tác không hợp lệ, xin vui lòng thử lại với thông tin khác.");

                    return mNote.GetHTML();
                }

                // Trả về mã HTML cho header từ template (Fixed)
                if (string.IsNullOrEmpty(MSISDN))
                {
                    MyNotify mNote = new MyNotify("Không nhận diện được thuê bao khách hàng, xin vui lòng sử dụng 3G hoặc GPRS để truy cập.");

                    return mNote.GetHTML();
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                string MSISDN_Temp = MSISDN;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN_Temp, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyNotify mNote = new MyNotify("Số điện thoại không đúng hoặc không thuộc mạng Vinaphone.");

                    return mNote.GetHTML();
                }

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                string Signature = MSISDN + "|HBWap|" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                Signature = MySecurity.AES.Encrypt(Signature, "wre34WD45F");
                System.Net.ServicePointManager.Expect100Continue = false;
                string Result = mClient.Reg((int)MyConfig.ChannelType.WAP, Signature, KeywordID);
                string[] Arr_Result = Result.Split('|');

                ErrorCode = Arr_Result[0];
                ErrorDesc = Arr_Result[1];

                switch (ErrorCode)
                {
                    case "1":
                        ErrorDesc = "Chúc mừng bạn đã đăng ký thành công dịch vụ "+ServiceName+".";
                        break;
                    case "0":
                        ErrorDesc = "Đăng ký dịch vụ không thành công, xin vui lòng thử lại sau ít phút.";
                        break;
                    case "2":
                        ErrorDesc = "Bạn đã đăng ký dịch vụ này trước đây.";
                        break;
                    case "3":
                        ErrorDesc = "Đăng ký không thành công, xin vui lòng thử lại sau ít phút.";
                        break;
                    case "-1":
                        ErrorDesc = "Đăng ký không thành công, xin vui lòng thử lại sau ít phút.";
                        break;
                    case "-2":
                        ErrorDesc = "Đăng ký không thành công, xin vui lòng thử lại sau ít phút.";
                        break;
                }


            }
            catch (Exception ex)
            {
                ErrorDesc = "Xin lỗi,hệ thống đang quá tải, xin vui lòng thử lại sau ít phút.";
                mLog.Error(ex);
                Write(MyNotice.EndUserError.LoadDataError);
            }
            finally
            {
                mLog.Debug("REGISTER", "REGISTER INFO:Keyword:" + Keyword + "|MSISDN:" + MSISDN + "|ErrorCode:" + ErrorCode + "|ErrorDesc:" + ErrorDesc);
            }

            MyNotify mNote_1 = new MyNotify(ErrorDesc);
            return mNote_1.GetHTML();
        }
    }
}