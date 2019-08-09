using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Request;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class UsuarioController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public UsuarioController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Models.Seguridad.UsuarioViewModel usuario = new Models.Seguridad.UsuarioViewModel();
            UsuarioViewModel model = new UsuarioViewModel();
            ResponseListModel<TablaDetalle> responseTD;
            ResponseListModel<UbigeoZiPago> response;
            Uri requestUrl;
            int posicion = 0;

            try
            {
                if (HttpContext.Session.Get<ResponseModel<Models.Seguridad.UsuarioViewModel>>("ZiPago.Session") != null)
                {
                    usuario = HttpContext.Session.Get<ResponseModel<Models.Seguridad.UsuarioViewModel>>("ZiPago.Session").Model;

                    model.Clave1 = usuario.Clave1;
                    model.IdUsuarioZiPago = usuario.IdUsuarioZiPago;
                    model.ApellidosUsuario = usuario.ApellidosUsuario;
                    model.NombresUsuario = usuario.NombresUsuario;
                    model.Nombres = usuario.NombresUsuario;

                    posicion = usuario.ApellidosUsuario.IndexOf(" ");
                    if (posicion > 0)
                    {
                        model.ApellidoPaterno = usuario.ApellidosUsuario.Substring(0, posicion);
                        model.ApellidoMaterno = usuario.ApellidosUsuario.Substring(posicion + 1);
                    }
                    else
                    {
                        model.ApellidoPaterno = usuario.ApellidosUsuario;
                    }
                    model.AceptoTerminos = usuario.AceptoTerminos;

                    responseTD = new ResponseListModel<TablaDetalle>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoPersona);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    model.TipoPersona = responseTD.Model;
                    model.CodigoTipoPersona = Constantes.strTipoPersonaJuridica;

                    responseTD = new ResponseListModel<TablaDetalle>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaRubroNegocio);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    model.RubroNegocio = responseTD.Model;
                    model.RubroNegocio.Insert(0, new TablaDetalle { Cod_Tabla = Constantes.strCodTablaRubroNegocio,
                                                                    Valor = "000",
                                                                    Descr_Valor = "Seleccione"
                                                                  });

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UbigeoZiPago_Listar) + Constantes.strUbigeoZiPago_Departamentos);
                    response = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                    model.Departamento = response.Model;
                    model.Departamento.Insert(0, new UbigeoZiPago { CodigoUbigeo = "XX",
                                                                    Nombre = "Seleccione"
                                                                  });



                    return View("~/Views/Afiliacion/Usuario.cshtml", model);
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
        public async Task<JsonResult> ListarPorUbigeo(string strCodigoUbigeo)
        {

            JsonResult response;
            Uri requestUrl;
            ResponseListModel<UbigeoZiPago> responseUbigeo = new ResponseListModel<UbigeoZiPago>();

            try
            {
                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.UbigeoZiPago_Listar) + strCodigoUbigeo);
                responseUbigeo = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                response = Json(responseUbigeo.Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        [HttpPost]
        public async Task<JsonResult> Registrar(UsuarioViewModel model)
        {

            JsonResult response;
            Uri requestUrl;
            
            try
            {
                AfiliacionDatosRequest request = new AfiliacionDatosRequest();
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

                requestUrl = ApiClientFactory.Instance.CreateRequestUri(string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_Registrar));
                response = Json(await ApiClientFactory.Instance.PostJsonAsync<AfiliacionDatosRequest>(requestUrl, request));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }


    }
}