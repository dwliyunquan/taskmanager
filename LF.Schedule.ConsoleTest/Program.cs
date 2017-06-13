using System;
using System.Runtime.InteropServices;
using System.Threading;
using LF.Schedule.Task;

namespace LF.Schedule.ConsoleTest
{
    public delegate bool ConsoleCtrlHandler(int dwCtrlType);
    class Program
    {
        // 当Console关闭时，系统会发送此信号   
        private const int CtrlCloseEvent = 2;

        /// <summary>
        /// 注册捕获Console信号处理方法
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandler handlerRoutine, bool add);

        static void Main(string[] args)
        {
            try
            {
                ServiceHelper.OnStart();
                SetConsoleCtrlHandler(new ConsoleCtrlHandler(ConsoleHandlerRoutine), true);

                while (true)
                {
                    if (string.Compare(Console.ReadLine(), "exit", StringComparison.OrdinalIgnoreCase) == 0)
                        break;
                }
            }
            finally
            { ServiceHelper.OnStop(); }
        }

        private static bool ConsoleHandlerRoutine(int ctrlType)
        {
            switch (ctrlType)
            {
                case CtrlCloseEvent:
                    try
                    {
                        ServiceHelper.OnStop();
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        // ignored
                    }
                    break;
            }

            return false;
        }
    }
}
