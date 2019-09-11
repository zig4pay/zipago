
namespace ZREL.ZiPago.Negocio.Responses
{
    public class SummaryResponse : ISummaryResponse
    {
        public int CantidadTotal { get; set; }
        public double MontoTotal { get; set; }
        public string Mensaje { get; set; }
        public bool HizoError { get; set; }
        public string MensajeError { get; set; }
    }
}
