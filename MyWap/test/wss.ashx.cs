using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using MyBase.MyWap;
using MyUtility;
using MyLoad_Wap.LoadStatic;
using MyLoad_Wap.LoadService;

namespace MyWap.test
{
    /// <summary>
    /// Summary description for wss
    /// </summary>
    public class wss : MyWapBase
    {

        public override void WriteHTML()
        {
            try
            {
                // Trả về mã HTML cho header từ template (Fixed)
                MyHeader header = new MyHeader();
                Write(header.GetHTML());

                MyContent mContent = new MyContent(MSISDN);
                mContent.InsertHTML_Change += new MyContent.InsertHTML(mContent_InsertHTML_Change);

                Write(mContent.GetHTML());
            }
            catch (Exception ex)
            {
                Write(ex.Message);
            }
            // Trả về mã HTML cho footer từ template (Fixed)
            MyFooter footer = new MyFooter();
            Write(footer.GetHTML());
        }

        string mContent_InsertHTML_Change()
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                if (Session == null)
                {
                    return "<p>Session là null</p>";
                }
                if (Session.Count == 0)
                {
                    return "<p>Session count = 0</p>";
                }
                for (int i = 0; i < Session.Count; i++)
                {
                    mBuilder.Append("<p>" + Session.Keys[i] + ":" + (Session[Session.Keys[i]] == null ? string.Empty : Session[Session.Keys[i]].ToString()) + "</p>");
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}