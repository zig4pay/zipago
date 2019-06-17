using System.Collections.Generic;

namespace ZREL.ZiPago.Negocio.Responses
{
    public class ListResponse<TModel> : IListResponse<TModel>
    {
        public IEnumerable<TModel> Model { get ; set ; }
        public string Mensaje { get; set; }
        public bool HizoError { get; set; }
        public string MensajeError { get; set; }
    }
}
