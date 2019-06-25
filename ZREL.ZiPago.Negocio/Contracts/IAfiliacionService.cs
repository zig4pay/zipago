using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IAfiliacionService : IService
    {

        Task<IResponse> RegistrarAsync(Logger logger, UsuarioZiPago entidadUsuario, DomicilioZiPago entidadDomicilio, List<ComercioCuentaZiPago> listComercioCuenta);

    }
}
