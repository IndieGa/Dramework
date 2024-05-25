using System.Diagnostics.CodeAnalysis;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Loggers
{
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    public static class ConsoleLogger
    {
        #region ================================ METHODS

        public static void Log(string message, string prefix = "", Object sender = null)
        {
#if DRAMEWORK_LOG
            message = LoggerTools.FormatMessage(message, prefix);
            Debug.Log(message, sender);
#endif
        }

        public static void LogError(string message, string prefix = "", Object sender = null)
        {
#if DRAMEWORK_LOG
            message = LoggerTools.FormatMessage(message, prefix);
            Debug.LogError(message, sender);
#endif
        }

        public static void LogWarning(string message, string prefix = "", Object sender = null)
        {
#if DRAMEWORK_LOG
            message = LoggerTools.FormatMessage(message, prefix);
            Debug.LogWarning(message, sender);
#endif
        }

        #endregion
    }
}