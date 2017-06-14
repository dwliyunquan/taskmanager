using System;
using System.Diagnostics;
using System.IO;
using LF.Schedule.Configuration;

namespace LF.Schedule.Install
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var writer = new StreamWriter("install.bat", false))
            {
                var installutilfile = string.Empty;
                if (Directory.Exists(ConfigurationHelper.FrameworkInstallPath))
                    installutilfile = Path.Combine(ConfigurationHelper.FrameworkInstallPath, "InstallUtil.exe");
                 if (File.Exists(Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe")))
                    installutilfile = Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe");
                else if(File.Exists(Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe")))
                    installutilfile = Path.Combine(Environment.SystemDirectory, @"..\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe");

                if (string.IsNullOrEmpty(installutilfile))
                    throw new Exception("未安装.Net Framework...");

                writer.WriteLine("@echo off");
                writer.WriteLine("@echo 安装服务");
                writer.WriteLine($"set svc_file=\"%cd%\\{ConfigurationHelper.ServiceFile}\"");
                writer.WriteLine($"sc create \"{ConfigurationHelper.ServiceName}\" binpath= %svc_file% displayName= \"{ConfigurationHelper.ServiceName}\"");
                writer.WriteLine($"net start {ConfigurationHelper.ServiceName}");
                writer.WriteLine("pause");
                writer.WriteLine("del install.bat");
                writer.WriteLine("@exit");
            }

            Process.Start("install.bat");
        }
    }
}
