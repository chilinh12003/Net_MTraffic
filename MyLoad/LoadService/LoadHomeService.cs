using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic.Service;
using System.Data;
namespace MyLoad.LoadService
{
    public class LoadHomeService : MyLoadBase
    {
        public LoadHomeService()
        {
            mTemplatePath = "~/Templates/Service/HomeService.htm";
            mTemplatePath_Repeat = "~/Templates/Service/HomeService_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                Service mService = new Service();
                DataTable mTable = mService.Select(4, string.Empty);
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] arr_repeat = {mRow["ServiceID"].ToString(),MyText.CreateRewriteURL(mRow["ServiceName"].ToString()),
                                            MyImage.GetFullPathImage(mRow,1),mRow["ServiceName"].ToString()};
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, arr_repeat));
                }
                string[] arr = { mBuilder.ToString() };
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
