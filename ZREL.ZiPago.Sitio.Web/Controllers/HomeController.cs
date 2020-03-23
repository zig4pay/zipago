using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZREL.ZiPago.Sitio.Web.Models;

namespace ZREL.ZiPago.Sitio.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
           return View("~/Views/Home/Index.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
