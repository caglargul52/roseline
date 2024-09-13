namespace RoseLine;

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
    TError? GetError();

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

    bool IsContinueProcess();
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
    StepError? GetError();

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

    bool IsContinueProcess();
}
