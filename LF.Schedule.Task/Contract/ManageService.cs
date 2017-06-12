using System;
using System.Linq;
using Common.Logging;
using LF.Schedule.Contract;

namespace LF.Schedule.Task.Contract
{
    public class ManageService : IManageService
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ManageService));

        public StateListResult GetStateList()
        {
            _log.Info("ManageService.GetStateList 调用成功");

            return new StateListResult()
            {
                ServiceStateList = ServiceHelper.ServiceStateInfoByServuceKey.Values.ToArray()
            };
        }

        public SendCommandResult SendCommand(SendCommandParam sendCommandParam)
        {
            if (sendCommandParam == null)
            {
                _log.Info("[SendCommand]:服务调用失败 [sendCommandParam参数不能为null]");
                return new SendCommandResult() {Success = false, Message = "sendCommandParam参数不能为null"};
            }

            if (!ServiceHelper.ServiceStateInfoByServuceKey.ContainsKey(sendCommandParam.ServiceKey))
            {
                _log.InfoFormat("[SendCommand] 调用失败，找不到该指定的服务 [{0}]", sendCommandParam);
                return new SendCommandResult()
                {
                    Success = false,
                    Message = $"找不到指定ID[{sendCommandParam.ServiceKey}]的服务"
                };
            }

            var serviceStateInfo = ServiceHelper.ServiceStateInfoByServuceKey[sendCommandParam.ServiceKey];
            SendCommandResult sendCommandResult;
            switch (sendCommandParam.Command)
            {
                case ServiceCommandEnum.Start:
                {
                    if (serviceStateInfo.ServiceState == ServiceStateEnum.Uninstall)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务已卸载 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]该指定的服务已卸载"
                        };
                    }

                    if (serviceStateInfo.ServiceState == ServiceStateEnum.Normal ||
                        serviceStateInfo.ServiceState == ServiceStateEnum.Failed)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务已在运行中 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务已在运行中"
                        };
                    }


                    sendCommandResult = ServiceInit.StartServiceByServiceKey(sendCommandParam.ServiceKey);
                    if (!sendCommandResult.Success)
                    {
                        _log.InfoFormat("[SendCommand] 启动失败 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务启动失败"
                        };
                    }
                }
                    break;

                case ServiceCommandEnum.Stop:
                {
                    if (serviceStateInfo.ServiceState == ServiceStateEnum.Uninstall)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务已卸载 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]该指定的服务已卸载"
                        };
                    }

                    if (
                        serviceStateInfo.ServiceState == ServiceStateEnum.Stopped)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务未启动 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务未启动"
                        };
                    }

                    sendCommandResult = ServiceInit.StopServiceByServiceKey(sendCommandParam.ServiceKey);
                    if (!sendCommandResult.Success)
                    {
                        _log.InfoFormat("[SendCommand] 停止失败 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务停止失败"
                        };
                    }
                }
                    break;

                case ServiceCommandEnum.ResetStart:
                {
                    if (serviceStateInfo.ServiceState == ServiceStateEnum.Uninstall)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务已卸载 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]该指定的服务已卸载"
                        };
                    }

                    sendCommandResult = ServiceInit.ResetStartServiceByServiceKey(sendCommandParam.ServiceKey);
                    if (!sendCommandResult.Success)
                    {
                        _log.InfoFormat("[SendCommand] 重新启动失败 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务重新启动失败"
                        };
                    }
                }
                    break;

                case ServiceCommandEnum.Install:
                {
                    if (serviceStateInfo.ServiceState != ServiceStateEnum.Uninstall)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务已安装 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务已被加载"
                        };
                    }

                    sendCommandResult = ServiceInit.InstallServiceByServiceKey(sendCommandParam.ServiceKey);
                    if (!sendCommandResult.Success)
                    {
                        _log.InfoFormat("[SendCommand] 加载失败 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务加载失败"
                        };
                    }
                }
                    break;

                case ServiceCommandEnum.Uninstall:
                {
                    if (serviceStateInfo.ServiceState == ServiceStateEnum.Uninstall)
                    {
                        _log.InfoFormat("[SendCommand] 调用失败，该指定的服务未被安装 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务未被加载"
                        };
                    }

                    sendCommandResult = ServiceInit.UninstallServiceByServiceKey(sendCommandParam.ServiceKey);
                    if (!sendCommandResult.Success)
                    {
                        _log.InfoFormat("[SendCommand] 卸载失败 [{0}]", sendCommandParam);
                        return new SendCommandResult()
                        {
                            Success = false,
                            Message = $"指定ID[{sendCommandParam.ServiceKey}]的服务卸载失败"
                        };
                    }

                }
                    break;
                case ServiceCommandEnum.Load:
                {
                    ServiceInit.LoadService();
                    _log.InfoFormat("[SendCommand]:服务加载成功");
                }
                    break;
                default:
                    return new SendCommandResult()
                    {
                        Success = false,
                        Message = $"[sendCommandParam.Command]指令不正确"
                    };
            }
            _log.InfoFormat("[SendCommand] 调用成功 [{0}]", sendCommandParam);
            return new SendCommandResult() {Success = true, Message = "指令执行成功"};
        }
    }
}
