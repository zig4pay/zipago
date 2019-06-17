
namespace ZREL.ZiPago.Negocio.Responses
{
    interface IPagedResponse<TModel> : IListResponse<TModel>
    {
        int ItemsCount { get; set; }
        double PageCount { get; }
    }
}
