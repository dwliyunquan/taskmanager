using System.Collections.Generic;
using LF.Schedule.Contract;
using LF.Schedule.ServiceBase;

namespace LF.Schedule.Task
{
    public static class ServiceHelper
    {
        private static readonly  ServiceInit ServiceInit=new ServiceInit();

        private static readonly ServiceConfig ServiceConfig=new ServiceConfig();


        public static Dictionary<string, ServiceJobBase> ServiceJobBaseByServiceKey => ServiceInit
            .ServiceJobBase;

        public static Dictionary<string, ServiceConfiguration> ConfigurationByserviceKey => ServiceConfig
            .ServiceConfiguration;

        public static Dictionary<string, ServiceStateInfo> ServiceStateInfoByServuceKey => ServiceInit.ServiceStateInfo;


        public static void OnStart()
        {
            ServiceInit.OnStart();
        }

        public static void OnStop()
        {
            ServiceInit.OnStop();
        }
    }
}
