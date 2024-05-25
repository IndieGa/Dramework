using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class VersionTools
        {
            #region ================================ METHODS

            public static string IncrementBuild(string version)
            {
                var parts = version.Split('.');
                if (parts.Length < 3)
                {
                    Debug.LogError("Incorrect version format");
                    return version;
                }
                if (int.TryParse(parts[2], out var build) == false) return version;
                build++;
                return $"{parts[0]}.{parts[1]}.{build}";
            }

            public static string IncrementMajor(string version)
            {
                var parts = version.Split('.');
                if (parts.Length < 3)
                {
                    Debug.LogError("Incorrect version format");
                    return version;
                }
                if (int.TryParse(parts[0], out var major) == false) return version;
                major++;
                return $"{major}.0.0";
            }

            public static string IncrementMinor(string version)
            {
                var parts = version.Split('.');
                if (parts.Length < 3)
                {
                    Debug.LogError("Incorrect version format");
                    return version;
                }
                if (int.TryParse(parts[1], out var minor) == false) return version;
                minor++;
                return $"{parts[0]}.{minor}.0";
            }

            #endregion
        }

        #endregion
    }
}