﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Datos.Afiliacion;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Seguridad;
using ZREL.ZiPago.Libreria;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Requests.Afiliacion;
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

        public async Task<ISingleResponse<DatosPersonales>> ObtenerDatosPersonalesAsync(Logger logger, int idUsuarioZiPago) {
            SingleResponse<DatosPersonales> response = new SingleResponse<DatosPersonales>();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.ObtenerDatosPersonalesAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago);

            try
            {
                response.Model = await DbContext.ObtenerDatosPersonalesAsync(idUsuarioZiPago);
                if (response.Model != null)
                    response.Model.Clave2 = "";
                response.Mensaje = response.Model != null ? Constantes.strConsultaRealizada : Constantes.strMensajeUsuarioNoRegistrado;

                logger.Info("[Negocio.Afiliacion.AfiliacionService.ObtenerDatosPersonalesAsync] | UsuarioZiPago: [{0}] | Mensaje: [{1}].",
                            JsonConvert.SerializeObject(response.Model),
                            response.Mensaje);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.ObtenerDatosPersonalesAsync", nameof(UsuarioZiPago), ex);
            }

            return response;
        }

        public async Task<ISingleResponse<ComercioZiPago>> ObtenerComercioZiPagoAsync(Logger logger, string codigoComercio)
        {
            SingleResponse<ComercioZiPago> response = new SingleResponse<ComercioZiPago>();
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

        public async Task<IResponse> RegistrarAsync(Logger logger, DatosPersonalesRequest request)
        {
            var response = new Response();
            string codRubro;
            
            logger.Info("[{0}] | UsuarioZiPago: [{1}] | Inicio.", nameof(RegistrarAsync));

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (!string.IsNullOrEmpty(request.OtroRubroNegocio) &&
                        !await tdService.VerificarExisteTablaDetalleAsync(logger, Constantes.strCodTablaRubroNegocio, request.OtroRubroNegocio)) {

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
                    DbContext.Entry(request.EntidadUsuario).Property("CodigoTipoDocumentoContacto").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("NumeroDocumentoContacto").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("ApellidoPaterno").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("ApellidoMaterno").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("Nombres").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("Sexo").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("FechaNacimiento").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("TelefonoMovil").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("TelefonoFijo").IsModified = true;
                    DbContext.Entry(request.EntidadUsuario).Property("FechaActualizacion").IsModified = true;                    
                    await DbContext.SaveChangesAsync();
                    
                    var domicilios = DbContext.DomiciliosZiPago.Where(p => p.IdUsuarioZiPago == request.EntidadUsuario.IdUsuarioZiPago).ToList();
                    domicilios.ForEach( a => 
                                            {
                                                a.Activo = Constantes.strValor_NoActivo;
                                                a.FechaActualizacion = DateTime.Now;
                                            }
                                        );
                    await DbContext.SaveChangesAsync();
                    
                    DbContext.Add(request.EntidadDomicilio);
                    await DbContext.SaveChangesAsync();
                    
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

        public async Task<IListResponse<BancoZiPago>> ListarBancosPorUsuarioAsync(Logger logger, int idUsuarioZiPago)
        {

            ListResponse<BancoZiPago> response = new ListResponse<BancoZiPago>();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarBancosPorUsuarioAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago);

            try
            {
                response.Model = await DbContext.ListarBancosPorUsuarioAsync(idUsuarioZiPago);
                response.Mensaje = Constantes.strConsultaRealizada;
                logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarBancosPorUsuarioAsync] | UsuarioZiPago: [{0}] | Mensaje: [{1}].", idUsuarioZiPago, Constantes.strConsultaRealizada);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.ListarBancosPorUsuarioAsync", nameof(UsuarioZiPago), ex);
            }

            return response;
        }

        public async Task<IListResponse<CuentaBancariaListado>> ListarCuentasBancariasAsync(Logger logger, int idUsuarioZiPago) {

            ListResponse<CuentaBancariaListado> response = new ListResponse<CuentaBancariaListado>();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarCuentasBancariasAsync] | UsuarioZiPago: [{0}] | Inicio.", idUsuarioZiPago);

            try
            {
                response.Model = await DbContext.ListarCuentasBancariasAsync(idUsuarioZiPago);
                response.Mensaje = Constantes.strConsultaRealizada;
                logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarCuentasBancariasAsync] | UsuarioZiPago: [{0}] | Mensaje: [{1}].", idUsuarioZiPago, Constantes.strConsultaRealizada);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.ObtenerCuentaBancariaAsync", nameof(UsuarioZiPago), ex);
            }

            return response;
        }

        public async Task<IListResponse<CuentaBancariaListaResumida>> ListarCuentasBancariasResumenAsync(Logger logger, int idUsuarioZiPago, int idBancoZiPago)
        {

            ListResponse<CuentaBancariaListaResumida> response = new ListResponse<CuentaBancariaListaResumida>();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarCuentasBancariasResumenAsync] | UsuarioZiPago: [{0}] - BancoZiPago: [{1}]  | Inicio.", idUsuarioZiPago, idBancoZiPago);

            try
            {
                response.Model = await DbContext.ListarCuentasBancariasResumenAsync(idUsuarioZiPago, idBancoZiPago);
                response.Mensaje = Constantes.strConsultaRealizada;
                logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarCuentasBancariasResumenAsync] | UsuarioZiPago: [{0}] | Mensaje: [{1}].", idUsuarioZiPago, Constantes.strConsultaRealizada);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.ListarCuentasBancariasResumenAsync", nameof(UsuarioZiPago), ex);
            }

            return response;
        }

        public async Task<IResponse> RegistrarCuentasBancariasAsync(Logger logger, List<CuentaBancariaZiPago> cuentasBancarias) {

            int idUsuarioZiPago = cuentasBancarias.ElementAt(0).IdUsuarioZiPago;

            Response response = new Response();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.RegistrarCuentasBancariasAsync] | CuentasBancarias: [{0}] | Inicio.",
                            JsonConvert.SerializeObject(cuentasBancarias));

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (CuentaBancariaZiPago cuenta in cuentasBancarias)
                    {
                        cuenta.Activo = Constantes.strValor_Activo;
                        cuenta.FechaCreacion = DateTime.Now;
                    }

                    DbContext.CuentasBancariasZiPago.AddRange(cuentasBancarias);
                    await DbContext.SaveChangesAsync();

                    txAsync.Commit();
                    response.Mensaje = Constantes.strRegistroRealizado;                    

                    logger.Info("[Negocio.Afiliacion.AfiliacionService.RegistrarCuentasBancariasAsync] | CuentasBancarias | Registro realizado.");
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Mensaje = ex.ToString();
                    response.SetError(logger, "[Negocio.Afiliacion.AfiliacionService.RegistrarCuentasBancariasAsync]", nameof(CuentaBancariaZiPago), ex);
                }
            }

            return response;
        }

        public async Task<IListResponse<ComercioListado>> ListarComerciosAsync(Logger logger, ComercioFiltros comercioFiltros)
        {

            ListResponse<ComercioListado> response = new ListResponse<ComercioListado>();
            logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarComerciosAsync] | ComercioFiltros: [{0}] | Inicio.", JsonConvert.SerializeObject(comercioFiltros));

            try
            {
                response.Model = await DbContext.ListarComerciosAsync(comercioFiltros);
                foreach (ComercioListado comercio in response.Model)
                {
                    comercio.Estado = comercio.Estado == Constantes.strValor_Activo ? "Activo" : "Inactivo";
                }
                response.Mensaje = Constantes.strConsultaRealizada;
                logger.Info("[Negocio.Afiliacion.AfiliacionService.ListarComerciosAsync] | ComercioFiltros: [{0}] | Mensaje: [{1}].", JsonConvert.SerializeObject(comercioFiltros), Constantes.strConsultaRealizada);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.ListarComerciosAsync", nameof(UsuarioZiPago), ex);
            }

            return response;
        }

        public async Task<IResponse> RegistrarComerciosAsync(Logger logger, List<ComercioCuentaZiPago> request)
        {
            var response = new Response();            
            logger.Info("[ZREL.ZiPago.Negocio.Afiliacion.AfiliacionService.RegistrarComerciosAsync] | request: [{0}] | Inicio.", JsonConvert.SerializeObject(request));

            using (var txAsync = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (ComercioCuentaZiPago item in request)
                    {
                        ComercioZiPago comercio = new ComercioZiPago
                        {
                            CodigoComercio = item.ComercioZiPago.CodigoComercio.ToUpper(),
                            IdUsuarioZiPago = item.ComercioZiPago.IdUsuarioZiPago,
                            Descripcion = item.ComercioZiPago.Descripcion,
                            CorreoNotificacion = item.ComercioZiPago.CorreoNotificacion,
                            Activo = Constantes.strValor_Activo,
                            FechaCreacion = DateTime.Now
                        };

                        CuentaBancariaZiPago cuenta = await DbContext.CuentasBancariasZiPago.FirstOrDefaultAsync(p => p.IdCuentaBancaria == item.CuentaBancariaZiPago.IdCuentaBancaria
                                                                                                                    && p.Activo == Constantes.strValor_Activo);
                        
                        ComercioCuentaZiPago comercioCuenta = new ComercioCuentaZiPago
                        {                            
                            Activo = Constantes.strValor_Activo,
                            FechaCreacion = DateTime.Now                            
                        };

                        comercioCuenta.ComercioZiPago = comercio;
                        comercioCuenta.CuentaBancariaZiPago = cuenta;

                        comercio.ComerciosCuentasZiPago.Add(comercioCuenta);
                        DbContext.Add(comercio);

                        await DbContext.SaveChangesAsync();
                    }

                    txAsync.Commit();
                    response.Mensaje = Constantes.strRegistroRealizado;
                }
                catch (Exception ex)
                {
                    txAsync.Rollback();
                    response.Mensaje = ex.ToString();
                    response.SetError(logger, "Negocio.Afiliacion.AfiliacionService.RegistrarComerciosAsync", nameof(List<ComercioCuentaZiPago>), ex);
                }
            }

            return response;
        }

    }
}
