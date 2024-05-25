using System;
using System.Diagnostics.CodeAnalysis;

using MemoryPack;


namespace IG.HappyCoder.Dramework3.Runtime.Timers
{
    [MemoryPackable]
    [SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
    internal partial class CountdownTimer
    {
        #region ================================ FIELDS

        private float second = 1f;

        #endregion

        #region ================================ EVENTS AND DELEGATES

        private event Action<float> onTick;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        [MemoryPackInclude]
        internal float Time { get; private set; }

        #endregion

        #region ================================ METHODS

        internal void Dispose()
        {
            onTick = null;
        }

        internal void Initialize(float time, Action<float> onTick)
        {
            Time = time;
            SubscribeOnTick(onTick);
        }

        internal bool Restore(float secondsFromLastSave)
        {
            Time -= secondsFromLastSave;
            return Time > 0;
        }

        internal void SubscribeOnTick(Action<float> onTick)
        {
            if (onTick == null) return;
            this.onTick += onTick;
            onTick.Invoke(Time);
        }

        internal void UnsubscribeFromTick(Action<float> onTick)
        {
            if (onTick == null) return;
            onTick.Invoke(Time);
            this.onTick -= onTick;
        }

        internal void Update()
        {
            if (Time <= 0) return;

            if (second <= 0)
            {
                onTick?.Invoke(Time);
                second = 1;
            }

            Time -= UnityEngine.Time.deltaTime;
            second -= UnityEngine.Time.deltaTime;

            if (Time <= 0)
                onTick?.Invoke(0);
        }

        #endregion
    }
}