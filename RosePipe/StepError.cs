namespace RosePipe;

public class StepError
{
    public bool IsUnhandledError { get; private set; }

    public string Step { get; private set; }

    public string Message { get; set; }

    internal void SetUnhandledError()
    {
        IsUnhandledError = true;
    }

    internal void SetStep(string step)
    {
        Step = step;
    }
}
