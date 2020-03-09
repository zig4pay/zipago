using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            
            Uri requestUrl;
            ComercioViewModel model = new ComercioViewModel();
            ResponseListModel<BancoZiPago> responseBanco;
            
            try
            {                
                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_BancosPorUsuarioListar) +
                        model.IdUsuarioZiPago.ToString()
                        );
                responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });

                model.Bancos = responseBanco.Model;
                
                return View("~/Views/Afiliacion/Comercio/Consulta.cshtml", model);
            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }

        [HttpGet]
        [Authorize]
        [Route("Comercio/ListarCuentasBancarias/")]
        public async Task<JsonResult> ListarCuentasBancarias(int idUsuarioZiPago, int idBancoZiPago)
        {

            JsonResult response;
            Uri requestUrl;
            ResponseListModel<CuentaBancariaListaResumida> responseCuentas = new ResponseListModel<CuentaBancariaListaResumida>();
            List<SelectListItem> lista = new List<SelectListItem>();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasListarResumen) + idUsuarioZiPago.ToString() + "/" + idBancoZiPago.ToString());
                responseCuentas = await ApiClientFactory.Instance.GetListAsync<CuentaBancariaListaResumida>(requestUrl);

                foreach (CuentaBancariaListaResumida item in responseCuentas.Model)
                {
                    lista.Add(new SelectListItem { Value = item.IdCuentaBancaria.ToString(), Text = item.Descripcion });
                }
                lista.Insert(0, new SelectListItem { Value = "0", Text = "Seleccione" });

                response = Json(lista);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarComercios(string order, int idUsuarioZiPago, string codigoComercio, string descripcion, string estado, int idBancoZiPago, string numeroCuenta)
        {                        
            Uri requestUrl;            
            JsonResult response;
            ResponseListModel<ComercioListado> responseComercio = new ResponseListModel<ComercioListado>();
            string responsePostJson;
            Int32 totalRegistros;

            try
            {
                ComercioFiltros comercioFiltros = new ComercioFiltros {
                    IdUsuarioZiPago = idUsuarioZiPago,
                    CodigoComercio = codigoComercio,
                    Descripcion = descripcion,
                    Estado = estado,
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
        [Authorize]
        [Route("Comercio/Registrar/VerificarExisteComercioZiPago/")]
        public async Task<JsonResult> VerificarExisteComercioZiPago(string strCodigoComercio)
        {
            JsonResult response;
            Uri requestUrl;
            ResponseModel<ComercioZiPagoReg> responseComercio = new ResponseModel<ComercioZiPagoReg>();
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComercioObtener) + strCodigoComercio);
                logger.Info("Aplicacion.Web.Controllers.Afiliacion.ComerciosController.VerificarExisteComercioZiPago requestUrl [{0}]", requestUrl.ToString());
                responseComercio = await ApiClientFactory.Instance.GetAsync<ComercioZiPagoReg>(requestUrl);

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

        [HttpGet]
        [Authorize]
        [Route("Comercio/Registrar/")]
        public async Task<IActionResult> Registrar()
        {

            Uri requestUrl;
            ComercioViewModel model = new ComercioViewModel();
            ResponseListModel<BancoZiPago> responseBanco;
            List<SelectListItem> bancos;
            List<SelectListItem> cuentas;

            try
            {
                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();
                model.CorreoNotificacion = User.GetLoggedInUserEmail();

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_BancosPorUsuarioListar) +
                    model.IdUsuarioZiPago.ToString()
                    );
                responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                bancos = new List<SelectListItem>();
                foreach (BancoZiPago item in responseBanco.Model)
                {
                    bancos.Add(new SelectListItem { Value = item.IdBancoZiPago.ToString(), Text = item.NombreLargo });
                }
                bancos.Insert(0, new SelectListItem { Value = "0", Text = "Seleccione" });
                ViewBag.Bancos = bancos;

                cuentas = new List<SelectListItem>();
                cuentas.Insert(0, new SelectListItem { Value = "0", Text = "Seleccione" });
                ViewBag.Cuentas = cuentas;

                return View("~/Views/Afiliacion/Comercio/Registro.cshtml", model);
            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }

        //[HttpGet]
        //[Authorize]
        //[Route("Comercio/Registrar/{idComercio}")]
        //public async Task<IActionResult> Registrar(int idComercio)
        //{

        //    Uri requestUrl;
        //    ComercioViewModel model = new ComercioViewModel();
        //    ResponseListModel<BancoZiPago> responseBanco;

        //    try
        //    {
        //        model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();
        //        model.CorreoNotificacion = User.GetLoggedInUserEmail();

        //        requestUrl = ApiClientFactory.Instance.CreateRequestUri(
        //            string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_BancosPorUsuarioListar) +
        //            model.IdUsuarioZiPago.ToString()
        //            );
        //        responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
        //        responseBanco.Model.Insert(0, new BancoZiPago { IdBancoZiPago = 0, NombreLargo = "Seleccione" });

        //        model.Bancos = responseBanco.Model;

        //        if (idComercio > 0)
        //        {
        //            ResponseModel<ComercioListado> responseComercio = new ResponseModel<ComercioListado>();
        //            requestUrl = ApiClientFactory.Instance.CreateRequestUri(
        //                            string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComercioObtener + idComercio.ToString()));
        //            responseComercio = await ApiClientFactory.Instance.GetAsync<ComercioListado>(requestUrl);

        //            if (responseComercio != null)
        //            {
        //                model.IdComercioZiPagoReg = responseComercio.Model.IdComercio;
        //                model.CodigoComercio = responseComercio.Model.Codigo;
        //                model.CorreoNotificacion = responseComercio.Model.CorreoNotificacion;
        //                model.Descripcion = responseComercio.Model.Descripcion;                        
        //                model.IdBancoZiPago = responseComercio.Model.IdBancoZiPago;                        
        //                model.CodigoCuenta = responseComercio.Model.IdCuentaBancaria;                        
        //            }
        //        }

        //        return View("~/Views/Afiliacion/Comercio/Registro.cshtml", model);
        //    }
        //    catch (Exception)
        //    {
        //        return View("~/Views/Seguridad/Login.cshtml");
        //    }

        //}

        [HttpPost]
        [Authorize]
        [Route("Comercio/RegistrarComercio/")]
        public async Task<JsonResult> Registrar(List<ComercioZiPagoReg> comercios)
        {
            JsonResult responseJson;
            Uri requestUrl;
            List<ComercioCuentaZiPago> lstComercioCuenta = new List<ComercioCuentaZiPago>();
            ComercioCuentaZiPago comercioCuenta;
            ComercioZiPagoReg comercio;
            CuentaBancariaZiPago cuentaBancaria;
            string response = string.Empty;
            try
            {
                foreach (ComercioZiPagoReg item in comercios)
                {
                    comercio = new ComercioZiPagoReg{
                        IdUsuarioZiPago = item.IdUsuarioZiPago,
                        CodigoComercio = item.CodigoComercio,
                        Descripcion = item.Descripcion,
                        CorreoNotificacion = item.CorreoNotificacion
                    };

                    cuentaBancaria = new CuentaBancariaZiPago{
                        IdCuentaBancaria = item.CodigoCuenta
                    };

                    comercioCuenta = new ComercioCuentaZiPago {
                        ComercioZiPagoReg = comercio,
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