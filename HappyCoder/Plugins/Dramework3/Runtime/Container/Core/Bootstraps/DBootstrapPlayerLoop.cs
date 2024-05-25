using IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs;
using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using UnityEngine.LowLevel;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Bootstraps
{
    public static partial class DBootstrap
    {
        #region ================================ METHODS

        private static void ModifyPlayerLoop()
        {
            RemoveSubsystemsFromPlayerLoop();
        }

        private static void RemoveSubsystemsFromPlayerLoop()
        {
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            foreach (var playerLoopConfig in DProjectConfig.Instance.PlayerLoopConfigs)
            {
                foreach (var subsystem in playerLoopConfig.SubsystemsByTypeFullNames)
                {
                    if (subsystem.Value) continue;
                    Helpers.UnityPlayerLoop.TryRemoveTypeFrom(ref playerLoop, subsystem.Key);
                }

                foreach (var subsystem in playerLoopConfig.SubsystemsByTypes)
                {
                    if (subsystem.Value) continue;
                    Helpers.UnityPlayerLoop.TryRemoveTypeFrom(ref playerLoop, subsystem.Key);
                }
            }

            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        #endregion
    }
}