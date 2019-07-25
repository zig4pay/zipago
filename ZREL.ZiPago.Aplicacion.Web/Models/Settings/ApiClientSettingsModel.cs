﻿
namespace ZREL.ZiPago.Aplicacion.Web.Models.Settings
{
    public class ApiClientSettingsModel
    {

        public string ZZiPagoUrl { get; set; }
        
        //Seguridad
        public string UsuarioZiPago_Autenticar { get; set; }
        public string UsuarioZiPago_Registrar { get; set; }

        //Comun
        public string BancoZiPago_Listar { get; set; }
        public string TablaDetalle_Listar { get; set; }
        public string UbigeoZiPago_Listar { get; set; }

        //Afiliacion
        public string AfiliacionZiPago_Registrar { get; set; }
        public string AfiliacionZiPago_ComercioObtener { get; set; }

    }
}
