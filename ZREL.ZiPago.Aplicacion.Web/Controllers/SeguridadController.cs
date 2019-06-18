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

        private readonly IOptions<ApiClientSettingsModel> apiClientSettingsModel;

        public SeguridadController(IOptions<ApiClientSettingsModel> app)
        {
            apiClientSettingsModel = app;
            ApiClientSettings.ZZiPagoUrl = apiClientSettingsModel.Value.ZZiPagoUrl;
            ApiClientSettings.UsuarioZiPago_Autenticar = apiClientSettingsModel.Value.UsuarioZiPago_Autenticar;
            ApiClientSettings.UsuarioZiPago_Registrar = apiClientSettingsModel.Value.UsuarioZiPago_Registrar;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UsuarioAutenticar(UsuarioViewModel model)
        {

            ResponseModel<UsuarioViewModel> response;

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioAutenticar), model.Clave1);

            try
            {
                model.Clave2 = Criptografia.Encriptar(model.Clave2);

                var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, ApiClientSettings.UsuarioZiPago_Autenticar));
                response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                return Json(response);

            }
            catch (Exception ex)
            {
                logger.Error("[{0}] | UsuarioViewModel: [{1}] | Excepcion: {2}.", nameof(UsuarioAutenticar), model.Clave1, ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsuarioRegistrar()
        {
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UsuarioRegistrar(UsuarioViewModel model)
        {

            ResponseModel<UsuarioViewModel> response = new ResponseModel<UsuarioViewModel>();

            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UsuarioViewModel: [{1}] | Inicio.", nameof(UsuarioRegistrar), model.Clave1);

            try
            {                
                if (ModelState.IsValid)
                {
                    model.Clave2 = Criptografia.Encriptar(model.Clave2);
                    model.AceptoTerminos = Constantes.strUsuarioZiPago_AceptoTerminos;
                    var requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClientSettingsModel.Value.UsuarioZiPago_Registrar));
                    response = await ApiClientFactory.Instance.PostAsync<UsuarioViewModel>(requestUrl, model);

                    if (!response.HizoError) {                        
                        logger.Info("[{0}] | UsuarioViewModel: [{1}] | Realizado.", nameof(UsuarioRegistrar), model.Clave1);
                        return View("~/Views/Afiliacion/Index.cshtml", response.Model);                        
                    }
                    else {                        
                        logger.Info("[{0}] | UsuarioViewModel: [{1}] | " + response.Mensaje, nameof(UsuarioRegistrar), model.Clave1);
                        ModelState.AddModelError("ErrorRegistro", response.MensajeError);
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