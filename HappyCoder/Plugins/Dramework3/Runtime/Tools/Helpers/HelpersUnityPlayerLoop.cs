using System;
using System.IO;
using System.Linq;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using UnityEngine.LowLevel;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class UnityPlayerLoop
        {
            #region ================================ FIELDS

            private static string _file;

            #endregion

            #region ================================ METHODS

            public static void AddSystem<T>(PlayerLoopSystem system) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    if (subSystems.All(s => s.type != system.type))
                        subSystems.Add(system);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static void InsertSystem<T>(int index, PlayerLoopSystem system) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    if (subSystems.All(s => s.type != system.type))
                        subSystems.Insert(index, system);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static void LogCurrentPlayerLoop(string path)
            {
                _file = string.Empty;
                var rootSystem = PlayerLoop.GetCurrentPlayerLoop();
                LogSubSystems(rootSystem, "");
                File.WriteAllText(path, _file);
            }

            public static void LogDefaultPlayerLoop(string path)
            {
                _file = string.Empty;
                var rootSystem = PlayerLoop.GetDefaultPlayerLoop();
                LogSubSystems(rootSystem, "");
                File.WriteAllText(path, _file);
            }

            public static void RemoveSystem<T>(Type systemType) where T : struct
            {
                var current = PlayerLoop.GetCurrentPlayerLoop();

                for (var i = 0; i < current.subSystemList.Length; i++)
                {
                    if (current.subSystemList[i].type != typeof(T)) continue;

                    var subSystems = current.subSystemList[i].subSystemList.ToList();
                    subSystems.RemoveAll(s => s.type == systemType);
                    current.subSystemList[i].subSystemList = subSystems.ToArray();
                    PlayerLoop.SetPlayerLoop(current);
                    return;
                }
            }

            public static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, Type type)
            {
                var subSystems = currentSystem.subSystemList;
                if (subSystems == null)
                {
                    return false;
                }

                for (var i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].type == type)
                    {
                        var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];

                        Array.Copy(subSystems, newSubSystems, i);
                        Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);

                        currentSystem.subSystemList = newSubSystems;

                        ConsoleLogger.Log($"Subsystem - «{type}» was removed from player loop");

                        return true;
                    }

                    if (TryRemoveTypeFrom(ref subSystems[i], type))
                    {
                        return true;
                    }
                }

                return false;
            }

            public static bool TryRemoveTypeFrom(ref PlayerLoopSystem currentSystem, string typeFullName)
            {
                var subSystems = currentSystem.subSystemList;
                if (subSystems == null)
                {
                    return false;
                }

                for (var i = 0; i < subSystems.Length; i++)
                {
                    if (subSystems[i].type.FullName == typeFullName)
                    {
                        var newSubSystems = new PlayerLoopSystem[subSystems.Length - 1];

                        Array.Copy(subSystems, newSubSystems, i);
                        Array.Copy(subSystems, i + 1, newSubSystems, i, subSystems.Length - i - 1);

                        currentSystem.subSystemList = newSubSystems;

                        ConsoleLogger.Log($"Subsystem - «{typeFullName}» was removed from player loop");

                        return true;
                    }

                    if (TryRemoveTypeFrom(ref subSystems[i], typeFullName))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static void LogSubSystems(PlayerLoopSystem playerLoopSystem, string indent)
            {
                _file = $"{_file}{indent}{playerLoopSystem.type}\n".Replace("UnityEngine.PlayerLoop.", "");
                if (playerLoopSystem.subSystemList is { Length: > 0 })
                {
                    indent = $"    {indent}";
                    foreach (var subSystem in playerLoopSystem.subSystemList)
                    {
                        LogSubSystems(subSystem, indent);
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}