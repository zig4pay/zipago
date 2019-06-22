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
            ResponseListModel<TablaDetalle> responseTD ;
            ResponseListModel<UbigeoZiPago> response = new ResponseListModel<UbigeoZiPago>();
            ResponseListModel<BancoZiPago> responseBanco = new ResponseListModel<BancoZiPago>();
            Uri requestUrl;

            try
            {
                registroModel.IdUsuarioZiPago = model.IdUsuarioZiPago;
                registroModel.Clave1 = model.Clave1;
                registroModel.NombresUsuario = model.NombresUsuario;
                registroModel.ApellidosUsuario = model.ApellidosUsuario;
                registroModel.AceptoTerminos = model.AceptoTerminos;

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoPersona);                
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.TipoPersona = responseTD.Model;
                registroModel.CodigoTipoPersona = Constantes.strTipoPersonaJuridica;

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaRubroNegocio);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.RubroNegocio = responseTD.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.UbigeoZiPago_Listar) + Constantes.strUbigeoZiPago_Departamentos);
                response = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                registroModel.Departamento = response.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.BancoZiPago_Listar));
                responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                registroModel.Banco = responseBanco.Model;

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.TipoCuenta = responseTD.Model;

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.Moneda = responseTD.Model;


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View("~/Views/Afiliacion/Registro.cshtml",registroModel);
        }

        [HttpGet]
        public async Task<JsonResult> ListarPorUbigeo(string strCodigoUbigeo) {

            JsonResult response;
            Uri requestUrl;
            ResponseListModel<UbigeoZiPago> responseUbigeo = new ResponseListModel<UbigeoZiPago>();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.UbigeoZiPago_Listar) + strCodigoUbigeo);
                responseUbigeo = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                response = Json(responseUbigeo.Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

    }
}