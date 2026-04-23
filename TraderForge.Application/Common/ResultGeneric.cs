namespace TraderForge.Application.Common;

public class ResultGeneric<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? ErrorMessage { get; set; }
    
<<<<<<< HEAD
    public static ResultGeneric<T> Success(T value) => new ResultGeneric<T> { IsSuccess = true, Value = value };
    public static ResultGeneric<T> Failure(string errorMessage) => new ResultGeneric<T> { IsSuccess = false, ErrorMessage = errorMessage };
}
=======
    public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
    public static Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };
}
>>>>>>> 1f749f1f578782d90b80008a7c1162a3cc9a80ba
