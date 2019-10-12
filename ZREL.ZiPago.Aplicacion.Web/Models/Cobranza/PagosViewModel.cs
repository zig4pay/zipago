using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Cobranza
{
    [DataContract]
    public class PagosViewModel
    {
        public int IdUsuarioZiPago { get; set; }

        public List<EntidadGenerica> Comercios { get; set; }

        public List<TablaDetalle> Servicios { get; set; }

        public List<TablaDetalle> EstadosTxn { get; set; }
    }
}
