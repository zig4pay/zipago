using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Requests;
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

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("DatosPersonalesObtener/{idUsuarioZiPago}")]
        public async Task<IActionResult> DatosPersonalesObtenerAsync(int idUsuarioZiPago) {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.DatosPersonalesObtenerAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ObtenerDatosPersonalesAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }
        
        [HttpGet("{codigoComercio}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComercioObtener/{codigoComercio}")]
        public async Task<IActionResult> ComercioObtenerAsync(string codigoComercio)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | ComercioZiPago: [Obtener] | Inicio.", nameof(ComercioObtenerAsync));

            var response = await oIAfiliacionService.ObtenerComercioZiPagoAsync(logger, codigoComercio);

            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("Registrar")]
        public async Task<IActionResult> RegistrarAsync([FromBody] AfiliacionRequest request)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync), request.EntidadUsuario.Clave1);

            var response = await oIAfiliacionService.RegistrarAsync(logger, request);

            return response.ToHttpResponse();
        }

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("BancoPorUsuarioListar/{idUsuarioZiPago}")]
        public async Task<IActionResult> BancoPorUsuarioListarAsync(int idUsuarioZiPago)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.BancoPorUsuarioListarAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ListarBancosPorUsuarioAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentasBancariasListar/{idUsuarioZiPago}")]
        public async Task<IActionResult> CuentasBancariasListarAsync(int idUsuarioZiPago)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentasBancariasListarAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ListarCuentasBancariasAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }

        [HttpGet("{idUsuarioZiPago}/{idBancoZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentasBancariasListarResumen/{idUsuarioZiPago}/{idBancoZiPago}")]
        public async Task<IActionResult> CuentasBancariasListarResumenAsync(int idUsuarioZiPago, int idBancoZiPago)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentasBancariasListarResumenAsync] | UsuarioZiPago: [{0}] - BancoZiPago: [{1}] | Inicio.", 
                idUsuarioZiPago.ToString(), idBancoZiPago.ToString());

            var response = await oIAfiliacionService.ListarCuentasBancariasResumenAsync(logger, idUsuarioZiPago, idBancoZiPago);

            return response.ToHttpResponse();

        }
        
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentasBancariasRegistrar")]
        public async Task<IActionResult> CuentasBancariasRegistrarAsync([FromBody] List<CuentaBancariaZiPago> request)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentasBancariasRegistrarAsync] | CuentaBancariaZiPago: [{0}] | Inicio.", request.ToString());

            var response = await oIAfiliacionService.RegistrarCuentasBancariasAsync(logger, request);
            return response.ToHttpResponse();
        }

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComerciosListar/{idUsuarioZiPago}")]
        public async Task<IActionResult> ComerciosListarAsync(int idUsuarioZiPago)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.ComerciosListarAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ListarComerciosAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }

    }
}