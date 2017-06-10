using System;
using Common.Logging;
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

                string msg;
                ServiceHelper.ServiceJobBaseByServiceKey[serviceKey].Execute(out msg);

            }
            catch (Exception ex)
            {
                _log.ErrorFormat("服务 [{0}] 执行异常", ex, ServiceHelper.ConfigurationByserviceKey[serviceKey].ServiceName);
            }
        }
    }
}
