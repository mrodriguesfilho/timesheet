using Microsoft.Extensions.Logging.Abstractions;

namespace TimeSheet.Database.AdoNet.Utils;

public static class QueryUtils
{
    public static string ToStringSingleQuoted(this object input)
    {
        if (input is null) return "null";
        
        return $"'{input}'";
    }
}