using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopPreUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Pre Update")]
        [LabelText("Pre Update")]
        [SerializeField] [ToggleLeft]
        private bool _preUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Physics Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdatePhysicsUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Physics2D Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdatePhysics2DUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Physics Cloth Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdatePhysicsClothUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Check Tex Field Input")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateCheckTexFieldInput = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("IMGUI Send Queued Events")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateIMGUISendQueuedEvents = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("New Input Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateNewInputUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Send Mouse Events")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateSendMouseEvents = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("AI Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateAIUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Wind Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateWindUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Pre Update")]
        [LabelText("Update Video (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_preUpdate")]
        private bool _preUpdateUpdateVideo = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames =>
            new Dictionary<string, bool>
            {
                { "UnityEngine.PlayerLoop.PreUpdate+PhysicsClothUpdate", _preUpdatePhysicsClothUpdate }
            };

        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(PreUpdate), _preUpdate },
                { typeof(PreUpdate.PhysicsUpdate), _preUpdatePhysicsUpdate },
                { typeof(PreUpdate.Physics2DUpdate), _preUpdatePhysics2DUpdate },
                { typeof(PreUpdate.CheckTexFieldInput), _preUpdateCheckTexFieldInput },
                { typeof(PreUpdate.IMGUISendQueuedEvents), _preUpdateIMGUISendQueuedEvents },
                { typeof(PreUpdate.NewInputUpdate), _preUpdateNewInputUpdate },
                { typeof(PreUpdate.SendMouseEvents), _preUpdateSendMouseEvents },
                { typeof(PreUpdate.AIUpdate), _preUpdateAIUpdate },
                { typeof(PreUpdate.WindUpdate), _preUpdateWindUpdate },
                { typeof(PreUpdate.UpdateVideo), _preUpdateUpdateVideo }
            };

        #endregion
    }
}