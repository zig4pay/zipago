using NLog;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IUsuarioZiPagoService : IService
    {
        Task<ISingleResponse<UsuarioZiPago>> ObtenerAsync(Logger logger, string clave1);
        Task<ISingleResponse<UsuarioZiPago>> AutenticarAsync(Logger logger, UsuarioZiPago entidad);
        Task<ISingleResponse<UsuarioZiPago>> RegistrarAsync(Logger logger, UsuarioZiPago entidad);
        Task<ISingleResponse<UsuarioZiPago>> RecuperarAsync(Logger logger, string clave1);
    }
}
