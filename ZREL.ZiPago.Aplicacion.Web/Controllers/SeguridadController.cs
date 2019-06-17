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
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult UsuarioRegistrar()
        {
            return View("~/Views/Seguridad/Registro.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UsuarioAutenticar(UsuarioViewModel model) {

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

    }
}