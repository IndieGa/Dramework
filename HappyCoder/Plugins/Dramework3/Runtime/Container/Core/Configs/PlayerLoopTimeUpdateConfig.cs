using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopTimeUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Time Update")]
        [LabelText("Time Update")]
        [SerializeField] [ToggleLeft]
        private bool _timeUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Time Update")]
        [LabelText("Wait For Last Presentation And Update Time")]
        [SerializeField] [ToggleLeft] [ShowIf("_timeUpdate")]
        private bool _timeUpdateWaitForLastPresentationAndUpdateTime = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames => new Dictionary<string, bool>();
        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(TimeUpdate), _timeUpdate },
                { typeof(TimeUpdate.WaitForLastPresentationAndUpdateTime), _timeUpdateWaitForLastPresentationAndUpdateTime }
            };

        #endregion
    }
}