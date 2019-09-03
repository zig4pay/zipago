using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;


namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class ComerciosController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public ComerciosController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            Utility.ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            Uri requestUrl;
            ComercioViewModel model = new ComercioViewModel();
            ResponseListModel<BancoZiPago> responseBanco;
            
            try
            {
                if (HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") != null) {
                    model.IdUsuarioZiPago = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session").IdUsuarioZiPago;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_BancosPorUsuarioListar) +
                        model.IdUsuarioZiPago.ToString()
                        );
                    responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                    responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });

                    model.Bancos = responseBanco.Model;

                    return View("~/Views/Afiliacion/Comercios.cshtml", model);
                }
                else
                {
                    return View("~/Views/Seguridad/Login.cshtml");
                }
            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }

        [HttpGet]
        public async Task<JsonResult> ListarCuentasBancarias(int idUsuarioZiPago, int idBancoZiPago)
        {

            JsonResult response;
            Uri requestUrl;
            ResponseListModel<CuentaBancariaListaResumida> responseCuentas = new ResponseListModel<CuentaBancariaListaResumida>();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasListarResumen) + idUsuarioZiPago.ToString() + "/" + idBancoZiPago.ToString());
                responseCuentas = await ApiClientFactory.Instance.GetListAsync<CuentaBancariaListaResumida>(requestUrl);
                response = Json(responseCuentas.Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        [HttpPost]
        public async Task<IActionResult> ListarComercios(ComercioFiltros comercioFiltros)
        {
            Uri requestUrl;
            JsonResult response;
            ResponseListModel<ComercioListado> responseComercio = new ResponseListModel<ComercioListado>();
            string responsePostJson;

            try
            {
                int recordsTotal = 0;

                var draw = HttpContext.Request.HasFormContentType ? HttpContext.Request.Form["draw"].FirstOrDefault() : "1";
                var start = HttpContext.Request.HasFormContentType ? HttpContext.Request.Form["start"].FirstOrDefault() : "1";
                
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComerciosListar)
                    );
                responsePostJson = await ApiClientFactory.Instance.PostJsonAsync<ComercioFiltros>(requestUrl, comercioFiltros);

                responsePostJson = responsePostJson.Replace("\\", string.Empty);
                responsePostJson = responsePostJson.Trim('"');                

                responseComercio = JsonConvert.DeserializeObject<ResponseListModel<ComercioListado>>(responsePostJson);
                recordsTotal = responseComercio.Model.Count();

                //responsePostJson = Json(responseComercio.Model).ToString();
                responsePostJson = JsonConvert.SerializeObject(responseComercio.Model);

                response = Json(new { draw, recordsFiltered = recordsTotal, recordsTotal, data = responseComercio.Model });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;            
        }

        [HttpGet]
        public async Task<IActionResult> Registrar()
        {

            Uri requestUrl;
            ComercioViewModel model = new ComercioViewModel();
            ResponseListModel<BancoZiPago> responseBanco;

            try
            {
                if (HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") != null)
                {
                    model.IdUsuarioZiPago = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session").IdUsuarioZiPago;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_BancosPorUsuarioListar) +
                        model.IdUsuarioZiPago.ToString()
                        );
                    responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                    responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });

                    model.Bancos = responseBanco.Model;

                    return View("~/Views/Afiliacion/Comercio/Registro.cshtml", model);
                }
                else
                {
                    return View("~/Views/Seguridad/Login.cshtml");
                }
            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }
    }
}