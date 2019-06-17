
namespace ZREL.ZiPago.Negocio.Responses
{
    public class Response : IResponse
    {
        public string Mensaje { get ; set ; }
        public bool HizoError { get ; set ; }
        public string MensajeError { get ; set ; }
    }
}
