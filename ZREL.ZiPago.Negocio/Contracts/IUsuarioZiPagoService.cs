using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IUsuarioZiPagoService : IService
    {
        Task<ISingleResponse<UsuarioZiPago>> AutenticarUsuarioZiPagoAsync(NLog.Logger logger, UsuarioZiPago entidad);

        Task<ISingleResponse<UsuarioZiPago>> RegistrarUsuarioZiPagoAsync(NLog.Logger logger, UsuarioZiPago entidad);
    }
}
