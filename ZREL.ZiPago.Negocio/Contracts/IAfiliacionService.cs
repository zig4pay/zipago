using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;
using ZREL.ZiPago.Negocio.Requests.Afiliacion;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IAfiliacionService : IService
    {

        #region DatosPersonales

        Task<ISingleResponse<DatosPersonales>> ObtenerDatosPersonalesAsync(Logger logger, int idUsuarioZiPago);

        Task<IResponse> RegistrarAsync(Logger logger, DatosPersonalesRequest request);

        Task<IListResponse<DomicilioHistorico>> ListarDomiciliosHistoricoAsync(Logger logger, int idUsuarioZiPago);

        #endregion

        #region Cuentas Bancarias

        Task<ISingleResponse<CuentaBancariaZiPago>> ObtenerCuentaBancariaZiPagoAsync(Logger logger, CuentaBancariaZiPago cuentabancaria);

        Task<ISingleResponse<CuentaBancariaZiPago>> ObtenerCuentaBancariaZiPagoPorIdAsync(Logger logger, int idCuentaBancaria);

        Task<IListResponse<BancoZiPago>> ListarBancosPorUsuarioAsync(Logger logger, int idUsuarioZiPago);

        Task<IListResponse<CuentaBancariaListado>> ListarCuentasBancariasAsync(Logger logger, CuentaBancariaFiltros cuentaFiltros);

        Task<IListResponse<CuentaBancariaListaResumida>> ListarCuentasBancariasResumenAsync(Logger logger, int idUsuarioZiPago, int idBancoZiPago);

        Task<IResponse> RegistrarCuentasBancariasAsync(Logger logger, CuentaBancariaZiPago cuentaBancaria);

        Task<ISummaryResponse> ObtenerCantidadCuentasBancariasPorUsuarioAsync(Logger logger, int idUsuarioZiPago);

        #endregion

        #region Comercios

        Task<ISingleResponse<ComercioZiPagoReg>> ObtenerComercioZiPagoAsync(Logger logger, string codigoComercio);

        Task<IListResponse<EntidadGenerica>> ListarComerciosAsync(Logger logger, int idUsuarioZiPago);

        Task<IListResponse<ComercioListado>> ListarComerciosAsync(Logger logger, ComercioFiltros comercioFiltros);

        Task<IResponse> RegistrarComerciosAsync(Logger logger, List<ComercioCuentaZiPago> request);

        Task<ISummaryResponse> ObtenerCantidadComerciosPorUsuarioAsync(Logger logger, int idUsuarioZiPago);

        #endregion
        
    }
}
