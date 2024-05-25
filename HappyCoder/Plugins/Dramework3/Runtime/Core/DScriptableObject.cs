using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Core
{
    [HideMonoScript]
    public class DScriptableObject : ScriptableObject
    {
        #region ================================ METHODS

        protected void Log(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.Log(message.ToString(), prefix, sender);
        }

        protected void LogError(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogError(message.ToString(), prefix, sender);
        }

        protected void LogWarning(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogWarning(message.ToString(), prefix, sender);
        }

        #endregion

#if UNITY_EDITOR

        [SerializeField] [HideInInspector]
        private Texture _logo;

        [SerializeField] [HideInInspector]
        // ReSharper disable once NotAccessedField.Global
        protected bool LockSaveButton;

        [SerializeField] [HideInInspector]
        // ReSharper disable once NotAccessedField.Global
        protected bool HideSaveButton;

        [OnInspectorGUI]
        [PropertyOrder(-100000)]
        private void DrawHeader()
        {
            if (_logo == null)
                _logo = Resources.Load<Texture>(EditorGUIUtility.isProSkin ? "D Framework 3 Logo Dark" : "D Framework 3 Logo Light");

            GUILayout.Label(_logo);
        }

        [FoldoutGroup("Menu", 1000)] [BoxGroup("Menu/Buttons", false)] [HorizontalGroup("Menu/Buttons/Horizontal")]
        [Button("Save", ButtonSizes.Medium)] [PropertyOrder(10000)]
        [DisableIf("LockSaveButton")] [HideIf("HideSaveButton")]
        public virtual void Save()
        {
            DeferredSave().Forget();
        }

        private async UniTask DeferredSave()
        {
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

#endif
    }

    [HideMonoScript]
    public class DScriptableObjectWS : ScriptableObject
    {
        #region ================================ METHODS

        protected void Log(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.Log(message.ToString(), prefix, sender);
        }

        protected void LogError(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogError(message.ToString(), prefix, sender);
        }

        protected void LogWarning(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogWarning(message.ToString(), prefix, sender);
        }

        #endregion

#if UNITY_EDITOR

        [SerializeField] [HideInInspector]
        private Texture _logo;

        [OnInspectorGUI]
        [PropertyOrder(-100000)]
        private void DrawHeader()
        {
            if (_logo == null)
                _logo = Resources.Load<Texture>(EditorGUIUtility.isProSkin ? "D Framework 3 Logo Dark" : "D Framework 3 Logo Light");

            GUILayout.Label(_logo);
        }

#endif
    }
}