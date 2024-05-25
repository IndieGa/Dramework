using System.Diagnostics.CodeAnalysis;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Behaviours
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class DGoBehaviour : DBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Renderers:")]
        [SerializeField] [GetComponentInChildren(includingThisObject: true, log: false)] [ReadOnly]
        protected Renderer[] _renderers;

        [FoldoutGroup("Components")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Colliders:")]
        [SerializeField] [GetComponentInChildren(includingThisObject: true, log: false)] [ReadOnly]
        protected Collider[] _colliders;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public bool IsShown { get; private set; }

        #endregion

        #region ================================ METHODS

        public void Hide(bool disableRenderers = true, bool disableColliders = true)
        {
            foreach (var rend in _renderers)
                rend.enabled = disableRenderers;

            foreach (var col in _colliders)
                col.enabled = disableColliders;

            IsShown = false;
        }

        public void Show(bool enableRenderers = true, bool enableColliders = true)
        {
            foreach (var rend in _renderers)
                rend.enabled = enableRenderers;

            foreach (var col in _colliders)
                col.enabled = enableColliders;

            IsShown = true;
        }

        #endregion
    }
}