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
    public class BancoZiPagoService : Service, IBancoZiPagoService
    {
        public BancoZiPagoService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<ListResponse<BancoZiPago>> ListarBancoZiPagoAsync(Logger logger)
        {
            ListResponse<BancoZiPago> response = new ListResponse<BancoZiPago>();
            logger.Info("[{0}] | BancoZiPago: [{1}] | Inicio.", nameof(ListarBancoZiPagoAsync));
            try
            {
                var query = DbContext.BancosZiPago.OrderBy(item => item.NombreLargo);

                response.Model = await query.ToListAsync();

                logger.Info("[{0}] | BancoZiPago: [{1}] | Mensaje: [Realizado].", nameof(ListarBancoZiPagoAsync));
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(ListarBancoZiPagoAsync), nameof(BancoZiPago), ex);
            }
            return response;
        }
    }
}
