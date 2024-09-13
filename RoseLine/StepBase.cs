using System.Net;
using System.Text.Json;

namespace RoseLine;

public abstract class StepBase<TBag, TError> : IStep<TBag, TError> 
    where TBag : BagBase
    where TError : StepError, new()
{
    private Pipeline<TBag, TError>? Pipeline { get; set; }
    private TError? Error { get; set; }
    private bool IsContinueProcess { get; set; }

    private TBag CurrentDto;

    protected abstract Task<TBag> ProcessAsync(TBag input);

    /// <summary>
    /// Throws or handles a given error of type <typeparamref name="TError"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="TBag"/>.
    /// </summary>
    /// <param name="error">The error of type <typeparamref name="TError"/> that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="TBag"/> after processing the error.</returns>
    protected TBag ThrowStepError(TError error)
    {            
        var index = this.Pipeline.CurrentStepIndex;
            
        var stepName = this.Pipeline.Steps[index].GetType().Name;

        error.SetStep(stepName);

        Error = error;

        return CurrentDto;
    }

    protected TBag Next(TBag output)
    {
        IsContinueProcess = true;
        return output;
    }

    protected TBag Stop(TBag output)
    {
        IsContinueProcess = false;
        return output;
    }

    async Task<TBag> IStep<TBag, TError>.ExecuteAsync(TBag input)
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

            var error = new TError()
            {
                Message = ex.ToString(),
            };

            error.SetUnhandledError();
            error.SetStep(stepName);

            var json = JsonSerializer.Serialize(error);

            this.Error = JsonSerializer.Deserialize<TError?>(json);
        }

        return CurrentDto;
    }

    void IStep<TBag, TError>.AddPipeline(Pipeline<TBag, TError> pipeline)
    {
        Pipeline = pipeline;
    }

    bool IStep<TBag, TError>.IsContinueProcess()
    {
        return IsContinueProcess;
    }

    TError? IStep<TBag, TError>.GetError()
    {
        return Error;
    }
}

public abstract class StepBase<TBag> : IStep<TBag> where TBag : BagBase
{
    private Pipeline<TBag>? Pipeline { get; set; }
    private StepError? Error { get; set; }
    private bool IsContinueProcess { get; set; }

    private TBag CurrentDto;

    protected abstract Task<TBag> ProcessAsync(TBag input);

    /// <summary>
    /// Throws or handles a given <see cref="StepError"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="TBag"/>.
    /// </summary>
    /// <param name="error">The <see cref="StepError"/> instance that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="TBag"/> after processing the error.</returns>
    protected TBag ThrowStepError(StepError error)
    {
        var index = this.Pipeline.CurrentStepIndex;

        var stepName = this.Pipeline.Steps[index].GetType().Name;

        error.SetStep(stepName);

        Error = error;

        return CurrentDto;
    }

    protected TBag Next(TBag output)
    {
        IsContinueProcess = true;
        return output;
    }

    protected TBag Stop(TBag output)
    {
        IsContinueProcess = false;
        return output;
    }

    async Task<TBag> IStep<TBag>.ExecuteAsync(TBag input)
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

    void IStep<TBag>.AddPipeline(Pipeline<TBag> pipeline)
    {
        Pipeline = pipeline;
    }

    bool IStep<TBag>.IsContinueProcess()
    {
        return IsContinueProcess;
    }

    StepError? IStep<TBag>.GetError()
    {
        return Error;
    }
}