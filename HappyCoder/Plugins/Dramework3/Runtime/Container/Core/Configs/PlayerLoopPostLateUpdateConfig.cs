using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopPostLateUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Post Late Update")]
        [SerializeField] [ToggleLeft]
        private bool _postLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Send Frame Started")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerSendFrameStarted = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Director Late Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateDirectorLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Script Run Delayed Dynamic Frame Rate")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateScriptRunDelayedDynamicFrameRate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Physics Skinned Cloth Begin Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePhysicsSkinnedClothBeginUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update RectTransform")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateRectTransform = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Update Canvases")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerUpdateCanvases = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Audio")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateAudio = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("VFX Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateVFXUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Particle System End Update All (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateParticleSystemEndUpdateAll = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("End Graphics Jobs After Script Late Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateEndGraphicsJobsAfterScriptLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Custom Render Textures")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateCustomRenderTextures = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("XR Post Late Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateXRPostLateUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update All Renderers")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateAllRenderers = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Light Probe Proxy Volumes")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateLightProbeProxyVolumes = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Enlighten Runtime Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateEnlightenRuntimeUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update All Skinned Meshes")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateAllSkinnedMeshes = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Process Web Send Messages")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateProcessWebSendMessages = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Sorting Groups Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateSortingGroupsUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Video Textures (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateVideoTextures = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Video (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateVideo = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Director Render Image (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateDirectorRenderImage = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Emit Canvas Geometry")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerEmitCanvasGeometry = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Render UIE Batch Mode Offscreen")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerRenderUIEBatchModeOffscreen = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Physics Skinned Cloth Finish Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePhysicsSkinnedClothFinishUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Finish Frame Rendering")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateFinishFrameRendering = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Batch Mode Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateBatchModeUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Send Frame Complete")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerSendFrameComplete = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Capture Screenshot")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateCaptureScreenshot = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Present After Draw")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePresentAfterDraw = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Clear Immediate Renderers")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateClearImmediateRenderers = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Player Send Frame Post Present")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdatePlayerSendFramePostPresent = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Update Resolution")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateUpdateResolution = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Input End Frame")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateInputEndFrame = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Trigger End Of Frame Callbacks")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateTriggerEndOfFrameCallbacks = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("GUI Clear Events")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateGUIClearEvents = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Shader Handle Errors")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateShaderHandleErrors = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Reset Input Axis")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateResetInputAxis = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Threaded Loading Debug")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateThreadedLoadingDebug = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Profiler Synchronize Stats")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateProfilerSynchronizeStats = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Memory Frame Maintenance")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateMemoryFrameMaintenance = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Execute Game Center Callbacks")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateExecuteGameCenterCallbacks = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("XR Pre End Frame (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateXRPreEndFrame = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Profiler End Frame")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateProfilerEndFrame = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Graphics Warmup Preloaded Shaders")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateGraphicsWarmupPreloadedShaders = true;

        [Indent(2)]
        [FoldoutGroup("Post Late Update")]
        [LabelText("Object Dispatcher Post Late Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_postLateUpdate")]
        private bool _postLateUpdateObjectDispatcherPostLateUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames =>
            new Dictionary<string, bool>
            {
                { "UnityEngine.PlayerLoop.PostLateUpdate+PlayerRenderUIEBatchModeOffscreen", _postLateUpdatePlayerRenderUIEBatchModeOffscreen }
            };

        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(PostLateUpdate), _postLateUpdate },
                { typeof(PostLateUpdate.PlayerSendFrameStarted), _postLateUpdatePlayerSendFrameStarted },
                { typeof(PostLateUpdate.DirectorLateUpdate), _postLateUpdateDirectorLateUpdate },
                { typeof(PostLateUpdate.ScriptRunDelayedDynamicFrameRate), _postLateUpdateScriptRunDelayedDynamicFrameRate },
                { typeof(PostLateUpdate.PhysicsSkinnedClothBeginUpdate), _postLateUpdatePhysicsSkinnedClothBeginUpdate },
                { typeof(PostLateUpdate.UpdateRectTransform), _postLateUpdateUpdateRectTransform },
                { typeof(PostLateUpdate.PlayerUpdateCanvases), _postLateUpdatePlayerUpdateCanvases },
                { typeof(PostLateUpdate.UpdateAudio), _postLateUpdateUpdateAudio },
                { typeof(PostLateUpdate.VFXUpdate), _postLateUpdateVFXUpdate },
                { typeof(PostLateUpdate.ParticleSystemEndUpdateAll), _postLateUpdateParticleSystemEndUpdateAll },
                { typeof(PostLateUpdate.EndGraphicsJobsAfterScriptLateUpdate), _postLateUpdateEndGraphicsJobsAfterScriptLateUpdate },
                { typeof(PostLateUpdate.UpdateCustomRenderTextures), _postLateUpdateUpdateCustomRenderTextures },
                { typeof(PostLateUpdate.XRPostLateUpdate), _postLateUpdateXRPostLateUpdate },
                { typeof(PostLateUpdate.UpdateAllRenderers), _postLateUpdateUpdateAllRenderers },
                { typeof(PostLateUpdate.UpdateLightProbeProxyVolumes), _postLateUpdateUpdateLightProbeProxyVolumes },
                { typeof(PostLateUpdate.EnlightenRuntimeUpdate), _postLateUpdateEnlightenRuntimeUpdate },
                { typeof(PostLateUpdate.UpdateAllSkinnedMeshes), _postLateUpdateUpdateAllSkinnedMeshes },
                { typeof(PostLateUpdate.ProcessWebSendMessages), _postLateUpdateProcessWebSendMessages },
                { typeof(PostLateUpdate.SortingGroupsUpdate), _postLateUpdateSortingGroupsUpdate },
                { typeof(PostLateUpdate.UpdateVideoTextures), _postLateUpdateUpdateVideoTextures },
                { typeof(PostLateUpdate.UpdateVideo), _postLateUpdateUpdateVideo },
                { typeof(PostLateUpdate.DirectorRenderImage), _postLateUpdateDirectorRenderImage },
                { typeof(PostLateUpdate.PlayerEmitCanvasGeometry), _postLateUpdatePlayerEmitCanvasGeometry },
                { typeof(PostLateUpdate.PhysicsSkinnedClothFinishUpdate), _postLateUpdatePhysicsSkinnedClothFinishUpdate },
                { typeof(PostLateUpdate.FinishFrameRendering), _postLateUpdateFinishFrameRendering },
                { typeof(PostLateUpdate.BatchModeUpdate), _postLateUpdateBatchModeUpdate },
                { typeof(PostLateUpdate.PlayerSendFrameComplete), _postLateUpdatePlayerSendFrameComplete },
                { typeof(PostLateUpdate.UpdateCaptureScreenshot), _postLateUpdateUpdateCaptureScreenshot },
                { typeof(PostLateUpdate.PresentAfterDraw), _postLateUpdatePresentAfterDraw },
                { typeof(PostLateUpdate.ClearImmediateRenderers), _postLateUpdateClearImmediateRenderers },
                { typeof(PostLateUpdate.PlayerSendFramePostPresent), _postLateUpdatePlayerSendFramePostPresent },
                { typeof(PostLateUpdate.UpdateResolution), _postLateUpdateUpdateResolution },
                { typeof(PostLateUpdate.InputEndFrame), _postLateUpdateInputEndFrame },
                { typeof(PostLateUpdate.TriggerEndOfFrameCallbacks), _postLateUpdateTriggerEndOfFrameCallbacks },
                { typeof(PostLateUpdate.GUIClearEvents), _postLateUpdateGUIClearEvents },
                { typeof(PostLateUpdate.ShaderHandleErrors), _postLateUpdateShaderHandleErrors },
                { typeof(PostLateUpdate.ResetInputAxis), _postLateUpdateResetInputAxis },
                { typeof(PostLateUpdate.ThreadedLoadingDebug), _postLateUpdateThreadedLoadingDebug },
                { typeof(PostLateUpdate.ProfilerSynchronizeStats), _postLateUpdateProfilerSynchronizeStats },
                { typeof(PostLateUpdate.MemoryFrameMaintenance), _postLateUpdateMemoryFrameMaintenance },
                { typeof(PostLateUpdate.ExecuteGameCenterCallbacks), _postLateUpdateExecuteGameCenterCallbacks },
                { typeof(PostLateUpdate.XRPreEndFrame), _postLateUpdateXRPreEndFrame },
                { typeof(PostLateUpdate.ProfilerEndFrame), _postLateUpdateProfilerEndFrame },
                { typeof(PostLateUpdate.GraphicsWarmupPreloadedShaders), _postLateUpdateGraphicsWarmupPreloadedShaders },
                { typeof(PostLateUpdate.ObjectDispatcherPostLateUpdate), _postLateUpdateObjectDispatcherPostLateUpdate }
            };

        #endregion
    }
}