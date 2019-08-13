using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioCuentaZiPago
    {
        public int IdComercioZiPago { get; set; }
        public ComercioZiPago ComercioZiPago { get; set; }
        
        public int IdCuentaBancaria { get; set; }
        public CuentaBancariaZiPago CuentaBancariaZiPago { get; set; }

        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
