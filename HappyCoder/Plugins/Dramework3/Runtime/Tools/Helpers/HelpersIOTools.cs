using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class IOTools
        {
            #region ================================ METHODS

            public static void AddFileAttributes(string path, FileAttributes attributes)
            {
                File.SetAttributes(path, File.GetAttributes(path) | attributes);
            }

            public static void ClearDirectory(string path, IReadOnlyCollection<string> excludedFiles = null, IReadOnlyCollection<string> excludedDirectories = null)
            {
                if (Directory.Exists(path) == false) return;

                var directory = new DirectoryInfo(path);

                foreach (var file in directory.GetFiles())
                {
                    if (excludedFiles != null && excludedFiles.Any(f => file.Name.Contains(f))) continue;
                    file.Delete();
                }

                foreach (var subdirectory in directory.GetDirectories())
                {
                    if (excludedDirectories != null && excludedDirectories.Any(d => subdirectory.Name.Contains(d))) continue;
                    ClearDirectory(subdirectory.FullName);
                    subdirectory.Delete();
                }
            }

            public static string GetRelativePath(string absolutePath)
            {
                return absolutePath.Remove(0, absolutePath.IndexOf("Assets", StringComparison.Ordinal));
            }

            public static void RemoveFileAttributes(string path, FileAttributes attributesToRemove)
            {
                File.SetAttributes(path, File.GetAttributes(path) & ~attributesToRemove);
            }

            #endregion
        }

        #endregion
    }
}