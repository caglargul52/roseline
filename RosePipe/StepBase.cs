using System.Net;
using System.Text.Json;

namespace RosePipe;

public abstract class StepBase<T, T2> : IStep<T, T2> 
    where T : BagBase
    where T2 : StepError, new()
{
    public Pipeline<T, T2>? Pipeline { get; private set; }
    public T2? Error { get; set; }
    public bool IsContinueProcess { get; set; }

    private T CurrentDto;

    protected abstract Task<T> ProcessAsync(T input);

    public T ThrowStepError(T2 error)
    {            
        var index = this.Pipeline.CurrentStepIndex;
            
        var stepName = this.Pipeline.Steps[index].GetType().Name;

        error.SetStep(stepName);

        Error = error;

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
            var index = this.Pipeline.CurrentStepIndex;
            
            var stepName = this.Pipeline.Steps[index].GetType().Name;

            var error = new T2()
            {
                Message = ex.ToString(),
            };

            error.SetUnhandledError();
            error.SetStep(stepName);

            var json = JsonSerializer.Serialize(error);

            this.Error = JsonSerializer.Deserialize<T2?>(json);
        }

        return CurrentDto;
    }

    void IStep<T, T2>.AddPipeline(Pipeline<T, T2> pipeline)
    {
        Pipeline = pipeline;
    }
}

public abstract class StepBase<T> : IStep<T> where T : BagBase
{
    public Pipeline<T>? Pipeline { get; private set; }
    public StepError? Error { get; set; }
    public bool IsContinueProcess { get; set; }

    private T CurrentDto;

    protected abstract Task<T> ProcessAsync(T input);

    public T ThrowStepError(StepError error)
    {
        var index = this.Pipeline.CurrentStepIndex;

        var stepName = this.Pipeline.Steps[index].GetType().Name;

        error.SetStep(stepName);

        Error = error;

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
            var index = this.Pipeline.CurrentStepIndex;

            var stepName = this.Pipeline.Steps[index].GetType().Name;

            var error = new StepError()
            {
                Message = ex.ToString(),
            };

            error.SetUnhandledError();
            error.SetStep(stepName);

            this.Error = error;
        }

        return CurrentDto;
    }

    void IStep<T>.AddPipeline(Pipeline<T> pipeline)
    {
        Pipeline = pipeline;
    }
}