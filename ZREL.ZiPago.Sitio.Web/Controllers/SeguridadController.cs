using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Text.Json;
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
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions{
                                                                    IgnoreNullValues = true,
                                                                    PropertyNameCaseInsensitive = true,
                                                                    WriteIndented = true
                                                                 };

        public SeguridadController(IConfiguration configuration, IOptions<WebSiteSettingsModel> app)
        {
            this.configuration = configuration;
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public IActionResult UsuarioRegistrar()
        {
            try
            {
                ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
                ViewData["UrlSitioWeb"] = webSettings.Value.ZZiPagoSitioUrl;
            }
            catch (Exception ex)
            {
                ViewBag.Incorrecto = true;
                ViewBag.Mensaje = Constantes.strMensajeGeneralError;
                ViewBag.Tipo = "error";
                Log.InvokeAppendLogError("SeguridadController.UsuarioRegistrar",
                                         "Exception [" + ex.ToString() + "] " +
                                         "Inner Exception [" + (ex.InnerException.ToString()) + "]");                
            }
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]        
        public async Task<IActionResult> UsuarioRegistrar(UsuarioViewModel model)
        {
            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();            
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
                       
            Log.InvokeAppendLog("SeguridadController.UsuarioRegistrar", "UsuarioZiPago: [{" + model.Clave1 + "}] | Inicio.");
            try
            {
                if (ModelState.IsValid)
                {
                    if (await GoogleReCaptchaValidation.ReCaptchaPassed(
                                Request.Form["g-recaptcha-response"],
                                webSettings.Value.SecretKey
                            )
                        )
                    {
                        model.Clave2 = Criptografia.Encriptar(model.Clave2);
                        model.AceptoTerminos = Constantes.strUsuarioZiPago_AceptoTerminos;

                        var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Registrar));
                        Log.InvokeAppendLog("SeguridadController.UsuarioRegistrar", "requestUrl: [" + requestUrl + "]");
                        response = await ApiClientFactory.Instance.PostAsync(requestUrl, model);
                        Log.InvokeAppendLog("SeguridadController.UsuarioRegistrar", "response: [" + JsonSerializer.Serialize(response, jsonOptions) + "]");

                        if (!response.HizoError)
                        {
                            if (response.Mensaje == Constantes.RegistroUsuario.UsuarioRegistradoCorrectamente.ToString())
                            {
                                EnviarCorreo(response.Model);
                                ViewBag.Incorrecto = false;
                                ViewBag.Mensaje = string.Format(Constantes.strMensajeUsuarioRegistroCorrecto, response.Model.Clave1);
                                ViewBag.Tipo = "success";
                                ViewBag.ZZiPagoPortalUrl = webSettings.Value.ZZiPagoPortalUrl;
                                Log.InvokeAppendLog("SeguridadController.UsuarioRegistrar", string.Format(Constantes.strMensajeUsuarioRegistroCorrecto, response.Model.Clave1));
                                return View("~/Views/Seguridad/Registro.cshtml");                               
                            }
                            else {
                                ViewBag.Incorrecto = true;
                                ViewBag.Mensaje = string.Format(Constantes.strMensajeUsuarioYaExiste, response.Model.Clave1);
                                ViewBag.Tipo = "warning";
                                Log.InvokeAppendLog("SeguridadController.UsuarioRegistrar", string.Format(Constantes.strMensajeUsuarioYaExiste, response.Model.Clave1));
                                return View("~/Views/Seguridad/Registro.cshtml");
                            }
                        }
                        else
                        {
                            ViewBag.Incorrecto = true;
                            ViewBag.Mensaje = Constantes.strMensajeGeneralError;
                            ViewBag.Tipo = "error";
                            Log.InvokeAppendLogError("SeguridadController.UsuarioRegistrar", "MensajeError: [" + response.MensajeError + "]");
                            return View("~/Views/Seguridad/Registro.cshtml");
                        }
                    }
                    else
                    {
                        ViewBag.Incorrecto = true;
                        ViewBag.Mensaje = Constantes.strMensajeErrorValidarCaptcha;
                        ViewBag.Tipo = "error";
                        Log.InvokeAppendLogError("SeguridadController.UsuarioRegistrar", "MensajeError: [" + Constantes.strMensajeErrorValidarCaptcha + "]");
                        return View("~/Views/Seguridad/Registro.cshtml");
                    }
                }
                else
                {
                    ViewBag.Incorrecto = true;
                    ViewBag.Mensaje = Constantes.strMensajeDatosIncorrectos;
                    ViewBag.Tipo = "error";
                    Log.InvokeAppendLogError("SeguridadController.UsuarioRegistrar", "MensajeError: [" + Constantes.strMensajeDatosIncorrectos + "]");
                    return View("~/Views/Seguridad/Registro.cshtml");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Incorrecto = true;
                ViewBag.Mensaje = Constantes.strMensajeGeneralError;
                ViewBag.Tipo = "error";
                Log.InvokeAppendLogError("SeguridadController.UsuarioRegistrar", "Exception: [" + ex.ToString() + "]");
                return View("~/Views/Seguridad/Registro.cshtml");
            }

        }

        [HttpGet]
        public IActionResult UsuarioAutenticar() {                            
            return Redirect(webSettings.Value.ZZiPagoPortalUrl);            
        }

        private void EnviarCorreo(UsuarioViewModel usuario) {
                        
            string respuestamail;
            Libreria.Mail.Manage mail = new Libreria.Mail.Manage();
            Libreria.Mail.Settings mailsettings = new Libreria.Mail.Settings();

            try
            {
                configuration.GetSection("ZRELZiPagoMail").Bind(mailsettings);
                respuestamail = mail.Enviar(usuario.NombresUsuario + " " + usuario.ApellidosUsuario,
                                        usuario.Clave1,
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Asunto"),
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRegistro:Mensaje").Replace("clave1", usuario.Clave1).Replace("clave2", usuario.Clave2),
                                        mailsettings);
                if (respuestamail.Length == 0)
                    Log.InvokeAppendLog("SeguridadController.EnviarCorreo", "mensaje: [" + Constantes.strMensajeEnvioMail + "]");
                else
                    Log.InvokeAppendLogError("SeguridadController.EnviarCorreo", "mensaje: [" + respuestamail + "]");                
            }
            catch (Exception ex)
            {
                Log.InvokeAppendLogError("SeguridadController.EnviarCorreo", "Exception: [" + ex.ToString() + "] - InnerException[" + ex.InnerException.ToString() + "]");
            }            
        }

    }
}