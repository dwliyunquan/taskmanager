using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using LF.Schedule.ServiceBase;

namespace LF.Schedule.Task
{
    internal class ServiceConfig
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ServiceConfig));

        private static readonly Dictionary<string, string> ServiceKeyByconfigFile = new Dictionary<string, string>();

        private static readonly Dictionary<string, ServiceConfiguration> ConfigurationByserviceKey = new Dictionary<string, ServiceConfiguration>();

        public Dictionary<string, ServiceConfiguration> ServiceConfiguration => ConfigurationByserviceKey;

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public ServiceConfig()
        {
            LoadServiceConfigs();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        public static void LoadServiceConfigs()
        {
            foreach (var configFile in ServiceKeyByconfigFile.Keys.ToArray())
            {
                var serviceId = ServiceKeyByconfigFile[configFile];
                if (File.Exists(configFile)) continue;

                ServiceKeyByconfigFile.Remove(configFile);

                if (ConfigurationByserviceKey.ContainsKey(serviceId))
                    ConfigurationByserviceKey.Remove(serviceId);
            }

            if (!Directory.Exists(ConfigSetting.ServiceLocation)) return;

            foreach (var configFile in Directory.GetFiles(ConfigSetting.ServiceLocation, "ServiceJob.config",
                SearchOption.AllDirectories))
            {
                if (ServiceKeyByconfigFile.ContainsKey(configFile)) continue;

                var serviceKey = Guid.NewGuid().ToString("N");
                ServiceKeyByconfigFile[configFile] = serviceKey;

                var serviceConfiguration = new ServiceConfiguration(configFile);
                ConfigurationByserviceKey[serviceKey] = serviceConfiguration;
                Log.InfoFormat("成功加载服务 [{0}] 配置信息 [serviceKey: {1}]", serviceConfiguration.ServiceName, serviceKey);
            }
        }
    }


}
