using System;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;
using ZREL.ZiPago.Libreria.Seguridad;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Negocio.Seguridad
{
    public class UsuarioZiPagoService : Service, IUsuarioZiPagoService
    {

        public UsuarioZiPagoService(ZiPagoDBContext dbContext) : base (dbContext)
        {

        }


        public async Task<ISingleResponse<UsuarioZiPago>> AutenticarUsuarioZiPagoAsync(NLog.Logger logger, UsuarioZiPago entidad)
        {
            var response = new SingleResponse<UsuarioZiPago>();
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(AutenticarUsuarioZiPagoAsync), entidad.Clave1);
            try
            {
                response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(entidad.Clave1);                
                if (response.Model != null && (Criptografia.Desencriptar(response.Model.Clave2.Trim()) == Criptografia.Desencriptar(entidad.Clave2))) {
                    response.Model.Clave2 = "";
                    response.Mensaje = "1";
                }
                else
                {
                    response.Model = null;
                    response.Mensaje = Constantes.strMensajeUsuarioIncorrecto;
                }
                logger.Info("[{0}] | UsuarioZiPago: [{1}] | Mensaje: [{2}].", nameof(AutenticarUsuarioZiPagoAsync), entidad.Clave1, response.Mensaje);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(AutenticarUsuarioZiPagoAsync), nameof(UsuarioZiPago), ex);
            }
            return response;
        }
    }
}
