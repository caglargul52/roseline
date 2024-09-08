namespace RosePipe;

public abstract class StepBase<T> : IStep<T> where T : BagBase
{
    public Pipeline<T>? Pipeline { get; private set; }
    public Exception? Error { get; set; }
    public bool IsContinueProcess { get; set; }

    private T CurrentDto;

    protected abstract Task<T> ProcessAsync(T input);

    public T StepError(Exception error)
    {
        var pipelineException = new PipelineException(error.Message, error.StackTrace);
            
        var index = this.Pipeline.CurrentStepIndex;
            
        var stepName = this.Pipeline.Steps[index].GetType().Name;
            
        pipelineException.ErrorStep = stepName;
        
        Error = pipelineException;

        return CurrentDto;
    }

    protected T Next(T output)
    {
        IsContinueProcess = true;
        return output;
    }

    protected T Stop(T output)
    {
        IsContinueProcess = false;
        return output;
    }

    public async Task<T> ExecuteAsync(T input)
    {
        CurrentDto = input;

        try
        {
            CurrentDto = await ProcessAsync(input);
        }
        catch (Exception ex)
        {
            var pipelineException = new PipelineException(ex.Message, ex.StackTrace);
            
            var index = this.Pipeline.CurrentStepIndex;
            
            var stepName = this.Pipeline.Steps[index].GetType().Name;
            
            pipelineException.ErrorStep = stepName;
            
            this.Error = pipelineException;
        }

        return CurrentDto;
    }

    public void AddPipeline(Pipeline<T> pipeline)
    {
        Pipeline = pipeline;
    }
}