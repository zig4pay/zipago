using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;
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

        [HttpGet("{clave1}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Obtener/{clave1}")]
        public async Task<IActionResult> ObtenerAsync(string clave1)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad.UsuarioZiPagoController.{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(ObtenerAsync), clave1);

            ISingleResponse<UsuarioZiPago> response = await oIUsuarioZiPagoService.ObtenerAsync(logger, clave1);

            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Autenticar")]
        public async Task<IActionResult> AutenticarAsync([FromBody] UsuarioZiPago entidad) {

            Logger logger = LogManager.GetCurrentClassLogger();            
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad.UsuarioZiPagoController.{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(AutenticarAsync), entidad.Clave1);

            ISingleResponse<UsuarioZiPago> response = await oIUsuarioZiPagoService.AutenticarAsync(logger, entidad);
            
            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Registrar")]
        public async Task<IActionResult> RegistrarAsync([FromBody] UsuarioZiPago entidad) {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad.UsuarioZiPagoController.{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync), entidad.Clave1);

            ISingleResponse<UsuarioZiPago> response = await oIUsuarioZiPagoService.RegistrarAsync(logger, entidad);

            return response.ToHttpResponse();
        }

        [HttpGet("{clave1}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Recuperar/{clave1}")]
        public async Task<IActionResult> RecuperarAsync(string clave1)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad.UsuarioZiPagoController.RecuperarAsync] | UsuarioZiPago: [{0}] | Inicio.", clave1);

            ISingleResponse<UsuarioZiPago> response = await oIUsuarioZiPagoService.RecuperarAsync(logger, clave1);

            return response.ToHttpResponse();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Restablecer")]
        public async Task<IActionResult> RestablecerAsync([FromBody] UsuarioZiPago entidad)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Seguridad.UsuarioZiPagoController.RestablecerAsync] | UsuarioZiPago: [{0}] | Inicio.", entidad.Clave1);

            IResponse response = await oIUsuarioZiPagoService.RestablecerAsync(logger, entidad);

            return response.ToHttpResponse();
        }

    }
}