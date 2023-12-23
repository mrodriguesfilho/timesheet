using Microsoft.Extensions.Logging.Abstractions;

namespace TimeSheet.Database.AdoNet.Utils;

public static class QueryUtils
{
    public static string ToStringSingleQuoted(this object input)
    {
        return input is null ? "null" : $"'{input}'";
    }

    public static string ToDateTimeStyle(this DateTime datetime)
    {
        return datetime == default ? "null" : $"'{datetime:yyyy-MM-dd HH:mm:ss}'";
    }
    
    public static string ToDateTimeStyle(this DateTime? datetime)
    {
        return datetime is null ? "null" : $"'{datetime.GetValueOrDefault():yyyy-MM-dd HH:mm:ss}'";
    }
}