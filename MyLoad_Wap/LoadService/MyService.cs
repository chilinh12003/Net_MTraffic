using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyBase.MyLoad;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyUtility;
using MySetting;

namespace MyLoad_Wap.LoadService
{
    public class MyService : MyLoadBase
    {
        Service mService = new Service();
        Subscriber mSub = new Subscriber();

        public string MSISDN = string.Empty;
        public MyService(string MSISDN)
        {
            this.MSISDN = MSISDN;
            mTemplatePath = "~/Templates/Service/Service.htm";
            mTemplatePath_Repeat = "~/Templates/Service/Service_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                DataTable mTable = mService.Select(4, string.Empty);

                DataTable mTable_Sub = new DataTable();

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    int PID = MyPID.GetPIDByPhoneNumber(MSISDN, WapSetting.MaxPID);
                    mTable_Sub = mSub.Select(7, PID.ToString(), MSISDN);
                }
                StringBuilder mBuilder = new StringBuilder(string.Empty);

                foreach (DataRow mRow in mTable.Rows)
                {
                    string ActionType = "1";// 1 --> Đăng ký, 2--> Hủy
                    string ButtonName = "Đăng ký dịch vụ";

                    if (mTable_Sub != null && mTable_Sub.Rows.Count > 0)
                    {
                        mTable_Sub.DefaultView.RowFilter = "ServiceID = " + mRow["ServiceID"].ToString();
                        if (mTable_Sub.DefaultView.Count > 0)
                        {
                            ActionType = "2";
                            ButtonName = "Hủy dịch vụ";
                        }
                    }
                    string[] arr = { mRow["ServiceID"].ToString(),mRow["ServiceName"].ToString(),MyImage.GetFullPathImage(mRow,2),mRow["RegKeyword"].ToString(),
                                   ActionType,ButtonName};
                    mBuilder.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, arr));
                }
                return mLoadTempLate.LoadTemplateByString(mTemplatePath, mBuilder.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
