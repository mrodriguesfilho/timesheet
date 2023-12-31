namespace TimeSheet.Database.AdoNet.Adapters.Interface;

public class DatabaseResult
{
    public bool Success { get; }
    public string Error { get; private set; }
    
    public Exception? Exception { get; private set; }
    
    protected DatabaseResult(bool success, string error, Exception? exception = null)
    {
        Success = success;
        Error = error;
        Exception = exception;
    }

    public static DatabaseResult Fail(string message, Exception? exception = null)
    {
        return new DatabaseResult(false, message, exception);
    }

    public static DatabaseResult<T> Fail<T>(T? value, string message, Exception? exception)
    {
        return new DatabaseResult<T>(value, false, message, exception);
    }
    
    public static DatabaseResult Ok()
    {
        return new DatabaseResult(true, string.Empty);
    }

    public static DatabaseResult<T> Ok<T>(T value)
    {
        return new DatabaseResult<T>(value, true, string.Empty);
    }
}

public class DatabaseResult<T> : DatabaseResult
{
    public T? Value { get; }

    protected internal DatabaseResult(T? value, bool success, string error, Exception? exception = null) : base(success, error, exception)
    {
        Value = value;
    }
}