using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Afiliacion
{
    public static class ZiPagoDBContextExtensions
    {
        public static async Task<CuentaBancariaZiPago> ObtenerCuentaBancariaZiPagoAsync(this ZiPagoDBContext dbContext, CuentaBancariaZiPago entidad)
        => await dbContext.CuentasBancariasZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.IdBancoZiPago == entidad.IdBancoZiPago && 
                                                                      item.NumeroCuenta == entidad.NumeroCuenta &&
                                                                      item.CodigoTipoCuenta == entidad.CodigoTipoCuenta &&  
                                                                      item.CodigoTipoMoneda == entidad.CodigoTipoMoneda &&
                                                                      item.CCI == entidad.CCI          
                                                                    );
        
        public static async Task<ComercioZiPago> ObtenerComercioZiPagoAsync(this ZiPagoDBContext dbContext, string codigoComercio)
            => await dbContext.ComerciosZiPago.AsNoTracking().FirstOrDefaultAsync(item => item.CodigoComercio == codigoComercio);
    }
}