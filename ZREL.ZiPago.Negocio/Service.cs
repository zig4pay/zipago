using System;
using System.Collections.Generic;
using System.Text;
using ZREL.ZiPago.Datos;
using ZREL.ZiPago.Negocio.Contracts;

namespace ZREL.ZiPago.Negocio
{
    public abstract class Service : IService
    {

        protected bool Disposed;

        public ZiPagoDBContext DbContext { get; }

        public Service(ZiPagoDBContext dbContext)
        {            
            DbContext = dbContext;
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                DbContext?.Dispose();
                Disposed = true;
            }
        }
    }
}
