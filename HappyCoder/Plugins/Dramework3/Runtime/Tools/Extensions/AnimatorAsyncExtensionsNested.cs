using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    public static partial class AnimatorAsyncExtensions
    {
        #region ================================ NESTED TYPES

        private sealed class AnimatorStateSource : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<AnimatorStateSource>
        {
            #region ================================ FIELDS

            private static TaskPool<AnimatorStateSource> _pool;
            private AnimatorStateSource _nextNode;

            private Animator _animator;
            private int _animationHash;
            private int _layer;
            private IProgress<float> _progress;
            private CancellationToken _cancellationToken;
            private CancellationTokenRegistration _cancellationTokenRegistration;
            private bool _cancelImmediately;
            private bool _completed;

            private UniTaskCompletionSourceCore<AsyncUnit> _core;

            #endregion

            #region ================================ CONSTRUCTORS AND DESTRUCTOR

            static AnimatorStateSource()
            {
                TaskPool.RegisterSizeGetter(typeof(AnimatorStateSource), () => _pool.Size);
            }

            private AnimatorStateSource()
            {
            }

            #endregion

            #region ================================ PROPERTIES AND INDEXERS

            public ref AnimatorStateSource NextNode => ref _nextNode;

            #endregion

            #region ================================ METHODS

            public static IUniTaskSource Create(Animator animator, int animation, int layer, PlayerLoopTiming timing,
                IProgress<float> progress, CancellationToken cancellationToken, bool cancelImmediately, out short token)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
                }

                if (!_pool.TryPop(out var result))
                {
                    result = new AnimatorStateSource();
                }

                result._animator = animator;
                result._animationHash = animation;
                result._layer = layer;
                result._progress = progress;
                result._cancellationToken = cancellationToken;
                result._cancelImmediately = cancelImmediately;
                result._completed = false;

                if (result._cancelImmediately && result._cancellationToken.CanBeCanceled)
                {
                    result._cancellationTokenRegistration = cancellationToken.RegisterWithoutCaptureExecutionContext(
                        state =>
                        {
                            var source = (AnimatorStateSource)state;
                            source._core.TrySetCanceled(source._cancellationToken);
                        }, result);
                }

                TaskTracker.TrackActiveTask(result, 3);
                PlayerLoopHelper.AddAction(timing, result);

                token = result._core.Version;
                return result;
            }

            public void GetResult(short token)
            {
                try
                {
                    _core.GetResult(token);
                }
                finally
                {
                    if (!(_cancelImmediately && _cancellationToken.IsCancellationRequested))
                    {
                        TryReturn();
                    }
                }
            }

            public UniTaskStatus GetStatus(short token)
            {
                return _core.GetStatus(token);
            }

            public bool MoveNext()
            {
                if (_completed || _animator == null || !_animator.enabled)
                {
                    return false;
                }

                if (_cancellationToken.IsCancellationRequested)
                {
                    _core.TrySetCanceled(_cancellationToken);
                    return false;
                }

                var stateInfo = _animator.GetCurrentAnimatorStateInfo(_layer);

                if (_animationHash != -1 && stateInfo.shortNameHash != _animationHash)
                    return true;

                var normalizedTime = stateInfo.normalizedTime;
                var progressValue = Mathf.Clamp01(normalizedTime);

                if (_progress != null)
                {
                    _progress.Report(progressValue);
                }

                if (progressValue < 1f)
                    return true;

                _core.TrySetResult(AsyncUnit.Default);
                return false;
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                _core.OnCompleted(continuation, state, token);
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return _core.UnsafeGetStatus();
            }

            private bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);

                _core.Reset();
                _animator = default;
                _progress = default;
                _cancellationToken = default;
                _cancellationTokenRegistration.Dispose();
                _cancelImmediately = default;

                return _pool.TryPush(this);
            }

            #endregion
        }

        #endregion
    }
}