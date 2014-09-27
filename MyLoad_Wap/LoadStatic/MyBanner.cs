using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;

namespace MyLoad_Wap.LoadStatic
{
    public class MyBanner : MyLoadBase
    {
        public enum PageSelected
        {
            Nothing = 0,
            Home = 1,
            Service = 2,
            Guide = 3,
        }
        public string LastJavascript = string.Empty;
        public string MSISDN = string.Empty;
        private PageSelected mPageSelected = PageSelected.Nothing;

        public MyBanner(PageSelected mPageSelected, string MSISDN)
        {
            this.MSISDN = MSISDN;
            this.mPageSelected = mPageSelected;

            mTemplatePath = "~/Templates/Static/banner.htm";

            Init();
        }
        
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                string HomeCss = string.Empty;
                string ServiceCss = string.Empty;
                string GuideCss = string.Empty;
                switch (mPageSelected)
                {
                    case PageSelected.Home:
                        HomeCss = "select";
                        break;
                    case PageSelected.Service:
                        ServiceCss = "select";
                        break;
                    case PageSelected.Guide:
                        GuideCss = "select";
                        break;

                }

                // Lấy template từ file HTML 
                // Đồng thời truyền tham số {0} dựa vào dạng template được truyền vào khi gọi hàm
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, new string[] { HomeCss, ServiceCss, GuideCss, MSISDN });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
