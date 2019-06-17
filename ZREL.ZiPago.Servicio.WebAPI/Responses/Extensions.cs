using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZREL.ZiPago.Negocio.Responses;

namespace ZREL.ZiPago.Servicio.WebAPI.Responses
{
    public static class Extensions
    {
        public static IActionResult ToHttpResponse<TModel>(this IListResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.HizoError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NoContent;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse<TModel>(this ISingleResponse<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (response.HizoError)
                status = HttpStatusCode.InternalServerError;
            else if (response.Model == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static IActionResult ToHttpResponse(this IResponse response)
        {
            var status = response.HizoError ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }
    }
}
