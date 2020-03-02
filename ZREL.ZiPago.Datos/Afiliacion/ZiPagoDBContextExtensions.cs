using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Datos.Afiliacion
{
    public static class ZiPagoDBContextExtensions
    {

        //Datos Personales
        public static async Task<DatosPersonales> ObtenerDatosPersonalesAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago) {
            try
            {
                var result = from usuario in dbContext.UsuariosZiPago.AsNoTracking()
                             where usuario.IdUsuarioZiPago == idUsuarioZiPago
                             join domicilio in dbContext.DomiciliosZiPago.AsNoTracking() on
                                new
                                {
                                    Key1 = usuario.IdUsuarioZiPago,
                                    Key2 = true
                                } equals
                                new
                                {
                                    Key1 = domicilio.IdUsuarioZiPago,
                                    Key2 = domicilio.Activo == Constantes.strValor_Activo
                                }
                             into datosPersonales
                             from dom in datosPersonales.DefaultIfEmpty()                             
                             select new DatosPersonales
                             {
                                 IdUsuarioZiPago = usuario.IdUsuarioZiPago,
                                 Clave1 = usuario.Clave1,
                                 Clave2 = usuario.Clave2,
                                 ApellidosUsuario = usuario.ApellidosUsuario,
                                 NombresUsuario = usuario.NombresUsuario,
                                 CodigoRubroNegocio = usuario.CodigoRubroNegocio ?? "",
                                 OtroRubroNegocio = usuario.OtroRubroNegocio ?? string.Empty,
                                 CodigoTipoPersona = usuario.CodigoTipoPersona ?? "",
                                 CodigoTipoDocumento = usuario.CodigoTipoDocumento ?? "",
                                 NumeroDocumento = usuario.NumeroDocumento ?? "",
                                 RazonSocial = usuario.RazonSocial ?? "",
                                 CodigoTipoDocumentoContacto = usuario.CodigoTipoDocumentoContacto ?? "",
                                 NumeroDocumentoContacto = usuario.NumeroDocumentoContacto ?? "",
                                 ApellidoPaterno = usuario.ApellidoPaterno ?? "",
                                 ApellidoMaterno = usuario.ApellidoMaterno ?? "",
                                 Nombres = usuario.Nombres ?? "",
                                 Sexo = usuario.Sexo ?? "",
                                 FechaNacimiento = usuario.FechaNacimiento ?? DateTime.Now,
                                 TelefonoMovil = usuario.TelefonoMovil ?? "",
                                 TelefonoFijo = usuario.TelefonoFijo ?? "",
                                 AceptoTerminos = usuario.AceptoTerminos,
                                 EstadoRegistro = usuario.EstadoRegistro,
                                 UsuarioActivo = usuario.Activo,
                                 IdDomicilioZiPago = dom.IdDomicilioZiPago > 0 ? dom.IdDomicilioZiPago : 0,
                                 CodigoDepartamento = dom.CodigoDepartamento ?? "",
                                 CodigoProvincia = dom.CodigoProvincia ?? "",
                                 CodigoDistrito = dom.CodigoDistrito ?? "",
                                 Via = dom.Via ?? "",
                                 DireccionFacturacion = dom.DireccionFacturacion ?? "",
                                 Referencia = dom.Referencia ?? "",
                                 DomicilioActivo = dom.Activo ?? ""
                             };
                return await result.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return new DatosPersonales();
                throw ex;
            }
            
        }

        public static async Task<IEnumerable<DomicilioHistorico>> ListarDomiciliosHistoricoAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago)
        {
            try
            {
                var result = from domicilio in dbContext.DomiciliosZiPago.AsNoTracking()
                             where domicilio.IdUsuarioZiPago == idUsuarioZiPago
                             join ubigeodepartamento in dbContext.UbigeosZiPago.AsNoTracking() on
                                new
                                {
                                    Key1 = true,
                                    Key2 = domicilio.CodigoDepartamento
                                } equals
                                new
                                {
                                    Key1 = ubigeodepartamento.CodigoUbigeoPadre == Constantes.strUbigeoZiPago_Departamentos,
                                    Key2 = ubigeodepartamento.CodigoUbigeo
                                }
                             join ubigeoprovincia in dbContext.UbigeosZiPago.AsNoTracking() on
                                new
                                {
                                    Key1 = domicilio.CodigoDepartamento,
                                    Key2 = domicilio.CodigoProvincia
                                } equals
                                new
                                {
                                    Key1 = ubigeoprovincia.CodigoUbigeoPadre,
                                    Key2 = ubigeoprovincia.CodigoUbigeo
                                }
                             join ubigeodistrito in dbContext.UbigeosZiPago.AsNoTracking() on
                                new
                                {
                                    Key1 = domicilio.CodigoProvincia,
                                    Key2 = domicilio.CodigoDistrito
                                } equals
                                new
                                {
                                    Key1 = ubigeodistrito.CodigoUbigeoPadre,
                                    Key2 = ubigeodistrito.CodigoUbigeo
                                }
                             orderby domicilio.FechaCreacion descending
                             select new DomicilioHistorico
                             {
                                 Id = domicilio.IdDomicilioZiPago,
                                 Departamento = ubigeodepartamento.Nombre,
                                 Provincia = ubigeoprovincia.Nombre,
                                 Distrito = ubigeodistrito.Nombre,
                                 Direccion = domicilio.Via + " " + domicilio.DireccionFacturacion.Trim(),
                                 Referencia = domicilio.Referencia,
                                 Estado = domicilio.Activo == Constantes.strValor_Activo ? "Activo" : "Inactivo",
                                 FechaRegistro = domicilio.FechaCreacion.Value.ToShortDateString() + " " + domicilio.FechaCreacion.Value.ToShortTimeString(),
                                 FechaActualizacion = domicilio.FechaActualizacion.Value != null ? 
                                                        domicilio.FechaActualizacion.Value.ToShortDateString() + " " + domicilio.FechaActualizacion.Value.ToShortTimeString() : 
                                                            string.Empty
                             };
                             
                return await result.ToListAsync();
            }
            catch (Exception ex)
            {                
                throw ex;
            }
        }

        //Cuentas Bancarias
        public static async Task<CuentaBancariaZiPago> ObtenerCuentaBancariaZiPagoAsync(this ZiPagoDBContext dbContext, CuentaBancariaZiPago entidad)
        {
            return await dbContext.CuentasBancariasZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.IdUsuarioZiPago == entidad.IdUsuarioZiPago &&
                                                                                                     item.IdBancoZiPago == entidad.IdBancoZiPago &&
                                                                                                     item.NumeroCuenta == entidad.NumeroCuenta 
                                                                                            );
        }

        public static async Task<CuentaBancariaZiPago> ObtenerCuentaBancariaZiPagoPorIdAsync(this ZiPagoDBContext dbContext, int idCuentaBancaria)
        {
            return await dbContext.CuentasBancariasZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.IdCuentaBancaria == idCuentaBancaria);
        }

        public static async Task<IEnumerable<BancoZiPago>> ListarBancosPorUsuarioAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago)
        {

            var result = from cuentabancaria in dbContext.CuentasBancariasZiPago.AsNoTracking()
                         where cuentabancaria.IdUsuarioZiPago == idUsuarioZiPago
                         join banco in dbContext.BancosZiPago.AsNoTracking()
                            on cuentabancaria.IdBancoZiPago equals banco.IdBancoZiPago
                         orderby banco.NombreLargo
                         select new BancoZiPago
                         {
                             IdBancoZiPago = cuentabancaria.IdBancoZiPago,
                             NombreLargo = banco.NombreLargo
                         };

            return await result.Distinct().ToListAsync();

        }
        
        public static async Task<IEnumerable<CuentaBancariaListado>> ListarCuentasBancariasAsync(this ZiPagoDBContext dbContext, CuentaBancariaFiltros cuentaFiltros)
        {

            var result = from cuentabancaria in dbContext.CuentasBancariasZiPago.AsNoTracking()                         
                         join banco in dbContext.BancosZiPago.AsNoTracking()
                            on cuentabancaria.IdBancoZiPago equals banco.IdBancoZiPago
                         join tipocuenta in dbContext.TablasDetalle.AsNoTracking()
                            on new
                            {
                                Key1 = true,
                                Key2 = cuentabancaria.CodigoTipoCuenta
                            } equals
                               new
                               {
                                   Key1 = tipocuenta.Cod_Tabla == Constantes.strCodTablaTipoCuenta,
                                   Key2 = tipocuenta.Valor
                               }
                         join tipomoneda in dbContext.TablasDetalle.AsNoTracking()
                            on new
                            {
                                Key1 = true,
                                Key2 = cuentabancaria.CodigoTipoMoneda
                            } equals
                               new
                               {
                                   Key1 = tipomoneda.Cod_Tabla == Constantes.strCodTablaTipoMoneda,
                                   Key2 = tipomoneda.Valor
                               }                         
                         orderby banco.NombreLargo
                         select new CuentaBancariaListado
                         {
                             Id = cuentabancaria.IdUsuarioZiPago,
                             IdCuentaBancaria = cuentabancaria.IdCuentaBancaria,
                             IdBancoZiPago = cuentabancaria.IdBancoZiPago,
                             Banco = banco.NombreLargo,
                             CodigoTipoCuenta = tipocuenta.Valor,
                             TipoCuenta = tipocuenta.Descr_Valor,
                             CodigoTipoMoneda = tipomoneda.Valor,
                             TipoMoneda = tipomoneda.Descr_Valor,
                             NumeroCuenta = cuentabancaria.NumeroCuenta,
                             CCI = cuentabancaria.CCI,
                             Estado = cuentabancaria.Activo,
                             FechaCreacion = cuentabancaria.FechaCreacion
                         };

            result = result.Where(p => p.Id == cuentaFiltros.IdUsuarioZiPago);

            if (cuentaFiltros.IdBancoZiPago != null && cuentaFiltros.IdBancoZiPago > 0)
                result = result.Where(p => p.IdBancoZiPago == cuentaFiltros.IdBancoZiPago);

            if (!string.IsNullOrWhiteSpace(cuentaFiltros.CodigoTipoCuenta) && cuentaFiltros.CodigoTipoCuenta != "00")
                result = result.Where(p => p.CodigoTipoCuenta == cuentaFiltros.CodigoTipoCuenta);

            if (!string.IsNullOrWhiteSpace(cuentaFiltros.CodigoTipoMoneda) && cuentaFiltros.CodigoTipoMoneda != "00")
                result = result.Where(p => p.CodigoTipoMoneda == cuentaFiltros.CodigoTipoMoneda);

            if (!string.IsNullOrWhiteSpace(cuentaFiltros.Activo) && cuentaFiltros.Activo != "0")
                result = result.Where(p => p.Estado == cuentaFiltros.Activo);

            if (!string.IsNullOrWhiteSpace(cuentaFiltros.NumeroCuenta))
                result = result.Where(p => p.NumeroCuenta.Contains(cuentaFiltros.NumeroCuenta));

            return await result.ToListAsync();

        }

        public static async Task<IEnumerable<CuentaBancariaListaResumida>> ListarCuentasBancariasResumenAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago, int idBancoZiPago)
        {

            var result = from cuentabancaria in dbContext.CuentasBancariasZiPago.AsNoTracking()
                         where cuentabancaria.IdUsuarioZiPago == idUsuarioZiPago && cuentabancaria.IdBancoZiPago == idBancoZiPago
                         join tipocuenta in dbContext.TablasDetalle.AsNoTracking()
                            on new
                            {
                                Key1 = true,
                                Key2 = cuentabancaria.CodigoTipoCuenta
                            } equals
                               new
                               {
                                   Key1 = tipocuenta.Cod_Tabla == Constantes.strCodTablaTipoCuenta,
                                   Key2 = tipocuenta.Valor
                               }
                         join tipomoneda in dbContext.TablasDetalle.AsNoTracking()
                            on new
                            {
                                Key1 = true,
                                Key2 = cuentabancaria.CodigoTipoMoneda
                            } equals
                               new
                               {
                                   Key1 = tipomoneda.Cod_Tabla == Constantes.strCodTablaTipoMoneda,
                                   Key2 = tipomoneda.Valor
                               }
                         orderby tipocuenta.Descr_Valor, tipomoneda.Descr_Valor
                         select new CuentaBancariaListaResumida
                         {
                             IdCuentaBancaria = cuentabancaria.IdCuentaBancaria,
                             Descripcion = tipocuenta.Descr_Valor.Trim() + " " +
                                           tipomoneda.Descr_Valor.Trim() +
                                           " - Nro: " + cuentabancaria.NumeroCuenta.Trim() +
                                           " - CCI: " + (cuentabancaria.CCI.Trim() ?? "")
                         };

            return await result.ToListAsync();

        }

        public static async Task<Int32> ObtenerCantidadCuentasBancariasPorUsuarioAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago)
        {
            return await dbContext.CuentasBancariasZiPago.Where(p => p.IdUsuarioZiPago == idUsuarioZiPago &&
                                                                p.Activo == Constantes.strValor_Activo).CountAsync();
        }

        //Comercios
        public static async Task<IEnumerable<EntidadGenerica>> ListarComerciosAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago)
        {
            return await dbContext.ComerciosZiPagoReg
                            .AsNoTracking()
                            .Where(item => item.IdUsuarioZiPago == idUsuarioZiPago &&
                                           item.Activo == Constantes.strValor_Activo)
                            .OrderBy(item => item.CodigoComercio)
                            .Select(item => new EntidadGenerica
                            {
                                IdEntidad = item.IdComercioZiPagoReg,
                                Descripcion = item.CodigoComercio
                            })
                            .ToListAsync();                            
        }

        public static async Task<ComercioZiPagoReg> ObtenerComercioZiPagoAsync(this ZiPagoDBContext dbContext, string codigoComercio)
        {
            return await dbContext.ComerciosZiPagoReg.AsNoTracking().FirstOrDefaultAsync(item => item.CodigoComercio == codigoComercio);
        }
        public static async Task<IEnumerable<ComercioListado>> ListarComerciosAsync(this ZiPagoDBContext dbContext, ComercioFiltros comercioFiltros)
        {

            var result = from comercios in dbContext.ComerciosZiPagoReg.AsNoTracking()
                         join comerciocuenta in dbContext.ComerciosCuentasZiPago.AsNoTracking()
                            on comercios.IdComercioZiPagoReg equals comerciocuenta.IdComercioZiPagoReg
                         join cuentabancaria in dbContext.CuentasBancariasZiPago.AsNoTracking()
                            on comerciocuenta.IdCuentaBancaria equals cuentabancaria.IdCuentaBancaria
                         join banco in dbContext.BancosZiPago.AsNoTracking()
                            on cuentabancaria.IdBancoZiPago equals banco.IdBancoZiPago
                         join tipocuenta in dbContext.TablasDetalle.AsNoTracking()
                            on new
                            {
                                Key1 = true,
                                Key2 = cuentabancaria.CodigoTipoCuenta
                            } equals
                            new
                            {
                                Key1 = tipocuenta.Cod_Tabla == Constantes.strCodTablaTipoCuenta,
                                Key2 = tipocuenta.Valor
                            }
                         join tipomoneda in dbContext.TablasDetalle.AsNoTracking()
                             on new
                             {
                                 Key1 = true,
                                 Key2 = cuentabancaria.CodigoTipoMoneda
                             } equals
                                new
                                {
                                    Key1 = tipomoneda.Cod_Tabla == Constantes.strCodTablaTipoMoneda,
                                    Key2 = tipomoneda.Valor
                                }
                         orderby comercios.CodigoComercio                         
                         select new ComercioListado
                         {
                             Id = comercios.IdUsuarioZiPago,
                             Codigo = comercios.CodigoComercio,
                             Descripcion = comercios.Descripcion,
                             CorreoNotificacion = comercios.CorreoNotificacion,
                             IdBancoZiPago = banco.IdBancoZiPago,
                             Banco = banco.NombreLargo,
                             TipoCuentaBancaria = tipocuenta.Descr_Valor.Trim(),
                             MonedaCuentaBancaria = tipomoneda.Descr_Valor.Trim(),
                             CuentaBancaria = Constantes.strBancosAfiliados_Codigos.Contains("["+ banco.IdBancoZiPago.ToString()+"]") ? cuentabancaria.NumeroCuenta.Trim() : cuentabancaria.CCI.Trim(),
                             Estado = comercios.Estado,
                             FechaCreacion = comercios.FechaCreacion
                         };

            result = result.Where(p => p.Id == comercioFiltros.IdUsuarioZiPago);

            if (!string.IsNullOrWhiteSpace(comercioFiltros.CodigoComercio))
                result = result.Where(p => p.Codigo.Contains(comercioFiltros.CodigoComercio));

            if (!string.IsNullOrWhiteSpace(comercioFiltros.Descripcion))
                result = result.Where(p => p.Descripcion.Contains(comercioFiltros.Descripcion));

            if (!string.IsNullOrWhiteSpace(comercioFiltros.Estado) && comercioFiltros.Estado != "0")
                result = result.Where(p => p.Estado == comercioFiltros.Estado);

            if (comercioFiltros.IdBancoZiPago != null && comercioFiltros.IdBancoZiPago > 0)
                result = result.Where(p => p.IdBancoZiPago == comercioFiltros.IdBancoZiPago);

            if (!string.IsNullOrWhiteSpace(comercioFiltros.NumeroCuenta))
                result = result.Where(p => p.CuentaBancaria.Contains(comercioFiltros.NumeroCuenta));

            return await result.ToListAsync();

        }

        public static async Task<Int32> ObtenerCantidadComerciosPorUsuarioAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago) {
            return await dbContext.ComerciosZiPagoReg.Where(p => p.IdUsuarioZiPago == idUsuarioZiPago &&
                                                                 p.Activo == Constantes.strValor_Activo).CountAsync();
        }

    }
}