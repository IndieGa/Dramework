using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


#pragma warning disable 0649

namespace IG.HappyCoder.Dramework3.Runtime.UI.UGUI.Base
{
    [HideMonoScript]
    [RequireComponent(typeof(CanvasGroup))]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public partial class DUIBehaviour : DBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Rect Transform", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Rect Transform:")]
        [SerializeField] [GetComponent] [ReadOnly]
        protected RectTransform _rectTransform;

        [FoldoutGroup("Components")] [BoxGroup("Components/Canvas Group", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Canvas Group:")]
        [SerializeField] [GetComponent] [ReadOnly]
        protected CanvasGroup _canvasGroup;

        [FoldoutGroup("Settings", 40)] [BoxGroup("Settings/Fade Duration", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Fade Duration:")]
        [SerializeField]
        protected float _fadeDuration = 0.3f;

        #endregion

        #region ================================ EVENTS AND DELEGATES

        public event Action onBeforeFadeIn;
        public event Action onBeforeFadeOut;
        public event Action onBeforeHide;
        public event Action onBeforeShow;
        public event Action<float> onFadeProgress;
        public event Action onHide;
        public event Action onShow;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        public float Alpha
        {
            get => _canvasGroup.alpha;
            set => _canvasGroup.alpha = value;
        }
        public bool IsShown => _canvasGroup.alpha >= 0.9f;
        public RectTransform RectTransform => _rectTransform;
        protected CancellationTokenSource Fader { get; } = new CancellationTokenSource();

        #endregion

        #region ================================ METHODS

        public virtual async UniTask FadeIn(float duration = 0)
        {
            if (IsShown)
            {
                onShow?.Invoke();
            }
            else
            {
                Fader.Cancel();
                duration = duration > 0 ? duration : _fadeDuration > 0 ? _fadeDuration : 1;
                onBeforeFadeIn?.Invoke();
                while (_canvasGroup.alpha < 1)
                {
                    var alpha = _canvasGroup.alpha;
                    alpha += Time.deltaTime / duration;
                    _canvasGroup.alpha = alpha;
                    onFadeProgress?.Invoke(alpha);
                    await UniTask.Yield(Fader.Token);
                }
                Show();
            }
        }

        public virtual async UniTask FadeOut(float duration = 0)
        {
            if (IsShown)
            {
                Fader.Cancel();
                duration = duration > 0 ? duration : _fadeDuration > 0 ? _fadeDuration : 1;
                onBeforeFadeOut?.Invoke();
                while (_canvasGroup.alpha > 0)
                {
                    var alpha = _canvasGroup.alpha;
                    alpha -= Time.deltaTime / duration;
                    _canvasGroup.alpha = alpha;
                    onFadeProgress?.Invoke(alpha);
                    await UniTask.Yield(Fader.Token);
                }
                Hide();
            }
            else
            {
                onHide?.Invoke();
            }
        }

        public virtual void Hide()
        {
            Fader.Cancel();
            onBeforeHide?.Invoke();
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            onHide?.Invoke();
        }

        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }

        public virtual void Show()
        {
            Fader.Cancel();
            onBeforeShow?.Invoke();
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            onShow?.Invoke();
        }

        public void Switch(bool show)
        {
            if (show)
                Show();
            else
                Hide();
        }

        protected virtual void OnDestroy()
        {
            Fader.Cancel();
            Fader.Dispose();
        }

        #endregion
    }
}