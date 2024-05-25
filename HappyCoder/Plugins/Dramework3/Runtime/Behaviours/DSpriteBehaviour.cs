using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Behaviours
{
    [HideMonoScript]
    [RequireComponent(typeof(SpriteRenderer))]
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public class DSpriteBehaviour : DBehaviour
    {
        #region ================================ FIELDS

        [FoldoutGroup("Components")] [BoxGroup("Components/Sprite Renderer", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Sprite Renderer:")]
        [SerializeField] [GetComponent] [ReadOnly]
        private SpriteRenderer _spriteRenderer;

        [FoldoutGroup("Settings", 40)] [BoxGroup("Settings/Fade Duration", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Fade Duration:")]
        [SerializeField]
        private protected float _fadeDuration = 0.3f;

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
            get => _spriteRenderer.color.a;
            set
            {
                var rendererColor = _spriteRenderer.color;
                rendererColor.a = value;
                _spriteRenderer.color = rendererColor;
            }
        }
        public bool IsShown => Alpha >= 0.9f;
        public Sprite sprite
        {
            get => _spriteRenderer.sprite;
            set => _spriteRenderer.sprite = value;
        }
        protected CancellationTokenSource Fader { get; } = new CancellationTokenSource();
        private Color color
        {
            get => _spriteRenderer.color;
            set => _spriteRenderer.color = value;
        }

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
                while (Alpha < 0.9f)
                {
                    var alpha = Alpha;
                    alpha += Time.deltaTime / duration;
                    Alpha = alpha;
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
                while (Alpha > 0.1f)
                {
                    var alpha = Alpha;
                    alpha -= Time.deltaTime / duration;
                    Alpha = alpha;
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
            Alpha = 0;
            onHide?.Invoke();
        }

        public virtual void Show()
        {
            Fader.Cancel();
            onBeforeShow?.Invoke();
            Alpha = 1;
            onShow?.Invoke();
        }

        public virtual void Switch(bool show)
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

#if UNITY_EDITOR

        [SerializeField] [HideInInspector]
        // ReSharper disable NotAccessedField.Global
        private protected string _hideButtonTitle = "Hide";
        [SerializeField] [HideInInspector]
        private protected string _showButtonTitle = "Show";
        // ReSharper restore NotAccessedField.Global

        protected void OnValidate()
        {
            _hideButtonTitle = "Hide Image";
            _showButtonTitle = "Show Image";
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade In", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && Alpha == 0")]
        private void EditorFadeIn()
        {
            FadeIn(_fadeDuration).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 70)]
        [Button("Fade Out", ButtonSizes.Medium)]
        [ShowIf("@EditorApplication.isPlaying && Alpha == 1")]
        private void EditorFadeOut()
        {
            FadeOut(_fadeDuration).Forget();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 100)]
        [Button("@_hideButtonTitle", ButtonSizes.Medium)]
        [HideIf("@Alpha == 0")]
        private void EditorHide()
        {
            Hide();
        }

        [FoldoutGroup("Editor Tools", 100)] [BoxGroup("Editor Tools/Box", false, Order = 100)] [HorizontalGroup("Editor Tools/Box/Buttons", 100)]
        [Button("@_showButtonTitle", ButtonSizes.Medium)]
        [ShowIf("@Alpha == 0")]
        private void EditorShow()
        {
            Show();
        }

#endif
    }
}