using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Sitio.Web.Clients;
using ZREL.ZiPago.Sitio.Web.Models.Response;
using ZREL.ZiPago.Sitio.Web.Models.Seguridad;
using ZREL.ZiPago.Sitio.Web.Models.Settings;
using ZREL.ZiPago.Sitio.Web.Utility;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Libreria.Seguridad;
using Newtonsoft.Json;

namespace ZREL.ZiPago.Sitio.Web.Controllers
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
        public IActionResult UsuarioRegistrar()
        {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]        
        public async Task<IActionResult> UsuarioRegistrar(UsuarioViewModel model)
        {

            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();            
            var logger = NLog.LogManager.GetCurrentClassLogger();
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;

            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioRegistrar), model.Clave1);

            try
            {
                if (ModelState.IsValid)
                {

                    if (await GoogleReCaptchaValidation.ReCaptchaPassed(
                                Request.Form["g-recaptcha-response"],
                                webSettings.Value.SecretKey,
                                logger
                            )
                        )
                    {
                        model.Clave2 = Criptografia.Encriptar(model.Clave2);
                        model.AceptoTerminos = Constantes.strUsuarioZiPago_AceptoTerminos;
                        var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Registrar));
                        response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                        if (!response.HizoError)
                        {
                            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Registro Realizado.", nameof(UsuarioRegistrar), model.Clave1);                                                        
                            return Redirect(webSettings.Value.ZZiPagoPortalUrl + model.Clave1.Trim());
                        }
                        else
                        {
                            ViewBag.Incorrecto = true;
                            ViewBag.MensajeError = response.MensajeError;
                            logger.Error("[{0}] | UsuarioViewModel: [{1}] | " + response.MensajeError, nameof(UsuarioRegistrar), model.Clave1);
                            return View("~/Views/Seguridad/Registro.cshtml");
                        }                        
                    }
                    else
                    {
                        ViewBag.Incorrecto = true;
                        ViewBag.MensajeError = Constantes.strMensajeErrorValidarCaptcha;
                        logger.Error("[{0}] | UsuarioViewModel: [{1}] | " + Constantes.strMensajeErrorValidarCaptcha, nameof(UsuarioRegistrar), model.Clave1);
                        return View("~/Views/Seguridad/Registro.cshtml");
                    }
                }
                else
                {
                    ViewBag.Incorrecto = true;
                    ViewBag.MensajeError = Constantes.strMensajeDatosIncorrectos;
                    logger.Error("[{0}] | UsuarioViewModel: [{1}] | " + Constantes.strMensajeDatosIncorrectos, nameof(UsuarioRegistrar), model.Clave1);
                    return View("~/Views/Seguridad/Registro.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Incorrecto = true;
                ViewBag.MensajeError = ex.ToString();
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioRegistrar), model.Clave1, ex.ToString());
                return View("~/Views/Seguridad/Registro.cshtml");
            }

        }

    }
}