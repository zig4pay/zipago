using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Libreria;

namespace ZREL.ZiPago.Datos.Afiliacion
{
    public static class ZiPagoDBContextExtensions
    {

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

        public static async Task<CuentaBancariaZiPago> ObtenerCuentaBancariaZiPagoAsync(this ZiPagoDBContext dbContext, CuentaBancariaZiPago entidad)
        {
            return await dbContext.CuentasBancariasZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.IdBancoZiPago == entidad.IdBancoZiPago &&
                                                                                  item.NumeroCuenta == entidad.NumeroCuenta &&
                                                                                  item.CodigoTipoCuenta == entidad.CodigoTipoCuenta &&
                                                                                  item.CodigoTipoMoneda == entidad.CodigoTipoMoneda &&
                                                                                  item.CCI == entidad.CCI
                                                                               );
        }

        public static async Task<ComercioZiPago> ObtenerComercioZiPagoAsync(this ZiPagoDBContext dbContext, string codigoComercio)
        {
            return await dbContext.ComerciosZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.CodigoComercio == codigoComercio);
        }

        public static async Task<IEnumerable<CuentaBancariaListado>> ListarCuentasBancariasAsync(this ZiPagoDBContext dbContext, int idUsuarioZiPago) {

            var result = from cuentabancaria in dbContext.CuentasBancariasZiPago.AsNoTracking()
                         where cuentabancaria.IdUsuarioZiPago == idUsuarioZiPago
                         join banco in dbContext.BancosZiPago.AsNoTracking()
                            on cuentabancaria.IdBancoZiPago equals banco.IdBancoZiPago
                         join tipocuenta in dbContext.TablasDetalle.AsNoTracking()
                            on new {
                                        Key1 = true,
                                        Key2 = cuentabancaria.CodigoTipoCuenta
                                   } equals 
                               new {
                                        Key1 = tipocuenta.Cod_Tabla == Constantes.strCodTablaTipoCuenta,
                                        Key2 = tipocuenta.Valor
                                   }
                         join tipomoneda in dbContext.TablasDetalle.AsNoTracking()
                            on new {
                                        Key1 = true,
                                        Key2 = cuentabancaria.CodigoTipoMoneda
                                   } equals 
                               new {
                                        Key1 = tipomoneda.Cod_Tabla == Constantes.strCodTablaTipoMoneda,
                                        Key2 = tipomoneda.Valor
                                   }
                         orderby banco.NombreLargo
                         select new CuentaBancariaListado 
                         {
                             IdCuentaBancaria = cuentabancaria.IdCuentaBancaria,
                             Banco = banco.NombreLargo,
                             TipoCuenta = tipocuenta.Descr_Valor,
                             TipoMoneda = tipomoneda.Descr_Valor,
                             NumeroCuenta = cuentabancaria.NumeroCuenta,
                             CCI = cuentabancaria.CCI,
                             Estado = cuentabancaria.Activo == Constantes.strValor_Activo ? "Activo" : "Inactivo",
                             FechaCreacion = cuentabancaria.FechaCreacion
                         };

            return await result.ToListAsync();

        }


    }
}