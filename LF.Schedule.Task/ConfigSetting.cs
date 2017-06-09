using System;
using System.IO;

namespace LF.Schedule.Task
{
    internal class ConfigSetting
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public static string ServiceLocation => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceTask");

    }
}
