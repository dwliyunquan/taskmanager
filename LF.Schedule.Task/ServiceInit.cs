using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LF.Schedule.ServiceBase;

namespace LF.Schedule.Task
{
    public class ServiceInit
    {
        private static Dictionary<string, StateInfo> _stateInfoByServiceKey = null;

        private static Dictionary<string, AppDomain> _serviceDomainByServiceKey = null;

        private static Dictionary<string, ServiceJobBase> _serviceJobBaseByServiceKey = null;

        private static readonly ServiceConfig ServiceConfig=new ServiceConfig();



        public ServiceInit()
        {
            if(null==_stateInfoByServiceKey)
                _stateInfoByServiceKey=new Dictionary<string, StateInfo>();
            if(null==_serviceDomainByServiceKey)
                _serviceDomainByServiceKey=new Dictionary<string, AppDomain>();
            if(null==_serviceJobBaseByServiceKey)
                _serviceJobBaseByServiceKey=new Dictionary<string, ServiceJobBase>();
            CreateAppDomain();
            CreateService();
        }

        private void CreateAppDomain()
        {
            var configurationByServiceKey = ServiceConfig.ServiceConfiguration;
            foreach (var serviceKey in configurationByServiceKey.Keys.ToArray())
            {
                if (!configurationByServiceKey[serviceKey].IsAddInServiceInstalled)
                    continue;

                CreateAppDomainByServiceKey(serviceKey, configurationByServiceKey[serviceKey]);
            }
        }

        private void CreateAppDomainByServiceKey(string serviceKey,ServiceConfiguration serviceConfiguration)
        {
            if (!serviceConfiguration.IsAddInServiceInstalled)
                return;

            var domainSetup = new AppDomainSetup
            {
                ApplicationName = $"{serviceConfiguration.ServiceName}-{serviceKey}",
                ApplicationBase = Path.GetDirectoryName(serviceConfiguration.ConfigFile),
                PrivateBinPath = Path.GetDirectoryName(serviceConfiguration.ConfigFile),
                ConfigurationFile = serviceConfiguration.ConfigFile
            };

            var serviceDomain =AppDomain.CreateDomain(domainSetup.ApplicationName, null, domainSetup);
            _serviceDomainByServiceKey[serviceKey] = serviceDomain;
        }

        private void CreateService()
        {
            foreach (var serviceKey in _serviceDomainByServiceKey.Keys.ToArray())
                CreateServiceByServiceKey(serviceKey);
        }


        private void CreateServiceByServiceKey(string serviceKey)
        {
            if (_serviceJobBaseByServiceKey.ContainsKey(serviceKey))
                return;
            var configurationByServiceKey = ServiceConfig.ServiceConfiguration;
            try
            {
                var serviceClass = configurationByServiceKey[serviceKey].ServiceClass.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var addInService = _serviceDomainByServiceKey[serviceKey].CreateInstanceAndUnwrap(serviceClass[1].Trim(), serviceClass[0].Trim()) as ServiceJobBase;
                _serviceJobBaseByServiceKey[serviceKey] = addInService;
            }
            catch (Exception ex)
            {
                Console.WriteLine("初始化异常："+ex.Message);
            }
            
        }

    }

    public class StateInfo
    {
        
    }
}
