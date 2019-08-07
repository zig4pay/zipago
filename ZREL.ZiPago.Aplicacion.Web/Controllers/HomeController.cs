using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Diagnostics;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                if (HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session") != null)
                {
                    ResponseModel<UsuarioViewModel> usuario = HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session");
                    ViewBag.Usuario = usuario.Model.NombresUsuario.Trim() + " " + usuario.Model.ApellidosUsuario.Trim();
                    ViewBag.Clave1 = usuario.Model.Clave1.Trim();
                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    return View("~/Views/Seguridad/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
