using System.ServiceProcess;
using LF.Schedule.Task;

namespace LF.Schedule.ConsoleService
{
    public partial class ManageService : ServiceBase
    {
        public ManageService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            ServiceHelper.OnStart();
        }

        protected override void OnStop()
        {
            ServiceHelper.OnStop();
        }
    }
}
