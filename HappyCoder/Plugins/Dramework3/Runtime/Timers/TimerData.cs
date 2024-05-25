using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using MemoryPack;

using DateTime = System.DateTime;


namespace IG.HappyCoder.Dramework3.Runtime.Timers
{
    [MemoryPackable]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal partial class TimerData
    {
        #region ================================ FIELDS

        [MemoryPackInclude] private DateTime _saveDateTime = DateTime.Now;
        [MemoryPackInclude] private Dictionary<int, CountdownTimer> _countdownTimers = new Dictionary<int, CountdownTimer>();
        private readonly List<int> _timersToRemove = new List<int>();

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal float SecondsFromLastSave => (float)(DateTime.Now - _saveDateTime).TotalMilliseconds / 1000f;

        #endregion

        #region ================================ METHODS

        internal int CreateCountdownTimer(float time, Action<float> onTick)
        {
            var countdownTimer = new CountdownTimer();
            countdownTimer.Initialize(time, onTick);
            var timerID = countdownTimer.GetHashCode();
            _countdownTimers.Add(timerID, countdownTimer);
            return timerID;
        }

        internal void Dispose()
        {
            foreach (var timer in _countdownTimers.Values)
            {
                timer.Dispose();
            }

            _countdownTimers = null;
        }

        internal void DisposeCountdownTimer(int timerID)
        {
            _countdownTimers[timerID].Dispose();
            _countdownTimers.Remove(timerID);
        }

        internal float GetCountdownTime(int timerID)
        {
            return _countdownTimers[timerID].Time;
        }

        internal void Initialize()
        {
            var timersID = new List<int>();
            foreach (var timer in _countdownTimers)
            {
                if (timer.Value.Restore(SecondsFromLastSave)) continue;
                timersID.Add(timer.Key);
            }

            foreach (var timerID in timersID)
                DisposeCountdownTimer(timerID);
        }

        internal bool IsCountdownTimerValid(int timerID)
        {
            return _countdownTimers.ContainsKey(timerID);
        }

        internal void PrepareForSave()
        {
            _saveDateTime = DateTime.Now;
        }

        internal void SubscribeOnCountdownTick(int timerID, Action<float> onTick)
        {
            _countdownTimers[timerID].SubscribeOnTick(onTick);
        }

        internal void UnsubscribeFromCountdownTick(int timerID, Action<float> onTick)
        {
            _countdownTimers[timerID].UnsubscribeFromTick(onTick);
        }

        internal void Update()
        {
            _timersToRemove.Clear();
            foreach (var timer in _countdownTimers)
            {
                timer.Value.Update();
                if (timer.Value.Time <= 0)
                    _timersToRemove.Add(timer.Key);
            }

            foreach (var timerID in _timersToRemove)
                DisposeCountdownTimer(timerID);
        }

        #endregion
    }
}