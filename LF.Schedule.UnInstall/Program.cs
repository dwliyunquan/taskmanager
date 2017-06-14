using LF.Schedule.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LF.Schedule.UnInstall
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamWriter writer = new StreamWriter("uninstall.bat", false))
            {
                string installutilfile = string.Empty;
                if (Directory.Exists(ConfigurationHelper.FrameworkInstallPath))
                    installutilfile = Path.Combine(ConfigurationHelper.FrameworkInstallPath, "InstallUtil.exe");
                else if (File.Exists(Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe")))
                    installutilfile = Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe");
                else if (File.Exists(Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe")))
                    installutilfile = Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe");

                if (string.IsNullOrEmpty(installutilfile))
                    throw new Exception("未安装.Net Framework...");

                writer.WriteLine($"net stop {ConfigurationHelper.ServiceName}");
                writer.WriteLine($"sc delete {ConfigurationHelper.ServiceName}");
                writer.WriteLine("pause");
                writer.WriteLine("del uninstall.bat");
                writer.WriteLine("@exit");
            }

            Process.Start("uninstall.bat");
        }
    }
}
