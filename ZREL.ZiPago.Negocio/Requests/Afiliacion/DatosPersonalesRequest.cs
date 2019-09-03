using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Negocio.Requests.Afiliacion
{
    public class DatosPersonalesRequest
    {
        public UsuarioZiPago EntidadUsuario { get; set; }

        public string OtroRubroNegocio { get; set; }

        public DomicilioZiPago EntidadDomicilio { get; set; }
    }
}
