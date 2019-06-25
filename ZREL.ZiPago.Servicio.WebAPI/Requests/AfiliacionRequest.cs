using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Seguridad;

namespace ZREL.ZiPago.Servicio.WebAPI.Requests
{
    public class AfiliacionRequest
    {
        public UsuarioZiPago entidadUsuario { get; set; }

        public DomicilioZiPago entidadDomicilio { get; set; }

        public List<ComercioCuentaZiPago> listComercioCuenta { get; set; }

    }
}
