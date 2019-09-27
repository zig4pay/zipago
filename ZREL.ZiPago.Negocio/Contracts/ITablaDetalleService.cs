using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface ITablaDetalleService : IService
    {
        Task<ListResponse<TablaDetalle>> ListarTablaDetalleAsync(Logger logger, string CodTabla);
        Task<TablaDetalle> VerificarExisteTablaDetalleAsync(Logger logger, string codTabla, string valor);
        Task<string> ObtenerMaxTablaDetalleAsync(Logger logger, string CodTabla);
    }
}
