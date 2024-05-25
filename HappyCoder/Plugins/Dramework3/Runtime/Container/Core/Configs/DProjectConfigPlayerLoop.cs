using System.Collections.Generic;

using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.LowLevel;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    internal sealed partial class DProjectConfig
    {
        #region ================================ FIELDS

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop", 100)]
        [HideLabel]
        [SerializeField]
        private PlayerLoopCustomUpdateConfig _playerLoopCustomUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopTimeUpdateConfig _playerLoopTimeUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopInitializationConfig _playerLoopInitializationConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopEarlyUpdateConfig _playerLoopEarlyUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopFixedUpdateConfig _playerLoopFixedUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopPreUpdateConfig _playerLoopPreUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopUpdateConfig _playerLoopUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopPreLateUpdateConfig _playerLoopPreLateUpdateConfig;

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HideLabel]
        [SerializeField]
        private PlayerLoopPostLateUpdateConfig _playerLoopPostLateUpdateConfig;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal List<IPlayerLoopConfig> PlayerLoopConfigs =>
            new List<IPlayerLoopConfig>
            {
                _playerLoopTimeUpdateConfig,
                _playerLoopInitializationConfig,
                _playerLoopEarlyUpdateConfig,
                _playerLoopFixedUpdateConfig,
                _playerLoopPreUpdateConfig,
                _playerLoopUpdateConfig,
                _playerLoopPreLateUpdateConfig,
                _playerLoopPostLateUpdateConfig
            };

        internal PlayerLoopCustomUpdateConfig PlayerLoopCustomUpdateConfig => _playerLoopCustomUpdateConfig;

        #endregion

        #region ================================ METHODS

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HorizontalGroup("Settings/Box/Player Loop/Horizontal")]
        [Button("Log Current Player Loop", ButtonSizes.Medium)]
        private void LogCurrentPlayerLoop()
        {
            Helpers.UnityPlayerLoop.LogCurrentPlayerLoop($"{Application.dataPath}/current_player_loop.txt");
        }

        [FoldoutGroup("Settings")] [BoxGroup("Settings/Box", false)] [FoldoutGroup("Settings/Box/Player Loop")]
        [HorizontalGroup("Settings/Box/Player Loop/Horizontal")]
        [Button("Set Default Player Loop", ButtonSizes.Medium)]
        private void SetDefaultPlayerLoop()
        {
            PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
        }

        #endregion
    }
}