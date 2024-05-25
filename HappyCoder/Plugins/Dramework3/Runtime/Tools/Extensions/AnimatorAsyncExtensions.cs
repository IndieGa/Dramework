using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static partial class AnimatorAsyncExtensions
    {
        #region ================================ METHODS

        public static UniTask WaitAnimationComplete
        (
            this Animator animator,
            string animationName,
            int layer = 0,
            IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.PostLateUpdate
        )
        {
            return WaitAnimationComplete
            (
                animator,
                Animator.StringToHash(animationName),
                layer,
                progress,
                timing,
                animator.GetCancellationTokenOnDestroy()
            );
        }

        public static UniTask WaitAnimationComplete
        (
            this Animator animator,
            int layer = 0,
            IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.PostLateUpdate
        )
        {
            return WaitAnimationComplete
            (
                animator,
                -1,
                layer,
                progress,
                timing,
                animator.GetCancellationTokenOnDestroy()
            );
        }

        public static UniTask WaitAnimationComplete
        (
            this Animator animator,
            int animationHash = -1,
            int layer = 0,
            IProgress<float> progress = null,
            PlayerLoopTiming timing = PlayerLoopTiming.PostLateUpdate, CancellationToken cancellationToken = default,
            bool cancelImmediately = false
        )
        {
            if (animator == null)
                throw new ArgumentNullException(nameof(animator));

            if (cancellationToken.IsCancellationRequested)
                return UniTask.FromCanceled(cancellationToken);

            return new UniTask
            (
                AnimatorStateSource.Create
                (
                    animator,
                    animationHash,
                    layer,
                    timing,
                    progress,
                    cancellationToken,
                    cancelImmediately,
                    out var token
                ),
                token
            );
        }

        #endregion
    }
}