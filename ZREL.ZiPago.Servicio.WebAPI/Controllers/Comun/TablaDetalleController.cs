using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Servicio.WebAPI.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers.Comun
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/TablaDetalle")]
    [ApiController]
    public class TablaDetalleController : ControllerBase
    {

        private readonly ITablaDetalleService oITablaDetalleService;

        public TablaDetalleController(ITablaDetalleService ITablaDetalleService)
        {
            oITablaDetalleService = ITablaDetalleService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Listar")]
        public async Task<IActionResult> ListarAsync([FromBody] TablaDetalle entidad)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | BancoZiPago: [Listar] | Inicio.", nameof(ListarAsync));

            var response = await oITablaDetalleService.ListarTablaDetalleAsync(logger, entidad);

            return response.ToHttpResponse();

        }

    }
}