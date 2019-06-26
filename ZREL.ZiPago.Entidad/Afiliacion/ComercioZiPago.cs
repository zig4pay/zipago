using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioZiPago
    {
        public int IdComercioZiPago { get; set; }
        public string CodigoComercio { get; set; }
        public int IdUsuarioZiPago { get; set; }
        public string Descripcion { get; set; }
        public string CorreoNotificacion { get; set; }
        public string Confirmado { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public List<ComercioCuentaZiPago> ComerciosCuentasZiPago { get; set; } = new List<ComercioCuentaZiPago>();

    }
}
