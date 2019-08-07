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
using ZREL.ZiPago.Aplicacion.Web.Utility;
using ZREL.ZiPago.Entidad.Comun;
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

    }
}