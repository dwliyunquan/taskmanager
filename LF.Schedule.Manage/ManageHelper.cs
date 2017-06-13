using System;
using System.Collections.Generic;

namespace LF.Schedule.Manage
{
    internal class ManageHelper
    {
        private static readonly ServiceClient ServiceClient = new ServiceClient();

        public static StateListResult GetStateList()
        {
            return ServiceClient.HttpGet<StateListResult>(ConfigSetting.DataServiceUrl);
        }

        private static SendCommandResult SendCommand(Dictionary<string,object> sendCommandParm)
        {
            return ServiceClient.HttpPost<SendCommandResult>(ConfigSetting.CommandServiceUrl, sendCommandParm);
        }

        public static string SendCommand(string serviceKey, ServiceCommandEnum command)
        {
            var sendCommandParm=new Dictionary<string, object>();
            var dic = new Dictionary<string, object>
            {
                {"ServiceKey", serviceKey},
                {"Command", command}
            };
            sendCommandParm.Add("sendCommandParam",dic);
            try
            {
                var sendCommandResult = SendCommand(sendCommandParm);
                return sendCommandResult.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
