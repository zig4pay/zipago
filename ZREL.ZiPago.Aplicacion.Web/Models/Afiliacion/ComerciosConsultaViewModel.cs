using System.Collections.Generic;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Afiliacion;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class ComerciosConsultaViewModel
    {
        public int IdUsuarioZiPago { get; set; }

        public List<BancoZiPago> Bancos { get; set; }
        
    }
}
