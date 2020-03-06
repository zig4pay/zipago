using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioListado
    {
        public int Id { get; set; }

        public int IdComercio { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public string CorreoNotificacion { get; set; }

        public int IdBancoZiPago { get; set; }

        public string Banco { get; set; }

        public string TipoCuentaBancaria { get; set; }

        public string MonedaCuentaBancaria { get; set; }

        public string CuentaBancaria { get; set; }

        public string Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }

    }
}
