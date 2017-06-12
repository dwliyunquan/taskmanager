using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using LF.Schedule.Contract;
using LF.Schedule.ServiceBase;
using LF.Schedule.Task.Contract;
using LF.Schedule.Task.Job;
using Quartz;
using Quartz.Impl;

namespace LF.Schedule.Task
{
    internal class ServiceInit
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServiceInit));

        private static readonly Dictionary<string, AppDomain> ServiceDomainByServiceKey = new Dictionary<string, AppDomain>();

        private static readonly Dictionary<string, ServiceJobBase> ServiceJobBaseByServiceKey = new Dictionary<string, ServiceJobBase>();

        private static readonly Dictionary<string,ServiceStateInfo>ServiceStateInfoByServuceKey=new Dictionary<string, ServiceStateInfo>();

        private  static IScheduler _serviceScheduler;

        private static readonly Dictionary<string, JobKey> ServiceJobKeyByServiceKey
            = new Dictionary<string, JobKey>();


        private static readonly ServiceConfig ServiceConfig=new ServiceConfig();


        public Dictionary<string, ServiceJobBase> ServiceJobBase => ServiceJobBaseByServiceKey;

        public Dictionary<string, ServiceStateInfo> ServiceStateInfo => ServiceStateInfoByServuceKey;


        public ServiceInit()
        {
            CreateServiceStateInfo();
            //CreateAppDomain();
            //CreateService();
        }

        /// <summary>
        /// 创建应用域
        /// </summary>
        private static void CreateAppDomain()
        {
            var configurationByServiceKey = ServiceConfig.ServiceConfiguration;
            foreach (var serviceKey in configurationByServiceKey.Keys.ToArray())
            {
                CreateAppDomainByServiceKey(serviceKey);
            }
        }

        /// <summary>
        /// 创建应用域
        /// </summary>
        /// <param name="serviceKey"></param>
        /// <param name="serviceConfiguration"></param>
        private static void CreateAppDomainByServiceKey(string serviceKey)
        {
            if (ServiceDomainByServiceKey.ContainsKey(serviceKey))
                return;

            var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
            if (serviceStateInfo.ServiceState != ServiceStateEnum.Uninstall)
                return;

            var serviceConfiguration = ServiceConfig.ServiceConfiguration[serviceKey];
            var domainSetup = new AppDomainSetup
            {
                ApplicationName = $"{serviceConfiguration.ServiceName}-{serviceKey}",
                ApplicationBase = Path.GetDirectoryName(serviceConfiguration.ConfigFile),
                PrivateBinPath = Path.GetDirectoryName(serviceConfiguration.ConfigFile),
                ConfigurationFile = serviceConfiguration.ConfigFile
            };

            var serviceDomain =AppDomain.CreateDomain(domainSetup.ApplicationName, null, domainSetup);
            ServiceDomainByServiceKey[serviceKey] = serviceDomain;
            Log.InfoFormat("成功初始化服务 [{0}] AppDomain [serviceKey: {1}]", serviceConfiguration.ServiceName, serviceKey);
        }

        /// <summary>
        /// 创建托管服务
        /// </summary>
        private static void CreateService()
        {
            foreach (var serviceKey in ServiceDomainByServiceKey.Keys.ToArray())
                CreateServiceByServiceKey(serviceKey);
        }

        /// <summary>
        /// 根据SrviceKey创建托管服务
        /// </summary>
        /// <param name="serviceKey"></param>
        private static void CreateServiceByServiceKey(string serviceKey)
        {
            if (ServiceJobBaseByServiceKey.ContainsKey(serviceKey))
                return;

            var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
            if(serviceStateInfo.ServiceState!=ServiceStateEnum.Uninstall)
                return;
            var configurationByServiceKey = ServiceConfig.ServiceConfiguration;
            try
            {
                var serviceClass = configurationByServiceKey[serviceKey].ServiceClass.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var serviceJobBase = ServiceDomainByServiceKey[serviceKey].CreateInstanceAndUnwrap(serviceClass[1].Trim(), serviceClass[0].Trim()) as ServiceJobBase;
                ServiceJobBaseByServiceKey[serviceKey] = serviceJobBase;

                string message;
                ServiceStateInfoByServuceKey[serviceKey].ServiceStartTime = DateTime.Now;
                var executeState = serviceJobBase.Init(out message);
                if (executeState == ExecuteStateEnum.Normal || executeState == ExecuteStateEnum.Failed)
                {
                    ServiceStateInfoByServuceKey[serviceKey].ExcuteDescription = message;
                    ServiceStateInfoByServuceKey[serviceKey].ServiceStopTime = null;
                    ServiceStateInfoByServuceKey[serviceKey].ServiceState = ServiceStateEnum.Normal;

                    Log.InfoFormat("服务 [{0}] 初始化完成 [serviceKey: {1}]", configurationByServiceKey.ContainsKey(serviceKey) ? configurationByServiceKey[serviceKey].ServiceName : string.Empty, serviceKey);
                }

                if (executeState == ExecuteStateEnum.Exception)
                    throw new System.Exception(message);
            }
            catch (Exception ex)
            {
                ServiceStateInfoByServuceKey[serviceKey].ExcuteDescription = ex.Message;
                ServiceStateInfoByServuceKey[serviceKey].ServiceStopTime = System.DateTime.Now;
                ServiceStateInfoByServuceKey[serviceKey].ServiceState = ServiceStateEnum.Exception;

                Log.WarnFormat("服务 [{0}] 初始化异常 [serviceKey: {1}]", ex, configurationByServiceKey.ContainsKey(serviceKey) ? configurationByServiceKey[serviceKey].ServiceName : string.Empty, serviceKey);
            }
            
        }

        /// <summary>
        /// 创建状态信息
        /// </summary>
        private static void CreateServiceStateInfo()
        {
            var configurationByServiceKey = ServiceConfig.ServiceConfiguration;
            foreach (var serviceKey in configurationByServiceKey.Keys.ToArray())
            {
                CreateServiceStateInfoByServiceKey(serviceKey);
            }
        }

        /// <summary>
        /// 根据serviceKey创建状态信息
        /// </summary>
        /// <param name="serviceKey"></param>
        /// <param name="serviceConfiguration"></param>
        private static void CreateServiceStateInfoByServiceKey(string serviceKey)
        {
            var serviceConfiguration = ServiceConfig.ServiceConfiguration[serviceKey];
            if (ServiceStateInfoByServuceKey.ContainsKey(serviceKey))
                return;
            ServiceStateInfoByServuceKey[serviceKey] = new ServiceStateInfo()
            {
                ServiceKey = serviceKey,
                Description = serviceConfiguration.Description,
                ServiceName = serviceConfiguration.ServiceName,
                ServiceState = ServiceStateEnum.Uninstall
            };
        }

        /// <summary>
        /// 创建计划任务
        /// </summary>
        private static void ExecuteService()
        {
            if (_serviceScheduler == null)
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                _serviceScheduler = schedulerFactory.GetScheduler();
            }

            foreach (var serviceKey in ServiceJobBaseByServiceKey.Keys.ToArray())
                ExecuteServiceByServiceKey(serviceKey);

            var serviceMonitorJob = JobBuilder.Create<ServiceMonitorJob>()
                .WithIdentity("ServiceMonitorJob")
                .Build();

            var serviceMonitorTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("ServiceMonitorJob")
                .StartAt(DateBuilder.FutureDate(1, IntervalUnit.Second))
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                .Build();

            _serviceScheduler.ScheduleJob(serviceMonitorJob, serviceMonitorTrigger);
            _serviceScheduler.Start();
        }

        /// <summary>
        /// 根据serviceKey创建计划任务
        /// </summary>
        /// <param name="serviceKey"></param>
        private static void ExecuteServiceByServiceKey(string serviceKey)
        {
            var configuration =ServiceConfig.ServiceConfiguration[serviceKey];

            if (!ServiceJobBaseByServiceKey.ContainsKey(serviceKey))
                return;

            try
            {
                var serviceExecuteJob = JobBuilder.Create<ServiceExecuteJob>()
                    .WithIdentity(serviceKey, "ServiceExecuteJobGroup")
                    .UsingJobData("ServiceKey", serviceKey)
                    .Build();

                ITrigger serviceTrigger = null;
                switch (configuration.CycleExecuteMode)
                {
                    case CycleExecuteModeEnum.OnlyOnce:
                    {
                        serviceTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                            .WithIdentity(serviceKey, "ServiceExecuteJobGroup")
                            .UsingJobData("ServiceKey", serviceKey)
                            .StartAt(DateBuilder.FutureDate(3, IntervalUnit.Second))
                            .Build();
                    }
                        break;

                    case CycleExecuteModeEnum.CalendarExecute:
                    {
                        serviceTrigger = (ICronTrigger)TriggerBuilder.Create()
                            .WithIdentity(serviceKey, "ServiceExecuteJobGroup")
                            .UsingJobData("ServiceKey", serviceKey)
                            .WithCronSchedule(configuration.CalendarExecuteExpression)
                            .Build();
                    }
                        break;

                    default:
                    {
                        serviceTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                            .WithIdentity(serviceKey, "ServiceExecuteJobGroup")
                            .UsingJobData("ServiceKey", serviceKey)
                            .StartAt(DateBuilder.FutureDate(3, IntervalUnit.Second))
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(configuration.CycleExecuteInterval).RepeatForever())
                            .Build();
                    }
                        break;
                }

                _serviceScheduler.ScheduleJob(serviceExecuteJob, serviceTrigger);
                ServiceJobKeyByServiceKey[serviceKey] = serviceExecuteJob.Key;

                Log.InfoFormat("服务 [{0}] ScheduleJob [{1} - {2} - {3}] 启动成功 [ServiceID: {4}]",
                    configuration.ServiceName, configuration.CycleExecuteMode, configuration.CycleExecuteInterval,
                    configuration.CalendarExecuteExpression, serviceKey);
            }
            catch (Exception ex)
            {
                ServiceStateInfoByServuceKey[serviceKey].ServiceStopTime = DateTime.Now;
                ServiceStateInfoByServuceKey[serviceKey].ServiceState = ServiceStateEnum.Exception;
                ServiceStateInfoByServuceKey[serviceKey].ServiceStartTime = null;
                Log.WarnFormat("服务 [{0}] ScheduleJob [{1} - {2} - {3}] 启动异常 [ServiceID: {4}]", ex,configuration.ServiceName,
                    configuration.CycleExecuteMode, configuration.CycleExecuteInterval, configuration.CalendarExecuteExpression, serviceKey);
                UninstallServiceDomainByserviceKey(serviceKey);
            }
        }

        /// <summary>
        /// 卸载服务
        /// </summary>
        /// <param name="serviceKey"></param>
        private static void UninstallServiceDomainByserviceKey(string serviceKey)
        {
            var configuration = ServiceConfig.ServiceConfiguration[serviceKey];
            if (ServiceJobKeyByServiceKey.ContainsKey(serviceKey))
            {
                try
                {
                    _serviceScheduler.DeleteJob(ServiceJobKeyByServiceKey[serviceKey]);
                    ServiceJobKeyByServiceKey.Remove(serviceKey);
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("服务 [{0}] ServiceJob 卸载异常", ex, configuration.ServiceName);
                    throw;
                }
            }

            if (ServiceJobBaseByServiceKey.ContainsKey(serviceKey))
            {
                try
                {
                    ServiceJobBaseByServiceKey[serviceKey].Dispose();
                }
                catch(Exception ex)
                {
                    Log.ErrorFormat("服务 [{0}] ServiceJobBase 卸载异常", ex, configuration.ServiceName);
                    throw;
                }

                ServiceJobBaseByServiceKey.Remove(serviceKey);
            }

            
            if (ServiceDomainByServiceKey.ContainsKey(serviceKey))
            {
                try
                {
                    AppDomain.Unload(ServiceDomainByServiceKey[serviceKey]);
                }
                catch(Exception ex)
                {
                    Log.ErrorFormat("服务 [{0}] AppDomain 卸载异常", ex, configuration.ServiceName);
                    throw;
                }

                ServiceDomainByServiceKey.Remove(serviceKey);

                Log.InfoFormat("成功卸载服务 [{0}] AppDomain [serviceKey: {1}]", configuration.ServiceName, serviceKey);
            }
        }

        /// <summary>
        /// 启动监听
        /// </summary>
        public static void OnStart()
        {
            ServiceConfig.LoadServiceConfigs();
            CreateServiceStateInfo();
            //CreateAppDomain();
            //CreateService();
            ExecuteService();
            ManageServiceHelper.StartService();
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public static void OnStop()
        {
            _serviceScheduler.Shutdown(true);
            _serviceScheduler = null;

            foreach (var serviceKey in ServiceJobBaseByServiceKey.Keys.ToArray())
            {
                try
                {
                    ServiceJobBaseByServiceKey[serviceKey].Dispose();

                    AppDomain.Unload(ServiceDomainByServiceKey[serviceKey]);

                    ServiceJobBaseByServiceKey.Clear();
                    ServiceDomainByServiceKey.Clear();
                    ServiceStateInfoByServuceKey.Clear();
                    ManageServiceHelper.StopService();
                }
                catch(Exception ex)
                {
                    var configuration = ServiceConfig.ServiceConfiguration[serviceKey];
                    Log.ErrorFormat("服务停止异常 [{0}]  [serviceKey: {1}]", ex,configuration.ServiceName, serviceKey);
                }
            }
        }

        #region 服务指令操作

        internal static SendCommandResult StartServiceByServiceKey(string serviceKey)
        {
            var sendCommandResult=new SendCommandResult();
            try
            {
                var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
                CreateAppDomainByServiceKey(serviceKey);
                CreateServiceByServiceKey(serviceKey);
                ExecuteServiceByServiceKey(serviceKey);
                serviceStateInfo.ServiceState = ServiceStateEnum.Normal;
                sendCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"启动服务异常:{ex.Message}";
            }
            return sendCommandResult;
        }

        internal static SendCommandResult StopServiceByServiceKey(string serviceKey)
        {
            var sendCommandResult = new SendCommandResult();
            try
            {
                var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
                serviceStateInfo.ServiceState = ServiceStateEnum.Stopped;
                sendCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"停止服务异常:{ex.Message}";
            }
            return sendCommandResult;


        }

        internal static SendCommandResult InstallServiceByServiceKey(string serviceKey)
        {
            var sendCommandResult = new SendCommandResult();
            try
            {
                CreateAppDomainByServiceKey(serviceKey);
                CreateServiceByServiceKey(serviceKey);

                var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
                serviceStateInfo.ServiceState = ServiceStateEnum.Stopped;
                sendCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"安装服务异常:{ex.Message}";
            }
            return sendCommandResult;
        }

        internal static SendCommandResult UninstallServiceByServiceKey(string serviceKey)
        {
            var sendCommandResult = new SendCommandResult();
            try
            {
                UninstallServiceDomainByserviceKey(serviceKey);
                var serviceStateInfo = ServiceStateInfoByServuceKey[serviceKey];
                serviceStateInfo.ServiceState = ServiceStateEnum.Uninstall;
                sendCommandResult.Success = true;
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"卸载服务异常:{ex.Message}";
            }
            return sendCommandResult;
        }

        internal static SendCommandResult ResetStartServiceByServiceKey(string serviceKey)
        {
            var sendCommandResult = new SendCommandResult();
            try
            {
                UninstallServiceDomainByserviceKey(serviceKey);
                InstallServiceByServiceKey(serviceKey);
                StartServiceByServiceKey(serviceKey);
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"重启服务异常:{ex.Message}";
            }
            return sendCommandResult;
        }

        /// <summary>
        /// 加载服务
        /// </summary>
        internal static SendCommandResult LoadService()
        {
            var sendCommandResult = new SendCommandResult();
            try
            {
                ServiceConfig.LoadServiceConfigs();
                CreateServiceStateInfo();
            }
            catch (Exception ex)
            {
                sendCommandResult.Success = true;
                sendCommandResult.Message = $"加载服务异常:{ex.Message}";
            }
            return sendCommandResult;

        }

        #endregion

        public static void ChangeServiceStateInfo(ServiceStateInfo serviceStateInfo)
        {
            if (string.IsNullOrEmpty(serviceStateInfo?.ServiceKey))
                return;
            if (ServiceStateInfoByServuceKey[serviceStateInfo.ServiceKey] == null)
                return;
            ServiceStateInfoByServuceKey[serviceStateInfo.ServiceKey].ServiceState = serviceStateInfo.ServiceState;
            if (!string.IsNullOrEmpty(serviceStateInfo.ExcuteDescription))
                ServiceStateInfoByServuceKey[serviceStateInfo.ServiceKey].ExcuteDescription =
                    serviceStateInfo.ExcuteDescription;

            if (null != serviceStateInfo.ServiceStopTime)
                ServiceStateInfoByServuceKey[serviceStateInfo.ServiceKey].ServiceStopTime = DateTime.Now;

        }
    }
}
