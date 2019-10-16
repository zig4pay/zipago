using System;

namespace ZREL.ZiPago.Entidad.Comun
{
    public class BancoZiPago
    {
        public BancoZiPago()
        {

        }

        public int IdBancoZiPago { get; set; }
        public string NombreLargo { get; set; }
        public string NombreCorto { get; set; }
        public string CodigoRecaudador { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
