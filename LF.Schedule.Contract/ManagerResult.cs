using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LF.Schedule.Contract
{
    public enum ServiceStateEnum
    {
        /// <summary>
        /// 运行正常
        /// </summary>
        Normal,

        /// <summary>
        /// 运行错误，但不影响程序正常运行
        /// </summary>
        Failed,

        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,

        /// <summary>
        /// 运行异常，影响程序正常运行
        /// </summary>
        Exception
    }

    public enum ServiceCommandEnum
    {
        /// <summary>
        /// 启动
        /// </summary>
        Start,

        /// <summary>
        /// 重新启动
        /// </summary>
        ResetStart,

        /// <summary>
        /// 停止
        /// </summary>
        Stop,

        /// <summary>
        /// 加载服务
        /// </summary>
        Load

    }

    public class ServiceStateInfo
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceKey { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 服务描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        public ServiceStateEnum ServiceState { get; set; }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string ExcuteDescription { get; set; }

        /// <summary>
        /// 服务开始时间
        /// </summary>
        public DateTime? ServiceStartTime { get; set; }

        /// <summary>
        /// 服务停止时间
        /// </summary>
        public DateTime? ServiceStopTime { get; set; }
    }

    public class StateListResult
    {
        public IEnumerable<ServiceStateInfo> ServiceStateList
        { get; set; }
    }

    public class SendCommandParam
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public string ServiceKey { get; set; }

        /// <summary>
        /// 服务指令
        /// </summary>
        [DataMember]
        public ServiceCommandEnum Command { get; set; }
    }

    [DataContract]
    public class SendCommandResult
    {
        /// <summary>
        /// 结果标识
        /// 0： 成功
        /// 1： 失败
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// 结果消息文本
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}
