namespace RosePipe;

/// <summary>
/// Represents a step in the pipeline that processes a data bag of type <typeparamref name="T"/>.
/// This step can handle custom exceptions of type <typeparamref name="T2"/>.
/// </summary>
/// <typeparam name="T">The type of the data bag that the step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
/// <typeparam name="T2">The type of the custom exception the step can handle, inheriting from <see cref="StepError"/>.</typeparam>
public interface IStep<T, T2> where T : BagBase
                              where T2 : StepError, new()
{
    /// <summary>
    /// Gets or sets the error encountered during the execution of the step, if any.
    /// </summary>
    T2? Error { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the pipeline should continue processing subsequent steps.
    /// </summary>
    bool IsContinueProcess { get; set; }

    /// <summary>
    /// Executes the step asynchronously using the provided data bag.
    /// </summary>
    /// <param name="input">The data bag to be processed by the step.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modified data bag.</returns>
    Task<T> ExecuteAsync(T input);

    /// <summary>
    /// Adds the pipeline to the step, allowing the step to access or modify pipeline state.
    /// </summary>
    /// <param name="pipeline">The pipeline that this step belongs to.</param>
    void AddPipeline(Pipeline<T, T2> pipeline);

    /// <summary>
    /// Throws or handles a given error of type <typeparamref name="T2"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="error">The error of type <typeparamref name="T2"/> that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="T"/> after processing the error.</returns>
    T ThrowStepError(T2 error);
}


/// <summary>
/// Represents a step in the pipeline that processes a data bag of type <typeparamref name="T"/>.
/// This step uses a standard exception type for error handling.
/// </summary>
/// <typeparam name="T">The type of the data bag that the step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
public interface IStep<T> where T : BagBase
{
    /// <summary>
    /// Gets or sets the error encountered during the execution of the step, if any.
    /// </summary>
    StepError? Error { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the pipeline should continue processing subsequent steps.
    /// </summary>
    bool IsContinueProcess { get; set; }

    /// <summary>
    /// Executes the step asynchronously using the provided data bag.
    /// </summary>
    /// <param name="input">The data bag to be processed by the step.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modified data bag.</returns>
    Task<T> ExecuteAsync(T input);

    /// <summary>
    /// Adds the pipeline to the step, allowing the step to access or modify pipeline state.
    /// </summary>
    /// <param name="pipeline">The pipeline that this step belongs to.</param>
    void AddPipeline(Pipeline<T> pipeline);

    /// <summary>
    /// Throws or handles a given <see cref="StepError"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="error">The <see cref="StepError"/> instance that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="T"/> after processing the error.</returns>
    T ThrowStepError(StepError error);
}
