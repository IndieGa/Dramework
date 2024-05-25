using System;
using System.IO;

using IG.HappyCoder.Dramework3.Runtime.Tools.Loggers;

using MemoryPack;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Timers
{
    public static class DTimer
    {
        #region ================================ FIELDS

        private static TimerData _timerData;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        private static string PATH => Path.Combine(Application.persistentDataPath, "cnttmr.dat");

        #endregion

        #region ================================ METHODS

        public static int CreateCountdownTimer(float time, Action<float> onTick)
        {
            return _timerData.CreateCountdownTimer(time, onTick);
        }

        public static float GetCountdownTime(int timerID)
        {
            return _timerData.GetCountdownTime(timerID);
        }

        public static bool IsCountdownTimerValid(int timerID)
        {
            return _timerData.IsCountdownTimerValid(timerID);
        }

        public static void ReleaseCountdownTimer(int timerID)
        {
            _timerData.DisposeCountdownTimer(timerID);
        }

        public static void SubscribeOnCountdownTick(int timerID, Action<float> onTick)
        {
            _timerData.SubscribeOnCountdownTick(timerID, onTick);
        }

        public static void UnsubscribeFromCountdownTick(int timerID, Action<float> onTick)
        {
            _timerData.UnsubscribeFromCountdownTick(timerID, onTick);
        }

        internal static void Initialize()
        {
            InitializeTimerData();
            _timerData.Initialize();
        }

        internal static void OnContinue()
        {
            _timerData.Initialize();
        }

        internal static void OnDestroy()
        {
            if (_timerData == null) return;
            SaveTimerData();
            _timerData.Dispose();
            _timerData = null;
        }

        internal static void OnPause()
        {
            SaveTimerData();
        }

        internal static void Update()
        {
            _timerData.Update();
        }

        private static void InitializeTimerData()
        {
            if (File.Exists(PATH))
            {
                try
                {
                    var bytes = File.ReadAllBytes(PATH);
                    _timerData = MemoryPackSerializer.Deserialize<TimerData>(bytes);
                }
                catch (Exception e)
                {
                    ConsoleLogger.LogError(e.Message, "[DTImer]");
                    _timerData = new TimerData();
                }
            }
            else
            {
                _timerData = new TimerData();
            }
        }

        private static void SaveTimerData()
        {
            _timerData.PrepareForSave();
            try
            {
                var bytes = MemoryPackSerializer.Serialize(_timerData);
                File.WriteAllBytes(PATH, bytes);
            }
            catch (Exception e)
            {
                ConsoleLogger.LogError(e.Message, "[DTImer]");
            }
        }

        #endregion
    }
}