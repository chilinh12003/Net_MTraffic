using System;
using System.Collections.Generic;
using System.Text;
using MyUtility;
namespace MySetting
{
    public class WapSetting
    {
        public static int MaxPID
        {
            get
            {
                return 20;
            }
        }

        public static string NotifyURL
        {
            get
            {
                return MyUtility.MyConfig.Domain+"/page/notify.html";
            }
        }
        public static string PasswordSpecial
        {
            get
            {
                return "ChiLInH";
            }
        }

        public static string ShoreCode
        {
            get
            {
                return "1546";
            }
        }
        public static string Charging_Servicename_VNP
        {
            get
            {
                if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("Charging_Servicename_VNP")))
                {
                    return MyConfig.GetKeyInConfigFile("Charging_Servicename_VNP");
                }
                else
                {
                    return "MTRAFFIC";
                }
            }
        }

        public static string Charging_URLGetMSISDN_VNP
        {
            get
            {
                if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("Charging_URLGetMSISDN_VNP")))
                {
                    return MyConfig.GetKeyInConfigFile("Charging_URLGetMSISDN_VNP");
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
