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
    public class UbigeoZiPagoService : Service, IUbigeoZiPagoService
    {

        public UbigeoZiPagoService(ZiPagoDBContext dbContext) : base(dbContext)
        {

        }


        public async Task<ListResponse<UbigeoZiPago>> ListarUbigeoZiPagoAsync(Logger logger, string CodigoUbigeoPadre)
        {
            ListResponse<UbigeoZiPago> response = new ListResponse<UbigeoZiPago>();
            logger.Info("[{0}] | UbigeoZiPago: [{1}] | Inicio.", nameof(ListarUbigeoZiPagoAsync), CodigoUbigeoPadre);
            try
            {
                
                response.Model = await DbContext.UbigeosZiPago
                                        .Where(item => item.CodigoUbigeoPadre == CodigoUbigeoPadre)
                                        .OrderBy(item => item.Nombre).ToListAsync();

                logger.Info("[{0}] | UbigeoZiPago: [{1}] | Mensaje: [Realizado].", nameof(ListarUbigeoZiPagoAsync), CodigoUbigeoPadre);
            }
            catch (Exception ex)
            {
                response.Model = null;
                response.SetError(logger, nameof(ListarUbigeoZiPagoAsync), nameof(UbigeoZiPago), ex);
            }
            return response;
        }
        
    }
}
