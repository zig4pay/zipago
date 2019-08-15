using System.Collections.Generic;

namespace ZREL.ZiPago.Negocio.Responses
{
    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Model { get; set; }
    }
}
