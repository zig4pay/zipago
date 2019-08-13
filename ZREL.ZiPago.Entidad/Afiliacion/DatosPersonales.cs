using System;

namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class DatosPersonales
    {

        public DatosPersonales()
        {

        }

        public int IdUsuarioZiPago { get; set; }
        public string Clave1 { get; set; }
        public string Clave2 { get; set; }
        public string ApellidosUsuario { get; set; }
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
        public string UsuarioActivo { get; set; }
        public int? IdDomicilioZiPago { get; set; }
        public string CodigoDepartamento { get; set; }
        public string CodigoProvincia { get; set; }
        public string CodigoDistrito { get; set; }
        public string Via { get; set; }
        public string DireccionFacturacion { get; set; }
        public string Referencia { get; set; }
        public string DomicilioActivo { get; set; }
    }
}
