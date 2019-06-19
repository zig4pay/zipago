using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IUsuarioZiPagoService : IService
    {
        Task<ISingleResponse<UsuarioZiPago>> AutenticarUsuarioZiPagoAsync(Logger logger, UsuarioZiPago entidad);

        Task<ISingleResponse<UsuarioZiPago>> RegistrarUsuarioZiPagoAsync(Logger logger, UsuarioZiPago entidad);
    }
}
