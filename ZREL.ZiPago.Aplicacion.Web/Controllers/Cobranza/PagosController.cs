using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Cobranza;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Cobranza
{
    public class PagosController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public PagosController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            Logger logger = LogManager.GetCurrentClassLogger();            
            PagosViewModel model = new PagosViewModel();
            ResponseListModel<TablaDetalle> responseTD;
            ResponseListModel<EntidadGenerica> response;
            
            Uri requestUrl;            

            try
            {
                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();

                // Comercios
                response = new ResponseListModel<EntidadGenerica>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComerciosListar) + "/" + User.GetLoggedInUserId<int>().ToString());
                response = await ApiClientFactory.Instance.GetListAsync<EntidadGenerica>(requestUrl);
                model.Comercios = response.Model;

                // Servicios
                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaServiciosRecaudacion);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                model.Servicios = responseTD.Model;

                // Estados TXN
                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaEstadoTransaccion);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                model.EstadosTxn = responseTD.Model;

            }
            catch (Exception ex)
            {
                throw ex;                                
            }

            return View("~/Views/Cobranza/Pagos/Consulta.cshtml", model);
        }
    }
}