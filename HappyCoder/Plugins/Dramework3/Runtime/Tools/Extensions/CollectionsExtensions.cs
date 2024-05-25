using System;
using System.Collections.Generic;
using System.Linq;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class CollectionsExtensions
    {
        #region ================================ METHODS

        public static IReadOnlyList<T> Get<T>(this IEnumerable<object> collection)
        {
            var result = new List<T>();
            foreach (var obj in collection)
            {
                if (obj is T tObj)
                    result.Add(tObj);
            }
            return result;
        }

        public static List<T> GetRandomItems<T>(this List<T> list, int count = 1)
        {
            return Helpers.Helpers.CollectionTools.GetRandomItems(list, count);
        }

        public static IReadOnlyList<T> GetRandomItems<T>(this IReadOnlyList<T> list, int count = 1, IReadOnlyList<T> excluding = null, int tryCount = 1000)
        {
            return Helpers.Helpers.CollectionTools.GetRandomItems(list, count, excluding);
        }

        public static T[] GetRandomItems<T>(this T[] array, int count = 1)
        {
            return Helpers.Helpers.CollectionTools.GetRandomItems(array, count);
        }

        public static IEnumerable<T> IfContains<T>(this IEnumerable<T> enumerable, T obj, Action onSuccess)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            if (array.Contains(obj))
                onSuccess.Invoke();

            return array;
        }

        public static IEnumerable<T> IfNotContains<T>(this IEnumerable<T> enumerable, T obj, Action onSuccess)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();
            if (array.Contains(obj) == false)
                onSuccess.Invoke();

            return array;
        }

        public static T[] ShuffleArray<T>(this T[] array)
        {
            return Helpers.Helpers.CollectionTools.ShuffleArray(array);
        }

        public static List<T> ShuffleList<T>(this List<T> list)
        {
            return Helpers.Helpers.CollectionTools.ShuffleList(list);
        }

        #endregion
    }
}