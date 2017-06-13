using System.Configuration;

namespace LF.Schedule.Manage
{
    internal class ConfigSetting
    {
        public static string DataServiceUrl => $"{ConfigurationManager.AppSettings["ServiceUrl"]}/GetStateList";

        public static string CommandServiceUrl=> $"{ConfigurationManager.AppSettings["ServiceUrl"]}/SendCommand";
    }


}
