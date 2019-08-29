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

        public string CodigoComercio { get; set; }

        public string Descripcion { get; set; }

        public string Activo { get; set; }

        public int IdBancoZiPago { get; set; }

        public string NumeroCuenta { get; set; }

        public List<BancoZiPago> Bancos { get; set; }
        
    }
}
