using HangFire.Models;
using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace HangFire.Services
{
    /// <summary>
    /// Класс для получения списка сервисов и их статусов на определённом сервере
    /// </summary>
    public class WinServiceRemoteControl
    {
        private string _serverName { get; set; }
        private string _service { get; set; }

        public WinServiceRemoteControl(WinService winService)
        {
            _serverName = winService.Server;
            _service = winService.ServiceName;
        }

        /// <summary>
        /// Получение списка всех служб
        /// </summary>
        public List<WinService> GetServicesOnRemoteMachine()
        {
            ServiceController[] sc = ServiceController.GetServices(_serverName);

            var list = new List<WinService>();

            foreach (var s in sc)
            {
                list.Add(new WinService { 
                    Server = _serverName,
                    ServiceName = s.ServiceName, 
                    DisplayName = s.DisplayName,
                    Status = s.Status.ToString()
                }) ;
            }

            return list;
        }

        public WinService GetServicesOnRemoteMachineByName()
        {
            var s = new ServiceController(_service, _serverName);

            var service = new WinService
            {
                Server = _serverName,
                ServiceName = s.ServiceName,
                DisplayName = s.DisplayName,
                Status = s.Status.ToString()
            };

            return service;
        }

        /// <summary>
        /// Включение службы
        /// </summary>
        /// <returns></returns>
        public WinService StartService()
        {
            return StartOrStopService(ServiceControllerStatus.Running);
        }

        /// <summary>
        /// Отключение службы
        /// </summary>
        /// <returns></returns>
        public WinService StopService()
        {
            return StartOrStopService(ServiceControllerStatus.Stopped);
        }
        
        /// <summary>
        /// Перезагрузка сервиса windows
        /// </summary>
        /// <returns></returns>
        public WinService RefreshService()
        {
            var service = new ServiceController(_service, _serverName);
            
            service.Refresh();

            return GetWinService(new ServiceController(_service, _serverName), _serverName);
        }

        /// <summary>
        /// Общий метод для остановки и запуска службы
        /// </summary>
        /// <param name="serviceControllerStatus"></param>
        /// <returns></returns>
        private WinService StartOrStopService(ServiceControllerStatus serviceControllerStatus)
        {
            var service = new ServiceController(_service, _serverName);

            switch (serviceControllerStatus)
            {
                case ServiceControllerStatus.Running:
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        throw new Exception("Не возможно запустить сервис, так как он уже запущен");
                    }
                    service.Start();
                    return GetWinService(new ServiceController(_service, _serverName), _serverName);
                case ServiceControllerStatus.Stopped:
                    if (service.Status == ServiceControllerStatus.Stopped)
                    {
                        throw new Exception("Не возможно остановить сервис, так как он уже остановлен");
                    }
                    service.Stop();
                    return GetWinService(new ServiceController(_service, _serverName), _serverName);
                //case ServiceControllerStatus.
                default:
                    return GetWinService(new ServiceController(_service, _serverName), _serverName);
            }
        }
        
        /// <summary>
        /// Формируем обьект для передачи ответа от api
        /// </summary>
        /// <param name="serviceController"></param>
        /// <param name="serverName"></param>
        /// <returns></returns>
        public WinService GetWinService(ServiceController serviceController, string serverName)
        {
            var s = new WinService
            {
                Server = serverName,
                ServiceName = serviceController.ServiceName,
                DisplayName = serviceController.DisplayName,
                Status = serviceController.Status.ToString(),
            };

            return s;
        }
    }
}
