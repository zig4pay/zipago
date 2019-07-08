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

        public async Task<bool> VerificarExisteTablaDetalleAsync(Logger logger, string codTabla, string valor) {

            bool result = false;
            IQueryable<TablaDetalle> response; 

            logger.Info("[{0}] | TablaDetalle: [{1} - {2}] | Inicio.", nameof(VerificarExisteTablaDetalleAsync), codTabla, valor);

            try
            {
                var query = DbContext.TablasDetalle.AsNoTracking().Where(item => item.Cod_Tabla == codTabla && item.Valor == valor);
                response = await Task.Run(() => query);
                result = response is null ? false : response.Count() > 0 ? true : false;                                
            }
            catch (Exception ex)
            {
                logger.Error("[{0}] | TablaDetalle: [{1} - {2}] | Error: [{3}].", nameof(ObtenerMaxTablaDetalleAsync), codTabla, valor, ex.ToString());                
            }
            return result;
        }

        public async Task<string> ObtenerMaxTablaDetalleAsync(Logger logger, string CodTabla)
        {
            //IQueryable<TablaDetalle> response;
            string codigo = "";
            logger.Info("[{0}] | TablaDetalle: [{1}] | Inicio.", nameof(ObtenerMaxTablaDetalleAsync), CodTabla);
            try
            {
                var query = DbContext.TablasDetalle.AsNoTracking().Where(item => item.Cod_Tabla == CodTabla).Max(item => item.Valor);
                codigo = await Task.Run(() => query);

                logger.Info("[{0}] | TablaDetalle: [{1}] | Mensaje: [Realizado].", nameof(ObtenerMaxTablaDetalleAsync), codigo);
            }
            catch (Exception ex)
            {
                logger.Error("[{0}] | TablaDetalle: [{1}] | Error: [{2}].", nameof(ObtenerMaxTablaDetalleAsync), CodTabla, ex.ToString());
            }
            return codigo;
        }

    }
}
