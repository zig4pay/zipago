namespace ZREL.ZiPago.Aplicacion.Web.Models.Settings
{
    public class WebSiteSettingsModel
    {

        public string ZZiPagoApiUrl { get; set; }

        //Seguridad
        public string UsuarioZiPago_Obtener { get; set; }
        public string UsuarioZiPago_Autenticar { get; set; }
        public string UsuarioZiPago_Registrar { get; set; }
        
        //Comun
        public string BancoZiPago_Listar { get; set; }
        public string TablaDetalle_Listar { get; set; }
        public string UbigeoZiPago_Listar { get; set; }

        //Afiliacion
        public string AfiliacionZiPago_DatosPersonalesObtener { get; set; }
        public string AfiliacionZiPago_DomiciliosHistoricoListar { get; set; }
        public string AfiliacionZiPago_ComercioObtener { get; set; }
        public string AfiliacionZiPago_Registrar { get; set; }
        public string AfiliacionZiPago_BancosPorUsuarioListar { get; set; }
        public string AfiliacionZiPago_CuentasBancariasListarResumen { get; set; }
        public string AfiliacionZiPago_CuentasBancariasListar { get; set; }
        public string AfiliacionZiPago_CuentasBancariasRegistrar { get; set; }
        public string AfiliacionZiPago_ComerciosListar { get; set; }
        public string AfiliacionZiPago_ComerciosRegistrar { get; set; }
        public string AfiliacionZiPago_CuentasBancariasObtenerCantidadPorUsuarioAsync { get; set; }
        public string AfiliacionZiPago_ComerciosObtenerCantidadPorUsuarioAsync { get; set; }
        

        //Google ReCaptcha
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
    }
}
