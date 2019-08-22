using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

                    return View("~/Views/Afiliacion/CuentaBancaria.cshtml", model);
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

        [HttpPost]
        public async Task<IActionResult> ListarCuentasBancarias([FromBody] UsuarioZiPago usuarioZiPago)
        {
             
            JsonResult response;
            Uri requestUrl;
            ResponseListModel<CuentaBancariaListado> responseCuentas = new ResponseListModel<CuentaBancariaListado>();

            try
            {
                var draw = "1"; //HttpContext.Request.Form["draw"].FirstOrDefault();
                //var start = Request.Form["start"].FirstOrDefault();
                //var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();                
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();                
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasListar) + "1"
                        //usuarioZiPago.IdUsuarioZiPago.ToString()
                    );
                responseCuentas = await ApiClientFactory.Instance.GetListAsync<CuentaBancariaListado>(requestUrl);                
                recordsTotal = responseCuentas.Model.ToList().Count();

                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}                
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    customerData = customerData.Where(m => m.Name == searchValue || m.Phoneno == searchValue || m.City == searchValue);
                //}

                response = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = responseCuentas.Model });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
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