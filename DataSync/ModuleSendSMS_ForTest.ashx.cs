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
    /// Summary description for ModuleSendSMS_ForTest
    /// </summary>
    public class ModuleSendSMS_ForTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string XMLRequest = "";
            string XMLResponse = "";
            try
            {
                StreamReader reader = new StreamReader(context.Request.InputStream);
                XMLRequest = reader.ReadToEnd();
                XMLRequest = XMLRequest.TrimEnd().TrimStart();

                context.Response.ContentType = "text/xml";
                XMLResponse = "<?xml version='1.0' encoding='UTF-8'?><soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\"><soapenv:Body><soapenv:Fault><faultcode>soapenv:Server</faultcode><faultstring>Authentication Failed .The Sp IP address is Wrong.</faultstring><detail><ns2:ServiceException xmlns:ns2=\"http://www.csapi.org/schema/parlayx/common/v2_1\"><messageId>01140229291310003279</messageId><text>Authentication Failed .The Sp IP address is Wrong.</text></ns2:ServiceException></detail></soapenv:Fault></soapenv:Body></soapenv:Envelope>";
                context.Response.Write(XMLResponse);
            }
            catch (Exception ex)
            {
                MyUtility.MyLogfile.WriteLogError(ex);
            }
            finally
            {                
                MyUtility.MyLogfile.WriteLogData("VNP_REQUEST", "REQUEST_XML --> " + XMLRequest);
                MyUtility.MyLogfile.WriteLogData("VNP_REQUEST", "RESPONSE_XML-- >" + XMLResponse);
            }            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}