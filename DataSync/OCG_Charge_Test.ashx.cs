using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;
using System.IO;
using MyUtility;
using System.Data;
using MyMTraffic.Service;
using MyMTraffic.Sub;
using MyBase.MyWeb;
namespace DataSync
{
    /// <summary>
    /// Summary description for OCG_Charge_Test
    /// </summary>
    public class OCG_Charge_Test : MyASHXBase
    {

        public override void WriteHTML()
        {

            string XMLRequest = "";
            string XMLResponse = "";
            try
            {
                //throw new Exception("Loi tu tao day");
                StreamReader reader = new StreamReader(Request.InputStream);
                XMLRequest = reader.ReadToEnd();
                XMLRequest = XMLRequest.TrimEnd().TrimStart();

                Response.ContentType = "text/xml";
                XMLResponse = MyFile.ReadFile(MyFile.GetFullPathFile("~/App_Data/OCG_Charge_Result.xml"));
                Response.Write(XMLResponse);
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
            }
            finally
            {
                mLog.Debug("CHAGRE_REQUEST", "REQUEST_XML --> " + XMLRequest);
                mLog.Debug("CHAGRE_REQUEST", "RESPONSE_XML-- >" + XMLResponse);
            }
        }


    }
}