using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using MyUtility;
using MyMTraffic;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyMTraffic.Gateway;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.IO;
namespace MyService
{
    /// <summary>
    /// Summary description for MTraffic
    /// </summary>
    [WebService(Namespace = "http://Mtraffic.vn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class MTraffic : System.Web.Services.WebService
    {

        Subscriber mSub = new Subscriber();
        UnSubscriber mUnSub = new UnSubscriber();
        Keyword mKeyword = new Keyword();
        sms_receive_queue mQuere = new sms_receive_queue(MySetting.AdminSetting.MySQLConnection_Gateway);

        private class PartnerSignature
        {
            public string MSISDN = string.Empty;
            public string UniqueID = string.Empty;
            public string PartnerKey = string.Empty;
            public bool Result = false;
            public string PackageName = string.Empty;
            public int ServiceID = 0;

        }

        private enum REGResult
        {
            [DescriptionAttribute("Không xác định được nguyên nhân")]
            UnknowError = -1,
            [DescriptionAttribute("Lỗi xảy ra trong quá trình thực hiện")]
            Error = -2,
            [DescriptionAttribute("Đăng ký dịch vụ thành công")]
            Success = 1,
            [DescriptionAttribute("Đắng ký thất bại")]
            Fail = 0,
            [DescriptionAttribute("Thuê bao đã đăng ký trước đó")]
            Exist = 2,
            [DescriptionAttribute("Thông tin truyền vào không hợp lệ")]
            InputInvalid = 3,
            [DescriptionAttribute("Đã hủy dịch vụ rồi")]
            Deregistered = 4,
            [DescriptionAttribute("Chưa từng sử dụng dịch vụ này")]
            NeverUseService = 5,
            [DescriptionAttribute("Thuê bao chưa đăng ký dịch vụ")]
            NotRegister = 6,
        }

        private string BuildResult(REGResult mResult)
        {
            return ((int)mResult).ToString() + "|" + MyEnum.StringValueOf(mResult);
        }
        private PartnerSignature CheckSignature(string Signature)
        {
            PartnerSignature mResult = new PartnerSignature();
            try
            {
                string Data = MySecurity.AES.Decrypt(Signature, MySetting.AdminSetting.RegWSKey);
                string[] arr = Data.Split('|');              

                mResult.MSISDN = arr[0];
                mResult.PartnerKey = arr[1];
                mResult.UniqueID = arr[2];
                if (arr.Length >= 4)
                {
                    mResult.PackageName = arr[3];
                }
                if(arr.Length >= 5)
                {
                    int.TryParse(arr[4], out mResult.ServiceID);
                }

                if (string.IsNullOrEmpty(mResult.MSISDN) ||
                    string.IsNullOrEmpty(mResult.PartnerKey) ||
                    string.IsNullOrEmpty(mResult.UniqueID))
                {
                    mResult.Result = false;
                }
                else
                {
                    mResult.Result = true;
                }
                return mResult;
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                return mResult;
            }
        }


        [WebMethod]
        public string Check(string Signature, int KeywordID)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();
            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int PID = MyPID.GetPIDByPhoneNumber(mSig.MSISDN, MySetting.AdminSetting.MaxPID);
                DataTable mTable_Keyword = mKeyword.Select(1, KeywordID.ToString(), string.Empty);

                if (mTable_Keyword == null || mTable_Keyword.Rows.Count < 1)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int ServiceID = (int)mTable_Keyword.Rows[0]["ServiceID"];
                string CommandCode = mTable_Keyword.Rows[0]["Keyword"].ToString();

                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), mSig.MSISDN, ServiceID.ToString());
                if (mTable_Sub.Rows.Count > 0)
                {
                    mResult = REGResult.Exist;
                    return BuildResult(mResult);
                }

                DataTable mTable_UnSub = mUnSub.Select(2, PID.ToString(), mSig.MSISDN);

                //Chưa từng sử dụng dịch vụ này
                mResult = REGResult.NeverUseService;
                return BuildResult(mResult);
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("CHECK-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }
        }

        /// <summary>
        /// Danh cho đăng ký từ wap khi khach hàng click vào link quảng cáo
        /// </summary>
        /// <param name="ChannelType"></param>
        /// <param name="Signature"></param>
        /// <param name="KeywordID"></param>
        /// <returns></returns>
        [WebMethod]
        public string Reg(int ChannelType, string Signature, int KeywordID)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();
          

            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);
                
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int PID = MyPID.GetPIDByPhoneNumber(mSig.MSISDN, MySetting.AdminSetting.MaxPID);
                DataTable mTable_Keyword = mKeyword.Select(1, KeywordID.ToString(), string.Empty);

