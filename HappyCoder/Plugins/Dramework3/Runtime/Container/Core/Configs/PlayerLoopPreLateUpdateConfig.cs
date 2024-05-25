using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopPreLateUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Pre Late Update")]
        [SerializeField] [ToggleLeft]
        private bool _preLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("AI Update Post Script (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateAIUpdatePostScript = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Director Update Animation Begin (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateDirectorUpdateAnimationBegin = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Legacy Animation Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateLegacyAnimationUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Director Update Animation End (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateDirectorUpdateAnimationEnd = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Director Deferred Evaluate (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateDirectorDeferredEvaluate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("UI Elements Update Panels")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateUIElementsUpdatePanels = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("End Graphics Jobs After Script Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateEndGraphicsJobsAfterScriptUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Constraint Manager Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateConstraintManagerUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Particle System Begin Update All (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateParticleSystemBeginUpdateAll = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Physics2D Late Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdatePhysics2DLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Physics Late Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdatePhysicsLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Late Update")]
        [LabelText("Script Run Behaviour Late Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_preLateUpdate")]
        private bool _preLateUpdateScriptRunBehaviourLateUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames => new Dictionary<string, bool>();
        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(PreLateUpdate), _preLateUpdate },
                { typeof(PreLateUpdate.AIUpdatePostScript), _preLateUpdateAIUpdatePostScript },
                { typeof(PreLateUpdate.DirectorUpdateAnimationBegin), _preLateUpdateDirectorUpdateAnimationBegin },
                { typeof(PreLateUpdate.LegacyAnimationUpdate), _preLateUpdateLegacyAnimationUpdate },
                { typeof(PreLateUpdate.DirectorUpdateAnimationEnd), _preLateUpdateDirectorUpdateAnimationEnd },
                { typeof(PreLateUpdate.DirectorDeferredEvaluate), _preLateUpdateDirectorDeferredEvaluate },
                { typeof(PreLateUpdate.UIElementsUpdatePanels), _preLateUpdateUIElementsUpdatePanels },
                { typeof(PreLateUpdate.EndGraphicsJobsAfterScriptUpdate), _preLateUpdateEndGraphicsJobsAfterScriptUpdate },
                { typeof(PreLateUpdate.ConstraintManagerUpdate), _preLateUpdateConstraintManagerUpdate },
                { typeof(PreLateUpdate.ParticleSystemBeginUpdateAll), _preLateUpdateParticleSystemBeginUpdateAll },
                { typeof(PreLateUpdate.Physics2DLateUpdate), _preLateUpdatePhysics2DLateUpdate },
                { typeof(PreLateUpdate.PhysicsLateUpdate), _preLateUpdatePhysicsLateUpdate },
                { typeof(PreLateUpdate.ScriptRunBehaviourLateUpdate), _preLateUpdateScriptRunBehaviourLateUpdate }
            };

        #endregion
    }
}