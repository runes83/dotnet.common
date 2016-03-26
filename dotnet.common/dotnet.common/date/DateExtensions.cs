using System;

namespace dotnet.common.date
{
    public static class DateExtensions
    {
        /// <summary>
        /// Removea milliseconds from a DateTime object
        /// </summary>
        /// <param name="dt">DateTime to remove milliseconds from</param>
        /// <returns>DateTime without milliseconds</returns>
        public static DateTime RemoveMillisecond(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }

        /// <summary>
        /// Removea milliseconds and ticks from a DateTime object
        /// </summary>
        /// <param name="dt">DateTime to remove milliseconds and ticks  from</param>
        /// <returns>DateTime without milliseconds and ticks</returns>
        public static DateTime RemoveMillisecondAndTicks(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        /// <summary>
        /// Converts a datetime to Norwegian time. Use case UTC time to Norwegian time
        /// </summary>
        /// <param name="timestamp">DateTime to convert</param>
        /// <returns>The datetime to format into Norwegian time</returns>
        public static DateTime? ToNorwegianTime(this DateTime? timestamp)
        {
            if (timestamp == null || timestamp.GetValueOrDefault(DateTime.MinValue).Equals(DateTime.MinValue))
            {
                return null;
            }
            //Set the time zone information to W. Europe Standard Time 
            //Get date and time in W. Europe Standard Time
            try
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(timestamp.Value, tzi); // convert from utc to local
            }
            catch (Exception)
            {
                return timestamp.Value;
            }
        }

        /// <summary>
        /// Converts a datetime to Norwegian time. Use case UTC time to Norwegian time
        /// </summary>
        /// <param name="timestamp">DateTime to convert</param>
        /// <returns>The datetime to format into Norwegian time</returns>
        public static DateTime ToNorwegianTime(this DateTime timestamp)
        {
            try
            {
                //Set the time zone information to W. Europe Standard Time 
                //Get date and time in W. Europe Standard Time
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(timestamp, tzi); // convert from utc to local
            }
            catch (Exception)
            {
                return timestamp;
            }
        }


    }
}
