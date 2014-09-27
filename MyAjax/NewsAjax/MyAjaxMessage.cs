using System;
using System.Collections.Generic;
using System.Text;

namespace MyAjax.NewsAjax
{
    public class MyAjaxMessage
    {
        public struct CommonError
        {
            public static string Error = "Có lỗi xảy ra trong quá trình xử lý.";
            public static string CaptchaNotTrue = "Mã xác nhận không chính xác, xin hãy nhập lại";
        }

        public struct UserError
        {
            public static string NotLogin = "Bạn chưa đăng nhập, xin vui lòng tiến hành đăng nhập";
            public static string NotActive = "Tài khoản của bạn chưa được kích hoạt, xin vui lòng kích hoạt.";
            public static string NotExists = "Tài khoản không tồn tại, xin vui lòng đăng ký.";
        }
        public struct AuctionError
        {
            public static string SessionNotExists = "Phiên đấu giá không tồn tại hoặc đã kết thúc.";
            public static string PriceNotSet = "Giá đâu chưa được nhập, xin vui lòng nhập giá đấu";
            public static string SessionNotSet = "Phiên đấu giá chưa được chọn, xin hãy chọn phiên đấu giá.";
            public static string StepPriceIsFail = "Giá đấu không phù hợp với bước giá của phiên đấu giá.";
            public static string NotEnoughMoney = "Tài khoản của bạn không đủ tiền, xin vui lòng nạp thêm tiền.";

        }

        public struct CommonSuccess
        {
            public static string Success = "Xử lý thành công.";
            public static string NoData = "Không có dữ liệu.";
        }
        public struct AuctionFail
        {
            public static string SetPriceFail = "Đặt giá không thành công, xin vui lòng thử lại sau.";
            public static string ChargeFail = "Trừ tiền không thành công, xin vui lòng thử lại sau.";
        }
    }
}
