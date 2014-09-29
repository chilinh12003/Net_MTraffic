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
    public class sms_receive_queue
    {
        MySQLExecuteData mExec;
        MySQLGetData mGet;
        public sms_receive_queue()
        {
            mExec = new MySQLExecuteData();
            mGet = new MySQLGetData();
        }

        public sms_receive_queue(string KeyConnect_InConfig)
        {
            mExec = new MySQLExecuteData(KeyConnect_InConfig);
            mGet = new MySQLGetData(KeyConnect_InConfig);
        }

        public bool Insert(string USER_ID, string SERVICE_ID, string COMMAND_CODE, string INFO, string REQUEST_ID, MyConfig.ChannelType CHANNEL_TYPE)
        {
            try
            {
                string MOBILE_OPERATOR = "GPC";              

                string Format_Query = "INSERT INTO sms_receive_queue(user_id,service_id,mobile_operator,info,command_code,REQUEST_ID, CHANNEL_TYPE) "+
                                        "VALUE ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";

                string[] arr_value = { USER_ID, SERVICE_ID, MOBILE_OPERATOR,INFO, COMMAND_CODE, 
					                    REQUEST_ID, ((int)CHANNEL_TYPE).ToString()};

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
