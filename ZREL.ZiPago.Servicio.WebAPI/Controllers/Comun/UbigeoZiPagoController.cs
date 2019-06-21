using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Servicio.WebAPI.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers.Comun
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/UbigeoZiPago")]
    [ApiController]
    public class UbigeoZiPagoController : Controller
    {
        private readonly IUbigeoZiPagoService oIUbigeoZiPagoService;

        public UbigeoZiPagoController(IUbigeoZiPagoService IUbigeoZiPagoService)
        {
            oIUbigeoZiPagoService = IUbigeoZiPagoService;
        }

        [HttpGet("{CodUbigeoPadre}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Listar/{CodUbigeoPadre}")]
        public async Task<IActionResult> ListarAsync(string CodUbigeoPadre)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UbigeoZiPago: [{1}] | Inicio.", nameof(ListarAsync), CodUbigeoPadre);

            var response = await oIUbigeoZiPagoService.ListarUbigeoZiPagoAsync(logger, CodUbigeoPadre);

            return response.ToHttpResponse();

        }
    }
}