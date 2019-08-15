using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class CuentaBancariaListado
    {
        public int IdCuentaBancaria { get; set; }

        public string Banco { get; set; }

        public string TipoCuenta { get; set; }

        public string TipoMoneda { get; set; }

        public string NumeroCuenta { get; set; }

        public string CCI { get; set; }

        public string Estado { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
