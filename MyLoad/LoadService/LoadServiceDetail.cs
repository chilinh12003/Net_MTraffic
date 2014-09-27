using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic.Service;
using System.Data;

namespace MyLoad.LoadService
{
    public class LoadServiceDetail : MyLoadBase
    {
        public int ServiceID = 0;
        public LoadServiceDetail(int ServiceID)
        {
            this.ServiceID = ServiceID;
            mTemplatePath = "~/Templates/Service/ServiceDetail.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                LoadOtherService mOther = new LoadOtherService(ServiceID);
                

                Service mService = new Service();
                DataTable mTable = mService.Select(1, ServiceID.ToString());
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                if (mTable == null || mTable.Rows.Count < 1)
                    return string.Empty;
                DataRow mRow = mTable.Rows[0];

                string[] arr = {mRow["ServiceGroupName"].ToString(),mRow["ServiceName"].ToString(),mRow["Description"].ToString(),
                                          mRow["RegKeyword"].ToString(),
                                          ((double)mRow["Price"]).ToString(MyConfig.DoubleFormat),
                                            mOther.GetHTML()};                    
              
                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}