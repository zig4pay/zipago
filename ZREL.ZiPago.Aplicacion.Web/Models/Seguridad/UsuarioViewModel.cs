using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Seguridad
{
    [DataContract]
    public class UsuarioViewModel : GoogleReCaptchaModelBase
    {

        [DataMember]
        public int IdUsuarioZiPago { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Id ZiPago")]
        [DataMember]
        public string Clave1 { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Clave")]
        [DataMember]
        public string Clave2 { get; set; }
                
        [Display(Name = "Apellidos")]
        [DataMember]
        public string ApellidosUsuario { get; set; }
                
        [Display(Name = "Nombres")]
        [DataMember]
        public string NombresUsuario { get; set; }

        [DataMember]
        public string AceptoTerminos { get; set; }

    }
}
