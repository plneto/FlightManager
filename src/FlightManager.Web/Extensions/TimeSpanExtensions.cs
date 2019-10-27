using System;

namespace FlightManager.Web.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToTotalHoursAndMinutesFormat(this TimeSpan timeSpan)
        {
            var hours = Math.Floor(timeSpan.TotalHours);
            var minutes = timeSpan.Minutes.ToString();

            if (timeSpan.Minutes <= 9)
            {
                minutes = $"0{minutes}";
            }

            return $"{hours}:{minutes}";
        }
    }
}