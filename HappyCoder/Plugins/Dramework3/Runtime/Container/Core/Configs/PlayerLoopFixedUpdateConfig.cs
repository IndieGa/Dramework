using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopFixedUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Fixed Update")]
        [SerializeField] [ToggleLeft]
        private bool _fixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Clear Lines")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateClearLines = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("New Input Fixed Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateNewInputFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Director Fixed Sample Time (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateDirectorFixedSampleTime = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Audio Fixed Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateAudioFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Script Run Behaviour Fixed Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateScriptRunBehaviourFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Director Fixed Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateDirectorFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Legacy Fixed Animation Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateLegacyFixedAnimationUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("XR Fixed Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateXRFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Physics Fixed Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdatePhysicsFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Physics2D Fixed Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdatePhysics2DFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Physics Cloth Fixed Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdatePhysicsClothFixedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Director Fixed Update Post Physics (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateDirectorFixedUpdatePostPhysics = true;

        [Indent(2)]
        [FoldoutGroup("Fixed Update")]
        [LabelText("Script Run Delayed Fixed Frame Rate")]
        [SerializeField] [ToggleLeft] [ShowIf("_fixedUpdate")]
        private bool _fixedUpdateScriptRunDelayedFixedFrameRate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames =>
            new Dictionary<string, bool>
            {
                { "UnityEngine.PlayerLoop.FixedUpdate+PhysicsClothFixedUpdate", _fixedUpdatePhysicsClothFixedUpdate }
            };

        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(FixedUpdate), _fixedUpdate },
                { typeof(FixedUpdate.ClearLines), _fixedUpdateClearLines },
                { typeof(FixedUpdate.NewInputFixedUpdate), _fixedUpdateNewInputFixedUpdate },
                { typeof(FixedUpdate.DirectorFixedSampleTime), _fixedUpdateDirectorFixedSampleTime },
                { typeof(FixedUpdate.AudioFixedUpdate), _fixedUpdateAudioFixedUpdate },
                { typeof(FixedUpdate.ScriptRunBehaviourFixedUpdate), _fixedUpdateScriptRunBehaviourFixedUpdate },
                { typeof(FixedUpdate.DirectorFixedUpdate), _fixedUpdateDirectorFixedUpdate },
                { typeof(FixedUpdate.LegacyFixedAnimationUpdate), _fixedUpdateLegacyFixedAnimationUpdate },
                { typeof(FixedUpdate.XRFixedUpdate), _fixedUpdateXRFixedUpdate },
                { typeof(FixedUpdate.PhysicsFixedUpdate), _fixedUpdatePhysicsFixedUpdate },
                { typeof(FixedUpdate.Physics2DFixedUpdate), _fixedUpdatePhysics2DFixedUpdate },
                { typeof(FixedUpdate.DirectorFixedUpdatePostPhysics), _fixedUpdateDirectorFixedUpdatePostPhysics },
                { typeof(FixedUpdate.ScriptRunDelayedFixedFrameRate), _fixedUpdateScriptRunDelayedFixedFrameRate }
            };

        #endregion
    }
}