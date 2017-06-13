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
