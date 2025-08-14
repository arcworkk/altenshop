namespace Api.Common;

public record ApiResult<T>(bool Success, T? Data, string? Error = null)
{
    public static ApiResult<T> Ok(T data) => new(true, data);
    public static ApiResult<T> Fail(string error) => new(false, default, error);
}