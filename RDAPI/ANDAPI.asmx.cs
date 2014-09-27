using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace RDAPI
{
    /// <summary>
    /// Summary description for ANDAPI
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ANDAPI : System.Web.Services.WebService
    {
        public enum RegResult
        {
            //0	Đăng ký thành công dịch vụ
            Success = 0,
            //1	Thuê bao này đã tồn tại
            ExistSub = 1,
            //2	Đăng ký rồi và đăng ký lại dịch vụ
            Repeat = 2,
            //3	Đăng ký thành công dịch vụ và không bị trừ cước đăng ký
            SuccessFree = 3,
            //4	Đăng ký thành công dịch vụ và bị trừ cước đăng ký
            SucessPay = 4,
            //5	Đăng ký không thành công do không đủ tiền trong tài khoản
            EnoughMoney = 6,
            //1xx	Đều là đăng ký không thành công
            Fail = 100,
            //Lỗi hệ thống
            SystemError = 101,
            //Thông tin nhập vào không hợp lệ
            InputInvalid = 102

        }

        [WebMethod]
        public string Register(int requestid, string msisdn, string packagename, string promotion, string trial,
                                string bundle, string note, string application, string channel, string username, string userip)
        {
            return "Hello World";
        }

        public enum DeregResult
        {
            //0: Success
            Success = 0,
            //1: Không tồn tại thuê bao này
            NotExist = 1,
            //1xx: Các mã lỗi do đối tác quy định
            Fail = 100,

        }
        [WebMethod]
        public string Register(int requestid,
                                string msisdn,
                                string packagename,
                                string policy,
                                string promotion,
                                string note,
                                string application,
                                string channel,
                                string username,
                                string userip)
        {

            return "Hello World";
        }

        public enum GetInfoResult
        {
            //0: Success
            Success = 0,          
            //1xx: Các mã lỗi do đối tác quy định
            Fail = 100,

        }
        [WebMethod]
        public string GetInfo(int requestid,
                                string msisdn,
                                string packagename,
                                string application,
                                string channel,
                                string username,
                                string userip)
        {
            return "Hello World";
        }
    }
}
