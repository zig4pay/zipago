using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZREL.ZiPago.Aplicacion.Web.Models;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (TempData["UZiPago"] != null)
            {
                var model = TempData["UZiPago"] as UsuarioViewModel;
                ViewBag.Clave1 = model.Clave1;
                ViewBag.NombresUsuario = model.NombresUsuario;
                ViewBag.ApellidosUsuario = model.ApellidosUsuario;                
            }
            return View("~/Views/Home/Index.cshtml");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
