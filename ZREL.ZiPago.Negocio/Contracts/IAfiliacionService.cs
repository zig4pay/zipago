using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Negocio.Requests;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IAfiliacionService : IService
    {

        Task<ISingleResponse<DatosPersonales>> ObtenerDatosPersonalesAsync(Logger logger, int idUsuarioZiPago);

        Task<ISingleResponse<ComercioZiPago>> ObtenerComercioZiPagoAsync(Logger logger, string codigoComercio);

        Task<IResponse> RegistrarAsync(Logger logger, AfiliacionRequest request);

        Task<IListResponse<CuentaBancariaListado>> ListarCuentasBancariasAsync(Logger logger, int idUsuarioZiPago);

        Task<IListResponse<CuentaBancariaListaResumida>> ListarCuentasBancariasResumenAsync(Logger logger, int idUsuarioZiPago, int idBancoZiPago);

        Task<IResponse> RegistrarCuentasBancariasAsync(Logger logger, List<CuentaBancariaZiPago> cuentasBancarias);

    }
}
