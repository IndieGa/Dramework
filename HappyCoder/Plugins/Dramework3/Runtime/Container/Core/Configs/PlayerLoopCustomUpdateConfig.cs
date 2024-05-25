using System;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopCustomUpdateConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Timer Update")]
        [SerializeField] [ToggleLeft]
        private bool _timerUpdate = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Early Update")]
        [SerializeField] [ToggleLeft]
        private bool _earlyUpdate = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Fixed Update")]
        [SerializeField] [ToggleLeft]
        private bool _fixedUpdate = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Pre Update")]
        [SerializeField] [ToggleLeft]
        private bool _preUpdate = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Update")]
        [SerializeField] [ToggleLeft]
        private bool _update = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Pre Late Update")]
        [SerializeField] [ToggleLeft]
        private bool _preLateUpdate = true;

        [Indent]
        [FoldoutGroup("Custom Update")]
        [LabelText("Post Late Update")]
        [SerializeField] [ToggleLeft]
        private bool _postLateUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool EarlyUpdate => _earlyUpdate;
        internal bool FixedUpdate => _fixedUpdate;
        internal bool PostLateUpdate => _postLateUpdate;
        internal bool PreLateUpdate => _preLateUpdate;
        internal bool PreUpdate => _preUpdate;
        internal bool TimerUpdate => _timerUpdate;
        internal bool Update => _update;

        #endregion
    }
}