using NLog;
using System;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Afiliacion;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Requests;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Afiliacion
{
    public class AfiliacionService : Service, IAfiliacionService
    {
        private readonly ITablaDetalleService tdService; 

        public AfiliacionService(ZiPagoDBContext dbContext, ITablaDetalleService tablaDetalleService) : base(dbContext)
        {
            tdService = tablaDetalleService;
        }

        public async Task<IResponse> RegistrarAsync(Logger logger, AfiliacionRequest request)
        {
            var response = new Response();
            string codRubro;
                        
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync));

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!await tdService.VerificarExisteTablaDetalleAsync(logger, Constantes.strCodTablaRubroNegocio, request.OtroRubroNegocio)) {

                        codRubro = await tdService.ObtenerMaxTablaDetalleAsync(logger, Constantes.strCodTablaRubroNegocio);
                        codRubro = Convert.ToString(Convert.ToInt32(codRubro) + 1).PadLeft(3, '0');

                        TablaDetalle td = new TablaDetalle {
                            Cod_Tabla = Constantes.strCodTablaRubroNegocio,
                            Valor = codRubro,
                            Descr_Valor = request.OtroRubroNegocio
                        };

                        DbContext.TablasDetalle.Add(td);
                        request.EntidadUsuario.CodigoRubroNegocio = codRubro;
                    }

                    DbContext.Attach(request.EntidadUsuario);
                    DbContext.Entry(request.EntidadUsuario).Property("CodigoRubroNegocio").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("CodigoTipoPersona").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("CodigoTipoDocumento").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("NumeroDocumento").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("RazonSocial").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("ApellidoPaterno").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("ApellidoMaterno").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("Nombres").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("Sexo").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("FechaNacimiento").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("TelefonoMovil").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("TelefonoFijo").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("FechaActualizacion").IsModified = true;                    
                    await DbContext.SaveChangesAsync();

                    DbContext.Add(request.EntidadDomicilio);
                    await DbContext.SaveChangesAsync();

                    foreach (ComercioCuentaZiPago item in request.ListComercioCuenta)
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
                        
                        ComercioCuentaZiPago comercioCuenta = new ComercioCuentaZiPago
                        {
                            Activo = Constantes.strValor_Activo,
                            FechaCreacion = DateTime.Now
                        };

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
