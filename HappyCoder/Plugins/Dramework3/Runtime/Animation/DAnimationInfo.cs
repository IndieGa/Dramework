namespace IG.HappyCoder.Dramework3.Runtime.Animation
{
    internal class DAnimationInfo
    {
        #region ================================ FIELDS

        private readonly PlayAnimationType _playAnimationType;
        private readonly int _animationNameHash;
        private readonly float _fadeDuration;
        private readonly int _layer;
        private readonly float _normalizedTimeOffset;
        private readonly float _delay;

        #endregion

        #region ================================ CONSTRUCTORS AND DESTRUCTOR

        internal DAnimationInfo(PlayAnimationType playAnimationType, int animationNameHash, float fadeDuration, int layer, float normalizedTimeOffset, float delay)
        {
            _playAnimationType = playAnimationType;
            _animationNameHash = animationNameHash;
            _fadeDuration = fadeDuration;
            _layer = layer;
            _normalizedTimeOffset = normalizedTimeOffset;
            _delay = delay;
        }

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal int AnimationNameHash => _animationNameHash;
        internal float Delay => _delay;
        internal float FadeDuration => _fadeDuration;
        internal int Layer => _layer;
        internal float NormalizedTimeOffset => _normalizedTimeOffset;
        internal PlayAnimationType PlayAnimationType => _playAnimationType;

        #endregion
    }
}