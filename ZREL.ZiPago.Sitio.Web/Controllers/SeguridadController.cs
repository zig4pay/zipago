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

            logger.Info("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{1}] | Inicio.", model.Clave1);

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
                            logger.Info("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{0}] | {1}.", response.Model.Clave1, response.Mensaje);
                            if (response.Mensaje == Constantes.RegistroUsuario.UsuarioRegistradoCorrectamente.ToString())
                            {
                                EnviarCorreo(response.Model);
                                return Redirect(webSettings.Value.ZZiPagoPortalUrl);
                            }
                            else {
                                ViewBag.Incorrecto = true;
                                ViewBag.Mensaje = string.Format(Constantes.strMensajeUsuarioYaExiste, response.Model.Clave1);
                                ViewBag.Tipo = "warning";
                                return View("~/Views/Seguridad/Registro.cshtml");
                            }
                        }
                        else
                        {
                            ViewBag.Incorrecto = true;
                            ViewBag.Mensaje = response.MensajeError;
                            ViewBag.Tipo = "error";
                            logger.Error("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{0}] | Error: {1}", model.Clave1, response.MensajeError);
                            return View("~/Views/Seguridad/Registro.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Incorrecto = true;
                        ViewBag.MensajeError = Constantes.strMensajeErrorValidarCaptcha;
                        logger.Error("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{0}] | Error: {1}", model.Clave1, Constantes.strMensajeErrorValidarCaptcha);
                        return View("~/Views/Seguridad/Registro.cshtml");
                    }
                }
                else
                {
                    ViewBag.Incorrecto = true;
                    ViewBag.MensajeError = Constantes.strMensajeDatosIncorrectos;
                    logger.Error("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{0}] | Error: {1}", model.Clave1, Constantes.strMensajeDatosIncorrectos);
                    return View("~/Views/Seguridad/Registro.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Incorrecto = true;
                ViewBag.MensajeError = ex.Message;
                logger.Error("[Sitio.Web.Controllers.SeguridadController.UsuarioRegistrar] | UsuarioZiPago: [{0}] | Excepcion: {1} - InnerException: {2}.", model.Clave1, ex.ToString(), ex.InnerException.ToString() ?? string.Empty);
                return View("~/Views/Seguridad/Registro.cshtml");
            }

        }

        public IActionResult UsuarioAutenticar() {
            return Redirect(webSettings.Value.ZZiPagoPortalUrl);
        }

        private bool EnviarCorreo(UsuarioViewModel usuario) {

            bool respuesta = false;    
            string mensaje = string.Empty;            
            var logger = NLog.LogManager.GetCurrentClassLogger();
            Libreria.Mail.Manage mail = new Libreria.Mail.Manage();
            Libreria.Mail.Settings mailsettings = new Libreria.Mail.Settings();

            try
            {
                configuration.GetSection("ZRELZiPagoMail").Bind(mailsettings);
                mensaje = mail.Enviar(usuario.NombresUsuario + " " + usuario.ApellidosUsuario,
                                        usuario.Clave1,
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Asunto"),
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Mensaje").Replace("clave1", usuario.Clave1).Replace("clave2", usuario.Clave2),
                                        mailsettings);
                if (mensaje.Trim().Length > 0)
                {                    
                    logger.Error("[Sitio.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioZiPago: [{0}] | Mensaje: {1}.", usuario.Clave1, mensaje);
                }
                else
                {
                    respuesta = true;
                    logger.Info("[Sitio.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioZiPago: [{0}] | {1}.", usuario.Clave1, Constantes.strMensajeEnvioMail);
                }                    
            }
            catch (Exception ex)
            {
                respuesta = false;
                logger.Error("[Sitio.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioZiPago: [{0}] | Excepcion: {1}.", usuario.Clave1, ex.ToString());
            }
            return respuesta;
        }

    }
}