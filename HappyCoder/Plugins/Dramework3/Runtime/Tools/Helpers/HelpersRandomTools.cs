using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ FIELDS

        private static readonly Random _random = new Random();

        #endregion

        #region ================================ NESTED TYPES

        public static class RandomTools
        {
            #region ================================ METHODS

            public static int GetRandom(int min, int max)
            {
                return _random.Next(min, max);
            }

            public static float GetRandom(float min, float max)
            {
                return (float)_random.NextDouble() * (max - min) + min;
            }

            #endregion
        }

        #endregion
    }
}