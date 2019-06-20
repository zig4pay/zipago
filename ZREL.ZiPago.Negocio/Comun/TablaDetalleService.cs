using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Comun
{
    public class TablaDetalleService : Service, ITablaDetalleService
    {
        public TablaDetalleService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<ListResponse<TablaDetalle>> ListarTablaDetalleAsync(Logger logger, TablaDetalle entidad)
        {
            ListResponse<TablaDetalle> response = new ListResponse<TablaDetalle>();
            logger.Info("[{0}] | TablaDetalle: [{1}] | Inicio.", nameof(ListarTablaDetalleAsync), entidad.Cod_Tabla);
            try
            {
                var query = DbContext.TablasDetalle.Where(item => item.Cod_Tabla == entidad.Cod_Tabla).OrderBy(item => item.Descr_Valor);

                response.Model = await query.ToListAsync();

                logger.Info("[{0}] | TablaDetalle: [{1}] | Mensaje: [Realizado].", nameof(ListarTablaDetalleAsync), entidad.Cod_Tabla);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(ListarTablaDetalleAsync), nameof(TablaDetalle), ex);
            }
            return response;
        }
    }
}
