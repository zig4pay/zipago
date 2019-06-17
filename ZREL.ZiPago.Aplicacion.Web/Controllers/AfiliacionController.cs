using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers
{
    public class AfiliacionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}