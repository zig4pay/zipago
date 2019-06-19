using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Datos.Comun
{
    public static class ZiPagoDBContextExtensions
    {
        
        public static IQueryable<TablaDetalle> ObtenerTablaDetalle(this ZiPagoDBContext dbContext, string codTabla) {
            var query = dbContext.TablasDetalle.AsQueryable();

            return query.Where(item => item.Cod_Tabla == codTabla).OrderBy(item => item.Valor);
        }

    }
}
