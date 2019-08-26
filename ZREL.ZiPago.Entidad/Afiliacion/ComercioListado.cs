using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioListado
    {
        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public string CorreoNotificacion { get; set; }

        public string Banco { get; set; }

        public string CuentaBancaria { get; set; }

        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

    }
}
