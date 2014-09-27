using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic.Service;
using System.Data;

namespace MyLoad.LoadService
{
    public class LoadOtherService : MyLoadBase
    {
        public int ServiceID = 0;
        public LoadOtherService(int ServiceID)
        {
            this.ServiceID = ServiceID;
            mTemplatePath = "~/Templates/Service/OtherService.htm";
            mTemplatePath_Repeat = "~/Templates/Service/OtherService_Repeat.htm";
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
                mTable.DefaultView.RowFilter = " ServiceID NOT IN ( " + ServiceID.ToString() + " )";

                foreach (DataRowView mRow in mTable.DefaultView)
                {
                    string[] arr_repeat = {mRow["ServiceID"].ToString(),MyText.CreateRewriteURL(mRow["ServiceName"].ToString()),
                                            mRow["ServiceName"].ToString()};

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