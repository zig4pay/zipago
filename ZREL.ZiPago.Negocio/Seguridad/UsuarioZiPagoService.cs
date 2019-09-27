using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task<ISingleResponse<UsuarioZiPago>> ObtenerAsync(Logger logger, string clave1) {
            var response = new SingleResponse<UsuarioZiPago>();
            logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.ObtenerAsync] | UsuarioZiPago: [{0}] | Inicio.", clave1);
            try
            {
                response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(clave1);
                if (response.Model != null)
                {
                    response.Model.Clave2 = "";
                    response.Mensaje = "1";
                    logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Response: [{2}]", nameof(ObtenerAsync), clave1, JsonConvert.SerializeObject(response));
                }
                else {
                    response.Mensaje = Constantes.strMensajeUsuarioNoRegistrado;
                    logger.Info ("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Mensaje: [{2}]", nameof(UsuarioZiPago), clave1, Constantes.strMensajeUsuarioNoRegistrado);
                }
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Seguridad.UsuarioZiPagoService.ObtenerAsync", nameof(UsuarioZiPago), ex);
            }
            return response;
        }
        
        public async Task<ISingleResponse<UsuarioZiPago>> AutenticarAsync(Logger logger, UsuarioZiPago entidad)
        {
            var response = new SingleResponse<UsuarioZiPago>();
            logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(AutenticarAsync), entidad.Clave1);
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
                logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Mensaje: [{2}].", nameof(AutenticarAsync), entidad.Clave1, response.Mensaje);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Seguridad.UsuarioZiPagoService.AutenticarAsync", nameof(UsuarioZiPago), ex);
            }
            return response;
        }

        public async Task<ISingleResponse<UsuarioZiPago>> RegistrarAsync(Logger logger, UsuarioZiPago entidad)
        {
            var response = new SingleResponse<UsuarioZiPago>();
            logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync), entidad.Clave1);

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(entidad.Clave1);

                    if (response.Model is null || response.Model.IdUsuarioZiPago == 0)
                    {
                        entidad.EstadoRegistro = Constantes.strEstadoRegistro_Nuevo;
                        entidad.Activo = Constantes.strValor_Activo;
                        entidad.FechaCreacion = DateTime.Now;
                        DbContext.Add(entidad);

                        await DbContext.SaveChangesAsync();
                        txAsync.Commit();
                        response.Mensaje = Constantes.strEstadoRegistro_Nuevo;
                        logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Transaccion realizada.", nameof(RegistrarAsync), entidad.Clave1);

                        response.Model = await DbContext.ObtenerUsuarioZiPagoAsync(entidad.Clave1);
                        response.Model.Clave2 = "";                        
                        logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Obtener usuario registrado.", nameof(RegistrarAsync), entidad.Clave1);

                    }
                    else
                    {
                        txAsync.Rollback();
                        response.HizoError = true;
                        response.Model.Clave2 = "";
                        response.MensajeError = string.Format(Constantes.strMensajeUsuarioYaExiste, response.Model.Clave1.Trim());
                        logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.{0}] | UsuarioZiPago: [{1}] | Id ZiPago ya se encuentra registrado.", nameof(RegistrarAsync), entidad.Clave1);
                    }
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Model = null;
                    response.Mensaje = Constantes.strMensajeUsuarioError;
                    response.SetError(logger, "Negocio.Seguridad.UsuarioZiPagoService.RegistrarAsync", nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<UsuarioZiPago>> RecuperarAsync(Logger logger, string clave1)
        {
            ISingleResponse<UsuarioZiPago> response = new SingleResponse<UsuarioZiPago>();
            UsuarioZiPago usuario = new UsuarioZiPago();
            string token = "";

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {   
                try
                {
                    usuario = await DbContext.ObtenerUsuarioZiPagoAsync(clave1);

                    if (usuario != null && !string.IsNullOrWhiteSpace(usuario.Clave1))
                    {
                        token = Criptografia.Encriptar(Criptografia.Encoder64(usuario.IdUsuarioZiPago.ToString() + "|" + usuario.Clave1));

                        usuario.Activo = Constantes.strValor_NoActivo;
                        usuario.ClaveRecuperacion = token;
                        usuario.FechaGeneracionClave = DateTime.Now;
                        usuario.FechaActualizacion = DateTime.Now;

                        DbContext.Attach(usuario);
                        DbContext.Entry(usuario).Property("Activo").IsModified = true;
                        DbContext.Entry(usuario).Property("ClaveRecuperacion").IsModified = true;
                        DbContext.Entry(usuario).Property("FechaGeneracionClave").IsModified = true;
                        DbContext.Entry(usuario).Property("FechaActualizacion").IsModified = true;
                        logger.Info("[Negocio.Seguridad.UsuarioZiPagoService.RecuperarAsync] | UsuarioZiPago: [{0}]", JsonConvert.SerializeObject(usuario));

                        await DbContext.SaveChangesAsync();
                        txAsync.Commit();

                        response.Model = await DbContext.UsuariosZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.Clave1 == clave1);
                        response.Model.Clave2 = string.Empty;
                        response.Mensaje = Constantes.strRegistroRealizado;
                    }
                    else {
                        response.HizoError = true;
                        response.MensajeError = Constantes.strMensajeUsuarioNoRegistrado;
                        txAsync.Rollback();
                    }
                                        
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Mensaje = Constantes.strMensajeUsuarioError;
                    response.SetError(logger, "Negocio.Seguridad.UsuarioZiPagoService.RecuperarAsync", nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }

        public async Task<IResponse> RestablecerAsync(Logger logger, UsuarioZiPago entidad)
        {
            IResponse response = new Response();
            
            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    entidad.Activo = Constantes.strValor_Activo;
                    entidad.FechaActualizacion = DateTime.Now;

                    DbContext.Attach(entidad);
                    DbContext.Entry(entidad).Property("Clave2").IsModified = true;
                    DbContext.Entry(entidad).Property("Activo").IsModified = true;
                    DbContext.Entry(entidad).Property("FechaActualizacion").IsModified = true;                    
                    await DbContext.SaveChangesAsync();
                    txAsync.Commit();

                    response.HizoError = false;
                    response.Mensaje = Constantes.strMensajeContrasenaRestablecida;
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Mensaje = Constantes.strMensajeErrorRestablecerContrasena;
                    response.SetError(logger, "Negocio.Seguridad.UsuarioZiPagoService.RecuperarAsync", nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }

    }
}
