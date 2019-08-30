namespace ZREL.ZiPago.Entidad.Afiliacion
{
    public class ComercioFiltros
    {
        public int IdUsuarioZiPago { get; set; }

        public string CodigoComercio { get; set; }

        public string Descripcion { get; set; }

        public string Activo { get; set; }

        public int? IdBancoZiPago { get; set; }

        public string NumeroCuenta { get; set; }
    }
}
