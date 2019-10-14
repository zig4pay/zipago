using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Util;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IBancoZiPagoService
    {
        Task<ListResponse<EntidadGenerica>> ListarBancoZiPagoAsync(Logger logger);
    }
}
