using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
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
            ApiClientSettings.ZZiPagoUrl = webSettings.Value.ZZiPagoUrl;            
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
            var logger = NLog.LogManager.GetCurrentClassLogger();
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;

            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioAutenticar), model.Clave1);

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

                        logger.Info("requestUrl: " + requestUrl.ToString());

                        response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                        if (response.Mensaje == "1")
                        {
                            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioAutenticar), model.Clave1);

                            return RedirectToAction("Iniciar", "Afiliacion", response.Model);
                            //return View("~/Views/Afiliacion/Registro.cshtml", response.Model);
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
                            return View("~/Views/Seguridad/Login.cshtml");
                        }
                    }
                    else
                    {
                        return View("~/Views/Seguridad/Login.cshtml");
                    }
                    
                }
                else
                {
                    return View("~/Views/Seguridad/Login.cshtml");
                }                                
            }
            catch (Exception ex)
            {                
                response.HizoError = true;
                response.MensajeError = ex.ToString();
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioAutenticar), model.Clave1, ex.ToString());
                ModelState.AddModelError("ErrorRegistro", ex.Message);
                return View("~/Views/Seguridad/Login.cshtml");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsuarioRegistrar()
        {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
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
                        logger))
                    {
                        model.Clave2 = Criptografia.Encriptar(model.Clave2);
                        model.AceptoTerminos = Constantes.strUsuarioZiPago_AceptoTerminos;
                        var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Registrar));
                        response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                        if (!response.HizoError)
                        {
                            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioRegistrar), model.Clave1);
                            return RedirectToAction("Iniciar", "Afiliacion", response.Model);
                        }
                        else
                        {
                            logger.Info("[{0}] | UsuarioViewModel: [{1}] | " + response.Mensaje, nameof(UsuarioRegistrar), model.Clave1);
                            ModelState.AddModelError("ErrorRegistro", response.MensajeError);
                            return View("~/Views/Seguridad/Registro.cshtml");
                        }
                    }
                    else
                    {
                        return View("~/Views/Seguridad/Registro.cshtml");
                    }
                                                            
                }
                else {
                    return View("~/Views/Seguridad/Registro.cshtml");
                }
            }
            catch (Exception ex)
            {
                response.HizoError = true;
                response.MensajeError = ex.ToString();
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioRegistrar), model.Clave1, ex.ToString());
                ModelState.AddModelError("ErrorRegistro", ex.Message);
                return View("~/Views/Seguridad/Registro.cshtml");
            }

        }

    }
}