using System;
using System.Collections.Generic;
using System.Text;
using MyBase.MyLoad;
using MyUtility;
using MyMTraffic.Service;
using System.Data;

namespace MyLoad.LoadService
{
    public class LoadListGroup : MyLoadBase
    {
        public LoadListGroup()
        {
            mTemplatePath = "~/Templates/Service/ListGroup.htm";
            mTemplatePath_Repeat = "~/Templates/Service/ListGroup_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                ServiceGroup mGroup = new ServiceGroup();
                DataTable mTable = mGroup.Select(2, string.Empty);
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string[] arr_repeat = {mRow["ServiceGroupID"].ToString(),MyText.CreateRewriteURL(mRow["ServiceGroupName"].ToString()),mRow["ServiceGroupName"].ToString()};
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
