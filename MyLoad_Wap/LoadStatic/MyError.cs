using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using System.ComponentModel;
using MyUtility;
namespace MyLoad_Wap.LoadStatic
{
    public class MyError : MyLoadBase
    {


        public MyError()
        {
            mTemplatePath = "~/Templates/Static/Error.htm";

            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                return mLoadTempLate.LoadTemplate(mTemplatePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
