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
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Aplicacion.Web.Request;
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class DatosPersonalesController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public DatosPersonalesController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            UsuarioViewModel usuario = new UsuarioViewModel();
            DatosPersonalesViewModel model = new DatosPersonalesViewModel();
            ResponseListModel<TablaDetalle> responseTD;
            ResponseListModel<UbigeoZiPago> response;
            ResponseModel<DatosPersonales> responseDatos;
            Uri requestUrl;
            int posicion = 0;

            try
            {
                if (HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session") != null)
                {
                    usuario = HttpContext.Session.Get<ResponseModel<UsuarioViewModel>>("ZiPago.Session").Model;
                    
                    // Tipo de Persona
                    responseTD = new ResponseListModel<TablaDetalle>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaTipoPersona);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    model.TipoPersona = responseTD.Model;

                    // Rubro de Negocio
                    responseTD = new ResponseListModel<TablaDetalle>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.TablaDetalle_Listar) + Constantes.strCodTablaRubroNegocio);
                    responseTD = await ApiClientFactory.Instance.GetListAsync<TablaDetalle>(requestUrl);
                    model.RubroNegocio = responseTD.Model;
                    model.RubroNegocio.Insert(0, new TablaDetalle
                    {
                        Cod_Tabla = Constantes.strCodTablaRubroNegocio,
                        Valor = "000",
                        Descr_Valor = "Seleccione"
                    });

                    // Departamento
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.UbigeoZiPago_Listar) + Constantes.strUbigeoZiPago_Departamentos);
                    response = await ApiClientFactory.Instance.GetListAsync<UbigeoZiPago>(requestUrl);
                    model.Departamento = response.Model;
                    model.Departamento.Insert(0, new UbigeoZiPago
                    {
                        CodigoUbigeo = "XX",
                        Nombre = "Seleccione"
                    });

                    // Datos registrados
                    responseDatos = new ResponseModel<DatosPersonales>();
                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                                    string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_DatosPersonalesObtener + usuario.IdUsuarioZiPago.ToString()));
                    responseDatos = await ApiClientFactory.Instance.GetAsync<DatosPersonales>(requestUrl);

                    if (responseDatos.Model != null)
                    {
                        model.IdUsuarioZiPago = responseDatos.Model.IdUsuarioZiPago;
                        model.Clave1 = responseDatos.Model.Clave1;
                        model.ApellidosUsuario = responseDatos.Model.ApellidosUsuario;
                        model.NombresUsuario = responseDatos.Model.NombresUsuario;                                              
                        
                        model.CodigoTipoPersona = responseDatos.Model.CodigoTipoPersona == "" ? Constantes.strTipoPersonaJuridica : responseDatos.Model.CodigoTipoPersona;
                        model.CodigoRubroNegocio = responseDatos.Model.CodigoRubroNegocio;
                        model.NumeroDocumento = responseDatos.Model.NumeroDocumento;
                        model.RazonSocial = responseDatos.Model.RazonSocial;
                        model.NumeroDocumentoContacto = responseDatos.Model.NumeroDocumentoContacto;
                        model.Nombres = string.IsNullOrEmpty(responseDatos.Model.Nombres) ? usuario.NombresUsuario : responseDatos.Model.Nombres;
                        posicion = string.IsNullOrEmpty(responseDatos.Model.ApellidoPaterno) ? usuario.ApellidosUsuario.IndexOf(" ") : -1;
                        model.ApellidoPaterno = string.IsNullOrEmpty(responseDatos.Model.ApellidoPaterno) ? posicion > 0 ? usuario.ApellidosUsuario.Substring(0, posicion) : usuario.ApellidosUsuario : responseDatos.Model.ApellidoPaterno;
                        model.ApellidoMaterno = string.IsNullOrEmpty(responseDatos.Model.ApellidoMaterno) ? posicion > 0 ? usuario.ApellidosUsuario.Substring(posicion + 1) : "" : responseDatos.Model.ApellidoMaterno;
                        model.Sexo = responseDatos.Model.Sexo;
                        model.FechaNacimiento = Convert.ToDateTime(responseDatos.Model.FechaNacimiento.Value.ToShortDateString());
                        model.TelefonoFijo = responseDatos.Model.TelefonoFijo;
                        model.TelefonoMovil = responseDatos.Model.TelefonoMovil;
                        model.CodigoDepartamento = responseDatos.Model.CodigoDepartamento;
                        model.CodigoProvincia = responseDatos.Model.CodigoProvincia;
                        model.CodigoDistrito = responseDatos.Model.CodigoDistrito;
                        model.Via = responseDatos.Model.Via;
                        model.DireccionFacturacion = responseDatos.Model.DireccionFacturacion;
                        model.Referencia = responseDatos.Model.Referencia;
                        model.AceptoTerminos = responseDatos.Model.AceptoTerminos;
                    }
                    else
                    {
                        return View("~/Views/Seguridad/Login.cshtml");
                    }
                    return View("~/Views/Afiliacion/DatosPersonales.cshtml", model);
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
        public async Task<JsonResult> Registrar(DatosPersonalesViewModel model)
        {

            JsonResult response;
            Uri requestUrl;
            
            try
            {
                DatosPersonalesRequest request = new DatosPersonalesRequest();
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
                    usuario.NumeroDocumento = model.NumeroDocumento;
                    usuario.RazonSocial = model.RazonSocial;
                    usuario.CodigoTipoDocumentoContacto = Constantes.strTipoDocIdDNI;
                    usuario.NumeroDocumentoContacto = model.NumeroDocumentoContacto;
                }
                else
                {
                    usuario.CodigoTipoDocumento = Constantes.strTipoDocIdDNI;
                    usuario.NumeroDocumento = model.NumeroDocumento;
                    usuario.CodigoTipoDocumentoContacto = Constantes.strTipoDocIdDNI;
                    usuario.NumeroDocumentoContacto = model.NumeroDocumentoContacto;
                }

                usuario.ApellidoPaterno = model.ApellidoPaterno;
                usuario.ApellidoMaterno = model.ApellidoMaterno;
                usuario.Nombres = model.Nombres;
                usuario.Sexo = model.Sexo;
                usuario.FechaNacimiento = model.FechaNacimiento;
                usuario.TelefonoMovil = model.TelefonoMovil;
                usuario.TelefonoFijo = model.TelefonoFijo;
                usuario.FechaActualizacion = DateTime.Now;

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
                response = Json(await ApiClientFactory.Instance.PostJsonAsync<DatosPersonalesRequest>(requestUrl, request));
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;

        }
        
    }
}