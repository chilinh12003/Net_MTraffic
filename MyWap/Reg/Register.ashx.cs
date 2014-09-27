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

namespace MyWap.Reg
{
    /// <summary>
    /// Summary description for Register
    /// </summary>
    public class Register : MyWapBase
    {
        Service mService = new Service();
        Keyword mKeyword = new Keyword();
        VNP.VNPGetMSISDN mGetMSISDN = new VNP.VNPGetMSISDN();


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
                return BuildRegister();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BuildRegister()
        {
            string Para = string.Empty;
            string ServiceName = string.Empty;
            int ServiceID = 0;
            int KeywordID = 0;
            int PartnerID = 0;
            string Keyword = string.Empty;

            string ErrorCode = string.Empty;
            string ErrorDesc = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["kid"]))
                {
                    int.TryParse(Request.QueryString["kid"].TrimEnd().TrimStart(), out KeywordID);
                }

                MSISDN = mGetMSISDN.GetMSISDN();

                // Trả về mã HTML cho header từ template (Fixed)
                if (string.IsNullOrEmpty(MSISDN))
                {
                    MyNotify mNote = new MyNotify("Không nhận diện được thuê bao khách hàng, xin vui lòng sử dụng 3G hoặc GPRS để truy cập.");
                    return mNote.GetHTML();
                }

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                string MSISDN_TEMP = MSISDN;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN_TEMP, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyNotify mNote = new MyNotify("Số điện thoại không đúng hoặc không thuộc mạng Vinaphone.");
                    return mNote.GetHTML();
                }

                //kiểm tra nếu truy cập quá nhanh
                if (Session["RequestTime"] != null)
                {
                    DateTime RequestTime = (DateTime)Session["RequestTime"];
                    TimeSpan Interval_Time = DateTime.Now - RequestTime;
                    if (Interval_Time.TotalSeconds < 60)
                    {
                        MyNotify mNote = new MyNotify("Bạn đang truy cập lặp lại quá nhanh, xin vui lòng chờ trong 1 phút và hãy thử lại.");
                        return mNote.GetHTML();
                    }
                }

                Session["RequestTime"] = DateTime.Now;

                DataTable mTable_Keyword = mKeyword.Select(2, KeywordID.ToString(), string.Empty);

                if (mTable_Keyword.Rows.Count < 1)
                {
                    MyNotify mNote = new MyNotify("Thông tin của đối tác không hợp lệ, xin vui lòng thử lại với thông tin khác.");
                    return mNote.GetHTML();
                }

                PartnerID = (int)mTable_Keyword.Rows[0]["PartnerID"];
                Keyword = mTable_Keyword.Rows[0]["Keyword"].ToString();
                ServiceID = (int)mTable_Keyword.Rows[0]["ServiceID"];


                DataTable mTable_Service = mService.Select(1, ServiceID.ToString());
                if (mTable_Service.Rows.Count < 1)
                {
                    MyNotify mNote = new MyNotify("Thông tin của đối tác không hợp lệ, xin vui lòng thử lại với thông tin khác.");
                    return mNote.GetHTML();
                }
                ServiceName = mTable_Service.Rows[0]["ServiceName"].ToString();

                WS_MTraffic.MTrafficSoapClient mClient = new WS_MTraffic.MTrafficSoapClient();
                string Signature = MSISDN + "|HBWap|" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                Signature = MySecurity.AES.Encrypt(Signature, "wre34WD45F");
                System.Net.ServicePointManager.Expect100Continue = false;

                //nếu chưa từng sử dụng dịch vụ lần nào và keyword này là yêu cầu confirm
                if ((bool)mTable_Keyword.Rows[0]["IsConfirm"])
                {
                    MyRegConfirm mConfirm = new MyRegConfirm(MSISDN, ServiceID, ServiceName, KeywordID);
                    return mConfirm.GetHTML();
                }
                //nếu không thì đăng ký ngay
                string Result = mClient.Reg((int)MyConfig.ChannelType.WAP, Signature, KeywordID);
                string[] Arr_Result = Result.Split('|');

                ErrorCode = Arr_Result[0];
                ErrorDesc = string.Empty;

                switch (ErrorCode)
                {
                    case "1":
                        ErrorDesc = "Chúc mừng bạn đã đăng ký thành công dịch vụ " + ServiceName + ".";
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
                MyLogfile.WriteLogError("_Error", ex, false, MyNotice.EndUserError.LoadDataError, "Chilinh");
                Write(MyNotice.EndUserError.LoadDataError);
            }
            finally
            {

                MyLogfile.WriteLogData("REGISTER", "REGISTER INFO: PartnerID:" + PartnerID.ToString() + "|Keyword:" + Keyword + "|MSISDN:" + MSISDN + "|ErrorCode:" + ErrorCode + "|ErrorDesc:" + ErrorDesc);
              
            }
            MyNotify mNote_1 = new MyNotify(ErrorDesc);
            return mNote_1.GetHTML();

        }
    }
}