﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZREL.ZiPago.Aplicacion.Web.Models;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return View("~/Views/Seguridad/Registro.cshtml");
            return View("~/Views/Seguridad/Login.cshtml");
            //return View("~/Views/Afiliacion/Index.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}