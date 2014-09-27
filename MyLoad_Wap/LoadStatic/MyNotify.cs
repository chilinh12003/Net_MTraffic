using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using System.ComponentModel;
using MyUtility;
namespace MyLoad_Wap.LoadStatic
{
    public class MyNotify : MyLoadBase
    {
        public enum NotifyType
        {
            [DescriptionAttribute("Xin lỗi quý khách về sự bất tiện này, xin vui lòng thử lại sau ít phút")]
            Nothing = 0,
            [DescriptionAttribute("Không nhận diện được số điện thoại, xin vui lòng truy cập bằng 3G hoặc GPRS")]
            NoMSISDN = 1,
            [DescriptionAttribute("Số điện thoại không hợp lệ, xin vui lòng kiểm tra lại")]
            InvalidMSISDN = 2,
            [DescriptionAttribute("Thê bao đã được đăng ký dịch vụ trước đó, nên không thể tiến hành đăng ký")]
            RegExistSub = 3,
            [DescriptionAttribute("Thê bao chưa đăng ký sử dụng dịch vụ này, nên không thể tiến hành Hủy dịch vụ.")]
            DeregNotReg = 4,
            [DescriptionAttribute("Thao tác thất bại, xin vui lòng thử lại sau ít phút")]
            Fail = 5,
            [DescriptionAttribute("Dịch vụ này hiện không tồn tại, xin vui lòng chọn lại dịch vụ khác.")]
            InvalidService = 6,
            [DescriptionAttribute("Thông tin không hợp lệ, xin vui lòng thừ lại.")]
            InvalidInput = 7,
            [DescriptionAttribute("Thông tin đã hết hạn, xin vui lòng thử lại.")]
            ExpireInput = 8,
            [DescriptionAttribute("Xin lỗi bạn: Một số thông tin truyền vào từ người dùng có thể không chính xác.<br />Xin vui lòng thử lại với thông tin chính xác hơn. <br />Chân thành cảm ơn!")]
            ErrorPage = 9,
        }

        NotifyType mType = NotifyType.Nothing;

        public string Message = string.Empty;
        public MyNotify(NotifyType mType)
        {
            this.mType = mType;
            mTemplatePath = "~/Templates/Static/Notify.htm";

            Init();
        }
        public MyNotify(string Message)
        {
            this.Message = Message;
            mTemplatePath = "~/Templates/Static/Notify.htm";

            Init();
        }
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                if (string.IsNullOrEmpty(Message))
                {
                    return mLoadTempLate.LoadTemplateByString(mTemplatePath, MyEnum.StringValueOf(mType));
                }
                else
                    return mLoadTempLate.LoadTemplateByString(mTemplatePath, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
