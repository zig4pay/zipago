using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IBancoZiPagoService
    {
        Task<ListResponse<BancoZiPago>> ListarBancoZiPagoAsync(Logger logger);
    }
}
