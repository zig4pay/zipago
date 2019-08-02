using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Servicio.WebAPI.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers.Seguridad
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/UsuarioZiPago")]
    [ApiController]
    public class UsuarioZiPagoController : ControllerBase
    {

        private readonly IUsuarioZiPagoService oIUsuarioZiPagoService;

        public UsuarioZiPagoController(IUsuarioZiPagoService IUsuarioZiPagoService)
        {
            oIUsuarioZiPagoService = IUsuarioZiPagoService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Obtener/{clave1}")]
        public async Task<IActionResult> ObtenerAsync([FromBody] UsuarioZiPago entidad)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(ObtenerAsync), entidad.Clave1);

            var response = await oIUsuarioZiPagoService.AutenticarAsync(logger, entidad);

            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Autenticar")]
        public async Task<IActionResult> AutenticarAsync([FromBody] UsuarioZiPago entidad) {

            var logger = LogManager.GetCurrentClassLogger();            
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(AutenticarAsync), entidad.Clave1);
            
            var response = await oIUsuarioZiPagoService.AutenticarAsync(logger, entidad);
            
            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Registrar")]
        public async Task<IActionResult> RegistrarAsync([FromBody] UsuarioZiPago entidad) {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync), entidad.Clave1);

            var response = await oIUsuarioZiPagoService.RegistrarAsync(logger, entidad);

            return response.ToHttpResponse();
        }

    }
}