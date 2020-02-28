using NLog;
using System;

namespace ZREL.ZiPago.Sitio.Web.Utility
{
    public enum ELogLevel
    {
        DEBUG = 1,
        ERROR = 2,
        FATAL = 3,
        INFO = 4,
        WARN = 5
    }

    public static class Log
    {

        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public static void WriteLog(ELogLevel logLevel, String log)
        {
            switch (logLevel)
            {
                case ELogLevel.DEBUG:
                default:
                    if (logger.IsDebugEnabled) logger.Debug(log);
                    break;
                case ELogLevel.ERROR:
                    if (logger.IsErrorEnabled | logger.IsInfoEnabled) logger.Error(log);
                    break;
                case ELogLevel.FATAL:
                    if (logger.IsFatalEnabled) logger.Fatal(log);
                    break;
                case ELogLevel.INFO:
                    if (logger.IsInfoEnabled) logger.Info(log);
                    break;
                case ELogLevel.WARN:
                    if (logger.IsWarnEnabled) logger.Warn(log);
                    break;
            }
        }

        public static void InvokeAppendLog(string modulo, string msg)
        {
            Log.WriteLog(ELogLevel.DEBUG, "[" + modulo + "] " + msg);
        }

        public static void InvokeAppendLogError(string modulo, string msg)
        {
            Log.WriteLog(ELogLevel.ERROR, "[" + modulo + "] " + msg);
        }

        public static void InvokeAppendLogInfo(string modulo, string msg)
        {
            Log.WriteLog(ELogLevel.INFO, "[" + modulo + "] " + msg);
        }

    }
}
