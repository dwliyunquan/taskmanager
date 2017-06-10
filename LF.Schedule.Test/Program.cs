using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using LF.Schedule.Task;

namespace LF.Schedule.Test
{
    class Program
    {
        private static readonly ILog _Log = LogManager.GetLogger("AddInService");
        static void Main(string[] args)
        {
           ServiceHelper.OnStart();
        }
    }
}
