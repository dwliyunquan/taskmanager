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


    }
}
