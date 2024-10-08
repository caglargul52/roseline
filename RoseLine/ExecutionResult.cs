﻿namespace RoseLine;

/// <summary>
/// Represents the result of executing a pipeline or step.
/// Contains information about the success or failure of the execution, the data bag processed, and any errors encountered.
/// </summary>
/// <typeparam name="TBag">The type of the data bag that was processed, inheriting from <see cref="BagBase"/>.</typeparam>
/// <typeparam name="TError">The type of the exception that can be thrown during execution, inheriting from <see cref="StepError"/>.</typeparam>
/// <param name="isSuccess">Indicates whether the execution was successful.</param>
/// <param name="data">The data bag that was processed during the execution.</param>
/// <param name="error">The error encountered during execution, if any.</param>
public sealed class ExecutionResult<TBag, TError>(bool isSuccess, TBag data, TError? error) where TError : StepError
{
    /// <summary>
    /// Gets or sets a value indicating whether the execution was successful.
    /// </summary>
    public bool IsSuccess { get; set; } = isSuccess;

    /// <summary>
    /// Gets or sets the data bag that was processed during the execution.
    /// </summary>
    public TBag Bag { get; set; } = data;

    /// <summary>
    /// Gets or sets the error encountered during the execution, if any.
    /// </summary>
    public TError? Error { get; set; } = error;
}
