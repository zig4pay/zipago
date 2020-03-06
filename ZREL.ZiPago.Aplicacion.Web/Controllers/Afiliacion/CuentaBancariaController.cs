﻿using Microsoft.AspNetCore.Authorization;
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
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;
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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Uri requestUrl;
            CuentaBancariaViewModel model = new CuentaBancariaViewModel();
            ResponseListModel<EntidadGenerica> responseBanco;
            ResponseListModel<TablaDetalle> responseTD;



            try
            {

                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.BancoZiPago_Listar));
                responseBanco = await ApiClientFactory.Instance.GetListAsync<EntidadGenerica>(requestUrl);
                responseBanco.Model.Insert(0, new EntidadGenerica { IdEntidad = 0, Descripcion = "Seleccione" });
                model.Bancos = responseBanco.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoCuenta, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoCuentas = responseTD.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoMoneda, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoMonedas = responseTD.Model;

                return View("~/Views/Afiliacion/CuentaBancaria/Consulta.cshtml", model);

            }
            catch (Exception ex)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }
        }

        [HttpPost]
        [Authorize]
        [Route("CuentaBancaria/Registrar/VerificarExistenciaCuentaBancaria/")]
        public async Task<JsonResult> VerificarExistenciaCuentaBancaria(CuentaBancariaZiPago cuentabancaria)
        {
            JsonResult response;
            Uri requestUrl;
            ResponseModel<CuentaBancariaZiPago> responseCuentaBancaria = new ResponseModel<CuentaBancariaZiPago>();
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                logger.Info("[Aplicacion.Web.Controllers.Afiliacion.ComerciosController.VerificarExistenciaCuentaBancaria] | CuentaBancaria [{0}]", JsonConvert.SerializeObject(cuentabancaria));
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentaBancariaObtener));
                responseCuentaBancaria = await ApiClientFactory.Instance.PostAsync<CuentaBancariaZiPago>(requestUrl, cuentabancaria);
                if (!responseCuentaBancaria.HizoError)
                {
                    responseCuentaBancaria.Mensaje = responseCuentaBancaria.Model is null ? "NoExiste" : "Existe";
                }
                response = Json(responseCuentaBancaria);
            }
            catch (Exception ex)
            {
                response = Json("");
                logger.Error("[Aplicacion.Web.Controllers.Afiliacion.ComerciosController.VerificarExistenciaCuentaBancaria] | Error [{0}]", ex.ToString());
            }
            return response;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        [Route("CuentaBancaria/Registrar/")]
        public async Task<IActionResult> Registrar()
        {

            Uri requestUrl;
            CuentaBancariaViewModel model = new CuentaBancariaViewModel();
            ResponseListModel<EntidadGenerica> responseBanco;
            ResponseListModel<TablaDetalle> responseTD;

            try
            {
                model.IdCuentaBancaria = 0;
                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.BancoZiPago_Listar));
                responseBanco = await ApiClientFactory.Instance.GetListAsync<EntidadGenerica>(requestUrl);
                responseBanco.Model.Insert(0, new EntidadGenerica { IdEntidad = 0, Descripcion = "Seleccione" });
                model.Bancos = responseBanco.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoCuenta, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoCuentas = responseTD.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoMoneda, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoMonedas = responseTD.Model;

                ViewData["BancosAfiliados"] = webSettings.Value.BancosAfiliados_Codigos;

                return View("~/Views/Afiliacion/CuentaBancaria/Registro.cshtml", model);

            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }

        [HttpGet("{idCuentaBancaria}")]
        [Authorize]
        [Route("CuentaBancaria/Registrar/{idCuentaBancaria}")]
        public async Task<IActionResult> Registrar(int idCuentaBancaria)
        {

            Uri requestUrl;
            CuentaBancariaViewModel model = new CuentaBancariaViewModel();
            ResponseListModel<EntidadGenerica> responseBanco;
            ResponseListModel<TablaDetalle> responseTD;

            try
            {
                model.IdUsuarioZiPago = User.GetLoggedInUserId<int>();

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.BancoZiPago_Listar));
                responseBanco = await ApiClientFactory.Instance.GetListAsync<EntidadGenerica>(requestUrl);
                responseBanco.Model.Insert(0, new EntidadGenerica { IdEntidad = 0, Descripcion = "Seleccione" });
                model.Bancos = responseBanco.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoCuenta, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoCuentas = responseTD.Model;

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                responseTD.Model.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaTipoMoneda, Valor = "00", Descr_Valor = "Seleccione" });
                model.TipoMonedas = responseTD.Model;

                if (idCuentaBancaria > 0)
                {
                    ResponseModel<CuentaBancariaZiPago> responseCuenta = new ResponseModel<CuentaBancariaZiPago>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentaBancariaObtenerPorId + idCuentaBancaria.ToString()));
                    responseCuenta = await ApiClientFactory.Instance.GetAsync<CuentaBancariaZiPago>(requestUrl);

                    if (responseCuenta != null)
                    {
                        model.IdCuentaBancaria = responseCuenta.Model.IdCuentaBancaria;
                        model.IdUsuarioZiPago = responseCuenta.Model.IdUsuarioZiPago;
                        model.IdBancoZiPago = responseCuenta.Model.IdBancoZiPago;
                        model.NumeroCuenta = responseCuenta.Model.NumeroCuenta;
                        model.CodigoTipoCuenta = responseCuenta.Model.CodigoTipoCuenta;
                        model.CodigoTipoMoneda = responseCuenta.Model.CodigoTipoMoneda;
                        model.CCI = responseCuenta.Model.CCI;
                        model.Activo = responseCuenta.Model.Activo;
                    }

                }

                ViewData["BancosAfiliados"] = webSettings.Value.BancosAfiliados_Codigos;

                return View("~/Views/Afiliacion/CuentaBancaria/Registro.cshtml", model);

            }
            catch (Exception)
            {
                return View("~/Views/Seguridad/Login.cshtml");
            }

        }

        [HttpPost]
        [Authorize]
        [Route("CuentaBancaria/Registrar/RegistrarCuentasBancarias")]
        public async Task<JsonResult> RegistrarCuentasBancarias(CuentaBancariaZiPago cuentaBancaria)
        {
            JsonResult response;
            Uri requestUrl;

            try
            {         
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasRegistrar));
                response = Json(await ApiClientFactory.Instance.PostJsonAsync<CuentaBancariaZiPago>(requestUrl, cuentaBancaria));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }


    }
}