using System;

namespace ZREL.ZiPago.Negocio.Responses
{
    public static class ResponseExtensions
    {
        public static void SetError(this IResponse response, NLog.Logger logger, string mensaje, string entityName, Exception ex)
        {
            response.HizoError = true;
            response.MensajeError = ex.Message;
            logger.Error("[{0}] | {1} | Excepcion: {2}.", mensaje, entityName, ex.ToString());
        }
    }
}
