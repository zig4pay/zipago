﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class ComerciosController : Controller
    {
        public IActionResult Index()
        {            
            return View("~/Views/Afiliacion/Comercios.cshtml");
        }
    }
}