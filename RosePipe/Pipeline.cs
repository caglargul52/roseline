namespace RosePipe;

/// <summary>
/// Represents a pipeline that processes a series of steps.
/// Each step operates on a data bag of type <typeparamref name="TBag"/> and can handle errors of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="TBag">The type of the data bag that each step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
/// <typeparam name="TError">The type of the custom exception for step errors, inheriting from <see cref="StepError"/>.</typeparam>
public sealed class Pipeline<TBag, TError> where TBag : BagBase
    where TError : StepError, new()
{
    /// <summary>
    /// Gets the list of steps in the pipeline.
    /// </summary>
    public List<IStep<TBag, TError>> Steps { get; } = new();

    /// <summary>
    /// Gets or sets the current step being executed in the pipeline.
    /// </summary>
    public IStep<TBag, TError>? CurrentStep { get; private set; }

    /// <summary>
    /// Gets or sets the index of the current step being executed.
    /// </summary>
    public int CurrentStepIndex { get; private set; } = 0;

    private TBag _currentDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline{TBag, TError}"/> class with the provided data bag.
    /// </summary>
    /// <param name="pipeDto">The data bag to be processed through the pipeline.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pipeDto"/> is null.</exception>
    public Pipeline(TBag? pipeDto)
    {
        if (pipeDto is null)
        {
            throw new ArgumentNullException(nameof(pipeDto));
        }
        else
        {
            _currentDto = pipeDto;
        }
    }

    /// <summary>
    /// Adds a step to the pipeline.
    /// </summary>
    /// <param name="step">The step to add to the pipeline.</param>
    /// <returns>The current pipeline instance for method chaining.</returns>
    public Pipeline<TBag, TError> AddStep(IStep<TBag, TError> step)
    {
        step.AddPipeline(this);

        Steps.Add(step);

        return this;
    }

    /// <summary>
    /// Adds a range of steps to the pipeline.
    /// </summary>
    /// <param name="steps">The steps to add to the pipeline.</param>
    /// <returns>The current pipeline instance for method chaining.</returns>
    public Pipeline<TBag, TError> AddRangeStep(List<IStep<TBag, TError>> steps)
    {
        foreach (IStep<TBag, TError> step in steps)
        {
            Steps.Add(step);
        }

        return this;
    }

    /// <summary>
    /// Executes all steps in the pipeline asynchronously.
    /// </summary>
    /// <returns>An <see cref="ExecutionResult{TBag, TError}"/> indicating the success or failure of the pipeline execution.</returns>
    public async Task<ExecutionResult<TBag, TError>> ExecuteAsync()
    {
        if (CurrentStep?.GetError() is not null)
        {
            return new ExecutionResult<TBag, TError>(false, _currentDto, CurrentStep?.GetError());
        }

        if (CurrentStep?.IsContinueProcess() == false)
        {
            return new ExecutionResult<TBag, TError>(true, _currentDto, null);
        }

        for (int i = CurrentStepIndex; i < Steps.Count; i++)
        {
            _currentDto = await Steps[i].ExecuteAsync(_currentDto);

            CurrentStep = Steps[i];

            if (Steps[i].GetError() is not null)
            {
                return new ExecutionResult<TBag, TError>(false, _currentDto, Steps[i].GetError());
            }

            if (!Steps[i].IsContinueProcess())
            {
                break;
            }

            CurrentStepIndex++;
        }

        return new ExecutionResult<TBag, TError>(true, _currentDto, null);
    }
}

/// <summary>
/// Represents a pipeline that processes a series of steps without custom exception handling.
/// Each step operates on a data bag of type <typeparamref name="TBag"/>.
/// </summary>
/// <typeparam name="TBag">The type of the data bag that each step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
public sealed class Pipeline<TBag> where TBag : BagBase
{
    /// <summary>
    /// Gets the list of steps in the pipeline.
    /// </summary>
    public List<IStep<TBag>> Steps { get; } = new();

    /// <summary>
    /// Gets or sets the current step being executed in the pipeline.
    /// </summary>
    public IStep<TBag>? CurrentStep { get; private set; }

    /// <summary>
    /// Gets or sets the index of the current step being executed.
    /// </summary>
    public int CurrentStepIndex { get; private set; } = 0;

    private TBag _currentDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline{TBag}"/> class with the provided data bag.
    /// </summary>
    /// <param name="pipeDto">The data bag to be processed through the pipeline.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pipeDto"/> is null.</exception>
    public Pipeline(TBag? pipeDto)
    {
        if (pipeDto is null)
        {
            throw new ArgumentNullException(nameof(pipeDto));
        }
        else
        {
            _currentDto = pipeDto;
        }
    }

    /// <summary>
    /// Adds a step to the pipeline.
    /// </summary>
    /// <param name="step">The step to add to the pipeline.</param>
    /// <returns>The current pipeline instance for method chaining.</returns>
    public Pipeline<TBag> AddStep(IStep<TBag> step)
    {
        step.AddPipeline(this);

        Steps.Add(step);

        return this;
    }

    /// <summary>
    /// Adds a range of steps to the pipeline.
    /// </summary>
    /// <param name="steps">The steps to add to the pipeline.</param>
    /// <returns>The current pipeline instance for method chaining.</returns>
    public Pipeline<TBag> AddRangeStep(List<IStep<TBag>> steps)
    {
        foreach (IStep<TBag> step in steps)
        {
            Steps.Add(step);
        }

        return this;
    }

    /// <summary>
    /// Executes all steps in the pipeline asynchronously.
    /// </summary>
    /// <returns>An <see cref="ExecutionResult{TBag, StepError}"/> indicating the success or failure of the pipeline execution.</returns>
    public async Task<ExecutionResult<TBag, StepError>> ExecuteAsync()
    {
        if (CurrentStep?.GetError() is not null)
        {
            return new ExecutionResult<TBag, StepError>(false, _currentDto, CurrentStep?.GetError());
        }

        if (CurrentStep?.IsContinueProcess() == false)
        {
            return new ExecutionResult<TBag, StepError>(true, _currentDto, null);
        }

        for (int i = CurrentStepIndex; i < Steps.Count; i++)
        {
            _currentDto = await Steps[i].ExecuteAsync(_currentDto);

            CurrentStep = Steps[i];

            if (Steps[i].GetError() is not null)
            {
                return new ExecutionResult<TBag, StepError>(false, _currentDto, Steps[i].GetError());
            }

            if (!Steps[i].IsContinueProcess())
            {
                break;
            }

            CurrentStepIndex++;
        }

        return new ExecutionResult<TBag, StepError>(true, _currentDto, null);
    }
}