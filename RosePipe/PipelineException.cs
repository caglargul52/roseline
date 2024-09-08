namespace RosePipe;

public class PipelineException : Exception
{
    public string ErrorStep { get; set; }
    
    public PipelineException(string message) : base(message)
    {
    }
    
    public PipelineException(string message, string stackTrace) : base(message)
    {
        this.StackTrace = stackTrace;
    }

    public override string StackTrace { get; }
}