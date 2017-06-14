using System.Collections.Specialized;
using System.Configuration;
using SXT.Configuration;

namespace LF.Schedule.Configuration
{
    public static class ConfigurationHelper
    {
        private static readonly DefaultConfiguration Configuration;

        static ConfigurationHelper()
        {
            Configuration = new DefaultConfiguration("ServiceJob.config", "ServiceJobConfig");
        }

        public static string ServiceFile => Configuration["ServiceFile"].Trim();

        public static string ServiceName => Configuration["ServiceName"].Trim();

        public static string FrameworkInstallPath => Configuration["FrameworkInstallPath"].Trim();
    }
}