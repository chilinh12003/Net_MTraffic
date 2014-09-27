using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.IO;
using MyUtility;
using System.Data;
using MyVOVTraffic.Service;
using MyVOVTraffic.Sub;
namespace DataSync
{
    /// <summary>
    /// Summary description for VNPRequest
    /// </summary>
    public class VNPRequest : IHttpHandler
    {
        /// <summary>
        /// ID của dịch vụ Giao thông theo tuyến đường
        /// </summary>
        public static int StreetServiceID
        {
            get
            {
                string ConfigValue = MyConfig.GetKeyInConfigFile("StreetServiceID");

                if (string.IsNullOrEmpty(ConfigValue))
                    return 0;
                else
                    return int.Parse(ConfigValue);
            }
        }

        /// <summary>
        /// Mã của tên đường nếu dịch vụ là Giao thông theo tuyến
        /// </summary>
        private int StreetID = 0;

        /// <summary>
        /// Chứa thông tin do VNP chuyển sang
        /// </summary>
        DataSyncVNP.DataSyncVNPObject mDataSyncVNPObject = new DataSyncVNP.DataSyncVNPObject();

        /// <summary>
        /// Chứa thông tin về dịch vụ (dựa vào thông tin của VNP chuyền sang để lấy)
        /// </summary>
        Service.ServiceObject mServiceObject = new Service.ServiceObject();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";      
            StreamReader reader = new StreamReader(context.Request.InputStream);
            string XML = reader.ReadToEnd();
            XML = XML.TrimEnd().TrimStart();
            DataSyncVNP.Result mResult_Response = DataSyncVNP.Result.Fail;

