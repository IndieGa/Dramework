using System;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    [Serializable]
    internal class DContainerObject
    {
        #region ================================ FIELDS

        private const int LABEL_WIDTH = 120;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Scene ID:")]
        [SerializeField] [ReadOnly] [ShowIf("@Instance != null")]
        internal string SceneID = string.Empty;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Instance:")]
        [SerializeField] [ReadOnly] [ShowIf("@Instance != null")]
        internal Component Instance;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Type:")]
        [SerializeField] [ReadOnly] [ShowIf("@Instance == null")]
        internal string AssemblyQualifiedName = string.Empty;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Instance ID:")]
        [SerializeField] [ReadOnly] [ShowIf("Bind")]
        internal string InstanceID = string.Empty;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Use Constructor:")]
        [SerializeField] [ReadOnly] [ShowIf("@Instance == null")]
        internal bool UseConstructor;

        [Indent] [FoldoutGroup("@Title")]
        [LabelWidth(LABEL_WIDTH)] [LabelText("Bind:")]
        [SerializeField] [ReadOnly] [ShowIf("@Instance == null")]
        internal bool Bind;

        #endregion

#if UNITY_EDITOR

        [SerializeField] [HideInInspector]
        internal bool HideSceneID;
        internal string Title => $"{(HideSceneID || Instance != null ? string.Empty : $"Scene{SceneID}_")}" +
                                 $"{(Instance == null ? Type.GetType(AssemblyQualifiedName)?.Name : Instance.name)}";

#endif
    }
}