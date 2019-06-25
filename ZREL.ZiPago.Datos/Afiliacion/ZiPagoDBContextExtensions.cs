using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;

namespace ZREL.ZiPago.Datos.Afiliacion
{
    public static class ZiPagoDBContextExtensions
    {
        public static async Task<CuentaBancariaZiPago> ObtenerCuentaBancariaZiPagoAsync(this ZiPagoDBContext dbContext, CuentaBancariaZiPago entidad)
        => await dbContext.CuentasBancariasZiPago.FirstOrDefaultAsync(item => item.IdBancoZiPago == entidad.IdBancoZiPago && 
                                                                      item.NumeroCuenta == entidad.NumeroCuenta &&
                                                                      item.CodigoTipoCuenta == entidad.CodigoTipoCuenta &&  
                                                                      item.CodigoTipoMoneda == entidad.CodigoTipoMoneda &&
                                                                      item.CCI == entidad.CCI          
                                                                    );
    }
}