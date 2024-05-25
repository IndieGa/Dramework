using System;


namespace IG.HappyCoder.Dramework3.Runtime.Tools.Helpers
{
    public static partial class Helpers
    {
        #region ================================ NESTED TYPES

        public static class DateTimeTools
        {
            #region ================================ METHODS

            public static long DateTimeToUnix(DateTime dateTime)
            {
                return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
            }

            public static DateTime UnixToDateTime(long unixTime)
            {
                var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                return dateTime.AddSeconds(unixTime).ToLocalTime();
            }

            #endregion
        }

        #endregion
    }
}