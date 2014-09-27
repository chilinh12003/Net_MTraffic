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
    /// Summary description for VNPCharge_TEst
    /// </summary>
    public class VNPCharge_TEst : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string XMLRequest = "";
            string XMLResponse = "";
            try
            {
                //throw new Exception("Loi tu tao day");
                StreamReader reader = new StreamReader(context.Request.InputStream);
                XMLRequest = reader.ReadToEnd();
                XMLRequest = XMLRequest.TrimEnd().TrimStart();

                context.Response.ContentType = "text/xml";
                XMLResponse = MyFile.ReadFile(MyFile.GetFullPathFile("~/App_Data/ResponseCharge.xml"));
                context.Response.Write(XMLResponse);
            }
            catch (Exception ex)
            {
                MyUtility.MyLogfile.WriteLogError(ex);
            }
            finally
            {
                MyUtility.MyLogfile.WriteLogData("CHAGRE_REQUEST", "REQUEST_XML --> " + XMLRequest);
                MyUtility.MyLogfile.WriteLogData("CHAGRE_REQUEST", "RESPONSE_XML-- >" + XMLResponse);
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