                if (mTable_Keyword == null || mTable_Keyword.Rows.Count < 1)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int ServiceID = (int)mTable_Keyword.Rows[0]["ServiceID"];
                string CommandCode = mTable_Keyword.Rows[0]["Keyword"].ToString();

                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), mSig.MSISDN, ServiceID.ToString());
                if (mTable_Sub.Rows.Count > 0)
                {
                    mResult = REGResult.Exist;
                    return BuildResult(mResult);
                }

                RequestID = MySecurity.CreateCode(9);
                if (mQuere.Insert(mSig.MSISDN, MySetting.AdminSetting.ShoreCode, CommandCode, CommandCode, RequestID, (MyConfig.ChannelType)ChannelType))
                {
                    mResult = REGResult.Success;
                    return BuildResult(mResult);
                }
                else
                {
                    mResult = REGResult.Fail;
                    return BuildResult(mResult);
                }
            }
            catch (Exception ex)
            {

                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("REQUEST-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|ChannelType:" + ChannelType.ToString() + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }

        }

        [WebMethod]
        public string Reg_CMD(int ChannelType, string Signature, string CommandCode)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();

            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }              

                RequestID = MySecurity.CreateCode(9);
                if (mQuere.Insert(mSig.MSISDN, MySetting.AdminSetting.ShoreCode, CommandCode, CommandCode, RequestID, (MyConfig.ChannelType)ChannelType))
                {
                    mResult = REGResult.Success;
                    return BuildResult(mResult);
                }
                else
                {
                    mResult = REGResult.Fail;
                    return BuildResult(mResult);
                }
            }
            catch (Exception ex)
            {

                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("REQUEST-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|ChannelType:" + ChannelType.ToString() + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }

        }



        [WebMethod]
        public string Dereg_CMD(int ChannelType, string Signature, string CommandCode)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();

            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int PID = MyPID.GetPIDByPhoneNumber(mSig.MSISDN, MySetting.AdminSetting.MaxPID);

                RequestID = MySecurity.CreateCode(9);
                if (mQuere.Insert(mSig.MSISDN, MySetting.AdminSetting.ShoreCode, CommandCode, CommandCode, RequestID, (MyConfig.ChannelType)ChannelType))
                {
                    mResult = REGResult.Success;
                    return BuildResult(mResult);
                }
                else
                {
                    mResult = REGResult.Fail;
                    return BuildResult(mResult);
                }
            }
            catch (Exception ex)
            {

                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("REQUEST-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|ChannelType:" + ChannelType.ToString() + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }

        }

        [WebMethod]
        public DataTable GetInfo(string Signature)
        {
            DataTable mTable_Return = new DataTable();
            try
            {

            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }

            return mTable_Return;
        }


        public static string VNPAPI_Link
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("VNPAPI_Link");
                if (string.IsNullOrEmpty(Temp))
                    return " http://10.1.10.173/vascmd/vasprovisioning/api";
                else
                    return Temp;
            }
        }

        public static string VNPApplication
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("VNPApplication");
                if (string.IsNullOrEmpty(Temp))
                    return "CP_HBCOM";
                else
                    return Temp;
            }
        }

        public static string VNPService
        {
            get
            {
                string Temp = MyConfig.GetKeyInConfigFile("VNPService");
                if (string.IsNullOrEmpty(Temp))
                    return "MTRAFFIC";
                else
                    return Temp;
            }
        }

        private REGResult ConvertResult(VNPResult mVNPResult)
        {
            if (mVNPResult.error.Equals("0") ||
                mVNPResult.error.Equals("3") ||
                mVNPResult.error.Equals("4") ||
                mVNPResult.error.Equals("6") ||
                mVNPResult.error.Equals("7") ||
                mVNPResult.error.Equals("8"))
            {
                return REGResult.Success;
            }
            else if (mVNPResult.error.Equals("1") ||
                mVNPResult.error.Equals("2"))
            {
                return REGResult.Exist;
            }
            else
                return REGResult.Fail;
        }
        [WebMethod]
        public string Reg_VNP(string UserName, string IP, string Signature, MyConfig.ChannelType mChannel)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();

            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int PID = MyPID.GetPIDByPhoneNumber(mSig.MSISDN, MySetting.AdminSetting.MaxPID);

                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), mSig.MSISDN, mSig.ServiceID.ToString());
                if (mTable_Sub.Rows.Count > 0)
                {
                    mResult = REGResult.Exist;
                    return BuildResult(mResult);
                }

                RequestID = MySecurity.CreateCode(9);

                string XML = BuildRequest_Reg(RequestID, mSig.MSISDN, VNPService, mSig.PackageName, VNPApplication, mChannel.ToString(), UserName, IP);

                VNPResult mVNPResult = PostXML(XML);
                mResult = ConvertResult(mVNPResult);
                return BuildResult(mResult);
            }
            catch (Exception ex)
            {

                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("REQUEST-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|ChannelType:" + mChannel.ToString() + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }
        }

        [WebMethod]
        public string DeReg_VNP(string UserName, string IP, string Signature, MyConfig.ChannelType mChannel)
        {
            REGResult mResult = REGResult.UnknowError;
            PartnerSignature mSig = new PartnerSignature();

            string RequestID = string.Empty;
            try
            {
                mSig = CheckSignature(Signature);

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (mSig.Result == false || !MyCheck.CheckPhoneNumber(ref mSig.MSISDN, ref mTelco, "84") || mTelco != MyConfig.Telco.Vinaphone)
                {
                    mResult = REGResult.InputInvalid;
                    return BuildResult(mResult);
                }

                int PID = MyPID.GetPIDByPhoneNumber(mSig.MSISDN, MySetting.AdminSetting.MaxPID);

                DataTable mTable_Sub = mSub.Select(2, PID.ToString(), mSig.MSISDN,mSig.ServiceID.ToString());
                if (mTable_Sub.Rows.Count < 1)
                {
                    mResult = REGResult.NotRegister;
                    return BuildResult(mResult);
                }

                RequestID = MySecurity.CreateCode(9);

                string XML = BuildRequest_DeReg(RequestID, mSig.MSISDN, VNPService, mSig.PackageName, VNPApplication, mChannel.ToString(), UserName, IP);

                VNPResult mVNPResult = PostXML(XML);

                mResult = ConvertResult(mVNPResult);
                return BuildResult(mResult);

            }
            catch (Exception ex)
            {

                MyLogfile.WriteLogError(ex);
                mResult = REGResult.Error;
                return BuildResult(mResult);
            }
            finally
            {
                MyLogfile.WriteLogData("REQUEST-->PartnerKey:" + mSig.PartnerKey + "|MSISDN:" + mSig.MSISDN + "|UniqueID:" + mSig.UniqueID + "|ChannelType:" + mChannel.ToString() + "|Signature:" + Signature + "|Result:" + BuildResult(mResult));
            }
        }

        private string BuildRequest_Reg(string requestid, string msisdn, string service, string package,
                                        string application, string channel, string username, string userip)
        {
            string Template = "<RQST>" +
                                  "<name>subscribe</name>" +
                                  "<requestid>{0}</requestid>" +
                                  "<msisdn>{1}</msisdn>" +
                                  "<service>{2}</service>" +
                                  "<package>{3}</package>" +
                                  "<promotion>0</promotion>" +
                                  "<trial>0</trial>" +
                                  "<bundle>0</bundle>" +
                                  "<note>Test</note>" +
                                  "<application>{4}</application>" +
                                  "<channel>{5}</channel>" +
                                  "<username>{6}</username>" +
                                  "<userip>{7}</userip>" +
                                "</RQST>";
            return string.Format(Template, new object[] { requestid, msisdn, service, package, application, channel, username, userip });

        }

        private string BuildRequest_DeReg(string requestid, string msisdn, string service, string package,
                                        string application, string channel, string username, string userip)
        {
            string Template = "<RQST>" +
                                  "<name>unsubscribe</name>" +
                                  "<requestid>{0}</requestid>" +
                                  "<msisdn>{1}</msisdn>" +
                                  "<service>{2}</service>" +
                                  "<package>{3}</package>" +
                                  "<policy>0</policy>" +
                                  "<promotion>0</promotion>" +
                                  "<note>Test</note>" +
                                  "<application>{4}</application>" +
                                  "<channel>{5}</channel>" +
                                  "<username>{6}</username>" +
                                  "<userip>{7}</userip>" +
                                "</RQST>";
            return string.Format(Template, new object[] { requestid, msisdn, service, package, application, channel, username, userip });
        }

        private class VNPResult
        {
            public string requestid = string.Empty;
            public string error = string.Empty;
            public string error_desc = string.Empty;
            public bool IsNull
            {
                get
                {
                    if (string.IsNullOrEmpty(requestid) ||
                        string.IsNullOrEmpty(error) ||
                        string.IsNullOrEmpty(error_desc))
                        return true;
                    else return false;
                }
            }
        }
        private VNPResult PostXML(string XML)
        {
            string XMLRquest = string.Empty;
            string XMLResponse = string.Empty;

            WebRequest req = null;
            WebResponse rsp = null;

            VNPResult mVNPResult = new VNPResult();

            try
            {
                XMLRquest = XML;
                System.Net.ServicePointManager.Expect100Continue = false;
                req = WebRequest.Create(VNPAPI_Link);
                //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
                req.Method = "POST";        // Post method
                req.ContentType = "text/xml";     // content type
                // Wrap the request stream with a text-based writer

                StreamWriter writer = new StreamWriter(req.GetRequestStream());

                // Write the XML text into the stream
                writer.WriteLine(XMLRquest);
                writer.Close();
                //Send the data to the webserver
                rsp = req.GetResponse();
                XMLResponse = new StreamReader(rsp.GetResponseStream()).ReadToEnd();

                DataSet mSet = MyXML.GetDataSetFromXMLString(XMLResponse);

                if (mSet.Tables.Count > 0 && mSet.Tables[0].Rows.Count > 0)
                {
                    mVNPResult.requestid = mSet.Tables[0].Rows[0]["requestid"].ToString();
                    mVNPResult.error = mSet.Tables[0].Rows[0]["error"].ToString();
                    mVNPResult.error_desc = mSet.Tables[0].Rows[0]["error_desc"].ToString();
                }
            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError(ex);
            }
            finally
            {
                MyLogfile.WriteLogData("POST_XML_VNP", "REQUEST XML --> " + XMLRquest);
                MyLogfile.WriteLogData("POST_XML_VNP", "RESPONSE XML --> " + XMLResponse);

                if (req != null) req.GetRequestStream().Close();
                if (rsp != null) rsp.GetResponseStream().Close();
            }

            return mVNPResult;
        }
    }
}
