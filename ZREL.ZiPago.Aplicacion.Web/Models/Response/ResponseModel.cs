using System.Runtime.Serialization;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Response
{
    [DataContract]
    public class ResponseModel<T>
    {
        [DataMember(Name = "Model")]
        public T Model { get; set; }

        [DataMember(Name = "Mensaje")]
        public string Mensaje { get; set; }

        [DataMember(Name = "HizoError")]
        public bool HizoError { get; set; }

        [DataMember(Name = "MensajeError")]
        public string MensajeError { get; set; }
    }
}
