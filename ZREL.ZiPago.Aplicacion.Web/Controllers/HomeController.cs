using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public HomeController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();
            Logger logger = LogManager.GetCurrentClassLogger();
            
            logger.Info("[Aplicacion.Web.Controllers.HomeController.{0}] | Inicio.", nameof(Index));

            try
            {
                if (HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session") != null)
                {
                    ResponseModel<UsuarioViewModel> usuario = HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session");
                    ViewBag.Usuario = usuario.Model.NombresUsuario.Trim() + usuario.Model.ApellidosUsuario.Trim();
                    ViewBag.Clave1 = usuario.Model.Clave1.Trim();
                    return View("~/Views/Home/Index.cshtml");
                }
                else {
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
