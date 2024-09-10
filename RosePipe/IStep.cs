namespace RosePipe;

/// <summary>
/// Represents a step in the pipeline that processes a data bag of type <typeparamref name="TBag"/>.
/// This step can handle custom exceptions of type <typeparamref name="TError"/>.
/// </summary>
/// <typeparam name="TBag">The type of the data bag that the step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
/// <typeparam name="TError">The type of the custom exception the step can handle, inheriting from <see cref="StepError"/>.</typeparam>
public interface IStep<TBag, TError> where TBag : BagBase
                              where TError : StepError, new()
{
    /// <summary>
    /// Gets or sets the error encountered during the execution of the step, if any.
    /// </summary>
    TError? Error { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the pipeline should continue processing subsequent steps.
    /// </summary>
    bool IsContinueProcess { get; set; }

    /// <summary>
    /// Executes the step asynchronously using the provided data bag.
    /// </summary>
    /// <param name="input">The data bag to be processed by the step.</param>
    /// <returns>A task that represents the asynchronous operation, containing the modified data bag.</returns>
    Task<TBag> ExecuteAsync(TBag input);

    /// <summary>
    /// Adds the pipeline to the step, allowing the step to access or modify pipeline state.
    /// </summary>
    /// <param name="pipeline">The pipeline that this step belongs to.</param>
    void AddPipeline(Pipeline<TBag, TError> pipeline);

    /// <summary>
    /// Throws or handles a given error of type <typeparamref name="TError"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="TBag"/>.
    /// </summary>
    /// <param name="error">The error of type <typeparamref name="TError"/> that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="TBag"/> after processing the error.</returns>
    TBag ThrowStepError(TError error);
}


/// <summary>
/// Represents a step in the pipeline that processes a data bag of type <typeparamref name="TBag"/>.
/// This step uses a standard exception type for error handling.
/// </summary>
/// <typeparam name="TBag">The type of the data bag that the step operates on, inheriting from <see cref="BagBase"/>.</typeparam>
public interface IStep<TBag> where TBag : BagBase
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
    Task<TBag> ExecuteAsync(TBag input);

    /// <summary>
    /// Adds the pipeline to the step, allowing the step to access or modify pipeline state.
    /// </summary>
    /// <param name="pipeline">The pipeline that this step belongs to.</param>
    void AddPipeline(Pipeline<TBag> pipeline);

    /// <summary>
    /// Throws or handles a given <see cref="StepError"/> during the execution of a step.
    /// This method allows the pipeline or step to process the provided error object and 
    /// return a corresponding result of type <typeparamref name="TBag"/>.
    /// </summary>
    /// <param name="error">The <see cref="StepError"/> instance that occurred during the step execution.</param>
    /// <returns>A result of type <typeparamref name="TBag"/> after processing the error.</returns>
    TBag ThrowStepError(StepError error);
}
