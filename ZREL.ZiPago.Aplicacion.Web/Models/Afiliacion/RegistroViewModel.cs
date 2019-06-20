using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class RegistroViewModel
    {
        [Required]
        [DataMember]
        public int IdUsuarioZiPago { get; set; }

        [Required]
        [EmailAddress]
        [DataMember]
        public string Clave1 { get; set; }
        
        [DataType(DataType.Password)]
        [DataMember]
        public string Clave2 { get; set; }
                
        [DataMember]
        public string ApellidosUsuario { get; set; }

        [DataMember]
        public string NombresUsuario { get; set; }

        [DataMember]
        public string CodigoRubroNegocio { get; set; }

        public string OtroRubroNegocio { get; set; }

        [DataMember]
        public string CodigoTipoPersona { get; set; }

        [DataMember]
        public string CodigoTipoDocumento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        public string NumeroRUC { get; set; }

        public string NumeroDNI { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public string ApellidoPaterno { get; set; }

        [DataMember]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        public string Nombres { get; set; }

        [DataMember]
        public string Sexo { get; set; }

        [DataMember]
        public DateTime? FechaNacimiento { get; set; }

        [DataMember]
        public string TelefonoMovil { get; set; }

        [DataMember]
        public string TelefonoFijo { get; set; }

        [DataMember]
        public string AceptoTerminos { get; set; }        
        
        public List<TablaDetalle> RubroNegocio { get; set; }

        public List<TablaDetalle> Departamento { get; set; }

        public List<TablaDetalle> Provincia { get; set; }

        public List<TablaDetalle> Distrito { get; set; }

        [DataMember]
        public int IdDomicilioZiPago { get; set; }

        [DataMember]
        public string CodigoDepartamento { get; set; }

        [DataMember]
        public string CodigoProvincia { get; set; }

        [DataMember]
        public string CodigoDistrito { get; set; }

        [DataMember]
        public string Via { get; set; }

        [DataMember]
        public string DireccionFacturacion { get; set; }

        [DataMember]
        public string Referencia { get; set; }

    }
}
