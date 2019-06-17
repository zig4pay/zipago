using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Negocio.Responses
{
    public interface IListResponse<TModel> : IResponse
    {
        IEnumerable<TModel> Model { get; set; }
    }
}
