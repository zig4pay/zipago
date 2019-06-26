using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NLog;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Servicio.WebAPI.Requests;
using ZREL.ZiPago.Servicio.WebAPI.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Controllers.Afiliacion
{
    [ApiVersion("1.0")]
    [Route("api/v1.0/AfiliacionZiPago")]
    [ApiController]
    public class AfiliacionController : ControllerBase
    {

        private readonly IAfiliacionService oIAfiliacionService;

        public AfiliacionController(IAfiliacionService IAfiliacionService)
        {
            oIAfiliacionService = IAfiliacionService;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Registrar")]
        public async Task<IActionResult> RegistrarAsync([FromBody] AfiliacionRequest request)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync), request.entidadUsuario.Clave1);

            var response = await oIAfiliacionService.RegistrarAsync(logger, request.entidadUsuario, request.entidadDomicilio, request.listComercioCuenta);

            return response.ToHttpResponse();
        }

        [HttpGet("{CodigoComercio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComercioObtener/{CodigoComercio}")]
        public async Task<IActionResult> ComercioObtenerAsync(string CodigoComercio)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | ComercioZiPago: [Obtener] | Inicio.", nameof(ComercioObtenerAsync));

            var response = await oIAfiliacionService.ObtenerComercioZiPagoAsync(logger, CodigoComercio);

            return response.ToHttpResponse();

        }
    }
}