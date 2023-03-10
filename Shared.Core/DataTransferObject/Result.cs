namespace Shared.Core.DataTransferObject;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !this.IsSuccess;
    public int StatusCode { get; }
    public string[] Messages { get; protected set; }

    protected Result(bool isSuccess, int statusCode)
    {
        this.IsSuccess = isSuccess;
        this.StatusCode = statusCode;
    }

    public static Result Success(int statusCode) => new(true, statusCode);

    public static Result Failure(int statusCode) => new(false, statusCode);

#nullable enable
    public static Result<TResult> Create<TResult>(TResult value, int successStatusCode, int failureStatusCode, params string[]? errorMessages) =>
        value is not null ?
        Result<TResult>.Success(value, successStatusCode) :
        Result<TResult>.Failure(failureStatusCode, errorMessages);
#nullable restore
}

public class Result<TResult> : Result
{
    public TResult Value { get; protected set; }

    protected Result(bool isSuccess, TResult result, int statusCode) : base(isSuccess, statusCode)
    {
        this.Value = result;
    }
#nullable enable
    protected Result(bool isSuccess, int statusCode, params string[]? message) : base(isSuccess, statusCode)
    {
        this.Messages = message;
    }
#nullable restore

    public static Result<TResult> Success(TResult result, int statusCode) => new(true, result, statusCode);
#nullable enable
    public static Result<TResult> Failure(int statusCode, params string[]? messages) => new(false, statusCode, messages);
}
