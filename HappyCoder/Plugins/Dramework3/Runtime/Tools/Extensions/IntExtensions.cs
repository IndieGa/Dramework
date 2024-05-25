using System;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class IntExtensions
    {
        #region ================================ METHODS

        public static int Clamp(this int value, int min, int max)
        {
            return Mathf.Clamp(value, min, max);
        }

        public static bool Equal(this int first, int second)
        {
            return first == second;
        }

        public static bool GreaterOrEqual(this int first, int second)
        {
            return first >= second;
        }

        public static bool GreaterThan(this int first, int second)
        {
            return first > second;
        }

        public static void IfEqual(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first == second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfGreater(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first > second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfGreaterOrEqual(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first >= second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfLessOrEqual(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first <= second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfLessThan(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first < second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static void IfNotEqual(this int first, int second, Action<int, int> onSuccess, Action<int, int> onFail)
        {
            if (first != second)
                onSuccess?.Invoke(first, second);
            else
                onFail?.Invoke(first, second);
        }

        public static bool LessOrEqual(this int first, int second)
        {
            return first <= second;
        }

        public static bool LessThan(this int first, int second)
        {
            return first < second;
        }

        public static bool NotEqual(this int first, int second)
        {
            return first != second;
        }

        #endregion
    }
}