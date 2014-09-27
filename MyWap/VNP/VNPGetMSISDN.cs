using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using MyUtility;

namespace MyWap.VNP
{
    public class VNPGetMSISDN
    {


        public static string getRemoteAddr()
        {
            try
            {
                string ipaddress = MyCurrent.CurrentPage.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ipaddress))
                {
                    ipaddress = MyCurrent.CurrentPage.Request.ServerVariables["REMOTE_ADDR"];
                }
                ipaddress = ipaddress.Trim();
                string[] arr = ipaddress.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length > 1)
                {
                    if (!string.IsNullOrEmpty(arr[arr.Length - 1]))
                    {
                        return arr[arr.Length - 1];
                    }
                    else if (string.IsNullOrEmpty(arr[arr.Length - 1]) && !string.IsNullOrEmpty(arr[0]))
                    {
                        return arr[0];
                    }
                    else
                        return ipaddress;

                }
                return ipaddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CheckType">
        /// <para>Type = 1: check Ip của F5</para>
        /// <para>Type = 2: check IP của WapGetday</para>
        /// </param>
        /// <param name="IPCheck"></param>
        /// <returns></returns>
        public bool CheckIP(int CheckType)
        {
            string IPCheck = string.Empty;
            string F5IPPattern = "(^(10)(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$)|(^(113\\.185\\.)([1-9]|1[0-9]|2[0-9]|3[0-1])(\\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])))";
            string WAPGWIPPattern = "(^172.16.30.1[1-2]$)|(113.185.0.16)";

            IPCheck = getRemoteAddr();

            if (CheckType == 1)
            {
                if (string.IsNullOrEmpty(IPCheck))
                    return false;
                string[] arr = IPCheck.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        return false;
                    }

                    if (Regex.IsMatch(item.Trim(), F5IPPattern, RegexOptions.IgnoreCase))
                        return true;
                }
                return false;
            }
            else if (CheckType == 2)
            {
                //Kiểm tra Ip qua WAPGate
                if (string.IsNullOrEmpty(IPCheck))
                    return false;

                string[] arr = IPCheck.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr)
                {
                    if (string.IsNullOrEmpty(item.Trim()))
                    {
                        return false;
                    }
                    if (Regex.IsMatch(item.Trim(), WAPGWIPPattern, RegexOptions.IgnoreCase))
                        return true;
                }
                return false;
            }

            return false;
        }

        public string GetMSISDN()
        {
            string URL = string.Empty;
            string Response = string.Empty;
            string GetFrom = "";
            string MSISDN_Return = string.Empty;
            try
            {

                //Nhận thực qua MIN
                string remoteip = getRemoteAddr();
                string msisdn = MyCurrent.CurrentPage.Request.Headers["msisdn"];
                string xipaddress = MyCurrent.CurrentPage.Request.Headers["X-ipaddress"];
                string xforwarded = MyCurrent.CurrentPage.Request.Headers["X-Forwarded-For"];
                string xwapmsisdn = MyCurrent.CurrentPage.Request.Headers["X-Wap-MSISDN"];
                string userip = MyCurrent.CurrentPage.Request.Headers["User-IP"];
                string service = MySetting.WapSetting.Charging_Servicename_VNP;

                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (CheckIP(1)) //Nếu qua F5
                {
                    GetFrom += "F5|";
                    if (xipaddress.Split(',').Length > 1)
                    {
                        return MSISDN_Return;
                    }
                    if (!string.IsNullOrEmpty(xipaddress) && xipaddress.StartsWith("10."))
                    {
                        MSISDN_Return = MyCurrent.CurrentPage.Request.Headers["msisdn"];

                        MyCheck.CheckPhoneNumber(ref MSISDN_Return, ref mTelco, "84");
                        if (mTelco == MyConfig.Telco.Vinaphone)
                            return MSISDN_Return;
                    }
                }
                else if (CheckIP(2)) //Nếu qua wapgate
                {
                    GetFrom += "WAPGATE|";
                    if (userip.Split(',').Length > 1)
                    {
                        return MSISDN_Return;
                    }
                    if (!string.IsNullOrEmpty(userip) && userip.StartsWith("10."))
                    {
                        MSISDN_Return = MyCurrent.CurrentPage.Request.Headers["X-Wap-MSISDN"];

                        MyCheck.CheckPhoneNumber(ref MSISDN_Return, ref mTelco, "84");
                        if (mTelco == MyConfig.Telco.Vinaphone)
                            return MSISDN_Return;
                    }
                }
                GetFrom += "MIN|";

                if (string.IsNullOrEmpty(remoteip))
                    remoteip = string.Empty;
                if (string.IsNullOrEmpty(msisdn))
                    msisdn = string.Empty;
                if (string.IsNullOrEmpty(xipaddress))
                    xipaddress = string.Empty;
                if (string.IsNullOrEmpty(xforwarded))
                    xforwarded = string.Empty;
                if (string.IsNullOrEmpty(xwapmsisdn))
                    xwapmsisdn = string.Empty;
                if (string.IsNullOrEmpty(userip))
                    userip = string.Empty;
                if (string.IsNullOrEmpty(service))
                    service = string.Empty;

                string[] arr_ip = xforwarded.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in arr_ip)
                {
                    MSISDN_Return = RequestVNP(msisdn, xipaddress, item.Trim(), xwapmsisdn, userip, remoteip, service);
                    if (string.IsNullOrEmpty(MSISDN_Return))
                        return MSISDN_Return;
                }

                return MSISDN_Return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                MyLogfile.WriteLogData("GET_MSISDN_VNP", "MSISDN_Return:" + MSISDN_Return + " || GetFrom:" + GetFrom);
            }
        }      

        private string RequestVNP(string msisdn, string xipaddress, string xforwarded, string xwapmsisdn, string userip, string remoteip, string service)
        {
            string Response = string.Empty;
            string URL = string.Empty;
            string MSISDN_Return = string.Empty;
            try
            {
                URL = MySetting.WapSetting.Charging_URLGetMSISDN_VNP + "?msisdn=" + msisdn + "&xipaddress=" + xipaddress + "&xforwarded=" + xforwarded + "&xwapmsisdn=" + xwapmsisdn + "&userip=" + userip + "&remoteip=" + remoteip + "&service=" + service;

                Response = MyFile.ReadContentFromURL(URL);

                if (string.IsNullOrEmpty(Response))
                    MSISDN_Return = string.Empty;
                else
                {
                    string[] arr = Response.Split('|');
                    if (arr.Length > 2)
                        MSISDN_Return = arr[1];
                    MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                    MyCheck.CheckPhoneNumber(ref MSISDN_Return, ref mTelco, "84");
                    if (mTelco != MyConfig.Telco.Vinaphone)
                        MSISDN_Return = string.Empty;
                }


            }
            catch (Exception ex)
            {
                MyLogfile.WriteLogError("Error_MIN_VNP", ex);
            }
            finally
            {
                MyLogfile.WriteLogData("GET_MSISDN_VNP", "URL_GET:" + URL + " || Response:" + Response + " || MSISDN_Return:" + MSISDN_Return + " || GetFrom:MIN ");
            }
            return MSISDN_Return;
        }
    }
}
