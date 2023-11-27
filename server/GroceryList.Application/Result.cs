namespace GroceryList.Application;

public class Result<T>
{
    public bool IsSuccessful { get; }
    public ResultStatusCode StatusCode { get; }
    public List<string> ErrorMessages { get; }
    public T Data { get; }

    private Result(bool isSuccessful, ResultStatusCode statusCode, List<string> errorMessages, T data)
    {
        IsSuccessful = isSuccessful;
        StatusCode = statusCode;
        ErrorMessages = errorMessages;
        Data = data;
    }

    public static Result<T> Success(T data)
    {
        return new Result<T>(true, ResultStatusCode.None, default, data);
    }

    public static Result<T> Failure(ResultStatusCode statusCode, string errorMessage)
    {
        return new Result<T>(false, statusCode, new List<string>() { errorMessage }, default);
    }

    public static Result<T> Failure(ResultStatusCode statusCode, List<string> errorMessages)
    {
        return new Result<T>(false, statusCode, errorMessages, default);
    }
}
