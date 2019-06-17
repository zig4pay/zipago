using System;
using ZREL.ZiPago.Datos;

namespace ZREL.ZiPago.Negocio.Contracts
{
    public interface IService : IDisposable
    {
        ZiPagoDBContext DbContext { get; }
    }
}
