using System;
using System.Linq;
using Common.Logging;
using LF.Schedule.ServiceBase;
using Quartz;

namespace LF.Schedule.Task.Job
{
    public class ServiceMonitorJob:IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceHelper));
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("执行成功");
            foreach (var serviceKey in ServiceHelper.ConfigurationByserviceKey.Keys.ToArray())
            {
                try
                {
                        var msg = string.Empty;
                        var executeState = ServiceHelper.ServiceJobBaseByServiceKey[serviceKey].ActiveTest(out msg);

                        if (executeState == ExecuteStateEnum.Normal)
                        {
                        }

                        if (executeState == ExecuteStateEnum.Failed)
                        {
                        }

                        if (executeState == ExecuteStateEnum.Exception)
                            throw new System.Exception(msg);
                }
                catch (Exception ex)
                {
                    //Log.ErrorFormat("服务 [{0}] 心跳测试异常", ex, WindowsServiceHelper.ServiceStateByServiceID[serviceKey].ServiceName);

                    //WindowsServiceHelper.ServiceStateByServiceID[serviceKey].StateDescription = ex.Message;
                    //WindowsServiceHelper.ServiceStateByServiceID[serviceKey].ServiceStopTime = System.DateTime.Now;
                    //WindowsServiceHelper.ServiceStateByServiceID[serviceKey].ServiceState = ServiceStateEnum.Exception;

                    //WindowsServiceHelper.UnloadAddInServiceDomainByServiceID(serviceKey);
                }
                finally
                {
                    //if (WindowsServiceHelper.ServiceStateByServiceID[serviceKey].ServiceState == ServiceStateEnum.Exception &&
                    //    WindowsServiceHelper.ServiceConfigurationByServiceID[serviceKey].AutoResetWhileException)
                    //{ WindowsServiceHelper.StartAddInServiceByServiceID(serviceKey); }
                }
            }
        }
    }
}
