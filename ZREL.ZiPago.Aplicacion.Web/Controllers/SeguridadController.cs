using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Libreria.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class SeguridadController : Controller
    {

        private readonly IConfiguration configuration;
        private readonly IOptions<WebSiteSettingsModel> webSettings;
        
        public SeguridadController(IConfiguration config, IOptions<WebSiteSettingsModel> app)
        {
            this.configuration = config;
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;            
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsuarioAutenticar() {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            ViewData["Post"] = false;
            ViewData["HizoError"] = false;
            ViewData["Mensaje"] = string.Empty;
            ViewData["UrlRegistrar"] = webSettings.Value.ZZiPagoWebSiteRegistrarUrl;
            return View("~/Views/Seguridad/Login.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]        
        public async Task<IActionResult> UsuarioAutenticar(UsuarioViewModel model)
        {

            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();
            Logger logger = LogManager.GetCurrentClassLogger();
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;

            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | UsuarioZiPago: [{0}] | Inicio.", model.Clave1);

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

                        logger.Info("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | requestUrl: [{0}].", requestUrl.ToString());

                        response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                        if (response.Mensaje == "1")
                        {
                            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Email);                            
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, response.Model.IdUsuarioZiPago.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.Email, response.Model.Clave1));                            
                            identity.AddClaim(new Claim(ClaimTypes.Name, response.Model.NombresUsuario));
                            identity.AddClaim(new Claim("ApellidosUsuario", response.Model.ApellidosUsuario));
                            identity.AddClaim(new Claim("AceptoTerminos", response.Model.ApellidosUsuario));

                            var principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                                                          principal, 
                                                          new AuthenticationProperties { IsPersistent = model.Recordarme });

                            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | UsuarioZiPago: [{0}] | {1}.", model.Clave1, response.Mensaje);                            
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (response.HizoError)
                            {
                                ModelState.AddModelError("ErrorLogin", response.MensajeError);
                                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | UsuarioZiPago: [{0}] | Error: {1}.", model.Clave1, response.MensajeError);
                            }
                            else
                            {
                                ModelState.AddModelError("ErrorLogin", response.Mensaje);
                                logger.Info("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | UsuarioZiPago: [{0}] | {1}", model.Clave1, response.Mensaje);
                            }
                            //return RedirectToAction("UsuarioAutenticar", "Seguridad");
                            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
                            ViewData["UrlRegistrar"] = webSettings.Value.ZZiPagoWebSiteRegistrarUrl;
                            //ViewData["Post"] = false;
                            //ViewData["HizoError"] = false;
                            //ViewData["Mensaje"] = string.Empty;
                            return View("~/Views/Seguridad/Login.cshtml");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorLogin", Constantes.strMensajeErrorValidarCaptcha);
                        logger.Warn("[Aplicacion.Web.Controllers.SeguridadController.UsuarioAutenticar] | UsuarioZiPago: [{0}] | {1}", model.Clave1, Constantes.strMensajeErrorValidarCaptcha);
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
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2} | Inner: {3}.", nameof(UsuarioAutenticar), model.Clave1, ex.ToString(), ex.InnerException.ToString());
                ModelState.AddModelError("ErrorRegistro", ex.Message);
                return RedirectToAction("UsuarioAutenticar", "Seguridad");
            }
        }

        [Route("Seguridad/UsuarioSalir")]
        public async Task<IActionResult> UsuarioSalir() {
            
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {                
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.UsuarioSalir] | UsuarioZiPago: [{0}] | Excepcion: {1}.", HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), ex.ToString());
            }

            return RedirectToAction("UsuarioAutenticar", "Seguridad");
        }
        
        [HttpGet]
        [AllowAnonymous]
        [Route("Seguridad/Recuperar")]
        public IActionResult Recuperar()
        {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            ViewData["Post"] = false;
            ViewData["HizoError"] = false;
            ViewData["Mensaje"] = string.Empty;

            return View("~/Views/Seguridad/Recuperar.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Seguridad/Recuperar")]
        public async Task<IActionResult> Recuperar(UsuarioViewModel model) {

            Logger logger = LogManager.GetCurrentClassLogger();
            Uri requestUrl;
            ResponseModel<UsuarioZiPago> response = new ResponseModel<UsuarioZiPago>();
            string responseGetJson;

            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            ViewData["Post"] = true;

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Recuperar) + model.Clave1);
                responseGetJson = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);
                responseGetJson = responseGetJson.Replace("\\", string.Empty);
                responseGetJson = responseGetJson.Trim('"');
                response = JsonConvert.DeserializeObject<ResponseModel<UsuarioZiPago>>(responseGetJson);                                
                ViewData["HizoError"] = response.HizoError;

                var callbackurl = response.HizoError ? string.Empty : Url.Action(
                                                                            controller: "Seguridad",
                                                                            action: "Restablecer",
                                                                            values: new { code = response.Model.ClaveRecuperacion },
                                                                            protocol: Request.Scheme
                                                                        );
                string respuestaEnvioMail = !string.IsNullOrWhiteSpace(callbackurl) ? EnviarCorreo(response.Model, callbackurl) : Constantes.strMensajeErrorEnvioEnlace;
                ViewData["Mensaje"] = respuestaEnvioMail.Trim().Length == 0 ? Constantes.strMensajeEnvioEnlace : respuestaEnvioMail;
            }
            catch (Exception ex)
            {
                ViewData["HizoError"] = true;
                ViewData["Mensaje"] = Constantes.strMensajeErrorEnvioEnlace;
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.Recuperar] | UsuarioZiPago: [{0}] | Excepcion: {1}.", model.Clave1, ex.ToString());
            }
            return View("~/Views/Seguridad/Recuperar.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("Seguridad/Restablecer")]
        public async Task<IActionResult> Restablecer(string code)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            ResponseModel<UsuarioZiPago> usuario = new ResponseModel<UsuarioZiPago>();
            Uri requestUrl;
            string token;
            string clave1;
            string responseGetJson;

            try
            {
                ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
                token = Criptografia.Decoder64(Criptografia.Desencriptar(code));
                clave1 = token.Substring(token.IndexOf("|") + 1);

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Obtener) + clave1);
                responseGetJson = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);
                responseGetJson = responseGetJson.Replace("\\", string.Empty);
                responseGetJson = responseGetJson.Trim('"');
                usuario = JsonConvert.DeserializeObject<ResponseModel<UsuarioZiPago>>(responseGetJson);

                if (usuario.Model.Activo == Constantes.strValor_NoActivo)
                {
                    UsuarioViewModel model = new UsuarioViewModel {
                                                IdUsuarioZiPago = usuario.Model.IdUsuarioZiPago,
                                                Clave1 = usuario.Model.Clave1,
                                                Clave2 = string.Empty,
                                                ApellidosUsuario = string.Empty,
                                                NombresUsuario = string.Empty,
                                                AceptoTerminos = usuario.Model.AceptoTerminos,
                                                Recordarme = false
                                            };
                    ViewData["clave1"] = usuario.Model.Clave1;
                    return View("~/Views/Seguridad/Restablecer.cshtml", model);
                }
                else
                {
                    return RedirectToAction("UsuarioAutenticar", "Seguridad");
                }
            }
            catch (Exception ex)
            {
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.Recuperar] | Clave: [{0}] | Excepcion: {1}.", code, ex.ToString());
                return RedirectToAction("UsuarioAutenticar", "Seguridad");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Seguridad/Restablecer")]
        public async Task<IActionResult> Restablecer(UsuarioViewModel model)
        {
            Response response = new Response();
            Logger logger = LogManager.GetCurrentClassLogger();            
            string responseJson = "";

            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.Restablecer] | UsuarioViewModel: [{0}] | Inicio.", model.Clave1);
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            ViewData["Post"] = true;

            try
            {
                if (ModelState.IsValid)
                {

                    if (await GoogleReCaptchaValidation.ReCaptchaPassed(
                            Request.Form["g-recaptcha-response"],
                            webSettings.Value.SecretKey,
                            logger)
                        )
                    {
                        model.Clave2 = Criptografia.Encriptar(model.Clave2);
                        var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Restablecer));
                        responseJson  = await ApiClientFactory.Instance.PostJsonAsync(requestUrl, model);
                        responseJson = responseJson.Replace("\\", string.Empty);
                        responseJson = responseJson.Trim('"');

                        response = JsonConvert.DeserializeObject<Response>(responseJson);
                        ViewData["HizoError"] = response.HizoError;

                        if (!response.HizoError)
                        {
                            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioAutenticar), model.Clave1);
                            ViewData["Mensaje"] = response.Mensaje;
                        }
                        else
                        {
                            logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(Restablecer), model.Clave1, response.MensajeError);
                            ModelState.AddModelError("ErrorRestablecer", response.MensajeError);
                            
                        }
                        model.Clave2 = string.Empty;
                        ViewData["clave1"] = model.Clave1;
                        return View("~/Views/Seguridad/Restablecer.cshtml", model);
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
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(Restablecer), model.Clave1, ex.ToString());
                ModelState.AddModelError("ErrorRestablecer", ex.Message);
                return RedirectToAction("UsuarioAutenticar", "Seguridad");
            }
        }

        private string EnviarCorreo(UsuarioZiPago usuario, string callbackurl)
        {

            string respuesta = "";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            Libreria.Mail.Manage mail = new Libreria.Mail.Manage();
            Libreria.Mail.Settings mailsettings = new Libreria.Mail.Settings();
            string nombres = "";
            
            try
            {
                //nombres = string.IsNullOrEmpty(usuario.Nombres) ?
                //            usuario.NombresUsuario + " " + usuario.ApellidosUsuario :
                //                usuario.Nombres + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;

                //configuration.GetSection("ZRELZiPagoMail").Bind(mailsettings);
                //respuesta = mail.Enviar(usuario.NombresUsuario + " " + usuario.ApellidosUsuario,
                //                        usuario.Clave1,
                //                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRecuperar:Asunto"),
                //                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRecuperar:Mensaje").Replace("usuario", nombres).Replace("callbackurl", callbackurl),
                //                        mailsettings);
                if (respuesta.Trim().Length > 0)
                    logger.Error("[Aplicacion.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioViewModel: [{0}] | Mensaje: {1}.", usuario.Clave1, respuesta);
            }
            catch (Exception ex)
            {
                respuesta = ex.ToString();
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.EnviarCorreo] | UsuarioViewModel: [{0}] | Excepcion: {1}.", usuario.Clave1, ex.ToString());
            }

            return respuesta;

        }
               
    }
}