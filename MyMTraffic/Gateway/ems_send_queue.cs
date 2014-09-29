using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MyConnect.MySQL;
using MyUtility;
using System.Collections;
using System.ComponentModel;
using MyConnect.MySQL;

namespace MyMTraffic.Gateway
{
    public class ems_send_queue
    {
        MySQLExecuteData mExec;
        MySQLGetData mGet;
        public ems_send_queue()
        {
            mExec = new MySQLExecuteData();
            mGet = new MySQLGetData();
        }

        public ems_send_queue(string KeyConnect_InConfig)
        {
            mExec = new MySQLExecuteData(KeyConnect_InConfig);
            mGet = new MySQLGetData(KeyConnect_InConfig);
        }

        public bool Insert(string USER_ID, string SERVICE_ID, string COMMAND_CODE, string INFO, string REQUEST_ID)
        {
            try
            {
                string MOBILE_OPERATOR = "GPC";
                string PROCESS_RESULT = "0";
                string MESSAGE_TYPE = "2"; //2 la khong tru tien
                string MESSAGE_ID = "1";
                string CPID = "0";
                string CONTENT_TYPE = "21";

                string Format_Query = "INSERT INTO ems_send_queue( USER_ID, SERVICE_ID, MOBILE_OPERATOR, "
                        + "COMMAND_CODE, INFO, SUBMIT_DATE, DONE_DATE, PROCESS_RESULT, MESSAGE_TYPE, REQUEST_ID,"
                        + " MESSAGE_ID, CONTENT_TYPE,CPID) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}')";

                string strDoneDate = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);
                string strSubmitDate = DateTime.Now.ToString(MyConfig.DateFormat_InsertToDB);

                string[] arr_value = { USER_ID, SERVICE_ID, MOBILE_OPERATOR, COMMAND_CODE, INFO, strSubmitDate, strDoneDate, PROCESS_RESULT, MESSAGE_TYPE,
					                    REQUEST_ID, MESSAGE_ID, CONTENT_TYPE, CPID };

                string Query = string.Format(Format_Query, arr_value);

                if (mExec.ExecQuery(Query) > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
