using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Libreria.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class SeguridadController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;
        
        public SeguridadController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;            
        }

        [HttpGet]
        public IActionResult UsuarioAutenticar() {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            return View("~/Views/Seguridad/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UsuarioAutenticar(UsuarioViewModel model)
        {

            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();
            Logger logger = LogManager.GetCurrentClassLogger();
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;

            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioAutenticar), model.Clave1);

            try
            {

                if (ModelState.IsValid) {

                    if (await GoogleReCaptchaValidation.ReCaptchaPassed(
                            Request.Form["g-recaptcha-response"],
                            webSettings.Value.SecretKey,
                            logger)
                        )
                    {
                        model.Clave2 = Criptografia.Encriptar(model.Clave2);
                        var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Autenticar));                        
                        response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                        if (response.Mensaje == "1")
                        {
                            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioAutenticar), model.Clave1);
                            HttpContext.Session.Set<UsuarioViewModel>("ZiPago.Session", response.Model);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (response.HizoError)
                            {
                                ModelState.AddModelError("ErrorLogin", response.MensajeError);
                                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Error: {2}.", nameof(UsuarioAutenticar), model.Clave1, response.MensajeError);
                            }
                            else
                            {
                                ModelState.AddModelError("ErrorLogin", response.Mensaje);
                                logger.Info("[{0}] | UsuarioViewModel: [{1}] | " + response.Mensaje, nameof(UsuarioAutenticar), model.Clave1);
                            }
                            return RedirectToAction("UsuarioAutenticar", "Seguridad");
                        }
                    }
                    else
                    {
                        return RedirectToAction("UsuarioAutenticar", "Seguridad");
                    }                    
                }
                else
                {
                    return RedirectToAction("UsuarioAutenticar", "Seguridad");
                }                                
            }
            catch (Exception ex)
            {                
                response.HizoError = true;
                response.MensajeError = ex.ToString();
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioAutenticar), model.Clave1, ex.ToString());
                ModelState.AddModelError("ErrorRegistro", ex.Message);
                return RedirectToAction("UsuarioAutenticar", "Seguridad");
            }
        }

        public IActionResult UsuarioSalir() {

            UsuarioViewModel usuario = new UsuarioViewModel();
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                usuario = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") ?? null;
                HttpContext.Session.Remove("ZiPago.Session");
                HttpContext.Session.Clear();
            }
            catch (Exception ex)
            {
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.UsuarioSalir] | UsuarioZiPago: [{0}] | Excepcion: {1}.", JsonConvert.SerializeObject(usuario), ex.ToString());
            }

            return RedirectToAction("UsuarioAutenticar", "Seguridad");
        }

    }
}