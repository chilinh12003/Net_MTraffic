using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
namespace DataSync
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    public class Test : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.Write(Post());
        }

        private string Post()
        {
            WebRequest req = null;
            WebResponse rsp = null;
            string ret = string.Empty;
            try
            {
                string XML = MyUtility.MyFile.ReadFile(MyUtility.MyFile.GetFullPathFile("~/App_Data/RequestFromVNP.xml"));
                string uri = "http://localhost:8080/MTraffic_DataSync/VNPRequest.ashx";
                req = WebRequest.Create(uri);
                //req.Proxy = WebProxy.GetDefaultProxy(); // Enable if using proxy
                req.Method = "POST";        // Post method
                req.ContentType = "text/xml";     // content type
                // Wrap the request stream with a text-based writer
                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                // Write the XML text into the stream
                writer.WriteLine(XML);
                writer.Close();
                // Send the data to the webserver
                rsp = req.GetResponse();
                ret = new StreamReader(rsp.GetResponseStream()).ReadToEnd();
            }
            catch (WebException webEx)
            {
                string s = webEx.Message;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                if (req != null) req.GetRequestStream().Close();
                if (rsp != null) rsp.GetResponseStream().Close();
            }
            return ret;
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