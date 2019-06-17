
namespace ZREL.ZiPago.Negocio.Responses
{
    public interface IResponse
    {
        string Mensaje { get; set; }

        bool HizoError { get; set; }

        string MensajeError { get; set; }
    }
}
