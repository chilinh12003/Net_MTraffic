using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
namespace MyLoad_Wap.LoadStatic
{
    public class MyHeader:MyLoadBase
    {
        public string CSSLink = string.Empty;
        public string JSLink = string.Empty;
        public string HeaderScript = string.Empty;
        public string Title = string.Empty;
        public string ImgURL = string.Empty;
        public string Description = string.Empty;

        public MyHeader()
        {
            mTemplatePath = "~/Templates/Static/Header.htm";
            Init();
        }
        public MyHeader(string CSSLink, string JSLink, string HeaderScript)
            : this()
        {
            this.CSSLink = CSSLink;
            this.JSLink = JSLink;
            this.HeaderScript = HeaderScript;
        }
        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                string[] arr = {Title,Description,ImgURL, CSSLink, JSLink, HeaderScript };
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
