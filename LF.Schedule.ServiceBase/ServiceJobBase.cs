using System;
using System.IO;
using Common.Logging;

namespace LF.Schedule.ServiceBase
{
    [Serializable]
    public abstract class ServiceJobBase:MarshalByRefObject,IDisposable
    {
        private readonly ILog _log = LogManager.GetLogger("AddInService");

        private ServiceConfiguration _serviceConfiguration = null;
        public ServiceConfiguration ServiceConfiguration => _serviceConfiguration ?? (_serviceConfiguration =
                                                                new ServiceConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                    "ServiceJob.config")));



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


        public ExecuteStateEnum Init(out string message)
        {
            message = string.Empty;

            try
            {
                if (OnInit(out message))
                {
                    _log.InfoFormat("服务 [{0}] 初始化成功", ServiceConfiguration.ServiceName);
                    return ExecuteStateEnum.Normal;
                }

                _log.InfoFormat("服务 [{0}] 初始化失败", ServiceConfiguration.ServiceName);
                return ExecuteStateEnum.Failed;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                _log.ErrorFormat("服务 [{0}] 在初始化时发生异常", ex, ServiceConfiguration.ServiceName);
                return ExecuteStateEnum.Exception;
            }
        }

        /// <summary>
        /// 执行定时任务
        /// </summary>
        /// <returns></returns>
        public ExecuteStateEnum Execute(out string message)
        {
            try
            {
                if (OnExecute(out message))
                {
                    _log.InfoFormat("服务 [{0}] 执行业务成功", ServiceConfiguration.ServiceName);
                    return ExecuteStateEnum.Normal;
                }

                _log.InfoFormat("服务 [{0}] 执行业务失败", ServiceConfiguration.ServiceName);
                return ExecuteStateEnum.Failed;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                _log.ErrorFormat("服务 [{0}] 在执行业务时发生异常", ex, ServiceConfiguration.ServiceName);

                return ExecuteStateEnum.Exception;
            }
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

        public ExecuteStateEnum ActiveTest(out string msg)
        {
            msg = string.Empty;

            try
            {
                if (OnActiveTest(out msg))
                {
                    return ExecuteStateEnum.Normal;
                }
                return ExecuteStateEnum.Failed;
            }
            catch (Exception ex)
            {
                msg = ex.Message;

                return ExecuteStateEnum.Exception;
            }
        }

        protected virtual bool OnActiveTest(out string msg)
        {
            msg = string.Empty;
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
    public enum ExecuteStateEnum
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
