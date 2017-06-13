using System;
using System.Collections.Generic;
using System.Linq;

namespace LF.Schedule.Manage
{
    internal class ManageHelper
    {
        private static readonly ServiceClient ServiceClient = new ServiceClient();

        public static StateListResult GetStateList()
        {
            var stateListResult=  ServiceClient.HttpGet<StateListResult>(ConfigSetting.DataServiceUrl);
            for (var i = 0; i < stateListResult.ServiceStateList.Count(); i++)
            {
                if (!string.IsNullOrEmpty(Convert.ToString(stateListResult.ServiceStateList[i].ServiceStopTime)))
                    stateListResult.ServiceStateList[i].ServiceStopTime = Convert
                        .ToDateTime(stateListResult.ServiceStateList[i].ServiceStopTime).ToLocalTime();
                if (!string.IsNullOrEmpty(Convert.ToString(stateListResult.ServiceStateList[i].ServiceStartTime)))
                    stateListResult.ServiceStateList[i].ServiceStartTime = Convert
                        .ToDateTime(stateListResult.ServiceStateList[i].ServiceStartTime).ToLocalTime();
                switch (stateListResult.ServiceStateList[i].ServiceState)
                {
                    case ServiceStateEnum.Uninstall:
                        stateListResult.ServiceStateList[i].ServiceStateName = "已卸载";
                        break;
                    case ServiceStateEnum.Stopped:
                        stateListResult.ServiceStateList[i].ServiceStateName = "服务停止";
                        break;
                    case ServiceStateEnum.Exception:
                        stateListResult.ServiceStateList[i].ServiceStateName = "服务异常";
                        break;
                    case ServiceStateEnum.Failed:
                        stateListResult.ServiceStateList[i].ServiceStateName = "服务失败";
                        break;
                    case ServiceStateEnum.Normal:
                        stateListResult.ServiceStateList[i].ServiceStateName = "服务正常";
                        break;
                    default:
                        stateListResult.ServiceStateList[i].ServiceStateName = "未知状态";
                        break;
                }
            }
            return stateListResult;
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
