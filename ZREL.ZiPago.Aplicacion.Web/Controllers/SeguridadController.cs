using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        public async Task<IActionResult> Recuperar(string clave1) {

            Logger logger = LogManager.GetCurrentClassLogger();
            Uri requestUrl;
            string response = "";
            
            try
            {

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UsuarioZiPago_Recuperar) + clave1);
                response = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);



            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}