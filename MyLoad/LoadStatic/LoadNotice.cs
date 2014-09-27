using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;

namespace MyLoad.LoadStatic
{
    public class LoadNotice : MyLoadBase
    {
        private string Message = string.Empty;
        public LoadNotice()
        {
            mTemplatePath = "~/Templates/Static/Notice.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                string[] arr = {  };
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
