using System;
using System.Collections.Generic;
using System.Web;

namespace MyAdmin.Admin
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class Ad_Catagories_Detail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
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
