using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Aplicacion.Web.Request
{
    public class AfiliacionDatosRequest
    {
        public UsuarioZiPago EntidadUsuario { get; set; }

        public string OtroRubroNegocio { get; set; }

        public DomicilioZiPago EntidadDomicilio { get; set; }
    }
}
