using System;

namespace ZREL.ZiPago.Entidad.Seguridad
{
    public class UsuarioZiPago : IEntidadAuditable
    {

        public UsuarioZiPago() {

        }        

        public UsuarioZiPago(string clave1)
        {
            Clave1 = clave1;
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
        public string CodigoTipoDocumentoContacto { get; set; }
        public string NumeroDocumentoContacto { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string TelefonoMovil { get; set; }
        public string TelefonoFijo { get; set; }
        public string AceptoTerminos { get; set; }
        public string EstadoRegistro { get; set; }
        public string Activo { get; set; }
        public DateTime? FechaCreacion { get ; set ; }
        public DateTime? FechaActualizacion { get; set ; }

    }
}
