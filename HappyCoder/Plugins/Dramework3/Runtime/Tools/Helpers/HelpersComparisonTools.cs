using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class ComparisonTools
        {
            #region ================================ METHODS

            public static bool Compare(Type type1, Type type2)
            {
                return type1 == type2 || type2.IsAssignableFrom(type1);
            }

            public static bool IsType(object obj, Type type2)
            {
                var type1 = obj.GetType();
                return type1 == type2 || type2.IsAssignableFrom(type1);
            }

            #endregion
        }

        #endregion
    }
}