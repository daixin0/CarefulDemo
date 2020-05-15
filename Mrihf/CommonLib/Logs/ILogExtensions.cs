using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Logs
{
    public static class ILogExtensions
    {
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Log<T>(this ILog logger, string message)
        {
            logger?.Log($"[{typeof(T).Name}] {message}", LogLevel.Debug);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        public static void Log(this ILog logger, string message)
        {
            logger.Log(message, LogLevel.Info);
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="logger">ILog</param>
        /// <param name="message">要输出的信息</param>
        /// <remarks>要向日志文件输出，则需要在 app.config 中修改 ClientLogLevel 的值为 1</remarks>
        public static void LogDebug(this ILog logger, string message)
        {
            logger?.Log($"{message}", LogLevel.Debug);
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="logger">ILog</param>
        /// <param name="message">要输出的信息</param>
        /// <remarks>要向日志文件输出，则需要在 app.config 中修改 ClientLogLevel 的值为 1</remarks>        
        public static void LogDebug<T>(this ILog logger, string message)
        {
            logger?.Log($"[{typeof(T).Name}] {message}", LogLevel.Debug);
        }

        public static void LogError(this ILog logger, Exception ex)
        {
            logger?.LogError(ex.ToString());
        }
    }
}
