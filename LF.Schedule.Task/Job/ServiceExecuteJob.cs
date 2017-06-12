using System;
using Common.Logging;
using LF.Schedule.Contract;
using LF.Schedule.ServiceBase;
using Quartz;

namespace LF.Schedule.Task.Job
{
    public class ServiceExecuteJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceHelper));

        public void Execute(IJobExecutionContext context)
        {
            var serviceKey = context.JobDetail.JobDataMap["ServiceKey"] as string;
            try
            {
                var serviceStateInfo = ServiceHelper.ServiceStateInfoByServuceKey[serviceKey];
                if (serviceStateInfo.ServiceState != ServiceStateEnum.Normal && serviceStateInfo.ServiceState != ServiceStateEnum.Failed)
                    return;

               string message;
               var executeState=  ServiceHelper.ServiceJobBaseByServiceKey[serviceKey].Execute(out message);

                if (executeState == ExecuteStateEnum.Normal)
                {
                    _log.InfoFormat("服务 [{0}] 业务执行成功", serviceStateInfo.ServiceName);
                }

                if (executeState == ExecuteStateEnum.Failed)
                {
                    _log.InfoFormat("服务 [{0}] 业务执行失败", serviceStateInfo.ServiceName);
                    ServiceInit.ChangeServiceStateInfo(new ServiceStateInfo()
                    {
                        ServiceState = ServiceStateEnum.Failed,
                        ExcuteDescription = message
                    });
                }

                if (executeState == ExecuteStateEnum.Exception)
                    throw new Exception(message);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("服务 [{0}] 执行异常", ex, ServiceHelper.ConfigurationByserviceKey[serviceKey].ServiceName);

                ServiceInit.ChangeServiceStateInfo(new ServiceStateInfo()
                {
                    ServiceState = ServiceStateEnum.Exception,
                    ExcuteDescription = ex.Message,
                    ServiceStopTime = DateTime.Now
                });
                ServiceInit.ResetStartServiceByServiceKey(serviceKey);
            }
        }
    }
}
