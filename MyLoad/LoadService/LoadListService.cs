using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic.Service;
using System.Data;
namespace MyLoad.LoadService
{
    public class LoadListService : MyLoadBase
    {
        public int ServiceGroupID = 0;
        public LoadListService(int ServiceGroupID)
        {
            this.ServiceGroupID = ServiceGroupID;
            mTemplatePath = "~/Templates/Service/ListService.htm";
            mTemplatePath_Repeat = "~/Templates/Service/ListService_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                Service mService = new Service();
                DataTable mTable = mService.Select(3, ServiceGroupID.ToString());
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] arr_repeat = {mRow["ServiceID"].ToString(),MyText.CreateRewriteURL(mRow["ServiceName"].ToString()),
                                            MyImage.GetFullPathImage(mRow,2),mRow["ServiceName"].ToString(),mRow["RegKeyword"].ToString()};

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
