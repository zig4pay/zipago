using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class CuentaBancariaZiPago
    {
        public int IdCuentaBancaria { get; set; }
        public int IdBancoZiPago { get; set; }
        public string NumeroCuenta { get; set; }
        public string CodigoTipoCuenta { get; set; }
        public string CodigoTipoMoneda { get; set; }
        public string CCI { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public int CodigoCuenta { get; set; }
        
        public List<ComercioCuentaZiPago> ComerciosCuentasZiPago { get; set; } = new List<ComercioCuentaZiPago>();

    }
}
