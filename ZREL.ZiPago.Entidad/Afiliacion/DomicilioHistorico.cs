using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class DomicilioHistorico
    {
        public int Id { get; set; }

        public string Departamento { get; set; }

        public string Provincia { get; set; }

        public string Distrito { get; set; }

        public string Direccion { get; set; }

        public string Referencia { get; set; }

        public string Estado { get; set; }

        public string FechaRegistro { get; set; }

        public string FechaActualizacion { get; set; }
    }
}
