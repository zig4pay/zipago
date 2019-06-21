using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Entidad.Comun
{
    public class UbigeoZiPago
    {

        public UbigeoZiPago()
        {
            
        }

        public int IdUbigeoZiPago { get; set; }
        public string CodigoUbigeo { get; set; }
        public string CodigoUbigeoPadre { get; set; }
        public string Nombre { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
