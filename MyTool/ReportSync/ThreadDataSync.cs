using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using MyUtility;
using System.IO;
using MyMTraffic.Service;
using MyMTraffic.Report;

namespace MyTool.ReportSync
{
    public class ThreadDataSync
    {
       
        /// <summary>
        /// Cho biết dừng thread hay không
        /// </summary>
        public static bool StopThread = false;

        /// <summary>
        /// Key của chuỗi kết nối trong config
        /// </summary>
        public  string ConnectionKey = "SQLConnecton_MTraffic";
        public string PahtXML = "App_Data\\DataSyncVNP.XML";
        /// <summary>
        /// Chứa danh sách giá trị LastUpdate của record cuối cùng của lần chạy cuối cùng, ứng với từng PID
        /// </summary>
        Dictionary<int, string> ListLastUpdate = new Dictionary<int, string>();

        public int MaxPID = 20;

        public string BeginDate = "2013-01-01 00:00:00";
        public int RowCont = 10;
        private string FormatDay = "yyyy-MM-dd";

        /// <summary>
        /// Thời gian delay cho mỗi một lần chạy
        /// </summary>
        public static int SleepMinute_DataSync
        {
            get
            {
                try
                {
                    int Temp = 1;
                    int.TryParse(MyConfig.GetKeyInConfigFile("SleepMinute_DataSync"), out Temp);
                    return Temp;
                }
                catch
                {
                    return 1;
                }
            }
        }

