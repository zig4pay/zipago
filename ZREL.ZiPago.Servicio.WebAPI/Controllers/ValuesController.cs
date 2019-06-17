using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Info("[Values] Get: Realizado");
                return new string[] { "ZREL.ZiPago.Servicio.WebAPI", "v1.0", "Prueba" };
            }
            catch (Exception ex)
            {
                return new string[] { "ZREL.ZiPago.Servicio.WebAPI", "v1.0", "Excepcion", ex.Message.ToString() };
            }            
        }

    }
}
