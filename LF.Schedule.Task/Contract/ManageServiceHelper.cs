﻿using System;
using System.Diagnostics;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using Common.Logging;
using SXT.Configuration;

namespace LF.Schedule.Task.Contract
{
    public static class ManageServiceHelper
    {
        private static WebServiceHost _serviceHost;

        private static readonly ILog Log = LogManager.GetLogger(typeof(ManageServiceHelper));

        public static void StartService()
        {
            if (_serviceHost != null) return;
            var manageServicePort = GetManageServicePort();
            Log.InfoFormat("ManageServiceHelper.GetManageServicePort 返回可用端口：{0}", manageServicePort);

                ;
            var baseAddress = new Uri($"http://127.0.0.1:{manageServicePort}/ManageService");
            _serviceHost = new WebServiceHost(typeof(ManageService), baseAddress);

            foreach (var endpoint in _serviceHost.Description.Endpoints)
            {
                var behavior = endpoint.Behaviors.Find<WebHttpBehavior>();
                if (null != behavior)
                {
                    endpoint.Behaviors.Remove(behavior);
                }
            }
            _serviceHost.Opened += delegate
            {
                Log.InfoFormat("ManageServiceHelper.StartService WCF管理服务启动成功，端口：{0}", manageServicePort);
            };
            _serviceHost.Open();
            foreach (var endpoint in _serviceHost.Description.Endpoints)
            {
                Debug.Assert(null != endpoint.Behaviors.Find<WebHttpBehavior>());
            }
        }

        public static void StopService()
        {
            _serviceHost?.Close();
            _serviceHost = null;
            Log.Info("ManageServiceHelper.StopService WCF管理服务停止成功");
        }

        private static string GetManageServicePort()
        {

            var serviceConfiguration = new DefaultConfiguration("ServiceJob.config", "ServiceJobConfig");
            var servicePort = serviceConfiguration["ManageServicePort"];

            var manageServicePort = 9000;
            if (!int.TryParse(servicePort, out manageServicePort))
                manageServicePort = 9000;

            var appSettingsSection = serviceConfiguration.Configuration.AppSettings;
            appSettingsSection.Settings.Add("ManageServicePort", manageServicePort.ToString());
            serviceConfiguration.Configuration.Save();

            return manageServicePort.ToString();
        }
    }
}