using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
using System.Data;
using System.IO;
using System.Reflection;
using MyMTraffic.Service;
using MyMTraffic.Report;
using MyMTraffic.Sub;
namespace MyTool.ReportSync
{
    public class SyncSub
    {
       

        /// <summary>
        /// Cho biết dừng thread hay không
        /// </summary>
        public static bool StopThread = false;

        /// <summary>
        /// Key của chuỗi kết nối trong config
        /// </summary>
        public string ConnectionKey_Source = "ConnectionKey_Source";
        public string ConnectionKey_Des = "ConnectionKey_Des";

        public int MaxPID = 20;

        /// <summary>
        /// Thời gian delay cho mỗi một lần chạy
        /// </summary>
        public static int SleepSecond
        {
            get
            {
                try
                {
                    int Temp = 1;
                    int.TryParse(MyConfig.GetKeyInConfigFile("SleepSecond"), out Temp);
                    return Temp;
                }
                catch
                {
                    return 1;
                }
            }
        }

        ChargeLog mChargeLog = null;

        RP_Sub mRP_Sub = null;

        Subscriber mSub = null;
        UnSubscriber mUnSub = null;

        public SyncSub()
        {
            try
            {
                mChargeLog = new ChargeLog(ConnectionKey_Source);
                mSub = new Subscriber(ConnectionKey_Source);
                mUnSub = new UnSubscriber(ConnectionKey_Source);

                mRP_Sub = new RP_Sub(ConnectionKey_Des);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu trong Table RP_Sub theo ngày hiện tại
        /// </summary>
        /// <param name="ReportDay"></param>
        /// <returns></returns>
        List<RP_Sub_Object> Get_RP_MO_ByDate(DateTime ReportDay)
        {
            try
            {
                DataTable mTable = mRP_Sub.Select(1, ReportDay.ToString(MyConfig.DateFormat_InsertToDB));
                return RP_Sub_Object.Convert(mTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        DateTime BeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        DateTime StartDate
        {
            get
            {
                if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("StartDate")))
                {

                    return DateTime.ParseExact(MyConfig.GetKeyInConfigFile("StartDate"), "yyyy-MM-dd", null);
                }
                return DateTime.MinValue;
            }
        }

        DateTime EndDate
        {
            get
            {
                return BeginDate.AddDays(1);
            }
        }

        /// <summary>
        /// ngày đầu tiên của tháng
        /// </summary>
        DateTime FirstDateOfMonth
        {
            get
            {
                return new DateTime(BeginDate.Year, BeginDate.Month, 1, 0, 0, 0);
            }
        }

        void UpdateToList(ref List<RP_Sub_Object> mList_Current, RP_Sub_Object.PropertyType mProType, DataTable mTable, bool IsSum)
        {

            try
            {
                FieldInfo mFieldInfo = RP_Sub_Object.GetField(mProType);
                if (mFieldInfo == null)
                    return;

                #region Update các trường được tạo từ các trường khác
                if (mProType == RP_Sub_Object.PropertyType.SubOther)
                {
                    foreach (RP_Sub_Object mObj in mList_Current)
                    {
                        mObj.SubOther = mObj.SubNew - mObj.SubSMS - mObj.SubWAP;
                    }
                    return;
                }

                if (mProType == RP_Sub_Object.PropertyType.UnsubOther)
                {
                    foreach (RP_Sub_Object mObj in mList_Current)
                    {
                        mObj.UnsubOther = mObj.UnsubNew - mObj.UnsubSelf - mObj.UnsubExtend;
                    }
                    return;
                }

                if (mProType == RP_Sub_Object.PropertyType.RenewFail)
                {
                    foreach (RP_Sub_Object mObj in mList_Current)
                    {
                        mObj.RenewFail = mObj.RenewTotal - mObj.RenewSuccess;
                    }
                    return;
                }
                if (mProType == RP_Sub_Object.PropertyType.RenewRate)
                {
                    foreach (RP_Sub_Object mObj in mList_Current)
                    {
                        if (mObj.RenewTotal > 0)
                            mObj.RenewRate = mObj.RenewSuccess / mObj.RenewTotal * 100;
                    }
                    return;
                }
                if (mProType == RP_Sub_Object.PropertyType.RateSaleDay)
                {
                    
                    return;
                }
                #endregion

                foreach (DataRow mRow in mTable.Rows)
                {
                    bool Exist = false;
                    foreach (RP_Sub_Object mObj in mList_Current)
                    {
                        if ((int)mRow["ServiceID"] == mObj.ServiceID &&
                            (int)mRow["PartnerID"] == mObj.PartnerID)
                        {
                            if (IsSum)
                            {
                                //nếu cho phép cộng dồn
                                double SumValue = (double)mFieldInfo.GetValue(mObj);
                                SumValue += double.Parse(mRow["Total"].ToString());
                                mFieldInfo.SetValue(mObj, SumValue);
                            }
                            else
                            {
                                mFieldInfo.SetValue(mObj, double.Parse(mRow["Total"].ToString()));
                            }
                            Exist = true;
                            break;
                        }
                    }

                    //nếu không tồn tại RP_Sub_Object thì thêm mới
                    if (!Exist)
                    {
                        RP_Sub_Object mObj = new RP_Sub_Object();
                        mObj.ReportDay = BeginDate;
                        mObj.ServiceID = (int)mRow["ServiceID"];
                        mObj.PartnerID = (int)mRow["PartnerID"];
                        mFieldInfo.SetValue(mObj, double.Parse( mRow["Total"].ToString()));
                        mList_Current.Add(mObj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        public void Run()
        {
            try
            {
                Console.WriteLine("------------------------------------------");
                Console.WriteLine("BAT DAU CHAY CHUONG TRINH");

                if (StartDate != DateTime.MinValue)
                {
                    BeginDate = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 0, 0, 0);
                }

                DataTable mTable_RP_Sub = mRP_Sub.Select(2, (new DateTime(BeginDate.Year, BeginDate.Month, 1)).ToString(MyConfig.DateFormat_InsertToDB),
                                                            (new DateTime(BeginDate.Year, BeginDate.Month, DateTime.DaysInMonth(BeginDate.Year, BeginDate.Month))).ToString(MyConfig.DateFormat_InsertToDB)
                                                            );
                //Ngày hiện tại
                RP_Sub mRP_Sub_Current = new RP_Sub(this.ConnectionKey_Des);
                while (!StopThread)
                {
                    try
                    {
                        if (BeginDate > DateTime.Now)
                        {
                            BeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                        }
                        //Ngày hiện tại
                        List<RP_Sub_Object> mList_Current = new List<RP_Sub_Object>();

                        //Ngày hôm qua
                        List<RP_Sub_Object> mList_Previos = Get_RP_MO_ByDate(BeginDate.AddDays(-1));

                        //Lấy thông tin tổng đăng ký
                        DataTable mTable = mSub.Select(12, string.Empty);
                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SubTotal, mTable, false);

                        mTable = mUnSub.Select(12, string.Empty);
                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.UnsubTotal, mTable, false);

                        for (int PID = 0; PID <= MaxPID; PID++)
                        {
                            if (StopThread)
                                break;

                            Console.WriteLine("BeginDate:" + BeginDate.ToString(MyConfig.LongDateFormat) + " || Lay du lieu voi pid = " + PID.ToString());

                            #region Đăng ký
                            //Thuê bao đăng ký mới trong ngày
                            mTable = mChargeLog.Select(7, PID.ToString(), ((int)ChargeLog.ChargeType.REG).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(),
                                                                         BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                         EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SubNew, mTable, true);

                            //Đăng ký mới trong ngày từ SMS
                            mTable = mChargeLog.Select(8, PID.ToString(), ((int)ChargeLog.ChargeType.REG).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(), 
                                                                        ((int)MyConfig.ChannelType.SMS).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SubSMS, mTable, true);

                            //Đăng ký mới trong ngày từ WAP
                            mTable = mChargeLog.Select(8, PID.ToString(), ((int)ChargeLog.ChargeType.REG).ToString(),((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(), 
                                                                        ((int)MyConfig.ChannelType.WAP).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));

                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SubWAP, mTable, true); 
                            #endregion

                            #region Hủy đăng ký

                            //Hủy đăng ký mới trong ngày
                            mTable = mChargeLog.Select(7, PID.ToString(), ((int)ChargeLog.ChargeType.UNREG).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(), 
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.UnsubNew, mTable, true);

                            //Hủy Đăng ký mới trong ngày từ SMS
                            mTable = mChargeLog.Select(8, PID.ToString(), ((int)ChargeLog.ChargeType.UNREG).ToString(),((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(), 
                                                                        ((int)MyConfig.ChannelType.SMS).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.UnsubSelf, mTable, true);

                            //Hủy Đăng ký mới trong ngày từ max retry
                            mTable = mChargeLog.Select(8, PID.ToString(), ((int)ChargeLog.ChargeType.UNREG).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(), 
                                                                         ((int)MyConfig.ChannelType.MAXRETRY).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.UnsubExtend, mTable, true);
                            
                            #endregion

                            #region Thuê bao Gia hạn

                            //Tổng thuê bao gia hạn Thành công +không thành công
                            mTable = mChargeLog.Select(6, PID.ToString(), ((int)ChargeLog.ChargeType.RENEW).ToString(), BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                         EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.RenewTotal, mTable, true);

                            //Tổng thuê bao gia hạn thành công
                            mTable = mChargeLog.Select(7, PID.ToString(), ((int)ChargeLog.ChargeType.RENEW).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.RenewSuccess, mTable, true);
                            
                            #endregion

                            #region Tổng tiền gia hạn thành công

                            //Tổng tiền đăng ký thành công
                            mTable = mChargeLog.Select(10, PID.ToString(), ((int)ChargeLog.ChargeType.REG).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SaleReg, mTable, true);

                            //Tổng tiền đăng ký thành công
                            mTable = mChargeLog.Select(10, PID.ToString(), ((int)ChargeLog.ChargeType.RENEW).ToString(), ((int)ChargeLog.ChargeStatus.ChargeSuccess).ToString(),
                                                                        BeginDate.ToString(MyConfig.DateFormat_InsertToDB),
                                                                        EndDate.ToString(MyConfig.DateFormat_InsertToDB));
                            UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SaleRenew, mTable, true);
                            
                            #endregion
                        }

                        //SubOther là trường được tính ra từ các trường đã có sẵn
                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.SubOther, mTable, false);

                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.UnsubOther, mTable, false);

                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.RenewFail, mTable, false);

                        UpdateToList(ref mList_Current, RP_Sub_Object.PropertyType.RenewRate, mTable, false);

                        //Insert vào table RP_MO
                        DataSet mSet = mRP_Sub.CreateDataSet();
                        DataTable mTable_Insert = mSet.Tables[0];

                        foreach (RP_Sub_Object mObj in mList_Current)
                        {
                            mObj.AddNewRow(ref mTable_Insert);
                        }

                        MyConvert.ConvertDateColumnToStringColumn(ref mSet);
                        mRP_Sub.Insert(0, mSet.GetXml());

                        BeginDate = BeginDate.AddDays(1);
                    }
                    catch (Exception ex)
                    {
                        MyLogfile.WriteLogError(ex);
                    }

                    if (BeginDate < DateTime.Now)
                        continue;


                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("CHUONG TRINH SE DELAY " + (SleepSecond / 60).ToString() + " phut.");
                    System.Threading.Thread.Sleep(SleepSecond * 1000);
                }
            }

            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }
        }
    }
}
