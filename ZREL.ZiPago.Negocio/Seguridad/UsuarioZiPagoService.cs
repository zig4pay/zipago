using NLog;
using System;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Libreria.Seguridad;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Seguridad
{
    public class UsuarioZiPagoService : Service, IUsuarioZiPagoService
    {

        public UsuarioZiPagoService(ZiPagoDBContext dbContext) : base (dbContext)
        {

        }
        
        public async Task<ISingleResponse<UsuarioZiPago>> AutenticarUsuarioZiPagoAsync(Logger logger, UsuarioZiPago entidad)
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

        public async Task<ISingleResponse<UsuarioZiPago>> RegistrarUsuarioZiPagoAsync(Logger logger, UsuarioZiPago entidad)
        {
            var response = new SingleResponse<UsuarioZiPago>();
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarUsuarioZiPagoAsync), entidad.Clave1);

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(entidad.Clave1);

                    if (response.Model is null || response.Model.IdUsuarioZiPago == 0)
                    {
                        entidad.Activo = Constantes.strValor_Activo;
                        entidad.FechaCreacion = DateTime.Now;                        
                        DbContext.Add(entidad);

                        await DbContext.SaveChangesAsync();
                        txAsync.Commit();
                        response.Mensaje = Constantes.strMensajeUsuarioRegistrado;                        
                        logger.Info("[{0}] | UsuarioZiPago: [{1}] | Transaccion realizada.", nameof(RegistrarUsuarioZiPagoAsync), entidad.Clave1);

                        response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(entidad.Clave1);
                        response.Model.Clave2 = "";                        
                        logger.Info("[{0}] | UsuarioZiPago: [{1}] | Obtener usuario registrado.", nameof(RegistrarUsuarioZiPagoAsync), entidad.Clave1);

                    }
                    else
                    {
                        response.HizoError = true;
                        response.Model.Clave2 = "";
                        response.MensajeError = string.Format(Constantes.strMensajeUsuarioYaExiste, response.Model.Clave1.Trim());
                        logger.Info("[{0}] | UsuarioZiPago: [{1}] | Id ZiPago ya se encuentra registrado.", nameof(RegistrarUsuarioZiPagoAsync), entidad.Clave1);
                    }                    
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Model = null;
                    response.Mensaje = Constantes.strMensajeUsuarioError;
                    response.SetError(logger, nameof(RegistrarUsuarioZiPagoAsync), nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }

    }
}
