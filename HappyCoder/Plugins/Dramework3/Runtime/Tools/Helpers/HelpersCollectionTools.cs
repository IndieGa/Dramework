using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class CollectionTools
        {
            #region ================================ METHODS

            public static IList<T> GetInsertionSorted<T>(IEnumerable<T> collection) where T : IComparable<T>
            {
                var copy = collection.ToList();
                InsertionSort(copy);
                return copy;
            }

            public static T[] GetInsertionSorted<T>(T[] collection) where T : IComparable<T>
            {
                var copy = new T[collection.Length];
                for (var i = 0; i < copy.Length; i++)
                {
                    copy[i] = collection[i];
                }
                InsertionSort(copy);
                return copy;
            }

            public static List<T> GetRandomItems<T>(List<T> list, int count = 1)
            {
                count = Mathf.Clamp(count, 0, list.Count);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, list.Count);
                    var item = list[index];
                    if (result.Contains(item)) continue;
                    result.Add(item);
                }

                return result;
            }

            public static IReadOnlyList<T> GetRandomItems<T>(IReadOnlyList<T> list, int count = 1, IReadOnlyList<T> excluding = null, int tryCount = 1000)
            {
                count = Mathf.Clamp(count, 0, list.Count);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, list.Count);
                    var item = list[index];
                    if (result.Contains(item) || (excluding != null && excluding.Contains(item)))
                    {
                        tryCount--;
                        if (tryCount == 0)
                            return result;

                        continue;
                    }
                    result.Add(item);
                }

                return result;
            }

            public static T[] GetRandomItems<T>(T[] array, int count = 1)
            {
                count = Mathf.Clamp(count, 0, array.Length);
                var result = new List<T>();
                while (result.Count < count)
                {
                    var index = _random.Next(0, array.Length);
                    var item = array[index];
                    if (result.Contains(item)) continue;
                    result.Add(item);
                }

                return result.ToArray();
            }

            public static void InsertionSort<T>(IList<T> collection) where T : IComparable<T>
            {
                for (var i = 1; i < collection.Count; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && collection[index - 1].CompareTo(current) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static void InsertionSort<T1, T2>(IList<T1> collection, Func<T1, T2> getComparable) where T2 : IComparable<T2>
            {
                for (var i = 1; i < collection.Count; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && getComparable.Invoke(collection[index - 1]).CompareTo(getComparable.Invoke(current)) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static void InsertionSort<T>(T[] collection) where T : IComparable<T>
            {
                for (var i = 1; i < collection.Length; i++)
                {
                    var index = i;
                    var current = collection[i];

                    while (index > 0 && collection[index - 1].CompareTo(current) > 0)
                    {
                        (collection[index], collection[index - 1]) = (collection[index - 1], collection[index]);
                        index--;
                    }

                    collection[index] = current;
                }
            }

            public static void ShellSort<T>(IList<T> collection) where T : IComparable<T>
            {
            }

            public static T[] ShuffleArray<T>(T[] array)
            {
                for (var i = array.Length - 1; i >= 1; i--)
                {
                    var j = _random.Next(i + 1);
                    (array[j], array[i]) = (array[i], array[j]);
                }
                return array;
            }

            public static List<T> ShuffleList<T>(List<T> list)
            {
                for (var i = list.Count - 1; i >= 1; i--)
                {
                    var j = _random.Next(i + 1);
                    (list[j], list[i]) = (list[i], list[j]);
                }
                return list;
            }

            #endregion
        }

        #endregion
    }
}