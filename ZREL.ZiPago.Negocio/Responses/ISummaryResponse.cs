using System;

namespace ZREL.ZiPago.Negocio.Responses
{
    public interface ISummaryResponse : IResponse
    {
        Int32 CantidadTotal { get; set; }
        Double MontoTotal { get; set; }
    }
}
