using System.ServiceModel;

namespace LF.Schedule.Contract
{
    [ServiceContract]
    public interface IManagerService
    {
        /// <summary>
        /// 获取服务状态
        /// </summary>
        [OperationContract]
        StateListResult GetStateList();

        /// <summary>
        /// 发送服务操作指令
        /// </summary>
        [OperationContract]
        SendCommandResult SendCommand(SendCommandParam sendCommandParam);
    }
}
