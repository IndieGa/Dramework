using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Extensions
{
    public static class DateTimeExtensions
    {
        #region ================================ METHODS

        public static bool Equal(this DateTime first, DateTime second)
        {
            return first == second;
        }

        public static bool Greater(this DateTime first, DateTime second)
        {
            return first > second;
        }

        public static bool GreaterOrEqual(this DateTime first, DateTime second)
        {
            return first >= second;
        }

        public static bool Less(this DateTime first, DateTime second)
        {
            return first < second;
        }

        public static bool LessOrEqual(this DateTime first, DateTime second)
        {
            return first <= second;
        }

        public static DateTime ToDateTime(this long unixTime)
        {
            return Helpers.Helpers.DateTimeTools.UnixToDateTime(unixTime);
        }

        public static long ToUnixTime(this DateTime dateTime)
        {
            return Helpers.Helpers.DateTimeTools.DateTimeToUnix(dateTime);
        }

        #endregion
    }
}