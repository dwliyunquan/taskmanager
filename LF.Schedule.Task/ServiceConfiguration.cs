using SXT.Configuration;
using System;
using System.IO;

namespace LF.Schedule.Task
{
    internal class ServiceConfiguration
    {
        public DefaultConfiguration ChildServiceConfiguration = null;

        public ServiceConfiguration(string serviceConfigFile)
        {
            ConfigFile = serviceConfigFile;

            ChildServiceConfiguration = new DefaultConfiguration(new FileInfo(serviceConfigFile), "ServiceJob");
        }

        public string ConfigFile { get; private set; }

        public string ServiceClass => ChildServiceConfiguration["ServiceClass"].Trim();

        public string ServiceName => ChildServiceConfiguration["ServiceName"].Trim();

        public string Description => ChildServiceConfiguration["Description"].Trim();

        public bool IsAddInServiceInstalled => string.Compare(ChildServiceConfiguration["ServiceState"].Trim(), "Installed", StringComparison.OrdinalIgnoreCase) == 0;

        public CycleExecuteModeEnum CycleExecuteMode => (CycleExecuteModeEnum) Enum.Parse(typeof(CycleExecuteModeEnum),
            ChildServiceConfiguration["CycleExecuteMode"].Trim(), true);

        public int CycleExecuteInterval => int.Parse(ChildServiceConfiguration["CycleExecuteInterval"].Trim());

        public string CalendarExecuteExpression => ChildServiceConfiguration["CalendarExecuteExpression"].Trim();
    }

    public enum CycleExecuteModeEnum
    {
        OnlyOnce,

        CycleExecute,

        CalendarExecute
    }
}
