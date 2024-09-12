namespace RoseLine.Test
{
    public class CustomError : StepError
    {
        public string Code { get; set; }
        public int Severity { get; set; }
    }
}
