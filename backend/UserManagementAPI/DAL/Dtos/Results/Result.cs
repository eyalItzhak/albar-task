namespace UserManagementAPI.DAL.Dtos.Results
{
    public class Result //without Type , only for success or failure
    {
        public bool IsSuccess { get; set; }
        public List<string> Errors { get; set; } = new();
        public int? StatusCode { get; set; }

        public static Result Success() => new Result { IsSuccess = true };

        public static Result Failure(List<string> errors, int? statusCode = null) =>
            new Result { IsSuccess = false, Errors = errors, StatusCode = statusCode };

        public static Result Failure(string error, int? statusCode = null) =>
            new Result { IsSuccess = false, Errors = new List<string> { error }, StatusCode = statusCode };
    }

    public class Result<T> : Result //with Type
    {
        public T? Value { get; set; }

        public static Result<T> Success(T value) =>
            new Result<T> { IsSuccess = true, Value = value };

        public new static Result<T> Failure(List<string> errors, int? statusCode = null) =>
            new Result<T> { IsSuccess = false, Errors = errors, StatusCode = statusCode };

        public new static Result<T> Failure(string error, int? statusCode = null) =>
            new Result<T> { IsSuccess = false, Errors = new List<string> { error }, StatusCode = statusCode };
    }
}
