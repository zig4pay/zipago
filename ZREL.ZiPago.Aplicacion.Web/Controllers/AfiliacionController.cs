using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Request;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class AfiliacionController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> apiClient;

        public AfiliacionController(IOptions<WebSiteSettingsModel> app)
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
            int intPos;

            try
            {
                registroModel.IdUsuarioZiPago = model.IdUsuarioZiPago;
                registroModel.Clave1 = model.Clave1;
                registroModel.NombresUsuario = model.NombresUsuario;
                registroModel.ApellidosUsuario = model.ApellidosUsuario;
                registroModel.Nombres = model.NombresUsuario;

                intPos = model.ApellidosUsuario.IndexOf(" ");
                if (intPos > 0)
                {
                    registroModel.ApellidoPaterno = model.ApellidosUsuario.Substring(0, intPos);
                    registroModel.ApellidoMaterno = model.ApellidosUsuario.Substring(intPos + 1);
                }
                else
                {
                    registroModel.ApellidoPaterno = model.ApellidosUsuario;
                }
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
                registroModel.RubroNegocio.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaRubroNegocio,
                                                                        Valor = "000",
                                                                        Descr_Valor = "Seleccione"});

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.UbigeoZiPago_Listar) + Constantes.strUbigeoZiPago_Departamentos);
                response = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                registroModel.Departamento = response.Model;
                registroModel.Departamento.Insert(0, new UbigeoZiPago {CodigoUbigeo = "XX",                    
                                                                       Nombre = "Seleccione"});

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.BancoZiPago_Listar));
                responseBanco = await ApiClientFactory.Instance.GetListAsync<BancoZiPago>(requestUrl);
                registroModel.Banco = responseBanco.Model;
                registroModel.Banco.Insert(0, new BancoZiPago
                {
                    IdBancoZiPago = 0,
                    NombreLargo = "Seleccione"
                });

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoCuenta);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.TipoCuenta = responseTD.Model;
                registroModel.TipoCuenta.Insert(0, new TablaDetalle
                {
                    Cod_Tabla = Constantes.strCodTablaTipoCuenta,
                    Valor = "00",
                    Descr_Valor = "Seleccione"
                });

                responseTD = new ResponseListModel<TablaDetalle>();
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoMoneda);
                responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                registroModel.Moneda = responseTD.Model;
                registroModel.Moneda.Insert(0, new TablaDetalle
                {
                    Cod_Tabla = Constantes.strCodTablaTipoMoneda,
                    Valor = "00",
                    Descr_Valor = "Seleccione"
                });

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

        [HttpGet]
        public async Task<JsonResult> VerificarExisteComercioZiPago(string strCodigoComercio)
        {
            JsonResult response;
            Uri requestUrl;
            ResponseModel<ComercioZiPago> responseComercio = new ResponseModel<ComercioZiPago>();
            var logger = LogManager.GetCurrentClassLogger();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.AfiliacionZiPago_ComercioObtener) + strCodigoComercio);
                logger.Info("VerificarExisteComercioZiPago requestUrl [{0}]", requestUrl.ToString());
                responseComercio = await ApiClientFactory.Instance.GetAsync<ComercioZiPago>(requestUrl);

                if (!responseComercio.HizoError)
                {
                    logger.Info("VerificarExisteComercioZiPago [1]");
                    responseComercio.Mensaje = responseComercio.Model is null ? "NoExiste":"Existe";
                    logger.Info("VerificarExisteComercioZiPago [2]");
                }
                
                response = Json(responseComercio);
                logger.Info("VerificarExisteComercioZiPago [1]");
            }
            catch (Exception ex)
            {
                response = Json("");
                logger.Error("VerificarExisteComercioZiPago Error [0]", ex.ToString());
            }

            return response;
        }
               
        [HttpPost]
        public async Task<JsonResult> Registrar(RegistroViewModel model) {

            JsonResult response;
            Uri requestUrl;
            ComercioCuentaZiPago comercioCuenta;

            try
            {
                AfiliacionZiPagoRequest request = new AfiliacionZiPagoRequest();
                UsuarioZiPago usuario = new UsuarioZiPago();
                DomicilioZiPago domicilio = new DomicilioZiPago();

                usuario.IdUsuarioZiPago = model.IdUsuarioZiPago;
                usuario.Clave1 = model.Clave1;
                usuario.ApellidosUsuario = model.ApellidosUsuario;
                usuario.NombresUsuario = model.NombresUsuario;
                usuario.CodigoRubroNegocio = model.CodigoRubroNegocio;
                usuario.CodigoTipoPersona = model.CodigoTipoPersona;

                if (model.CodigoTipoPersona == Constantes.strTipoPersonaJuridica)
                {
                    usuario.CodigoTipoDocumento = Constantes.strTipoDocIdRUC;
                    usuario.NumeroDocumento = model.NumeroRUC;
                    usuario.RazonSocial = model.RazonSocial;
                }
                else
                {
                    usuario.CodigoTipoDocumento = Constantes.strTipoDocIdDNI;
                    usuario.NumeroDocumento = model.NumeroDNI;                    
                }

                usuario.ApellidoPaterno = model.ApellidoPaterno;
                usuario.ApellidoMaterno = model.ApellidoMaterno;
                usuario.Nombres = model.Nombres;
                usuario.Sexo = model.Sexo;
                usuario.FechaNacimiento = model.FechaNacimiento;
                usuario.TelefonoMovil = model.TelefonoMovil;
                usuario.TelefonoFijo = model.TelefonoFijo;
                
                domicilio.IdUsuarioZiPago = model.IdUsuarioZiPago;
                domicilio.CodigoDepartamento = model.CodigoDepartamento;
                domicilio.CodigoProvincia = model.CodigoProvincia;
                domicilio.CodigoDistrito = model.CodigoDistrito;
                domicilio.Via = model.Via;
                domicilio.DireccionFacturacion = model.DireccionFacturacion;
                domicilio.Referencia = model.Referencia;

                //----------------Luego mover a Servicio (Negocio)
                domicilio.Activo = Constantes.strValor_Activo;
                domicilio.FechaCreacion = DateTime.Now;

                request.EntidadUsuario = usuario;
                request.OtroRubroNegocio = model.OtroRubroNegocio;
                request.EntidadDomicilio = domicilio;

                foreach (CuentaBancariaZiPago cuenta in model.CuentasBancariaZiPago)
                {                    
                    foreach (ComercioZiPago comercio in model.ComerciosZiPago)
                    {
                        if (comercio.CodigoCuenta == cuenta.CodigoCuenta)
                        {
                            comercioCuenta = new ComercioCuentaZiPago
                            {
                                ComercioZiPago = comercio,
                                CuentaBancariaZiPago = cuenta                                
                            };
                            request.ListComercioCuenta.Add(comercioCuenta);
                        }
                    }
                }

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, apiClient.Value.AfiliacionZiPago_Registrar));                
                response = Json(await ApiClientFactory.Instance.PostJsonAsync<AfiliacionZiPagoRequest>(requestUrl, request));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }

    }
}