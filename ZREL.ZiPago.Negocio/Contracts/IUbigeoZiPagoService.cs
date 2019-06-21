using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IUbigeoZiPagoService
    {

        Task<ListResponse<UbigeoZiPago>> ListarUbigeoZiPagoAsync(Logger logger, string CodigoUbigeoPadre);
        
    }
}
