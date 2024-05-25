using System;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Loggers
{
    internal static class LoggerTools
    {
        #region ================================ METHODS

        internal static string FormatMessage(string message, string prefix = "")
        {
            var timestamp = $"[{DateTime.Now:hh:mm:ss ffff}] ";
            var frameNumber = $"[Frame:{Time.frameCount}] ";
            return $"{timestamp}{frameNumber}{prefix} - {message}";
        }

        #endregion
    }
}