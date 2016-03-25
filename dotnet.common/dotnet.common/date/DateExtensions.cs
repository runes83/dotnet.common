using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnet.common.date
{
    public static class DateExtensions
    {

        public static DateTime RemoveMillisecond(this DateTime dt)
        {
            return dt.AddMilliseconds(-dt.Millisecond);
        }

        public static DateTime RemoveMillisecondAndTicks(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

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
