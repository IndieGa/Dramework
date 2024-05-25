using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Update")]
        [LabelText("Update")]
        [SerializeField] [ToggleLeft]
        private bool _update = true;

        [Indent(2)]
        [FoldoutGroup("Update")]
        [LabelText("Script Run Behaviour Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_update")]
        private bool _updateScriptRunBehaviourUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Update")]
        [LabelText("Script Run Delayed Dynamic Frame Rate")]
        [SerializeField] [ToggleLeft] [ShowIf("_update")]
        private bool _updateScriptRunDelayedDynamicFrameRate = true;

        [Indent(2)]
        [FoldoutGroup("Update")]
        [LabelText("Script Run Delayed Tasks")]
        [SerializeField] [ToggleLeft] [ShowIf("_update")]
        private bool _updateScriptRunDelayedTasks = true;

        [Indent(2)]
        [FoldoutGroup("Update")]
        [LabelText("Director Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_update")]
        private bool _updateDirectorUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames => new Dictionary<string, bool>();
        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(Update), _update },
                { typeof(Update.ScriptRunBehaviourUpdate), _updateScriptRunBehaviourUpdate },
                { typeof(Update.ScriptRunDelayedDynamicFrameRate), _updateScriptRunDelayedDynamicFrameRate },
                { typeof(Update.ScriptRunDelayedTasks), _updateScriptRunDelayedTasks },
                { typeof(Update.DirectorUpdate), _updateDirectorUpdate }
            };

        #endregion
    }
}