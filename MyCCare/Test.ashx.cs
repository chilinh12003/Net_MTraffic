using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace MyCCare
{
    /// <summary>
    /// Summary description for Test
    /// </summary>
    public class Test : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("username:" + Login1.GetUserName() + "|Role:" + Login1.GetRole());
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