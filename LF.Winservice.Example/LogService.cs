using System;
using System.IO;
using System.Text;
using LF.Schedule.ServiceBase;

namespace LF.Winservice.Example
{
    public class LogService : ServiceJobBase
    {
        private static readonly object Obj=new object();
        protected override bool OnExecute(out string message)
        {
            lock (Obj)
            {
                WriteInfo("这是我要测试的日志");
                message = "返回日志";
                return true;
            }
        }

        protected override bool OnActiveTest(out string msg)
        {
            Console.WriteLine("自定义心跳测试日志");
            msg = null;
            return true;
        }


        private void WriteInfo(string message)
        {
            try
            {

                var folder = AppDomain.CurrentDomain.BaseDirectory + "/" + "TaskInfo/";
                Console.WriteLine(folder);
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                var sb = new StringBuilder();
                var dt = DateTime.Now;
                sb.AppendLine(dt.ToString("yyyy-MM-dd HH:mm:ss ") + message);
                File.AppendAllText(folder + "/" + dt.ToString("yyyyMMddHH") + ".Info.log", sb.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"日志写入异常:{ex.Message}");
                return;
            }
        }
    }
}
