using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using IG.HappyCoder.Dramework3.Runtime.Behaviours;
using IG.HappyCoder.Dramework3.Runtime.Initialization.Attributes.Getting;
using IG.HappyCoder.Dramework3.Runtime.Tools.Constants;

using Sirenix.OdinInspector;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Animation
{
    [HideMonoScript]
    [RequireComponent(typeof(Animator))]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DAnimatorBehaviour : DGoBehaviour
    {
        #region ================================ FIELDS

        [Indent] [FoldoutGroup("Components")] [BoxGroup("Components/Box", false)]
        [LabelWidth(ConstantValues.Int_140)] [LabelText("Animator:")]
        [SerializeField] [GetComponent] [PropertyOrder(-1)] [ReadOnly]
        protected Animator _animator;

        private readonly Dictionary<int, AnimationClip> _clips = new Dictionary<int, AnimationClip>();
        private readonly Queue<DAnimationInfo> _queue = new Queue<DAnimationInfo>();

        private bool _lock;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        protected IReadOnlyList<AnimationClip> Clips => _animator.runtimeAnimatorController.animationClips;

        #endregion

        #region ================================ METHODS

        public void CrossFade(int animationNameHash, float fadeDuration, int layer, float normalizedTimeOffset, float delay)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _queue.Enqueue(new DAnimationInfo(PlayAnimationType.CrossFade, animationNameHash, fadeDuration, layer, normalizedTimeOffset, delay));
            PlayLoop().Forget();
        }

        public void Play(int animationNameHash, int layer, float normalizedTimeOffset, float delay)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _queue.Enqueue(new DAnimationInfo(PlayAnimationType.Play, animationNameHash, 0, layer, normalizedTimeOffset, delay));
            PlayLoop().Forget();
        }

        public void Stop()
        {
            _queue.Clear();
            _cancellationTokenSource.Cancel();
            _lock = false;
        }

        protected virtual void Awake()
        {
            foreach (var animationClip in _animator.runtimeAnimatorController.animationClips)
                _clips.TryAdd(Animator.StringToHash(animationClip.name), animationClip);
        }

        private async UniTaskVoid PlayLoop()
        {
            if (_lock) return;
            _lock = true;

            while (_queue.Count > 0)
            {
                var info = _queue.Dequeue();

                switch (info.PlayAnimationType)
                {
                    case PlayAnimationType.Play:
                        _animator.Play(info.AnimationNameHash, info.Layer, info.NormalizedTimeOffset);
                        break;
                    case PlayAnimationType.CrossFade:
                        _animator.CrossFade(info.AnimationNameHash, info.FadeDuration, info.Layer, info.NormalizedTimeOffset);
                        break;
                }

                if (info.Delay < 0)
                {
                    var clip = _clips[info.AnimationNameHash];
                    var delay = (int)(clip.length / _animator.GetCurrentAnimatorStateInfo(info.Layer).speed * 1000);
                    await UniTask.Delay(delay, cancellationToken: _cancellationTokenSource.Token);
                }
                else
                {
                    var delay = (int)(info.Delay * 1000);
                    await UniTask.Delay(delay, cancellationToken: _cancellationTokenSource.Token);
                }
            }

            _lock = false;
        }

        #endregion
    }
}