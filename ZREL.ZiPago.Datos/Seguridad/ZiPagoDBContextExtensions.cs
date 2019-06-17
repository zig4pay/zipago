using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Datos.Configuraciones.Seguridad
{
    public static class ZiPagoDBContextExtensions
    {
        public static async Task<UsuarioZiPago> ObtenerUsuarioZiPagoAsync(this ZiPagoDBContext dbContext, string clave1)            
        => await dbContext.UsuariosZiPago.FirstOrDefaultAsync(item => item.Clave1 == clave1);
    }
}
