using System;

namespace DatingCode.Extensions
{
    public static class DateTimeExtensions
    {
        public static int YearsToDate(this DateTime from, DateTime to)
        {
            var zeroTime = new DateTime(1, 1, 1);
            var span = to - from;
            var years = (zeroTime + span).Year - 1;
            return years;
        }

        public static int YearsToUtcNow(this DateTime from)
        {
            return from.YearsToDate(DateTime.UtcNow);
        }
    }
}
