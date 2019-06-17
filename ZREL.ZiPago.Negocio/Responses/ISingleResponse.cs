
namespace ZREL.ZiPago.Negocio.Responses
{
    public interface ISingleResponse<TModel> : IResponse
    {
        TModel Model { get; set; }
    }
}
