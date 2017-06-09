using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LF.Schedule.Task
{
    internal class ServiceConfig
    {
        private static Dictionary<string, string> _serviceKeyByconfigFile = null;

        private static Dictionary<string, ServiceConfiguration> _configurationByserviceKey = null;

        public Dictionary<string, string> ServiceConfigFiles => _serviceKeyByconfigFile;

        public Dictionary<string, ServiceConfiguration> ServiceConfiguration => _configurationByserviceKey;

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public ServiceConfig()
        {
            if (_serviceKeyByconfigFile == null)
                _serviceKeyByconfigFile=new Dictionary<string, string>();
            if(_configurationByserviceKey==null)
                _configurationByserviceKey=new Dictionary<string, ServiceConfiguration>();

            LoadServiceConfigs();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private static void LoadServiceConfigs()
        {
            foreach (var configFile in _serviceKeyByconfigFile.Keys.ToArray())
            {
                var serviceId = _serviceKeyByconfigFile[configFile];
                if (File.Exists(configFile)) continue;

                _serviceKeyByconfigFile.Remove(configFile);

                if (_configurationByserviceKey.ContainsKey(serviceId))
                    _configurationByserviceKey.Remove(serviceId);
            }

            if (!Directory.Exists(ConfigSetting.ServiceLocation)) return;

            foreach (var configFile in Directory.GetFiles(ConfigSetting.ServiceLocation, "ServiceJob.config",
                SearchOption.AllDirectories))
            {
                if (_serviceKeyByconfigFile.ContainsKey(configFile)) continue;

                var serviceId = Guid.NewGuid().ToString("N");
                _serviceKeyByconfigFile[configFile] = serviceId;

                var serviceConfiguration = new ServiceConfiguration(configFile);
                _configurationByserviceKey[serviceId] = serviceConfiguration;
            }
        }
    }


}
