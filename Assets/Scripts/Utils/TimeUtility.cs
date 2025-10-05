using System;

namespace Converters
{
    public static class TimeUtility
    {
        /// <summary>
        /// Converts a Unix timestamp (in seconds) to a DateTime (UTC).
        /// </summary>
        public static DateTime ToDateTime(int timestamp)
        {
            return DateTime.UnixEpoch.AddSeconds(timestamp);
        }

        public static long GetCurrentUTCTimestamp()
        {
            return (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
        }
    }
}