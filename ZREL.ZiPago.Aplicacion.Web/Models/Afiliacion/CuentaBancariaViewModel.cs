using System.Collections.Generic;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Comun;
using ZREL.ZiPago.Entidad.Util;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class CuentaBancariaViewModel
    {
        public int IdUsuarioZiPago { get; set; }

        [DataMember]
        public int IdCuentaBancaria { get; set; }

        [DataMember]
        public int IdBancoZiPago { get; set; }

        [DataMember]
        public string NumeroCuenta { get; set; }

        [DataMember]
        public string CodigoTipoCuenta { get; set; }

        [DataMember]
        public string CodigoTipoMoneda { get; set; }

        [DataMember]
        public string CCI { get; set; }

        [DataMember]
        public string Activo { get; set; }

        public List<EntidadGenerica> Bancos { get; set; }
        public List<TablaDetalle> TipoCuentas { get; set; }
        public List<TablaDetalle> TipoMonedas { get; set; }


    }
}
