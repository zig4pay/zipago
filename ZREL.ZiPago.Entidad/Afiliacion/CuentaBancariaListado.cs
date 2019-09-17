using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class CuentaBancariaListado
    {
        public int Id { get; set; }

        public int IdCuentaBancaria { get; set; }

        public int IdBancoZiPago { get; set; }

        public string Banco { get; set; }

        public string CodigoTipoCuenta { get; set; }

        public string TipoCuenta { get; set; }

        public string CodigoTipoMoneda { get; set; }

        public string TipoMoneda { get; set; }

        public string NumeroCuenta { get; set; }

        public string CCI { get; set; }

        public string Activo { get; set; }

        public DateTime? FechaCreacion { get; set; }
    }
}
