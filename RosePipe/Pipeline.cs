namespace RosePipe;

/// <summary>
/// Represents a pipeline that processes a series of steps.
/// Each step operates on a data bag of type <typeparamref name="T"/> and can handle errors of type <typeparamref name="T2"/>.
/// </summary>
/// <typeparam name="T">The type of the data bag that each step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
/// <typeparam name="T2">The type of the custom exception for step errors, inheriting from <see cref="StepError"/>.</typeparam>
public sealed class Pipeline<T, T2> where T : BagBase
    where T2 : StepError, new()
{
    /// <summary>
    /// Gets the list of steps in the pipeline.
    /// </summary>
    public List<IStep<T, T2>> Steps { get; } = new();

    /// <summary>
    /// Gets or sets the current step being executed in the pipeline.
    /// </summary>
    public IStep<T, T2>? CurrentStep { get; private set; }

    /// <summary>
    /// Gets or sets the index of the current step being executed.
    /// </summary>
    public int CurrentStepIndex { get; private set; } = 0;

    private T _currentDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline{T, T2}"/> class with the provided data bag.
    /// </summary>
    /// <param name="pipeDto">The data bag to be processed through the pipeline.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pipeDto"/> is null.</exception>
    public Pipeline(T? pipeDto)
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
    public Pipeline<T, T2> AddStep(IStep<T, T2> step)
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
    public Pipeline<T, T2> AddRangeStep(List<IStep<T, T2>> steps)
    {
        foreach (IStep<T, T2> step in steps)
        {
            Steps.Add(step);
        }

        return this;
    }

    /// <summary>
    /// Executes all steps in the pipeline asynchronously.
    /// </summary>
    /// <returns>An <see cref="ExecutionResult{T, T2}"/> indicating the success or failure of the pipeline execution.</returns>
    public async Task<ExecutionResult<T, T2>> ExecuteAsync()
    {
        if (CurrentStep?.Error is not null)
        {
            return new ExecutionResult<T, T2>(false, _currentDto, CurrentStep?.Error);
        }

        if (CurrentStep?.IsContinueProcess == false)
        {
            return new ExecutionResult<T, T2>(true, _currentDto, null);
        }

        for (int i = CurrentStepIndex; i < Steps.Count; i++)
        {
            _currentDto = await Steps[i].ExecuteAsync(_currentDto);

            CurrentStep = Steps[i];

            if (Steps[i].Error is not null)
            {
                return new ExecutionResult<T, T2>(false, _currentDto, Steps[i].Error);
            }

            if (!Steps[i].IsContinueProcess)
            {
                break;
            }

            CurrentStepIndex++;
        }

        return new ExecutionResult<T, T2>(true, _currentDto, null);
    }
}

/// <summary>
/// Represents a pipeline that processes a series of steps without custom exception handling.
/// Each step operates on a data bag of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of the data bag that each step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
public sealed class Pipeline<T> where T : BagBase
{
    /// <summary>
    /// Gets the list of steps in the pipeline.
    /// </summary>
    public List<IStep<T>> Steps { get; } = new();

    /// <summary>
    /// Gets or sets the current step being executed in the pipeline.
    /// </summary>
    public IStep<T>? CurrentStep { get; private set; }

    /// <summary>
    /// Gets or sets the index of the current step being executed.
    /// </summary>
    public int CurrentStepIndex { get; private set; } = 0;

    private T _currentDto;

    /// <summary>
    /// Initializes a new instance of the <see cref="Pipeline{T}"/> class with the provided data bag.
    /// </summary>
    /// <param name="pipeDto">The data bag to be processed through the pipeline.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pipeDto"/> is null.</exception>
    public Pipeline(T? pipeDto)
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
    public Pipeline<T> AddStep(IStep<T> step)
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
    public Pipeline<T> AddRangeStep(List<IStep<T>> steps)
    {
        foreach (IStep<T> step in steps)
        {
            Steps.Add(step);
        }

        return this;
    }

    /// <summary>
    /// Executes all steps in the pipeline asynchronously.
    /// </summary>
    /// <returns>An <see cref="ExecutionResult{T, StepException}"/> indicating the success or failure of the pipeline execution.</returns>
    public async Task<ExecutionResult<T, StepError>> ExecuteAsync()
    {
        if (CurrentStep?.Error is not null)
        {
            return new ExecutionResult<T, StepError>(false, _currentDto, CurrentStep?.Error);
        }

        if (CurrentStep?.IsContinueProcess == false)
        {
            return new ExecutionResult<T, StepError>(true, _currentDto, null);
        }

        for (int i = CurrentStepIndex; i < Steps.Count; i++)
        {
            _currentDto = await Steps[i].ExecuteAsync(_currentDto);

            CurrentStep = Steps[i];

            if (Steps[i].Error is not null)
            {
                return new ExecutionResult<T, StepError>(false, _currentDto, Steps[i].Error);
            }

            if (!Steps[i].IsContinueProcess)
            {
                break;
            }

            CurrentStepIndex++;
        }

        return new ExecutionResult<T, StepError>(true, _currentDto, null);
    }
}