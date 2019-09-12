using System.Runtime.Serialization;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Response
{
    [DataContract]
    public class ResponseSummaryModel
    {
        [DataMember(Name = "CantidadTotal")]
        public int CantidadTotal { get; set; }

        [DataMember(Name = "MontoTotal")]
        public double MontoTotal { get; set; }

        [DataMember(Name = "Mensaje")]
        public string Mensaje { get; set; }

        [DataMember(Name = "HizoError")]
        public bool HizoError { get; set; }

        [DataMember(Name = "MensajeError")]
        public string MensajeError { get; set; }
    }
}
