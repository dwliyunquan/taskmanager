namespace LF.Schedule.Manage
{
    partial class Manage
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnResetStart = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.btnUninstall = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.serviceDataGridView = new System.Windows.Forms.DataGridView();
            this.ServiceKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ServiceStopTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ExcuteDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(3, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启  动";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(84, 2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "停  止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnResetStart
            // 
            this.btnResetStart.Location = new System.Drawing.Point(165, 3);
            this.btnResetStart.Name = "btnResetStart";
            this.btnResetStart.Size = new System.Drawing.Size(75, 23);
            this.btnResetStart.TabIndex = 2;
            this.btnResetStart.Text = "重  启";
            this.btnResetStart.UseVisualStyleBackColor = true;
            this.btnResetStart.Click += new System.EventHandler(this.btnResetStart_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(246, 3);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 3;
            this.btnLoad.Text = "加  载";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Location = new System.Drawing.Point(327, 3);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 4;
            this.btnInstall.Text = "安 装";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // btnUninstall
            // 
            this.btnUninstall.Location = new System.Drawing.Point(408, 3);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(75, 23);
            this.btnUninstall.TabIndex = 5;
            this.btnUninstall.Text = "卸  载";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnUninstall);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnInstall);
            this.panel1.Controls.Add(this.btnResetStart);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1044, 28);
            this.panel1.TabIndex = 6;
            // 
            // serviceDataGridView
            // 
            this.serviceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serviceDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ServiceKey,
            this.ServiceName,
            this.ServiceState,
            this.ServiceStartTime,
            this.ServiceStopTime,
            this.Description,
            this.ExcuteDescription});
            this.serviceDataGridView.Location = new System.Drawing.Point(0, 49);
            this.serviceDataGridView.Name = "serviceDataGridView";
            this.serviceDataGridView.RowTemplate.Height = 23;
            this.serviceDataGridView.Size = new System.Drawing.Size(1044, 438);
            this.serviceDataGridView.TabIndex = 7;
            // 
            // ServiceKey
            // 
            this.ServiceKey.DataPropertyName = "ServiceKey";
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ServiceKey.DefaultCellStyle = dataGridViewCellStyle1;
            this.ServiceKey.Frozen = true;
            this.ServiceKey.HeaderText = "服务主键";
            this.ServiceKey.Name = "ServiceKey";
            this.ServiceKey.ReadOnly = true;
            this.ServiceKey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ServiceKey.Width = 120;
            // 
            // ServiceName
            // 
            this.ServiceName.DataPropertyName = "ServiceName";
            this.ServiceName.HeaderText = "服务名称";
            this.ServiceName.Name = "ServiceName";
            this.ServiceName.ReadOnly = true;
            this.ServiceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ServiceName.Width = 160;
            // 
            // ServiceState
            // 
            this.ServiceState.DataPropertyName = "ServiceState";
            this.ServiceState.HeaderText = "服务状态";
            this.ServiceState.Name = "ServiceState";
            this.ServiceState.ReadOnly = true;
            this.ServiceState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ServiceStartTime
            // 
            this.ServiceStartTime.DataPropertyName = "ServiceStartTime";
            this.ServiceStartTime.HeaderText = "服务开启时间";
            this.ServiceStartTime.Name = "ServiceStartTime";
            this.ServiceStartTime.ReadOnly = true;
            this.ServiceStartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ServiceStartTime.Width = 120;
            // 
            // ServiceStopTime
            // 
            this.ServiceStopTime.DataPropertyName = "ServiceStopTime";
            this.ServiceStopTime.HeaderText = "服务停止时间";
            this.ServiceStopTime.Name = "ServiceStopTime";
            this.ServiceStopTime.ReadOnly = true;
            this.ServiceStopTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ServiceStopTime.Width = 120;
            // 
            // Description
            // 
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "服务描述";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Description.Width = 180;
            // 
            // ExcuteDescription
            // 
            this.ExcuteDescription.DataPropertyName = "ExcuteDescription";
            this.ExcuteDescription.HeaderText = "运行状态描述";
            this.ExcuteDescription.Name = "ExcuteDescription";
            this.ExcuteDescription.Width = 200;
            // 
            // Manage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 486);
            this.Controls.Add(this.serviceDataGridView);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Name = "Manage";
            this.Text = "定时服务管理工具";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnResetStart;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnInstall;
        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView serviceDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceState;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ServiceStopTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn ExcuteDescription;
    }
}

