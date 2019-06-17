using System;
using System.Collections.Generic;
using System.Text;

namespace ZREL.ZiPago.Negocio.Responses
{
    public class SingleResponse<TModel> : ISingleResponse<TModel> where TModel : new()
    {
        public SingleResponse()
        {
            Model = new TModel();
        }

        public TModel Model { get; set; }

        public string Mensaje { get; set; }

        public bool HizoError { get; set; }

        public string MensajeError { get; set; }

    }
}
