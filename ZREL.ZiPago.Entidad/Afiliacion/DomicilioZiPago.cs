using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class DomicilioZiPago : IEntidadAuditable
    {
        public DomicilioZiPago()
        {

        }

        public int IdDomicilioZiPago { get; set; }
        public int IdUsuarioZiPago { get; set; }        
        public string CodigoDepartamento { get; set; }
        public string CodigoProvincia { get; set; }
        public string CodigoDistrito { get; set; }
        public string Via { get; set; }
        public string DireccionFacturacion { get; set; }
        public string Referencia { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
