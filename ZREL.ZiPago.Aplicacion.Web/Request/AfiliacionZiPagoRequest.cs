using System.Collections.Generic;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Request
{
    public class AfiliacionZiPagoRequest
    {
        public UsuarioZiPago EntidadUsuario { get; set; }

        public DomicilioZiPago EntidadDomicilio { get; set; }

        public List<ComercioCuentaZiPago> ListComercioCuenta { get; set; }
    }
}
