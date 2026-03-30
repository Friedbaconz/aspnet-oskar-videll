

namespace Application.Common.Results;


public enum ErrorTypes
{
    BadRequest = 400,
    NotFound = 404,
    Conflict = 409,
    Error = 500
}
public record Result(bool Success, ErrorTypes? ErrorType = null, string? ErrorMessage = null)
{
    public static Result Ok() => new(true);

    public static Result BadRequest(string message) => new(false, ErrorTypes.BadRequest, message);
    public static Result NotFound(string message) => new(false, ErrorTypes.NotFound, message);
    public static Result Conflict(string message) => new(false, ErrorTypes.Conflict, message);
    public static Result Error(string message = "An unexpected error occurred") => new(false, ErrorTypes.Error, message);
}

public record Result<T>(bool Success, T? value = default, ErrorTypes? ErrorType = null, string? ErrorMessage = null)
{
    public static Result<T> Ok(T value) => new(true, value);

    public static Result<T> BadRequest(string message) => new(false, default,ErrorTypes.BadRequest, message);
    public static Result<T> NotFound(string message) => new(false, default,ErrorTypes.NotFound, message);
    public static Result<T> Conflict(string message, T? value = default) => new(false, value, ErrorTypes.Conflict, message);
    public static Result<T> Error(string message = "An unexpected error occurred") => new(false, default, ErrorTypes.Error, message);
}
