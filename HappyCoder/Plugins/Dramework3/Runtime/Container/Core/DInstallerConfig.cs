using System;
using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Tools.Helpers;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Container.Core
{
    [Serializable] [HideLabel]
    [SuppressMessage("ReSharper", "NotAccessedField.Local")]
    internal class DInstallerConfig
    {
        #region ================================ FIELDS

        [HorizontalGroup("Horizontal", 14)]
        [HideLabel]
        [SerializeField]
        private bool _enabled = true;

        [HorizontalGroup("Horizontal", 100)]
        [HideLabel]
        [SerializeField] [ReadOnly]
        private string _sceneID;

        [HorizontalGroup("Horizontal", 50)]
        [HideLabel] [ReadOnly]
        [SerializeField]
        private int _order;

        [HorizontalGroup("Horizontal")]
        [HideLabel] [ReadOnly]
        [SerializeField]
        private string _name;

        [SerializeField] [HideInInspector]
        private string _assemblyQualifiedName;

        [SerializeField] [HideInInspector]
        private int _sceneBuildOrder;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        public DInstallerConfig(string sceneID, int order, string assemblyQualifiedName, string name)
        {
            _enabled = true;
            _sceneID = sceneID;
            _order = order;
            _assemblyQualifiedName = assemblyQualifiedName;
            _name = name;
#if UNITY_EDITOR
            _sceneBuildOrder = Helpers.EditorTools.GetSceneIndex(sceneID);
#endif
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal string AssemblyQualifiedName => _assemblyQualifiedName;
        internal bool Enabled => _enabled;
        internal int Order => _order;
        internal int SceneBuildOrder => _sceneBuildOrder;
        internal string SceneID => _sceneID;

        #endregion
    }
}