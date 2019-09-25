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

                            logger.Info("[Aplicacion.Web.Controllers.SeguridadController.{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioAutenticar), model.Clave1);
                            //HttpContext.Session.Set<UsuarioViewModel>("ZiPago.Session", response.Model);
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
        [Route("Seguridad/Recuperar/{correo}")]
        public async Task<JsonResult> Recuperar(string correo) {

            Logger logger = LogManager.GetCurrentClassLogger();
            Uri requestUrl;
            ResponseModel<UsuarioZiPago> response = new ResponseModel<UsuarioZiPago>();
            string responseGetJson;
            JsonResult result;
            
            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Recuperar) + correo);
                responseGetJson = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);
                responseGetJson = responseGetJson.Replace("\\", string.Empty);
                responseGetJson = responseGetJson.Trim('"');
                response = JsonConvert.DeserializeObject<ResponseModel<UsuarioZiPago>>(responseGetJson);

                if (!response.HizoError) {
                    var callbackurl = Url.Action(
                                            controller: "Seguridad",
                                            action: "Restablecer",
                                            values: new { code = response.Model.ClaveRecuperacion },
                                            protocol: Request.Scheme
                                        );
                    EnviarCorreo(response.Model, callbackurl);
                }

                response.Mensaje = "Se realizo el envio de un enlace a su correo electronico para que pueda restablecer su contrasena.";
                response.Model = null;

                result = Json(response);
            }
            catch (Exception ex)
            {
                response.HizoError = true;
                response.MensajeError = ex.Message;
                result = Json(response);
                logger.Error("[Aplicacion.Web.Controllers.SeguridadController.Recuperar] | UsuarioZiPago: [{0}] | Excepcion: {1}.", correo, ex.ToString());
            }

            return result;
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
                                                Clave1 = usuario.Model.Clave1
                                                };
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
        
        private string EnviarCorreo(UsuarioZiPago usuario, string callbackurl)
        {

            string respuesta = "";
            var logger = NLog.LogManager.GetCurrentClassLogger();
            Libreria.Mail.Manage mail = new Libreria.Mail.Manage();
            Libreria.Mail.Settings mailsettings = new Libreria.Mail.Settings();
            string nombres = "";
            
            try
            {
                nombres = string.IsNullOrEmpty(usuario.Nombres) ?
                            usuario.NombresUsuario + " " + usuario.ApellidosUsuario :
                                usuario.Nombres + " " + usuario.ApellidoPaterno + " " + usuario.ApellidoMaterno;

                configuration.GetSection("ZRELZiPagoMail").Bind(mailsettings);
                respuesta = mail.Enviar(usuario.NombresUsuario + " " + usuario.ApellidosUsuario,
                                        usuario.Clave1,
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRecuperar:Asunto"),
                                        configuration.GetValue<string>("ZRELZiPagoCuerpoMailRecuperar:Mensaje").Replace("usuario", nombres).Replace("callbackurl", callbackurl),
                                        mailsettings);
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