        void SaveInfo()
        {
            try
            {
                DataTable mTable = new DataTable("DataSyncInfo");
                DataColumn col_PID = new DataColumn("PID", typeof(string));
                DataColumn col_LastUpdate = new DataColumn("LastUpdate", typeof(string));
                mTable.Columns.AddRange(new DataColumn[] { col_PID, col_LastUpdate });
                
                DataSet mSet = new DataSet("DataSet");
                mSet.Tables.Add(mTable);

                foreach(var item in ListLastUpdate)
                {
                    DataRow mRow = mTable.NewRow();
                    mRow["PID"] = item.Key;
                    mRow["LastUpdate"] = item.Value;
                    mTable.Rows.Add(mRow);
                } 
                mSet.WriteXml(MyFile.GetFullPathFile(PahtXML));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void ReadInfo()
        {
            try
            {
                if (!File.Exists(MyFile.GetFullPathFile(PahtXML)))
                {
                    return;
                }
                DataSet mSet = MyXML.GetXMLData(MyFile.GetFullPathFile(PahtXML));
                if (mSet == null || mSet.Tables.Count < 1)
                    return;
                if (mSet.Tables[0].Rows.Count < 1)
                    return;
                ListLastUpdate.Clear();
                foreach (DataRow mRow in mSet.Tables[0].Rows)
                {
                    int PID = 0;
                    string LastUpdate = mRow["LastUpdate"].ToString();
                    if (int.TryParse(mRow["PID"].ToString(), out PID) && !string.IsNullOrEmpty(LastUpdate))
                    {
                        ListLastUpdate.Add(PID, LastUpdate);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetLocalServiceID(DataTable mTable_Sync, DataTable mTable_Service)
        {
            try
            {
                if (!mTable_Sync.Columns.Contains("LocalServiceID"))
                {
                    DataColumn col_1 = new DataColumn("LocalServiceID", typeof(string));
                    mTable_Sync.Columns.Add(col_1);
                }
                foreach (DataRow mRow in mTable_Sync.Rows)
                {
                    mTable_Service.DefaultView.RowFilter = "VNPServiceID = '" + mRow["ServiceID"].ToString() + "'";
                    if (mTable_Service.DefaultView.Count > 0)
                    {
                        mRow["LocalServiceID"] = mTable_Service.DefaultView[0]["ServiceID"].ToString();
                    }
                    else
                    {
                        mRow["LocalServiceID"] = 0;
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

                DataSyncVNP mDataSyncVNP = new DataSyncVNP(ConnectionKey);
                Service mService = new Service(ConnectionKey);
                RPSubByDay mRPSubByDay = new RPSubByDay(ConnectionKey);
                DataSet mSet_RP = mRPSubByDay.CreateDataSet();
                MyConvert.ConvertDateColumnToStringColumn(ref mSet_RP);
                DataTable mTable_RP = mSet_RP.Tables[0];

                DataTable mTable_Service = mService.Select(6, string.Empty);

                ReadInfo();

                while (!StopThread)
                {
                    try
                    {
                        int TotalRow = 1;
                        for (int i = 0; i <= MaxPID; i++)
                        {
                            if (StopThread)
                                break;

                            int PID = i;
                            string LastUpdate = BeginDate;
                            if (ListLastUpdate.ContainsKey(PID))
                            {
                                LastUpdate = ListLastUpdate[PID];
                            }
                            else
                            {
                                ListLastUpdate.Add(PID, LastUpdate);
                            }

                            DataTable mTable = mDataSyncVNP.Select(3, PID.ToString(), RowCont.ToString(), LastUpdate);
                            while (mTable != null && mTable.Rows.Count > 0 && !StopThread)
                            {
                                GetLocalServiceID(mTable, mTable_Service);
                                foreach (DataRow mRow in mTable.Rows)
                                {
                                    string ReportDay = ((DateTime)mRow["LastUpdate"]).ToString(FormatDay);
                                    //CultureInfo ci = CultureInfo.InvariantCulture;
                                    LastUpdate = ((DateTime)mRow["LastUpdate"]).ToString(MyConfig.DateFormat_InsertToDB);
                                    ListLastUpdate[PID] = LastUpdate;

                                    int ServiceID = int.Parse(mRow["LocalServiceID"].ToString());
                                    bool IsReg = mRow["UpdateType"].ToString() == "1" ? true : false;

                                    //nếu đã có từ trước, thì cộng thêm 1
                                    mTable_RP.DefaultView.RowFilter = "ReportDay = '" + ReportDay + "' AND ServiceID = " + ServiceID.ToString();

                                    Console.WriteLine("TotalRow:"+TotalRow++.ToString());
                                    #region MyRegion
                                    if (mTable_RP.DefaultView.Count > 0)
                                    {
                                        if (IsReg)
                                        {
                                            mTable_RP.DefaultView[0]["RegCount"] = (int)mTable_RP.DefaultView[0]["RegCount"] + 1;
                                        }
                                        else
                                        {
                                            mTable_RP.DefaultView[0]["DeregCount"] = (int)mTable_RP.DefaultView[0]["DeregCount"] + 1;
                                        }
                                    }
                                    else
                                    {
                                        DataRow mNewRow = mTable_RP.NewRow();
                                        mNewRow["ServiceID"] = ServiceID;
                                        mNewRow["ReportDay"] = ReportDay;
                                        mNewRow["LastUpdate"] = LastUpdate;
                                        if (IsReg)
                                        {
                                            mNewRow["RegCount"] = 1;
                                            mNewRow["DeregCount"] = 0;
                                        }
                                        else
                                        {
                                            mNewRow["DeregCount"] = 1;
                                            mNewRow["RegCount"] = 0;
                                        }

                                        mTable_RP.Rows.Add(mNewRow);
                                    }
                                    #endregion
                                }
                                if (mTable_RP.Rows.Count >= 10)
                                {
                                    //Update vao database
                                    if (mRPSubByDay.Sync(0, mSet_RP.GetXml()))
                                    {
                                        SaveInfo();
                                        mTable_RP.Clear();
                                    }
                                }
                                mTable = mDataSyncVNP.Select(3, PID.ToString(), RowCont.ToString(), LastUpdate);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MyLogfile.WriteLogError(ex);
                    }

                    if (mTable_RP.Rows.Count > 0)
                    {
                        //Update vao database
                        if (mRPSubByDay.Sync(0, mSet_RP.GetXml()))
                        {
                            SaveInfo();
                            mTable_RP.Clear();
                        }
                    }
                    Console.WriteLine("------------------------------------------");
                    Console.WriteLine("CHUONT RINH SE DELAY " + SleepMinute_DataSync.ToString() + " phut.");
                    System.Threading.Thread.Sleep(SleepMinute_DataSync * 60 * 1000);
                }
                
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }

            try
            {
                SaveInfo();
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }
        }
    }
}
