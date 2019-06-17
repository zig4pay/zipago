using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Seguridad
{
    [DataContract]
    public class UsuarioViewModel
    {

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

        [Required]
        [Display(Name = "Apellidos")]
        public string ApellidosUsuario { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        public string NombresUsuario { get; set; }

        public string CodigoRubroNegocio { get; set; }

        public string CodigoTipoPersona { get; set; }

        public string CodigoTipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

        public string RazonSocial { get; set; }

        public string ApellidoPaterno { get; set; }

        public string ApellidoMaterno { get; set; }

        public string Nombres { get; set; }

        public string Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string TelefonoMovil { get; set; }

        public string TelefonoFijo { get; set; }

        public string AceptoTerminos { get; set; }

        public string Activo { get; set; }
        
    }
}
