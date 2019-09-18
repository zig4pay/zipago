using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class CuentaBancariaController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public CuentaBancariaController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Uri requestUrl;            
            CuentaBancariaViewModel model = new CuentaBancariaViewModel();
            ResponseListModel<BancoZiPago> responseBanco;
            ResponseListModel<TablaDetalle> responseTD;

            try
            {
                if (HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") != null) {

                    model.IdUsuarioZiPago = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session").IdUsuarioZiPago;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.BancoZiPago_Listar));
                    responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                    responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });
                    model.Bancos = responseBanco.Model;
                    
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    responseTD.Model.Insert(0, new TablaDetalle {Cod_Tabla = Constantes.strCodTablaTipoCuenta, Valor = "00", Descr_Valor = "Seleccione"});
                    model.TipoCuentas = responseTD.Model;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoMoneda, Valor = "00", Descr_Valor = "Seleccione" });
                    model.TipoMonedas = responseTD.Model;

                    return View("~/Views/Afiliacion/CuentaBancaria/Consulta.cshtml", model);
                }
                else
                {
                    return View("~/Views/Seguridad/Login.cshtml");
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }            
        }

        [HttpGet]
        public async Task<IActionResult> Listar(string order, int idUsuarioZiPago, int idBancoZiPago, string codigoTipoCuenta, string codigoTipoMoneda, string activo, string numeroCuenta)
        {
            Uri requestUrl;
            JsonResult response;
            ResponseListModel<CuentaBancariaListado> responseCuenta = new ResponseListModel<CuentaBancariaListado>();
            string responsePostJson;
            Int32 totalRegistros = 0;

            try
            {
                CuentaBancariaFiltros cuentaBancariaFiltros = new CuentaBancariaFiltros
                {
                    IdUsuarioZiPago = idUsuarioZiPago,
                    IdBancoZiPago = idBancoZiPago,
                    CodigoTipoCuenta = codigoTipoCuenta,
                    CodigoTipoMoneda = codigoTipoMoneda,
                    NumeroCuenta = numeroCuenta,
                    Activo = activo                   
                };

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasListar)
                    );
                responsePostJson = await ApiClientFactory.Instance.PostJsonAsync<CuentaBancariaFiltros>(requestUrl, cuentaBancariaFiltros);

                responsePostJson = responsePostJson.Replace("\\", string.Empty);
                responsePostJson = responsePostJson.Trim('"');

                responseCuenta = JsonConvert.DeserializeObject<ResponseListModel<CuentaBancariaListado>>(responsePostJson);
                totalRegistros = responseCuenta.Model.Count();

                responsePostJson = JsonConvert.SerializeObject(responseCuenta.Model);

                response = Json(new { total = totalRegistros, totalNotFiltered = totalRegistros, rows = responseCuenta.Model });
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
            CuentaBancariaViewModel model = new CuentaBancariaViewModel();
            ResponseListModel<BancoZiPago> responseBanco;
            ResponseListModel<TablaDetalle> responseTD;

            try
            {
                if (HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") != null)
                {
                    model.IdUsuarioZiPago = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session").IdUsuarioZiPago;
                    
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.BancoZiPago_Listar));
                    responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                    responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });
                    model.Bancos = responseBanco.Model;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoCuenta, Valor = "00", Descr_Valor = "Seleccione" });
                    model.TipoCuentas = responseTD.Model;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoMoneda, Valor = "00", Descr_Valor = "Seleccione" });
                    model.TipoMonedas = responseTD.Model;

                    return View("~/Views/Afiliacion/CuentaBancaria/Registro.cshtml", model);
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

        [HttpPost]
        public async Task<JsonResult> RegistrarCuentasBancarias(List<CuentaBancariaZiPago> cuentasBancarias)
        {
            JsonResult response;
            Uri requestUrl;

            try
            {         
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasRegistrar));
                response = Json(await ApiClientFactory.Instance.PostJsonAsync<List<CuentaBancariaZiPago>>(requestUrl, cuentasBancarias));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }
    }
}