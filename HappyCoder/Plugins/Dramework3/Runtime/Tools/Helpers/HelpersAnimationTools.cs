using System.Collections.Generic;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class AnimationTools
        {
            #region ================================ METHODS

            public static AnimatorOverrideController GetAnimatorOverrideController(Animator animator, string clipName, AnimationClip newClip)
            {
                var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                var animations = new List<KeyValuePair<AnimationClip, AnimationClip>>();
                foreach (var originalClip in animatorOverrideController.animationClips)
                {
                    animations.Add(originalClip.name == clipName
                        ? new KeyValuePair<AnimationClip, AnimationClip>(originalClip, newClip)
                        : new KeyValuePair<AnimationClip, AnimationClip>(originalClip, originalClip));
                }
                animatorOverrideController.ApplyOverrides(animations);
                return animatorOverrideController;
            }

            public static IEnumerable<AnimatorOverrideController> GetAnimatorOverrideControllers(Animator animator, string clipName, IEnumerable<AnimationClip> clips)
            {
                var result = new List<AnimatorOverrideController>();

                foreach (var newClip in clips)
                {
                    var animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                    var animations = new List<KeyValuePair<AnimationClip, AnimationClip>>();

                    foreach (var originalClip in animatorOverrideController.animationClips)
                    {
                        animations.Add(originalClip.name.StartsWith(clipName)
                            ? new KeyValuePair<AnimationClip, AnimationClip>(originalClip, newClip)
                            : new KeyValuePair<AnimationClip, AnimationClip>(originalClip, originalClip));
                    }

                    animatorOverrideController.ApplyOverrides(animations);
                    result.Add(animatorOverrideController);
                }

                return result;
            }

            #endregion
        }

        #endregion
    }
}