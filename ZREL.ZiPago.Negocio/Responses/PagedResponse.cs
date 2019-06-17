using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Negocio.Responses
{
    public class PagedResponse<TModel> : IPagedResponse<TModel>
    {
        public IEnumerable<TModel> Model { get ; set ; }

        public string Mensaje { get; set; }

        public bool HizoError { get; set; }

        public string MensajeError { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int ItemsCount { get; set; }

        public double PageCount => ItemsCount<PageSize? 1 : (int) (((double) ItemsCount / PageSize) + 1);
    }
}
