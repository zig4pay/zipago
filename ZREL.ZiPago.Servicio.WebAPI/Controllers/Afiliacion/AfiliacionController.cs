using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Requests.Afiliacion;
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

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("DomiciliosHistoricoListar/{idUsuarioZiPago}")]
        public async Task<IActionResult> DomiciliosHistoricoListarAsync(int idUsuarioZiPago)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.DomiciliosHistoricoListarAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ListarDomiciliosHistoricoAsync(logger, idUsuarioZiPago);

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
        public async Task<IActionResult> RegistrarAsync([FromBody] DatosPersonalesRequest request)
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentaBancariaObtener/")]
        public async Task<IActionResult> CuentaBancariaObtenerAsync([FromBody] CuentaBancariaZiPago cuentabancaria)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.CuentaBancariaObtenerAsync] | CuentaBancaria: [0] | Inicio.", JsonConvert.SerializeObject(cuentabancaria));
            var response = await oIAfiliacionService.ObtenerCuentaBancariaZiPagoAsync(logger, cuentabancaria);
            return response.ToHttpResponse();
        }
        
        [HttpGet("{idCuentaBancaria}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentaBancariaObtenerPorId/{idCuentaBancaria}")]
        public async Task<IActionResult> CuentaBancariaObtenerPorIdAsync(int idCuentaBancaria)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentaBancariaObtenerPorIdAsync] | idCuentaBancaria: [{0}] | Inicio.", idCuentaBancaria.ToString());

            var response = await oIAfiliacionService.ObtenerCuentaBancariaZiPagoPorIdAsync(logger, idCuentaBancaria);

            return response.ToHttpResponse();

        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentasBancariasListar/")]
        public async Task<IActionResult> CuentasBancariasListarAsync(CuentaBancariaFiltros cuentaBancariaFiltros)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentasBancariasListarAsync] | Filtros: [{0}] | Inicio.", JsonConvert.SerializeObject(cuentaBancariaFiltros));

            var response = await oIAfiliacionService.ListarCuentasBancariasAsync(logger, cuentaBancariaFiltros);

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
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.ComerciosListarAsync] | idUsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());
            var response = await oIAfiliacionService.ListarComerciosAsync(logger, idUsuarioZiPago);
            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComerciosListar")]
        public async Task<IActionResult> ComerciosListarAsync([FromBody] ComercioFiltros comercioFiltros)
        {

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.ComerciosListarAsync] | ComercioFiltros: [{0}] | Inicio.", JsonConvert.SerializeObject(comercioFiltros));

            var response = await oIAfiliacionService.ListarComerciosAsync(logger, comercioFiltros);

            return response.ToHttpResponse();

        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComerciosRegistrar")]
        public async Task<IActionResult> ComerciosRegistrarAsync([FromBody] List<ComercioCuentaZiPago> request)
        {

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.ComerciosRegistrarAsync] | request: [{0}] | Inicio.",  JsonConvert.SerializeObject(request));

            var response = await oIAfiliacionService.RegistrarComerciosAsync(logger, request);

            return response.ToHttpResponse();
        }

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("CuentasBancariasObtenerCantidadPorUsuarioAsync/{idUsuarioZiPago}")]
        public async Task<IActionResult> CuentasBancariasObtenerCantidadPorUsuarioAsync(int idUsuarioZiPago)
        {            
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.CuentasBancariasObtenerCantidadPorUsuarioAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ObtenerCantidadCuentasBancariasPorUsuarioAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }

        [HttpGet("{idUsuarioZiPago}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Route("ComerciosObtenerCantidadPorUsuarioAsync/{idUsuarioZiPago}")]
        public async Task<IActionResult> ComerciosObtenerCantidadPorUsuarioAsync(int idUsuarioZiPago)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("[Servicio.WebAPI.Controllers.Afiliacion.AfiliacionController.ComerciosObtenerCantidadPorUsuarioAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago.ToString());

            var response = await oIAfiliacionService.ObtenerCantidadComerciosPorUsuarioAsync(logger, idUsuarioZiPago);

            return response.ToHttpResponse();

        }

    }
}