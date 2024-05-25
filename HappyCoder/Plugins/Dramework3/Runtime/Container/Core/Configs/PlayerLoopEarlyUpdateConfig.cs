using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.PlayerLoop;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopEarlyUpdateConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Early Update")]
        [LabelText("Early Update")]
        [SerializeField] [ToggleLeft]
        private bool _earlyUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Poll Player Connection")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePollPlayerConnection = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Gpu Timestamp")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateGpuTimestamp = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Analytics Core Stats Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateAnalyticsCoreStatsUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Unity Web Request Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUnityWebRequestUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Execute Main Thread Jobs")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateExecuteMainThreadJobs = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Process Mouse In Window")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateProcessMouseInWindow = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Clear Intermediate Renderers")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateClearIntermediateRenderers = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Clear Lines")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateClearLines = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Present Before Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePresentBeforeUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Reset Frame Stats After Present")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateResetFrameStatsAfterPresent = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Async Instantiate (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateAsyncInstantiate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Async Readback Manager")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateAsyncReadbackManager = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Streaming Manager")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateStreamingManager = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Texture Streaming Manager")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateTextureStreamingManager = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Preloading")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdatePreloading = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Content Loading")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateContentLoading = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Renderer Notify Invisible")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateRendererNotifyInvisible = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Player Cleanup Cached Data")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePlayerCleanupCachedData = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Main Game View Rect")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateMainGameViewRect = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Canvas RectTransform")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateCanvasRectTransform = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("XR Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateXRUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Input Manager")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateInputManager = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Process Remote Input")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateProcessRemoteInput = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Script Run Delayed Startup Frame")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateScriptRunDelayedStartupFrame = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Update Kinect")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateUpdateKinect = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Deliver Ios Platform Events")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateDeliverIosPlatformEvents = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("AR Core Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateARCoreUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Dispatch Event Queue Events")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateDispatchEventQueueEvents = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Physics2D Early Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePhysics2DEarlyUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Physics Reset Interpolated Transform Position (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePhysicsResetInterpolatedTransformPosition = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Sprite Atlas Manager Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdateSpriteAtlasManagerUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Early Update")]
        [LabelText("Performance Analytics Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_earlyUpdate")]
        private bool _earlyUpdatePerformanceAnalyticsUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames => new Dictionary<string, bool>();
        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(EarlyUpdate), _earlyUpdate },
                { typeof(EarlyUpdate.PollPlayerConnection), _earlyUpdatePollPlayerConnection },
                { typeof(EarlyUpdate.GpuTimestamp), _earlyUpdateGpuTimestamp },
                { typeof(EarlyUpdate.AnalyticsCoreStatsUpdate), _earlyUpdateAnalyticsCoreStatsUpdate },
                { typeof(EarlyUpdate.UnityWebRequestUpdate), _earlyUpdateUnityWebRequestUpdate },
                { typeof(EarlyUpdate.ExecuteMainThreadJobs), _earlyUpdateExecuteMainThreadJobs },
                { typeof(EarlyUpdate.ProcessMouseInWindow), _earlyUpdateProcessMouseInWindow },
                { typeof(EarlyUpdate.ClearIntermediateRenderers), _earlyUpdateClearIntermediateRenderers },
                { typeof(EarlyUpdate.ClearLines), _earlyUpdateClearLines },
                { typeof(EarlyUpdate.PresentBeforeUpdate), _earlyUpdatePresentBeforeUpdate },
                { typeof(EarlyUpdate.ResetFrameStatsAfterPresent), _earlyUpdateResetFrameStatsAfterPresent },
                { typeof(EarlyUpdate.UpdateAsyncInstantiate), _earlyUpdateUpdateAsyncInstantiate },
                { typeof(EarlyUpdate.UpdateAsyncReadbackManager), _earlyUpdateUpdateAsyncReadbackManager },
                { typeof(EarlyUpdate.UpdateStreamingManager), _earlyUpdateUpdateStreamingManager },
                { typeof(EarlyUpdate.UpdateTextureStreamingManager), _earlyUpdateUpdateTextureStreamingManager },
                { typeof(EarlyUpdate.UpdatePreloading), _earlyUpdateUpdatePreloading },
                { typeof(EarlyUpdate.UpdateContentLoading), _earlyUpdateUpdateContentLoading },
                { typeof(EarlyUpdate.RendererNotifyInvisible), _earlyUpdateRendererNotifyInvisible },
                { typeof(EarlyUpdate.PlayerCleanupCachedData), _earlyUpdatePlayerCleanupCachedData },
                { typeof(EarlyUpdate.UpdateMainGameViewRect), _earlyUpdateUpdateMainGameViewRect },
                { typeof(EarlyUpdate.UpdateCanvasRectTransform), _earlyUpdateUpdateCanvasRectTransform },
                { typeof(EarlyUpdate.XRUpdate), _earlyUpdateXRUpdate },
                { typeof(EarlyUpdate.UpdateInputManager), _earlyUpdateUpdateInputManager },
                { typeof(EarlyUpdate.ProcessRemoteInput), _earlyUpdateProcessRemoteInput },
                { typeof(EarlyUpdate.ScriptRunDelayedStartupFrame), _earlyUpdateScriptRunDelayedStartupFrame },
                { typeof(EarlyUpdate.UpdateKinect), _earlyUpdateUpdateKinect },
                { typeof(EarlyUpdate.DeliverIosPlatformEvents), _earlyUpdateDeliverIosPlatformEvents },
                { typeof(EarlyUpdate.ARCoreUpdate), _earlyUpdateARCoreUpdate },
                { typeof(EarlyUpdate.DispatchEventQueueEvents), _earlyUpdateDispatchEventQueueEvents },
                { typeof(EarlyUpdate.Physics2DEarlyUpdate), _earlyUpdatePhysics2DEarlyUpdate },
                { typeof(EarlyUpdate.PhysicsResetInterpolatedTransformPosition), _earlyUpdatePhysicsResetInterpolatedTransformPosition },
                { typeof(EarlyUpdate.SpriteAtlasManagerUpdate), _earlyUpdateSpriteAtlasManagerUpdate },
                { typeof(EarlyUpdate.PerformanceAnalyticsUpdate), _earlyUpdatePerformanceAnalyticsUpdate }
            };

        #endregion
    }
}