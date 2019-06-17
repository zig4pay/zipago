using System;

namespace ZREL.ZiPago.Negocio.Responses
{
    public static class ResponseExtensions
    {
        public static void SetError(this IResponse response, NLog.Logger logger, string actionName, string entityName, Exception ex)
        {
            response.HizoError = true;
            logger.Error("[{0}] | {1} | Excepcion: {2}.", actionName, entityName, ex.ToString());
            response.MensajeError = ex.Message;
        }
    }
}
