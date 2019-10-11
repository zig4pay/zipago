using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Cobranza
{
    public class PagosController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Cobranza/Pagos/Consulta.cshtml");
        }
    }
}