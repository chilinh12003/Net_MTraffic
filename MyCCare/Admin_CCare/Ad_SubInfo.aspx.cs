using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MyUtility;using MyBase.MyWeb;
using MyMTraffic;
using MyMTraffic.Service;
using MyMTraffic.Sub;


namespace MyCCare.Admin_CCare
{
    public partial class Ad_SubInfo : MyASPXBase
    {
        public int PageIndex = 1;
        Subscriber mSub = new Subscriber();
        UnSubscriber mUnSub = new UnSubscriber();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MyCCare.MasterPages.Admin mMaster = (MyCCare.MasterPages.Admin)Page.Master;
                mMaster.Title = "GUI - Thông tin dịch vụ";

                if (!IsPostBack)
                {
                    ViewState["SortBy"] = string.Empty;
                    tbx_MSISDN.Value = MySetting.AdminSetting.MSISDN;
                }

                Admin_Paging1.rpt_Data = rpt_Data;
                Admin_Paging1.GetData_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetData_Callback(Admin_Paging1_GetData_Callback_Change);
                Admin_Paging1.GetTotalPage_Callback_Change += new MyAdmin.Admin_Control.Admin_Paging.GetTotalPage_Callback(Admin_Paging1_GetTotalPage_Callback_Change);
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.LoadDataError, true, ex);
            }
        }

        int Admin_Paging1_GetTotalPage_Callback_Change()
        {
            try
            {
                string SortBy = ViewState["SortBy"].ToString();
                string SearchContent = tbx_MSISDN.Value;

                int PID = 0;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return 1;
                }
                PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), SearchContent);
                DataTable mTable_UnSub = mUnSub.Select(2, PID.ToString(), SearchContent);

                if (mTable_Sub.Rows.Count + mTable_UnSub.Rows.Count == 0)
                    return 1;

                return mTable_Sub.Rows.Count + mTable_UnSub.Rows.Count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        DataTable Admin_Paging1_GetData_Callback_Change()
        {
            try
            {
                string SortBy = ViewState["SortBy"].ToString();
                string SearchContent = tbx_MSISDN.Value;

                int PID = 0;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref SearchContent, ref mTelco, "84");

                DataTable mTable = mSub.Select(0);
                Service mService = new Service();
                DataTable mTable_Service = mService.Select(4, null);
                
                mTable.Columns.Add(new DataColumn("ServiceName", typeof(string)));

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    return mTable;
                }
                else
                {
                    PID = MyPID.GetPIDByPhoneNumber(SearchContent, MySetting.AdminSetting.MaxPID);

                    DataTable mTable_Sub = mSub.Select(7, PID.ToString(), SearchContent);
                    DataTable mTable_UnSub = mUnSub.Select(7, PID.ToString(), SearchContent);
                    mTable_Sub.Columns.Add(new DataColumn("ServiceName", typeof(string)));
                    mTable_UnSub.Columns.Add(new DataColumn("ServiceName", typeof(string)));

                    foreach(DataRow mRow in mTable_Sub.Rows)
                    {
                        mRow["StatusName"] = "Hoạt động";

                        mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";

                        if (mTable_Service.DefaultView.Count > 0)
                            mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                        else
                            continue;
                        mTable.ImportRow(mRow);
                    }
                    foreach (DataRow mRow in mTable_UnSub.Rows)
                    {
                        mTable_Service.DefaultView.RowFilter = "ServiceID = '" + mRow["ServiceID"].ToString() + "'";

                        if (mTable_Service.DefaultView.Count > 0)
                            mRow["ServiceName"] = mTable_Service.DefaultView[0]["ServiceName"].ToString();
                        else
                            continue;

                        mRow["StatusName"] = "Không hoạt động";
                        mTable.ImportRow(mRow);
                    }

                    mTable_Service.DefaultView.RowFilter = string.Empty;
                }
                foreach(DataRow mRow_Service in mTable_Service.Rows)
                {
                    mTable.DefaultView.RowFilter = "ServiceID = " + mRow_Service["ServiceID"].ToString();
                    if (mTable.DefaultView.Count > 0)
                        continue;

                    DataRow mRow = mTable.NewRow();
                    mRow["ServiceID"] = mRow["ServiceID"];
                    mRow["ServiceName"] = mRow["ServiceName"];
                    mRow["StatusName"] = "Chưa từng sử dụng";
                    mRow["EffectiveDate"] = DBNull.Value;
                    mRow["ExpiryDate"] = DBNull.Value;
                    mTable.Rows.Add(mRow);
                }

                mTable.DefaultView.RowFilter = "";
                return mTable;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                string MSISDN = tbx_MSISDN.Value;
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, "84");

                if (mTelco != MyConfig.Telco.Vinaphone)
                {
                    MyMessage.ShowError("Số điện thoại không chính xác, xin vui lòng kiểm tra lại");
                    return;
                }
                tbx_MSISDN.Value = MSISDN;
                MySetting.AdminSetting.MSISDN = MSISDN;
                Admin_Paging1.ResetLoadData();
            }
            catch (Exception ex)
            {
                mLog.Error(MyNotice.AdminError.SeachError, true, ex);
            }
        }

    }
}