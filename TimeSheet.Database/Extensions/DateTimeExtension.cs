namespace TimeSheet.Database.Extensions;

public static class DateTimeExtension
{
    public static string ToPT0H0M0S(this DateTime dateTime)
    {
        TimeSpan timeSpan = dateTime.TimeOfDay;
        return $"PT{timeSpan.Hours}H{timeSpan.Minutes}M{timeSpan.Seconds}S";
    }
    
    public static DateTime? FromPT0H0M0S(this string ptFormat)
    {
        if (string.IsNullOrEmpty(ptFormat)) return null;
        
        var timeSpan = ParsePTFormat(ptFormat);
        return DateTime.MinValue.Add(timeSpan);
    }
    
    private static TimeSpan ParsePTFormat(this string duration)
    {
        if (string.IsNullOrWhiteSpace(duration) || !duration.StartsWith("PT"))
        {
            throw new FormatException("Invalid duration format");
        }

        duration = duration.Substring(2); // Remove "PT"
        int hoursIndex = duration.IndexOf('H');
        int minutesIndex = duration.IndexOf('M');
        int secondsIndex = duration.IndexOf('S');

        int hours = hoursIndex > 0 ? int.Parse(duration.Substring(0, hoursIndex)) : 0;
        int minutes = minutesIndex > 0 ? int.Parse(duration.Substring(hoursIndex + 1, minutesIndex - hoursIndex - 1)) : 0;
        int seconds = secondsIndex > 0 ? int.Parse(duration.Substring(minutesIndex + 1, secondsIndex - minutesIndex - 1)) : 0;

        return new TimeSpan(hours, minutes, seconds);
    }
}