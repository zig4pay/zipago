namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class CuentaBancariaFiltros
    {
        
        public int IdUsuarioZiPago { get; set; }

        public int? IdBancoZiPago { get; set; }

        public string NumeroCuenta { get; set; }

        public string CodigoTipoCuenta { get; set; }

        public string CodigoTipoMoneda { get; set; }
                
        public string Activo { get; set; }

    }
}
