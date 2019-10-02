using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ZREL.ZiPago.Entidad.Comun;

namespace ZREL.ZiPago.Aplicacion.Web.Models.Afiliacion
{
    [DataContract]
    public class DatosPersonalesViewModel
    {

        #region -- Usuario --
        [Required]
        [DataMember]
        public int IdUsuarioZiPago { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Id ZiPago")]
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

        [DataMember]
        public string OtroRubroNegocio { get; set; }

        [DataMember]
        public string CodigoTipoPersona { get; set; }

        [DataMember]
        public string CodigoTipoDocumento { get; set; }

        [DataMember]
        public string NumeroDocumento { get; set; }

        [DataMember]
        public string RazonSocial { get; set; }

        [DataMember]
        public string CodigoTipoDocumentoContacto { get; set; }

        [DataMember]
        public string NumeroDocumentoContacto { get; set; }

        [DataMember]
        public string ApellidoPaterno { get; set; }

        [DataMember]
        public string ApellidoMaterno { get; set; }

        [DataMember]
        public string Nombres { get; set; }

        [DataMember]
        public string Sexo { get; set; }

        [DataMember]
        public string FechaNacimiento { get; set; }

        [DataMember]
        public string TelefonoMovil { get; set; }

        [DataMember]
        public string TelefonoFijo { get; set; }

        [DataMember]
        public string AceptoTerminos { get; set; }

        [DataMember]
        public string EstadoRegistro { get; set; }

        #endregion

        #region -- Domicilio --
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
        #endregion

        #region -- Listas --
        public List<TablaDetalle> TipoPersona { get; set; }
        public List<TablaDetalle> RubroNegocio { get; set; }
        public List<TablaDetalle> TipoDocIdentidad { get; set; }
        public List<UbigeoZiPago> Departamento { get; set; }
        public List<UbigeoZiPago> Provincia { get; set; }
        public List<UbigeoZiPago> Distrito { get; set; }
        #endregion

    }
}
