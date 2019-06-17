using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Entidad
{
    public interface IEntidadAuditable : IEntidad
    {
        DateTime? FechaCreacion { get; set; }
        DateTime? FechaActualizacion { get; set; }
    }
}
