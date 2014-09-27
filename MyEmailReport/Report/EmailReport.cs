using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
using System.Data;
using System.IO;
using MyMTraffic.Service;
using MyMTraffic.Report;
using MyMTraffic.Sub;
using System.Web;
namespace MyEmailReport.Report
{
    public class EmailReport
    {
        /// <summary>
        /// Key của chuỗi kết nối trong config
        /// </summary>
        public string ConnectionKey = "SQLConnecton_MTraffic";

        public string EmailAccount = string.Empty;
        public string Password = string.Empty;
        public string EmailReceiveList = string.Empty;
        public string Subject = string.Empty;

        public string Format = string.Empty;
        public string Format_TR_Repeat = string.Empty;
        public string Format_TR_Repeat_2 = string.Empty;
        public void Init()
        {
            try
            {
                EmailAccount = MyConfig.GetKeyInConfigFile("EmailReport_EmailAccount");
                Password = MyConfig.GetKeyInConfigFile("EmailReport_Password");
                EmailReceiveList = MyConfig.GetKeyInConfigFile("EmailReport_EmailReceiveList");
                Subject = MyConfig.GetKeyInConfigFile("EmailReport_Subject");

                Format = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay.htm"));
                Format_TR_Repeat = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TR_Repeat.htm"));
                Format_TR_Repeat_2 = MyFile.ReadFile(MyFile.GetFullPathFile("\\Templates\\ReportByDay_TR_Repeat_2.htm"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
     
        public string BuildEmail()
        {
            try
            {
                DateTime BeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                EndDate = EndDate.AddDays(1);

                RP_Sub mRP_Sub = new RP_Sub(ConnectionKey);

                DataTable mFullData = mRP_Sub.Select(3, BeginDate.ToString(MyConfig.DateFormat_InsertToDB), EndDate.ToString(MyConfig.DateFormat_InsertToDB));

                if (mFullData == null || mFullData.Rows.Count < 1)
                {
                    return string.Empty;
                }                
                double TotalSub_Month = 0;
                double TotalUnSub_Month = 0;
                double TotalRenewSuccess_Month = 0;
                double TotalSale_Month = 0;
                double TotalSale_Day = 0;

                string ReportDay_Previous = string.Empty;
                StringBuilder mBuild_TR = new StringBuilder(string.Empty);
                foreach (DataRow mRow in mFullData.Rows)
                {
                    string ReportDay = ((DateTime)mRow["ReportDay"]).ToString(MyConfig.ShortDateFormat);
                    string ServiceName = mRow["ServiceName"].ToString();
                    string SubTotal = ((double)mRow["SubTotal"]).ToString(MyConfig.IntFormat);
                    string SubNew = ((double)mRow["SubNew"]).ToString(MyConfig.IntFormat);
                    string SubSMS = ((double)mRow["SubSMS"]).ToString(MyConfig.IntFormat);
                    string SubWAP = ((double)mRow["SubWAP"]).ToString(MyConfig.IntFormat);
                    string SubOther = ((double)mRow["SubOther"]).ToString(MyConfig.IntFormat);
                    string UnsubTotal = ((double)mRow["UnsubTotal"]).ToString(MyConfig.IntFormat);
                    string UnsubNew = ((double)mRow["UnsubNew"]).ToString(MyConfig.IntFormat);
                    string UnsubSelf = ((double)mRow["UnsubSelf"]).ToString(MyConfig.IntFormat);
                    string UnsubExtend = ((double)mRow["UnsubExtend"]).ToString(MyConfig.IntFormat);
                    string UnsubOther = ((double)mRow["UnsubOther"]).ToString(MyConfig.IntFormat);
                    string RenewTotal = ((double)mRow["RenewTotal"]).ToString(MyConfig.IntFormat);
                    string RenewSuccess = ((double)mRow["RenewSuccess"]).ToString(MyConfig.IntFormat);
                    string RenewFail = ((double)mRow["RenewFail"]).ToString(MyConfig.IntFormat);
                    string RateRenew = "0";
                    if((double)mRow["RenewTotal"] > 0)
                    {
                        RateRenew = (((double)mRow["RenewSuccess"] * 100) / (double)mRow["RenewTotal"]).ToString(MyConfig.IntFormat);
                    }
                    string SaleReg = ((double)mRow["SaleReg"]).ToString(MyConfig.IntFormat);
                    string SaleRenew = ((double)mRow["SaleRenew"]).ToString(MyConfig.IntFormat);
                    string SaleTotal = ((double)mRow["SaleReg"] + (double)mRow["SaleRenew"]).ToString(MyConfig.IntFormat);

                    TotalSub_Month += (double)mRow["SubNew"] ;
                    TotalUnSub_Month += (double)mRow["UnsubNew"] ;
                    TotalRenewSuccess_Month += (double)mRow["RenewSuccess"];
                    TotalSale_Month += (double)mRow["SaleReg"] + (double)mRow["SaleRenew"];

                    if(!ReportDay_Previous.Equals(ReportDay,StringComparison.CurrentCultureIgnoreCase))
                    {
                        TotalSale_Day = (double)mFullData.Compute("SUM(SaleReg) +SUM(SaleRenew)", "ReportDay = #" + ((DateTime)mRow["ReportDay"]).ToString("MM/dd/yyyy") + "#");

                        mBuild_TR.Append(string.Format(Format_TR_Repeat,
                                                    new string[] { ReportDay, ServiceName, SubTotal, SubNew, SubSMS,SubWAP,SubOther,
                                                    UnsubTotal,UnsubNew,UnsubSelf,UnsubExtend,UnsubOther,
                                                    RenewTotal,RenewSuccess,RenewFail,RateRenew,
                                                    SaleReg,SaleRenew,SaleTotal,
                                                    TotalSale_Day.ToString(MyConfig.IntFormat)
                                                    }));

                        ReportDay_Previous = ReportDay;
                    }
                    else
                    {
                        mBuild_TR.Append(string.Format(Format_TR_Repeat_2,
                                                   new string[] { ServiceName, SubTotal, SubNew, SubSMS,SubWAP,SubOther,
                                                    UnsubTotal,UnsubNew,UnsubSelf,UnsubExtend,UnsubOther,
                                                    RenewTotal,RenewSuccess,RenewFail,RateRenew,
                                                    SaleReg,SaleRenew,SaleTotal
                                                    }));
                    }
                                                                 
                }
                //string Time = ReportDay.ToString(MyConfig.ShortDateFormat) + " LÚC " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                string Time = DateTime.Now.ToString("MM/yyyy");
                return string.Format(Format, Time, mBuild_TR.ToString(), TotalSub_Month.ToString(MyConfig.IntFormat),
                                                                            TotalUnSub_Month.ToString(MyConfig.IntFormat),
                                                                            TotalRenewSuccess_Month.ToString(MyConfig.IntFormat),
                                                                            TotalSale_Month.ToString(MyConfig.IntFormat));
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                throw ex;
            }
        }

        public void Run()
        {
            try
            {
                Console.WriteLine("Bat dau chay chuong trinh");
                Console.WriteLine("Lay thong tin config, va khoi tao");
                Init();

                string pSubject = Subject + DateTime.Now.ToString("dd-MM-yyyy") + " LÚC " + DateTime.Now.Hour + ":" + DateTime.Now.Minute;

                Console.WriteLine("Build Noi dung Email");
                string Body = BuildEmail();
                Console.WriteLine("Bat dau gui email...");

                bool Result = MySendEmail.SendEmail_Google_New(EmailAccount, Password, EmailReceiveList, pSubject, Body, string.Empty);

                MyLogfile.LogEmail(Subject, Body, EmailAccount, Password);

                if (!Result)
                {
                    Console.WriteLine("---GUI EMAIL KHONG THANH CONG----");
                }
                System.Threading.Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("_Error", ex, false);
                Console.WriteLine("---CO LOI XAY RA----");
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
