namespace RosePipe;

public sealed class ExecutionResult<T>(bool isSuccess, T data, Exception? error)
{
    public bool IsSuccess { get; set; } = isSuccess;
    public T Data { get; set; } = data;
    public Exception? Error { get; set; } = error;
}