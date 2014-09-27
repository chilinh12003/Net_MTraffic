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
    public class MyDetail : MyLoadBase
    {
        Service mService = new Service();
        Subscriber mSub = new Subscriber();

        public string MSISDN = string.Empty;
        public int ServiceID = 0;

        public MyDetail(string MSISDN, int ServiceID)
        {
            this.MSISDN = MSISDN;
            this.ServiceID = ServiceID;

            mTemplatePath = "~/Templates/Service/Detail.htm";
            mTemplatePath_Repeat = "~/Templates/Service/OtherService_Repeat.htm";
            Init();
        }

        // Hàm trả về chuỗi có chứa mã HTML
        protected override string BuildHTML()
        {
            try
            {
                DataTable mTable = mService.Select(1, ServiceID.ToString());

                DataTable mTable_Orther = mService.Select(4, string.Empty);

                mTable_Orther.DefaultView.RowFilter = "ServiceID = " + ServiceID.ToString();

                if (mTable_Orther.DefaultView.Count > 0)
                    mTable_Orther.DefaultView[0].Delete();
                mTable_Orther.AcceptChanges();

                DataTable mTable_Sub = new DataTable();

                if (!string.IsNullOrEmpty(MSISDN))
                {
                    int PID = MyPID.GetPIDByPhoneNumber(MSISDN, WapSetting.MaxPID);
                    mTable_Sub = mSub.Select(2, PID.ToString(), MSISDN, ServiceID.ToString());
                }

                StringBuilder mBuilder = new StringBuilder(string.Empty);
                StringBuilder mBuilder_Other = new StringBuilder(string.Empty);

                foreach (DataRow mRow_Other in mTable_Orther.Rows)
                {
                    mBuilder_Other.Append(mLoadTempLate.LoadTemplateByArray(mTemplatePath_Repeat, new string[] { mRow_Other["ServiceID"].ToString(), mRow_Other["ServiceName"].ToString() }));
                }

                DataRow mRow = mTable.Rows[0];

                string ActionType = "1";
                string ButtonName = "Đăng ký dịch vụ";

                if (mTable_Sub != null && mTable_Sub.Rows.Count > 0)
                {
                    ActionType = "2";
                    ButtonName = "Hủy dịch vụ";
                }

                string[] arr = { mRow["ServiceGroupName"].ToString(),mRow["ServiceName"].ToString(),
                                       mRow["Description"].ToString(),mRow["RegKeyword"].ToString(),
                                       mRow["ServiceID"].ToString(),ActionType,ButtonName , mBuilder_Other.ToString()};

                return mLoadTempLate.LoadTemplateByArray(mTemplatePath, arr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
