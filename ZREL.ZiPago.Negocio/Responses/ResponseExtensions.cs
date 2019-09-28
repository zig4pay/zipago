using System;

namespace ZREL.ZiPago.Negocio.Responses
{
    public static class ResponseExtensions
    {
        public static void SetError(this IResponse response, NLog.Logger logger, string ruta, string datos, Exception ex)
        {
            response.HizoError = true;
            response.MensajeError = ex.Message;
            logger.Error("[{0}] | {1} | Exception: {2} - InnerException: {3}.", ruta, datos, ex.ToString(), ex.InnerException.ToString() ?? string.Empty);
        }
    }
}
