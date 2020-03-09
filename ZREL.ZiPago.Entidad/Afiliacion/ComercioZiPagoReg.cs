using System;
using System.Collections.Generic;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioZiPagoReg
    {
        public int IdComercioZiPagoReg { get; set; }
        public string CodigoComercio { get; set; }
        public int IdUsuarioZiPago { get; set; }
        public string Descripcion { get; set; }
        public string CorreoNotificacion { get; set; }
        public string Estado { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public int CodigoCuenta { get; set; }

        public List<ComercioCuentaZiPago> ComerciosCuentasZiPago { get; set; } = new List<ComercioCuentaZiPago>();

    }
}
