using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.UI;


#pragma warning disable 0649

namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base
{
    [HideMonoScript]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public abstract class DScreen : DBehaviour, IScreen
    {
        #region ================================ FIELDS

        [Indent] [FoldoutGroup("Components")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Canvas:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private Canvas _canvas;
        [Indent] [FoldoutGroup("Components")]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Canvas Group:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private CanvasGroup _canvasGroup;

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public event Func<UniTask> onAfterHide;
        public event Func<UniTask> onAfterShow;
        public event Func<UniTask> onBeforeHide;
        public event Func<UniTask> onBeforeShow;
        public event Action<float> onFadeProgress;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public bool IsShown => _canvasGroup != null && _canvasGroup.alpha > 0.9f;
        public int SortingLayerID
        {
            get => _canvas.sortingLayerID;
            set => _canvas.sortingLayerID = value;
        }
        public int SortingOrder
        {
            get => _canvas.sortingOrder;
            set => _canvas.sortingOrder = value;
        }
        protected Canvas Canvas => _canvas;

        #endregion

        #region ================================ METHODS

        public async UniTask Hide(float duration)
        {
            if (IsShown == false)
                return;

            if (onBeforeHide != null)
                await onBeforeHide.Invoke();

            _cancellationTokenSource.Cancel();
            while (_canvasGroup.alpha > 0)
            {
                var alpha = _canvasGroup.alpha;
                alpha -= Time.deltaTime / duration;
                _canvasGroup.alpha = alpha;
                onFadeProgress?.Invoke(alpha);
                await UniTask.Yield(_cancellationTokenSource.Token);
            }

            Hide();

            if (onAfterHide != null)
                await onAfterHide.Invoke();
        }

        public void Hide()
        {
            _cancellationTokenSource.Cancel();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public async UniTask Show(float duration)
        {
            if (IsShown)
                return;

            if (onBeforeShow != null)
                await onBeforeShow.Invoke();

            _cancellationTokenSource.Cancel();
            while (_canvasGroup.alpha < 1)
            {
                var alpha = _canvasGroup.alpha;
                alpha += Time.deltaTime / duration;
                _canvasGroup.alpha = alpha;
                onFadeProgress?.Invoke(alpha);
                await UniTask.Yield(_cancellationTokenSource.Token);
            }

            Show();

            if (onAfterShow != null)
                await onAfterShow.Invoke();
        }

        public void Show()
        {
            _cancellationTokenSource.Cancel();
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        protected virtual void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion

#if UNITY_EDITOR

        #region ================================ FIELDS

        // ReSharper disable NotAccessedField.Global
        [SerializeField] [HideInInspector]
        private protected string _showSoloButtonTitle = "Show Solo";
        [SerializeField] [HideInInspector]
        private protected string _hideButtonTitle = "Hide";
        [SerializeField] [HideInInspector]
        private protected string _showButtonTitle = "Show";
        // ReSharper restore NotAccessedField.Global

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private IEnumerable SortingLayers
        {
            get
            {
                var layers = new ValueDropdownList<int>();
                foreach (var layer in SortingLayer.layers)
                    layers.Add(layer.name, layer.id);

                return layers;
            }
        }

        #endregion

        #region ================================ METHODS

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade In", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && IsShown == false")]
        private void EditorFadeIn()
        {
            Show(0.8f).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade Out", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && IsShown")]
        private void EditorFadeOut()
        {
            Hide(0.8f).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", MinWidth = 46)]
        [Button("@_hideButtonTitle", ButtonSizes.Medium)]
        [HideIf("@IsShown == false")]
        private void EditorHide()
        {
            Hide();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", MinWidth = 46)]
        [Button("@_showButtonTitle", ButtonSizes.Medium)]
        [ShowIf("@IsShown == false")]
        private void EditorShow()
        {
            Show();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", MinWidth = 74)]
        [Button("@_showSoloButtonTitle", ButtonSizes.Medium)]
        [ShowIf("@IsShown == false")]
        private void EditorShowSolo()
        {
            foreach (var data in FindObjectsByType<DScreen>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).Where(d => d != this))
                data.EditorHide();

            EditorShow();
        }

        #endregion

#endif
    }
}