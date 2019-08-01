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
            ApiClientSettings.ZZiPagoUrl = webSettings.Value.ZZiPagoUrl;
        }

        [HttpGet]        
        public IActionResult UsuarioRegistrar()
        {
            ViewData["ReCaptchaKey"] = webSettings.Value.SiteKey;
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]        
        public async Task<string> UsuarioRegistrar(UsuarioViewModel model)
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
                            ModelState.Clear();
                        }
                        else
                        {
                            logger.Error("[{0}] | UsuarioViewModel: [{1}] | " + response.Mensaje, nameof(UsuarioRegistrar), model.Clave1);
                        }
                        var respuesta = JsonConvert.SerializeObject(response);
                        return respuesta;
                    }
                    else
                    {
                        response.HizoError = true;
                        response.MensajeError = "No hemos podido validar que no seas un robot.";
                        return JsonConvert.SerializeObject(response);
                    }
                }
                else
                {
                    response.HizoError = true;
                    response.MensajeError = "Ingrese correctamente todos los datos solicitados.";
                    return JsonConvert.SerializeObject(response);                    
                }
            }
            catch (Exception ex)
            {
                response.HizoError = true;
                response.MensajeError = ex.ToString();
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioRegistrar), model.Clave1, ex.ToString());                
                return JsonConvert.SerializeObject(response);
            }

        }

    }
}