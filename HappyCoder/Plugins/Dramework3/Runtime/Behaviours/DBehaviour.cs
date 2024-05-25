using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using IG.HappyCoder.Dramework3.Runtime.Container.Core;
using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;
using IG.HappyCoder.Dramework3.Runtime.Validation.Interfaces;

using Sirenix.OdinInspector;

using UnityEngine;

using Object = UnityEngine.Object;


#pragma warning disable 0414

namespace IG.HappyCoder.Dramework3.Runtime.Behaviours
{
    [HideMonoScript]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public partial class DBehaviour : MonoBehaviour, IValidable
    {
        #region ================================ METHODS

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.Log(message.ToString(), prefix, sender);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogError(message.ToString(), prefix, sender);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(object message, string prefix = "", Object sender = null)
        {
            ConsoleLogger.LogWarning(message.ToString(), prefix, sender);
        }

        protected void Bind(object obj)
        {
            DCore.Bind(obj, gameObject.scene.name, string.Empty);
        }

        protected void Bind(object obj, string containerID)
        {
            DCore.Bind(obj, containerID, string.Empty);
        }

        protected void Bind(object obj, string containerID, string instanceID)
        {
            DCore.Bind(obj, containerID, instanceID);
        }

        protected virtual void EditorInitialize()
        {
        }

        protected void InitializeObject(object obj)
        {
            DCore.InitializeContainerObject(obj);
            ConsoleLogger.Log($"Object type of «{obj.GetType()}» is initialized", $"[{nameof(DBehaviour)}.{nameof(InitializeObject)}()]");
        }

        internal void InternalInitialize()
        {
            EditorInitialize();
        }

        void IValidable.Validate()
        {
#if UNITY_EDITOR
            InitializeObject();
            ApplyPrefabInstance();
#endif
        }

        #endregion
    }
}