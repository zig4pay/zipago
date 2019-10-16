using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;
using ZREL.ZiPago.Negocio.Contracts;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Comun
{
    public class BancoZiPagoService : Service, IBancoZiPagoService
    {
        public BancoZiPagoService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }

        public async Task<ListResponse<EntidadGenerica>> ListarBancoZiPagoAsync(Logger logger)
        {
            ListResponse<EntidadGenerica> response = new ListResponse<EntidadGenerica>();
            logger.Info("[Negocio.Comun.BancoZiPagoService.ListarBancoZiPagoAsync] | Inicio.");
            try
            {
                var query = DbContext.BancosZiPago.
                                Select(item => new EntidadGenerica
                                {
                                    IdEntidad = item.IdBancoZiPago,
                                    Descripcion = item.NombreLargo
                                }).
                                OrderBy(item => item.Descripcion);
                response.Model = await query.ToListAsync();
                logger.Info("[Negocio.Comun.BancoZiPagoService.ListarBancoZiPagoAsync] | Realizado.");
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
