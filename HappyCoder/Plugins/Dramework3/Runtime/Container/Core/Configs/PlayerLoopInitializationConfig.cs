using System;
using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core.Configs
{
    [Serializable]
    internal class PlayerLoopInitializationConfig : IPlayerLoopConfig
    {
        #region ================================ FIELDS

        [Indent]
        [FoldoutGroup("Initialization")]
        [LabelText("Initialization")]
        [SerializeField] [ToggleLeft]
        private bool _initialization = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Profiler Start Frame")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationProfilerStartFrame = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Update Camera Motion Vectors")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationUpdateCameraMotionVectors = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Director Sample Time (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationDirectorSampleTime = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Async Upload Time Sliced Update")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationAsyncUploadTimeSlicedUpdate = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Synchronize Inputs")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationSynchronizeInputs = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("Synchronize State")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationSynchronizeState = true;

        [Indent(2)]
        [FoldoutGroup("Initialization")]
        [LabelText("XR Early Update (Verified)")]
        [SerializeField] [ToggleLeft] [ShowIf("_initialization")]
        private bool _initializationXREarlyUpdate = true;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        Dictionary<string, bool> IPlayerLoopConfig.SubsystemsByTypeFullNames => new Dictionary<string, bool>();
        Dictionary<Type, bool> IPlayerLoopConfig.SubsystemsByTypes =>
            new Dictionary<Type, bool>
            {
                { typeof(UnityEngine.PlayerLoop.Initialization), _initialization },
                { typeof(UnityEngine.PlayerLoop.Initialization.ProfilerStartFrame), _initializationProfilerStartFrame },
                { typeof(UnityEngine.PlayerLoop.Initialization.UpdateCameraMotionVectors), _initializationUpdateCameraMotionVectors },
                { typeof(UnityEngine.PlayerLoop.Initialization.DirectorSampleTime), _initializationDirectorSampleTime },
                { typeof(UnityEngine.PlayerLoop.Initialization.AsyncUploadTimeSlicedUpdate), _initializationAsyncUploadTimeSlicedUpdate },
                { typeof(UnityEngine.PlayerLoop.Initialization.SynchronizeInputs), _initializationSynchronizeInputs },
                { typeof(UnityEngine.PlayerLoop.Initialization.SynchronizeState), _initializationSynchronizeState },
                { typeof(UnityEngine.PlayerLoop.Initialization.XREarlyUpdate), _initializationXREarlyUpdate }
            };

        #endregion
    }
}