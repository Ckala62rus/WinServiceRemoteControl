using HangFire.Models;
using HangFire.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HangFire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// 
        /// Add role admin for user
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> AddUserToAdminRole()
        {
            // Создание роли
            await _roleManager.CreateAsync(new IdentityRole("HangfireAdmin"));

            //Поиск ползователя по Имени (почте)
            var user = await _userManager.FindByNameAsync("agr.akyla@mail.ru");

            // Привязка пользователя к роли
            await _userManager.AddToRoleAsync(user, "HangfireAdmin");

            return Ok();
        }

        public IActionResult Index()
        {
            //RecurringJob.AddOrUpdate("The first job", () => Console.WriteLine("First"), "* * * * *");
            //RecurringJob.AddOrUpdate("The second job", () => Console.WriteLine("Second"), "* * * * *");
            
            return View();
        }

        /// <summary>
        /// Получение списка всех сервисов на удалённом сервере
        /// </summary>
        /// <param name="winService"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetServices(WinService winService)
        {
            try
            {
                var winServiceRemoteControl = new WinServiceRemoteControl(winService);
                var services = winServiceRemoteControl.GetServicesOnRemoteMachine();

                return Json(services);
            } catch (Exception ex)
            {
                return Json(new WinServiceError { Error = ex.Message });
            }
        }

        /// <summary>
        /// Получение сервиса по имени на определённом сервере
        /// </summary>
        /// <param name="winService"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetServiceByName(WinService winService)
        {
            try
            {
                var winServiceRemoteControl = new WinServiceRemoteControl(winService);
                var service = winServiceRemoteControl.GetServicesOnRemoteMachineByName();
                return Json(service);
            } catch (Exception ex)
            {
                return Json(new WinServiceError { Error = ex.Message });
            }
        }

        /// <summary>
        /// Запуск сервиса на удаленном сервере
        /// </summary>
        /// <param name="winService"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult StartService(WinService winService)
        {
            try
            {
                var winServiceRemoteControl = new WinServiceRemoteControl(winService);
                var service = winServiceRemoteControl.StartService();
                return Json(service);
            }
            catch (Exception ex)
            {
                return Json(new WinServiceError { Error = ex.Message });
            }
        }

        /// <summary>
        /// Остановка сервиса на удаленном сервере
        /// </summary>
        /// <param name="winService"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult StopService(WinService winService)
        {
            try
            {
                var winServiceRemoteControl = new WinServiceRemoteControl(winService);
                var service = winServiceRemoteControl.StopService();
                return Json(service);
            }
            catch (Exception ex)
            {
                return Json(new WinServiceError { Error = ex.Message });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
