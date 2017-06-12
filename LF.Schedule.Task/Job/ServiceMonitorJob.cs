using System;
using System.Linq;
using Common.Logging;
using LF.Schedule.Contract;
using LF.Schedule.ServiceBase;
using Quartz;

namespace LF.Schedule.Task.Job
{
    public class ServiceMonitorJob:IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ServiceHelper));
        public void Execute(IJobExecutionContext context)
        {
            var configurationByserviceKey = ServiceHelper.ConfigurationByserviceKey;
            foreach (var serviceKey in configurationByserviceKey.Keys.ToArray())
            {
                try
                {

                    var serviceStateInfo = ServiceHelper.ServiceStateInfoByServuceKey[serviceKey];
                    if(serviceStateInfo.ServiceState!= ServiceStateEnum.Normal && serviceStateInfo.ServiceState != ServiceStateEnum.Failed)
                        return;

                    string msg;
                    var executeState = ServiceHelper.ServiceJobBaseByServiceKey[serviceKey].ActiveTest(out msg);
                    if (executeState == ExecuteStateEnum.Normal)
                    {
                        _log.InfoFormat("服务 [{0}] 心跳测试成功",configurationByserviceKey[serviceKey].ServiceName);
                        ServiceInit.ChangeServiceStateInfo(new ServiceStateInfo()
                        {
                            ServiceState = ServiceStateEnum.Normal
                        });
                    }

                    if (executeState == ExecuteStateEnum.Failed)
                    {
                        _log.InfoFormat("服务 [{0}] 心跳测试失败", configurationByserviceKey[serviceKey].ServiceName);
                        ServiceInit.ChangeServiceStateInfo(new ServiceStateInfo()
                        {
                            ServiceState = ServiceStateEnum.Failed
                        });
                    }

                    if (executeState == ExecuteStateEnum.Exception)
                        throw new Exception(msg);
                }
                catch (Exception ex)
                {

                    _log.ErrorFormat("服务 [{0}] 心跳测试异常", ex, configurationByserviceKey[serviceKey].ServiceName);
                    ServiceInit.ChangeServiceStateInfo(new ServiceStateInfo()
                    {
                        ExcuteDescription = ex.Message,
                        ServiceState = ServiceStateEnum.Exception
                    });
                    ServiceInit.UninstallServiceByServiceKey(serviceKey);
                }
                finally
                {
                    if (ServiceHelper.ServiceStateInfoByServuceKey[serviceKey].ServiceState ==
                        ServiceStateEnum.Exception)
                    {
                        ServiceInit.ResetStartServiceByServiceKey(serviceKey);
                    }
                }
            }
        }
    }
}