            try
            {
                //Lấy thông tin từ VNP
                GetVNPInfo(XML);

                if (mDataSyncVNPObject.IsNull)
                {
                    mResult_Response = DataSyncVNP.Result.ValueInvalid;
                    return;
                }
                //Insert xuống dataSyncVNP trong db

                if (!InsertToDataSyncVNP())
                {
                    mResult_Response = DataSyncVNP.Result.Fail;
                    return;
                }

                //Lấy thông tin về dịch vụ
                GetServiceInfo(mDataSyncVNPObject.ServiceID, mDataSyncVNPObject.ProductID);

                if (mServiceObject.IsNull)
                {
                    mResult_Response = DataSyncVNP.Result.ServiceInvalid;
                    return;
                }
                if (mDataSyncVNPObject.UpdateType == 1) //Đăng ký dịch vụ
                {
                    if (!InsertToSub())
                    {
                        mResult_Response = DataSyncVNP.Result.Fail;
                        return;
                    }
                }
                else if(mDataSyncVNPObject.UpdateType == 2) //Huy đăng ký
                {
                    if (!DeleteFromSub())
                    {
                        mResult_Response = DataSyncVNP.Result.DeregisterNotSuccess;
                        return;
                    }
                }

                mResult_Response = DataSyncVNP.Result.Success;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                mResult_Response = DataSyncVNP.Result.SystemError;
            }
            finally
            {
                string Response_XML = GetResponse(mResult_Response);
                MyLogfile.WriteLogData("VNP_REQUEST","REQUEST_XML --> " +XML);
                MyLogfile.WriteLogData("VNP_REQUEST", "RESPONSE_XML-- >" + Response_XML);
                context.Response.Write(Response_XML);
            }
        }       
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Build chuỗi XML response trả về cho VNP
        /// </summary>
        /// <param name="mResult"></param>
        /// <returns></returns>
        private string GetResponse(DataSyncVNP.Result mResult)
        {
            try
            {
                string Format = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:loc=\"http://www.csapi.org/schema/parlayx/data/sync/v1_0/local\">" +
                                    "<soapenv:Header/>" +
                                    "<soapenv:Body>" +
                                    "<loc:syncOrderRelationResponse>" +
                                        "<loc:result>{0}</loc:result>" +
                                        "<loc:resultDescription>{1}</loc:resultDescription>" +
                                    "</loc:syncOrderRelationResponse>" +
                                    "</soapenv:Body>" +
                                "</soapenv:Envelope>";

                return string.Format(Format, new string[] { ((int)mResult).ToString(), MyEnum.StringValueOf(mResult) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy thông tin từ VNP chuyển sang
        /// </summary>
        /// <param name="XML"></param>
        /// <returns></returns>
        private void GetVNPInfo(string XML)
        {
            try
            {
                DataSet mSet_VNP = MyXML.GetDataSetFromXMLString(XML);
                if (mSet_VNP == null || mSet_VNP.Tables.Count < 1)
                {
                    return;
                }            

                mDataSyncVNPObject.ProductOrderKey = mSet_VNP.Tables["namedParameters"].Rows[0]["value"].ToString();
                mDataSyncVNPObject.MSISDN = mSet_VNP.Tables["userID"].Rows[0]["ID"].ToString();
                string MSISDNType = mSet_VNP.Tables["userID"].Rows[0]["type"].ToString();
                if (!string.IsNullOrEmpty(MSISDNType.Trim()))
                {
                    int.TryParse(MSISDNType,out mDataSyncVNPObject.MSISDNType);
                }

                mDataSyncVNPObject.SPID = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["spID"].ToString();
                mDataSyncVNPObject.ProductID = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["productID"].ToString();
                mDataSyncVNPObject.ServiceID = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["serviceID"].ToString();
                mDataSyncVNPObject.ServiceList = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["serviceList"].ToString();

                string UpdateType = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["updateType"].ToString();
                if (!string.IsNullOrEmpty(UpdateType.Trim()))
                {
                    int.TryParse(UpdateType, out mDataSyncVNPObject.UpdateType);
                }

                string UpdateTime = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["updateTime"].ToString();
                if (!string.IsNullOrEmpty(UpdateTime.Trim()))
                {
                    DateTime mTime = new DateTime();
                    if (DateTime.TryParseExact(UpdateTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out mTime))
                    {
                        mDataSyncVNPObject.UpdateTime= mTime;
                    }
                }

                mDataSyncVNPObject.UpdateDesc = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["updateDesc"].ToString();

                string EffectiveTime = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["effectiveTime"].ToString();
                if (!string.IsNullOrEmpty(EffectiveTime.Trim()))
                {
                    DateTime mTime = new DateTime();
                    if (DateTime.TryParseExact(EffectiveTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out mTime))
                    {
                        mDataSyncVNPObject.EffectiveTime = mTime;
                    }
                }

                string ExpiryTime = mSet_VNP.Tables["syncOrderRelation"].Rows[0]["expiryTime"].ToString();
                if (!string.IsNullOrEmpty(ExpiryTime.Trim()))
                {
                    DateTime mTime = new DateTime();
                    if (DateTime.TryParseExact(ExpiryTime, "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out mTime))
                    {
                        mDataSyncVNPObject.ExpiryTime = mTime;
                    }
                }

                mDataSyncVNPObject.PID = MyPID.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Lấy thông tin về service dựa vào 2 thông tin ServiceID, ProductID do VNP chuyển sang
        /// </summary>
        /// <param name="VNPServiceID"></param>
        /// <param name="VNPProductID"></param>
        /// <returns></returns>
        private void GetServiceInfo(string VNPServiceID, string VNPProductID)
        {
            try
            {
                Service mService = new Service();
                DataTable mTable = new DataTable();

                //Nếu không có thì kiểm tra trong table street
                //đối với dịch vụ theo tên đường
                MyVOVTraffic.News.Street mStreet = new MyVOVTraffic.News.Street();
                DataTable mTable_Street = mStreet.Select(6, VNPServiceID, VNPProductID);

                if (mTable_Street != null && mTable_Street.Rows.Count > 0)
                {
                    StreetID = (int)mTable_Street.Rows[0]["StreetID"]; 
                    mTable = mService.Select(1, StreetServiceID.ToString());
                }

                if (mTable != null && mTable.Rows.Count > 0)
                {
                    mServiceObject = Service.ServiceObject.Convert(mTable);
                    return;
                }

                mTable = mService.Select(2, VNPServiceID, VNPProductID);

                if (mTable != null && mTable.Rows.Count > 0)
                {
                    mServiceObject = Service.ServiceObject.Convert(mTable);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert xuống table DataSyncVNP trong database
        /// <para>Nếu đã tồn tại ProductOrderKey thì update, còn không thì insert</para>
        /// </summary>
        /// <returns></returns>
        private bool InsertToDataSyncVNP()
        {
            try
            {
                DataSyncVNP mDataSync = new DataSyncVNP();
                DataSet mSet = new DataSet("Parent");
                DataTable mTable = DataSyncVNP.DataSyncVNPObject.Convert(mDataSyncVNPObject);
                mTable.TableName = "Child";
                mSet.Tables.Add(mTable.Copy());

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                return mDataSync.SyncData(0, mSet.GetXml());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert xuống các table sub trong database
        /// </summary>
        private bool InsertToSub()
        {
            try
            {
                Subscriber mSub = new Subscriber();
                int PID = MyPID.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);

                DataSet mSet = new DataSet();
                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), mDataSyncVNPObject.MSISDN, mServiceObject.ServiceID.ToString());
                bool IsExist = false;

                if (mTable_Sub != null && mTable_Sub.Rows.Count > 0)
                {
                    IsExist = true;
                }
              
                mSet = mSub.CreateDataSet();                
                
                mSet.DataSetName = "Parent";
                mSet.Tables[0].TableName = "Child";
                if (IsExist)
                {
                    mSet.Tables[0].ImportRow(mTable_Sub.Rows[0]);
                    mSet.Tables[0].Rows[0]["ProductOrderKey"] = mDataSyncVNPObject.ProductOrderKey;
                    mSet.Tables[0].Rows[0]["EffectiveTime"] = mDataSyncVNPObject.EffectiveTime;
                    mSet.Tables[0].Rows[0]["ExpiryTime"] = mDataSyncVNPObject.ExpiryTime;
                    mSet.Tables[0].Rows[0]["UpdateTime"] = mDataSyncVNPObject.UpdateTime;
                }
                else
                {
                    DataRow mNewRow = mSet.Tables[0].NewRow();
                    mNewRow["ServiceID"] = mServiceObject.ServiceID;
                    mNewRow["MSISDN"] = mDataSyncVNPObject.MSISDN;
                    mNewRow["ProductOrderKey"] = mDataSyncVNPObject.ProductOrderKey;
                    mNewRow["CreateDate"] = DateTime.Now;
                    mNewRow["EffectiveTime"] = mDataSyncVNPObject.EffectiveTime;
                    mNewRow["ExpiryTime"] = mDataSyncVNPObject.ExpiryTime;
                    mNewRow["UpdateTime"] = mDataSyncVNPObject.UpdateTime;
                    mNewRow["ChannelTypeID"] = (int)Subscriber.ChannelType.SMS;
                    mNewRow["ChannelTypeName"] = MyEnum.StringValueOf(Subscriber.ChannelType.SMS);
                    mNewRow["StatusID"] = (int)Subscriber.Status.Active;
                    mNewRow["PID"] = PID;
                    mNewRow["TotalMT"] = 0;
                    mNewRow["TotalMTByDay"] = 0;

                    mSet.Tables[0].Rows.Add(mNewRow);
                }

                MyConvert.ConvertDateColumnToStringColumn(ref mSet);

                if (IsExist)
                {
                    if (!mSub.Update(0, mSet.GetXml()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!mSub.Insert(0, mSet.GetXml()))
                    {
                        return false;
                    }
                }              

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa tên đường đã đăng ký
        /// </summary>
        /// <returns></returns>
        private bool DeleteRegisterStreet()
        {
            try
            {
                RegisterStreet mRegStreet = new RegisterStreet();

                int PID = MyPID.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);
                DataSet mSet = new DataSet("Parent");
                DataTable mTable = new DataTable("Child");
                DataRow mRow = mTable.NewRow();

                mRow["PID"] = PID;
                mRow["StreetID"] = StreetID;
                mRow["MSISDN"] = mDataSyncVNPObject.MSISDN;
                mTable.Rows.Add(mRow);

                mSet.Tables.Add(mTable);

                return mRegStreet.Delete(3, mSet.GetXml());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Huy đăng ký dịch vụ
        /// </summary>
        /// <returns></returns>
        private bool DeleteFromSub()
        {
            try
            {
                Subscriber mSub = new Subscriber();
                int PID = MyPID.GetPIDByPhoneNumber(mDataSyncVNPObject.MSISDN);

                DataTable mTable_Sub = mSub.Select(5, PID.ToString(), mDataSyncVNPObject.ProductOrderKey);

                //nếu không tồn tại ProductOrderKey thì coi như hủy thành công
                if (mTable_Sub == null || mTable_Sub.Rows.Count < 1)
                    return true;

                DataSet mSet = new DataSet("Parent");
                DataTable mTable = new DataTable("Child");
                DataColumn col_PID = new DataColumn("PID", typeof(int));
                DataColumn col_ServiceID = new DataColumn("ServiceID", typeof(int));
                DataColumn col_MSISDN = new DataColumn("MSISDN", typeof(string));

                mTable.Columns.AddRange(new DataColumn[] {col_PID,col_ServiceID,col_MSISDN });

                DataRow mRow = mTable.NewRow();
                mRow["PID"] = PID;
                mRow["ServiceID"] = mServiceObject.ServiceID;
                mRow["MSISDN"] = mDataSyncVNPObject.MSISDN;

                mTable.Rows.Add(mRow);

                mSet.Tables.Add(mTable);

                if (!mSub.Delete(3, mSet.GetXml()))
                    return false;             

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}