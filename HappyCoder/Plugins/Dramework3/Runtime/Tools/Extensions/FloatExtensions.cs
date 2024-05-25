using System;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class FloatExtensions
    {
        #region ================================ METHODS

        public static float Clamp(this float value, float min, float max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        public static bool Equal(this float first, float second, float tolerance)
        {
            return Math.Abs(first - second) < tolerance;
        }

        public static bool Greater(this float first, float second)
        {
            return first > second;
        }

        public static bool GreaterOrEqual(this float first, float second)
        {
            return first >= second;
        }

        public static void IfEqual(this float first, float second, float tolerance, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (Math.Abs(first - second) < tolerance)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfGreater(this float first, float second, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (first > second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfGreaterOrEqual(this float first, float second, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (first >= second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfLessOrEqual(this float first, float second, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (first <= second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfLessThan(this float first, float second, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (first < second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfNotEqual(this float first, float second, float tolerance, Action<float, float> onSuccess, Action<float, float> onFail)
        {
            if (Math.Abs(first - second) > tolerance)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static bool Less(this float first, float second)
        {
            return first < second;
        }

        public static bool LessOrEqual(this float first, float second)
        {
            return first <= second;
        }

        public static bool NotEqual(this float first, float second, float tolerance)
        {
            return Math.Abs(first - second) > tolerance;
        }

        #endregion
    }
}