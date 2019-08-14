using Microsoft.AspNetCore.Mvc;

namespace ZREL.ZiPago.Aplicacion.Web.Controllers.Afiliacion
{
    public class CuentaBancariaController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Afiliacion/CuentaBancaria.cshtml");
        }
    }
}