using System;
using System.IO;
using System.Text;
using LF.Schedule.ServiceBase;

namespace LF.Winservice.Test1
{
    public class Class1:ServiceJobBase
    {
        protected override bool OnExecute(out string msg)
        {
            msg = string.Empty;
            string appsetting = System.Configuration.ConfigurationManager.AppSettings["ServiceName"];
            Info("服务名称:" + appsetting);
            return true;
        }

        void Info(string message)
        {
            try
            {

                string folder = AppDomain.CurrentDomain.BaseDirectory + "/" + "TaskInfo/";
                Console.WriteLine(folder);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                StringBuilder sb = new StringBuilder();
                DateTime dt = DateTime.Now;
                sb.AppendLine(dt.ToString("yyyy-MM-dd HH:mm:ss ") + message);
                File.AppendAllText(folder + "/" + dt.ToString("yyyyMMddHH") + ".Info.log", sb.ToString());
            }
            catch (Exception ex)
            {
                return;
            }
        }
    }
}
