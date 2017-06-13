using System;
using System.Windows.Forms;

namespace LF.Schedule.Manage
{
    public partial class Manage : Form
    {
        public Manage()
        {
            InitializeComponent();
            LoadData();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            SendCommand(ServiceCommandEnum.Start);

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            SendCommand(ServiceCommandEnum.Stop);
        }

        private void btnResetStart_Click(object sender, EventArgs e)
        {
            SendCommand(ServiceCommandEnum.ResetStart);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            SendCommand(ServiceCommandEnum.Load);
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            SendCommand(ServiceCommandEnum.Install);
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            SendCommand(ServiceCommandEnum.Uninstall);
        }

        private void LoadData()
        {
            try
            {
                var serviceStateResult = ManageHelper.GetStateList();
                serviceDataGridView.DataSource = serviceStateResult.ServiceStateList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SendCommand(ServiceCommandEnum serviceCommandEnum)
        {
            if (serviceDataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show(@"请选择需要操作的服务");
                return;
            }
            var serviceKey = Convert.ToString(serviceDataGridView.SelectedRows[0].Cells["serviceKey"].Value);
            var resultMessage = ManageHelper.SendCommand(serviceKey, serviceCommandEnum);
            MessageBox.Show(resultMessage);
            LoadData();
        }
    }
}
