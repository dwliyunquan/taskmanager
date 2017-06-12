using System.ServiceModel;
using System.ServiceModel.Web;

namespace LF.Schedule.Contract
{
    [ServiceContract]
    public interface IManageService
    {
        /// <summary>
        /// 获取服务状态
        /// </summary>
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        StateListResult GetStateList();

        /// <summary>
        /// 发送服务操作指令
        /// </summary>
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest,ResponseFormat = WebMessageFormat.Json)]
        SendCommandResult SendCommand(SendCommandParam sendCommandParam);
    }
}
