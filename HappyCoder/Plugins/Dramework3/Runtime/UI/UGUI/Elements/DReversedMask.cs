using IG.HappyCoder.Dramework3.Runtime.Behaviours;

using Sirenix.OdinInspector;

using UnityEditor;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Elements
{
    [ExecuteInEditMode]
    public class DReversedMask : DBehaviour, IMaterialModifier
    {
        #region ================================ FIELDS

        private static readonly Vector2 _center = new Vector2(0.5f, 0.5f);

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/SETTINGS", 40)] [BoxGroup("@Title/SETTINGS/BOX", false)]
        [SerializeField]
        private RectTransform _fitTarget;

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/SETTINGS", 40)] [BoxGroup("@Title/SETTINGS/BOX", false)]
        [SerializeField]
        private bool _fitOnLateUpdate;

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/SETTINGS", 40)] [BoxGroup("@Title/SETTINGS/BOX", false)]
        [SerializeField]
        private bool _onlyForChildren;

        [TitleGroup("@Title")]
        [FoldoutGroup("@Title/SETTINGS", 40)] [BoxGroup("@Title/SETTINGS/BOX", false)]
        [SerializeField]
        private bool _showHoleGraphic;

        private Material _holeMaterial;
        private Material _revertHoleMaterial;
        private Graphic _graphic;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public bool ShowUnmaskGraphic
        {
            get => _showHoleGraphic;
            set
            {
                _showHoleGraphic = value;
                SetDirty();
            }
        }

        private bool FitOnLateUpdate
        {
            get => _fitOnLateUpdate;
            set => _fitOnLateUpdate = value;
        }

        private RectTransform FitTarget
        {
            get => _fitTarget;
            set
            {
                _fitTarget = value;
                FitTo(_fitTarget);
            }
        }


        private Graphic Graphic => _graphic != null ? _graphic : _graphic = GetComponent<Graphic>();

        private bool OnlyForChildren
        {
            get => _onlyForChildren;
            set
            {
                _onlyForChildren = value;
                SetDirty();
            }
        }

        #endregion

        #region ================================ METHODS

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (!isActiveAndEnabled)
            {
                return baseMaterial;
            }

            var stopAfter = MaskUtilities.FindRootSortOverrideCanvas(transform);
            var stencilDepth = MaskUtilities.GetStencilDepth(transform, stopAfter);
            var desiredStencilBit = 1 << stencilDepth;

            StencilMaterial.Remove(_holeMaterial);
            _holeMaterial = StencilMaterial.Add(baseMaterial, desiredStencilBit - 1, StencilOp.Invert, CompareFunction.Equal, _showHoleGraphic ? ColorWriteMask.All : 0, desiredStencilBit - 1, (1 << 8) - 1);

            // Unmask affects only for children.
            var canvasRenderer = Graphic.canvasRenderer;
            if (_onlyForChildren)
            {
                StencilMaterial.Remove(_revertHoleMaterial);
                _revertHoleMaterial = StencilMaterial.Add(baseMaterial, 1 << 7, StencilOp.Invert, CompareFunction.Equal, 0, 1 << 7, (1 << 8) - 1);
                canvasRenderer.hasPopInstruction = true;
                canvasRenderer.popMaterialCount = 1;
                canvasRenderer.SetPopMaterial(_revertHoleMaterial, 0);
            }
            else
            {
                canvasRenderer.hasPopInstruction = false;
                canvasRenderer.popMaterialCount = 0;
            }

            return _holeMaterial;
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            SetDirty();
        }
#endif

        private void FitTo(RectTransform target)
        {
            var rt = transform as RectTransform;
            if (rt == null)
            {
                LogError($"RectTransform component is not found on object \"{name}\"", "", gameObject);
                return;
            }

            rt.pivot = target.pivot;
            rt.position = target.position;
            rt.rotation = target.rotation;

            var s1 = target.lossyScale;
            var s2 = rt.parent.lossyScale;
            rt.localScale = new Vector3(s1.x / s2.x, s1.y / s2.y, s1.z / s2.z);
            rt.sizeDelta = target.rect.size;
            rt.anchorMax = rt.anchorMin = _center;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR
            if (_fitTarget != null && (_fitOnLateUpdate || EditorApplication.isPlayingOrWillChangePlaymode == false))
#else
			if (_fitTarget && _fitOnLateUpdate)
#endif
            {
                FitTo(_fitTarget);
            }
        }

        private void OnDisable()
        {
            StencilMaterial.Remove(_holeMaterial);
            StencilMaterial.Remove(_revertHoleMaterial);
            _holeMaterial = null;
            _revertHoleMaterial = null;

            if (Graphic != null)
            {
                var canvasRenderer = Graphic.canvasRenderer;
                canvasRenderer.hasPopInstruction = false;
                canvasRenderer.popMaterialCount = 0;
                Graphic.SetMaterialDirty();
            }
            SetDirty();
        }

        private void OnEnable()
        {
            if (_fitTarget)
            {
                FitTo(_fitTarget);
            }
            SetDirty();
        }

        private void SetDirty()
        {
            if (Graphic == null) return;
            Graphic.SetMaterialDirty();
        }

        #endregion
    }
}