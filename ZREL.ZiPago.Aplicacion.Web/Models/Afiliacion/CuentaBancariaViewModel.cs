using System.Collections.Generic;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class CuentaBancariaViewModel
    {
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

        public List<BancoZiPago> Bancos { get; set; }
        public List<TablaDetalle> TipoCuentas { get; set; }
        public List<TablaDetalle> TipoMonedas { get; set; }


    }
}
