using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Afiliacion;
using ZREL.ZiPago.Datos.Configuraciones.Seguridad;
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
                    DbContext.Update(entidadUsuario);
                    await DbContext.SaveChangesAsync();

                    DbContext.Add(entidadDomicilio);
                    await DbContext.SaveChangesAsync();

                    foreach (ComercioCuentaZiPago item in listComercioCuenta)
                    {

                        ComercioZiPago comercio = new ComercioZiPago
                        {
                            CodigoComercio = item.ComercioZiPago.CodigoComercio,
                            IdUsuarioZiPago = item.ComercioZiPago.IdUsuarioZiPago,
                            Descripcion = item.ComercioZiPago.Descripcion,
                            CorreoNotificacion = item.ComercioZiPago.CorreoNotificacion,
                            Activo = Constantes.strValor_Activo,
                            FechaCreacion = DateTime.Now
                        };

                        CuentaBancariaZiPago cuenta = new CuentaBancariaZiPago
                        {
                            IdBancoZiPago = item.CuentaBancariaZiPago.IdBancoZiPago,
                            NumeroCuenta = item.CuentaBancariaZiPago.NumeroCuenta,
                            CodigoTipoCuenta = item.CuentaBancariaZiPago.CodigoTipoCuenta,
                            CodigoTipoMoneda = item.CuentaBancariaZiPago.CodigoTipoMoneda,
                            CCI = item.CuentaBancariaZiPago.CCI,
                            Activo = Constantes.strValor_Activo,
                            FechaCreacion = DateTime.Now
                        };

                        ComercioCuentaZiPago comercioCuenta = new ComercioCuentaZiPago();
                        comercioCuenta.Activo = Constantes.strValor_Activo;
                        comercioCuenta.FechaCreacion = DateTime.Now;

                        var responseCtaExiste = await DbContext.ObtenerCuentaBancariaZiPagoAsync(cuenta);

                        if (responseCtaExiste is null || responseCtaExiste.IdCuentaBancaria == 0)
                        {

                            comercioCuenta.ComercioZiPago = comercio;
                            comercioCuenta.CuentaBancariaZiPago = cuenta;

                            cuenta.ComercioCuentaZiPago.Add(comercioCuenta);

                            DbContext.Add(comercio);
                            DbContext.Add(cuenta);

                            await DbContext.SaveChangesAsync();
                        }
                        else
                        {                                
                            comercioCuenta.ComercioZiPago = comercio;
                            comercioCuenta.CuentaBancariaZiPago = responseCtaExiste;

                            comercio.ComercioCuentaZiPago.Add(comercioCuenta);

                            DbContext.Add(comercio);
                                
                            await DbContext.SaveChangesAsync();
                        }

                    }

                    txAsync.Commit();                       

                }
                catch (Exception ex)
                {
                    txAsync.Rollback();                    
                    response.Mensaje = Constantes.strMensajeUsuarioError;
                    response.SetError(logger, nameof(RegistrarAsync), nameof(UsuarioZiPago), ex);
                }
            }

            return response;
        }
    }
}
