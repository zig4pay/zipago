using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Libreria.Seguridad;
using ZREL.ZiPago.Sitio.Web.Clients;
using ZREL.ZiPago.Sitio.Web.Models.Response;
using ZREL.ZiPago.Sitio.Web.Models.Seguridad;
using ZREL.ZiPago.Sitio.Web.Models.Settings;
using ZREL.ZiPago.Sitio.Web.Utility;

namespace ZREL.ZiPago.Sitio.Web.Controllers
{
    public class SeguridadController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IOptions<WebSiteSettingsModel> webSettings;        

        public SeguridadController(IConfiguration configuration, IOptions<WebSiteSettingsModel> app)
        {
            this.configuration = configuration;
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

            logger.Info("[Sitio.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioRegistrar), model.Clave1);

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
                            logger.Info("[Sitio.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Registro Realizado.", nameof(UsuarioRegistrar), model.Clave1);
                            var mensaje = EnviarCorreo(response.Model);
                            return Redirect(webSettings.Value.ZZiPagoPortalUrl);

                        }
                        else
                        {
                            ViewBag.Incorrecto = true;
                            ViewBag.MensajeError = response.MensajeError;
                            logger.Error("Sitio.Web.Controllers.SeguridadController.[{0}] | UsuarioViewModel: [{1}] | Error: " + response.MensajeError, nameof(UsuarioRegistrar), model.Clave1);
                            return View("~/Views/Seguridad/Registro.cshtml");
                        }                        
                    }
                    else
                    {
                        ViewBag.Incorrecto = true;
                        ViewBag.MensajeError = Constantes.strMensajeErrorValidarCaptcha;
                        logger.Error("[Sitio.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Error: " + Constantes.strMensajeErrorValidarCaptcha, nameof(UsuarioRegistrar), model.Clave1);
                        return View("~/Views/Seguridad/Registro.cshtml");
                    }
                }
                else
                {
                    ViewBag.Incorrecto = true;
                    ViewBag.MensajeError = Constantes.strMensajeDatosIncorrectos;
                    logger.Error("[Sitio.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Error: " + Constantes.strMensajeDatosIncorrectos, nameof(UsuarioRegistrar), model.Clave1);
                    return View("~/Views/Seguridad/Registro.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Incorrecto = true;
                ViewBag.MensajeError = ex.ToString();
                logger.Error("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioRegistrar), model.Clave1, ex.ToString());
                return View("~/Views/Seguridad/Registro.cshtml");
            }

        }

        public IActionResult UsuarioAutenticar() {
            return Redirect(webSettings.Value.ZZiPagoPortalUrl);
        }

        private string EnviarCorreo(UsuarioViewModel usuario) {
            
            string respuesta = "";            
            var logger = NLog.LogManager.GetCurrentClassLogger();
            Libreria.Mail.Manage mail = new Libreria.Mail.Manage();
            Libreria.Mail.Settings mailsettings = new Libreria.Mail.Settings();

            try
            {
                configuration.GetSection("ZRELZiPagoMail").Bind(mailsettings);
                respuesta = mail.Enviar(usuario.NombresUsuario + " " + usuario.ApellidosUsuario,
                                        usuario.Clave1,
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Asunto"),
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Mensaje").Replace("clave1", usuario.Clave1).Replace("clave2", usuario.Clave2),
                                        mailsettings);
                if (respuesta.Trim().Length > 0)                
                    logger.Error("[Sitio.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioViewModel: [{0}] | Mensaje: {1}.", nameof(UsuarioRegistrar), usuario.Clave1, respuesta);
                
            }
            catch (Exception ex)
            {
                respuesta = ex.ToString();
                logger.Error("[Sitio.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioViewModel: [{0}] | Excepcion: {1}.", nameof(UsuarioRegistrar), usuario.Clave1, ex.ToString());
            }

            return respuesta;

        }

    }
}