using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Servicio.WebAPI.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers.Comun
{

    [ApiVersion("1.0")]
    [Route("api/v1.0/BancoZiPago")]
    [ApiController]
    public class BancoZiPagoController : ControllerBase
    {
        private readonly IBancoZiPagoService oIBancoZiPagoService;

        public BancoZiPagoController(IBancoZiPagoService IBancoZiPagoService)
        {
            oIBancoZiPagoService = IBancoZiPagoService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Listar")]
        public async Task<IActionResult> ListarAsync()
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers] | BancoZiPago: [{1}] | Inicio.", nameof(ListarAsync));

            var response = await oIBancoZiPagoService.ListarBancoZiPagoAsync(logger);

            return response.ToHttpResponse();

        }
    }
}