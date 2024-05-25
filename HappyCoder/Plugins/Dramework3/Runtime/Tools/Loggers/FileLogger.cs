using System;
using System.IO;
using System.Threading.Tasks;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Loggers
{
    internal static class FileLogger
    {
        #region ================================ FIELDS

        private static string _file;
        private static string Directory;
        private static string Path;
        private static bool _saving;

        #endregion

        #region ================================ METHODS

#if DRAMEWORK_LOG && LOG_TO_FILE_ENABLED
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
        private static void Initialize()
        {
            _file = string.Empty;
            Directory = System.IO.Path.Combine(Application.persistentDataPath, "Logs");
            Path = System.IO.Path.Combine(Directory, $"{DateTime.Now:yyyy_MM_dd_hh_mm_ss}.log");
            if (System.IO.Directory.Exists(Directory) == false)
                System.IO.Directory.CreateDirectory(Directory);

            Application.logMessageReceived += OnLogMessageReceived;
        }

        private static void OnLogMessageReceived(string condition, string stacktrace, LogType type)
        {
            _file = $"{_file}[{type}] {condition}\n{stacktrace}\n\n";
            Save();
        }

        private static async void Save()
        {
            var text = string.Copy(_file);
            while (_saving) await Task.Yield();
            _saving = true;
            await File.WriteAllTextAsync(Path, text);
            _saving = false;
        }

        #endregion
    }
}