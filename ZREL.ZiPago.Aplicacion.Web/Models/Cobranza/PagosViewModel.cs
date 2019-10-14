using System.Collections.Generic;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Cobranza
{
    [DataContract]
    public class PagosViewModel
    {
        public int IdUsuarioZiPago { get; set; }

        public string Clave1 { get; set; }

        public string Nombre { get; set; }

        public List<EntidadGenerica> Comercios { get; set; }

        public List<TablaDetalle> Servicios { get; set; }

        public List<TablaDetalle> EstadosTxn { get; set; }

        public List<BancoZiPago> Bancos { get; set; }
    }
}
