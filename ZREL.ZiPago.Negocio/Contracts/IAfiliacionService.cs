using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Requests.Afiliacion;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IAfiliacionService : IService
    {

        Task<ISingleResponse<DatosPersonales>> ObtenerDatosPersonalesAsync(Logger logger, int idUsuarioZiPago);

        Task<IResponse> RegistrarAsync(Logger logger, DatosPersonalesRequest request);


        Task<IListResponse<BancoZiPago>> ListarBancosPorUsuarioAsync(Logger logger, int idUsuarioZiPago);

        Task<IListResponse<CuentaBancariaListado>> ListarCuentasBancariasAsync(Logger logger, int idUsuarioZiPago);

        Task<IListResponse<CuentaBancariaListaResumida>> ListarCuentasBancariasResumenAsync(Logger logger, int idUsuarioZiPago, int idBancoZiPago);

        Task<IResponse> RegistrarCuentasBancariasAsync(Logger logger, List<CuentaBancariaZiPago> cuentasBancarias);


        Task<ISingleResponse<ComercioZiPago>> ObtenerComercioZiPagoAsync(Logger logger, string codigoComercio);

        Task<IListResponse<ComercioListado>> ListarComerciosAsync(Logger logger, ComercioFiltros comercioFiltros);

        Task<IResponse> RegistrarComerciosAsync(Logger logger, List<ComercioCuentaZiPago> request);

    }
}
