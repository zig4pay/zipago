﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using ZREL.ZiPago.Aplicacion.Web.Clients;
using ZREL.ZiPago.Aplicacion.Web.Extensions;
using ZREL.ZiPago.Aplicacion.Web.Models.Response;
using ZREL.ZiPago.Aplicacion.Web.Models.Seguridad;
using ZREL.ZiPago.Aplicacion.Web.Models.Settings;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IOptions<WebSiteSettingsModel> webSettings;

        public HomeController(IOptions<WebSiteSettingsModel> app)
        {
            webSettings = app;
            Utility.ApiClientSettings.ZZiPagoApiUrl = webSettings.Value.ZZiPagoApiUrl;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            Uri requestUrl;
            ResponseSummaryModel response;
            string jsonResponse = "";

            try
            {
                if (HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session") != null)
                {
                    UsuarioViewModel usuario = HttpContext.Session.Get<UsuarioViewModel>("ZiPago.Session");

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_ComerciosObtenerCantidadPorUsuarioAsync) +
                        usuario.IdUsuarioZiPago.ToString()
                        );
                    jsonResponse = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);
                    jsonResponse = jsonResponse.Replace("\\", string.Empty);
                    jsonResponse = jsonResponse.Trim('"');
                    response = JsonConvert.DeserializeObject<ResponseSummaryModel>(jsonResponse);
                    ViewData["ComerciosCantidad"] = response.CantidadTotal;
                    ViewData["ComerciosTexto"] = Constantes.strComerciosTexto;

                    requestUrl = ApiClientFactory.Instance.CreateRequestUri(
                        string.Format(CultureInfo.InvariantCulture, webSettings.Value.AfiliacionZiPago_CuentasBancariasObtenerCantidadPorUsuarioAsync) +
                        usuario.IdUsuarioZiPago.ToString()
                        );
                    jsonResponse = await ApiClientFactory.Instance.GetJsonAsync(requestUrl);
                    jsonResponse = jsonResponse.Replace("\\", string.Empty);
                    jsonResponse = jsonResponse.Trim('"');
                    response = JsonConvert.DeserializeObject<ResponseSummaryModel>(jsonResponse);
                    ViewData["CuentasBancariasCantidad"] = response.CantidadTotal;
                    ViewData["CuentasBancariasTexto"] = Constantes.strCuentasBancariasTexto;

                    ViewData["TransaccionesCantidad"] = "0";
                    ViewData["TransaccionesTexto"] = Constantes.strTransaccionesTexto;

                    ViewData["PagosMonto"] = "S/ 0.00";
                    ViewData["PagosTexto"] = Constantes.strMontoPagosTexto;

                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    return RedirectToAction("UsuarioAutenticar", "Seguridad");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("UsuarioAutenticar", "Seguridad");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
