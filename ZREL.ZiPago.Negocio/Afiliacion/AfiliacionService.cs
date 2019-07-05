using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Afiliacion;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Afiliacion
{
    public class AfiliacionService : Service, IAfiliacionService
    {
        public AfiliacionService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<IResponse> RegistrarAsync(Logger logger, UsuarioZiPago entidadUsuario, DomicilioZiPago entidadDomicilio, List<ComercioCuentaZiPago> listComercioCuenta)
        {
            var response = new Response();
            
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync));

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    DbContext.Attach(entidadUsuario);
                    DbContext.Entry(entidadUsuario).Property("CodigoRubroNegocio").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("CodigoTipoPersona").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("CodigoTipoDocumento").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("NumeroDocumento").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("RazonSocial").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("ApellidoPaterno").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("ApellidoMaterno").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("Nombres").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("Sexo").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("FechaNacimiento").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("TelefonoMovil").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("TelefonoFijo").IsModified = true;
                    DbContext.Entry(entidadUsuario).Property("FechaActualizacion").IsModified = true;                    
                    await DbContext.SaveChangesAsync();

                    DbContext.Add(entidadDomicilio);
                    await DbContext.SaveChangesAsync();

                    foreach (ComercioCuentaZiPago item in listComercioCuenta)
                    {

                        ComercioZiPago comercio = new ComercioZiPago();

                        comercio.CodigoComercio = item.ComercioZiPago.CodigoComercio;
                        comercio.IdUsuarioZiPago = item.ComercioZiPago.IdUsuarioZiPago;
                        comercio.Descripcion = item.ComercioZiPago.Descripcion;
                        comercio.CorreoNotificacion = item.ComercioZiPago.CorreoNotificacion;
                        comercio.Activo = Constantes.strValor_Activo;
                        comercio.FechaCreacion = DateTime.Now;


                        CuentaBancariaZiPago cuenta = new CuentaBancariaZiPago();

                        cuenta.IdBancoZiPago = item.CuentaBancariaZiPago.IdBancoZiPago;
                        cuenta.NumeroCuenta = item.CuentaBancariaZiPago.NumeroCuenta;
                        cuenta.CodigoTipoCuenta = item.CuentaBancariaZiPago.CodigoTipoCuenta;
                        cuenta.CodigoTipoMoneda = item.CuentaBancariaZiPago.CodigoTipoMoneda;
                        cuenta.CCI = item.CuentaBancariaZiPago.CCI;
                        cuenta.Activo = Constantes.strValor_Activo;
                        cuenta.FechaCreacion = DateTime.Now;
                        

                        ComercioCuentaZiPago comercioCuenta = new ComercioCuentaZiPago();
                        comercioCuenta.Activo = Constantes.strValor_Activo;
                        comercioCuenta.FechaCreacion = DateTime.Now;

                        var responseCtaExiste = await DbContext.ObtenerCuentaBancariaZiPagoAsync(cuenta);

                        if (responseCtaExiste is null || responseCtaExiste.IdCuentaBancaria == 0)
                        {

                            comercioCuenta.ComercioZiPago = comercio;
                            comercioCuenta.CuentaBancariaZiPago = cuenta;

                            cuenta.ComerciosCuentasZiPago.Add(comercioCuenta);

                            DbContext.Add(comercio);
                            DbContext.Add(cuenta);

                            await DbContext.SaveChangesAsync();
                        }
                        else
                        {                                
                            comercioCuenta.ComercioZiPago = comercio;
                            comercioCuenta.CuentaBancariaZiPago = responseCtaExiste;

                            comercio.ComerciosCuentasZiPago.Add(comercioCuenta);

                            DbContext.Add(comercio);
                                
                            await DbContext.SaveChangesAsync();
                        }

                    }

                    txAsync.Commit();

                    response.Mensaje = Constantes.strRegistroRealizado;

                }
                catch (Exception ex)
                {
                    txAsync.Rollback();                                        
                    response.Mensaje = ex.ToString();                    
                    response.SetError(logger, nameof(RegistrarAsync), nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }

        public async Task<ISingleResponse<ComercioZiPago>> ObtenerComercioZiPagoAsync(Logger logger, string codigoComercio)
        {

            var response = new SingleResponse<ComercioZiPago>();
            logger.Info("[{0}] | ComercioZiPago: [{1}] | Inicio.", nameof(ObtenerComercioZiPagoAsync), codigoComercio);
            try
            {
                response.Model = await DbContext.ObtenerComercioZiPagoAsync(codigoComercio);
                response.Mensaje = Constantes.strConsultaRealizada;
                logger.Info("[{0}] | ComercioZiPago: [{1}] | Mensaje: [{2}].", nameof(ObtenerComercioZiPagoAsync), codigoComercio, response.Mensaje);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(ObtenerComercioZiPagoAsync), nameof(ComercioZiPago), ex);
            }
            return response;
        }

    }
}
