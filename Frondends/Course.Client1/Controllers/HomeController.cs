using Course.Client1.Exceptions;
using Course.Client1.Models;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService) 
        {
            this.catalogService = catalogService;
            _logger = logger;
        }


        public async  Task<IActionResult> Index()
        {

            return View(await catalogService.GetAllCourseAsync());
        }
        public async Task<IActionResult> Detail(string id)
        {
            var result =await catalogService.GetByCourseId(id);

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if(errorFeature!=null &&errorFeature.Error is UnAuthorizeExeption)
            {
                return RedirectToAction("Logout", "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
