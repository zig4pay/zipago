using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class AfiliacionController : Controller
    {

        private readonly IOptions<ApiClientSettingsModel> apiClient;

        public AfiliacionController(IOptions<ApiClientSettingsModel> app)
        {
            apiClient = app;
            ApiClientSettings.ZZiPagoUrl = apiClient.Value.ZZiPagoUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Iniciar(UsuarioViewModel model) {

            RegistroViewModel registroModel = new RegistroViewModel();
            ResponseListModel<TablaDetalle> responseTD = new ResponseListModel<TablaDetalle>(); ;
            Uri requestUrl;

            try
            {
                registroModel.IdUsuarioZiPago = model.IdUsuarioZiPago;
                registroModel.Clave1 = model.Clave1;
                registroModel.NombresUsuario = model.NombresUsuario;
                registroModel.ApellidosUsuario = model.ApellidosUsuario;
                registroModel.AceptoTerminos = model.AceptoTerminos;
                
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoPersona);                
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.TipoPersona = responseTD.Model;
                registroModel.CodigoTipoPersona = Constantes.strTipoPersonaJuridica;

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaRubroNegocio);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.RubroNegocio = responseTD.Model;


                
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("~/Views/Afiliacion/Registro.cshtml",registroModel);
        }
    }
}