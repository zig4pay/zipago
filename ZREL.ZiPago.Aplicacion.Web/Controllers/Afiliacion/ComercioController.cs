using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
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
    public class ComercioController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public ComercioController(IOptions<WebSiteSettingsModel> app)
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

                    return View("~/Views/Afiliacion/Comercio/Consulta.cshtml", model);
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

        [HttpGet]
        public async Task<IActionResult> ListarComercios(string order, int idUsuarioZiPago, string codigoComercio, string descripcion, string activo, int idBancoZiPago, string numeroCuenta)
        {                        
            Uri requestUrl;            
            JsonResult response;
            ResponseListModel<ComercioListado> responseComercio = new ResponseListModel<ComercioListado>();
            string responsePostJson;
            Int32 totalRegistros = 0;

            try
            {

                ComercioFiltros comercioFiltros = new ComercioFiltros {
                    IdUsuarioZiPago = idUsuarioZiPago,
                    CodigoComercio = codigoComercio,
                    Descripcion = descripcion,
                    Activo = activo,
                    IdBancoZiPago = idBancoZiPago,
                    NumeroCuenta = numeroCuenta
                };

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComerciosListar)
                    );
                responsePostJson = await ApiClientFactory.Instance.PostJsonAsync<ComercioFiltros>(requestUrl, comercioFiltros);

                responsePostJson = responsePostJson.Replace("\\", string.Empty);
                responsePostJson = responsePostJson.Trim('"');                

                responseComercio = JsonConvert.DeserializeObject<ResponseListModel<ComercioListado>>(responsePostJson);
                totalRegistros = responseComercio.Model.Count();
                                
                responsePostJson = JsonConvert.SerializeObject(responseComercio.Model);

                response = Json(new { total = totalRegistros, totalNotFiltered = totalRegistros, rows = responseComercio.Model });

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
                    model.Clave1 = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session").Clave1;

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

        [HttpGet]
        public async Task<JsonResult> VerificarExisteComercioZiPago(string strCodigoComercio)
        {
            JsonResult response;
            Uri requestUrl;
            ResponseModel<ComercioZiPago> responseComercio = new ResponseModel<ComercioZiPago>();
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComercioObtener) + strCodigoComercio);
                logger.Info("Aplicacion.Web.Controllers.Afiliacion.ComerciosController.VerificarExisteComercioZiPago requestUrl [{0}]", requestUrl.ToString());
                responseComercio = await ApiClientFactory.Instance.GetAsync<ComercioZiPago>(requestUrl);

                if (!responseComercio.HizoError)
                {                    
                    responseComercio.Mensaje = responseComercio.Model is null ? "NoExiste" : "Existe";                    
                }

                response = Json(responseComercio);                
            }
            catch (Exception ex)
            {
                response = Json("");
                logger.Error("VerificarExisteComercioZiPago Error [0]", ex.ToString());
            }

            return response;
        }

        [HttpPost]
        public async Task<JsonResult> Registrar(List<ComercioZiPago> comercios)
        {
            JsonResult responseJson;
            Uri requestUrl;
            List<ComercioCuentaZiPago> lstComercioCuenta = new List<ComercioCuentaZiPago>();
            ComercioCuentaZiPago comercioCuenta;
            ComercioZiPago comercio;
            CuentaBancariaZiPago cuentaBancaria;
            string response = string.Empty;

            try
            {

                foreach (ComercioZiPago item in comercios)
                {
                    comercio = new ComercioZiPago{
                        IdUsuarioZiPago = item.IdUsuarioZiPago,
                        CodigoComercio = item.CodigoComercio,
                        Descripcion = item.Descripcion,
                        CorreoNotificacion = item.CorreoNotificacion
                    };

                    cuentaBancaria = new CuentaBancariaZiPago{
                        IdCuentaBancaria = item.CodigoCuenta
                    };

                    comercioCuenta = new ComercioCuentaZiPago {
                        ComercioZiPago = comercio,
                        CuentaBancariaZiPago = cuentaBancaria
                    };

                    lstComercioCuenta.Add(comercioCuenta);
                }

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComerciosRegistrar));
                response = await ApiClientFactory.Instance.PostJsonAsync<List<ComercioCuentaZiPago>>(requestUrl, lstComercioCuenta);
                response = response.ToString().Replace("\\", string.Empty);
                response = response.Trim('"');

                responseJson = Json(response);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseJson;

        }

    }
}