namespace RosePipe;

public interface IStep<T> where T : BagBase
{
    Exception? Error { get; set; }
    bool IsContinueProcess { get; set; }
    Task<T> ExecuteAsync(T input);
    void AddPipeline(Pipeline<T> pipeline);
}