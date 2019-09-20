namespace ZREL.ZiPago.Sitio.Web.Models.Settings
{
    public class WebSiteSettingsModel
    {
        public string ZZiPagoApiUrl { get; set; }

        public string ZZiPagoPortalUrl { get; set; }

        //Seguridad        
        public string UsuarioZiPago_Registrar { get; set; }
        
        //Google ReCaptcha
        public string SiteKey { get; set; }
        public string SecretKey { get; set; }
    }
}
