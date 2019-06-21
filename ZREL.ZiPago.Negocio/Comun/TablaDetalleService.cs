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

        public async Task<ListResponse<TablaDetalle>> ListarTablaDetalleAsync(Logger logger, string CodTabla)
        {
            ListResponse<TablaDetalle> response = new ListResponse<TablaDetalle>();
            logger.Info("[{0}] | TablaDetalle: [{1}] | Inicio.", nameof(ListarTablaDetalleAsync), CodTabla);
            try
            {
                var query = DbContext.TablasDetalle.Where(item => item.Cod_Tabla == CodTabla).OrderBy(item => item.Descr_Valor);

                response.Model = await query.ToListAsync();

                logger.Info("[{0}] | TablaDetalle: [{1}] | Mensaje: [Realizado].", nameof(ListarTablaDetalleAsync), CodTabla);
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
