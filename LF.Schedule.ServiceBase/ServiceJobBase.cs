using System;

namespace LF.Schedule.ServiceBase
{
    public abstract class ServiceJobBase:IDisposable
    {
        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <returns></returns>
        public RunningStateEnum Init()
        {
            return RunningStateEnum.Normal;
        }

        /// <summary>
        /// 提供重写初始化程序
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool OnInit(out string message)
        {
            message = string.Empty;
            return true;
        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <returns></returns>
        public RunningStateEnum Execute()
        {
            return RunningStateEnum.Normal;
        }


        /// <summary>
        /// 提供重写执行
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool OnExecute(out string message)
        {
            message = string.Empty;
            return true;
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }

        protected virtual void OnDispose()
        {
            
        }
    }

    /// <summary>
    /// 服务运行状态
    /// </summary>
    public enum RunningStateEnum
    {
        /// <summary>
        /// 运行正常
        /// </summary>
        Normal,

        /// <summary>
        /// 运行错误
        /// </summary>
        Failed,

        /// <summary>
        /// 运行异常
        /// </summary>
        Exception
    }
}
