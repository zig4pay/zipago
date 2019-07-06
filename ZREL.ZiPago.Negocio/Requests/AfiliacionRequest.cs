using System.Collections.Generic;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Negocio.Requests
{
    public class AfiliacionRequest
    {
        public UsuarioZiPago EntidadUsuario { get; set; }

        public string OtroRubroNegocio { get; set; }

        public DomicilioZiPago EntidadDomicilio { get; set; }

        public List<ComercioCuentaZiPago> ListComercioCuenta { get; set; }
    }
}
